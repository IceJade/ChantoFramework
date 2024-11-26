public interface IGame
{
    /// <summary>
    /// 游戏框架模块轮询。
    /// </summary>
    /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
    /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
    void Update(float elapsedTime, float realElapsedTime);

    void FixedUpdate();

    void LateUpdate();

    void OnApplicationFocus(bool focus);

    void OnApplicationPause(bool paused);

    /// <summary>
    /// 关闭并清理游戏框架模块。
    /// </summary>
    void Shutdown();
}
