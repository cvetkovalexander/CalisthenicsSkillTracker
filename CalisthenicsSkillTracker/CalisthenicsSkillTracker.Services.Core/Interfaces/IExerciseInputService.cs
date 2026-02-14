using CalisthenicsSkillTracker.ViewModels.ExerciseViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CalisthenicsSkillTracker.Services.Core.Interfaces;

public interface IExerciseInputService
{
    CreateExerciseViewModel CreateExerciseViewModelWithEnums();
    List<SelectListItem> FetchSelectedEnum(string key);
    void FetchEnums(CreateExerciseViewModel model);
    Task<bool> ExerciseNameExistsAsync(string name);
    string RemoveWhitespaces(string input);
    Task CreateExerciseAsync(CreateExerciseViewModel model);
}
