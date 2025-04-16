using Base.DataBaseAndIdentity.Common;

namespace Base.DataBaseAndIdentity.Entities;

public sealed class HistoryEntity : BaseEntity<Guid>
{
    public string Action { get; set; }
    public string PlantUMLCode { get; set; }
    public Guid UserId { get; set; }

    #region Navigations
    public IdentityUserEntity IdentityUser { get; set; }
    public IEnumerable<MessageEntity> Messages { get; set; }
    #endregion

    public static class Metadata
    {
        public static readonly string TableName = "history";

        public static class Properties
        {
            public static class Action
            {
                public const string ColumnName = "action";
                public const bool IsNotNull = true;
                public const short MaxLength = 255;
            }

            public static class PlantUMLCode
            {
                public const string ColumnName = "plant_uml_code";
                public const bool IsNotNull = false;
                public const ushort MaxLength = ushort.MaxValue;
            }

            public static class UserId
            {
                public const string ColumnName = "user_id";
                public const bool IsNotNull = true;
            }
        }
    }
}
