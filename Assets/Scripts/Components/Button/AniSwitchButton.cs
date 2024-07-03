using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/********************************************************************************
 * 说    明: 切换按钮,点击后自动播放切换动画
 * 版    本: 1.0
 * 创建时间: 2021.10.18
 * 作    者: zhoumingfeng
 * 修改记录:
 * 
 ********************************************************************************/
namespace Chanto
{
    public class AniSwitchButton : Button
    {
        public Animator ani;

        public bool isOn = false;

        // 点击CD时间间隔(秒);
        public float clickCD = 1.0f;

        // 最后一次的点击时间;
        private float lastClickTime = 0.0f;

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (clickCD > 0.0f)
            {
                if (Time.time - lastClickTime > clickCD)
                {
                    lastClickTime = Time.time;

                    this.Checked(!this.isOn);

                    base.OnPointerClick(eventData);
                }
            }
            else
            {
                this.Checked(!this.isOn);

                base.OnPointerClick(eventData);
            }
        }

        public virtual void Checked(bool check)
        {
            this.isOn = check;

            if(null != this.ani)
                ani.SetTrigger(this.isOn ? "on" : "off");
        }

        public bool IsOn()
        {
            return this.isOn;
        }

        public void SetState(bool _isOn)
        {
            this.isOn = _isOn;
        }

        public void ResetState()
        {
            ani.ResetTrigger("on");
            ani.ResetTrigger("off");
            ani.SetTrigger("init");
        }
    }
}
