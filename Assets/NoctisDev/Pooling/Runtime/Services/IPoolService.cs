using NoctisDev.Pooling.Runtime.Configs;
using NoctisDev.Pooling.Runtime.Factories;
using UnityEngine;

namespace NoctisDev.Pooling.Runtime.Services
{
    public interface IPoolService
    {
        void Initialize();
        //void AddPool(IPoolableItemFactory factory, PoolConfiguration poolConfiguration = null);
        void AddPool<T>(IPoolableItemFactory<T> factory, PoolConfiguration poolConfiguration = null) where T : Component;
        T Spawn<T>(T prefab, Vector3 position = default, Quaternion rotation = default, Transform parent = null) where T : Component;
        void Despawn<T>(T item, float delay = 0) where T : Component;
        T SpawnAndDespawnInTime<T>(T prefab, float timeToDespawn, Vector3 position, Quaternion rotation, Transform parent = null) where T : Component;
        bool HasItemsToSpawn<T>(T item) where T : Component;
    }
}