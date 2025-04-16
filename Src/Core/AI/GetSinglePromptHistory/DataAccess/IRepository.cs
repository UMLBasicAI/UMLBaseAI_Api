using Base.DataBaseAndIdentity.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GetSinglePromptHistory.Models;
using System.Reflection;

namespace GetSinglePromptHistory.DataAccess;

public interface IRepository
{
    Task<bool> IsHistoryOwnedByUser(Guid historyId, Guid userId, CancellationToken cancellationToken);

    Task<List<MessageEntity>> getMessagesByHistoryId(Guid historyId, int page, int size, CancellationToken cancellationToken);
    Task<int> countMessagesByHistoryId(Guid historyId, CancellationToken cancellationToken);
}
