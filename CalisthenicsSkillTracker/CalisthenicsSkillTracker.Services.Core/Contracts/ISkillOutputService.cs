using CalisthenicsSkillTracker.ViewModels;
using CalisthenicsSkillTracker.ViewModels.SkillViewModels;
using static CalisthenicsSkillTracker.GCommon.ApplicationConstants;

namespace CalisthenicsSkillTracker.Services.Core.Interfaces;

public interface ISkillOutputService
{
    Task<PaginationResultViewModel<ListTableItemViewModel>> GetAllSkillsAsync(string? lastName, Guid? lastId, string? filter = null, int pageSize = DefaultPageSize);

    Task<DetailsSkillViewModel> GetSkillDetailsAsync(Guid id);

    Task<bool> SkillExistsAsync(Guid id);
}
