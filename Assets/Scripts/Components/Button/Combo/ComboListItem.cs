using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Chanto
{
    public class ComboListItem : MonoBehaviour
    {
        public enum E_State
        {
            UnChecked = 0,
            Checked
        }

        public Button btnItem;

        public IMTextMeshProUGUI txtTitle;
        public GameObject objSelected;
        public RectTransform rectItem;

        private int _index = 0;
        private UnityAction<ComboListItem> _callback = null;

        private void Start()
        {
            btnItem.onClick.RemoveAllListeners();
            btnItem.onClick.AddListener(() =>
            {
                this.SetState(E_State.Checked);

                _callback?.Invoke(this);
            });
        }

        public void Init(int index, UnityAction<ComboListItem> callback)
        {
            this._index = index;
            this._callback = callback;
        }

        public void SetState(E_State state)
        {
            if(null != this.objSelected)
                this.objSelected.SetActiveEx(state == E_State.Checked);
        }

        public void SetData(int index, string title, UnityAction<ComboListItem> callback)
        {
            this._index = index;
            this._callback = callback;

            this.SetTitle(title);
        }

        public void SetData(int index, int titleId, UnityAction<ComboListItem> callback)
        {
            this._index = index;
            this._callback = callback;

            this.SetTitleId(titleId);
        }

        public void SetTitleId(int titleId)
        {
            this.txtTitle.SetDialogId(titleId.ToString());
        }

        public void SetTitle(string title)
        {
            this.txtTitle.SetTextEx(title);
        }

        public string GetTitle()
        {
            if (null != this.txtTitle)
                return this.txtTitle.text;

            return string.Empty;
        }

        public int GetIndex()
        {
            return this._index;
        }

        public float GetHeight()
        {
            return rectItem.sizeDelta.y;
        }
    }
}
