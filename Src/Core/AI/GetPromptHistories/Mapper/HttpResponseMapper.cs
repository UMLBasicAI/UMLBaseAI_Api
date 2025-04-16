using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using GetSinglePromptHistory.Common;
using GetSinglePromptHistory.Models;
using GetSinglePromptHistory.Presentation;
using GetSinglePromptHistory.Presentation.Filter.SetStageBag;

namespace GetSinglePromptHistory.Mapper;

public static class HttpResponseMapper
{
    private static ConcurrentDictionary<
        Constant.AppCode,
        Func<AppRequestModel, AppResponseModel, HttpContext, Response>
    > _httpResponseMapper;

    private static void Init()
    {
        if (_httpResponseMapper != null) return;

        _httpResponseMapper = new();

        _httpResponseMapper.TryAdd(
            Constant.AppCode.SUCCESS,
            (appRequest, appResponse, httpContext) =>
            {
                return new Response
                {
                    HttpCode = StatusCodes.Status200OK,
                    AppCode = Constant.AppCode.SUCCESS.ToString(),
                    Body = new Response.BodyDto
                    {
                        HistoryId = appResponse.Body?.HistoryId,
                        Messages = appResponse.Body?.Messages,
                        IsHasNextPage = appResponse.Body?.IsHasNextPage ?? false,
                        IsHasPreviousPage = appResponse.Body?.IsHasPreviousPage ?? false
                    }
                };
            }
        );

        _httpResponseMapper.TryAdd(
            Constant.AppCode.VALIDATION_FAILED,
            (appRequest, appResponse, httpContext) =>
            {
                return new Response
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
                return new Response
                {
                    HttpCode = StatusCodes.Status500InternalServerError,
                    AppCode = Constant.AppCode.SERVER_ERROR.ToString()
                };
            }
        );

        _httpResponseMapper.TryAdd(
            Constant.AppCode.UNAUTHORIZED,
            (appRequest, appResponse, httpContext) =>
            {
                return new Response
                {
                    HttpCode = StatusCodes.Status401Unauthorized,
                    AppCode = Constant.AppCode.UNAUTHORIZED.ToString()
                };
            }
        );

        _httpResponseMapper.TryAdd(
            Constant.AppCode.FORBIDDEN,
            (appRequest, appResponse, httpContext) =>
            {
                return new Response
                {
                    HttpCode = StatusCodes.Status403Forbidden,
                    AppCode = Constant.AppCode.FORBIDDEN.ToString()
                };
            }
        );

        _httpResponseMapper.TryAdd(
            Constant.AppCode.HISTORY_NOT_FOUND,
            (appRequest, appResponse, httpContext) =>
            {
                return new Response
                {
                    HttpCode = StatusCodes.Status404NotFound,
                    AppCode = Constant.AppCode.HISTORY_NOT_FOUND.ToString()
                };
            }
        );
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
        if (stageBag != null)
        {
            stageBag.HttpResponse = httpResponse;
        }

        return httpResponse;
    }
}
