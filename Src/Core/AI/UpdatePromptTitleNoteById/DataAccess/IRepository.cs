namespace UpdatePromptTitleNoteById.DataAccess;

public interface IRepository
{
    //async - task
    Task<bool> FindAllMessageAndDeleteByHistoryId(Guid historyId, CancellationToken cancellationToken);

    Task<bool> FindAndDeleteHistoryById(Guid historyId, CancellationToken cancellationToken);
}
