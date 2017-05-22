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

        public CSVReader(string csvText)
        {
            csvGrid = SplitCsvGrid(csvText);
        }

        /// <summary>
        /// 将CSV文本分割
        /// </summary>
        /// <param name="csvText">csvText</param>
        /// <returns></returns>
        private string[,] SplitCsvGrid(string csvText)
        {
            string[] row = csvText.Split(new string[] { "\r\n" }, StringSplitOptions.None);

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
                keyNameList[x] = outputGird[1, x];
            }

            return outputGird;
        }

        /// <summary>
        /// 将一行CSV文本分割
        /// </summary>
        /// <param name="line">line</param>
        /// <returns></returns>
        private string[] SplitCsvRow(string line)
        {
            return (from System.Text.RegularExpressions.Match m in System.Text.RegularExpressions.Regex.Matches(line,
            @"(((?<x>(?=[,\r\n]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,\r\n]+)),?)",
            System.Text.RegularExpressions.RegexOptions.ExplicitCapture)
                    select m.Groups[1].Value).ToArray();
        }

        private int[] Search<T>(ref T[] array, Predicate<T> match)
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

        /*public string[] GetValue(string primaryKey, string keyName)
        {
            int[] primaryKeyIndex = Search(ref primaryKeyList, out primaryKeyIndex, pk => pk == primaryKey);
            if (primaryKeyIndex.Length > 0)
            {
                int[] keyNameIndex;
                Search(ref keyNameList, out keyNameIndex, k => k == primaryKey);
                if(keyNameIndex.Length)
            }
            else
            {
                Debug.LogWarning("CSVReader 异常:主键不存在,返回null.");
                return null;
            }
        }*/
    }
}
