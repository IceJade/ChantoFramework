using Framework.Pool;

namespace Framework.Event
{
    /// <summary>
    /// 游戏逻辑事件基类。
    /// </summary>
    public class GameEventArgs : BaseEventArgs
    {
        protected EventId m_EventId;
        public EventId Id { get { return m_EventId; } }

        public GameEventArgs()
        {
            m_EventId = EventId.None;
        }

        public GameEventArgs(EventId eventId)
        {
            m_EventId = eventId;
        }

        public override void SetEventId(EventId eventId)
        {
            m_EventId = eventId;
        }

        public override void Clear()
        {
            m_EventId = EventId.None;
        }
    }
}
