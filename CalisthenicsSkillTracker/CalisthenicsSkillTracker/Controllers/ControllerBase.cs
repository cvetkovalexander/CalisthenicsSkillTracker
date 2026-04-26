using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

using static CalisthenicsSkillTracker.GCommon.ApplicationConstants;

namespace CalisthenicsSkillTracker.Controllers;

[Authorize]
public abstract class ControllerBase : Controller
{
    private ILogger _logger =>
        this.HttpContext.RequestServices
        .GetRequiredService<ILoggerFactory>()
        .CreateLogger(this.GetType());

    protected string? GetUserId()
        => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    protected void HandleException(Exception exception, string errorMessage)
    {
        this._logger.LogError(exception, errorMessage);

        this.TempData[ErrorTempDataKey] = errorMessage;
    }

    protected void HandleUnexpectedException(Exception exception)
    {
        this._logger.LogError(exception, UnexpectedErrorMessage);

        this.TempData[ErrorTempDataKey] = UnexpectedErrorMessage;
    }
}
