using Microsoft.AspNetCore.Http;

namespace SignIn.Common;

public static class Constant
{
    public const string CONTROLLER_NAME = "Authentication Endpoints";
    public const string ENDPOINT_PATH = "Login";
    public const string REQUEST_ARGUMENT_NAME = "request";

    public static class APP_USER_REFRESH_TOKEN
    {
        public const string NAME = "AppUserRefreshToken";

        public static class DURATION_IN_MINUTES
        {
            public const int REMEMBER_ME = 60 * 24 * 365;

            public const int NOT_REMEMBER_ME = 60 * 24 * 7;
        }
    }

    public static class APP_USER_ACCESS_TOKEN
    {
        public const int DURATION_IN_MINUTES = 60;
    }

    public static class DefaultResponse
    {
        public static class App
        {
            public static readonly Models.AppResponseModel USER_NOT_FOUND =
                new() { AppCode = AppCode.USER_NOT_FOUND };
            public static readonly Models.AppResponseModel EMAIL_NOT_VERIFIED =
                new() { AppCode = AppCode.EMAIL_NOT_VERIFIED };

            public static readonly Models.AppResponseModel PASSWORD_IS_INVALID =
                new() { AppCode = AppCode.PASSWORD_IS_INVALID };

            public static readonly Models.AppResponseModel VALIDATION_FAILED =
                new() { AppCode = AppCode.VALIDATION_FAILED };

            public static readonly Models.AppResponseModel SERVER_ERROR =
                new() { AppCode = AppCode.SERVER_ERROR };

            public static readonly Models.AppResponseModel TEMPORARY_BANNED =
                new() { AppCode = AppCode.TEMPORARY_BANNED };
            public static readonly Models.AppResponseModel PASSWORD_IS_INCORRECT =
                new() { AppCode = AppCode.PASSWORD_IS_INCORRECT };
        }

        public static class Http
        {
            public static readonly Presentation.Response PASSWORD_IS_INVALID =
                new()
                {
                    HttpCode = StatusCodes.Status422UnprocessableEntity,
                    AppCode = AppCode.PASSWORD_IS_INVALID.ToString(),
                };

            public static readonly Presentation.Response USER_NOT_FOUND =
                new()
                {
                    HttpCode = StatusCodes.Status409Conflict,
                    AppCode = AppCode.USER_NOT_FOUND.ToString(),
                };

            public static readonly Presentation.Response EMAIL_NOT_VERIFIED =
                new()
                {
                    HttpCode = StatusCodes.Status409Conflict,
                    AppCode = AppCode.EMAIL_NOT_VERIFIED.ToString(),
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

            public static readonly Presentation.Response TEMPORARY_BANNED =
                new()
                {
                    HttpCode = StatusCodes.Status500InternalServerError,
                    AppCode = AppCode.SERVER_ERROR.ToString(),
                };

            public static readonly Presentation.Response PASSWORD_IS_INCORRECT =
                new()
                {
                    HttpCode = StatusCodes.Status401Unauthorized,
                    AppCode = AppCode.PASSWORD_IS_INCORRECT.ToString(),
                };
        }
    }

    public enum AppCode
    {
        SUCCESS,
        USER_NOT_FOUND,
        EMAIL_NOT_VERIFIED,
        PASSWORD_IS_INVALID,
        VALIDATION_FAILED,
        SERVER_ERROR,
        TEMPORARY_BANNED,
        PASSWORD_IS_INCORRECT,
    }
}
