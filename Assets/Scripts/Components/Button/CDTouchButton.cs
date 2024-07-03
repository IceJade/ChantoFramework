/**********************************************************************************************
 * @说    明: 相比CDButton 增加了onTouchBegan接口 对齐cocos的onTouBegan
 * @作    者: wuzi
 * @版 本 号: V1.00
 * @创建时间: 2024.6.15
 **********************************************************************************************/
using UnityEngine;
using UnityEngine.EventSystems;


namespace Chanto
{
    public class CDTouchButton : CDButton
    {
        // 最后一次的触摸时间
        protected float LastTouchTime = 0.0f;

        private ButtonClickedEvent m_OnTouchBegan = new ButtonClickedEvent();

        public ButtonClickedEvent onTouchBegan
        {
            get { return m_OnTouchBegan; }
            set { m_OnTouchBegan = value; }
        }

        public override void OnPointerDown(PointerEventData eventData) {
            if (ClickCD > 0.0f) {
                if (Time.time - LastTouchTime > ClickCD) {
                    LastTouchTime = Time.time;
                    base.OnPointerDown(eventData);
                    m_OnTouchBegan?.Invoke();
                }
            } else {
                base.OnPointerDown(eventData);
                m_OnTouchBegan?.Invoke();
            }
        }
    }
}
