using System;
using System.Collections.Generic;
using System.ComponentModel;
using Zenject;

namespace MyCore.StateMachine
{
    public class GameplayStatesFactory : StateMachineFactory<GameplayStateMachineBaseState, GameplayContext>
    {

        public GameplayStatesFactory(DiContainer container) : base(container)
        {

        }

        public override Dictionary<Type, GameplayStateMachineBaseState> CreateStates() => new Dictionary<Type, GameplayStateMachineBaseState>
        {
            { typeof(GameState), _container.Instantiate<GameState>() },
            { typeof(WinGameState), _container.Instantiate<WinGameState>() },
            { typeof(LoseGameState), _container.Instantiate<LoseGameState>() },
        };
    }
}