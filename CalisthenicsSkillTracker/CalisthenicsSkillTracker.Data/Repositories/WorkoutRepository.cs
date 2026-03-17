using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.Data.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

    public async Task<bool> AddWorkoutExerciseAsync(WorkoutExercise exercise)
    {
        await this._context.WorkoutExercises.AddAsync(exercise);
        int resultCount = await this.SaveChangesAsync();

        return resultCount == 1;
    }

    public async Task<bool> AddWorkoutSetAsync(WorkoutSet set)
    {
        await this._context.WorkoutSets.AddAsync(set);
        int resultCount = await this.SaveChangesAsync();

        return resultCount == 1;
    }

    public async Task<Workout> GetWorkoutWithExercisesAsync(Guid id)
    {
        return await this._context
            .Workouts
            .AsNoTracking()
            .Include(w => w.WorkoutExercises)
            .ThenInclude(we => we.Exercise)
            .SingleAsync(w => w.Id == id);
    }

    public async Task<WorkoutExercise> GetWorkoutExerciseAsync(Guid workoutId, Guid workoutExerciseId)
    {
        return await this._context
            .WorkoutExercises
            .FirstAsync(we => we.WorkoutId == workoutId && we.Id == workoutExerciseId);
    }

    public async Task<Workout> GetWorkoutWithExercisesAndSetsAsync(Guid id, string userId)
    {
        return await this._context
            .Workouts
            .Where(w => w.UserId == userId && w.Id == id)
            .Include(w => w.WorkoutExercises)
            .ThenInclude(we => we.Sets)
            .Include(w => w.WorkoutExercises)
            .ThenInclude(we => we.Exercise)
            .AsNoTracking()
            .FirstAsync();
    }
    public IQueryable<Exercise> GetAllExercisesAsNoTracking()
    {
        return this._context
            .Exercises
            .AsNoTracking();
    }
    public async Task<bool> WorkoutExerciseExistsAsync(Guid workoutId, Guid workoutExerciseId)
    {
        return await this._context.
            WorkoutExercises
            .AnyAsync(we => we.WorkoutId == workoutId && we.Id == workoutExerciseId);
    }
    public async Task<bool> EntityExistsAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
            => await this._context.Set<TEntity>().AnyAsync(predicate);

    private async Task<int> SaveChangesAsync()
        => await this._context.SaveChangesAsync();

}
