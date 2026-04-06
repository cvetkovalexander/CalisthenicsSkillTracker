using CalisthenicsSkillTracker.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace CalisthenicsSkillTracker.Data.Models;

using static GCommon.EntityValidation.Skill;

public class Skill
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(NameMaxLength)]
    public string Name { get; set; } = null!;

    [MaxLength(DescriptionMaxLength)]
    public string? Description { get; set; }

    [Url(ErrorMessage = "Please enter valid URL.")]
    public string? ImageUrl { get; set; }

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

    public virtual ICollection<ApplicationUser> FavoritedByUsers { get; set; }
        = new List<ApplicationUser>();
}
