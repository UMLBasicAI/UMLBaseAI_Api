using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using DeletePromptHistoryById.Common;
using DeletePromptHistoryById.Models;
using DeletePromptHistoryById.Presentation;
using DeletePromptHistoryById.Presentation.Filter.SetStageBag;

namespace DeletePromptHistoryById.Mapper;

public static class HttpResponseMapper
{
    private static ConcurrentDictionary<
        Constant.AppCode,
        Func<AppRequestModel, AppResponseModel, HttpContext, Response>
    > _httpResponseMapper;

    private static void Init()
    {
        if (Equals(_httpResponseMapper, null))
        {
            _httpResponseMapper = new();
         
            _httpResponseMapper.TryAdd(
                Constant.AppCode.SUCCESS,
                (appRequest, appResponse, httpContext) =>
                {
                    return new()
                    {
                        AppCode = Constant.AppCode.SUCCESS.ToString(),
                    };
                }
            );

            _httpResponseMapper.TryAdd(
                Constant.AppCode.SERVER_ERROR,
                (appRequest, appResponse, httpContext) =>
                {
                    return Constant.DefaultResponse.Http.SERVER_ERROR;
                }
            );
            _httpResponseMapper.TryAdd(
                Constant.AppCode.UNAUTHORIZED,
                (appRequest, appResponse, httpContext) =>
                {
                    return Constant.DefaultResponse.Http.UNAUTHORIZED;
                }
            );
        }
    }

    public static Response Get(
        AppRequestModel request,
        AppResponseModel response,
        HttpContext context
    )
    {
        Init();
        var stageBag = context.Items[nameof(StageBag)] as StageBag;

        var httpResponse = _httpResponseMapper[response.AppCode](request, response, context);
        stageBag!.HttpResponse = httpResponse;

        return httpResponse;
    }
}
