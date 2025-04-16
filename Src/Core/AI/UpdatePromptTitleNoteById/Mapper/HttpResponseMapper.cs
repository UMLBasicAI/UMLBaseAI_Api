using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using UpdatePromptTitleNoteById.Common;
using UpdatePromptTitleNoteById.Models;
using UpdatePromptTitleNoteById.Presentation;
using UpdatePromptTitleNoteById.Presentation.Filter.SetStageBag;

namespace UpdatePromptTitleNoteById.Mapper;

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
                    return new() { AppCode = Constant.AppCode.SUCCESS.ToString(), };
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
            _httpResponseMapper.TryAdd(
                Constant.AppCode.NOT_BELONG_TO_USER,
                (appRequest, appResponse, httpContext) =>
                {
                    return Constant.DefaultResponse.Http.NOT_BELONG_TO_USER;
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
