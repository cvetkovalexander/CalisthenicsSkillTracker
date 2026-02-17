using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.ViewModels.ExerciseViewModels;
using CalisthenicsSkillTracker.ViewModels.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CalisthenicsSkillTracker.Services.Core.Interfaces;

public interface IExerciseInputService
{
    Task<CreateExerciseViewModel> CreateExerciseViewModelWithEnumsAsync();
    List<SelectListItem> FetchSelectedEnum(string key);
    void FetchEnums(IExerciseViewModel model);
    Task<bool> ExerciseNameExistsAsync(string name);
    string RemoveWhitespaces(string input);
    Task CreateExerciseAsync(CreateExerciseViewModel model);
    Task<EditExerciseViewModel> CreateEditExerciseViewModelAsync(Guid id);
    Task<Exercise> GetExerciseByIdAsync(Guid id);
    Task<bool> ExerciseNameExcludingCurrentExistsAsync(Guid id, string name);
    Task EditExerciseDataAsync(EditExerciseViewModel model);
    Task DeleteExerciseAsync(Guid id);
    Task<List<SelectListItem>> GetAvailableSkillsAsync();
    Task<Skill> GetSkillAsync(Guid id);
    Task<bool> SkillExistsAsync(Guid id);
}
