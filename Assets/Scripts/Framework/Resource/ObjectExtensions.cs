using UnityEngine;
using XLua;

namespace Framework
{
    // [扩展]Object 管理 
    public static class ObjectExtension
    {
        // Object 实例化
        public static Object CreateInstClone(this Object original)
        {
            return Object.Instantiate(original);
        }

        public static Object CreateInstClone(this Object original, Vector3 position, Quaternion rotation,
            Transform parent = null)
        {
            return Object.Instantiate(original, position, rotation, parent);
        }

        public static Object CreateInstClone(this Object original, Transform parent,
            bool instantiateInWorldSpace = false)
        {
            return Object.Instantiate(original, parent, instantiateInWorldSpace);
        }

        // 泛型支持 不导入xlua
        [BlackList]
        public static T CreateInstClone<T>(this T original) where T : Object => (T)CreateInstClone((Object)original);

        [BlackList]
        public static T CreateInstClone<T>(this T original, Vector3 position, Quaternion rotation, Transform parent)
            where T : Object =>
            (T)CreateInstClone((Object)original, position, rotation, parent);

        [BlackList]
        public static T CreateInstClone<T>(this T original, Transform parent, bool worldPositionStays = false)
            where T : Object =>
            (T)CreateInstClone((Object)original, parent, worldPositionStays);

        // GameObject特例接口 ：TODO 需要处理一些池化细节
        public static GameObject CreateInstClone(this GameObject original)
        {
            // 判断是否是池化的prefab
            if (UsedObjectPool.HavePool(original))
                return original.Spawn();
            // 判断是否是池化的prefab派生的实例化
            var prefab = UsedObjectPool.GetPrefab(original);
            if (prefab)
                return prefab.Spawn();
            return CreateInstClone<GameObject>(original);
        }

        public static GameObject CreateInstClone(this GameObject original, Vector3 position, Quaternion rotation,
            Transform parent = null)
        {
            // 判断是否是池化的prefab
            if (UsedObjectPool.HavePool(original))
                return original.Spawn(parent, position, rotation);
            // 判断是否是池化的prefab派生的实例化
            var prefab = UsedObjectPool.GetPrefab(original);
            if (prefab)
                return prefab.Spawn(parent, position, rotation);
            return CreateInstClone<GameObject>(original, position, rotation, parent);
        }

        public static GameObject CreateInstClone(this GameObject original, Transform parent,
            bool instantiateInWorldSpace = false)
        {
            // 判断是否是池化的prefab
            if (UsedObjectPool.HavePool(original))
               return original.Spawn(parent);
            // 判断是否是池化的prefab派生的实例化
            var prefab = UsedObjectPool.GetPrefab(original);
            if (prefab)
                return prefab.Spawn(parent);
            return CreateInstClone<GameObject>(original, parent, instantiateInWorldSpace);
        }
    }

    public static class UsedObjectPoolExtensions
    {
        // public static void CreatePool<T>(this T prefab) where T : Component
        // {
        //     UsedObjectPool.CreatePool(prefab, 0);
        // }


        public static void CreatePool(this GameObject prefab)
        {
            UsedObjectPool.CreatePool(prefab, 0);
        }

        public static void CreatePool(this GameObject prefab, int initialPoolSize)
        {
            UsedObjectPool.CreatePool(prefab, initialPoolSize);
        }

        public static void CreatePool(this Component prefab)
        {
            UsedObjectPool.CreatePool(prefab, 0);
        }

        public static void CreatePool(this Component prefab, int initialPoolSize)
        {
            UsedObjectPool.CreatePool(prefab, initialPoolSize);
        }

        [BlackList]
        public static void CreatePool<T>(this T prefab, int initialPoolSize) where T : Component
        {
            UsedObjectPool.CreatePool<T>(prefab, initialPoolSize);
        }


        public static GameObject Spawn(this GameObject prefab, Transform parent, Vector3 position, Quaternion rotation)
        {
            return UsedObjectPool.Spawn(prefab, parent, position, rotation);
        }

        public static GameObject Spawn(this GameObject prefab, Vector3 position, Quaternion rotation)
        {
            return UsedObjectPool.Spawn(prefab, null, position, rotation);
        }

        public static GameObject Spawn(this GameObject prefab, Transform parent, Vector3 position)
        {
            return UsedObjectPool.Spawn(prefab, parent, position, Quaternion.identity);
        }

        public static GameObject Spawn(this GameObject prefab, Vector3 position)
        {
            return UsedObjectPool.Spawn(prefab, null, position, Quaternion.identity);
        }

        public static GameObject Spawn(this GameObject prefab, Transform parent)
        {
            return UsedObjectPool.Spawn(prefab, parent, Vector3.zero, Quaternion.identity);
        }
        public static GameObject Spawn(this GameObject prefab, Transform parent, bool resetPos)
        {
            return UsedObjectPool.Spawn(prefab, parent, Vector3.zero, Quaternion.identity, resetPos);
        }

