﻿using Chanto;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Framework.UI
{
    /// <summary>
    /// 界面组件。
    /// </summary>
    [DisallowMultipleComponent]
    public class UIComponent : FrameworkComponent
    {
        public const int DefaultPriority = 99;

        [SerializeField]
        private float m_InstanceAutoReleaseInterval = 60f;

        [SerializeField]
        private int m_InstanceCapacity = 2;

        [SerializeField]
        private float m_InstanceExpireTime = 10f;

        [SerializeField]
        private int m_InstancePriority = 0;

        [SerializeField]
        private Transform m_InstanceRoot = null;

        [SerializeField] private Camera m_UICamera = null;

        [SerializeField]
        private UIGroupNode[] m_UIGroups = null;

        [SerializeField]
        private Material m_GrayMaterial = null;

        public Material grayMaterial => m_GrayMaterial;

        private int lastScrWidth, lastScrHeight;
        private Canvas m_RootCanvas;
        private CanvasScaler m_RootScaler;
        public Camera GetCamera() { return m_UICamera; }
        /// <summary>
        /// 获取界面组数量。
        /// </summary>
        public int UIGroupCount
        {
            get
            {
                return UIManager.Instance.UIGroupCount;
            }
        }

        /// <summary>
        /// 获取或设置界面实例对象池自动释放可释放对象的间隔秒数。
        /// </summary>
        public float InstanceAutoReleaseInterval
        {
            get
            {
                return UIManager.Instance.InstanceAutoReleaseInterval;
            }
            set
            {
                UIManager.Instance.InstanceAutoReleaseInterval = m_InstanceAutoReleaseInterval = value;
            }
        }

        /// <summary>
        /// 获取或设置界面实例对象池的容量。
        /// </summary>
        public int InstanceCapacity
        {
            get
            {
                return UIManager.Instance.InstanceCapacity;
            }
            set
            {
                UIManager.Instance.InstanceCapacity = m_InstanceCapacity = value;
            }
        }

        /// <summary>
        /// 获取或设置界面实例对象池对象过期秒数。
        /// </summary>
        public float InstanceExpireTime
        {
            get
            {
                return UIManager.Instance.InstanceExpireTime;
            }
            set
            {
                UIManager.Instance.InstanceExpireTime = m_InstanceExpireTime = value;
            }
        }

        /// <summary>
        /// 获取或设置界面实例对象池的优先级。
        /// </summary>
        public int InstancePriority
        {
            get
            {
                return UIManager.Instance.InstancePriority;
            }
            set
            {
                UIManager.Instance.InstancePriority = m_InstancePriority = value;
            }
        }

        /// <summary>
        /// 根节点
        /// </summary>
        public Transform InstanceRoot
        {
            get { return m_InstanceRoot; }
        }

        private void Update()
        {
            if (lastScrWidth != Screen.width || lastScrHeight != Screen.height)
            {
                OnResolutionChanged(Screen.width, Screen.height);
                lastScrWidth = Screen.width;
                lastScrHeight = Screen.height;
            }
        }

        private void OnResolutionChanged(int width, int height)
        {
            // m_UICamera.orthographicSize = height / 2.0f;
            m_UICamera.transform.position = new Vector3(width / 2.0f, height / 2.0f, 0);
        }

        /// <summary>
        /// 游戏框架组件初始化。
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            UIManager.Instance.OpenUIFormSuccess += OnOpenUIFormSuccess;
            UIManager.Instance.OpenUIFormFailure += OnOpenUIFormFailure;
            UIManager.Instance.OpenUIFormUpdate += OnOpenUIFormUpdate;
            UIManager.Instance.OpenUIFormDependencyAsset += OnOpenUIFormDependencyAsset;
            UIManager.Instance.CloseUIFormComplete += OnCloseUIFormComplete;

            m_RootCanvas = InstanceRoot.GetComponent<Canvas>();
            m_RootScaler = InstanceRoot.GetComponent<CanvasScaler>();

            m_UICamera.depth = 999;
            m_UICamera.orthographic = true;
            OnResolutionChanged(Screen.width, Screen.height);
            lastScrWidth = Screen.width;
            lastScrHeight = Screen.height;
        }

        private void Start()
        {
            BaseComponent baseComponent = GameEntry.GetComponent<BaseComponent>();
            if (baseComponent == null)
            {
                Log.Info("Base component is invalid.");
                return;
            }

            UIManager.Instance.CreateInstancePool();
            UIManager.Instance.InstanceAutoReleaseInterval = m_InstanceAutoReleaseInterval;
            UIManager.Instance.InstanceCapacity = m_InstanceCapacity;
            UIManager.Instance.InstanceExpireTime = m_InstanceExpireTime;
            UIManager.Instance.InstancePriority = m_InstancePriority;

            if (m_InstanceRoot == null)
            {
                m_InstanceRoot = new GameObject("UI Form Instances").transform;
                m_InstanceRoot.SetParent(gameObject.transform);
                m_InstanceRoot.localScale = Vector3.one;
            }

            m_InstanceRoot.gameObject.layer = LayerMask.NameToLayer("UI");

            for (int i = 0; i < m_UIGroups.Length; i++)
            {
                if (!AddUIGroup(m_UIGroups[i].Name, m_UIGroups[i].Depth))
                {
                    Log.Warning($"Add UI group '{m_UIGroups[i].Name}' failure.");
                    continue;
                }
            }
        }

        private void OnDestroy()
        {
            UIManager.Instance.OpenUIFormSuccess -= OnOpenUIFormSuccess;
            UIManager.Instance.OpenUIFormFailure -= OnOpenUIFormFailure;
            UIManager.Instance.OpenUIFormUpdate -= OnOpenUIFormUpdate;
            UIManager.Instance.OpenUIFormDependencyAsset -= OnOpenUIFormDependencyAsset;
            UIManager.Instance.CloseUIFormComplete -= OnCloseUIFormComplete;
        }

        /// <summary>
        /// 是否存在界面组。
        /// </summary>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <returns>是否存在界面组。</returns>
        public bool HasUIGroup(string uiGroupName)
        {
            return UIManager.Instance.HasUIGroup(uiGroupName);
        }

        /// <summary>
        /// 获取界面组。
        /// </summary>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <returns>要获取的界面组。</returns>
        public UIGroup GetUIGroup(string uiGroupName)
        {
            return UIManager.Instance.GetUIGroup(uiGroupName);
        }

        /// <summary>
        /// 获取所有界面组。
        /// </summary>
        /// <returns>所有界面组。</returns>
        public UIGroup[] GetAllUIGroups()
        {
            return UIManager.Instance.GetAllUIGroups();
        }

        /// <summary>
        /// 获取所有界面组。
        /// </summary>
        /// <param name="results">所有界面组。</param>
        public void GetAllUIGroups(List<UIGroup> results)
        {
            UIManager.Instance.GetAllUIGroups(results);
        }

        /// <summary>
        /// 增加界面组。
        /// </summary>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <returns>是否增加界面组成功。</returns>
        public bool AddUIGroup(string uiGroupName)
        {
            return AddUIGroup(uiGroupName, 0);
        }

        /// <summary>
        /// 增加界面组。
        /// </summary>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="depth">界面组深度。</param>
        /// <returns>是否增加界面组成功。</returns>
        public bool AddUIGroup(string uiGroupName, int depth)
        {
            if (UIManager.Instance.HasUIGroup(uiGroupName))
            {
                return false;
            }

            return UIManager.Instance.AddUIGroup(uiGroupName, depth, m_InstanceRoot);
        }

        /// <summary>
        /// 是否存在界面。
        /// </summary>
        /// <param name="serialId">界面序列编号。</param>
        /// <returns>是否存在界面。</returns>
        public bool HasUIForm(int serialId)
        {
            return UIManager.Instance.HasUIForm(serialId);
        }

        /// <summary>
        /// 是否存在界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <returns>是否存在界面。</returns>
        public bool HasUIForm(string uiFormAssetName)
        {
            return UIManager.Instance.HasUIForm(uiFormAssetName);
        }

        /// <summary>
        /// 获取界面。
        /// </summary>
        /// <param name="serialId">界面序列编号。</param>
        /// <returns>要获取的界面。</returns>
        public UIForm GetUIForm(int serialId)
        {
            return UIManager.Instance.GetUIForm(serialId);
        }

        /// <summary>
        /// 获取界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <returns>要获取的界面。</returns>
        public UIForm GetUIForm(string uiFormAssetName)
        {
            return UIManager.Instance.GetUIForm(uiFormAssetName);
        }

        /// <summary>
        /// 获取界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <returns>要获取的界面。</returns>
        public UIForm[] GetUIForms(string uiFormAssetName)
        {
            UIForm[] uiForms = UIManager.Instance.GetUIForms(uiFormAssetName);
            UIForm[] uiFormImpls = new UIForm[uiForms.Length];
            for (int i = 0; i < uiForms.Length; i++)
            {
                uiFormImpls[i] = uiForms[i];
            }

            return uiFormImpls;
        }

        public UIForm GetPopupLastUIForm()
        {
            var uiGroup = GetUIGroup(Constant.UIGroup.Dialog);
            if (null != uiGroup)
            {
                return uiGroup.CurrentUIForm;
            }
            else
            {
                uiGroup = GetUIGroup(Constant.UIGroup.Default);
                if (null != uiGroup)
                {
                    return uiGroup.CurrentUIForm;
                }
            }

            return null;
        }

        public void ClosePopupLastUIForm()
        {
            var uiForm = GetPopupLastUIForm();
            CloseUIForm(uiForm.SerialId);
        }

        /// <summary>
        /// 是否正在加载界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <returns>是否正在加载界面。</returns>
        public bool IsLoadingUIForm(string uiFormAssetName)
        {
            return UIManager.Instance.IsLoadingUIForm(uiFormAssetName);
        }

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <returns>界面的序列编号。</returns>
        public int OpenUIForm(string uiFormAssetName, string uiGroupName = Constant.UIGroup.Default, Action<UIForm> openCallback = null)
        {
            return OpenUIForm(uiFormAssetName, uiGroupName, DefaultPriority, false, null, openCallback);
        }

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <returns>界面的序列编号。</returns>
        public int OpenUIForm(string uiFormAssetName, object userData, Action<UIForm> openCallback = null)
        {
            return OpenUIForm(uiFormAssetName, Constant.UIGroup.Default, DefaultPriority, false, userData, openCallback);
        }

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>界面的序列编号。</returns>
        public int OpenUIForm(string uiFormAssetName, string uiGroupName, object userData, Action<UIForm> openCallback = null)
        {
            return OpenUIForm(uiFormAssetName, uiGroupName, DefaultPriority, false, userData, openCallback);
        }

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>界面的序列编号。</returns>
        public int OpenUIForm(string uiFormAssetName, string uiGroupName, bool pauseCoveredUIForm, object userData, Action<UIForm> openCallback = null)
        {
            return OpenUIForm(uiFormAssetName, uiGroupName, DefaultPriority, pauseCoveredUIForm, userData, openCallback);
        }

        /// <summary>
        /// 打开界面。F
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="priority">加载界面资源的优先级。</param>
        /// <param name="pauseCoveredUIForm">是否暂停被覆盖的界面。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>界面的序列编号。</returns>
        private int OpenUIForm(string uiFormAssetName, string uiGroupName, int priority, bool pauseCoveredUIForm, object userData, Action<UIForm> openCallback)
        {
            return UIManager.Instance.OpenUIForm(uiFormAssetName, uiGroupName, false, priority, pauseCoveredUIForm, userData, openCallback);
        }
        public int OpenUIForm(string uiFormAssetName, string uiGroupName, bool isMultiplenInstance, int priority, bool pauseCoveredUIForm, object userData, Action<UIForm> openCallback)
        {
            return UIManager.Instance.OpenUIForm(uiFormAssetName, uiGroupName, isMultiplenInstance, priority, pauseCoveredUIForm, userData, openCallback);
        }
        /// <summary>
        /// 关闭界面。
        /// </summary>
        /// <param name="serialId">要关闭界面的序列编号。</param>
        public void CloseUIForm(int serialId)
        {
            UIManager.Instance.CloseUIForm(serialId);
        }

        /// <summary>
        /// 关闭界面。
        /// </summary>
        /// <param name="panelType">要关闭界面的类型。</param>
        public void CloseUIForm(string uiFormAssetName)
        {
            var form = GetUIForm(uiFormAssetName);
            if (form == null)
                return;

            CloseUIForm(form);
        }

        /// <summary>
        /// 关闭界面。
        /// </summary>
        /// <param name="uiForm">要关闭的界面。</param>
        public void CloseUIForm(UIForm uiForm)
        {
            if (null == uiForm) return;

            CloseUIForm(uiForm.SerialId);
        }

        /// <summary>
        /// 关闭某组所有的界面
        /// </summary>
        /// <param name="groupName">Group name.</param>
        /// <param name="filter">过滤哪些form Filter.</param>
        public void CloseByGroup(string groupName, List<string> filter = null)
        {
            UIGroup[] groups = GetAllUIGroups();
            for (int index = 0; index < groups.Length; index++)
            {
                UIGroup group = groups[index];
                if (group.Name == groupName)
                {
                    UIForm[] uIForms = group.GetAllUIForms();
                    for (int i = 0; i < uIForms.Length; i++)
                    {
                        CloseUIForm(uIForms[i].SerialId);
                    }
                }
            }
        }

        /// <summary>
        /// 关闭所有打开界面
        /// </summary>
        public void ClosePopUpGroup()
        {
            CloseByGroup(Constant.UIGroup.Default);
            CloseByGroup(Constant.UIGroup.Dialog);
        }

        /// <summary>
        /// 是否有打开界面
        /// </summary>
        public bool IsHavePopUpUIForm()
        {
            var count = GetGroupUIFormCount(Constant.UIGroup.Default);
            if (count > 0)
            {
                return true;
            }

            count = GetGroupUIFormCount(Constant.UIGroup.Default);
            return count > 0;
        }

        public int GetGroupUIFormCount(string groupName)
        {
            var uiGroup = GetUIGroup(groupName);
            if (null == uiGroup)
            {
                Log.Error($"不存在这个UI组:{groupName}");
                return 0;
            }

            return uiGroup.UIFormCount;
        }

        public void CloseAll()
        {
            CloseByGroup(Constant.UIGroup.Scene3D);
            CloseByGroup(Constant.UIGroup.Scene);
            CloseByGroup(Constant.UIGroup.Default);
            CloseByGroup(Constant.UIGroup.Dialog);
            CloseByGroup(Constant.UIGroup.Global);
            CloseByGroup(Constant.UIGroup.Loading);
        }

        #region UI事件

        /// <summary>
        /// 打开界面成功事件。
        /// </summary>
        public event EventHandler<OpenUIFormSuccessEventArgs> OpenUIFormSuccess
        {
            add { UIManager.Instance.OpenUIFormSuccess += value; }
            remove { UIManager.Instance.OpenUIFormSuccess -= value; }
        }

        /// <summary>
        /// 打开界面失败事件。
        /// </summary>
        public event EventHandler<OpenUIFormFailureEventArgs> OpenUIFormFailure
        {
            add { UIManager.Instance.OpenUIFormFailure += value; }
            remove { UIManager.Instance.OpenUIFormFailure -= value; }
        }

        /// <summary>
        /// 打开界面更新事件。
        /// </summary>
        public event EventHandler<OpenUIFormUpdateEventArgs> OpenUIFormUpdate
        {
            add { UIManager.Instance.OpenUIFormUpdate += value; }
            remove { UIManager.Instance.OpenUIFormUpdate -= value; }
        }

        /// <summary>
        /// 打开界面时加载依赖资源事件。
        /// </summary>
        public event EventHandler<OpenUIFormDependencyAssetEventArgs> OpenUIFormDependencyAsset
        {
            add { UIManager.Instance.OpenUIFormDependencyAsset += value; }
            remove { UIManager.Instance.OpenUIFormDependencyAsset -= value; }
        }

        /// <summary>
        /// 关闭界面完成事件。
        /// </summary>
        public event EventHandler<CloseUIFormCompleteEventArgs> CloseUIFormComplete
        {
            add { UIManager.Instance.CloseUIFormComplete += value; }
            remove { UIManager.Instance.CloseUIFormComplete -= value; }
        }

        private void OnOpenUIFormSuccess(object sender, OpenUIFormSuccessEventArgs e)
        {

        }

        private void OnOpenUIFormFailure(object sender, OpenUIFormFailureEventArgs e)
        {
            //Log.WarningFormat("Open UI form failure, asset name '{0}', UI group name '{1}', pause covered UI form '{2}', error message '{3}'.", e.UIFormAssetName, e.UIGroupName, e.PauseCoveredUIForm.ToString(), e.ErrorMessage);

        }

        private void OnOpenUIFormUpdate(object sender, OpenUIFormUpdateEventArgs e)
        {

        }

        private void OnOpenUIFormDependencyAsset(object sender, OpenUIFormDependencyAssetEventArgs e)
        {

        }

        private void OnCloseUIFormComplete(object sender, CloseUIFormCompleteEventArgs e)
        {

        }

        #endregion

        public RenderTexture TextureToRenderTexture(Texture texture)
        {
            RenderTexture renderTexture = RenderTexture.GetTemporary(texture.width, texture.height, 32);
            Graphics.Blit(texture, renderTexture);
            return renderTexture;
        }

        public CanvasScaler ContainerCanvasScaler => m_RootScaler;

        public UIForm[] GetAllLoadedUIForms()
        {
            return UIManager.Instance.GetAllLoadedUIForms();
        }
    }
}
