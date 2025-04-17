using System.Security.Claims;
using Base.Gemini.Handler;
using FCommon.AccessToken;
using FCommon.Constants;
using FCommon.FeatureService;
using FCommon.RefreshToken;
using GetSinglePromptHistory.Common;
using GetSinglePromptHistory.DataAccess;
using GetSinglePromptHistory.Models;
using Microsoft.AspNetCore.Http;

namespace GetSinglePromptHistory.BusinessLogic;

public sealed class Service : IServiceHandler<AppRequestModel, AppResponseModel>
{
    private readonly Lazy<IRepository> _repository;
    private readonly Lazy<IHttpContextAccessor> _httpContextAccessor;

    public Service(Lazy<IRepository> repository, Lazy<IHttpContextAccessor> httpContextAccessor)
    {
        _repository = repository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<AppResponseModel> ExecuteAsync(
        AppRequestModel request,
        CancellationToken cancellationToken
    )
    {
        if (string.IsNullOrWhiteSpace(request.HistoryId) || request.Page <= 0 || request.Size <= 0)
        {
            return new AppResponseModel { AppCode = Constant.AppCode.VALIDATION_FAILED };
        }

        var userId = _httpContextAccessor.Value.HttpContext.User.FindFirstValue("sub");
        if (string.IsNullOrWhiteSpace(userId))
        {
            return new AppResponseModel { AppCode = Constant.AppCode.UNAUTHORIZED };
        }

        var historyId = Guid.Parse(request.HistoryId);
        var userGuid = Guid.Parse(userId);

        // Kiểm tra xem lịch sử có tồn tại và thuộc về user không
        var isOwnedByUser = await _repository.Value.IsHistoryOwnedByUser(
            historyId,
            userGuid,
            cancellationToken
        );
        if (!isOwnedByUser)
        {
            return new AppResponseModel { AppCode = Constant.AppCode.UNAUTHORIZED };
        }

        var page = request.Page;
        var size = request.Size;

        var messages = await _repository.Value.getMessagesByHistoryId(
            historyId,
            page,
            size,
            cancellationToken
        );
        var plantUmlCode = await _repository.Value.GetLastPlantUmlCode(
            historyId,
            cancellationToken
        );
        var totalCount = await _repository.Value.countMessagesByHistoryId(
            historyId,
            cancellationToken
        );
        var totalPages = (int)Math.Ceiling((double)totalCount / size);

        return new AppResponseModel
        {
            AppCode = Constant.AppCode.SUCCESS,
            Body = new AppResponseModel.BodyModel
            {
                HistoryId = request.HistoryId,
                Messages = messages,
                LastPlantUmlCode = plantUmlCode,
                IsHasNextPage = page < totalPages,
                IsHasPreviousPage = page > 1,
            },
        };
    }
}
