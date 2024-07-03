using UnityEngine;

public class InitGameModuleState : LaunchState
{
    public override float ProgressValue => 20f;
    public override LaunchStateEnum Name => LaunchStateEnum.InitGameModules;

    protected override void OnInit()
    {
        Condition = context => !context.isReload;
        FailurePassTo = LaunchStateEnum.PrepareConnectServer;
    }

    protected override void OnEnter(LaunchStateEnum from)
    {
        Application.backgroundLoadingPriority = ThreadPriority.High;
    }

    protected override void OnExit(LaunchStateEnum to)
    {
    }
}
