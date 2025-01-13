using Zenject;
using UnityEngine;

namespace Installers
{
    public class BootInstaller : MonoInstaller
    {
        [SerializeField] private LevelLoader levelLoader;
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<BootLoader>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<ApplicationContext>().AsSingle().NonLazy();
            Container.Bind<LevelLoader>().FromInstance(levelLoader).AsSingle().NonLazy();
        }
    }
}