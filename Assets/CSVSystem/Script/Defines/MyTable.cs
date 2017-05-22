using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyTable
{
    static MyTable instance;
    // Dictionary<表名, Dictionary<主键, Dictionary<表字段, 数据值>>>
    Dictionary<string, Dictionary<string, Dictionary<string, string>>> Tables;

    public static MyTable GetInstance()
    {
        if (instance == null)
            instance = new MyTable();
        return instance;
    }

    MyTable()
    {
        initialize();
    }

    /// <summary>
    /// 获取表
    /// </summary>
    /// <param name="tableName">表名</param>
    /// <returns></returns>
    public Dictionary<string, Dictionary<string, string>> GetTable(string tableName)
    {
        //检索表是否存在
        if (Tables.ContainsKey(tableName))
            return Tables[tableName];
        else
        {
            Debug.LogWarning(tableName + "表不存在！");
            return null;
        }
    }

    /// <summary>
    /// 获取行
    /// </summary>
    /// <param name="tableName">表名</param>
    /// <param name="primaryKey">主键名</param>
    /// <returns></returns>
    public Dictionary<string,string> GetRow(string tableName, string primaryKey)
    {
        Dictionary<string, Dictionary<string, string>> table;
        //检索表是否存在
        if ((table = GetTable(tableName)) == null)
        {
            Debug.LogWarning(tableName + "表不存在！");
            return null;
        }
        //检索主键是否存在
        if (table.ContainsKey(primaryKey))
            return table[primaryKey];
        else
        {
            Debug.LogWarning(primaryKey + "主键不存在！");
            return null;
        }
    }

    /// <summary>
    /// 随机获取行
    /// </summary>
    /// <param name="tableName">表名</param>
    /// <returns></returns>
    public Dictionary<string,string> GetRandomRow(string tableName)
    {
        string primaryKey = "";
        foreach (string key in GetTable(tableName).Keys)
        {
            foreach (string keyName in GetRow(tableName,key).Keys)
            {
                primaryKey = keyName;
                break;
            }
            break;
        }
        List<string> primaryKeys = GetCols(tableName, primaryKey);
        string randomPrimaryKey = primaryKeys[Random.Range(0, primaryKeys.Count)];
        Dictionary<string, string> randomRow = GetRow(tableName, randomPrimaryKey);
        return randomRow;
    }

    /// <summary>
    /// 获取多行
    /// </summary>
    /// <param name="tableName">表名</param>
    /// <param name="keyColsName">标识字段名</param>
    /// <param name="colsName">条件字段名</param>
    /// <param name="colsValue">条件字段值</param>
    /// <returns></returns>
    public Dictionary<string,Dictionary<string,string>> GetRows(string tableName,string keyColsName,string[] colsName,string[] colsValue)
    {
        Dictionary<string, Dictionary<string, string>> table = GetTable(tableName);
        Dictionary<string, Dictionary<string, string>> rows = new Dictionary<string, Dictionary<string, string>>();
        foreach (Dictionary<string, string> row in table.Values)
        {
            bool isPass = true;
            for (int i = 0; i < colsName.Length; i++)
            {
                if (row[colsName[i]] != colsValue[i])
                {
                    isPass = false;
                    break;
                }
            }
            if (isPass)
                rows.Add(row[keyColsName], row);
        }
        return rows;
    }

    /// <summary>
    /// 获取列
    /// </summary>
    /// <param name="tableName">表名</param>
    /// <param name="colsName">字段名</param>
    /// <returns></returns>
    public List<string> GetCols(string tableName,string colsName)
    {
        List<string> cols = new List<string>();
        Dictionary<string, Dictionary<string, string>> table;
        //检索表是否存在
        if (Tables.ContainsKey(tableName))
            table = Tables[tableName];
        else
        {
            Debug.LogWarning(tableName + "表不存在！");
            return null;
        }
        foreach (Dictionary<string, string> row in table.Values)
        {
            if(!row.ContainsKey(colsName))
            {
                Debug.LogWarning(colsName + "表字段不存在！");
                return null;
            }
            cols.Add(row[colsName]);
        }
        return cols;
    }

    /// <summary>
    /// 获取值
    /// </summary>
    /// <param name="tableName">表名</param>
    /// <param name="primaryKey">主键</param>
    /// <param name="colsName">字段名</param>
    /// <returns></returns>
    public string GetValue(string tableName, string primaryKey, string colsName)
    {
        Dictionary<string, string> row;
        //检索主键是否存在
        if((row = GetRow(tableName,primaryKey)) == null)
        {
            Debug.LogWarning(primaryKey + "主键不存在！");
            return null;
        }
        //检索数据是否存在
        if (row.ContainsKey(colsName))
            return row[colsName];
        else
        {
            Debug.LogWarning(colsName + "表字段不存在！");
            return null;
        }
    }

    /// <summary>
    /// 初始化读取表数据
    /// </summary>
    void initialize()
    {
        Tables = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();
        TextAsset[] tables = Resources.LoadAll<TextAsset>("Tables");
        foreach (TextAsset table in tables)
        {
            Tables.Add(table.name, ReadTable(table));
        }
    }

    /// <summary>
    /// 读取表数据,进行解析
    /// </summary>
    /// <param name="table">表文件</param>
    /// <returns>解析后的数据</returns>
    Dictionary<string, Dictionary<string, string>> ReadTable(TextAsset table)
    {
        //表字符串分割为每行字符串
        string[] bytes = table.text.Split(new string[] { "\r\n" }, System.StringSplitOptions.None);
        //拆分完的表数据
        Dictionary<string, Dictionary<string,string>> data = new Dictionary<string, Dictionary<string, string>>();
        //表字段
        string[] colsName = bytes[1].Split(',');
        for (int i = 2; i < bytes.Length - 1; i++)
        {
            //每行字符串分割为数据组
            string[] row = bytes[i].Split(',');
            //将每个数据和字段关联
            Dictionary<string, string> value = new Dictionary<string, string>();
            for (int j = 0; j < row.Length; j++)
            {
                value.Add(colsName[j], row[j]);
            }
            //将每行数据和主键关联
            data.Add(row[0], value);
        }
        return data;
    }
}
