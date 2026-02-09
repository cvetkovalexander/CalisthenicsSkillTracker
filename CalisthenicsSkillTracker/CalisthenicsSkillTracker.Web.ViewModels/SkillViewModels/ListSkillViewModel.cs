using CalisthenicsSkillTracker.Data.Models.Enums;

namespace CalisthenicsSkillTracker.ViewModels.SkillViewModels;

public class ListSkillViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public Difficulty Difficulty { get; set; }
}
