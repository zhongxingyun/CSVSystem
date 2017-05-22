using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;

public static class AutomaticConfigTable
{
    //Tag池
    static Dictionary<object, Dictionary<string, TablePropertyTag>> pool = new Dictionary<object, Dictionary<string, TablePropertyTag>>();

    /// <summary>
    /// 自动绑定读表
    /// </summary>
    /// <param name="bindObject">执行对象</param>
    public static void Bind(object bindObject)
    {
        DetectionAttribute(bindObject);
        Type type = bindObject.GetType();
        foreach (string fieldName in pool[bindObject].Keys)
        {
            TablePropertyTag tag = pool[bindObject][fieldName];
            if (!string.IsNullOrEmpty(tag.tableName) && !string.IsNullOrEmpty(tag.primaryKey) && !string.IsNullOrEmpty(tag.key))
            {
                string value = MyTable.GetInstance().GetValue(tag.tableName, tag.primaryKey, tag.key);
                FieldInfo info = type.GetField(fieldName);
                if (string.IsNullOrEmpty(value))
                    info.SetValue(bindObject, null);
                else
                    info.SetValue(bindObject, TypeDescriptor.GetConverter(info.FieldType).ConvertFrom(value));
            }
        }
    }

    /// <summary>
    /// 自动根据字段名更新字段Tag
    /// </summary>
    /// <param name="bindObject">执行对象</param>
    /// <param name="tableName">表名</param>
    /// <param name="primaryKey">主键名</param>
    public static void UpdateTagByField(object bindObject, string tableName, string primaryKey, params string[] filterFields)
    {
        foreach (FieldInfo field in bindObject.GetType().GetFields())
        {
            if (filterFields.Length != 0)
            {
                bool isContinue = false;
                foreach (string filterField in filterFields)
                {
                    if (filterField == field.Name)
                    {
                        isContinue = true;
                        break;
                    }
                }
                if (isContinue)
                    continue;
            }
            UpdateTag(bindObject, field.Name, TablePropertyTag.TABLENAME, tableName, TablePropertyTag.PRIMARYKEY, primaryKey, TablePropertyTag.KEY, field.Name);
        }
    }

    /// <summary>
    /// 更新Tag
    /// </summary>
    /// <param name="bindObject">执行对象</param>
    /// <param name="field">更新字段</param>
    /// <param name="parameters">更新参数</param>
    public static void UpdateTag(object bindObject, string field, params string[] parameters)
    {
        if (FieldAttribute(bindObject, field))
        {
            if (!ObjectIsExist(bindObject))
                AddObject(bindObject);
            if (!FieldIsExist(bindObject, field))
                AddField(bindObject, field, new TablePropertyTag());
            lock (pool)
            {
                TablePropertyTag tag = pool[bindObject][field];
                for (int i = 0; i < parameters.Length; i += 2)
                {
                    FieldInfo info = tag.GetType().GetField(parameters[i]);
                    info.SetValue(tag, parameters[i + 1]);
                }
            }
        }
    }

    /// <summary>
    /// 清空池
    /// </summary>
    public static void Clear()
    {
        lock (pool)
        {
            pool.Clear();
        }
    }

    /// <summary>
    /// 检测特性
    /// </summary>
    /// <param name="bindObject">执行对象</param>
    static void DetectionAttribute(object bindObject)
    {
        FieldInfo[] fields = bindObject.GetType().GetFields();
        foreach (FieldInfo field in fields)
        {
            if (!FieldIsExist(bindObject, field.Name))
            {
                foreach (object attribute in field.GetCustomAttributes(true))
                {
                    Type type = attribute.GetType();
                    if (type.Equals(Type.GetType("UnityEngine.AutomaticBindAttribute")))
                    {
                        string tableName = (string)type.GetField("tableName").GetValue(attribute);
                        string primaryKey = (string)type.GetField("primaryKey").GetValue(attribute);
                        string key = (string)type.GetField("key").GetValue(attribute);
                        if (!string.IsNullOrEmpty(tableName) && !string.IsNullOrEmpty(primaryKey) && !string.IsNullOrEmpty(key))
                            UpdateTag(bindObject, field.Name, TablePropertyTag.TABLENAME, tableName, TablePropertyTag.PRIMARYKEY, primaryKey, TablePropertyTag.KEY, key);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 添加对象
    /// </summary>
    /// <param name="bindObject">执行对象</param>
    static void AddObject(object bindObject)
    {
        lock (pool)
            if (!ObjectIsExist(bindObject))
                pool.Add(bindObject, new Dictionary<string, TablePropertyTag>());
    }

    /// <summary>
    /// 添加字段
    /// </summary>
    /// <param name="bindObject">执行对象</param>
    /// <param name="field">字段</param>
    /// <param name="tag">tag</param>
    static void AddField(object bindObject, string field, TablePropertyTag tag)
    {
        lock (pool)
            if (!FieldIsExist(bindObject, field) && ObjectIsExist(bindObject))
                pool[bindObject].Add(field, tag);
    }

    /// <summary>
    /// 检测对象是否存在池中
    /// </summary>
    /// <param name="bindObject">执行对象</param>
    /// <returns></returns>
    static bool ObjectIsExist(object bindObject)
    {
        return pool.ContainsKey(bindObject);
    }

    /// <summary>
    /// 检测字段是否存在
    /// </summary>
    /// <param name="bindObject">执行对象</param>
    /// <param name="field">字段</param>
    /// <returns></returns>
    static bool FieldIsExist(object bindObject, string field)
    {
        if (ObjectIsExist(bindObject))
            return pool[bindObject].ContainsKey(field);
        else
            return false;
    }

    /// <summary>
    /// 检测字段是否有自动配置特性
    /// </summary>
    /// <param name="field">字段</param>
    /// <returns></returns>
    static bool FieldAttribute(object bindObject, string fieldName)
    {
        FieldInfo field = bindObject.GetType().GetField(fieldName);
        foreach (object attribute in field.GetCustomAttributes(true))
        {
            Type type = attribute.GetType();
            if (type.Equals(Type.GetType("UnityEngine.AutomaticBindAttribute")))
                return true;
        }
        return false;
    }
}
