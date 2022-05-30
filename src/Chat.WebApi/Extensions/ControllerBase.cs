using Chat.Core;
using Microsoft.AspNetCore.Mvc;

namespace Chat.WebApi.Extensions;

public class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
{
    [NonAction]
    protected ObjectResult InternalServerError()
    {
        return new ObjectResult(new ApiResponse
        {
            Errors = new[] { "Oops! Something went wrong" }
        });
    }
}