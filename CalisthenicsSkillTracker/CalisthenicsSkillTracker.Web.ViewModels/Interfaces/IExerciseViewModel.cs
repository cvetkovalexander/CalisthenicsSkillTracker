using CalisthenicsSkillTracker.Data.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CalisthenicsSkillTracker.ViewModels.Interfaces;

public interface IExerciseViewModel
{
    string Name { get; set; }
    string? Description { get; set; }

    string? ImageUrl { get; set; }
    Measurement Measurement { get; set; }
    List<SelectListItem> MeasurementOptions { get; set; }
    Category Category { get; set; }
    List<SelectListItem> CategoryOptions { get; set; }
    SkillType ExerciseType { get; set; }
    List<SelectListItem> ExerciseTypeOptions { get; set; }
    Difficulty Difficulty { get; set; }
    List<SelectListItem> DifficultyOptions { get; set; }
}
