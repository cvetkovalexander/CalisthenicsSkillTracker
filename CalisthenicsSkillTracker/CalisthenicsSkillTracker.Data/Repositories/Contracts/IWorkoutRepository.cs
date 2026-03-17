using CalisthenicsSkillTracker.Data.Models;

namespace CalisthenicsSkillTracker.Data.Repositories.Contracts;

public interface IWorkoutRepository
{
    IQueryable<Workout> GetAllUserWorkouts(string userId); 
}
