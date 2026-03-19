    using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.Data.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CalisthenicsSkillTracker.Data.Repositories;

public class SkillRepository : BaseRepository, ISkillRepository
{
    public SkillRepository(ApplicationDbContext context) 
        : base(context)
    {
    }

    public async Task<bool> AddSkillAsync(Skill skill)
    {
        await this.Context.Skills.AddAsync(skill);
        int resultCount = await this.SaveChangesAsync();

        return resultCount == 1;
    }

    public async Task<bool> EditSkillAsync(Skill skill)
    {
        this.Context.Skills.Update(skill);
        int resultCount = await this.SaveChangesAsync();

        return resultCount == 1;
    }
    public Task<bool> SoftDeleteSkillAsync(Skill skill)
    {
        // TODO: Consider adding isDeleted property to Skill and implementing soft delete instead of hard delete
        throw new NotImplementedException();
    }

    public async Task<bool> HardDeleteSkillAsync(Skill skill)
    {
        this.Context.Skills.Remove(skill);
        int resultCount = await this.SaveChangesAsync();

        return resultCount == 1;
    }

    public async Task<bool> SkillNameExistsAsync(string name)
        => await this.Context.Skills.AnyAsync(s => s.Name.ToLower() == this.RemoveWhitespaces(name).ToLower());

    public async Task<bool> SkillNameExcludingCurrentExistsAsync(Guid id, string name)
        => await this.Context
            .Skills
            .AsNoTracking()
            .AnyAsync(s => s.Id != id && s.Name.ToLower() == this.RemoveWhitespaces(name).ToLower());

    public async Task<Skill> GetSkillByIdAsync(Guid id)
      => await this.Context
            .Skills
            .AsNoTracking()
            .SingleAsync(s => s.Id == id);

    public IQueryable<Skill> GetAllSkills()
        =>  this.Context
            .Skills
            .AsNoTracking();

    public async Task<Skill> GetSkillWithExercisesByIdAsync(Guid id)
        => await this.Context
            .Skills
            .AsNoTracking()
            .Include(s => s.Exercises)
            .SingleAsync(s => s.Id == id);

    public async Task<bool> SkillExistsAsync(Guid id)
        => await this.Context
            .Skills
            .AnyAsync(s => s.Id == id);

    public string RemoveWhitespaces(string input)
        => string.Concat(input.Where(c => !char.IsWhiteSpace(c)));
}
