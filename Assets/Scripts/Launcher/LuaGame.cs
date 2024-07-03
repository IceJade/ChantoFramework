public interface LuaGame
{
    void OnPrepare();
    void OnEnter();
    void Update();
    void LateUpdate();
    void FixedUpdate();
    void OnApplicationFocus(bool focus);
    void OnApplicationQuit();
}