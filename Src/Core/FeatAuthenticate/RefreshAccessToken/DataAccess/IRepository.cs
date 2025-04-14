using RefreshAccessToken.Models;

namespace RefreshAccessToken.DataAccess;

public interface IRepository
{
    Task<RefreshTokenModel> FoundRefreshTokenBelongToAccessTokenJtiAsync(
        string accessTokenId,
        string refreshTokenValue,
        Guid userId,
        CancellationToken cancellationToken
    );

    Task<bool> UpdateRefreshTokenAsync(
        UpdateRefreshTokenModel refreshTokenModel,
        CancellationToken cancellationToken
    );
}
