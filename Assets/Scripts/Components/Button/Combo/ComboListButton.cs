/**********************************************************************************************
 * @说    明: 组合列表按钮,点击一次展开列表,再次点击收起列表
 * @作    者: zhoumingfeng
 * @版 本 号: V1.00
 * @创建时间: 2022.7.13
 **********************************************************************************************/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Chanto
{
    public class ComboListButton : MonoBehaviour
    {
        public Button btnArrow;
        public Image imgArrow;
        public IMTextMeshProUGUI txtTitle;

        public RectTransform rectCombo;
        public RectTransform rectExtend;

        public List<ComboListItem> comboItemList = new List<ComboListItem>(5);

        [Header("边缘高度")]
        public float EdgeHeight = 0.0f;
        [Header("Item间隔")]
        public float ItemSpace = 3.0f;

        private UnityAction<ComboListItem> _callback = null;

        private void Awake()
        {
            btnArrow.onClick.RemoveAllListeners();
            btnArrow.onClick.AddListener(() =>
            {
                rectExtend.gameObject.SetActiveEx(!rectExtend.gameObject.activeSelf);

                this.SetState(rectExtend.gameObject.activeSelf);
            });

            if (null == comboItemList)
                return;

            for(int i = 0; i < comboItemList.Count; i++)
            {
                if (null != comboItemList[i])
                    comboItemList[i].Init(i, ClickComboItemCallback);
            }
        }

        /// <summary>
        /// 初始化下拉列表
        /// </summary>
        /// <param name="select_index">默认显示第几项</param>
        /// <param name="item_total">下拉列表的总项数</param>
        /// <param name="click_callback">点击下拉列表的回调</param>
        public void Init(int select_index, int item_total, UnityAction<ComboListItem> click_callback)
        {
            this.InitUI(item_total);
            this.SetState(false);
            this.RefreshItemState(select_index);

            this._callback = click_callback;
        }

        private void InitUI(int item_total)
        {
            for (int i = 0; i < comboItemList.Count; i++)
            {
                if (null != comboItemList[i])
                    comboItemList[i].gameObject.SetActiveEx(i < item_total);
            }

            var size = rectExtend.sizeDelta;
            size.y = (comboItemList[0].GetHeight() + ItemSpace) * item_total - ItemSpace + EdgeHeight;
            rectExtend.sizeDelta = size;

            //var corners = new Vector3[4];
            //rectCombo.GetWorldCorners(corners);

            //var postion = rectExtend.position;
            //postion.y = corners[0].y + 0.2f;
            //rectExtend.position = postion;
        }

        public void ClickComboItemCallback(ComboListItem item)
        {
            this.SetState(false);
            this.RefreshItemState(item);

            this.txtTitle.SetText(item.GetTitle());

            this._callback?.Invoke(item);
        }

        public void SetState(bool isExtand)
        {
            this.rectExtend.gameObject.SetActiveEx(isExtand);

            this.imgArrow.transform.localEulerAngles = isExtand ? new Vector3(0,0,180) : Vector3.zero;
        }

        private void RefreshItemState(int index)
        {
            for (int i = 0; i < comboItemList.Count; i++)
                comboItemList[i].SetState(index == i ? ComboListItem.E_State.Checked : ComboListItem.E_State.UnChecked);

            if (index < comboItemList.Count)
                this.txtTitle.SetText(comboItemList[index].GetTitle());
        }

        private void RefreshItemState(ComboListItem item)
        {
            for (int i = 0; i < comboItemList.Count; i++)
            {
                if (item != comboItemList[i])
                    comboItemList[i].SetState(ComboListItem.E_State.UnChecked);
                else
                    comboItemList[i].SetState(ComboListItem.E_State.Checked);
            }
        }
    }
}
