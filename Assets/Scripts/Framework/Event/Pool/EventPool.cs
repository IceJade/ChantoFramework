using Framework;
using System;
using System.Collections.Generic;

namespace Framework.Event.Pool
{
    /// <summary>
    /// 事件池。
    /// </summary>
    /// <typeparam name="T">事件类型。</typeparam>
    public partial class EventPool<T> where T : GameEventArgs
    {
        private readonly Dictionary<EventId, EventHandler<T>> m_EventHandlers = new();
        private readonly MultiKeyDictionary<object, EventId, EventHandler<T>> m_ObjectEventHandlers = new();
        private readonly Queue<Event> m_Events = new();
        private readonly EventPoolMode m_EventPoolMode;
        private EventHandler<T> m_DefaultHandler;

        /// <summary>
        /// 初始化事件池的新实例。
        /// </summary>
        /// <param name="mode">事件池模式。</param>
        public EventPool(EventPoolMode mode)
        {
            m_EventPoolMode = mode;
            m_DefaultHandler = null;
        }

        /// <summary>
        /// 获取事件处理函数的数量。
        /// </summary>
        public int EventHandlerCount
        {
            get
            {
                return m_EventHandlers.Count;
            }
        }

        /// <summary>
        /// 获取事件数量。
        /// </summary>
        public int Count
        {
            get
            {
                return m_Events.Count;
            }
        }

        /// <summary>
        /// 事件池轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            while (m_Events.Count > 0)
            {
                Event e = null;
                lock (m_Events)
                {
                    e = m_Events.Dequeue();
                }

                HandleEvent(e.Sender, e.EventArgs);
            }
        }

        /// <summary>
        /// 关闭并清理事件池。
        /// </summary>
        public void Shutdown()
        {
            Clear();
            m_DefaultHandler = null;
        }

        /// <summary>
        /// 清理事件。
        /// </summary>
        public void Clear()
        {
            lock (m_Events)
            {
                m_Events.Clear();
            }
            m_EventHandlers.Clear();
        }

        /// <summary>
        /// 检查订阅事件处理函数。
        /// </summary>
        /// <param name="id">事件类型编号。</param>
        /// <param name="handler">要检查的事件处理函数。</param>
        /// <returns>是否存在事件处理函数。</returns>
        public bool Check(EventId id, EventHandler<T> handler)
        {
            if (handler == null)
            {
                throw new Exception("Event handler is invalid.");
            }

            EventHandler<T> handlers = null;
            if (!m_EventHandlers.TryGetValue(id, out handlers))
            {
                return false;
            }

            if (handlers == null)
            {
                return false;
            }

            foreach (EventHandler<T> i in handlers.GetInvocationList())
            {
                if (i == handler)
                {
                    return true;
                }
            }

            return false;
        }

        public bool Check(object owner, EventId id)
        {
            if (owner == null)
            {
                throw new Exception("owner is invalid.");
            }

            return m_ObjectEventHandlers.ContainsKey(owner, id);
        }

        /// <summary>
        /// 订阅事件处理函数。
        /// </summary>
        /// <param name="id">事件类型编号。</param>
        /// <param name="handler">要订阅的事件处理函数。</param>
        public void Subscribe(EventId id, EventHandler<T> handler)
        {
            if (handler == null)
            {
                throw new Exception("Event handler is invalid.");
            }

            EventHandler<T> eventHandler = null;
            if (!m_EventHandlers.TryGetValue(id, out eventHandler) || eventHandler == null)
            {
                m_EventHandlers[id] = handler;
            }
            else if ((m_EventPoolMode & EventPoolMode.AllowMultiHandler) == 0)
            {
                Log.Error(string.Format("Event '{0}' not allow multi handler.", id.ToString()));
                return;
            }
            else if ((m_EventPoolMode & EventPoolMode.AllowDuplicateHandler) == 0 && Check(id, handler))
            {
                Log.Error(string.Format("Event '{0}' not allow duplicate handler.", id.ToString()));
                return;
            }
            else
            {
                eventHandler += handler;
                m_EventHandlers[id] = eventHandler;
            }
        }

