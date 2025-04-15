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
                Constant.AppCode.EMAIL_ALREADY_EXISTS,
                (appRequest, appResponse, httpContext) =>
                {
                    return Constant.DefaultResponse.Http.EMAIL_ALREADY_EXISTS;
                }
            );

            _httpResponseMapper.TryAdd(
                Constant.AppCode.PASSWORD_IS_INVALID,
                (appRequest, appResponse, httpContext) =>
                {
                    return Constant.DefaultResponse.Http.PASSWORD_IS_INVALID;
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
