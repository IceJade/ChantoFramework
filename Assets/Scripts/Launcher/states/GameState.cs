using Framework;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public class GameState : LaunchState
{
    public override LaunchStateEnum Name => LaunchStateEnum.Game;
    public override bool Updatable => true;

    protected override void OnEnter(LaunchStateEnum from)
    {
        this.Context.HideLoading();

        LuaModule.Instance.luaGame.OnEnter();

        //if (Context.IsCross)
        //{
        //    SceneController.getInstance().gotoScene(CocosDefine.SCENE_ID_WORLD);
        //    Context.IsCross = false;
        //}
    }

    protected override void OnExit(LaunchStateEnum to)
    {
        //NetManager.Instance.OnConnectionLost -= OnLostConnection;
    }

    protected override void OnUpdate(float deltaTime)
    {

    }
}
