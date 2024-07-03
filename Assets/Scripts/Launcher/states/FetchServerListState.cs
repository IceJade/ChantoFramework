using System;
using System.Collections.Generic;
using Framework.Network;
using Framework;
using UnityEngine.Networking;

/// <summary>
/// 获取服务器列表，获取超时后重试3次，如果依然超时则弹出对话窗口，用户确认后继续重试
/// </summary>
public class FetchServerListState : LaunchState
{
    public override float ProgressValue => 35f;
    
    private const string ServerListAssetAddress = "Assets/Main/Prefabs/Debug/UIDebugChooseServer.prefab";
    private const string ERROR_CODE_KEY = "90821031";

    public override LaunchStateEnum Name => LaunchStateEnum.FetchServerList;

    protected override void OnEnter(LaunchStateEnum from)
    {

    }

    protected override void OnExit(LaunchStateEnum to) { }
}
