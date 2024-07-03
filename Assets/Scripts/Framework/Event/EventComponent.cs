using System;
using UnityEngine;

namespace Framework.Event
{
    /// <summary>
    /// 事件组件。
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class EventComponent : FrameworkComponent
    {
        /// <summary>
        /// 获取事件数量。
        /// </summary>
        public int Count
        {
            get
            {
                return EventManager.Instance.Count;
            }
        }

        #region 检查事件

        /// <summary>
        /// 检查订阅事件处理回调函数。
        /// </summary>
        /// <param name="id">事件类型编号。</param>
        /// <param name="handler">要检查的事件处理回调函数。</param>
        /// <returns>是否存在事件处理回调函数。</returns>
        public bool Check(int id, EventHandler<GameEventArgs> handler)
        {
            return EventManager.Instance.Check((EventId)id, handler);
        }

        /// <summary>
        /// 检查订阅事件处理回调函数。
        /// </summary>
        /// <param name="id">事件类型编号。</param>
        /// <param name="handler">要检查的事件处理回调函数。</param>
        /// <returns>是否存在事件处理回调函数。</returns>
        public bool Check(EventId id, EventHandler<GameEventArgs> handler)
        {
            return EventManager.Instance.Check(id, handler);
        }

        public bool Check(object owner, EventId id)
        {
            return EventManager.Instance.Check(owner, id);
        }

        #endregion

        #region 订阅和取消事件

        /// <summary>
        /// 订阅事件处理回调函数。
        /// </summary>
        /// <param name="id">事件类型编号。</param>
        /// <param name="handler">要订阅的事件处理回调函数。</param>
        public void Subscribe(int id, EventHandler<GameEventArgs> handler)
        {
            EventManager.Instance.Subscribe((EventId)id, handler);
        }

        public void Subscribe(EventId id, EventHandler<GameEventArgs> handler)
        {
            EventManager.Instance.Subscribe(id, handler);
        }

        public void Subscribe(object owner, EventId id, EventHandler<GameEventArgs> handler)
        {
            EventManager.Instance.Subscribe(owner, id, handler);
        }

        /// <summary>
        /// 取消订阅事件处理回调函数。
        /// </summary>
        /// <param name="id">事件类型编号。</param>
        /// <param name="handler">要取消订阅的事件处理回调函数。</param>
        public void Unsubscribe(int id, EventHandler<GameEventArgs> handler)
        {
            EventManager.Instance.Unsubscribe((EventId)id, handler);
        }

        public void Unsubscribe(EventId id, EventHandler<GameEventArgs> handler)
        {
            EventManager.Instance.Unsubscribe(id, handler);
        }

        public void Unsubscribe(object owner, EventId eventId)
        {
            EventManager.Instance.Unsubscribe(owner, eventId);
        }

        public void UnsubscribeAll(object owner)
        {
            EventManager.Instance.UnsubscribeAll(owner);
        }

        public void UnsubscribeAll()
        {
            EventManager.Instance.UnsubscribeAll();
        }

        #endregion

        #region 触发事件

        public void Fire(EventId eventId)
        {
            Fire(null, eventId);
        }

        public void Fire(EventId eventId, bool userData)
        {
            Fire(null, eventId, userData);
        }

        public void Fire(EventId eventId, int userData)
        {
            Fire(null, eventId, userData);
        }

        public void Fire(EventId eventId, float userData)
        {
            Fire(null, eventId, userData);
        }

        public void Fire(EventId eventId, string userData)
        {
            Fire(null, eventId, userData);
        }

        public void Fire(EventId eventId, GameEventArgs e)
        {
            Fire(null, eventId, e);
        }

        public void FireEx(EventId eventId, object userData)
        {
            FireEx(null, eventId, userData);
        }

        public void Fire(object sender, EventId eventId)
        {
            var e = EventUtils.CreateArgs<GameEventArgs>(eventId);
            EventManager.Instance.Fire(sender, e);
        }

        public void Fire(object sender, EventId eventId, bool userData)
        {
            var e = EventUtils.CreateArgs<CommonEventArgs>(eventId, userData);
            EventManager.Instance.Fire(sender, e);
        }

        public void Fire(object sender, EventId eventId, int userData)
        {
            var e = EventUtils.CreateArgs<CommonEventArgs>(eventId, userData);
            EventManager.Instance.Fire(sender, e);
        }

        public void Fire(object sender, EventId eventId, float userData)
        {
            var e = EventUtils.CreateArgs<CommonEventArgs>(eventId, userData);
            EventManager.Instance.Fire(sender, e);
        }

        public void Fire(object sender, EventId eventId, string userData)
        {
            var e = EventUtils.CreateArgs<CommonEventArgs>(eventId, userData);
            EventManager.Instance.Fire(sender, e);
        }

        public void Fire(object sender, EventId eventId, GameEventArgs e)
        {
            EventManager.Instance.Fire(sender, e);
        }

        public void FireEx(object sender, EventId eventId, object userData)
        {
            var e = EventUtils.CreateArgs<CommonEventArgs>(eventId, userData);
            EventManager.Instance.Fire(sender, e);
        }

        public void FireNow(EventId eventId)
        {
            FireNow(null, eventId);
        }

        public void FireNow(EventId eventId, bool userData)
        {
            FireNow(null, eventId, userData);
        }

        public void FireNow(EventId eventId, int userData)
        {
            FireNow(null, eventId, userData);
        }

        public void FireNow(EventId eventId, float userData)
        {
            FireNow(null, eventId, userData);
        }

        public void FireNow(EventId eventId, string userData)
        {
            FireNow(null, eventId, userData);
        }

        public void FireNow(EventId eventId, object userData)
        {
            FireNow(null, eventId, userData);
        }

        public void FireNow(EventId eventId, GameEventArgs e)
        {
            FireNow(null, eventId, e);
        }

        public void FireNow(object sender, EventId eventId)
        {
            var e = EventUtils.CreateArgs<GameEventArgs>(eventId);
            EventManager.Instance.FireNow(sender, e);
        }

        public void FireNow(object sender, EventId eventId, bool userData)
        {
            var e = EventUtils.CreateArgs<CommonEventArgs>(eventId, userData);
            EventManager.Instance.FireNow(sender, e);
        }

        public void FireNow(object sender, EventId eventId, int userData)
        {
            var e = EventUtils.CreateArgs<CommonEventArgs>(eventId, userData);
            EventManager.Instance.FireNow(sender, e);
        }

        public void FireNow(object sender, EventId eventId, float userData)
        {
            var e = EventUtils.CreateArgs<CommonEventArgs>(eventId, userData);
            EventManager.Instance.FireNow(sender, e);
        }

        public void FireNow(object sender, EventId eventId, string userData)
        {
            var e = EventUtils.CreateArgs<CommonEventArgs>(eventId, userData);
            EventManager.Instance.FireNow(sender, e);
        }

        public void FireNow(object sender, EventId eventId, object userData)
        {
            var e = EventUtils.CreateArgs<CommonEventArgs>(eventId, userData);
            EventManager.Instance.FireNow(sender, e);
        }

        public void FireNow(object sender, EventId eventId, GameEventArgs e)
        {
            EventManager.Instance.FireNow(sender, e);
        }

        #endregion
    }
}
