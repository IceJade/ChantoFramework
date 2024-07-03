/**********************************************************************************************
 * @说    明: 所有业务按钮的基类
 * @作    者: zhoumingfeng
 * @版 本 号: V1.00
 * @创建时间: 2024.6.3
 **********************************************************************************************/
using UnityEngine.UI;

namespace Chanto
{
    public class BaseButton : Button
    {
        public int m_hintType;

        private int _tag = 0;
        public int getTag() { return _tag; }
        public void setTag(int tag) { _tag = tag; }

        public bool isVisible() { return this.gameObject.activeSelf; }
        public void setVisible(bool visible) { this.gameObject.SetActiveEx(visible); }

        public void setEnabled( bool isEnable)
        {
            //UIUtils.SetGray(this.transform, !isEnable, isEnable);
        }

        public void setEnabled2(bool isEnable)
        {
            //UIUtils.SetGray(this.transform, isEnable, isEnable);
        }
    }
}
