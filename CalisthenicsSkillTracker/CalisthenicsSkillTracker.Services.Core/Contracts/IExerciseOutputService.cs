using CalisthenicsSkillTracker.ViewModels;
using CalisthenicsSkillTracker.ViewModels.ExerciseViewModels;
using static CalisthenicsSkillTracker.GCommon.ApplicationConstants;

namespace CalisthenicsSkillTracker.Services.Core.Interfaces;

public interface IExerciseOutputService
{
    Task<bool> ExerciseExistsAsync(Guid id);
    Task<PaginationResultViewModel<ListTableItemViewModel>> GetAllExercisesAsync(string? indexName, Guid? indexId, bool isPreviousPage, string? filter = null, int pageSize = DefaultPageSize);
    Task<DetailsExerciseViewModel> GetExerciseDetailsAsync(Guid id);
}
