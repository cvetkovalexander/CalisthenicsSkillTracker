using CalisthenicsSkillTracker.ViewModels;
using CalisthenicsSkillTracker.ViewModels.SkillViewModels;

namespace CalisthenicsSkillTracker.Services.Core.Interfaces;

public interface ISkillOutputService
{
    Task<IEnumerable<ListTableItemViewModel>> GetAllSkillsAsync(string? filter);

    Task<DetailsSkillViewModel> GetSkillDetailsAsync(Guid id);

    Task<bool> SkillExistsAsync(Guid id);
}
