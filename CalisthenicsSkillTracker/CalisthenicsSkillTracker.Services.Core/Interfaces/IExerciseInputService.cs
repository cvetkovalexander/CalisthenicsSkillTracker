using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.ViewModels.ExerciseViewModels;
using CalisthenicsSkillTracker.ViewModels.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CalisthenicsSkillTracker.Services.Core.Interfaces;

public interface IExerciseInputService
{
    Task CreateExerciseAsync(CreateExerciseViewModel model);
    Task<EditExerciseViewModel> CreateEditExerciseViewModelAsync(Guid id);
    Task<CreateExerciseViewModel> CreateExerciseViewModelWithEnumsAsync();
    Task EditExerciseDataAsync(EditExerciseViewModel model);
    Task DeleteExerciseAsync(Guid id);

    /* Helper methods */

    List<SelectListItem> FetchSelectedEnum(string key);
    Task<bool> ExerciseNameExcludingCurrentExistsAsync(Guid id, string name);
    Task<bool> ExerciseNameExistsAsync(string name);
    Task<List<SelectListItem>> GetAvailableSkillsAsync();
    Task<bool> SkillExistsAsync(Guid id);
    void FetchEnums(IExerciseViewModel model);
}
