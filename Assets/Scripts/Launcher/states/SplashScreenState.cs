using UnityEngine;

public class SplashScreenState : LaunchState
{
    public override LaunchStateEnum Name => LaunchStateEnum.SplashScreen;

    // private const string LOADING_PREFAB_ADDRESS = "Assets/Main/Dynamic/UILoading/UILoading.prefab";
    private const string LOADING_PREFAB_NAME = "UILoading";
    private const string UI_ROOT_PREFAB_PATH = "Launcher/UI/DefaultUI";

    protected override void OnEnter(LaunchStateEnum from)
    {
        //if (Context.UILoading == null)
        //{
        //    Context.UILoading = LoadUILoadingPanel();
        //    Context.SetLoadingBarProgress(0f);
        //}
        //else
        //{
        //    Context.DisplayLoading();
        //}
        //LaunchI18N.StartInitWord();
        Transition(LaunchStateEnum.InitGameModules);
    }

    //private UILoadingComponent LoadUILoadingPanel()
    //{
    //    GameObject instance = GameObject.Find($"{UI_ROOT_PREFAB_PATH}/{LOADING_PREFAB_NAME}");
    //    // GameObject prefab = Resources.Load<GameObject>(LOADING_PREFAB_NAME);
    //    // GameObject instance = ObjectExtension.CreateInstClone(prefab, uiRoot.transform, true);
    //    // instance.name = LOADING_PREFAB_NAME;
        
    //    // normalize rect transform
    //    RectTransform rect = instance.GetComponent<RectTransform>();       
    //    rect.localScale = Vector3.one;
    //    rect.anchorMin = Vector2.zero;
    //    rect.anchorMax = Vector2.one;
    //    rect.offsetMin = Vector2.zero;
    //    rect.offsetMax = Vector2.zero;

    //    instance.GetComponent<Canvas>().sortingOrder = 330;
        
    //    return instance.GetComponent<UILoadingComponent>();
    //}
    
    protected override void OnExit(LaunchStateEnum to) { }
}
