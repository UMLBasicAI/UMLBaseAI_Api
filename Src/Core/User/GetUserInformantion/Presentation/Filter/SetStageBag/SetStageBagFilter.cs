using System.Net.Mime;
using System.Text.Json;
using GetUserInformation.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GetUserInformation.Presentation.Filter.SetStageBag;

public sealed class SetStageBagFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next
    )
    {
        var doesRequestExist = context.ActionArguments.Any(argument =>
            argument.Key.Equals(Constant.REQUEST_ARGUMENT_NAME)
        );

        if (!doesRequestExist)
        {
            context.Result = new ContentResult
            {
                StatusCode = Constant.DefaultResponse.Http.VALIDATION_FAILED.HttpCode,
                Content = JsonSerializer.Serialize(Constant.DefaultResponse.Http.VALIDATION_FAILED),
                ContentType = MediaTypeNames.Application.Json,
            };
            return;
        }

        var stageBag = new StageBag
        {
            HttpRequest = context.ActionArguments[Constant.REQUEST_ARGUMENT_NAME] as Request,
        };

        context.HttpContext.Items.Add(nameof(StageBag), stageBag);

        await next();
    }
}
