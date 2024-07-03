using UnityEngine;

/********************************************************************************
 * 说    明: 切换按钮,点击后自动切换
 * 版    本: 1.0
 * 创建时间: 2021.06.09
 * 作    者: zhoumingfeng
 * 修改记录:
 * 
 ********************************************************************************/
namespace Chanto
{
    public class SwitchButton : CheckBoxButton
    {
        // 不选中时显示的节点
        public GameObject UncheckNode;

        // 文本
        public IMTextMeshProUGUI NodeName;

        // 选中时显示的文本
        public string CheckLangId;

        // 未选中时显示的文本
        public string UncheckLangId;

        public override void Checked(bool check)
        {
            base.Checked(check);

            if(null != UncheckNode)
                UncheckNode.SetActiveEx(!check);

            if (null != NodeName
                && !string.IsNullOrEmpty(CheckLangId)
                && !string.IsNullOrEmpty(UncheckLangId))
            {
                NodeName.SetDialogId(check ? CheckLangId : UncheckLangId);
            }
        }
    }
}
