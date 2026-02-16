namespace CalisthenicsSkillTracker.ViewModels.WorkoutViewModels;

public class WorkoutDetailsViewModel
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan Start { get; set; }
    public TimeSpan End { get; set; }
    public TimeSpan Duration { get; set; }
    public string? Notes { get; set; }
}
