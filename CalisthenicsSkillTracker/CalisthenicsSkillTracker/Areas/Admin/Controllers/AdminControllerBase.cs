using static CalisthenicsSkillTracker.GCommon.ApplicationConstants;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace CalisthenicsSkillTracker.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public abstract class AdminControllerBase : Controller
{
    private readonly ILogger<AdminControllerBase> _logger;
    protected AdminControllerBase(ILogger<AdminControllerBase> logger)
    {
        this._logger = logger;
    }

    protected string? GetAdminUserId()
        => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    protected IActionResult SuccessRedirectToIndex(string message)
    {
        TempData[SuccessTempDataKey] = message;

        return this.RedirectToAction(nameof(Index));
    }

    protected IActionResult ErrorRedirectToIndex(string message)
    {
        TempData[ErrorTempDataKey] = message;

        this._logger.LogError(message);

        return this.RedirectToAction(nameof(Index));
    }
}
