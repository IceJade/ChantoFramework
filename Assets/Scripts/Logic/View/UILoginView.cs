using Chanto;
using Framework;
using System;
using TMPro;
using UnityEngine;

public class UILoginView : UIBaseView
{
    public TMP_InputField inputUserName;
    public TMP_InputField inputPassword;
    public CDButton btnLogin;

    public static void OpenUI(Action<UILoginView> openCallback = null)
    {
        GameEntry.UI.OpenUIForm(Constant.UIAssets.UILoginView, openCallback: (form) =>
        {
            var view = form.Logic as UILoginView;
            openCallback?.Invoke(view);
        });
    }

    protected internal override void OnOpen(object userData)
    {
        //GameEntry.Event.Subscribe(EventId,);

    }

    protected internal override void OnClose(object userData)
    {

    }

    public override void OnAddEventListener()
    {


        btnLogin.onClick.RemoveAllListeners();
        btnLogin.onClick.AddListener(() =>
        {
            if (!CheckInputText())
                return;
        });
    }

    public override void OnRemoveEventListener()
    {

    }

    private bool CheckInputText()
    {
        if (this.inputUserName.text.IsNullOrEmpty())
        {
            return false;
        }

        if (this.inputPassword.text.IsNullOrEmpty())
        {
            return false;
        }
        return true;
    }

    public void OnConnectSuccess()
    {
        Debug.LogError("连接成功");

    }

    public void OnConnectFailure()
    {
        Debug.LogError("连接失败");
    }

    public void OnLoginSuccess()
    {
        Debug.LogError("登录成功");
        GameEntry.StateMachine.Transition(LaunchStateEnum.Game);
        this.CloseSelf();
    }
}
