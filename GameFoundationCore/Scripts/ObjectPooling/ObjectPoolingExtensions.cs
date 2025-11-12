using System.Collections.Generic;
using UnityEngine;

namespace GameDevelopmentKit.GameFoundationCore.ObjectPooling
{
    public static class ObjectPoolingingExtensions
    {
        public static ObjectPooling CreatePool<T>(this T prefab, GameObject root = null) where T : Component
        {
            return ObjectPoolingManager.Instance.CreatePool(prefab, 0, root);
        }

        public static ObjectPooling CreatePool<T>(this T prefab, int initialPoolSize, GameObject root = null) where T : Component
        {
            return ObjectPoolingManager.Instance.CreatePool(prefab, initialPoolSize, root);
        }

        public static ObjectPooling CreatePool(this GameObject prefab, GameObject root = null)
        {
            return ObjectPoolingManager.Instance.CreatePool(prefab, 0, root);
        }

        public static ObjectPooling CreatePool(this GameObject prefab, int initialPoolSize, GameObject root = null)
        {
            return ObjectPoolingManager.Instance.CreatePool(prefab, initialPoolSize, root);
        }

        public static T Spawn<T>(this T prefab, Transform parent, Vector3 position, Quaternion rotation) where T : Component
        {
            return ObjectPoolingManager.Instance.Spawn(prefab, parent, position, rotation);
        }

        public static T Spawn<T>(this T prefab, Vector3 position, Quaternion rotation) where T : Component
        {
            return ObjectPoolingManager.Instance.Spawn(prefab, null, position, rotation);
        }

        public static T Spawn<T>(this T prefab, Transform parent, Vector3 position) where T : Component
        {
            return ObjectPoolingManager.Instance.Spawn(prefab, parent, position, Quaternion.identity);
        }

        public static T Spawn<T>(this T prefab, Vector3 position) where T : Component
        {
            return ObjectPoolingManager.Instance.Spawn(prefab, null, position, Quaternion.identity);
        }

        public static T Spawn<T>(this T prefab, Transform parent) where T : Component
        {
            return ObjectPoolingManager.Instance.Spawn(prefab, parent, Vector3.zero, Quaternion.identity);
        }

        public static T Spawn<T>(this T prefab) where T : Component
        {
            return ObjectPoolingManager.Instance.Spawn(prefab, null, Vector3.zero, Quaternion.identity);
        }

        public static GameObject Spawn(this GameObject prefab, Transform parent, Vector3 position, Quaternion rotation)
        {
            return ObjectPoolingManager.Instance.Spawn(prefab, parent, position, rotation);
        }

        public static GameObject Spawn(this GameObject prefab, Vector3 position, Quaternion rotation)
        {
            return ObjectPoolingManager.Instance.Spawn(prefab, null, position, rotation);
        }

        public static GameObject Spawn(this GameObject prefab, Transform parent, Vector3 position)
        {
            return ObjectPoolingManager.Instance.Spawn(prefab, parent, position, Quaternion.identity);
        }

        public static GameObject Spawn(this GameObject prefab, Vector3 position)
        {
            return ObjectPoolingManager.Instance.Spawn(prefab, null, position, Quaternion.identity);
        }

        public static GameObject Spawn(this GameObject prefab, Transform parent)
        {
            return ObjectPoolingManager.Instance.Spawn(prefab, parent, Vector3.zero, Quaternion.identity);
        }

        public static GameObject Spawn(this GameObject prefab)
        {
            return ObjectPoolingManager.Instance.Spawn(prefab, null, Vector3.zero, Quaternion.identity);
        }

        public static void Recycle<T>(this T obj) where T : Component
        {
            ObjectPoolingManager.Instance.Recycle(obj);
        }

        public static void Recycle(this GameObject obj)
        {
            ObjectPoolingManager.Instance.Recycle(obj);
        }

        public static void Recycle(this GameObject obj, Transform parent)
        {
            ObjectPoolingManager.Instance.Recycle(obj, parent);
        }

