using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BaseState<TStateMachineContext> : MyCore.StateMachine.IState where TStateMachineContext :  IStateMachineContext
{
    public TStateMachineContext Context { get; private set; }

    protected BaseState(TStateMachineContext context)
    {
        Context = context;
    }

    public abstract void Enter();

    public abstract void Exit();
}
