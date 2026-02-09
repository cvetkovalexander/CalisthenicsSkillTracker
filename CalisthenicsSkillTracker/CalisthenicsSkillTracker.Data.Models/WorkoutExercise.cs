using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CalisthenicsSkillTracker.Data.Models;

public class WorkoutExercise
{
    [Key]
    [Required]
    public Guid Id { get; set; }

    [Required]
    [ForeignKey(nameof(Workout))]
    public Guid WorkoutId { get; set; }

    [Required]
    public virtual Workout Workout { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(Exercise))]
    public Guid ExerciseId { get; set; }

    [Required]
    public virtual Exercise Exercise { get; set; } = null!;

    public virtual ICollection<WorkoutSet> Sets { get; set; } 
        = new List<WorkoutSet>();

}
