namespace CalisthenicsSkillTracker.ViewModels.WorkoutViewModels;

using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using static GCommon.EntityValidation.Workout;

public class CreateWorkoutViewModel
{
    [Required]
    public DateTime Date { get; set; }

    [Required]
    public Guid UserId { get; set; }

    public IEnumerable<SelectListItem>? Users { get; set; }

    [MinLength(NotesMinLength)]
    [MaxLength(NotesMaxLength)]
    public string? Notes { get; set; }

}
