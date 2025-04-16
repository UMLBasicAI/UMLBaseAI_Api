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
        var stageBag = new StageBag();

        if (
            context.ActionArguments.TryGetValue(Constant.REQUEST_ARGUMENT_NAME, out var requestObj)
            && requestObj is Request request
        )
        {
            stageBag.HttpRequest = request;
        }

        context.HttpContext.Items[nameof(StageBag)] = stageBag;

        await next();
    }
}
