/**********************************************************************************************
 * @说    明: 鼠标事件按钮,支持点击、长按、按下、抬起、离开焦点的按钮
 * @作    者: zhoumingfeng
 * @版 本 号: V1.00
 * @创建时间: 2024.3.6
 **********************************************************************************************/
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Chanto
{
    public class PointerButton : Button
    {
        // 长按时长(多久算长按);
        public float LongPressInterval = 0.1f;

        // 滑动距离(多大距离范围内不算移动,单位为像素);
        public float SlidingDistance = 10.0f;

        // 是否按下;
        private bool IsPressDown = false;

        // 是否抬起;
        public bool IsPressUp { get; private set; }

        // 是否长按;
        private bool IsLongPressDown = false;

        // 按下开始时间;
        private float PressDownStartTime = -1.0f;

        // 按下时的坐标;
        private Vector2 PressPosition = default(Vector2);
        
     

        [FormerlySerializedAs("onLongPressDown")]
        [SerializeField]
        private ButtonClickedEvent m_OnLongPressDown = new ButtonClickedEvent();

        public ButtonClickedEvent onLongPressDown
        {
            get { return m_OnLongPressDown; }
            set { m_OnLongPressDown = value; }
        }

        [FormerlySerializedAs("onLongPressUp")]
        [SerializeField]
        private ButtonClickedEvent m_OnLongPressUp = new ButtonClickedEvent();

        public ButtonClickedEvent onLongPressUp
        {
            get { return m_OnLongPressUp; }
            set { m_OnLongPressUp = value; }
        }

        [FormerlySerializedAs("onLongPressExit")]
        [SerializeField]
        private ButtonClickedEvent m_OnLongPressExit = new ButtonClickedEvent();

        public ButtonClickedEvent onLongPressExit
        {
            get { return m_OnLongPressExit; }
            set { m_OnLongPressExit = value; }
        }

        [XLua.CSharpCallLua]
        public delegate void OnLongPressHandler(GameObject obj);

        private OnLongPressHandler m_OnLongPress = null;

        // 长按委托;
        public OnLongPressHandler OnLongPress
        {
            get { return m_OnLongPress; }
            set { m_OnLongPress = value; }
        }

        // Update is called once per frame
        void Update()
        {
            if (this.IsPressDown && !this.IsLongPressDown
                && Time.time - PressDownStartTime > LongPressInterval)
            {
                // 长按标记;
                IsLongPressDown = true;

                if (null != OnLongPress)
                {
                    OnLongPress.Invoke(this.gameObject);
                }

                //Debug.LogWarning("长按");
            }
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            IsPressDown = true;
            IsPressUp = false;
            IsLongPressDown = false;

            PressPosition = eventData.position;
            PressDownStartTime = Time.time;

            m_OnLongPressDown?.Invoke();

            //Debug.LogWarning("鼠标按下");
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            IsPressDown = false;
            IsPressUp = true;
            IsLongPressDown = false;

            onLongPressUp?.Invoke();

            //Debug.LogWarning("鼠标抬起");
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            /*
            Debug.LogWarningFormat("OnPointerExit position=({0},{1}), pressPosition=({2},{3}, delta=({4},{5})", 
                eventData.position.x, eventData.position.y, 
                eventData.pressPosition.x, eventData.pressPosition.y,
                eventData.delta.x, eventData.delta.y);

            if (IsPressDown && !IsPressUp 
                && eventData.delta.x > -SlidingDistance && eventData.delta.x < SlidingDistance
                && eventData.delta.y > -SlidingDistance && eventData.delta.y < SlidingDistance)
                return;
                */

            IsPressDown = false;
            IsLongPressDown = false;

            m_OnLongPressExit?.Invoke();

            //Debug.LogWarning("鼠标退出");
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (null != onClick
                && Time.time - PressDownStartTime <= LongPressInterval)
                onClick.Invoke();

            //Debug.LogWarning("点击事件");
        }
    }
}
