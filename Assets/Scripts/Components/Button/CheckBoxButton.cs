﻿using UnityEngine;
using UnityEngine.EventSystems;

/********************************************************************************
 * 说    明: CheckBox按钮
 * 版    本: 1.0
 * 创建时间: 2021.06.10
 * 作    者: zhoumingfeng
 * 修改记录:
 * 
 ********************************************************************************/
namespace Chanto
{
    public class CheckBoxButton : BaseButton
    {
        // 选中时显示的节点
        public GameObject CheckedNode;

        // 当前是否选中
        public bool IsOn = false;

        protected override void Awake()
        {
            this.Checked(this.IsOn);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            this.Checked(!this.IsOn);

            base.OnPointerClick(eventData);
        }

        public virtual void Checked(bool check)
        {
            this.IsOn = check;

            CheckedNode.SetActiveEx(check);
        }

        public bool IsChecked()
        {
            return this.IsOn;
        }

        public void SetActive(bool show)
        {
            this.gameObject.SetActiveEx(show);
        }
    }
}
