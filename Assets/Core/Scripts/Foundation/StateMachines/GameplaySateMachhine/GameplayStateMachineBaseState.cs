using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameplayStateMachineBaseState : BaseState<GameplayContext>
{
    public GameplayStateMachineBaseState(GameplayContext context) : base(context)
    {
    }
}
