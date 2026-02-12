using CalisthenicsSkillTracker.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CalisthenicsSkillTracker.Data.Models;

using static GCommon.EntityValidation.Skill;

public class Exercise
{
    [Key]
    [Required]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(NameMaxLength)]
    public string Name { get; set; } = null!;

    [MaxLength(DescriptionMaxLength)]
    public string? Description { get; set; }

    [Required]
    public Difficulty Difficulty { get; set; }

    public virtual ICollection<Skill> Skills { get; set; } 
        = new List<Skill>();

    [Required]
    public SkillType ExerciseType { get; set; }

    public Category Category { get; set; }
}
