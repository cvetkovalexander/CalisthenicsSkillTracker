using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.Data.Repositories.Contracts;

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
        Context.Skills.Update(skill);
        int resultCount = await this.SaveChangesAsync();

        return resultCount == 1;
    }
}
