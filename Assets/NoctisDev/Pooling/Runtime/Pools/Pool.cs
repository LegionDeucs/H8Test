using System;
using System.Collections.Generic;
using System.Linq;
using NoctisDev.Pooling.Runtime.Configs;
using NoctisDev.Pooling.Runtime.Factories;
using NoctisDev.Pooling.Runtime.Helpers;
using UnityEngine;

namespace NoctisDev.Pooling.Runtime.Pools
{
    public class Pool<TItem> : IPool where TItem : Component
    {
        private readonly PoolConfiguration _poolConfiguration;
        private readonly IPoolableItemFactory<TItem> _factory;
        private readonly IDelayedInvoker _delayedInvoker;
        private readonly Transform _itemsHolder;
        private readonly PoolInfo _poolInfo;
        private readonly List<Component> _storedItems = new();
        private readonly List<Component> _spawnedItems = new();
        private readonly List<Component> _totalItems = new();

        public PoolInfo Info => _poolInfo;

        public Pool(PoolConfiguration poolConfiguration,
            IPoolableItemFactory<TItem> factory,
            IDelayedInvoker delayedInvoker,
            Transform itemsHolder)
        {
            _factory = factory;
            _delayedInvoker = delayedInvoker;
            _itemsHolder = itemsHolder;
            _poolConfiguration = poolConfiguration;
            _poolInfo = new PoolInfo(_storedItems, _spawnedItems, _totalItems);
        }

        public void Initialize()
        {
            for (int i = 0; i < _poolConfiguration.Preload; i++) 
                CreateNewItem();
        }

        public TComponent Spawn<TComponent>() where TComponent : Component
        {
            TComponent item = GetItemToSpawn<TComponent>();

            if (item == null) 
                return item;
            
            RemoveItem(item);
            item.gameObject.SetActive(true);
            NotifyOnSpawn(item);

            return item;
        }

        public void Despawn<TComponent>(TComponent item) where TComponent : Component
        {
            if(!_spawnedItems.Contains(item))
                return;
                
            NotifyOnDespawn(item);
            AddItem(item);
        }

        public void DespawnDelayed<TComponent>(TComponent item, float delay) where TComponent : Component => 
           _delayedInvoker.DelayedInvoke(delay, () => Despawn(item));

        public bool HasItemsToSpawn() => _storedItems.Count > 0;

        private void NotifyOnSpawn(Component item)
        {
            if (item is IPoolable poolable) 
                poolable.OnSpawn();
        }

        private void NotifyOnDespawn(Component item)
        {
            if (item is IPoolable poolable) 
                poolable.OnDespawn();
        }

        private TComponent GetItemToSpawn<TComponent>() where TComponent : Component
        {
            Component itemToSpawn = _storedItems.FirstOrDefault();
            
            if (itemToSpawn == null)
            {
                if(_poolConfiguration.Recycle && _totalItems.Count > 0)
                    itemToSpawn = RecycleItem();
                else
                    itemToSpawn = CreateNewItem();
            }

            if (itemToSpawn == null)
                return null;
            
            if (itemToSpawn is TComponent item)
                return item;
            else
                throw new InvalidCastException("Poolable object type is wrong, check object type that configured or make sure it's not null");
        }

        private Component RecycleItem()
        {
            Component item = _spawnedItems.First();
            Despawn(item);
            return item;
        }

        private void AddItem(Component item)
        {
            _storedItems.Add(item);
            _spawnedItems.Remove(item);
            item.transform.SetParent(_itemsHolder);
            item.gameObject.SetActive(false);
        }

        private void RemoveItem(Component item)
        {
            _storedItems.Remove(item);
            _spawnedItems.Add(item);
            item.transform.SetParent(null);
            item.gameObject.SetActive(true);
        }

        private TItem CreateNewItem()
        {
            int capacity = _poolConfiguration.Capacity;
            if (capacity != 0 && capacity <= _totalItems.Count)
            {
#if UNITY_EDITOR
                Debug.LogWarning("The pool is out of capacity");          
#endif
                return null;   
            }
            
            TItem item = _factory.Create(_itemsHolder);
            AddItem(item);
            _totalItems.Add(item);
            return item;
        }
    }
}