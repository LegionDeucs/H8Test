using MyCore.StateMachine;
using Zenject;
using UnityEngine;

namespace Installers
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private InputSystem inputSystem;
        [SerializeField] private SwipeSystem swipeSystem;

        public override void InstallBindings()
        {
            SetupGameplayStateMachine();

            Container.BindInterfacesAndSelfTo<GameplayLoader>().AsSingle().NonLazy();

            Container.Bind<InputSystem>().FromInstance(inputSystem).AsSingle().NonLazy();
            Container.Bind<SwipeSystem>().FromInstance(swipeSystem).AsSingle().NonLazy();
        }

        private void SetupGameplayStateMachine()
        {
            Container.Bind<GameplayStatesFactory>().AsSingle();
            Container.Bind<GameplayStateMachine>().AsSingle();
        }
    }
}