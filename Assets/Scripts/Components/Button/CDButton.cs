/**********************************************************************************************
 * @说    明: CD按钮,点击一次按钮后需要等待一段时间后点击事件才会生效, 防止狂点时逻辑处理不过来
 * @作    者: zhoumingfeng
 * @版 本 号: V1.00
 * @创建时间: 2021.7.29
 **********************************************************************************************/
using UnityEngine;
using UnityEngine.EventSystems;

namespace Chanto
{
    public class CDButton : BaseButton
    {
        private bool _highlighted;

        // 点击CD时间间隔(秒);
        public float ClickCD = 1.0f;

        // 最后一次的点击时间;
        protected float LastClickTime = 0.0f;

        public override void OnPointerClick(PointerEventData eventData)
        {
            if(ClickCD > 0.0f)
            {
                if(Time.time - LastClickTime > ClickCD)
                {
                    LastClickTime = Time.time;

                    this.ClickTriggerEvent();

                    base.OnPointerClick(eventData);
                }
            }
            else
            {
                this.ClickTriggerEvent();

                base.OnPointerClick(eventData);
            }
        }

        protected virtual void ClickTriggerEvent()
        {

        }

        public void setHighlighted(bool bHighlighted)
        {
            _highlighted = bHighlighted;
        }

        public bool isHighlighted(bool bHighlighted)
        {
            return _highlighted;
        }

        public void setBackgroundSprite(string imageName, bool isNeedNativeSize = false)
        {
            var background = this.GetComponent<IMImage>();
            if(null != background)
            {
                background.LoadAsyncEx(imageName);
            }
            else
            {
                background = this.GetComponentInChildren<IMImage>();
                if (null != background)
                    background.LoadAsyncEx(imageName);
            }
        }

        public void setPosition(Vector2 point)
        {
            this.transform.setPosition(point);
        }

        public void setPosition(float x, float y)
        {
            this.transform.setPosition(x, y);
        }
    }
}
