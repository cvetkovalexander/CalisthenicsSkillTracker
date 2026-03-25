using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.Data.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CalisthenicsSkillTracker.Data.Repositories;

public class SkillProgressRepository : BaseRepository, ISkillProgressRepository
{
    public SkillProgressRepository(ApplicationDbContext context) 
        : base(context)
    {
    }

    public async Task<bool> AddSkillProgressAsync(SkillProgress skillProgress)
    {
        await this.Context.SkillProgressRecords.AddAsync(skillProgress);
        int resultCount = await this.SaveChangesAsync();

        return resultCount == 1;
    }
    public Task<bool> SoftDeleteSkillProgressAsync(SkillProgress skillProgress)
    {
        // TODO: Consider adding isDeleted property to Skill and implementing soft delete instead of hard delete
        throw new NotImplementedException();
    }

    public async Task<bool> HardDeleteSkillProgressAsync(SkillProgress skillProgress)
    {
        this.Context.SkillProgressRecords.Remove(skillProgress);
        int resultCount = await this.SaveChangesAsync();

        return resultCount == 1;
    }

    public IQueryable<SkillProgress> GetUserSkillProgressRecords(string userId)
        => this.Context
            .SkillProgressRecords
            .Where(sp => sp.UserId.ToString() == userId);

    public IQueryable<Skill> GetAllSkills()
        => this.Context
            .Skills
            .AsNoTracking();

    public async Task<bool> SkillRecordExistsAsync(Guid id)
        => await this.Context
            .SkillProgressRecords
            .AnyAsync(sp => sp.Id == id);

    public async Task<bool> SkillExistsAsync(Guid id)
        => await this.Context
            .Skills
            .AnyAsync(s => s.Id == id);

    public Task<SkillProgress> GetSkillRecord(Guid id)
        => this.Context
            .SkillProgressRecords
            .AsNoTracking()
            .SingleAsync(sp => sp.Id == id);
}
