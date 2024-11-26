using DG.Tweening;
using Framework;
using System;
using System.Collections;
using UnityEngine;

public class ChantoEntry : IGame
{
    public static float DeltaTime;
    public static ChantoEntry Instance { get; protected set; }

    public event Action<float, float> OnUpdate;

    public static bool Logined;
    public void StartGame()
    {
        Instance = this;

        InitModules();

        GameEntry.StateMachine.Start();
    }

    private void InitModules()
    {
        DOTween.Init();
        GameEntry.Module.Init();
    }

    public void BeforeUpdate()
    {

    }

    public void Update(float elapsedTime, float realElapsedTime)
    {
        DeltaTime = elapsedTime;

        OnUpdate?.Invoke(elapsedTime, realElapsedTime);

        GameEntry.StateMachine.Update(elapsedTime);
        GameEntry.Module.Update(elapsedTime, realElapsedTime);
    }

    public void LateUpdate()
    {
        GameEntry.Module.LateUpdate();
    }

    public void FixedUpdate()
    {
        GameEntry.Module.FixedUpdate();
    }

    public void OnApplicationFocus(bool focus)
    {
        Debug.Log("OnApplicationFocus" + focus);
        GameEntry.Module.OnApplicationFocus(focus);
    }

    public void OnApplicationPause(bool pause)
    {
        GameEntry.Module.OnApplicationPause(pause);
    }

    public void Shutdown()
    {
        GameEntry.Controller.Destroy();
        GameEntry.Module.Shutdown();
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
