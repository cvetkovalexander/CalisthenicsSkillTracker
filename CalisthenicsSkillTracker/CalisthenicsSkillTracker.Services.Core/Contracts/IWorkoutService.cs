using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.ViewModels.WorkoutViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;

namespace CalisthenicsSkillTracker.Services.Core.Interfaces;

// TODO: Separate the helper methods in a individual interface.

public interface IWorkoutService
{
    Task<Workout> CreateWorkoutAsync(CreateWorkoutViewModel model, TimeSpan start, TimeSpan end);
    Task CreateWorkoutSetAsync(AddWorkoutSetViewModel model);
    Task CreateWorkoutExerciseAsync(AddWorkoutExerciseViewModel model);
    Task<Workout> GetWorkoutWithExercisesAsync(Guid id);
    Task<WorkoutExercise> GetWorkoutExerciseAsync(Guid workoutId, Guid workoutExerciseId);
    Task<AddWorkoutExerciseViewModel> CreateWorkoutExerciseViewModelAsync(Guid workoutId);
    CreateWorkoutViewModel CreateWorkoutViewModel(string userId);
    AddWorkoutSetViewModel CreateAddWorkoutSetViewModel(Workout workout);
    Task<IEnumerable<WorkoutDetailsViewModel>> CreateWorkoutDetailsViewModelsAsync(string userId);
    Task<Workout> GetWorkoutWithExercisesAndSetsAsync(Guid id, string userId);
    WorkoutExercisesViewModel GetWorkoutExercisesDetailsViewModel(Workout workout);

    /* Helper methods */

    Task<List<SelectListItem>> FetchExercisesAsync();
    List<SelectListItem> FetchWorkoutExercises(Workout workout);
    Task<List<SelectListItem>> GetWorkoutExercisesAsync(Guid id);
    Task<bool> WorkoutExerciseExistsAsync(Guid workoutId, Guid workoutExerciseId);
    List<SelectListItem> FetchProgressions();
    bool IsTimeValid(string input, out TimeSpan output);
    Task<bool> ExerciseAlreadyAddedAsync(Guid workoutId, Guid exerciseId);
    Dictionary<Guid, string> FetchWorkoutMeasurementTypes(Guid workoutId);
    Task<bool> EntityExistsAsync<TEntity>(Expression<Func<TEntity, bool>> predicate)
        where TEntity : class;
}
