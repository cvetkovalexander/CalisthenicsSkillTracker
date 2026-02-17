namespace CalisthenicsSkillTracker.ViewModels.SkillProgressViewModels;

public class ListRecordViewModel
{
    public Guid Id { get; set; }
    public string SkillName { get; set; } = null!;
    public DateTime Date { get; set; }
    public string? Progression { get; set; }
    public int? Repetitions { get; set; }
    public int? Duration { get; set; } // in seconds
    public string? Notes { get; set; }
}

