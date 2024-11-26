public interface IGame
{
    /// <summary>
    /// ��Ϸ���ģ����ѯ��
    /// </summary>
    /// <param name="elapseSeconds">�߼�����ʱ�䣬����Ϊ��λ��</param>
    /// <param name="realElapseSeconds">��ʵ����ʱ�䣬����Ϊ��λ��</param>
    void Update(float elapsedTime, float realElapsedTime);

    void FixedUpdate();

    void LateUpdate();

    void OnApplicationFocus(bool focus);

    void OnApplicationPause(bool paused);

    /// <summary>
    /// �رղ�������Ϸ���ģ�顣
    /// </summary>
    void Shutdown();
}
