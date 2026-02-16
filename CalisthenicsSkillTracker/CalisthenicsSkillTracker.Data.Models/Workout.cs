using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CalisthenicsSkillTracker.Data.Models;

using static GCommon.EntityConstants;
using static GCommon.EntityValidation.Workout;

public class Workout
{
    [Key]
    [Required]
    public Guid Id { get; set; }

    [Required]
    [Column(TypeName = DateTimeColumnType)]
    public DateTime Date { get; set; }

    [Required]
    public TimeSpan Start { get; set; }

    [Required]
    public TimeSpan End { get; set; }

    [Required]
    public TimeSpan Duration { get; set; }

    [Required]
    [ForeignKey(nameof(User))]
    public string UserId { get; set; } = null!;

    [Required]
    public virtual ApplicationUser User { get; set; } = null!;

    [MaxLength(NotesMaxLength)]
    public string? Notes { get; set; } 

    public virtual ICollection<WorkoutExercise> WorkoutExercises { get; set; } 
        = new List<WorkoutExercise>();
}
