//using AppsFlyerSDK;

public class LoginState : LaunchState
{
    public override LaunchStateEnum Name => LaunchStateEnum.Login;
    public override float ProgressValue => 50f;

    protected override void OnEnter(LaunchStateEnum from)
    {

    }


    protected override void OnExit(LaunchStateEnum to)
    {

    }

    private void OnLoginSuccess()
    {

    }

    private void OnLoginFailure()
    {

    }
}
