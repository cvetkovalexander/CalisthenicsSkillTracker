using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CalisthenicsSkillTracker.Controllers;

[Authorize]
public abstract class ControllerBase : Controller
{
    protected string? GetUserId()
        => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
}
