using Base.Mail.Handler;
using FCommon.FeatureService;
using UpdatePromptTitleNoteById.Common;
using UpdatePromptTitleNoteById.DataAccess;
using UpdatePromptTitleNoteById.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace UpdatePromptTitleNoteById.BusinessLogic;

public sealed class Service : IServiceHandler<AppRequestModel, AppResponseModel>
{
    private readonly Lazy<IRepository> _repository;
    private readonly Lazy<IEmailSendingHandler> _emailSendingHandler;
    private readonly Lazy<IHttpContextAccessor> _httpContextAccessor;

    public Service(Lazy<IRepository> repository, Lazy<IEmailSendingHandler> emailSendingHandler, Lazy<IHttpContextAccessor> httpContextAccessor)
    {
        _repository = repository;
        _emailSendingHandler = emailSendingHandler;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<AppResponseModel> ExecuteAsync(
        AppRequestModel request,
        CancellationToken cancellationToken
    )
    {
        //step-1: Found User Id From Http Context

        var userId = _httpContextAccessor.Value.HttpContext.User.FindFirstValue(claimType: "sub");

        if (userId == null)
        {
            return new() { AppCode =  Constant.AppCode.UNAUTHORIZED};
        }

        //step-2: Found And Update Prompt Title/Note From History With Id

        var deleteMessageBelongToHistoryIdResult = await _repository.Value.FindAllMessageAndDeleteByHistoryId(Guid.Parse(request.HistoryId), cancellationToken);

        if (!deleteMessageBelongToHistoryIdResult)
        {
            return new() { AppCode = Constant.AppCode.SERVER_ERROR };
        }

        //step-3:Found Update Prompt Title/Note History By Id
        var deleteHistoryByIdResult = await _repository.Value.FindAndDeleteHistoryById(Guid.Parse(request.HistoryId), cancellationToken);

        if (!deleteHistoryByIdResult)
        {
            return new() { AppCode = Constant.AppCode.SERVER_ERROR };
        }

        return new() { AppCode = Constant.AppCode.SUCCESS };

    }
}
