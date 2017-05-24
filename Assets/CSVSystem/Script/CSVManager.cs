using UnityEngine;
using System.Collections.Generic;
using CSVSystem;

/// <summary>
/// CSV管理类
/// </summary>
public class CSVManager : Singleton<CSVManager>
{
    private Dictionary<string, CSVReader> data;

    void Awake()
    {
        //初始化
        data = new Dictionary<string, CSVReader>();

        var configs = Resources.LoadAll<CSVSystemConfig>("CSVSystem");

        if (configs.Length > 1)
            Debug.LogError("CSVManager初始化错误：只能有一个配置文件。");
        else if (configs.Length == 0)
            Debug.LogError("CSVManager初始化错误：找不到配置文件，请创建一个配置文件并放入Resources/CSVSystem文件夹下。");
        else
            ResourcesAsset(configs[0].assetsPath, configs[0].firstRemoveRowNum, configs[0].lastRemoveRowNum);
    }

    /// <summary>
    /// 加载资源
    /// </summary>
    /// <param name="path">path</param>
    public void ResourcesAsset(string path, int firstRemoveRowNum = 0, int lastRemoveRowNum = 0)
    {
        data.Clear();
        TextAsset[] assets = Resources.LoadAll<TextAsset>(path);
        for (int i = 0; i < assets.Length; i++)
        {
            data.Add(assets[i].name, new CSVReader(assets[i].text, firstRemoveRowNum, lastRemoveRowNum));
        }
    }

    /// <summary>
    /// 资源读取
    /// </summary>
    /// <param name="assetName"></param>
    /// <returns></returns>
    public CSVReader GetAssetReader(string assetName)
    {
        if (data.ContainsKey(assetName))
        {
            return data[assetName];
        }
        else
        {
            return null;
        }
    }
}
