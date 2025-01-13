namespace MyCore.StateMachine
{
    public class ApplicationStateMachine : StateMachine<ApplicationContext, ApplicationStateMachineBaseState>
    {
        private ApplicationStatesFactory _applicationStatesFactory;

        public ApplicationStateMachine(ApplicationStatesFactory applicationStatesFactory)
        {
            _applicationStatesFactory = applicationStatesFactory;
        }

        public override void Initialize()
        {
            _states = _applicationStatesFactory.CreateStates();
        }
    }
}