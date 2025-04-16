using Microsoft.AspNetCore.Http;

namespace UpdatePromptTitleNoteById.Common;

public static class Constant
{
    public const string CONTROLLER_NAME = "History Endpoints";
    public const string ENDPOINT_PATH = "/update-prompt-history-action";
    public const string REQUEST_ARGUMENT_NAME = "request";

    public static class DefaultResponse
    {
        public static class App
        {
            public static readonly Models.AppResponseModel VALIDATION_FAILED =
                new() { AppCode = AppCode.VALIDATION_FAILED };

            public static readonly Models.AppResponseModel SERVER_ERROR =
                new() { AppCode = AppCode.SERVER_ERROR };
            public static readonly Models.AppResponseModel UNAUTHORIZED =
                new() { AppCode = AppCode.UNAUTHORIZED };
            public static readonly Models.AppResponseModel NOT_BELONG_TO_USER =
                new() { AppCode = AppCode.NOT_BELONG_TO_USER };
        }

        public static class Http
        {
            public static readonly Presentation.Response VALIDATION_FAILED =
                new()
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    AppCode = AppCode.VALIDATION_FAILED.ToString(),
                };

            public static readonly Presentation.Response SERVER_ERROR =
                new()
                {
                    HttpCode = StatusCodes.Status500InternalServerError,
                    AppCode = AppCode.SERVER_ERROR.ToString(),
                };

            public static readonly Presentation.Response UNAUTHORIZED =
                new()
                {
                    HttpCode = StatusCodes.Status401Unauthorized,
                    AppCode = AppCode.UNAUTHORIZED.ToString(),
                };
            public static readonly Presentation.Response NOT_BELONG_TO_USER =
                new()
                {
                    HttpCode = StatusCodes.Status417ExpectationFailed,
                    AppCode = AppCode.NOT_BELONG_TO_USER.ToString(),
                };
        }
    }

    public enum AppCode
    {
        SUCCESS,
        UNAUTHORIZED,
        VALIDATION_FAILED,
        SERVER_ERROR,
        NOT_BELONG_TO_USER,
    }
}
