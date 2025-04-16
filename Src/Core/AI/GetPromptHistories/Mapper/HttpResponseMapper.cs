using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using GetPromptHistories.Common;
using GetPromptHistories.Models;
using GetPromptHistories.Presentation;
using GetPromptHistories.Presentation.Filter.SetStageBag;

namespace GetPromptHistories.Mapper;

public static class HttpResponseMapper
{
    private static ConcurrentDictionary<Constant.AppCode, Func<AppRequestModel, AppResponseModel, HttpContext, Response>> _httpResponseMapper;

    private static void Init()
    {
        if (_httpResponseMapper != null) return;

        _httpResponseMapper = new();

        // Mapping for SUCCESS response
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
                        Histories = appResponse.Body?.Histories,
                        IsHasNextPage = appResponse.Body?.IsHasNextPage ?? false,
                        IsHasPreviousPage = appResponse.Body?.IsHasPreviousPage ?? false
                    }
                };
            }
        );

        // Mapping for VALIDATION_FAILED response
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

        // Mapping for SERVER_ERROR response
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

        // Mapping for UNAUTHORIZED response
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

        // Mapping for FORBIDDEN response
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

        // Mapping for HISTORY_NOT_FOUND response
        _httpResponseMapper.TryAdd(
            Constant.AppCode.HISTORIES_NOT_FOUND,
            (appRequest, appResponse, httpContext) =>
            {
                return new Response
                {
                    HttpCode = StatusCodes.Status404NotFound,
                    AppCode = Constant.AppCode.HISTORIES_NOT_FOUND.ToString()
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
