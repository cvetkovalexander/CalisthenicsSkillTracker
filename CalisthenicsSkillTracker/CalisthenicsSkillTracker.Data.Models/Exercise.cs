using CalisthenicsSkillTracker.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;

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

    [Url(ErrorMessage = "Please enter valid URL.")]
    public string? ImageUrl { get; set; }

    [Required]
    public Difficulty Difficulty { get; set; }

    [Required]
    public Measurement MeasurementType { get; set; }

    public virtual ICollection<Skill> Skills { get; set; } 
        = new List<Skill>();

    [Required]
    public SkillType ExerciseType { get; set; }

    public Category Category { get; set; }
}
