namespace CalisthenicsSkillTracker.ViewModels.WorkoutViewModels;

public class WorkoutExercisesViewModel
{
    public Guid WorkoutId { get; set; }
    public ICollection<WorkoutExerciseDetailsViewModel> Exercises { get; set; }
        = new HashSet<WorkoutExerciseDetailsViewModel>();
}
