using CalisthenicsSkillTracker.Data.Models;

namespace CalisthenicsSkillTracker.Data.Repositories.Contracts;

public interface ISkillRepository
{
    Task<bool> AddSkillAsync(Skill skill);

    Task<bool> EditSkillAsync(Skill skill);
}
