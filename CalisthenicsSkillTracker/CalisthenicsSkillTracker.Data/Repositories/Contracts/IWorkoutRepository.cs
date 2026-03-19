using CalisthenicsSkillTracker.Data.Models;
using System.Linq.Expressions;

namespace CalisthenicsSkillTracker.Data.Repositories.Contracts;

public interface IWorkoutRepository
{
    Task<IEnumerable<Workout>> GetAllUserWorkoutsWithProjectionAsync(string userId, Func<Workout, Workout>? projectFunc = null);

    Task<bool> AddWorkoutAsync(Workout workout);

    Task<bool> AddWorkoutSetAsync(WorkoutSet set);

    Task<bool> AddWorkoutExerciseAsync(WorkoutExercise exercise);

    Task<Workout> GetWorkoutWithExercisesAsync(Guid id);

    Task<WorkoutExercise> GetWorkoutExerciseAsync(Guid workoutId, Guid workoutExerciseId);

    Task<Workout> GetWorkoutWithExercisesAndSetsAsync(Guid id, string userId);

    IQueryable<Exercise> GetAllExercisesAsNoTracking();

    Task<bool> WorkoutExerciseExistsAsync(Guid workoutId, Guid workoutExerciseId);

    Task<bool> EntityExistsAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) 
        where TEntity : class;
}
