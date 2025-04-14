using System.Data;
using Base.DataBaseAndIdentity.DBContext;
using Base.DataBaseAndIdentity.Entities;
using Microsoft.EntityFrameworkCore;
using RefreshAccessToken.Models;

namespace RefreshAccessToken.DataAccess;

public sealed class Repository : IRepository
{
    private readonly AppDbContext _appDbContext;

    public Repository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<RefreshTokenModel> FoundRefreshTokenBelongToAccessTokenJtiAsync(
        string accessTokenId,
        string refreshTokenValue,
        Guid userId,
        CancellationToken cancellationToken
    )
    {
        var foundToken = await _appDbContext
            .Set<IdentityUserTokenEntity>()
            .AsNoTracking()
            .Where(token =>
                token.LoginProvider.Equals(accessTokenId)
                && token.Value.Equals(refreshTokenValue)
                && token.UserId.Equals(userId)
            )
            .Select(token => new IdentityUserTokenEntity { ExpireAt = token.ExpireAt })
            .FirstOrDefaultAsync();

        if (foundToken is null)
            return null;

        var refreshTokenModel = new RefreshTokenModel { ExpiredAt = foundToken.ExpireAt };
        return refreshTokenModel;
    }

    public async Task<bool> UpdateRefreshTokenAsync(
        UpdateRefreshTokenModel updateRefreshTokenModel,
        CancellationToken cancellationToken
    )
    {
        var dbResult = true;

        await _appDbContext
            .Database.CreateExecutionStrategy()
            .ExecuteAsync(async () =>
            {
                await using var dbTransaction = await _appDbContext.Database.BeginTransactionAsync(
                    IsolationLevel.ReadCommitted,
                    cancellationToken: cancellationToken
                );

                try
                {
                    var rowsAffected = await _appDbContext
                        .Set<IdentityUserTokenEntity>()
                        .Where(token =>
                            token.LoginProvider.Equals(updateRefreshTokenModel.CurrentId)
                        )
                        .ExecuteUpdateAsync(setProps =>
                            setProps
                                .SetProperty(
                                    entity => entity.LoginProvider,
                                    updateRefreshTokenModel.NewId
                                )
                                .SetProperty(
                                    entity => entity.Value,
                                    updateRefreshTokenModel.NewValue
                                )
                        );

                    if (rowsAffected == 0)
                    {
                        throw new DbUpdateException();
                    }

                    await dbTransaction.CommitAsync();
                }
                catch (DbUpdateException)
                {
                    await dbTransaction.RollbackAsync();
                    dbResult = false;
                }
            });
        return dbResult;
    }
}
