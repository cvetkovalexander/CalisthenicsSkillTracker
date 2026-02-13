using CalisthenicsSkillTracker.ViewModels.SkillProgressViewModels;

namespace CalisthenicsSkillTracker.Services.Core.Interfaces;

public interface ISkillProgressService
{
    CreateSkillProgressViewModel CreateSkillProgressViewModel();

    Task CreateSkillProgress(CreateSkillProgressViewModel model);

    /* Helper methods */

    void PopulateSelectListItems(CreateSkillProgressViewModel model);
    Task<bool> UserExistsAsync(Guid id);
    Task<bool> SkillExistsAsync(Guid id);
}
