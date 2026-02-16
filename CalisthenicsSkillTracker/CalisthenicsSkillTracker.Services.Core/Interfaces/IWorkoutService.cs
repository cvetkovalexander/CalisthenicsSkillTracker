using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.ViewModels.WorkoutViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CalisthenicsSkillTracker.Services.Core.Interfaces;

// TODO: Separate the helper methods in a individual interface.

public interface IWorkoutService
{
    Task<Workout> CreateWorkoutAsync(CreateWorkoutViewModel model);
    Task CreateWorkoutSetAsync(AddWorkoutSetViewModel model);
    Task CreateWorkoutExerciseAsync(AddWorkoutExerciseViewModel model);
    Task<Workout> GetWorkoutWithExercisesAsync(Guid id);
    Task<WorkoutExercise> GetWorkoutExerciseAsync(Guid workoutId, Guid workoutExerciseId);
    Task<AddWorkoutExerciseViewModel> CreateWorkoutExerciseViewModelAsync(Guid workoutId);
    Task<CreateWorkoutViewModel> CreateWorkoutViewModelAsync();
    AddWorkoutSetViewModel AddWorkoutSetViewModel(Workout workout);

    /* Helper methods */
    Task<bool> UserExistsAsync(string id);
    Task<bool> ExerciseExistsAsync(Guid id);
    Task<bool> WorkoutExistsAsync(Guid id);
    Task<List<SelectListItem>> FetchUsersAsync();
    Task<List<SelectListItem>> FetchExercisesAsync();
    List<SelectListItem> GetWorkoutExercises(Workout workout);
    Task<List<SelectListItem>> GetWorkoutExercisesAsync(Guid id);
    Task<bool> WorkoutExerciseExistsAsync(Guid workoutId, Guid workoutExerciseId);
    List<SelectListItem> FetchProgressions();
}