        public static GameObject Spawn(this GameObject prefab)
        {
            return UsedObjectPool.Spawn(prefab, null, Vector3.zero, Quaternion.identity);
        }

        public static Component Spawn(this Component prefab, Transform parent)
        {
            return UsedObjectPool.Spawn(prefab, parent, Vector3.zero, Quaternion.identity);
        }

        [BlackList]
        public static T Spawn<T>(this T prefab, Transform parent) where T : Component
        {
            return UsedObjectPool.Spawn(prefab, parent, Vector3.zero, Quaternion.identity);
        }
        
        // public static void Recycle(this GameObject obj)
        // {
        //     UsedObjectPool.Recycle(obj);
        // }
        public static void Recycle(this GameObject obj)
        {
            UsedObjectPool.Recycle(obj);
        }

        public static void Recycle(this Component obj)
        {
            UsedObjectPool.Recycle(obj);
        }

        [BlackList]
        public static void Recycle<T>(this T obj) where T : Component
        {
            UsedObjectPool.Recycle(obj);
        }


        public static void RecycleAll(this GameObject prefab)
        {
            UsedObjectPool.RecycleAll(prefab);
        }

        public static void RecycleAll(this Component prefab)
        {
            UsedObjectPool.RecycleAll(prefab);
        }

        [BlackList]
        public static void RecycleAll<T>(this T prefab) where T : Component
        {
            UsedObjectPool.RecycleAll(prefab);
        }
        
        public static void DestoryOjbectInPooled(this GameObject obj)
        {
            UsedObjectPool.DestoryObject(obj);
        }
        public static void DestroyPooled(this GameObject prefab)
        {
            UsedObjectPool.DestroyPooled(prefab);
        }

        public static void DestroyPooled(this Component prefab)
        {
            UsedObjectPool.DestroyPooled(prefab.gameObject);
        }

        [BlackList]
        public static void DestroyPooled<T>(this T prefab) where T : Component
        {
            UsedObjectPool.DestroyPooled(prefab.gameObject);
        }
    }

    // lua 静态接口
    public static class LuaObjectExtension
    {
        public static Object CreateInstClone(Object original)
        {
            return original.CreateInstClone();
        }

        public static Object CreateInstClone(Object original, Vector3 position, Quaternion rotation,
            Transform parent = null)
        {
            return original.CreateInstClone(position, rotation, parent);
        }

        public static Object CreateInstClone(Object original, Transform parent, bool instantiateInWorldSpace = false)
        {
            return original.CreateInstClone(parent, instantiateInWorldSpace);
        }

        public static GameObject CreateInstClone(GameObject original)
        {
            return original.CreateInstClone();
        }

        public static GameObject CreateInstClone(GameObject original, Vector3 position, Quaternion rotation,
            Transform parent = null)
        {
            return original.CreateInstClone(position, rotation, parent);
        }

        public static GameObject CreateInstClone(GameObject original, Transform parent,
            bool instantiateInWorldSpace = false)
        {
            return original.CreateInstClone(parent, instantiateInWorldSpace);
        }
        // 池化静态接口
        /*
        public static void CreatePool(GameObject prefab)
        {
            prefab.CreatePool(0);
        }

        public static void CreatePool(GameObject prefab, int initialPoolSize)
        {
            prefab.CreatePool(initialPoolSize);
        }

        public static void CreatePool(Component prefab)
        {
            prefab.CreatePool();
        }

        public static void CreatePool(Component prefab, int initialPoolSize)
        {
            prefab.CreatePool(initialPoolSize);
        }

        public static GameObject Spawn(GameObject prefab, Transform parent, Vector3 position, Quaternion rotation)
        {
            return prefab.Spawn(parent, position, rotation);
        }

        public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            return prefab.Spawn(null, position, rotation);
        }

        public static GameObject Spawn(GameObject prefab, Transform parent, Vector3 position)
        {
            return prefab.Spawn(parent, position, Quaternion.identity);
        }

        public static GameObject Spawn(GameObject prefab, Vector3 position)
        {
            return prefab.Spawn(null, position, Quaternion.identity);
        }

        public static GameObject Spawn(GameObject prefab, Transform parent)
        {
            return prefab.Spawn(parent, Vector3.zero, Quaternion.identity);
        }

        public static GameObject Spawn(GameObject prefab)
        {
            return prefab.Spawn(null, Vector3.zero, Quaternion.identity);
        }
        
        public static Component Spawn(Component prefab, Transform parent)
        {
            return prefab.Spawn(parent);
        }

        public static void Recycle(GameObject obj)
        {
            obj.Recycle();
        }
        
        public static void Recycle(Component obj)
        {
            obj.Recycle();
        }

        public static void RecycleAll(GameObject prefab)
        {
            prefab.RecycleAll();
        }
        
        public static void RecycleAll(Component prefab)
        {
            prefab.RecycleAll();
        }

        public static void DestroyPooled(GameObject prefab)
        {
            prefab.DestroyPooled();
        }
        
        public static void DestroyPooled(Component prefab)
        {
            prefab.DestroyPooled();
        }
        */
    }
}