namespace UpdatePromptTitleNoteById.DataAccess;

public interface IRepository
{
    Task<bool> DoesHistoryBelongToUserId(
        Guid userId,
        Guid historyId,
        CancellationToken cancellationToken
    );
    Task<bool> UpdateHistoryNameById(
        Guid historyId,
        string newAction,
        CancellationToken cancellationToken
    );
}
