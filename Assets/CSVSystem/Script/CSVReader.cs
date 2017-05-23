using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSVSystem
{
    public class CSVReader
    {
        /// <summary>
        /// 数据网格
        /// </summary>
        private string[,] csvGrid;

        /// <summary>
        /// 主键列表
        /// </summary>
        private string[] primaryKeyList;

        /// <summary>
        /// 键名列表
        /// </summary>
        private string[] keyNameList;

        /// <summary>
        /// 全文本读取构造函数
        /// </summary>
        public CSVReader(string csvText)
        {
            csvGrid = SplitCsvGrid(csvText);
        }

        /// <summary>
        /// 部分文本读取构造函数
        /// </summary>
        public CSVReader(string csvText, int firstRemoveRowNum, int lastRemoveRowNum)
        {
            csvGrid = SplitCsvGrid(csvText, firstRemoveRowNum, lastRemoveRowNum);
        }

        /// <summary>
        /// 将CSV文本分割
        /// </summary>
        /// <param name="csvText">csvText</param>
        /// <param name="firstIgnoreRowNum">首部移除文本行数</param>
        /// <param name="lastIgnoreRowNum">尾部移除文本行数</param>
        /// <returns></returns>
        private string[,] SplitCsvGrid(string csvText, int firstRemoveRowNum = 0, int lastRemoveRowNum = 0)
        {
            string[] row = csvText.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            //移除文本首尾行
            int totalRemoveRowNum = firstRemoveRowNum + lastRemoveRowNum;
            if (totalRemoveRowNum > 0)
            {
                if (totalRemoveRowNum < row.Length)
                {
                    string[] _row = new string[row.Length - totalRemoveRowNum];
                    Array.Copy(row, firstRemoveRowNum, _row, 0, _row.Length);
                    row = _row;
                }
                else
                {
                    Debug.LogWarning("CSVReader 分割文本异常:移除文本总行数大于文本总行数,移除无效");
                }
            }

            //分割文本
            int columnWidth = 0;
            for (int i = 0; i < row.Length; i++)
            {
                string[] column = SplitCsvRow(row[i]);
                if (columnWidth < column.Length)
                    columnWidth = column.Length;
            }

            string[,] outputGird = new string[row.Length, columnWidth];
            for (int y = 0; y < row.Length; y++)
            {
                string[] column = SplitCsvRow(row[y]);
                for (int x = 0; x < column.Length; x++)
                {
                    outputGird[y, x] = column[x];
                    outputGird[y, x] = outputGird[y, x].Replace("\"\"", "\"");
                }
            }

            //添加主键和键名列表,默认添加第二行键名
            primaryKeyList = new string[row.Length];
            keyNameList = new string[columnWidth];
            for (int y = 0; y < primaryKeyList.Length; y++)
            {
                primaryKeyList[y] = outputGird[y, 0];
            }
            for (int x = 0; x < keyNameList.Length; x++)
            {
                keyNameList[x] = outputGird[0, x];
            }

            return outputGird;
        }

        /// <summary>
        /// 将一行CSV文本分割
        /// </summary>
        /// <param name="row">row</param>
        /// <returns></returns>
        private string[] SplitCsvRow(string row)
        {
            return (from System.Text.RegularExpressions.Match m in System.Text.RegularExpressions.Regex.Matches(row,
            @"(((?<x>(?=[,\r\n]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,\r\n]+)),?)",
            System.Text.RegularExpressions.RegexOptions.ExplicitCapture)
                    select m.Groups[1].Value).ToArray();
        }

        /// <summary>
        /// 搜索所有匹配元素的下标
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="array">array</param>
        /// <param name="match">match</param>
        /// <returns></returns>
        private int[] SearchIndex<T>(ref T[] array, Predicate<T> match)
        {
            List<int> _indexList = new List<int>();
            for (int i = 0; i < array.Length; i++)
            {
                if (match(array[i]))
                {
                    _indexList.Add(i);
                }
            }
            return _indexList.ToArray();
        }

        /// <summary>
        /// 匹配获取多行
        /// </summary>
        /// <param name="primaryKey">primaryKey</param>
        /// <returns></returns>
        public string[,] GetRows(string primaryKey)
        {
            int[] primaryKeyIndex = SearchIndex(ref primaryKeyList, pk => pk == primaryKey);
            if (primaryKeyIndex.Length > 0)
            {
                string[,] rows = new string[primaryKeyIndex.Length, csvGrid.GetLength(1)];
                for (int y = 0; y < rows.GetLength(0); y++)
                {
                    for (int x = 0; x < rows.GetLength(1); x++)
                    {
                        rows[y, x] = csvGrid[primaryKeyIndex[y], x];
                    }
                }
                return rows;
            }
            else
            {
                Debug.LogWarning("CSVReader 获取行数据异常:主键不存在,返回null.");
                return null;
            }
        }

        /// <summary>
        /// 匹配获取第一行
        /// </summary>
        /// <param name="primaryKey">primaryKey</param>
        /// <returns></returns>
        public string[] GetRowAtFirst(string primaryKey)
        {
            string[,] rows = GetRows(primaryKey);
            if (rows != null)
            {
                string[] row = new string[rows.GetLength(1)];
                for (int x = 0; x < row.Length; x++)
                {
                    row[x] = rows[0, x];
                }
                return row;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 匹配获取最后一行
        /// </summary>
        /// <param name="primaryKey">primaryKey</param>
        /// <returns></returns>
        public string[] GetRowAtLast(string primaryKey)
        {
            string[,] rows = GetRows(primaryKey);
            if (rows != null)
            {
                string[] row = new string[rows.GetLength(1)];
                for (int x = 0; x < row.Length; x++)
                {
                    row[x] = rows[rows.GetLength(0) - 1, x];
                }
                return row;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 匹配获取多列
        /// </summary>
        /// <param name="keyName">keyName</param>
        /// <returns></returns>
        public string[,] GetColumns(string keyName)
        {
            int[] keyNameIndex = SearchIndex(ref keyNameList, k => k == keyName);
            if (keyNameIndex.Length > 0)
            {
                string[,] columns = new string[keyNameIndex.Length, csvGrid.GetLength(0)];
                for (int x = 0; x < columns.GetLength(0); x++)
                {
                    for (int y = 0; y < columns.GetLength(1); y++)
                    {
                        columns[x, y] = csvGrid[y, keyNameIndex[x]];
                    }
                }
                return columns;
            }
            else
            {
                Debug.LogWarning("CSVReader 获取列数据异常:键名不存在,返回null.");
                return null;
            }
        }

        /// <summary>
        /// 匹配获取第一列
        /// </summary>
        /// <param name="keyName">keyName</param>
        /// <param name="ignoreRowNum">忽略行数,因为大多数情况下第一行为键名,所以默认忽略</param>
        /// <returns></returns>
        public string[] GetColumnAtFirst(string keyName, int ignoreRowNum = 1)
        {
            string[,] columns = GetColumns(keyName);
            if (columns != null && columns.GetLength(1) > ignoreRowNum)
            {
                string[] column = new string[columns.GetLength(1) - ignoreRowNum];
                for (int y = 0; y < column.Length; y++)
                {
                    column[y] = columns[0, y + ignoreRowNum];
                }
                return column;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 匹配获取最后一列
        /// </summary>
        /// <param name="keyName">keyName</param>
        /// <param name="ignoreRowNum">忽略行数,因为大多数情况下第一行为键名,所以默认忽略</param>
        /// <returns></returns>
        public string[] GetColumnAtLast(string keyName, int ignoreRowNum = 1)
        {
            string[,] columns = GetColumns(keyName);
            if (columns != null && columns.GetLength(1) > ignoreRowNum)
            {
                string[] column = new string[columns.GetLength(1) - ignoreRowNum];
                for (int y = 0; y < column.Length; y++)
                {
                    column[y] = columns[columns.GetLength(0) - 1, y + ignoreRowNum];
                }
                return column;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 匹配获取多个值
        /// </summary>
        /// <param name="primaryKey">primaryKey</param>
        /// <param name="keyName">keyName</param>
        /// <returns></returns>
        public string[] GetValues(string primaryKey, string keyName)
        {
            string[,] rows = GetRows(primaryKey);
            if (rows == null)
                return null;

            int[] keyNameIndex = SearchIndex(ref keyNameList, k => k == keyName);
            if (keyNameIndex.Length > 0)
            {
                string[] values = new string[keyNameIndex.Length * rows.GetLength(0)];
                for (int y = 0; y < rows.GetLength(0); y++)
                {
                    for (int x = 0; x < keyNameIndex.Length; x++)
                    {
                        values[(y * (keyNameIndex.Length - 1)) + x] = rows[y, keyNameIndex[x]];
                    }
                }
                return values;
            }
            else
            {
                Debug.LogWarning("CSVReader 获取值异常:键名不存在,返回null.");
                return null;
            }
        }

        /// <summary>
        /// 匹配获取第一个值
        /// </summary>
        /// <param name="primaryKey">primaryKey</param>
        /// <param name="keyName">keyName</param>
        /// <returns></returns>
        public string GetValueAtFirst(string primaryKey, string keyName)
        {
            string[] values = GetValues(primaryKey, keyName);
            if (values != null)
            {
                return values[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 匹配获取最后一个值
        /// </summary>
        /// <param name="primaryKey">primaryKey</param>
        /// <param name="keyName">keyName</param>
        /// <returns></returns>
        public string GetValueAtLast(string primaryKey, string keyName)
        {
            string[] values = GetValues(primaryKey, keyName);
            if (values != null)
            {
                return values[values.Length - 1];
            }
            else
            {
                return null;
            }
        }
    }
}
