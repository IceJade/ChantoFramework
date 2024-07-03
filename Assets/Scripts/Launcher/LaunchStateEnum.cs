
public enum LaunchStateEnum
{
    None,
    SplashScreen,
    InitGameModules,
    LoadXML,
    PrepareConnectServer,
    FetchServerList,
    ConnectServer,
    CheckServerStatus,
    Login,
    AssetsUpdate,
    AppUpdate,
    WaitingPushInit,
    Auth,
    PrepareGame,
    Game,
    ReloadGame,
    Error,
    Quit,

    // 进入到lua脚本中的状态
    Lua,
}
