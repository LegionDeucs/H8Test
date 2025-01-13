using MyCore.StateMachine;
using UnityEngine;
using Zenject;

public class GameplayLoader : IInitializable
{
    [Inject] private GameplayStateMachine _gameplayStateMachine;

    public void Initialize()
    {
        _gameplayStateMachine.Initialize();
        _gameplayStateMachine.Enter<GameState>();
    }
}