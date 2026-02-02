using System.ComponentModel.DataAnnotations;

namespace CalisthenicsSkillTracker.ViewModels.WorkoutViewModels;

public class WorkoutInProgressViewModel
{
    [Required]
    public DateTime Date { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [Required]
    public string Notes { get; set; } = null!;


}
