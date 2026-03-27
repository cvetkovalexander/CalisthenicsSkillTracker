using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CalisthenicsSkillTracker.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public abstract class AdminControllerBase : Controller
{
    protected string? GetAdminUserId()
        => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
}
