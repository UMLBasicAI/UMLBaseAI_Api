using Base.Mail.Handler;
using FCommon.FeatureService;
using DeletePromptHistoryById.Common;
using DeletePromptHistoryById.DataAccess;
using DeletePromptHistoryById.Models;

namespace DeletePromptHistoryById.BusinessLogic;

public sealed class Service : IServiceHandler<AppRequestModel, AppResponseModel>
{
    private readonly Lazy<IRepository> _repository;
    private readonly Lazy<IEmailSendingHandler> _emailSendingHandler;

    public Service(Lazy<IRepository> repository, Lazy<IEmailSendingHandler> emailSendingHandler)
    {
        _repository = repository;
        _emailSendingHandler = emailSendingHandler;
    }

    public async Task<AppResponseModel> ExecuteAsync(
        AppRequestModel request,
        CancellationToken cancellationToken
    )
    {
        return new();
    } 
}
