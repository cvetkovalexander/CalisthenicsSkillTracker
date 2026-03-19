using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.Data.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CalisthenicsSkillTracker.Data.Repositories;

public class ExerciseRepository : BaseRepository, IExerciseRepository
{
    public ExerciseRepository(ApplicationDbContext context) 
        : base(context)
    {
    }

    public async Task<bool> AddExerciseAsync(Exercise exercise, bool isAdded)
    {
        await this.Context.Exercises.AddAsync(exercise);
        int resultCount = await this.SaveChangesAsync();

        if (isAdded)
            return resultCount == 2;

        return resultCount == 1;
    }

    public async Task<bool> EditExerciseAsync(Exercise exercise, bool isAdded)
    {
        this.Context.Exercises.Update(exercise);
        int resultCount = await this.SaveChangesAsync(); 
        
        if (isAdded)
            return resultCount == 2;
        
        return resultCount == 1;
    }

    public Task<bool> SoftDeleteExerciseAsync(Exercise exercise)
    {
        // TODO: Consider adding isDeleted property to Skill and implementing soft delete instead of hard delete
        throw new NotImplementedException();
    }

    public async Task<bool> HardDeleteExerciseAsync(Exercise exercise)
    {
        this.Context.Exercises.Remove(exercise);
        int resultCount = await this.SaveChangesAsync();

        return resultCount == 1;
    }

    public async Task<bool> ExerciseNameExcludingCurrentExistsAsync(Guid id, string name)
        => await this.Context
            .Exercises
            .AsNoTracking()
            .AnyAsync(e => e.Id != id && e.Name.ToLower() == this.RemoveWhitespaces(name).ToLower());

    public async Task<bool> ExerciseNameExistsAsync(string name)
        => await this.Context.Exercises.AnyAsync(s => s.Name.ToLower() == this.RemoveWhitespaces(name).ToLower());

    public async Task<Exercise> GetExerciseByIdAsync(Guid id)
        => await this.Context
            .Exercises
            .AsNoTracking()
            .SingleAsync(e => e.Id == id);
    public IQueryable<Skill> GetAllSkills()
        => this.Context
            .Skills
            .AsNoTracking();
    
    public async Task<Skill> GetSkillAsync(Guid id)
        => await this.Context
            .Skills
            .FirstAsync(s => s.Id == id);
    public async Task<bool> SkillExistsAsync(Guid id)
        => await this.Context
            .Skills
            .AnyAsync(s => s.Id == id);

    public IQueryable<Exercise> GetAllExercises()
        => this.Context
            .Exercises
            .AsNoTracking();
    public async Task<Exercise> GetExerciseWithSkillsAsync(Guid id)
        => await this.Context 
            .Exercises
            .AsNoTracking()
            .Include(e => e.Skills)
            .SingleAsync(e => e.Id == id);
    public async Task<bool> ExerciseExistsAsync(Guid id)
        => await this.Context
            .Exercises
            .AnyAsync(e => e.Id == id);

    public string RemoveWhitespaces(string input)
        => string.Concat(input.Where(c => !char.IsWhiteSpace(c)));

}
