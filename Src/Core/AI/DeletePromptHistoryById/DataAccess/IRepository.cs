using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeletePromptHistoryById.Models;

namespace DeletePromptHistoryById.DataAccess;

public interface IRepository
{
    //async - task
    Task<bool> FindAllMessageAndDeleteByHistoryId(Guid historyId, CancellationToken cancellationToken);

    Task<bool> FindAndDeleteHistoryById(Guid historyId, CancellationToken cancellationToken);
}
