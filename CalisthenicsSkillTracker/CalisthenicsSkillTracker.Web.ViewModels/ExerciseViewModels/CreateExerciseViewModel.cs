namespace CalisthenicsSkillTracker.ViewModels.ExerciseViewModels;

using CalisthenicsSkillTracker.Data.Models.Enums;
using CalisthenicsSkillTracker.ViewModels.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using static GCommon.EntityValidation.Exercise;

public class CreateExerciseViewModel : IExerciseViewModel
{
    [Required]
    [MinLength(NameMinLength)]
    [MaxLength(NameMaxLength)]
    public string Name { get; set; } = null!;

    public string? SkillId { get; set; }

    [MinLength(DescriptionMinLength, ErrorMessage = "Description must be with a minimum length of 5 characters.")]
    [MaxLength(DescriptionMaxLength, ErrorMessage = "Description must be with a maximum length of 500 characters.")]
    public string? Description { get; set; }

    [Url(ErrorMessage = "Please enter a valid image URL")]
    public string? ImageUrl { get; set; }

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

    public List<SelectListItem> AvailableExercises { get; set; }
        = new List<SelectListItem>();
}
