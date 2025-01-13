using NoctisDev.Pooling.Runtime.Configs;
using UnityEngine;
using Zenject;

namespace NoctisDev.Pooling.Runtime.Factories
{
    public class DefaultPoolableItemFactory : IPoolableItemFactory<Component>
    {
        private readonly IInstantiator _instantiator;
        private readonly Component _prefab;

        public Component Prefab => _prefab;

        public DefaultPoolableItemFactory(PoolConfiguration poolConfiguration, IInstantiator instantiator)
        {
            _instantiator = instantiator;
            _prefab = poolConfiguration.Prefab;
        }
        
        public Component Create(Transform parent)
        {
            Component item = _instantiator.InstantiatePrefab(_prefab).GetComponent(_prefab.GetType());
            item.transform.SetParent(parent);
            return item;
        }
    }
}