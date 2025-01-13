using System;
using System.Collections.Generic;
using System.Linq;
using NoctisDev.Pooling.Runtime.Behaviours;
using NoctisDev.Pooling.Runtime.Configs;
using NoctisDev.Pooling.Runtime.Factories;
using NoctisDev.Pooling.Runtime.Helpers;
using NoctisDev.Pooling.Runtime.Pools;
using UnityEngine;
using Zenject;

namespace NoctisDev.Pooling.Runtime.Services
{
    public class PoolService : MonoBehaviour, IPoolService, IDelayedInvoker
    {
        [SerializeField] private PoolConfiguration _defaultPoolConfig;
        [SerializeField] private PoolConfiguration[] _poolConfigs;

        private Dictionary<GameObject, IPool> _pools = new();
        private Dictionary<GameObject, IPool> _links = new();

        private PoolFactoryProvider _factoryProvider;

        [Inject]
        public void Construct(PoolFactoryProvider factoryProvider)
        {
            _factoryProvider = factoryProvider;
        }
        
        public void Initialize()
        {
            for (int index = 0; index < _poolConfigs.Length; index++)
            {
                PoolConfiguration poolConfiguration = _poolConfigs[index];
                AddPool<Component>(poolConfiguration);
            }
        }
        
        public void AddPool<T>(IPoolableItemFactory<T> factory, PoolConfiguration poolConfiguration = null) where T : Component
        {
            if (poolConfiguration == null)
                poolConfiguration = GetPoolConfig<Component>(null);
            
            AddPool(poolConfiguration, factory);
        }

        public T Spawn<T>(T prefab, Vector3 position = default, Quaternion rotation = default, Transform parent = null) where T : Component
        {
            T item = Spawn(prefab);
            
            if (item == null)
                return item;
            
            item.transform.SetParent(parent);
            item.transform.SetPositionAndRotation(position, rotation);
            return item;
        }

        public T SpawnAndDespawnInTime<T>(T prefab, float timeToDespawn, Vector3 position, Quaternion rotation, Transform parent = null) where T : Component
        {
            T item = Spawn(prefab, position, rotation, parent);
            Despawn(item, timeToDespawn);
            return item;
        }

        public void Despawn<T>(T item, float delay = 0) where T : Component
        {
            if (item == null)
                throw new Exception("The item you are trying to despawn is null");
            
            IPool pool = GetPoolFromLink(item.gameObject);
            _links.Remove(item.gameObject);

            if (delay > 0)
                pool.DespawnDelayed(item, delay);
            else
                pool.Despawn(item);
        }

        public bool HasItemsToSpawn<T>(T item) where T : Component
        {
            IPool pool = GetPool(item);
            return pool.HasItemsToSpawn();
        }

        public void DelayedInvoke(float delay, Action action) => 
            this.DelayInvoke(delay, action);

        private T Spawn<T>(T prefab) where T : Component
        {
            if (prefab == null)
                throw new Exception("The item you are trying to spawn is null");
            
            IPool pool = GetPool(prefab);
            T item = pool.Spawn<T>();
            
            if(item != null)
                _links.TryAdd(item.gameObject, pool);
            
            return item;
        }

        private IPool GetPool<T>(T prefab) where T : Component
        {
            if (!_pools.TryGetValue(prefab.gameObject, out IPool pool))
                pool = AddPool<T>(GetPoolConfig(prefab));

            return pool;
        }

        private IPool GetPoolFromLink(GameObject clone)
        {
            if (!_links.TryGetValue(clone, out IPool pool))
                throw new Exception("The item you are trying to despawn is either already despawned or was never spawned");
            
            return pool;
        }

        private PoolConfiguration GetPoolConfig<T>(T prefab) where T : Component
        {
            PoolConfiguration config;
            if (prefab != null)
            {
                config = _poolConfigs.FirstOrDefault(config => config.Prefab.gameObject == prefab.gameObject);
                
                if (config == null)
                    config = _defaultPoolConfig.Clone<T>(prefab.gameObject);   
            }
            else
            {
                config = _defaultPoolConfig.Clone();
            }

            return config;
        }

        private IPool AddPool<T>(PoolConfiguration poolConfiguration) where T : Component => 
            AddPool(poolConfiguration, _factoryProvider.GetFactory<Component>(poolConfiguration));

        private IPool AddPool<T>(PoolConfiguration poolConfiguration, IPoolableItemFactory<T> factory) where T : Component
        {
            IPool pool = CreatePool(poolConfiguration, factory);
            _pools.Add(factory.Prefab.gameObject, pool);
            return pool;
        }

        private IPool CreatePool<T>(PoolConfiguration poolConfiguration, IPoolableItemFactory<T> factory) where T : Component
        {
            PoolObject poolObject = new GameObject(GetPoolObjectName(poolConfiguration)).AddComponent<PoolObject>();
            Pool<T> pool = new Pool<T>(poolConfiguration, factory, this, poolObject.transform);
            poolObject.transform.SetParent(transform, false);
            poolObject.Construct(pool);
            
            pool.Initialize();
            return pool;
        }

        private string GetPoolObjectName(PoolConfiguration poolConfiguration)
        {
            string poolName = poolConfiguration.Prefab != null ? poolConfiguration.Prefab.name : "Custom";
            return $"{poolName} Pool";
        }
    }
}