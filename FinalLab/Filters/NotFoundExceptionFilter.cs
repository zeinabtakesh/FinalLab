using Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Presentation.API.Filters;

public sealed class NotFoundExceptionFilter : IExceptionFilter
{
    private readonly ILogger<NotFoundExceptionFilter> _log;
    public NotFoundExceptionFilter(ILogger<NotFoundExceptionFilter> log) => _log = log;

    public void OnException(ExceptionContext context)
    {
        if (context.Exception is NotFoundException ex)
        {
            _log.LogWarning(ex, "Resource not found");
            context.Result = new NotFoundObjectResult(new { error = ex.Message });
            context.ExceptionHandled = true;
        }
    }
}