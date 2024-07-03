using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace Framework
{
    // public sealed class AutoRecycle : MonoBehaviour
    // {
    //     private voi
    // }
    // public sealed class UsedObjectAutoRecycal : MonoBehaviour
    // {
    //     private void Awake()
    //     {
    //         
    //     }
    //
    //     private void OnDestroy()
    //     {
    //         throw new NotImplementedException();
    //     }
    // }

    // 拥有这个组件的 GameObject 都是AssetPrefab派生实例
    // 只要存在生成的实例
    public sealed class UsedObjectPool : SingletonBehaviour<UsedObjectPool>
    {
        public enum StartupPoolMode { Awake, Start, CallManually };

        [System.Serializable]
        public class StartupPool
        {
            public int size;
            public GameObject prefab;
        }

        static List<int> tempList = new List<int>();

#if ODIN_INSPECTOR
        [ShowInInspector, ShowIf("showOdinInfo"),
         DictionaryDrawerSettings(IsReadOnly = true, DisplayMode = DictionaryDisplayOptions.Foldout)]
#endif
        Dictionary<GameObject, List<GameObject>>
            pooledObjects = new Dictionary<GameObject, List<GameObject>>(); // prefab, List<obj>
#if ODIN_INSPECTOR
        [ShowInInspector, ShowIf("showOdinInfo"),
         DictionaryDrawerSettings(IsReadOnly = true, DisplayMode = DictionaryDisplayOptions.OneLine)]
#endif
        Dictionary<int, GameObject> spawnedObjects = new Dictionary<int, GameObject>(); // obj.GetInstance(), prefab

        Dictionary<int, GameObject> instanceObjects = new Dictionary<int, GameObject>(); // obj.GetInstance(), instance

        public StartupPoolMode startupPoolMode = StartupPoolMode.CallManually;
        public StartupPool[] startupPools;

        bool startupPoolsCreated;

        public override void Release()
        {
            base.Release();
        }

        void Awake()
        {
            if (startupPoolMode == StartupPoolMode.Awake)
                CreateStartupPools();
        }

        void Start()
        {
            if (startupPoolMode == StartupPoolMode.Start)
                CreateStartupPools();

            Instance.StartCoroutine(Instance.CheckGC());
        }

        public static void CreateStartupPools()
        {
            if (!Instance.startupPoolsCreated)
            {
                Instance.startupPoolsCreated = true;
                var pools = Instance.startupPools;
                if (pools != null && pools.Length > 0)
                    for (int i = 0; i < pools.Length; ++i)
                        CreatePool(pools[i].prefab, pools[i].size);
            }
        }

        public bool TryAddObjectInstance(GameObject obj, GameObject prefab)
        {
            if (spawnedObjects.ContainsKey(obj.GetInstanceID()))
                return false;
            spawnedObjects.Add(obj.GetInstanceID(), prefab);
            instanceObjects.Add(obj.GetInstanceID(), obj);
            return true;
        }

        public void TryRemoveObjectInstance(GameObject obj)
        {
            TryRemoveObjectInstance(obj.GetInstanceID());
        }

        public void TryRemoveObjectInstance(int instanceID)
        {
            if (spawnedObjects.ContainsKey(instanceID))
            {
                spawnedObjects.Remove(instanceID);
                instanceObjects.Remove(instanceID);
            }
        }

        public IEnumerator CheckGC()
        {
            while (true)
            {
                yield return new WaitForSeconds(20f);

                foreach (var instanceItem in instanceObjects)
                {
                    if (instanceItem.Value == null)
                    {
                        tempList.Add(instanceItem.Key);
                    }
                }

                int len = tempList.Count;
                for (int j = 0; j < len; j++)
                {
                    TryRemoveObjectInstance(tempList[j]);
                }

                tempList.Clear();

                List<GameObject> needClearPrefabList = new List<GameObject>();
                foreach (var item in pooledObjects)
                {
                    var i = 0;
                    var count = item.Value.Count();
                    if (count > 0)
                    {
                        //清除池子里不用的实例化资源
                        while (i < count)
                        {
                            Destroy(item.Value[i]);
                            i++;
                        }

                        item.Value.Clear();
                    }

                    bool isSpawned = false;
                    foreach (var spawnedItem in spawnedObjects)
                    {
                        //如果正在用的池子有相同的prefab资源，那么就不能清除原始资源
                        if (spawnedItem.Value == item.Key)
                        {
                            isSpawned = true;
                            break;
                        }
                    }

                    if (!isSpawned)
                    {
                        needClearPrefabList.Add(item.Key);
                    }
                }


                while (needClearPrefabList.Count() > 0)
                {
                    GameObject prefab = needClearPrefabList[0];
                    needClearPrefabList.RemoveAt(0);
                    // 移除缓存池
                    _instance.pooledObjects.Remove(prefab); 
                    // 顺序不能调换， UnloadAssetWithObject 对 _instance.pooledObjects 有检测
                    // 池子已经是延迟清理了，所以可以立即删除
                    if (ResourceManager.Instance.HaveAssetCache(prefab))
                        ResourceManager.Instance.UnloadAssetWithObject(prefab, true);
                }
            }
            yield return null;
        }

        public static void CreatePool<T>(T prefab, int initialPoolSize) where T : Component
        {
            CreatePool(prefab.gameObject, initialPoolSize);
        }

        public static void CreatePool(GameObject prefab, int initialPoolSize)
        {
            if (prefab == null)
                return;
            // 实现 ResourceManager 产生  prefab 的实例 池子唯一性
            if (Instance.spawnedObjects.TryGetValue(prefab.GetInstanceID(), out GameObject go))
                prefab = go;
            
            if (prefab != null && !Instance.pooledObjects.ContainsKey(prefab))
            {
                var list = new List<GameObject>();
                Instance.pooledObjects[prefab] = list;

                if (initialPoolSize > 0)
                {
                    bool active = prefab.activeSelf;
                    prefab.SetActive(false);
                    Transform parent = Instance.transform;
                    while (list.Count < initialPoolSize)
                    {
                        var obj = Instantiate(prefab);
                        obj.name = obj.name.Replace("(Clone)", "(Spawn)"); // mark object as "Spawn"
                        obj.transform.SetParent(parent, false);
                        list.Add(obj);
                    }

                    prefab.SetActive(active);
                }
            }
        }

        public static T Spawn<T>(T prefab, Transform parent, Vector3 position, Quaternion rotation) where T : Component
        {
            return Spawn(prefab.gameObject, parent, position, rotation).GetComponent<T>();
        }

        public static T Spawn<T>(T prefab, Vector3 position, Quaternion rotation) where T : Component
        {
            return Spawn(prefab.gameObject, null, position, rotation).GetComponent<T>();
        }

        public static T Spawn<T>(T prefab, Transform parent, Vector3 position) where T : Component
        {
            return Spawn(prefab.gameObject, parent, position, Quaternion.identity).GetComponent<T>();
        }

        public static T Spawn<T>(T prefab, Vector3 position) where T : Component
        {
            return Spawn(prefab.gameObject, null, position, Quaternion.identity).GetComponent<T>();
        }

        public static T Spawn<T>(T prefab, Transform parent) where T : Component
        {
            return Spawn(prefab.gameObject, parent, Vector3.zero, Quaternion.identity).GetComponent<T>();
        }

        public static T Spawn<T>(T prefab) where T : Component
        {
            return Spawn(prefab.gameObject, null, Vector3.zero, Quaternion.identity).GetComponent<T>();
        }

        public static GameObject Spawn(GameObject prefab, Transform parent, Vector3 position, Quaternion rotation, bool resetPos = true)
        {
            Transform trans;
            if (Instance.spawnedObjects.TryGetValue(prefab.GetInstanceID(), out GameObject go))
                prefab = go;
            GameObject obj;
            if (Instance.pooledObjects.TryGetValue(prefab, out List<GameObject> list))
            {
                obj = null;
                if (list.Count > 0)
                {
                    // the object in list maybe null
                    while (obj == null && list.Count > 0)
                    {
                        obj = list[0];
                        list.RemoveAt(0);
                    }
                }
                if (obj == null)
                {
                    obj = Instantiate(prefab);
                    obj.name = obj.name.Replace("(Clone)", "(Spawn)"); // mark object as "Spawn"
                }
                Instance.TryAddObjectInstance(obj, prefab);
            }
            else
            {
                obj = Instantiate(prefab);
            }


            // obj.name = obj.name.Replace("(Clone)", "(Spawn)");  // mark object as "Spawn"

            trans = obj.GetComponent<Transform>();
            trans.SetParent(parent, false);
            if (resetPos)
            {
                trans.localPosition = position;
            }
            trans.localRotation = rotation;
            obj.SetActive(true);
            return obj;
        }

        public static GameObject Spawn(GameObject prefab, Transform parent, Vector3 position)
        {
            return Spawn(prefab, parent, position, Quaternion.identity);
        }

        public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            return Spawn(prefab, null, position, rotation);
        }

        public static GameObject Spawn(GameObject prefab, Transform parent)
        {
            return Spawn(prefab, parent, Vector3.zero, Quaternion.identity);
        }

        public static GameObject Spawn(GameObject prefab, Vector3 position)
        {
            return Spawn(prefab, null, position, Quaternion.identity);
        }

        public static GameObject Spawn(GameObject prefab)
        {
            return Spawn(prefab, null, Vector3.zero, Quaternion.identity);
        }

        public static void Recycle<T>(T obj) where T : Component
        {
            Recycle(obj.gameObject);
        }

        public static void Recycle(GameObject obj)
        {
            if (Instance != null && Instance.spawnedObjects.TryGetValue(obj.GetInstanceID(), out GameObject prefab))
            {
                Recycle(obj, prefab);
            }
            else
            {
                Log.WarningFormat("{0} has been recycled or not in pooled, it will be destroyed", obj);
                Destroy(obj);
            }
        }

        static void Recycle(GameObject obj, GameObject prefab)
        {
            Instance.TryRemoveObjectInstance(obj.GetInstanceID());
            if (!obj) return;
            if (Instance.pooledObjects.TryGetValue(prefab, out List<GameObject> list))
            {
                list.Add(obj);
                obj.transform.SetParent(Instance.transform, false);
                obj.SetActive(false);
            }
            else
            {
                Log.WarningFormat("{0} has been recycled or not in pooled, it will be destroyed", obj);
                Destroy(obj);
            }
        }
        
        public static void DestoryObject(GameObject obj)
        {
           
           
            if (Instance.spawnedObjects.TryGetValue(obj.GetInstanceID(), out GameObject prefab))
            {
                if (Instance.pooledObjects.TryGetValue(prefab, out List<GameObject> list))
                {
                    list.Remove(obj);
                    if (list.Count == 0)
                    {
                        Instance.pooledObjects.Remove(prefab);
                        ResourceManager.Instance.UnloadAssetWithObject(prefab, true);
                    }
                }
            }
            Instance.TryRemoveObjectInstance(obj.GetInstanceID());
            if (!obj) return;
            Destroy(obj);
        }

        public static void RecycleAll<T>(T prefab) where T : Component
        {
            RecycleAll(prefab.gameObject);
        }

        public static void RecycleAll(GameObject prefab)
        {
            if (Instance != null)
            {
                foreach (var item in Instance.spawnedObjects)
                {
                    if (item.Value == prefab)
                        tempList.Add(item.Key);
                }

                GameObject obj;
                for (int i = 0; i < tempList.Count; ++i)
                {
                    if (Instance.instanceObjects.TryGetValue(tempList[i], out obj))
                    {
                        Recycle(obj);
                    }
                }

                tempList.Clear();
            }
        }

        public static bool HavePool(GameObject obj)
        {
            return Instance.pooledObjects.ContainsKey(obj);
        }

        public static bool IsSpawned(GameObject obj)
        {
            return Instance.spawnedObjects.ContainsKey(obj.GetInstanceID());
        }

        public static int CountPooled<T>(T prefab) where T : Component
        {
            return CountPooled(prefab.gameObject);
        }

        public static int CountPooled(GameObject prefab)
        {
            return Instance.pooledObjects.TryGetValue(prefab, out List<GameObject> list) ? list.Count : 0;
        }

        public static int CountSpawned<T>(T prefab) where T : Component
        {
            return CountSpawned(prefab.gameObject);
        }

        public static int CountSpawned(GameObject prefab)
        {
            int count = 0;
            foreach (var instancePrefab in Instance.spawnedObjects.Values)
                if (prefab == instancePrefab)
                    ++count;
            return count;
        }

        public static int CountAllPooled()
        {
            int count = 0;
            foreach (var list in Instance.pooledObjects.Values)
                count += list.Count;
            return count;
        }

        // public static List<GameObject> GetPooled(GameObject prefab, List<GameObject> list, bool appendList)
        // {
        //     if (list == null)
        //         list = new List<GameObject>();
        //     if (!appendList)
        //         list.Clear();
        //     if (Instance.pooledObjects.TryGetValue(prefab, out List<GameObject> pooled))
        //     {
        //         list.AddRange(pooled);
        //     }
        //
        //     return list;
        // }
        // public static List<T> GetPooled<T>(T prefab, List<T> list, bool appendList) where T : Component
        // {
        //     if (list == null)
        //         list = new List<T>();
        //     if (!appendList)
        //         list.Clear();
        //     if (Instance.pooledObjects.TryGetValue(prefab.gameObject, out List<GameObject> pooled))
        //     {
        //         for (int i = 0; i < pooled.Count; ++i)
        //             list.Add(pooled[i].GetComponent<T>());
        //     }
        //
        //     return list;
        // }

        //TODO
        // public static List<GameObject> GetSpawned(GameObject prefab, List<GameObject> list, bool appendList)
        // {
        //     if (list == null)
        //         list = new List<GameObject>();
        //     if (!appendList)
        //         list.Clear();
        //     foreach (var item in Instance.spawnedObjects)
        //         if (item.Value == prefab)
        //             list.Add(item.Key);
        //     return list;
        // }
        // public static List<T> GetSpawned<T>(T prefab, List<T> list, bool appendList) where T : Component
        // {
        //     if (list == null)
        //         list = new List<T>();
        //     if (!appendList)
        //         list.Clear();
        //     var prefabObj = prefab.gameObject;
        //     foreach (var item in Instance.spawnedObjects)
        //         if (item.Value == prefabObj)
        //             list.Add(item.Key.GetComponent<T>());
        //     return list;
        // }

        public static void DestroyPooled(GameObject prefab)
        {
            if (Instance != null && Instance.pooledObjects.TryGetValue(prefab, out List<GameObject> pooled))
            {
                Instance.pooledObjects.Remove(prefab);
                for (int i = 0; i < pooled.Count; ++i)
                    Destroy(pooled[i]);
                pooled.Clear();
            }
        }
        // public static void DestroyPooled<T>(T prefab) where T : Component
        // {
        //     DestroyPooled(prefab.gameObject);
        // }

        public static GameObject GetPrefab(GameObject obj)
        {
            return Instance.spawnedObjects.TryGetValue(obj.GetInstanceID(), out GameObject prefab) ? prefab : null;
        }

        // public static GameObject GetPrefab<T>(T obj) where T : Component
        // {
        //     return GetPrefab(obj.gameObject);
        // }
    }

   
}