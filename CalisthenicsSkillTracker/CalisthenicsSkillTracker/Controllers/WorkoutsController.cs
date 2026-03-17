using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.GCommon.Exceptions;
using CalisthenicsSkillTracker.Services.Core.Interfaces;
using CalisthenicsSkillTracker.ViewModels.WorkoutViewModels;
using static CalisthenicsSkillTracker.GCommon.OutputMessages;
using static CalisthenicsSkillTracker.GCommon.EntityConstants;

using Microsoft.AspNetCore.Mvc;

namespace CalisthenicsSkillTracker.Controllers;

public class WorkoutsController : ControllerBase
{
    private readonly IWorkoutService _workoutService;
    private readonly ILogger<WorkoutsController> _logger;

    public WorkoutsController(IWorkoutService workoutService, ILogger<WorkoutsController> logger)
    {
        this._workoutService = workoutService;
        this._logger = logger;
    }

    [HttpGet]
    public IActionResult Log()
    {
        string? userId = GetUserId();
        if (userId is null)
            return this.Unauthorized();

        CreateWorkoutViewModel model = this._workoutService.CreateWorkoutViewModel(userId);

        return this.View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Log(CreateWorkoutViewModel model)
    {
        if (string.IsNullOrEmpty(model.UserId))
            ModelState.AddModelError(string.Empty, "User ID is missing.");

        if (!await this._workoutService.EntityExistsAsync<ApplicationUser>(u => u.Id == model.UserId))
            ModelState.AddModelError(string.Empty, "User is not found.");

        if (!this._workoutService.IsTimeValid(model.Start, out TimeSpan start))
            ModelState.AddModelError(nameof(model.Start), "Use time format {HH:mm} for start");

        if (!this._workoutService.IsTimeValid(model.End, out TimeSpan end))
            ModelState.AddModelError(nameof(model.End), "Use time format {HH:mm} for end");

        if (!ModelState.IsValid)
        {
            return this.View(model);
        }

        Workout workout = null!;
        try
        {
            workout = await this._workoutService.CreateWorkoutAsync(model, start, end);
        }
        catch (EntityCreatePersistException ecpe)
        {
            this._logger.LogError(ecpe, string.Format(EntitySaveError, nameof(Workout)));

            ModelState.AddModelError(string.Empty, string.Format(EntitySaveError, nameof(Workout)));

            return this.View(model);
        }
        catch (Exception ex) 
        {
            this._logger.LogError(ex, UnexpectedErrorMessage);

            ModelState.AddModelError(string.Empty, UnexpectedErrorMessage);

            return this.View(model);
        }

        return this.RedirectToAction("AddExercises", new { workoutId = workout.Id });
    }

    [HttpGet]
    public async Task<IActionResult> AddExercises(Guid workoutId)
    {
        if (!await this._workoutService.EntityExistsAsync<Workout>(w => w.Id == workoutId))
            return this.NotFound();

        AddWorkoutExerciseViewModel model = await this._workoutService
            .CreateWorkoutExerciseViewModelAsync(workoutId);

        return this.View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddExercises(AddWorkoutExerciseViewModel model)
    {
        model.AvailableExercises = await this._workoutService.FetchExercisesAsync();
        if (!await this._workoutService.EntityExistsAsync<Workout>(w => w.Id == model.WorkoutId))
        {
            ModelState.AddModelError(string.Empty, "Invalid workout id!");

            return this.View(model);
        }

        Workout workout = await this._workoutService.GetWorkoutWithExercisesAsync(model.WorkoutId);
        model.HasExercises = workout.WorkoutExercises.Any();

        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Please select exercise!");

            return this.View(model);
        }

        if (!await this._workoutService.EntityExistsAsync<Exercise>(e => e.Id == model.ExerciseId)) 
        {
            ModelState.AddModelError(nameof(model.ExerciseId), "Invalid exercise id!");

            return this.View(model);
        }


        if (await this._workoutService.ExerciseAlreadyAddedAsync(model.WorkoutId, model.ExerciseId)) 
        {
            ModelState.AddModelError(nameof(model.ExerciseId), "Exercise already added, please select another one.");

            return this.View(model);
        }
                
        try 
        {
            await this._workoutService.CreateWorkoutExerciseAsync(model);
        }
        catch (EntityCreatePersistException ecpe)
        {
            this._logger.LogError(ecpe, string.Format(EntitySaveError, nameof(WorkoutExercise)));

            ModelState.AddModelError(string.Empty, string.Format(EntitySaveError, nameof(WorkoutExercise)));

            return this.View(model);
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, UnexpectedErrorMessage);

            ModelState.AddModelError(string.Empty, UnexpectedErrorMessage);

            return this.View(model);
        }

        TempData["SuccessMessage"] = "Exercise added successfully! You can add more exercises or proceed to add sets.";

        return this.RedirectToAction("AddExercises", new { workoutId = model.WorkoutId });
    }

