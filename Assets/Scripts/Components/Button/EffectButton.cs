/**********************************************************************************************
 * @说    明: 特效按钮,点击时播放特效
 * @作    者: zhoumingfeng
 * @版 本 号: V1.00
 * @创建时间: 2023.9.1
 **********************************************************************************************/

namespace Chanto
{
    public class EffectButton : CDButton
    {
        public UIExtend Effect;

        protected override void ClickTriggerEvent()
        {
            this.Effect.Reset();
        }
    }
}
