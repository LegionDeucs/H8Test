using MyCore.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterAplicationState : ApplicationStateMachineBaseState
{
    public EnterAplicationState(ApplicationContext context) : base(context)
    {
    }

    public override void Enter()
    {
        Context.StateMachine.Enter<LoadingSceneApplicationState>();
    }

    public override void Exit()
    {
        
    }
}
