using System.Security.Claims;
using GetUserInformation.Common;
using GetUserInformation.DataAccess;
using GetUserInformation.Models;
using FCommon.FeatureService;
using Microsoft.AspNetCore.Http;

namespace GetUserInformation.BusinessLogic;

public sealed class Service : IServiceHandler<AppRequestModel, AppResponseModel>
{
    private readonly Lazy<IRepository> _repository;
    private readonly Lazy<IHttpContextAccessor> _httpContextAccessor;

    public Service(
        Lazy<IRepository> repository,
        Lazy<IHttpContextAccessor> httpContextAccessor
    )
    {
        _repository = repository;
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

        var result = await _repository.Value.getUserInformation(Guid.Parse(userId));

        if (result == null || result == default) {
            return new() { AppCode = Constant.AppCode.SERVER_ERROR };
        }

        return new() { 
            AppCode = Constant.AppCode.SUCCESS,
            Body = result
        };
    }
}
