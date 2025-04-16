using Microsoft.AspNetCore.Http;

namespace GetPromptHistories.Common;

public static class Constant
{
    public const string CONTROLLER_NAME = "History Endpoints";
    public const string ENDPOINT_PATH = "/histories"; // Đã loại bỏ HistoryId, chỉ cần UserId
    public const string REQUEST_ARGUMENT_NAME = "request";

    public static class DefaultResponse
    {
        public static readonly Models.AppResponseModel VALIDATION_FAILED =
            new() { AppCode = AppCode.VALIDATION_FAILED };

        public static readonly Models.AppResponseModel SERVER_ERROR =
            new() { AppCode = AppCode.SERVER_ERROR };

        public static readonly Models.AppResponseModel UNAUTHORIZED =
            new() { AppCode = AppCode.UNAUTHORIZED };

        public static readonly Models.AppResponseModel FORBIDDEN =
            new() { AppCode = AppCode.FORBIDDEN };

        public static readonly Models.AppResponseModel HISTORIES_NOT_FOUND =
            new() { AppCode = AppCode.HISTORIES_NOT_FOUND };

        public static readonly Models.AppResponseModel SUCCESS =
            new() { AppCode = AppCode.SUCCESS };
    }

    public static class Http
    {
        public static readonly Presentation.Response SUCCESS =
            new() { HttpCode = StatusCodes.Status200OK, AppCode = AppCode.SUCCESS.ToString() };

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

        public static readonly Presentation.Response FORBIDDEN =
            new()
            {
                HttpCode = StatusCodes.Status403Forbidden,
                AppCode = AppCode.FORBIDDEN.ToString(),
            };

        public static readonly Presentation.Response HISTORIES_NOT_FOUND =
            new()
            {
                HttpCode = StatusCodes.Status404NotFound,
                AppCode = AppCode.HISTORIES_NOT_FOUND.ToString(),
            };
    }

    public enum AppCode
    {
        SUCCESS,
        VALIDATION_FAILED,
        SERVER_ERROR,
        UNAUTHORIZED,
        FORBIDDEN,
        HISTORIES_NOT_FOUND // Thêm mã lỗi cho trường hợp không tìm thấy lịch sử
        ,
    }
}
