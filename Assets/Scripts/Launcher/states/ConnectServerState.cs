using UnityEngine;
using Framework.Network;

public class ConnectServerState : LaunchState
{
    private static readonly string CATCH_PUSH_TYPE = "CATCH_PUSH_TYPE";
        
    public override LaunchStateEnum Name => LaunchStateEnum.ConnectServer;
    public override float ProgressValue => 40f;

    protected override void OnEnter(LaunchStateEnum from)
    {
        //打点
        //PostEventLog.Dot(PostEventLog.DotDefines.ConnectStart);
        
        //NetManager.Instance.OnConnectSuccess += OnConnectSuccess;
        //NetManager.Instance.OnConnectFailure += OnConnectFailure;
        //NetManager.Instance.Connect(GameDataCenter.Server.ServerConfig);
    }

    protected override void OnExit(LaunchStateEnum to)
    {
        //NetManager.Instance.OnConnectSuccess -= OnConnectSuccess;
        //NetManager.Instance.OnConnectFailure -= OnConnectFailure;
    }

    private void OnConnectSuccess()
    {
        ////打点
        //PostEventLog.Dot(PostEventLog.DotDefines.ConnectSuccess);
        
        //string pushType = PlayerPrefs.GetString(CATCH_PUSH_TYPE, string.Empty);
        //if (!string.IsNullOrEmpty(pushType))
        //{
        //    PlayerPrefs.SetString(CATCH_PUSH_TYPE, string.Empty);
        //}
        //this.Parent.Transition(LaunchStateEnum.Login);
    }

    private void OnConnectFailure(string errorMessage)
    {
        //errorMessage = errorMessage.Split('\n')[0];
        //errorMessage = errorMessage.Substring(errorMessage.LastIndexOf(':'));

        //if (errorMessage.Contains("Network is unreachable"))
        //{
        //    ResolveConnectionError(ConnectionConst.ERR_UNREACHABLE, errorMessage);
        //}
        //else
        //{
        //    ResolveConnectionError(ConnectionConst.ERR_NETWORK, errorMessage);
        //}
    }

    private void ResolveConnectionError(int err, string errorMessage)
    {
        //Debug.LogError($"connect error code：{err}, message：{errorMessage}");
        //LuaModule.Instance.NetReset("websocket"); //todo lua自己注册连接事件处理

        //PostEventLog.Dot(PostEventLog.DotDefines.ConnectFailed, errorMessage);
        //Transition(LaunchStateEnum.CheckServerStatus);
    }
}
