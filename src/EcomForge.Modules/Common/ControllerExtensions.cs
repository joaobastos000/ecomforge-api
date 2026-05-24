using EcomForge.Common.Results;
using Microsoft.AspNetCore.Mvc;

namespace EcomForge.Modules.Common;

internal static class ControllerExtensions
{
    public static ActionResult ToActionResult(this ControllerBase controller, Result result)
    {
        return result.IsSuccess
            ? controller.NoContent()
            : controller.BadRequest(new { result.Error.Code, result.Error.Message });
    }

    public static ActionResult<T> ToActionResult<T>(this ControllerBase controller, Result<T> result)
    {
        return result.IsSuccess
            ? controller.Ok(result.Value)
            : controller.BadRequest(new { result.Error.Code, result.Error.Message });
    }
}
