using CalisthenicsSkillTracker.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CalisthenicsSkillTracker.Models;

public class WorkoutSet
{
    [Key]
    [Required]
    public Guid Id { get; set; }

    [Required]
    [ForeignKey(nameof(WorkoutExercise))]
    public Guid WorkoutExerciseId { get; set; }

    [Required]
    public virtual WorkoutExercise WorkoutExercise { get; set; } = null!;

    public int SetNumber { get; set; }

    public int? Repetitions { get; set; }

    public int? Duration { get; set; }

    public Progression? Progression { get; set; }

    public string? Notes { get; set; }
}
