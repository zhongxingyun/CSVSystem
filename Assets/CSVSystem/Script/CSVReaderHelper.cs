using UnityEngine;
using System.Collections;
using System.ComponentModel;
using System;

namespace CSVSystem
{
    public static class CSVReaderHelper
    {
        /// <summary>
        /// 将获取到的字符串转换成指定类型赋给变量
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="reader">reader</param>
        /// <param name="primaryKey">primaryKey</param>
        /// <param name="keyName">keyName</param>
        /// <returns></returns>
        public static void SetValue<T>(this CSVReader reader, ref T _variable, string primaryKey, string keyName)
        {
            string value = reader.GetValueAtFirst(primaryKey, keyName);
            try
            {
                _variable = (T)Convert.ChangeType(value, typeof(T));
            }
            catch (InvalidCastException)
            {
                _variable = default(T);
            }
            catch (FormatException)
            {
                _variable = default(T);
            }
        }

        /// <summary>
        /// 将行获取到的字符串数组转换成指定类型赋给数组
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="reader">reader</param>
        /// <param name="primaryKey">primaryKey</param>
        /// <returns></returns>
        public static void SetRow<T>(this CSVReader reader, ref T[] _array, string primaryKey)
        {
            _array = Array.ConvertAll(reader.GetRowAtFirst(primaryKey), value =>
            {
                try
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }
                catch (InvalidCastException)
                {
                    return default(T);
                }
                catch (FormatException)
                {
                    return default(T);
                }
            });
        }

        /// <summary>
        /// 将列获取到的字符串数组转换成指定类型赋给数组
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="reader">reader</param>
        /// <param name="primaryKey">primaryKey</param>
        /// <returns></returns>
        public static void SetColumn<T>(this CSVReader reader, ref T[] _array, string primaryKey)
        {
            _array = Array.ConvertAll(reader.GetColumnAtFirst(primaryKey), value =>
            {
                try
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }
                catch (InvalidCastException)
                {
                    return default(T);
                }
                catch (FormatException)
                {
                    return default(T);
                }
            });
        }
    }
}
