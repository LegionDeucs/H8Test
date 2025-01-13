namespace MyCore.StateMachine
{
   public class GameplayStateMachine : StateMachine<GameplayContext, GameplayStateMachineBaseState>
    {
        private GameplayStatesFactory _gameplayStatesFactory;

        public GameplayStateMachine(GameplayStatesFactory gameplayStatesFactory)
        {
            _gameplayStatesFactory = gameplayStatesFactory;
        }

        public override void Initialize()
        {
            _states = _gameplayStatesFactory.CreateStates();
        }
    }
}