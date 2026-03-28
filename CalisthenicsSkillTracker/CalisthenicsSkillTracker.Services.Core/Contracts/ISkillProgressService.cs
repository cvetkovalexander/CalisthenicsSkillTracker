using CalisthenicsSkillTracker.ViewModels.SkillProgressViewModels;

namespace CalisthenicsSkillTracker.Services.Core.Interfaces;

public interface ISkillProgressService
{
    CreateSkillProgressViewModel CreateSkillProgressViewModel(string userId);
    Task<IEnumerable<ListRecordViewModel>> GetRecordsAsync(string userId);
    Task CreateSkillProgress(CreateSkillProgressViewModel model);
    Task DeleteSkillRecordAsync(Guid id);

    /* Helper methods */
    void PopulateSelectListItems(CreateSkillProgressViewModel model);
    Task<bool> SkillExistsAsync(Guid id);
    Task<bool> SkillRecordExistsAsync(Guid id);
}
