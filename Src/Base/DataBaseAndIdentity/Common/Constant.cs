namespace Base.DataBaseAndIdentity.Common;

internal static class Constant
{
    public const string DatabaseSchema = "uml_base_ai";

    public static class Collation
    {
        public const string CASE_INSENSITIVE = "case_insensitive";
    }

    public static class DatabaseType
    {
        public const string VARCHAR = "VARCHAR";

        public const string TEXT = "TEXT";

        public const string LONG = "BIGINT";

        public const string TIMESTAMPZ = "TIMESTAMP WITH TIME ZONE";
    }
}
