namespace CalisthenicsSkillTracker.ViewModels.WorkoutViewModels;

using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

public class CreateWorkoutViewModel
{
    [Required]
    public DateTime Date { get; set; }

    [Required]
    public Guid UserId { get; set; }

    public IEnumerable<SelectListItem>? Users { get; set; }

    [Required]
    public string Notes { get; set; } = null!;

}