        public void Subscribe(object owner, EventId id, EventHandler<T> handler)
        {
            if (handler == null)
            {
                throw new Exception("Event handler is invalid.");
            }

            EventHandler<T> eventHandler = null;
            if (!m_EventHandlers.TryGetValue(id, out eventHandler) || eventHandler == null)
            {
                m_EventHandlers[id] = handler;
            }
            else if ((m_EventPoolMode & EventPoolMode.AllowMultiHandler) == 0)
            {
                Log.Error(string.Format("Event '{0}' not allow multi handler.", id.ToString()));
                return;
            }
            else if ((m_EventPoolMode & EventPoolMode.AllowDuplicateHandler) == 0 && Check(id, handler))
            {
                Log.Error(string.Format("Event '{0}' not allow duplicate handler.", id.ToString()));
                return;
            }
            else
            {
                eventHandler += handler;
                m_EventHandlers[id] = eventHandler;
            }
            
            if(null != owner)
            {
                if (m_ObjectEventHandlers.ContainsKey(owner, id))
                {
                    m_ObjectEventHandlers[owner][id] = handler;
                }
                else
                {
                    if (!m_ObjectEventHandlers.ContainsKey(owner))
                        m_ObjectEventHandlers.Add(owner, new Dictionary<EventId, EventHandler<T>>() { { id, handler } });
                    else
                        m_ObjectEventHandlers[owner].Add(id, handler);
                }
            }
        }

        /// <summary>
        /// 取消订阅事件处理函数。
        /// </summary>
        /// <param name="id">事件类型编号。</param>
        /// <param name="handler">要取消订阅的事件处理函数。</param>
        public void Unsubscribe(EventId id, EventHandler<T> handler)
        {
            if (handler == null)
            {
                Log.Error("Event handler is invalid.");
                return;
                // throw new Exception("Event handler is invalid.");
            }

            if (m_EventHandlers.ContainsKey(id))
            {
                m_EventHandlers[id] -= handler;
            }
        }

        /// <summary>
        /// 取消订阅事件处理函数。
        /// </summary>
        /// <param name="id">事件类型编号。</param>
        /// <param name="handler">要取消订阅的事件处理函数。</param>
        public void Unsubscribe(object owner, EventId eventId)
        {
            if (owner == null)
            {
                Log.Error("owner is invalid.");
                return;
                // throw new Exception("Event handler is invalid.");
            }

            if (m_ObjectEventHandlers.ContainsKey(owner, eventId))
            {
                var handler = m_ObjectEventHandlers[owner][eventId];

                if (m_EventHandlers.ContainsKey(eventId))
                    m_EventHandlers[eventId] -= handler;

                var dic = m_ObjectEventHandlers[owner];
                dic.Remove(eventId);
            }
        }

        public void UnsubscribeAll(object owner)
        {
            if (owner == null)
            {
                Log.Error("owner is invalid.");
                return;
                // throw new Exception("Event handler is invalid.");
            }

            if (m_ObjectEventHandlers.ContainsKey(owner))
            {
                foreach(var iter in m_ObjectEventHandlers[owner])
                {
                    var eventId = iter.Key;
                    var handler = iter.Value;

                    if (m_EventHandlers.ContainsKey(eventId))
                        m_EventHandlers[eventId] -= handler;
                }

                m_ObjectEventHandlers.Remove(owner);
            }
        }

        /// <summary>
        /// 设置默认事件处理函数。
        /// </summary>
        /// <param name="handler">要设置的默认事件处理函数。</param>
        public void SetDefaultHandler(EventHandler<T> handler)
        {
            m_DefaultHandler = handler;
        }

        /// <summary>
        /// 抛出事件，这个操作是线程安全的，即使不在主线程中抛出，也可保证在主线程中回调事件处理函数，但事件会在抛出后的下一帧分发。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">事件参数。</param>
        public void Fire(object sender, T e)
        {
            Event eventNode = new Event(sender, e);
            lock (m_Events)
            {
                m_Events.Enqueue(eventNode);
            }
        }

        /// <summary>
        /// 抛出事件立即模式，这个操作不是线程安全的，事件会立刻分发。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">事件参数。</param>
        public void FireNow(object sender, T e)
        {
            HandleEvent(sender, e);
        }

        /// <summary>
        /// 处理事件结点。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">事件参数。</param>
        private void HandleEvent(object sender, T e)
        {
            var eventId = e.Id;
            EventHandler<T> handlers = null;
            if (m_EventHandlers.TryGetValue(eventId, out handlers))
            {
                if (handlers != null)
                {
                    handlers(sender, e);
                }
            }

            if (handlers == null && m_DefaultHandler != null)
            {
                handlers = m_DefaultHandler;
                handlers(sender, e);
            }

            e.SetDirty();

            if (handlers == null && (m_EventPoolMode & EventPoolMode.AllowNoHandler) == 0)
            {
                throw new Exception(string.Format("Event '{0}' not allow no handler.", eventId.ToString()));
            }
        }
    }
}
