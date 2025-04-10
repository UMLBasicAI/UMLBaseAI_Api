using Base.Mail.Handler;
using FCommon.FeatureService;
using SignUp.Common;
using SignUp.DataAccess;
using SignUp.Models;

namespace SignUp.BusinessLogic;

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
        var isEmailFound = await _repository.Value.DoesEmailExistedAsync(
            request.Email,
            cancellationToken
        );

        if (isEmailFound)
        {
            return Constant.DefaultResponse.App.EMAIL_ALREADY_EXISTS;
        }

        var isPasswordValid = await _repository.Value.IsPasswordValidAsync(
            request.Email,
            request.Password,
            cancellationToken
        );

        if (!isPasswordValid)
        {
            return Constant.DefaultResponse.App.PASSWORD_IS_INVALID;
        }
        var user = CreateNewUser(request);

        var result = await _repository.Value.CreateUserAsync(user, cancellationToken);

        if (!result)
        {
            return Constant.DefaultResponse.App.SERVER_ERROR;
        }

        var testEmail = _emailSendingHandler.Value.SendAsync(
            new()
            {
                To = "vuvo070403@gmail.com",
                Body = "SOME THING HERE!!",
                Subject = "TEST EMAIL SENDER",
            },
            cancellationToken
        );

        return new() { AppCode = Constant.AppCode.SUCCESS };
    }

    private UserInformationModal CreateNewUser(AppRequestModel request)
    {
        return new UserInformationModal
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            Password = request.Password,
            IsEmailConfirmed = true,
            AdditionalUserInfor = new UserInformationModal.AdditionalUserInforModel
            {
                FirstName = string.Empty,
                LastName = string.Empty,
            },
        };
    }
}
