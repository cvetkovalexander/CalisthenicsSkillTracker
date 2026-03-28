using CalisthenicsSkillTracker.ViewModels;
using CalisthenicsSkillTracker.ViewModels.ExerciseViewModels;

namespace CalisthenicsSkillTracker.Services.Core.Interfaces;

public interface IExerciseOutputService
{
    Task<bool> ExerciseExistsAsync(Guid id);
    Task<IEnumerable<ListTableItemViewModel>> GetAllExercisesAsync(string? filter);
    Task<DetailsExerciseViewModel> GetExerciseDetailsAsync(Guid id);
}
