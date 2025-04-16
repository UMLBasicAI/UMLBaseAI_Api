using GetUserInformation.Models;

namespace GetUserInformation.DataAccess;

public interface IRepository
{
    //async - task
    Task<UserInformationModal> etUserInformation(Guid userId);
}
