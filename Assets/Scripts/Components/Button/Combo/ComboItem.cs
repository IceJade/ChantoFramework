using UnityEngine.Events;
using UnityEngine.UI;

namespace Chanto
{
    public class ComboItem : UIMultiScrollIndex
    {
        public Button btnItem;
        public IMTextMeshProUGUI txtTitle;
        public IMImage imgIcon;

        private string _icon_name;

        public int IntParam { get; set; } = -1;
        public string StringParam { get; set; } = string.Empty;

        protected UnityAction<ComboItem> _callback = null;

        protected virtual void Start()
        {
            btnItem.onClick.RemoveAllListeners();
            btnItem.onClick.AddListener(() =>
            {
                this.CheckedAndNotify();

                _callback?.Invoke(this);
            });
        }

        public void Init(int index, UnityAction<ComboItem> callback)
        {
            this.Index = index;
            this._callback = callback;
        }

        public void SetTitle(string title)
        {
            if (null != this.txtTitle)
                this.txtTitle.SetTextEx(title);
        }

        public void SetTitleId(int lang_id)
        {
            this.SetTitleId(lang_id.ToString());
        }

        public void SetTitleId(string lang_id)
        {
            if (null != this.txtTitle)
                this.txtTitle.SetDialogId(lang_id);
        }

        public string GetTitle()
        {
            if (null != this.txtTitle)
                return this.txtTitle.text;

            return string.Empty;
        }

        public void SetIcon(string atlas, string icon)
        {
            this._icon_name = icon;

            if(null != this.imgIcon)
                this.imgIcon.LoadAsyncWithAtlas(atlas, icon);
        }

        public string GetIcon()
        {
            return this._icon_name;
        }

        public int GetIndex()
        {
            return this.Index;
        }
    }
}
