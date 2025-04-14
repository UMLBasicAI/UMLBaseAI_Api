using FCommon.AccessToken;
using FCommon.Constants;
using FCommon.FeatureService;
using FCommon.RefreshToken;
using RefreshAccessToken.Common;
using RefreshAccessToken.DataAccess;
using RefreshAccessToken.Models;

namespace RefreshAccessToken.BusinessLogic;

public sealed class Service : IServiceHandler<AppRequestModel, AppResponseModel>
{
    private readonly Lazy<IRepository> _repository;
    private readonly Lazy<IAppRefreshTokenHandler> _appRefreshTokenHandler;
    private readonly Lazy<IAppAccessTokenHandler> _appAccessTokenHandler;

    public Service(
        Lazy<IRepository> repository,
        Lazy<IAppRefreshTokenHandler> appRefreshTokenHandler,
        Lazy<IAppAccessTokenHandler> appAccessTokenHandler
    )
    {
        _repository = repository;
        _appRefreshTokenHandler = appRefreshTokenHandler;
        _appAccessTokenHandler = appAccessTokenHandler;
    }

    public async Task<AppResponseModel> ExecuteAsync(
        AppRequestModel request,
        CancellationToken cancellationToken
    )
    {
        var foundRefreshToken =
            await _repository.Value.FoundRefreshTokenBelongToAccessTokenJtiAsync(
                accessTokenId: request.AccessTokenId,
                refreshTokenValue: request.RefreshToken,
                userId: new Guid(request.UserId),
                cancellationToken
            );

        if (foundRefreshToken is null)
        {
            return new() { AppCode = Constant.AppCode.REFRESH_TOKEN_IS_NOT_FOUND };
        }

        if (foundRefreshToken.ExpiredAt < DateTime.UtcNow)
        {
            return new() { AppCode = Constant.AppCode.REFRESH_TOKEN_IS_EXPIRED };
        }

        var newTokenId = Guid.NewGuid().ToString();

        var updateRefreshtoken = new UpdateRefreshTokenModel()
        {
            CurrentId = request.AccessTokenId,
            NewId = newTokenId,
            NewValue = _appRefreshTokenHandler.Value.GenerateRefreshToken(),
        };

        var newAccessToken = _appAccessTokenHandler.Value.GenerateJWT(
            [
                new(AppContant.JsonWebToken.ClaimType.JTI, newTokenId.ToString()),
                new(AppContant.JsonWebToken.ClaimType.SUB, request.UserId.ToString()),
                new(
                    AppContant.JsonWebToken.ClaimType.PURPOSE.Name,
                    AppContant.JsonWebToken.ClaimType.PURPOSE.Value.USER_IN_APP
                ),
            ],
            Constant.APP_USER_ACCESS_TOKEN.DURATION_IN_MINUTES
        );

        var dbResult = await _repository.Value.UpdateRefreshTokenAsync(
            updateRefreshtoken,
            cancellationToken
        );

        if (!dbResult)
        {
            return new() { AppCode = Constant.AppCode.SERVER_ERROR };
        }
        return new()
        {
            AppCode = Constant.AppCode.SUCCESS,
            Body = new()
            {
                AccessToken = newAccessToken,
                RefreshToken = updateRefreshtoken.NewValue,
            },
        };
    }
}
