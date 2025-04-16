using System.Security.Claims;
using Base.Mail.Handler;
using DeletePromptHistoryById.Common;
using DeletePromptHistoryById.DataAccess;
using DeletePromptHistoryById.Models;
using FCommon.FeatureService;
using Microsoft.AspNetCore.Http;

namespace DeletePromptHistoryById.BusinessLogic;

public sealed class Service : IServiceHandler<AppRequestModel, AppResponseModel>
{
    private readonly Lazy<IRepository> _repository;
    private readonly Lazy<IEmailSendingHandler> _emailSendingHandler;
    private readonly Lazy<IHttpContextAccessor> _httpContextAccessor;

    public Service(
        Lazy<IRepository> repository,
        Lazy<IEmailSendingHandler> emailSendingHandler,
        Lazy<IHttpContextAccessor> httpContextAccessor
    )
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
            return new() { AppCode = Constant.AppCode.UNAUTHORIZED };
        }

        //step-2: Found And Delete All Message From History With Id

        var deleteMessageBelongToHistoryIdResult =
            await _repository.Value.FindAllMessageAndDeleteByHistoryId(
                Guid.Parse(request.HistoryId),
                cancellationToken
            );

        if (!deleteMessageBelongToHistoryIdResult)
        {
            return new() { AppCode = Constant.AppCode.SERVER_ERROR };
        }

        //step-3: Found And Delete History By Id
        var deleteHistoryByIdResult = await _repository.Value.FindAndDeleteHistoryById(
            Guid.Parse(request.HistoryId),
            cancellationToken
        );

        if (!deleteHistoryByIdResult)
        {
            return new() { AppCode = Constant.AppCode.SERVER_ERROR };
        }

        return new() { AppCode = Constant.AppCode.SUCCESS };
    }
}
