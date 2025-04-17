using System.Reflection;
using Base.DataBaseAndIdentity.Entities;
using GetSinglePromptHistory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GetSinglePromptHistory.DataAccess;

public interface IRepository
{
    Task<string> GetLastPlantUmlCode(Guid historyId, CancellationToken cancellationToken);
    Task<bool> IsHistoryOwnedByUser(
        Guid historyId,
        Guid userId,
        CancellationToken cancellationToken
    );

    Task<List<MessageEntity>> getMessagesByHistoryId(
        Guid historyId,
        int page,
        int size,
        CancellationToken cancellationToken
    );
    Task<int> countMessagesByHistoryId(Guid historyId, CancellationToken cancellationToken);
}
