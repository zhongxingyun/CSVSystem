namespace System
{
    [AttributeUsage(AttributeTargets.Field)]
    public class AutomaticBindTable : Attribute
    {
        public string tableName;
        public string primaryKey;
        public string key;

        public AutomaticBindTable(string tableName, string primaryKey, string key)
        {
            this.tableName = tableName;
            this.primaryKey = primaryKey;
            this.key = key;
        }
    }
}
