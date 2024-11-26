using Chanto;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Framework.UI
{
    /// <summary>
    /// 界面逻辑基类。
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(GraphicRaycaster))]
    [RequireComponent(typeof(UIForm))]
    public abstract class UIFormLogic : BaseMono
    {
        public const int DepthFactor = 100;

        private int m_OriginalLayer = 0;
        private string uiKey = string.Empty;

        public Canvas canvas { get; private set; }
        public CanvasGroup canvasGroup { get; private set; }
        public RectTransform recrTransform { get; private set; }

        protected Action FormPartOnpe;
        protected Action<float, float> FormPartUpdate;
        protected Action FormPartClose;

        //[SerializeField]
        //private readonly List<BaseUIItem> children = new List<BaseUIItem>();

        /// <summary>
        /// 获取界面。
        /// </summary>
        public UIForm UIForm
        {
            get
            {
                return GetComponent<UIForm>();
            }
        }

        /// <summary>
        /// 获取或设置界面名称。
        /// </summary>
        public string Name
        {
            get
            {
                return gameObject.name;
            }
            set
            {
                gameObject.name = value;
            }
        }

        /// <summary>
        /// 获取界面是否可用。
        /// </summary>
        public bool IsAvailable
        {
            get
            {
                return gameObject.activeSelf;
            }
        }

        /// <summary>
        /// 获取已缓存的 Transform。
        /// </summary>
        public Transform CachedTransform
        {
            get;
            private set;
        }

        //public List<BaseUIItem> Children => children;

        //public void AddChildItem(BaseUIItem item)
        //{
        //    children.Add(item);
        //}

        /// <summary>
        /// 界面初始化。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        protected internal virtual void InternalOnInit(object userData)
        {
            if (CachedTransform == null)
            {
                CachedTransform = transform;
            }

            m_OriginalLayer = gameObject.layer;

            canvasGroup = gameObject.GetOrAddComponent<CanvasGroup>();
            canvasGroup.alpha = 1;

            canvas = gameObject.GetOrAddComponent<Canvas>();
            canvas.sortingOrder = 0;
            canvas.overrideSorting = true;
            canvas.worldCamera = GameEntry.UI.GetCamera();

            gameObject.GetOrAddComponent<GraphicRaycaster>();

            recrTransform = GetComponent<RectTransform>();
        }

        /// <summary>
        /// 界面打开。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        protected internal virtual void InternalOnOpen(object userData)
        {
            ResetUIParams();

            gameObject.SetActive(true);
        }

        protected internal void ResetUIParams()
        {
            if (canvas != null)
            {
                recrTransform.localScale = Vector3.one;
                recrTransform.offsetMin = Vector3.zero;
                recrTransform.offsetMax = Vector3.zero;
                recrTransform.anchorMin = Vector2.zero;
                recrTransform.anchorMax = Vector2.one;
                recrTransform.pivot = new Vector2(0.5f, 0.5f);
                recrTransform.sizeDelta = Vector2.zero;
                recrTransform.anchoredPosition = Vector2.zero;
                recrTransform.SetAsLastSibling();

                int uiLayerIndex = LayerMask.NameToLayer("UI");
                if (gameObject.layer != uiLayerIndex)
                    gameObject.layer = uiLayerIndex;
            }
        }

        /// <summary>
        /// 界面关闭。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        protected internal virtual void InternalOnClose(object userData)
        {
            //while (children.Count > 0)
            //{
            //    BaseUIItem item = children[0];
            //    children.RemoveAt(0);

            //    if (item != null)
            //        item.Release();
            //}

            gameObject.SetLayerRecursively(m_OriginalLayer);
        }

        /// <summary>
        /// 界面暂停。
        /// </summary>
        protected internal virtual void OnPause()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 界面暂停恢复。
        /// </summary>
        protected internal virtual void OnResume()
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// 界面遮挡。
        /// </summary>
        protected internal virtual void OnCover()
        {

        }

        /// <summary>
        /// 界面遮挡恢复。
        /// </summary>
        protected internal virtual void OnReveal()
        {

        }

        /// <summary>
        /// 界面激活。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        protected internal virtual void OnRefocus(object userData)
        {

        }

        /// <summary>
        /// 界面轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        protected internal virtual void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {

        }

        /// <summary>
        /// 界面深度改变。
        /// </summary>
        /// <param name="uiGroupDepth">界面组深度。</param>
        /// <param name="depthInUIGroup">界面在界面组中的深度。</param>
        protected internal virtual void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
        {
            int deltaDepth = UIGroupMono.DepthFactor * uiGroupDepth + DepthFactor * depthInUIGroup;

            if (null == this.canvas)
                this.canvas = gameObject.GetOrAddComponent<Canvas>();

            this.canvas.sortingOrder = deltaDepth;

            var childUpdateOrders = this.GetComponentsInChildren<IForceUpdateOrder>(true);
            if (null != childUpdateOrders)
            {
                for (int i = 0; i < childUpdateOrders.Length; i++)
                    childUpdateOrders[i].UpdateSortingOrder(deltaDepth, "Default");
            }
        }

        protected internal virtual bool OnBack()
        {
            Log.Debug(name + " OnBack");
            return false;
        }

        public void AddUIPartOpen(Action handle)
        {
            if (handle != null)
            {
                FormPartOnpe -= handle;
                FormPartOnpe += handle;
            }
        }

        public void RemoveUIPartOpne(Action handle)
        {
            if (handle != null)
            {
                FormPartOnpe -= handle;
            }
        }

        public void AddUIPartClose(Action handle)
        {
            if (handle != null)
            {
                FormPartClose -= handle;
                FormPartClose += handle;
            }
        }

        public void RemoveUIPartClose(Action handle)
        {
            if (handle != null)
            {
                FormPartClose -= handle;
            }
        }


        public void AddUIPartUpdate(Action<float, float> handle)
        {
            if (handle != null)
            {
                FormPartUpdate -= handle;
                FormPartUpdate += handle;
            }
        }

        public void RemoveUIPartUpdate(Action<float, float> handle)
        {
            if (handle != null)
            {
                FormPartUpdate -= handle;
            }
        }

        public virtual void CloseSelf()
        {

        }

    }
}
