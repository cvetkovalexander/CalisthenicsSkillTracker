using CalisthenicsSkillTracker.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CalisthenicsSkillTracker.Models;

using static Common.EntityValidation.SkillProgress;
using static Common.EntityConstants;

public class SkillProgress
{
    [Key]
    public Guid Id { get; set; }

    [ForeignKey(nameof(User))]
    public Guid UserId { get; set; }

    [Required]
    public virtual User PerformedBy { get; set; } = null!;

    [ForeignKey(nameof(Skill))]
    public Guid SkillId { get; set; }

    [Required]
    public virtual Skill Skill { get; set; } = null!;

    [Required]
    [Column(TypeName = DateTimeColumnType)]
    public DateTime Date { get; set; }

    public Progression? Progression { get; set; }

    public int? Repetitions { get; set; }

    public int? Duration { get; set; }

    [MaxLength(NotesMaxLength)]
    public string? Notes { get; set; }

    // Idea for the future: Add a optional property for video URL
}
