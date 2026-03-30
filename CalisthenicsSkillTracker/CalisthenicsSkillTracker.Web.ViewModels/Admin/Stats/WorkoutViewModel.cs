namespace CalisthenicsSkillTracker.ViewModels.Admin.Stats;

public class WorkoutViewModel
{
    public string UserName { get; set; } = null!;

    public DateTime SubmittedOn { get; set; }

    public string? Notes { get; set; }

    public IReadOnlyCollection<string> Exercises { get; set; }
        = new List<string>();
}
