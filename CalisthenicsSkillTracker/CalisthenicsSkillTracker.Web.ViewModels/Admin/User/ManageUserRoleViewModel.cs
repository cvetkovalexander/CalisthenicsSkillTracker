using System.ComponentModel.DataAnnotations;

namespace CalisthenicsSkillTracker.ViewModels.Admin.User;

public class ManageUserRoleViewModel
{
    [Required]
    public string UserId { get; set; } = null!;

    [Required]
    public string Role { get; set; } = null!;
}
