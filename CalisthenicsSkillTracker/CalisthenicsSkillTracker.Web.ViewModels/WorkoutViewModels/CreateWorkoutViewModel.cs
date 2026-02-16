namespace CalisthenicsSkillTracker.ViewModels.WorkoutViewModels;

using CalisthenicsSkillTracker.GCommon.Utilities.Attributes;
using System.ComponentModel.DataAnnotations;
using static GCommon.EntityValidation.Workout;

public class CreateWorkoutViewModel
{
    [Required]
    [NotFutureDate]
    public DateTime Date { get; set; }

    [Required]
    public string UserId { get; set; } = null!;

    [Required]
    public string Start { get; set; } = null!;

    [Required]
    public string End { get; set; } = null!;

    [MinLength(NotesMinLength)]
    [MaxLength(NotesMaxLength)]
    public string? Notes { get; set; }

}
