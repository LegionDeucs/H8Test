using UnityEngine;
using MyCore.StateMachine;
using Zenject;

namespace Installers
{
    public class ApplicationInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ApplicationStatesFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<ApplicationStateMachine>().AsSingle();

            InstallSaveSystem();

            Container.Bind<LevelLoader>().AsSingle().NonLazy();
            InstallService();
        }

        private void InstallSaveSystem()
        {
//            Container.Bind<IFilePathProvider>().To<FilePathProvider>().AsSingle();
//            Container.Bind<ISaveDataProvider>().To<SaveDataProvider>().AsSingle();

//#if UNITY_EDITOR
//            Container.Bind<IDataFormatter>().To<JsonDataFormatter>().AsSingle();
//#else
//            Container.Bind<IDataFormatter>().To<BinaryDataFormatter>().AsSingle();
//#endif

//            Container.Bind<IDataFileStreamer>().To<FileDataStreamer>().AsSingle();

//            Container.Bind<ISaveLoadSystem>().To<SaveLoadSystem>().AsSingle().NonLazy();
        }

        private void InstallService()
        {
        }
    }
}