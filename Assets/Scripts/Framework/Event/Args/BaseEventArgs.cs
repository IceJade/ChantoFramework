using Framework.Pool;

namespace Framework.Event
{
    /// <summary>
    /// 事件基类。
    /// </summary>
    public abstract class BaseEventArgs : BaseLogicData
    {
        public abstract void SetEventId(EventId eventId);
        public virtual void SetParam(bool param) { }
        public virtual void SetParam(int param) { }
        public virtual void SetParam(long param) { }
        public virtual void SetParam(float param) { }
        public virtual void SetParam(string param) { }
        public virtual void SetParam(object param) { }
    }
}
