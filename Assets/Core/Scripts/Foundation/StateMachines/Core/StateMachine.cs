using System;
using System.Collections.Generic;

namespace MyCore.StateMachine
{
    public abstract class StateMachine<TContext, TStateMachineState> where TContext : IStateMachineContext where TStateMachineState : BaseState<TContext>
    {
        protected Dictionary<Type, TStateMachineState> _states;
        protected IExitableState _currentState;

        public virtual void Initialize() { }
        
        public void Enter<TState>() where TState : BaseState<TContext>
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        private TState ChangeState<TState>() where TState :BaseState<TContext>
        {
            _currentState?.Exit();

            TState state = GetState<TState>();
            _currentState = state;

            return state;
        }

        private TState GetState<TState>() where TState : BaseState<TContext>
        {
            return _states[typeof(TState)] as TState;
        }
    }
}