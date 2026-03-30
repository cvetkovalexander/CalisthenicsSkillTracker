namespace CalisthenicsSkillTracker.ViewModels.Admin.Stats;

public class SkillRecordViewModel
{
    public string UserName { get; set; } = null!;

    public string SkillName { get; set; } = null!;

    public DateTime SubmittedOn { get; set; }

    public string Progression { get; set; } = null!;
}
