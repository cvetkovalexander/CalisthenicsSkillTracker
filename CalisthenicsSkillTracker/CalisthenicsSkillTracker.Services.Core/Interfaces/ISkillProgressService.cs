using CalisthenicsSkillTracker.ViewModels.SkillProgressViewModels;

namespace CalisthenicsSkillTracker.Services.Core.Interfaces;

public interface ISkillProgressService
{
    CreateSkillProgressViewModel CreateSkillProgressViewModel();

    Task CreateSkillProgress(CreateSkillProgressViewModel model);

    /* Helper methods */

    void PopulateSelectListItems(CreateSkillProgressViewModel model);
    Task<bool> UserExistsAsync(string id);
    Task<bool> SkillExistsAsync(Guid id);
}
