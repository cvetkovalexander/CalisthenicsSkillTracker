using CalisthenicsSkillTracker.Data.Models.Enums;

namespace CalisthenicsSkillTracker.ViewModels.WorkoutViewModels;

public class WorkoutSetDetailsViewModel
{
    public Guid Id { get; set; }
    public int SetNumber { get; set; }

    public int? Repetitions { get; set; }
    public int? Duration { get; set; }

    public Progression? Progression { get; set; }

    public string? Notes { get; set; }
}
