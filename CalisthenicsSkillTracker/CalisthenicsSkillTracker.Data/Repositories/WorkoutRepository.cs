using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.Data.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CalisthenicsSkillTracker.Data.Repositories;

public class WorkoutRepository : IWorkoutRepository
{
    private readonly ApplicationDbContext _context;
    public WorkoutRepository(ApplicationDbContext context)
    {
        this._context = context;
    }

    public IQueryable<Workout> GetAllUserWorkouts(string userId)
    {
        return this._context
            .Workouts
            .AsNoTracking()
            .Where(w => w.UserId == userId);
    }
}
