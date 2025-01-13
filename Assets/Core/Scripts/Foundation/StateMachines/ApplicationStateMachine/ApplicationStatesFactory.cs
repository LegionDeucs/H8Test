using System;
using System.Collections.Generic;
using Zenject;

namespace MyCore.StateMachine
{
    public class ApplicationStatesFactory : StateMachineFactory<ApplicationStateMachineBaseState, ApplicationContext>
    {
        public ApplicationStatesFactory(DiContainer container) : base(container)
        {
        }

        public override Dictionary<Type, ApplicationStateMachineBaseState> CreateStates() => new Dictionary<Type, ApplicationStateMachineBaseState>
        {
            { typeof(StartupApplicationState), _container.Instantiate<StartupApplicationState>() },
            { typeof(LoadingSceneApplicationState), _container.Instantiate<LoadingSceneApplicationState>() },
            { typeof(GameApplicationState), _container.Instantiate<GameApplicationState>() }
        };
    }
}