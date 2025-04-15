using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PromptToAI.Common;
using PromptToAI.Models;
using PromptToAI.Presentation;
using PromptToAI.Presentation.Filter.SetStageBag;

namespace PromptToAI.Mapper;

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
                Constant.AppCode.USER_NOT_FOUND,
                (appRequest, appResponse, httpContext) =>
                {
                    return Constant.DefaultResponse.Http.USER_NOT_FOUND;
                }
            );

            _httpResponseMapper.TryAdd(
                Constant.AppCode.SUCCESS,
                (appRequest, appResponse, httpContext) =>
                {
                    return new()
                    {
                        HttpCode = StatusCodes.Status200OK,
                        AppCode = Constant.AppCode.SUCCESS.ToString(),
                        Body = new()
                        {
                            HistoryId = appResponse.Body.HistoryId,
                            PlantUML = appResponse.Body.PlantUML,
                            ResponseText = appResponse.Body.ResponseText,
                        },
                    };
                }
            );

            _httpResponseMapper.TryAdd(
                Constant.AppCode.VALIDATION_FAILED,
                (appRequest, appResponse, httpContext) =>
                {
                    return new()
                    {
                        HttpCode = StatusCodes.Status400BadRequest,
                        AppCode = Constant.AppCode.VALIDATION_FAILED.ToString()
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
                Constant.AppCode.REFRESH_TOKEN_IS_NOT_FOUND,
                (appRequest, appResponse, httpContext) =>
                {
                    return Constant.DefaultResponse.Http.REFRESH_TOKEN_IS_NOT_FOUND;
                }
            );
            _httpResponseMapper.TryAdd(
                Constant.AppCode.REFRESH_TOKEN_IS_EXPIRED,
                (appRequest, appResponse, httpContext) =>
                {
                    return Constant.DefaultResponse.Http.REFRESH_TOKEN_IS_EXPIRED;
                }
            );
        }
    }

    public static Response Get(
        AppRequestModel request,
        Models.AppResponseModel response,
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
