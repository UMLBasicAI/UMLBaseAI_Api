using Microsoft.AspNetCore.Http;

namespace SignUp.Common;

public static class Constant
{
    public const string CONTROLLER_NAME = "Authentication Endpoints";
    public const string ENDPOINT_PATH = "/SignUp";
    public const string REQUEST_ARGUMENT_NAME = "request";

    public static class DefaultResponse
    {
        public static class App
        {
            public static readonly Models.AppResponseModel EMAIL_ALREADY_EXISTS =
                new() { AppCode = AppCode.EMAIL_ALREADY_EXISTS };

            public static readonly Models.AppResponseModel PASSWORD_IS_INVALID =
                new() { AppCode = AppCode.PASSWORD_IS_INVALID };

            public static readonly Models.AppResponseModel VALIDATION_FAILED =
                new() { AppCode = AppCode.VALIDATION_FAILED };

            public static readonly Models.AppResponseModel SERVER_ERROR =
                new() { AppCode = AppCode.SERVER_ERROR };
        }

        public static class Http
        {
            public static readonly Presentation.Response PASSWORD_IS_INVALID =
                new()
                {
                    HttpCode = StatusCodes.Status422UnprocessableEntity,
                    AppCode = AppCode.PASSWORD_IS_INVALID.ToString(),
                };

            public static readonly Presentation.Response EMAIL_ALREADY_EXISTS =
                new()
                {
                    HttpCode = StatusCodes.Status409Conflict,
                    AppCode = AppCode.EMAIL_ALREADY_EXISTS.ToString(),
                };

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
        }
    }

    public enum AppCode
    {
        SUCCESS,
        EMAIL_ALREADY_EXISTS,
        PASSWORD_IS_INVALID,
        VALIDATION_FAILED,
        SERVER_ERROR,
    }
}
