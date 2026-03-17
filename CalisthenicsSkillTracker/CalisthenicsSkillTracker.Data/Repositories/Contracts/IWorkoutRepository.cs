using CalisthenicsSkillTracker.Data.Models;

namespace CalisthenicsSkillTracker.Data.Repositories.Contracts;

public interface IWorkoutRepository
{
    Task<IEnumerable<Workout>> GetAllUserWorkoutsWithProjectionAsync(string userId, Func<Workout, Workout>? projectFunc = null);

    Task<bool> AddWorkoutAsync(Workout workout);

    Task<bool> AddWorkoutSetAsync(WorkoutSet set);
}
