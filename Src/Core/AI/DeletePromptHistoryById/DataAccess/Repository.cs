using System.Data;
using Base.DataBaseAndIdentity.DBContext;
using Base.DataBaseAndIdentity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DeletePromptHistoryById.Models;

namespace DeletePromptHistoryById.DataAccess;

public sealed class Repository : IRepository
{
    private readonly AppDbContext _appDbContext;

    public Repository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<bool> FindAllMessageAndDeleteByHistoryId(Guid historyId, CancellationToken cancellationToken)
    {
        var result = false;

        var messageIds = await _appDbContext.Set<MessageEntity>().Where(entity => entity.HistoryId == historyId).Select(entity => entity.Id).ToListAsync(cancellationToken);

        if (messageIds.Any()) {
            try
            {
                var res =  await _appDbContext.Set<MessageEntity>().Where(entity => messageIds.Contains(entity.Id)).ExecuteDeleteAsync(cancellationToken);


                result = true;
            }
            catch (Exception) {
                result = false;
            }
        }

        return result;
    }

    public async Task<bool> FindAndDeleteHistoryById(Guid historyId, CancellationToken cancellationToken)
    {
        var result = true;
        try
        {
            await _appDbContext.Set<HistoryEntity>().Where(entity => entity.Id == historyId).ExecuteDeleteAsync(cancellationToken);
        }
        catch (Exception)
        {
            result = false;
        }
        return result;
    }
}
