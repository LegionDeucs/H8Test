using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ApplicationStateMachineBaseState : BaseState<ApplicationContext>
{
    protected ApplicationStateMachineBaseState(ApplicationContext context) : base(context)
    {
    }
}
