using System.Data;
using Base.DataBaseAndIdentity.DBContext;
using Base.DataBaseAndIdentity.Entities;
using GetSinglePromptHistory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Base.DataBaseAndIdentity.Entities.HistoryEntity.Metadata.Properties;
using static Base.DataBaseAndIdentity.Entities.MessageEntity.Metadata.Properties;

namespace GetSinglePromptHistory.DataAccess;

public sealed class Repository : IRepository
{
    private readonly AppDbContext _dbContext;

    public Repository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<MessageEntity>> getMessagesByHistoryId(
        Guid historyId,
        int page,
        int size,
        CancellationToken cancellationToken
    )
    {
        return await _dbContext
            .Set<MessageEntity>()
            .Where(m => m.HistoryId == historyId)
            .OrderByDescending(m => m.CreatedAt) // hoặc OrderByDescending tùy bạn muốn thứ tự cũ -> mới hay ngược lại
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> countMessagesByHistoryId(
        Guid historyId,
        CancellationToken cancellationToken
    )
    {
        return await _dbContext
            .Set<MessageEntity>()
            .CountAsync(m => m.HistoryId == historyId, cancellationToken);
    }

    public async Task<bool> IsHistoryOwnedByUser(
        Guid historyId,
        Guid userId,
        CancellationToken cancellationToken
    )
    {
        return await _dbContext
            .Set<HistoryEntity>()
            .AnyAsync(h => h.Id == historyId && h.UserId == userId, cancellationToken);
    }

    public async Task<string> GetLastPlantUmlCode(
        Guid historyId,
        CancellationToken cancellationToken
    )
    {
        return await _dbContext
                .Set<HistoryEntity>()
                .Where(h => h.Id == historyId)
                .Select(h => h.PlantUMLCode)
                .FirstOrDefaultAsync(cancellationToken) ?? string.Empty;
    }
}
