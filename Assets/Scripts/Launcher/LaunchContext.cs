using Framework.Network;
using System;

/// <summary>
/// 登录过程状态之间上下文存储
/// </summary>
public class LaunchContext
{

    public readonly int REQUEST_TIMEOUT = 8;
    public readonly int REQUEST_RETRY_COUNT = 3;

    public bool Logined = false;
    public bool isReload = false;
    public bool IsPushInitReceived = false;
    public bool IsNeedPassword = false;

    public bool useLuaScene = false; //使用lua重构的场景

    //public UILoadingComponent UILoading;

    public string errorMessage;
    public int IsGmLogin = 0;
    public bool IsCross = false; //跨服直接进入世界
    
    public void DisplayLoading()
    {
        //UILoading.SetState(string.Empty);
        //UILoading.gameObject.SetActive(true);
    }

    public void HideLoading()
    {
        //UILoading.gameObject.SetActive(false);
    }

    public void UpdateProgressBar(string content, float progress, bool isForce = false)
    {
        // this.UILoading.SetProgressAndState(content, progress);
        // unity加载流程和cocos略有却别，其中多语言字段没有，暂时试用cococs的加载中...字段
        SetLoadingBarProgress(progress, isForce);
    }

    public void SetLoadingBarState(string content)
    {
        //UILoading?.SetState(content);
    }

    public void SetLoadingBarProgress(float val, bool isForce = false)
    {
        
        //UILoading?.OnSetProgress(val, isForce);
    }

    //public UWebRequest CreateWebRequest(Action<UWebRequest, UnityWebRequestResult> completed)
    //{
    //    UWebRequest request = new UWebRequest();
    //    request.Timeout = REQUEST_TIMEOUT;
    //    request.RetryCount = REQUEST_RETRY_COUNT;
    //    request.Completed += completed;
    //    return request;
    //}
}