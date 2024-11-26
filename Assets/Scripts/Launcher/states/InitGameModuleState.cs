using AssetBundles;
using Framework;
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

        GameEntry.Controller.Instantiate();
        GameEntry.Controller.Init();

        string localPath = Framework.Utility.GetStreamingAssetsDirectory();
        string remotepath = Framework.Utility.GetPersistentDataPath();
        ResourceManager.Instance.Initialize(localPath, remotepath, OnInitializedCallback, AssetBundleManager.LoadMode.RemoteFirst, AssetBundleManager.LogMode.JustErrors);

        Transition(LaunchStateEnum.Login);
    }

    private void OnInitializedCallback(string key, object obj, string err = null)
    {

    }

    protected override void OnExit(LaunchStateEnum to)
    {
    }
}
