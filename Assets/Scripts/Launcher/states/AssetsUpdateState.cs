using System;
using System.Collections;
using Framework.Network;
using UnityEngine;

/// <summary>
/// 登录时资源热更新状态 
/// </summary>
public class AssetsUpdateState : LaunchState
{
    public override float ProgressValue => 10f;

    public override LaunchStateEnum Name => LaunchStateEnum.AssetsUpdate;

    protected override void OnInit()
    {
        Condition = context => !context.isReload;
        FailurePassTo = LaunchStateEnum.PrepareConnectServer;
    }

    protected override void OnEnter(LaunchStateEnum from)
    {

    }

    protected override void OnExit(LaunchStateEnum to)
    {

    }
}
