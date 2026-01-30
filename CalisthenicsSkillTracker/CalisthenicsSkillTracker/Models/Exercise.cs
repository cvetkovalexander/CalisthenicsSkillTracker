using CalisthenicsSkillTracker.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CalisthenicsSkillTracker.Models;

public class Exercise
{
    [Key]
    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    [Required]
    public Difficulty Difficulty { get; set; }

    [Required]
    [ForeignKey(nameof(Skill))]
    public Guid SkillId { get; set; }

    [Required]
    public virtual Skill Skill { get; set; } = null!;

    [Required]
    public SkillType ExerciseType { get; set; }

    public Category Category { get; set; }
}
