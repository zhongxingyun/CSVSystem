using UnityEngine;
using System.Collections.Generic;
using CSVSystem;

public class CSVManager : Singleton<CSVManager>
{
    private Dictionary<string, CSVReader> data;

    void Awake()
    {
        data = new Dictionary<string, CSVReader>();
    }

    /// <summary>
    /// 加载资源
    /// </summary>
    /// <param name="path"></param>
    public void ResourcesAsset(string path)
    {
        data.Clear();
        TextAsset[] assets = Resources.LoadAll<TextAsset>(path);
        for (int i = 0; i < assets.Length; i++)
        {
            data.Add(assets[i].name, new CSVReader(assets[i].text));
        }
    }
}
