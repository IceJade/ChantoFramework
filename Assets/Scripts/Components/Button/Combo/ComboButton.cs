/**********************************************************************************************
 * @说    明: 组合列表按钮,点击一次展开列表,再次点击收起列表
 * @作    者: zhoumingfeng
 * @版 本 号: V1.00
 * @创建时间: 2023.2.10
 **********************************************************************************************/

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Chanto
{
    public class ComboButton : MonoBehaviour
    {
        public Button btnCombo;
        public Button btnCloseList;

        public Image imgArrow;
        public IMImage imgIcon;

        public IMTextMeshProUGUI txtTitle;

        public RectTransform rectExtend;

        public UIMultiScroller scroller;

        [Header("勾选为下拉列表,否则为上拉列表")]
        public bool IsDownList = true;

        private Vector3 _arrow_init_vector = new Vector3(0, 0, 180);

        private UnityAction<ComboItem> _click_item_callback = null;
        private UnityAction<ComboItem> _visit_item_callback = null;

        protected virtual void Awake()
        {
            btnCombo.onClick.RemoveAllListeners();
            btnCombo.onClick.AddListener(() =>
            {
                this.SetState(!rectExtend.gameObject.activeSelf);
            });

            btnCloseList.onClick.RemoveAllListeners();
            btnCloseList.onClick.AddListener(() =>
            {
                this.SetState(false);
            });
        }

        /// <summary>
        /// 初始化下拉列表
        /// </summary>
        /// <param name="total">下拉列表的总项数</param>
        /// <param name="click_item_callback">点击下拉列表中的某一项的回调</param>
        /// <param name="visit_item_callback">初始化下拉列表的项的回调</param>
        /// <param name="default_check_index">默认选中第几项</param>
        public void Init(int total, UnityAction<ComboItem> click_item_callback, UnityAction<ComboItem> visit_item_callback = null, int default_check_index = 0 )
        {
            this._click_item_callback = click_item_callback;
            this._visit_item_callback = visit_item_callback;

            this.SetState(false);

            this.InitItems(total, default_check_index);

            this.InitTitle(default_check_index);
        }

        protected void InitItems(int item_total, int check_index)
        {
            this.scroller.DataCount = item_total;
            this.scroller.OnItemCreate = OnItemCreate;
            this.scroller.ResetScroller(check_index, true);
        }

        protected void InitTitle(int index)
        {
            var objItem = this.scroller.GetItemByIndex(index);
            if (null == objItem)
                return;

            var item = objItem.GetComponent<ComboItem>();
            if (null == item)
                return;

            if (null != this.txtTitle && null != item)
                this.txtTitle.SetText(item.GetTitle());

            if (null != this.imgIcon && null != item)
                this.imgIcon.LoadAsyncEx(item.GetIcon());
        }


        protected virtual void OnItemCreate(int index, GameObject obj)
        {
            var item = obj.GetComponent<ComboItem>();
            if (null != item)
            {
                item.Init(index, ClickComboItemCallback);
                this._visit_item_callback?.Invoke(item);
            }
        }

        public void ClickComboItemCallback(ComboItem item)
        {
            this.SetState(false);

            if(null != this.txtTitle)
                this.txtTitle.SetText(item.GetTitle());

            this._click_item_callback?.Invoke(item);
        }

        public void SetState(bool is_extend)
        {
            this.rectExtend.gameObject.SetActiveEx(is_extend);

            Vector3 angle = Vector3.zero;

            if(this.IsDownList)
                angle = is_extend ? _arrow_init_vector : Vector3.zero;
            else
                angle = is_extend ? Vector3.zero : _arrow_init_vector;

            this.imgArrow.transform.localEulerAngles = angle;
        }
    }
}
