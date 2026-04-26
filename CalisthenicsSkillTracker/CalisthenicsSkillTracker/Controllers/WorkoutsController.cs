using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.GCommon.Exceptions;
using CalisthenicsSkillTracker.Services.Core.Interfaces;
using CalisthenicsSkillTracker.ViewModels.WorkoutViewModels;
using static CalisthenicsSkillTracker.GCommon.OutputMessages.EntityMessages;
using static CalisthenicsSkillTracker.GCommon.ApplicationConstants;

using Microsoft.AspNetCore.Mvc;
using CalisthenicsSkillTracker.Data.Models.Enums;

namespace CalisthenicsSkillTracker.Controllers;

public class WorkoutsController : ControllerBase
{
    private readonly IWorkoutService _workoutService;

    public WorkoutsController(IWorkoutService workoutService)
    {
        this._workoutService = workoutService;
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

        if (!await this._workoutService.EntityExistsAsync<ApplicationUser>(u => u.Id.ToString() == model.UserId))
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
            this.HandleException(ecpe, string.Format(EntitySaveError, nameof(Workout)));

            return this.View(model);
        }
        catch (Exception ex) 
        {
            this.HandleUnexpectedException(ex);

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

        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Please select exercise!");

            return this.View(model);
        }
                
        try 
        {
            await this._workoutService.CreateWorkoutExerciseAsync(model);
        }
        catch (EntityCreatePersistException ecpe)
        {
            this.HandleException(ecpe, string.Format(EntitySaveError, nameof(WorkoutExercise)));

            return this.View(model);
        }
        catch (Exception ex)
        {
            this.HandleUnexpectedException(ex);

            return this.View(model);
        }

        TempData[SuccessTempDataKey] = ExerciseSuccessfullyLogged;

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
        model.Progressions = this._workoutService.FetchProgressions();
        model.ExerciseMeasurementTypes = this._workoutService.FetchWorkoutMeasurementTypes(model.WorkoutId);

        if (!await this._workoutService.WorkoutExerciseExistsAsync(model.WorkoutId, model.WorkoutExerciseId)) 
            ModelState.AddModelError(nameof(model.WorkoutExerciseId), "Invalid workout exercise id!");

        WorkoutExercise workoutExercise = await this._workoutService.GetWorkoutExerciseAsync(model.WorkoutId, model.WorkoutExerciseId);

        if (workoutExercise.Exercise.MeasurementType == Measurement.Repetitions)
        {
            model.Duration = null;

            if (!model.Repetitions.HasValue)
                ModelState.AddModelError(nameof(model.Repetitions), "Repetitions are required for this exercise.");
        }
        else if (workoutExercise.Exercise.MeasurementType == Measurement.Duration)
        {
            model.Repetitions = null;

            if (!model.Duration.HasValue)
                ModelState.AddModelError(nameof(model.Duration), "Duration is required for this exercise.");
        }

        if (!ModelState.IsValid)
            return this.View(model);

        try
        {
            await this._workoutService.CreateWorkoutSetAsync(model);
        }
        catch (EntityCreatePersistException ecpe)
        {
            this.HandleException(ecpe, string.Format(EntitySaveError, nameof(WorkoutSet)));

            return this.View(model);
        }
        catch (Exception ex)
        {
            this.HandleUnexpectedException(ex);

            return this.View(model);
        }

        TempData[SuccessTempDataKey] = SetSuccessfullyLogged;

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
