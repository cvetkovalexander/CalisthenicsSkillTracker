using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.ViewModels.Interfaces;
using CalisthenicsSkillTracker.ViewModels.SkillViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CalisthenicsSkillTracker.Services.Core.Interfaces;

public interface ISkillInputService
{
    Task CreateSkillAsync(CreateSkillViewModel model);

    Task<EditSkillViewModel> CreateEditSkillViewModelAsync(Guid id);

    Task EditSkillDataAsync(EditSkillViewModel model);

    Task DeleteSkillAsync(Guid id);

    /* Helper methods */
    Task<Skill> GetSkillByIdAsync(Guid id);
    Task<bool> SkillNameExistsAsync(string name);
    Task<bool> SkillNameExcludingCurrentExistsAsync(Guid id, string name);
    List<SelectListItem> FetchSelectedEnum(string key);
    CreateSkillViewModel CreateSkillViewModelWithEnums();
    void FetchEnums(ISkillViewModel model);
    string RemoveWhitespaces(string input);
    Task<List<SelectListItem>> GetAvailableExercisesAsync();
}
