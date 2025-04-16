using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using GetSinglePromptHistory.Common;

namespace GetSinglePromptHistory.Presentation.Filter.SetStageBag
{
    public sealed class SetStageBagFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next
        )
        {
            // Kiểm tra xem request có tồn tại trong action arguments hay không
            var requestExists = context.ActionArguments.TryGetValue(
                Constant.REQUEST_ARGUMENT_NAME,
                out var requestObj
            );

            if (!requestExists || requestObj is not Request request)
            {
                context.Result = new ContentResult
                {
                    StatusCode = Constant.Http.VALIDATION_FAILED.HttpCode,
                    Content = JsonSerializer.Serialize(Constant.Http.VALIDATION_FAILED),
                    ContentType = MediaTypeNames.Application.Json
                };
                return;
            }

            // Gán request vào StageBag và lưu vào HttpContext
            var stageBag = new StageBag
            {
                HttpRequest = request
            };

            context.HttpContext.Items[nameof(StageBag)] = stageBag;

            // Tiếp tục thực thi pipeline
            await next();
        }
    }
}
