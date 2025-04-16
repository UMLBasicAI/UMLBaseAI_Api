using System.Data;
using Base.DataBaseAndIdentity.DBContext;
using Base.DataBaseAndIdentity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UpdatePromptTitleNoteById.Models;

namespace UpdatePromptTitleNoteById.DataAccess;

public sealed class Repository : IRepository
{
    private readonly AppDbContext _appDbContext;

    public Repository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<bool> DoesHistoryBelongToUserId(
        Guid userId,
        Guid historyId,
        CancellationToken cancellationToken
    )
    {
        return await _appDbContext
            .Set<HistoryEntity>()
            .AnyAsync(entity => entity.Id.Equals(historyId) && entity.UserId.Equals(userId));
    }

    public async Task<bool> UpdateHistoryNameById(
        Guid historyId,
        string newAction,
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
                    cancellationToken
                );

                try
                {
                    var rowsAffected = await _appDbContext
                        .Set<HistoryEntity>()
                        .Where(history => history.Id.Equals(historyId))
                        .ExecuteUpdateAsync(setProps =>
                            setProps.SetProperty(entity => entity.Action, newAction)
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
