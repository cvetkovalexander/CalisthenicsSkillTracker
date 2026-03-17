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

    public async Task<IEnumerable<Workout>> GetAllUserWorkoutsWithProjectionAsync(string userId, Func<Workout, Workout>? projectFunc = null)
    {
        IQueryable<Workout> fetchQuery = this._context
            .Workouts
            .AsNoTracking()
            .Where(w => w.UserId == userId);

        if (projectFunc is not null) 
        {
            fetchQuery = fetchQuery
                .Select(w => projectFunc(w))
                .AsQueryable();
        }

        return await fetchQuery.ToArrayAsync();
    }

    public async Task<bool> AddWorkoutAsync(Workout workout)
    {
        await this._context.Workouts.AddAsync(workout);
        int resultCount = await this.SaveChangesAsync();

        return resultCount == 1;
    }

    public async Task<bool> AddWorkoutSetAsync(WorkoutSet set)
    {
        await this._context.WorkoutSets.AddAsync(set);
        int resultCount = await this.SaveChangesAsync();

        return resultCount == 1;
    }

    private async Task<int> SaveChangesAsync()
        => await this._context.SaveChangesAsync();
}
