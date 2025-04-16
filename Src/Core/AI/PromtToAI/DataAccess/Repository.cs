using System.Data;
using Base.DataBaseAndIdentity.DBContext;
using Base.DataBaseAndIdentity.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PromptToAI.Models;
using static Base.DataBaseAndIdentity.Entities.HistoryEntity.Metadata.Properties;
using static Base.DataBaseAndIdentity.Entities.MessageEntity.Metadata.Properties;

namespace PromptToAI.DataAccess;

public sealed class Repository : IRepository
{
    private readonly AppDbContext _appDbContext;

    public Repository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<Guid> createAnHistory(string action, string? plantUml, Guid userId, CancellationToken c)
    {
        var user = await _appDbContext.Set<IdentityUserEntity>().FindAsync(new object[] { userId }, c);

        if (user == null)
            throw new Exception("User not found.");

        var result = await _appDbContext.Set<HistoryEntity>().AddAsync(new HistoryEntity()
        {
            Id = Guid.NewGuid(),
            Action = action,
            PlantUMLCode = plantUml,
            UserId = userId,
        }, c);
        await _appDbContext.SaveChangesAsync(c);
        return result.Entity.Id;
    }

    public async Task<bool> saveMessageToHistory(Guid historyId, string content, string type, CancellationToken c)
    {
        await _appDbContext.Set<MessageEntity>().AddAsync(new MessageEntity
        {
            Id = Guid.NewGuid(),
            HistoryId = historyId,
            Content = content,
            MessageType = type,
            SentAt = DateTime.UtcNow.ToString(),
            CreatedAt = DateTime.UtcNow
        }, c);

        await _appDbContext.SaveChangesAsync(c);

        return true;
    }

    public async Task<bool> updateHistory(string? plantUml, Guid historyId, CancellationToken c)
    {
        var history = await _appDbContext.Set<HistoryEntity>()
        .FirstOrDefaultAsync(h => h.Id == historyId, c);

        if (history == null)
        {
            return false;
        }

        history.PlantUMLCode = plantUml;

        await _appDbContext.SaveChangesAsync(c);
        return true;
    }

}
