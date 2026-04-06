using CalisthenicsSkillTracker.Data.Models;
using System.Linq.Expressions;

namespace CalisthenicsSkillTracker.Data.Repositories.Contracts;

public interface ISkillRepository
{
    Task<bool> AddSkillAsync(Skill skill);

    Task<bool> EditSkillAsync(Skill skill);

    // TODO: Consider adding isDeleted property to Skill and implementing soft delete instead of hard delete
    Task<bool> SoftDeleteSkillAsync(Skill skill);

    Task<bool> HardDeleteSkillAsync(Skill skill);

    Task<bool> RemoveSkillFromFavorites(Skill skill, ApplicationUser user);

    Task<bool> AddSkillToFavorites(Skill skill, ApplicationUser user);

    Task<ApplicationUser> GetUserWithFavoriteSkillsAsync(Guid userId);

    Task<HashSet<Guid>> GetUserFavoriteSkills(Guid userId);

    Task<bool> SkillNameExistsAsync(string name);

    Task<bool> SkillNameExcludingCurrentExistsAsync(Guid id, string name);

    Task<Skill> GetSkillByIdAsync(Guid id);

    Task<Skill> GetTrackedSkillByIdAsync(Guid id);

    public string RemoveWhitespaces(string input);

    IQueryable<Skill> GetAllSkills(Expression<Func<Skill, bool>>? filterQuery = null, Expression<Func<Skill, Skill>>? projectionQuery = null);

    Task<Skill> GetSkillWithExercisesByIdAsync(Guid id);

    Task<bool> SkillExistsAsync(Guid id);
}
