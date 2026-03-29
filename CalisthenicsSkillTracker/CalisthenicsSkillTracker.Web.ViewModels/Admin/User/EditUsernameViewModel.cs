using static CalisthenicsSkillTracker.GCommon.EntityValidation.User;

using System.ComponentModel.DataAnnotations;

namespace CalisthenicsSkillTracker.ViewModels.Admin.User;

public class EditUsernameViewModel
{
    [Required]
    public string UserId { get; set; } = null!;

    [Required]
    [MinLength(UsernameMinLength, ErrorMessage = "Username must be at least 5 characters.")]
    [MaxLength(UsernameMaxLength, ErrorMessage = "Username must be Username must not exceed 30 characters.")]
    public string UserName { get; set; } = null!;
}
