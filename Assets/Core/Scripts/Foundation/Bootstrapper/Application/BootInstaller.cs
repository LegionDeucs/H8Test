using Zenject;
using UnityEngine;

namespace Installers
{
    public class BootInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<BootLoader>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<ApplicationContext>().AsSingle().NonLazy();
        }
    }
}