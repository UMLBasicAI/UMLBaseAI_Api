using System.Net.Mime;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using GetPromptHistories.Common;
using GetPromptHistories.Presentation.Filter.SetStageBag;

namespace GetPromptHistories.Presentation.Validation;

public sealed class ValidationFilter : IAsyncActionFilter
{
    private readonly IValidator<Request> _validator;

    public ValidationFilter(IValidator<Request> validator)
    {
        _validator = validator;
    }

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next
    )
    {
        var stageBage = context.HttpContext.Items[nameof(StageBag)] as StageBag;
        var request = stageBage!.HttpRequest;
        var result = await _validator.ValidateAsync(request);

        if (!result.IsValid)
        {
            context.Result = new ContentResult
            {
                StatusCode = Constant.Http.VALIDATION_FAILED.HttpCode,
                Content = JsonSerializer.Serialize(Constant.Http.VALIDATION_FAILED),
                ContentType = MediaTypeNames.Application.Json,
            };
            return;
        }
        await next();
    }
}
