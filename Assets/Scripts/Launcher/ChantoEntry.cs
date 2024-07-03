using DG.Tweening;
using Framework;
using System;
using System.Collections;
using UnityEngine;

public class ChantoEntry : IGameEntry
{
    public static Camera UICamera;

    public static float DeltaTime;
    public static ChantoEntry Instance { get; protected set; }
    public static LaunchStateMachine StateMachine { get; private set; }
    private LaunchStateMachine m_launchStateMachine;
    public event Action<float, float> OnUpdate;

    public static bool Logined;
    public void StartGame()
    {
        Instance = this;

        //获取ui相机
        UICamera = GameObject.Find("Launcher/UI/GUICamera").GetComponent<Camera>();

        InitModules();

        m_launchStateMachine = new LaunchStateMachine();
        StateMachine = m_launchStateMachine;
        m_launchStateMachine.Start();
    }

    private void InitModules()
    {
        //FirebaseProxy.Init();
        // AIHelpProxy.Init();
        DOTween.Init();

        var moduleManager = GameLauncher.Instance.ModuleManager;
        //moduleManager.Add(NetManager.Instance);
        moduleManager.Add(TimerManager.Instance);
        //moduleManager.Add(TimerComponent.Instance);
        moduleManager.Add(LuaModule.Instance);
    }

    public void BeforeUpdate()
    {

    }

    public void Update(float elapsedTime, float realElapsedTime)
    {
        DeltaTime = elapsedTime;
        // #if UNITY_ANDROID
        //         if (Input.GetKeyDown(KeyCode.Escape))
        //         {
        //             CommandManager.Instance.ExecuteCommand(Notifications.ON_KEYCODE_BACK_DOWN);
        //         }
        // #endif
        OnUpdate?.Invoke(elapsedTime, realElapsedTime);
        m_launchStateMachine.Update(elapsedTime);
        //SceneContainer.Instance.Update();
        //ScenesManager.getInstance().Update();
    }

    public void LateUpdate()
    {
        //SceneContainer.Instance.LateUpdate();
        //ScenesManager.getInstance().LateUpdate();
    }

    public void FixedUpdate()
    {

    }

    public void OnApplicationFocus(bool focus)
    {
        Debug.Log("OnApplicationFocus" + focus);
    }

    public void OnApplicationPause(bool pause)
    {

    }

    public void OnApplicationQuit()
    {

    }

    public void OnLowMemoryCallback()
    {

    }

    public Coroutine StartCoroutine(IEnumerator iterator)
    {
        return GameLauncher.Instance.StartCoroutine(iterator);
    }

    public void StopCoroutine(Coroutine coroutine)
    {
        GameLauncher.Instance.StopCoroutine(coroutine);
    }

    public void StopCoroutine(IEnumerator iterator)
    {
        GameLauncher.Instance.StopCoroutine(iterator);
    }

    public void StopAllCoroutines()
    {
        GameLauncher.Instance.StopAllCoroutines();
    }
}
