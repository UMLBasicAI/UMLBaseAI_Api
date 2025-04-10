using Microsoft.AspNetCore.Identity;

namespace Base.DataBaseAndIdentity.Entities;

public sealed class IdentityUserEntity : IdentityUser<Guid>
{
    #region Navigations
    public AdditionalUserInformationEntity AdditionUserInformation { get; set; }
    public IEnumerable<HistoryEntity> Histories { get; set; }
    #endregion

    public static class Metadata
    {
        public const string TableName = "user";
    }
}
