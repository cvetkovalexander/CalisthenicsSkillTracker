using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.Data.Models.Enums;

namespace CalisthenicsSkillTracker.ViewModels.ExerciseViewModels;

public class DetailsExerciseViewModel
{
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public Measurement Measurement { get; set; }

    public Category Category { get; set; }


    public SkillType ExerciseType { get; set; }


    public Difficulty Difficulty { get; set; }

    public ICollection<Skill> Skills { get; set; }
        = new List<Skill>();
}
