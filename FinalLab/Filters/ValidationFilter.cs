using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Presentation.API.Filters;

public class ValidationFilter : IAsyncActionFilter
{
    private readonly IValidatorFactory _factory;
    public ValidationFilter(IValidatorFactory factory) => _factory = factory;

    public async Task OnActionExecutionAsync(ActionExecutingContext ctx, ActionExecutionDelegate next)
    {
        foreach (var arg in ctx.ActionArguments.Values)
        {
            if (arg is null) continue;
            var validator = _factory.GetValidator(arg.GetType());
            if (validator is null) continue;

            var result = await validator.ValidateAsync(new ValidationContext<object>(arg));
            if (!result.IsValid)
            {
                ctx.Result = new BadRequestObjectResult(
                    new ValidationProblemDetails(result.ToDictionary()));
                return;
            }
        }
        await next();   
    }
}
