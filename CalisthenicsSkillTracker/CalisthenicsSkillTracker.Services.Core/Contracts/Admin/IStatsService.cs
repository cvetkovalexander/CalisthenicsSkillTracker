using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.ViewModels.Admin.Stats;

namespace CalisthenicsSkillTracker.Services.Core.Contracts.Admin;

public interface IStatsService
{
    Task<OverviewViewModel> CreateOverviewViewModelAsync();

    Task<IEnumerable<WorkoutViewModel>> CreateWorkoutViewModelsAsync();

    Task<IEnumerable<SkillRecordViewModel>> CreateSkillRecordViewModelsAsync();

    string ProgressDisplayName(SkillProgress skillProgress);
}
