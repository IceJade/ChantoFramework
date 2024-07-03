using Framework.Event.Pool;
using Framework;
using System;
using Framework.Pool;

namespace Framework.Event
{
    /// <summary>
    /// 事件管理器。
    /// </summary>
    public class EventManager : GameBaseSingletonModule<EventManager>
    {
        private readonly EventPool<GameEventArgs> m_EventPool;

        /// <summary>
        /// 初始化事件管理器的新实例。
        /// </summary>
        public EventManager()
        {
            m_EventPool = new EventPool<GameEventArgs>(EventPoolMode.AllowNoHandler | EventPoolMode.AllowMultiHandler);
            LogicPoolManager.Instance.SetCapacity<GameEventArgs>(200);
            LogicPoolManager.Instance.SetCapacity<CommonEventArgs>(200);
        }

        /// <summary>
        /// 获取事件数量。
        /// </summary>
        public int Count
        {
            get
            {
                return m_EventPool.Count;
            }
        }

        /// <summary>
        /// 事件管理器轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            m_EventPool.Update(elapseSeconds, realElapseSeconds);
        }

        /// <summary>
        /// 关闭并清理事件管理器。
        /// </summary>
        public override void Shutdown()
        {
            m_EventPool.Shutdown();
        }

        /// <summary>
        /// 检查订阅事件处理函数。
        /// </summary>
        /// <param name="id">事件类型编号。</param>
        /// <param name="handler">要检查的事件处理函数。</param>
        /// <returns>是否存在事件处理函数。</returns>
        public bool Check(EventId id, EventHandler<GameEventArgs> handler)
        {
            return m_EventPool.Check(id, handler);
        }

        public bool Check(object owner, EventId id)
        {
            return m_EventPool.Check(owner, id);
        }

        /// <summary>
        /// 订阅事件处理函数。
        /// </summary>
        /// <param name="id">事件类型编号。</param>
        /// <param name="handler">要订阅的事件处理函数。</param>
        public void Subscribe(EventId id, EventHandler<GameEventArgs> handler)
        {
            m_EventPool.Subscribe(id, handler);
        }

        public void Subscribe(object owner, EventId id, EventHandler<GameEventArgs> handler)
        {
            m_EventPool.Subscribe(owner, id, handler);
        }

        /// <summary>
        /// 取消订阅事件处理函数。
        /// </summary>
        /// <param name="id">事件类型编号。</param>
        /// <param name="handler">要取消订阅的事件处理函数。</param>
        public void Unsubscribe(EventId id, EventHandler<GameEventArgs> handler)
        {
            m_EventPool.Unsubscribe(id, handler);
        }

        public void Unsubscribe(object owner, EventId eventId)
        {
            m_EventPool.Unsubscribe(owner, eventId);
        }

        /// <summary>
        /// 设置默认事件处理函数。
        /// </summary>
        /// <param name="handler">要设置的默认事件处理函数。</param>
        public void SetDefaultHandler(EventHandler<GameEventArgs> handler)
        {
            m_EventPool.SetDefaultHandler(handler);
        }

        /// <summary>
        /// 抛出事件，这个操作是线程安全的，即使不在主线程中抛出，也可保证在主线程中回调事件处理函数，但事件会在抛出后的下一帧分发。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">事件参数。</param>
        public void Fire(object sender, GameEventArgs e)
        {
            m_EventPool.Fire(sender, e);
        }

        /// <summary>
        /// 抛出事件立即模式，这个操作不是线程安全的，事件会立刻分发。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">事件参数。</param>
        public void FireNow(object sender, GameEventArgs e)
        {
            m_EventPool.FireNow(sender, e);
        }

        public void UnsubscribeAll(object owner)
        {
            m_EventPool.UnsubscribeAll(owner);
        }

        public void UnsubscribeAll()
        {
            m_EventPool?.Clear();
        }
    }
}
