using UnityEngine;
using System.Collections;

namespace CSVSystem
{
    /// <summary>
    /// 单例封装
    /// </summary>
    /// <typeparam name="T">T</typeparam>
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        /// <summary>
        /// 单例
        /// </summary>
        private static T _instance;

        /// <summary>
        /// 锁
        /// </summary>
        private static object _lock = new object();

        /// <summary>
        /// 应用是否退出
        /// </summary>
        private static bool applicationIsQuitting = false;

        /// <summary>
        /// 全局调用接口
        /// </summary>
        public static T Instance
        {
            get
            {
                if (applicationIsQuitting)
                {
                    Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                    "' 应用退出时被删除" +
                    " 无法重新创建 - 返回null.");
                    return null;
                }

                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = FindObjectOfType(typeof(T)) as T;
                        if (FindObjectsOfType(typeof(T)).Length > 1)
                        {
                            Debug.LogError("[Singleton] 异常 " +
                            " - 不应该存在多个同样的单例类!");
                            return _instance;
                        }

                        if (_instance == null)
                        {
                            GameObject singleton = new GameObject();
                            _instance = singleton.AddComponent<T>();
                            //_instance.SingletonInitialize();

                            singleton.name = "(singleton)" + typeof(T).ToString();

                            DontDestroyOnLoad(singleton);
                        }
                    }

                    return _instance;
                }
            }
        }

        public void OnDestroy()
        {
            applicationIsQuitting = true;
        }
    }
}
