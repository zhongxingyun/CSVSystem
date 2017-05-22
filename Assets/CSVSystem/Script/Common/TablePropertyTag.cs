using System;
using System.Collections;

namespace UnityEngine
{
    [Serializable]
    public class TablePropertyTag
    {
        public const string TABLENAME = "tableName";
        public const string PRIMARYKEY = "primaryKey";
        public const string KEY = "key";

        public string tableName;
        public string primaryKey;
        public string key;

        public TablePropertyTag() { }

        public TablePropertyTag (string tableName,string primaryKey,string key)
        {
            this.tableName = tableName;
            this.primaryKey = primaryKey;
            this.key = key;
        }
    }
}
