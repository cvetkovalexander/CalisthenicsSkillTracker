using CalisthenicsSkillTracker.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CalisthenicsSkillTracker.ViewModels.WorkoutViewModels;

using static Common.EntityValidation.SkillSet;

public class AddWorkoutSetViewModel
{
    [Required]
    public Guid WorkoutId { get; set; }

    [Required]
    public Guid WorkoutExerciseId { get; set; }

    [Required]
    [Range(1, 100)]
    public int SetNumber { get; set; }

    [Range(RepetitionsMinValue, RepetitionsMaxValue)]
    public int? Repetitions { get; set; }

    [Range(DurationMinValue, DurationMaxValue)]
    public int? Duration { get; set; }

    public Progression? Progression { get; set; }

    public List<SelectListItem> Progressions { get; set; } 
        = new List<SelectListItem>();

    [MinLength(NotesMinLength)]
    [MaxLength(NotesMaxLength)]
    public string? Notes { get; set; }

    public List<SelectListItem> Exercises { get; set; } 
        = new List<SelectListItem>();
}
