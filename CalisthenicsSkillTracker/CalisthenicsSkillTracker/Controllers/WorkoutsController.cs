using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.Services.Core.Interfaces;
using CalisthenicsSkillTracker.ViewModels.WorkoutViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Packaging.Signing;
using System.Threading.Tasks;

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
    public async Task<IActionResult> Log(CreateWorkoutViewModel model)
    {
        if (string.IsNullOrEmpty(model.UserId))
            ModelState.AddModelError(string.Empty, "User ID is missing.");

        if (!await this._workoutService.UserExistsAsync(model.UserId))
            ModelState.AddModelError(string.Empty, "User is not found.");

        if (!this._workoutService.isTimeValid(model.Start, out TimeSpan start))
            ModelState.AddModelError(nameof(model.Start), "Use time format {HH:mm} for start");

        if (!this._workoutService.isTimeValid(model.End, out TimeSpan end))
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
        catch (Exception e) 
        {
            this._logger.LogError(e, "Exception occured while trying to save a workout in database");

            ModelState.AddModelError(string.Empty, "An error occurred while creating the workout. Please try again.");

            return this.View(model);
        }

        return this.RedirectToAction("AddExercises", new { workoutId = workout.Id });
    }

    [HttpGet]
    public async Task<IActionResult> AddExercises(Guid workoutId)
    {
        if (!await this._workoutService.WorkoutExistsAsync(workoutId))
            return this.NotFound();

        AddWorkoutExerciseViewModel model = await this._workoutService
            .CreateWorkoutExerciseViewModelAsync(workoutId);

        return this.View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AddExercises(AddWorkoutExerciseViewModel model)
    {
        model.AvailabeExercises = await this._workoutService.FetchExercisesAsync();
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Please select exercise!");

            return this.View(model);
        }

        if (!await this._workoutService.ExerciseExistsAsync(model.ExerciseId)) 
        {
            ModelState.AddModelError(string.Empty, "Invalid exercise id!");

            return this.View(model);
        }

        if (!await this._workoutService.WorkoutExistsAsync(model.WorkoutId))
        {
            ModelState.AddModelError(string.Empty, "Invalid workout id!");

            return this.View(model);
        }

        if (await this._workoutService.ExerciseAlreadyAddedAsync(model.WorkoutId, model.ExerciseId)) 
        {
            ModelState.AddModelError(string.Empty, "Exercise already added, please select another one.");

            return this.View(model);
        }
                
        try 
        {
            await this._workoutService.CreateWorkoutExerciseAsync(model);
        }
        catch (Exception e) 
        {
            this._logger.LogError(e, "Exception occured while trying to save a workout exercise in database");

            ModelState.AddModelError(string.Empty, "An error occurred while adding the exercise to the workout. Please try again.");
            return this.View(model);
        }

        TempData["SuccessMessage"] = "Exercise added successfully! You can add more exercises or proceed to add sets.";

        return this.RedirectToAction("AddExercises", new { workoutId = model.WorkoutId });
    }

    [HttpGet]
    public async Task<IActionResult> AddSets(Guid workoutId)
    {
        if (!await this._workoutService.WorkoutExistsAsync(workoutId))
            return this.NotFound();

        Workout workout = await this._workoutService.GetWorkoutWithExercisesAsync(workoutId);

        AddWorkoutSetViewModel model = this._workoutService.AddWorkoutSetViewModel(workout);

        return this.View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AddSets(AddWorkoutSetViewModel model)
    {
        model.Exercises = await this._workoutService.GetWorkoutExercisesAsync(model.WorkoutId);

        if (!await this._workoutService.WorkoutExerciseExistsAsync(model.WorkoutId, model.WorkoutExerciseId)) 
            ModelState.AddModelError(string.Empty, "Invalid workout exercise id!");

        if (!ModelState.IsValid)
            return this.View(model);

        WorkoutExercise workoutExercise = await this._workoutService.GetWorkoutExerciseAsync(model.WorkoutId, model.WorkoutExerciseId);

        try
        {
            await this._workoutService.CreateWorkoutSetAsync(model);
        }
        catch (Exception e) 
        {
            this._logger.LogError(e, "Exception occured while trying to save a workout set in database");

            ModelState.AddModelError(string.Empty, "An error occurred while adding the set to the workout. Please try again.");

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

        IEnumerable<WorkoutDetailsViewModel> models = await this._workoutService.CreateWorkoutDetailsViewModels(userId);

        return this.View(models);
    }
}
