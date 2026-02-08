using CalisthenicsSkillTracker.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace CalisthenicsSkillTracker.Models;

using static Common.EntityValidation.Skill;

public class Skill
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(NameMaxLength)]
    public string Name { get; set; } = null!;

    [MaxLength(DescriptionMaxLength)]
    public string? Description { get; set; }

    [Required]
    public Measurement MeasurementType { get; set; }

    [Required]
    public Category Category { get; set; }

    [Required]
    public SkillType SkillType { get; set; }

    [Required]
    public Difficulty Difficulty { get; set; }

    public virtual ICollection<SkillProgress> SkillProgressRecords { get; set; }
        = new List<SkillProgress>();

    public virtual ICollection<Exercise> Exercises { get; set; }
        = new List<Exercise>();
}