        public static void RecycleAll<T>(this T prefab) where T : Component
        {
            ObjectPoolingManager.Instance.RecycleAll(prefab);
        }

        public static void RecycleAll(this GameObject prefab)
        {
            ObjectPoolingManager.Instance.RecycleAll(prefab);
        }

        public static int CountPooled<T>(this T prefab) where T : Component
        {
            return ObjectPoolingManager.Instance.CountPooled(prefab);
        }

        public static int CountPooled(this GameObject prefab)
        {
            return ObjectPoolingManager.Instance.CountPooled(prefab);
        }

        public static int CountSpawned<T>(this T prefab) where T : Component
        {
            return ObjectPoolingManager.Instance.CountSpawned(prefab);
        }

        public static int CountSpawned(this GameObject prefab)
        {
            return ObjectPoolingManager.Instance.CountSpawned(prefab);
        }

        public static List<GameObject> GetSpawned(this GameObject prefab, List<GameObject> list, bool appendList)
        {
            return ObjectPoolingManager.Instance.GetSpawned(prefab, list, appendList);
        }

        public static List<GameObject> GetSpawned(this GameObject prefab, List<GameObject> list)
        {
            return ObjectPoolingManager.Instance.GetSpawned(prefab, list, false);
        }

        public static List<GameObject> GetSpawned(this GameObject prefab)
        {
            return ObjectPoolingManager.Instance.GetSpawned(prefab, null, false);
        }

        public static List<T> GetSpawned<T>(this T prefab, List<T> list, bool appendList) where T : Component
        {
            return ObjectPoolingManager.Instance.GetSpawned(prefab, list, appendList);
        }

        public static List<T> GetSpawned<T>(this T prefab, List<T> list) where T : Component
        {
            return ObjectPoolingManager.Instance.GetSpawned(prefab, list, false);
        }

        public static List<T> GetSpawned<T>(this T prefab) where T : Component
        {
            return ObjectPoolingManager.Instance.GetSpawned(prefab, null, false);
        }

        public static List<GameObject> GetPooled(this GameObject prefab, List<GameObject> list, bool appendList)
        {
            return ObjectPoolingManager.Instance.GetPooled(prefab, list, appendList);
        }

        public static List<GameObject> GetPooled(this GameObject prefab, List<GameObject> list)
        {
            return ObjectPoolingManager.Instance.GetPooled(prefab, list, false);
        }

        public static List<GameObject> GetPooled(this GameObject prefab)
        {
            return ObjectPoolingManager.Instance.GetPooled(prefab, null, false);
        }

        public static List<T> GetPooled<T>(this T prefab, List<T> list, bool appendList) where T : Component
        {
            return ObjectPoolingManager.Instance.GetPooled(prefab, list, appendList);
        }

        public static List<T> GetPooled<T>(this T prefab, List<T> list) where T : Component
        {
            return ObjectPoolingManager.Instance.GetPooled(prefab, list, false);
        }

        public static List<T> GetPooled<T>(this T prefab) where T : Component
        {
            return ObjectPoolingManager.Instance.GetPooled(prefab, null, false);
        }

        public static void CleanUpPooled(this GameObject prefab)
        {
            ObjectPoolingManager.Instance.CleanUpPooled(prefab);
        }

        public static void CleanUpPooled<T>(this T prefab) where T : Component
        {
            ObjectPoolingManager.Instance.CleanUpPooled(prefab.gameObject);
        }

        public static void CleaUpAll(this GameObject prefab)
        {
            ObjectPoolingManager.Instance.CleanUpAll(prefab);
        }

        public static void CleaUpAll<T>(this T prefab) where T : Component
        {
            ObjectPoolingManager.Instance.CleanUpAll(prefab.gameObject);
        }

        public static void DestroyPool(this GameObject prefab)
        {
            ObjectPoolingManager.Instance.DestroyPool(prefab);
        }

        public static void DestroyPool<T>(this T prefab) where T : Component
        {
            ObjectPoolingManager.Instance.DestroyPool(prefab.gameObject);
        }
    }
}