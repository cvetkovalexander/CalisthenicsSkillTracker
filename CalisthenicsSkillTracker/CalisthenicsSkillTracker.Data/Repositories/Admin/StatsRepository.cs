using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.Data.Repositories.Contracts.Admin;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CalisthenicsSkillTracker.Data.Repositories.Admin;

public class StatsRepository : IStatsRepository
{
    private readonly ApplicationDbContext _context;

    public StatsRepository(ApplicationDbContext context)
    {
        this._context = context;
    }

    public async Task<IEnumerable<SkillProgress>> GetAllSkillRecordsAsync(Expression<Func<SkillProgress, bool>>? filterQuery = null, Expression<Func<SkillProgress, SkillProgress>>? projectionQuery = null)
    {
        IQueryable<SkillProgress> query = this._context
            .SkillProgressRecords
            .Include(sp => sp.PerformedBy)
            .Include(sp => sp.Skill)
            .OrderByDescending(sp => sp.Date)
            .AsNoTracking();

        if (filterQuery is not null)
            query = query.Where(filterQuery);

        if (projectionQuery is not null)
            query = query.Select(projectionQuery);

        return await query.ToArrayAsync();
    }

    public async Task<IEnumerable<Workout>> GetAllWorkoutsAsync(Expression<Func<Workout, bool>>? filterQuery = null, Expression<Func<Workout, Workout>>? projectionQuery = null)
    {
        IQueryable<Workout> query = this._context
            .Workouts
            .Include(w => w.WorkoutExercises)
            .ThenInclude(we => we.Exercise)
            .Include(w => w.User)
            .OrderByDescending(w => w.Date)
            .AsNoTracking();

        if (filterQuery is not null)
            query = query.Where(filterQuery);

        if (projectionQuery is not null)
            query = query.Select(projectionQuery);

        return await query.ToArrayAsync();
    }

    public async Task<int> CountAsync<T>() where T : class
        => await this._context.Set<T>().CountAsync();
}
