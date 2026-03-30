namespace CalisthenicsSkillTracker.ViewModels.Admin.Stats;

public class OverviewViewModel
{
    public int TotalExercises { get; set; }

    public int TotalSkills { get; set; }

    public int TotalWorkouts { get; set; }

    public int TotalSkillRecords { get; set; }

    public IEnumerable<WorkoutViewModel> Workouts { get; set; }
        = new List<WorkoutViewModel>();

    public IEnumerable<SkillRecordViewModel> SkillRecords { get; set; }
        = new List<SkillRecordViewModel>();
}
