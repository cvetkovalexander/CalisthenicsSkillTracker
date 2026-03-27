using Microsoft.AspNetCore.Identity;

namespace CalisthenicsSkillTracker.Data.Models;

public class ApplicationUserRole : IdentityUserRole<Guid>
{
    public ApplicationUser User { get; set; } = null!;
    public IdentityRole<Guid> Role { get; set; } = null!;
}
