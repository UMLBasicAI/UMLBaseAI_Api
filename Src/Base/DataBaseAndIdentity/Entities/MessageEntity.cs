using Base.DataBaseAndIdentity.Common;

namespace Base.DataBaseAndIdentity.Entities;

public sealed class MessageEntity : BaseEntity<Guid>
{
    public string Content { get; set; }
    public string MessageType { get; set; }
    public string SentAt { get; set; }

    public Guid HistoryId { get; set; }

    #region Navigations
    public HistoryEntity History { get; set; }
    #endregion

    public static class Metadata
    {
        public static readonly string TableName = "message";

        public static class Properties
        {
            public static class Content
            {
                public const string ColumnName = "content";
                public const bool IsNotNull = true;
                public const short MaxLength = 255;
            }

            public static class MessageType
            {
                public const string ColumnName = "message_type";
                public const bool IsNotNull = true;
                public const short MaxLength = 255;
            }

            public static class SentAt
            {
                public const string ColumnName = "sent_at";
                public const bool IsNotNull = true;
            }

            public static class HistoryId
            {
                public const string ColumnName = "history_id";
                public const bool IsNotNull = true;
            }
        }
    }
}
