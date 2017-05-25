using UnityEngine;
using System.Collections;
using CSVSystem;

public class CSVReaderExample : MonoBehaviour
{
    public TextAsset asset;

    void Start()
    {
        //不忽略首尾行全文本读取对象
        CSVReader allReader = new CSVReader(asset.text);
        //首部忽略首部一行和忽略尾部一行，部分文本读取对象
        CSVReader partReader = new CSVReader(asset.text, 1, 1);

        //全文本读取所有行主键为‘1’
        string debug1 = "(allReader)GetRows:\r\n";
        string[,] gird = allReader.GetRows("1");
        for (int i = 0; i < gird.GetLength(0); i++)
        {
            debug1 += "第" + (i + 1) + "行数据: # ";
            for (int j = 0; j < gird.GetLength(1); j++)
            {
                debug1 += gird[i, j] + " # ";
            }
            debug1 += "\r\n";
        }
        Debug.Log(debug1);

        //全文本读取第一行主键为‘primaryKeyNote’
        string debug2 = "(allReader)GetRowAtFirst: # ";
        foreach (var item in allReader.GetRowAtFirst("primaryKeyNote"))
        {
            debug2 += item + " # ";
        }
        Debug.Log(debug2);

        //全文本读取最后一行主键为‘1’
        string debug3 = "(allReader)GetRowAtLast: # ";
        foreach (var item in allReader.GetRowAtLast("1"))
        {
            debug3 += item + " # ";
        }
        Debug.Log(debug3);

        //全文本读取所有列主键为‘key1Note’
        string debug4 = "(allReader)GetColumns:\r\n";
        string[,] gird2 = allReader.GetColumns("key1Note");
        for (int i = 0; i < gird2.GetLength(0); i++)
        {
            debug4 += "第" + (i + 1) + "列数据: # ";
            for (int j = 0; j < gird2.GetLength(1); j++)
            {
                debug4 += gird2[i, j] + " # ";
            }
            debug4 += "\r\n";
        }
        Debug.Log(debug4);

        //全文本读取第一列键名为‘key1Note’
        //读取单列的时候返回的数据默认忽略了键名 [ignoreRowNum = 1]
        string debug5 = "(allReader)GetColumnAtFirst: # ";
        foreach (var item in allReader.GetColumnAtFirst("key1Note"))
        {
            debug5 += item + " # ";
        }
        Debug.Log(debug5);

        //全文本读取第一列键名为‘key1Note’
        //读取单列的时候返回的数据默认忽略了键名 [ignoreRowNum = 1]
        string debug6 = "(allReader)GetColumnAtLast: # ";
        foreach (var item in allReader.GetColumnAtLast("key1Note"))
        {
            debug6 += item + " # ";
        }
        Debug.Log(debug6);

        //全文本读取多个值主键为‘1’,键名为key1Note
        string debug7 = "(allReader)GetValues: # ";
        foreach (var item in allReader.GetValues("1", "key1Note"))
        {
            debug7 += item + " # ";
        }
        Debug.Log(debug7);

        //全文本读取第一个值主键为‘1’，键名为key1Note
        string debug8 = "(allReader)GetValueAtFirst: ";
        Debug.Log(debug8 + allReader.GetValueAtFirst("1", "key1Note"));

        //全文本读取最后一个值主键为‘1’，键名为key1Note
        string debug9 = "(allReader)GetValueAtLast: ";
        Debug.Log(debug9 + allReader.GetValueAtLast("1", "key1Note"));


        /**
         * 
         * 部分文本读取方式一样，只是部分文本获取数据时忽略了首部第一行注释行和尾部最后一行空行，使得可以控制读取有效数据行
         * 
         **/
    }
}
