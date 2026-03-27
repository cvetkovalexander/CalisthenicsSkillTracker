using CalisthenicsSkillTracker.ViewModels.Admin.User;

namespace CalisthenicsSkillTracker.Services.Core.Interfaces.Admin;

public interface IUserService
{
    Task<IEnumerable<ManageUserViewModel>> GetAllManageableUsersAsync(string adminUserId);
}
