using System.Security.Claims;
using Base.Mail.Handler;
using FCommon.FeatureService;
using Microsoft.AspNetCore.Http;
using UpdatePromptTitleNoteById.Common;
using UpdatePromptTitleNoteById.DataAccess;
using UpdatePromptTitleNoteById.Models;

namespace UpdatePromptTitleNoteById.BusinessLogic;

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
        var userId = _httpContextAccessor.Value.HttpContext.User.FindFirstValue(claimType: "sub");

        var checkHistoryBelongToUser = await _repository.Value.DoesHistoryBelongToUserId(
            Guid.Parse(userId),
            Guid.Parse(request.HistoryId),
            cancellationToken
        );

        if (!checkHistoryBelongToUser)
        {
            return new() { AppCode = Constant.AppCode.NOT_BELONG_TO_USER };
        }

        var dbResult = await _repository.Value.UpdateHistoryNameById(
            Guid.Parse(request.HistoryId),
            request.NewAction,
            cancellationToken
        );

        if (!dbResult)
        {
            return new() { AppCode = Constant.AppCode.SERVER_ERROR, };
        }

        return new() { AppCode = Constant.AppCode.SUCCESS };
    }
}
