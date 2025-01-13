using NoctisDev.Pooling.Runtime.Configs;
using NoctisDev.Pooling.Runtime.Helpers;
using UnityEngine;
using Zenject;

namespace NoctisDev.Pooling.Runtime.Factories
{
    public class PoolFactoryProvider
    {
        private readonly DiContainer _diContainer;

        public PoolFactoryProvider(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public IPoolableItemFactory<T> GetFactory<T>(PoolConfiguration poolConfiguration) where T : Component
        {
            DiContainer subContainer = _diContainer.CreateSubContainer();
            subContainer.Bind<PoolConfiguration>().FromInstance(poolConfiguration).AsSingle();
            var f = subContainer.Instantiate(TypeSerializer.Deserialize(poolConfiguration.FactoryTypeSerialized));

            return (IPoolableItemFactory<T>)f;
        }
    }
}