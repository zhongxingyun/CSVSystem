using System;

namespace UnityEngine
{
    [AttributeUsage(AttributeTargets.Field)]
    public class AutomaticBindAttribute : Attribute
    {
        public string tableName;
        public string primaryKey;
        public string key;

        public AutomaticBindAttribute(string tableName, string primaryKey, string key)
        {
            this.tableName = tableName;
            this.primaryKey = primaryKey;
            this.key = key;
        }

        public AutomaticBindAttribute() { }
    }
}
