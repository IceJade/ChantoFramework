using System.Collections.Generic;
using UnityEngine;

/********************************************************************************
 * 说    明: 切换按钮组管理器,根据点击事件自动管理内部ToggleButton的状态
 * 版    本: 1.0
 * 创建时间: 2021.06.09
 * 作    者: zhoumingfeng
 * 修改记录:
 * 
 ********************************************************************************/
namespace Chanto
{
    public class ToggleButtonGroup : MonoBehaviour
    {
        private List<ToggleButton> btnToggleList = new List<ToggleButton>(5);

        private void OnDestroy()
        {
            btnToggleList.Clear();
        }

        public void Register(ToggleButton button)
        {
            btnToggleList.Add(button);

            this.RefreshToggleState();
        }

        /// <summary>
        /// 通知刷新组按钮的状态
        /// </summary>
        /// <param name="toggle"></param>
        public void NotifyToggleOn(ToggleButton toggle)
        {
#if UNITY_EDITOR
            ValidateToggleIsInGroup(toggle);
#endif

            // disable all toggles in the group
            for (var i = 0; i < btnToggleList.Count; i++)
            {
                if (btnToggleList[i] == toggle)
                    continue;

                btnToggleList[i].RefreshState(ToggleButton.E_State.UnChecked);
            }
        }

        /// <summary>
        /// 编辑器模式下如果配置错误,提示错误信息
        /// </summary>
        /// <param name="toggle"></param>
        private void ValidateToggleIsInGroup(ToggleButton toggle)
        {
            if (toggle == null || !btnToggleList.Contains(toggle))
                Log.Error($"Toggle {toggle.name} is not part of ToggleGroup {this.name}");
        }

        /// <summary>
        /// 刷新内部按钮状态
        /// </summary>
        private void RefreshToggleState()
        {
            foreach(var item in this.btnToggleList)
            {
                if(null != item && item.GetState() == ToggleButton.E_State.Checked)
                {
                    this.NotifyToggleOn(item);
                    break;
                }
            }
        }

        /// <summary>
        /// 是否包含某个按钮
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool Contains(ToggleButton button)
        {
            if (null == btnToggleList 
                || btnToggleList.Count <= 0)
                return false;

            return btnToggleList.Contains(button);
        }
    }
}

