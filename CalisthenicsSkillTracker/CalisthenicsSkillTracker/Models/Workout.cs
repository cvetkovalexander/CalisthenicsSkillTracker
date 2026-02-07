using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CalisthenicsSkillTracker.Models;

using static Common.EntityConstants;
using static Common.EntityValidation.Workout;

public class Workout
{
    [Key]
    [Required]
    public Guid Id { get; set; }

    [Required]
    [Column(TypeName = DateTimeColumnType)]
    public DateTime Date { get; set; }

    [Required]
    [ForeignKey(nameof(User))]
    public Guid UserId { get; set; }

    [Required]
    public virtual User User { get; set; } = null!;

    [MaxLength(NotesMaxLength)]
    public string? Notes { get; set; } 

    public virtual ICollection<WorkoutExercise> WorkoutExercises { get; set; } 
        = new List<WorkoutExercise>();
}
