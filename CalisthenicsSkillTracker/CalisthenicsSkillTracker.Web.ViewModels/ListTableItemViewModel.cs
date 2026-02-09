using CalisthenicsSkillTracker.Data.Models.Enums;

namespace CalisthenicsSkillTracker.ViewModels;

public class ListTableItemViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public Difficulty Difficulty { get; set; }
}
