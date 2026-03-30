using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.Data.Repositories.Contracts.Admin;
using CalisthenicsSkillTracker.Services.Core.Contracts.Admin;
using CalisthenicsSkillTracker.ViewModels.Admin.Stats;

namespace CalisthenicsSkillTracker.Services.Core.Services.Admin;

public class StatsService : IStatsService
{
    private readonly IStatsRepository _repository;

    public StatsService(IStatsRepository repository)
    {
        this._repository = repository;
    }

    public async Task<OverviewViewModel> CreateOverviewViewModelAsync()
    {
        OverviewViewModel viewModel = new OverviewViewModel
        {
            TotalExercises = await this._repository.CountAsync<Exercise>(),
            TotalSkillRecords = await this._repository.CountAsync<SkillProgress>(),
            TotalSkills = await this._repository.CountAsync<Skill>(),
            TotalWorkouts = await this._repository.CountAsync<Workout>(),
            Workouts = await this.CreateWorkoutViewModelsAsync(),
            SkillRecords = await this.CreateSkillRecordViewModelsAsync()
        };

        return viewModel;
    }

    public async Task<IEnumerable<SkillRecordViewModel>> CreateSkillRecordViewModelsAsync()
    {
        IEnumerable<SkillProgress> records = await this._repository
            .GetAllSkillRecordsAsync(filterQuery: null,
            projectionQuery: sp => new SkillProgress 
            {
                PerformedBy = sp.PerformedBy,
                Skill = sp.Skill,
                Date = sp.Date,
                Progression = sp.Progression,
                Duration = sp.Duration,
                Repetitions = sp.Repetitions,
                Notes = sp.Notes,
            });
        
        IEnumerable<SkillRecordViewModel> models = records
            .Select(r => new SkillRecordViewModel
            {
                UserName = r.PerformedBy.UserName!,
                SkillName = r.Skill.Name,
                SubmittedOn = r.Date,
                Progression = ProgressDisplayName(r),
                Notes = r.Notes,
            })
            .ToArray();

        return models;
    }

    public async Task<IEnumerable<WorkoutViewModel>> CreateWorkoutViewModelsAsync()
    {
        IEnumerable<Workout> workouts = await this._repository
            .GetAllWorkoutsAsync(filterQuery: null,
            projectionQuery: w => new Workout 
            {
                User = w.User,
                Date = w.Date,
                Notes = w.Notes,
                WorkoutExercises = w.WorkoutExercises.Select(we => new WorkoutExercise 
                {
                    Exercise = we.Exercise
                }).ToArray()
            });

        IEnumerable<WorkoutViewModel> models = workouts
            .Select(w => new WorkoutViewModel
            {
                UserName = w.User.UserName!,
                SubmittedOn = w.Date,
                Notes = w.Notes,
                Exercises = w.WorkoutExercises
                    .Select(we => we.Exercise.Name)
                    .ToArray()
            });

        return models;
    }

    public string ProgressDisplayName(SkillProgress skillProgress)
    {
        string output = string.Empty;

        if (skillProgress.Progression is not null)
            output += skillProgress.Progression.ToString();

        if (skillProgress.Repetitions is not null)
            output += $" {skillProgress.Repetitions.ToString()} reps";

        if (skillProgress.Duration is not null)
            output += $" {skillProgress.Duration.ToString()} secs";

        return output;
    }
}
