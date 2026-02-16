namespace CalisthenicsSkillTracker.ViewModels.WorkoutViewModels;

public class WorkoutExerciseDetailsViewModel
{
    public Guid Id { get; set; }
    public Guid ExerciseId { get; set; }

    public string ExerciseName { get; set; } = null!;

    public ICollection<WorkoutSetDetailsViewModel> Sets { get; set; }
        = new HashSet<WorkoutSetDetailsViewModel>();
}