    [HttpGet]
    public async Task<IActionResult> AddSets(Guid workoutId)
    {
        if (!await this._workoutService.EntityExistsAsync<Workout>(w => w.Id == workoutId))
            return this.NotFound();

        Workout workout = await this._workoutService.GetWorkoutWithExercisesAsync(workoutId);

        AddWorkoutSetViewModel model = this._workoutService.CreateAddWorkoutSetViewModel(workout);

        return this.View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddSets(AddWorkoutSetViewModel model)
    {
        if (!await this._workoutService.EntityExistsAsync<Workout>(w => w.Id == model.WorkoutId))
            return this.NotFound();

        model.Exercises = await this._workoutService.GetWorkoutExercisesAsync(model.WorkoutId);

        if (!await this._workoutService.WorkoutExerciseExistsAsync(model.WorkoutId, model.WorkoutExerciseId)) 
            ModelState.AddModelError(nameof(model.WorkoutExerciseId), "Invalid workout exercise id!");

        if (!ModelState.IsValid) 
            return this.View(model);


        WorkoutExercise workoutExercise = await this._workoutService.GetWorkoutExerciseAsync(model.WorkoutId, model.WorkoutExerciseId);

        try
        {
            await this._workoutService.CreateWorkoutSetAsync(model);
        }
        catch (EntityCreatePersistException ecpe)
        {
            this._logger.LogError(ecpe, string.Format(EntitySaveError, nameof(WorkoutSet)));

            ModelState.AddModelError(string.Empty, string.Format(EntitySaveError, nameof(WorkoutSet)));

            return this.View(model);
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, UnexpectedErrorMessage);

            ModelState.AddModelError(string.Empty, UnexpectedErrorMessage);

            return this.View(model);
        }

        TempData["SuccessMessage"] = "Exercise set added successfully!";

        return this.RedirectToAction("AddSets", new { workoutId = model.WorkoutId });
    }

    [HttpGet]
    public async Task<IActionResult> MyWorkouts() 
    {
        string? userId = GetUserId();
        if (userId is null)
            return this.Unauthorized();

        IEnumerable<WorkoutDetailsViewModel> models = await this._workoutService.CreateWorkoutDetailsViewModelsAsync(userId);

        return this.View(models);
    }

    [HttpGet]
    public async Task<IActionResult> MyWorkoutDetails(Guid id) 
    {
        string? userId = GetUserId();
        if (userId is null)
            return this.Unauthorized();

        if (!await this._workoutService.EntityExistsAsync<Workout>(w => w.Id == id))
            return this.NotFound();

        Workout workout = await this._workoutService.GetWorkoutWithExercisesAndSetsAsync(id, userId);

        WorkoutExercisesViewModel model = this._workoutService.GetWorkoutExercisesDetailsViewModel(workout);

        return this.View(model);
    }
}
