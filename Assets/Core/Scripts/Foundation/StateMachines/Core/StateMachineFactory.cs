using MyCore.StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class StateMachineFactory<TBaseState, TContext> where TBaseState : BaseState<TContext> where TContext : IStateMachineContext
{
    protected readonly DiContainer _container;

    public StateMachineFactory(DiContainer container)
    {
        _container = container;
    }

    public abstract Dictionary<Type, TBaseState> CreateStates();
}
