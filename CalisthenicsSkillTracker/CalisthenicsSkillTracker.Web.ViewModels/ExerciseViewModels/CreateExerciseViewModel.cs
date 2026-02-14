namespace CalisthenicsSkillTracker.ViewModels.ExerciseViewModels;

using CalisthenicsSkillTracker.Data.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using static GCommon.EntityValidation.Exercise;

public class CreateExerciseViewModel
{
    [Required]
    [MinLength(NameMinLength)]
    [MaxLength(NameMaxLength)]
    public string Name { get; set; } = null!;

    [MinLength(DescriptionMinLength)]
    [MaxLength(DescriptionMaxLength)]
    public string? Description { get; set; }

    [Required]
    public Measurement Measurement { get; set; }

    public List<SelectListItem> MeasurementOptions { get; set; }
        = new List<SelectListItem>();

    [Required]
    public SkillType ExerciseType { get; set; }
    public List<SelectListItem> ExerciseTypeOptions { get; set; }
        = new List<SelectListItem>();

    [Required]
    public Difficulty Difficulty { get; set; }
    public List<SelectListItem> DifficultyOptions { get; set; }
        = new List<SelectListItem>();

    [Required]
    public Category Category { get; set; }
    public List<SelectListItem> CategoryOptions { get; set; }
        = new List<SelectListItem>();
}
