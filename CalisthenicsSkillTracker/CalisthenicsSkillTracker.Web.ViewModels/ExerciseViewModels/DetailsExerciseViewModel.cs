using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.Data.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CalisthenicsSkillTracker.ViewModels.ExerciseViewModels;

public class DetailsExerciseViewModel
{
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public Measurement Measurement { get; set; }

    public List<SelectListItem> MeasurementOptions { get; set; } = new List<SelectListItem>();
    public Category Category { get; set; }

    public List<SelectListItem> CategoryOptions { get; set; } = new List<SelectListItem>();

    public SkillType ExerciseType { get; set; }

    public List<SelectListItem> SkillTypeOptions { get; set; } = new List<SelectListItem>();

    public Difficulty Difficulty { get; set; }

    public List<SelectListItem> DifficultyOptions { get; set; } = new List<SelectListItem>();

    public ICollection<Skill> Skills { get; set; }
        = new List<Skill>();
}
