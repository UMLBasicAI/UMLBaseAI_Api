using GetUserInformation.Models;

namespace GetUserInformation.DataAccess;

public interface IRepository
{
    //async - task
    Task<UserInformationModal?> getUserInformation(Guid userId);
}
