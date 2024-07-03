public class LaunchErrorState : LaunchState
{
    public override LaunchStateEnum Name => LaunchStateEnum.Error;
    protected override void OnEnter(LaunchStateEnum from)
    {
    }

    protected override void OnExit(LaunchStateEnum to)
    {
    }

    private void OnConfirm()
    {
    }
}
