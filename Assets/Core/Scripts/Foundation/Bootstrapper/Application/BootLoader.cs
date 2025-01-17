using MyCore.StateMachine;
using UnityEngine;
using Zenject;

public class BootLoader : IInitializable
{
    private readonly ApplicationStateMachine _applicationStateMachine;
    //private readonly ISaveLoadSystem _sls;

    public BootLoader(ApplicationStateMachine applicationStateMachine
        //,ISaveLoadSystem sls
        )
    {
        _applicationStateMachine = applicationStateMachine;
        //_sls = sls;
    }

    public void Initialize()
    {
        Application.targetFrameRate = 60;

        //_sls.Load();
        _applicationStateMachine.Initialize();
        _applicationStateMachine.Enter<StartupApplicationState>();
    }
}