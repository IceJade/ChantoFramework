using Framework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBaseView : UIFormLogic
{
    protected internal override void InternalOnInit(object userData)
    {
        base.InternalOnInit(userData);

        OnInit(userData);
    }

    protected internal override void InternalOnOpen(object userData)
    {
        base.InternalOnOpen(userData);

        OnOpen(userData);
        OnAddEventListener();
    }

    protected internal override void InternalOnClose(object userData)
    {
        base.InternalOnClose(userData);

        OnRemoveEventListener();
        OnClose(userData);
    }

    /// <summary>
    /// 界面初始化。
    /// </summary>
    /// <param name="userData">用户自定义数据。</param>
    protected internal virtual void OnInit(object userData)
    {

    }

    /// <summary>
    /// 界面打开。
    /// </summary>
    /// <param name="userData">用户自定义数据。</param>
    protected internal virtual void OnOpen(object userData)
    {

    }

    /// <summary>
    /// 界面关闭。
    /// </summary>
    /// <param name="userData">用户自定义数据。</param>
    protected internal virtual void OnClose(object userData)
    {

    }
}
