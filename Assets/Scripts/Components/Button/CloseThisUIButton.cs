/**********************************************************************************************
 * @说    明: 关闭本UI的按钮,点击直接关闭当前打开的UI
 * @作    者: zhoumingfeng
 * @版 本 号: V1.00
 * @创建时间: 2023.01.16
 **********************************************************************************************/
using Framework;
using Framework.UI;
using UnityEngine.EventSystems;

namespace Chanto
{
    public class CloseThisUIButton : BaseButton
    {
        private UIFormLogic _form;

        // 向上最大寻找层级的数量, 防止死循环
        private int _loop_max_count = 100;

        public override void OnPointerClick(PointerEventData eventData)
        {
            if(null == this._form)
            {
                int count = 0;
                var parentTransform = this.transform.parent;
                while(null != parentTransform && count < this._loop_max_count)
                {
                    var tmpForm = parentTransform.GetComponent<UIFormLogic>();
                    if(null != tmpForm && null != tmpForm.UIForm)
                    {
                        this._form = tmpForm;
                        this._form.CloseSelf();

                        //GameEntry.Sound.PlayEffect(GameDefines.SoundAssets.Music_Effect_UI_BUTTON_RETURN);

                        break;
                    }
                    else
                    {
                        parentTransform = parentTransform.parent;
                    }

                    count++;
                }

#if UNITY_EDITOR
                if (null == parentTransform)
                    Log.Error("UI有问题,向上没有查找到继承自PopupBaseView的节点,请检查!");

                if (count >= this._loop_max_count)
                    Log.Error("向上查找了100个父节点,没有找到继承自PopupBaseView的节点,请检查!");
#endif
            }
            else
            {
                this._form.CloseSelf();
            }

            base.OnPointerClick(eventData);
        }
    }
}
