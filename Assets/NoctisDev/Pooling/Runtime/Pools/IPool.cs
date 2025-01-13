using UnityEngine;

namespace NoctisDev.Pooling.Runtime.Pools
{
    public interface IPool
    {
        PoolInfo Info { get; }
        
        void Initialize();
        T Spawn<T>() where T : Component;
        void Despawn<T>(T item) where T : Component;
        void DespawnDelayed<T>(T item, float delay) where T : Component;
        bool HasItemsToSpawn();
    }
}