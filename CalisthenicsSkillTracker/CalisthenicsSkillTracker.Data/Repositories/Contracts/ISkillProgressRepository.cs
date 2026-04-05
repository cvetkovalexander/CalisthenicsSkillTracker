using CalisthenicsSkillTracker.Data.Models;

namespace CalisthenicsSkillTracker.Data.Repositories.Contracts;

public interface ISkillProgressRepository
{
    Task<bool> AddSkillProgressAsync(SkillProgress skillProgress);

    // TODO: Consider adding isDeleted property to SkillProgress and implementing soft delete instead of hard delete
    Task<bool> SoftDeleteSkillProgressAsync(SkillProgress skillProgress);

    Task<bool> HardDeleteSkillProgressAsync(SkillProgress skillProgress);

    IQueryable<SkillProgress> GetUserSkillProgressRecords(string userId);

    IQueryable<Skill> GetAllSkills();

    Task<bool> SkillRecordExistsAsync(Guid id);

    Task<bool> SkillExistsAsync(Guid id);

    Task<SkillProgress> GetSkillRecordAsync(Guid id);
}
