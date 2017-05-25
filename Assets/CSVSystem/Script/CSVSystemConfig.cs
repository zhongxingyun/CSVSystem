using UnityEngine;
using System.Collections;

namespace CSVSystem
{
    [CreateAssetMenu(fileName = "CSVSystemConfig", menuName = "CSVSystem/CreateConfig")]
    public class CSVSystemConfig : ScriptableObject
    {
        [Header("CSV资源路径")]
        public string assetsPath = "CSVSystem/TextAssets";

        [Header("读取资源首部移除行数")]
        public int firstRemoveRowNum = 1;

        [Header("读取资源尾部移除行数")]
        public int lastRemoveRowNum = 1;
    }
}
