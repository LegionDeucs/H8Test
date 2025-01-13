using NoctisDev.Pooling.Runtime.Factories;
using NoctisDev.Pooling.Runtime.Services;
using UnityEngine;
using Zenject;

namespace NoctisDev.Pooling.Runtime.Installers
{
    public class PoolInstaller : MonoInstaller
    {
        [SerializeField] private PoolService _poolService;
        
        public override void InstallBindings()
        {
            Container.Bind<IPoolService>().FromInstance(_poolService).AsSingle();
            Container.Bind<PoolFactoryProvider>().AsSingle();
        }
    }
}