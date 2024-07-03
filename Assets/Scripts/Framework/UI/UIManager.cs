using Framework.Pool;
using Framework;
using Framework.Resource;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Framework.UI
{
	/// <summary>
	/// 界面管理器。
	/// </summary>
	public class UIManager : GameBaseSingletonModule<UIManager>
	{
		private readonly Dictionary<string, UIGroup> m_UIGroups;
		private readonly Dictionary<int, string> m_UIFormsBeingLoaded;
		private readonly HashSet<int> m_UIFormsToReleaseOnLoad;
		private readonly Queue<UIForm> m_RecycleQueue;
		private readonly LoadAssetCallbacks m_LoadAssetCallbacks;
		private ObjectPool<UIFormInstanceObject> m_InstancePool;
		private int m_Serial;
		private EventHandler<OpenUIFormSuccessEventArgs> m_OpenUIFormSuccessEventHandler;
		private EventHandler<OpenUIFormFailureEventArgs> m_OpenUIFormFailureEventHandler;
		private EventHandler<OpenUIFormUpdateEventArgs> m_OpenUIFormUpdateEventHandler;
		private EventHandler<OpenUIFormDependencyAssetEventArgs> m_OpenUIFormDependencyAssetEventHandler;
		private EventHandler<CloseUIFormCompleteEventArgs> m_CloseUIFormCompleteEventHandler;

		private readonly Stack<UIForm> m_UIFormOpenStack;
        private readonly Stack<UIForm> m_TempStack;


        /// <summary>
        /// 初始化界面管理器的新实例。
        /// </summary>
        public UIManager()
		{
			m_UIFormOpenStack = new Stack<UIForm>();
            m_TempStack = new Stack<UIForm>();

			m_UIGroups = new Dictionary<string, UIGroup>();
			m_UIFormsBeingLoaded = new();
			m_UIFormsToReleaseOnLoad = new HashSet<int>();
			m_RecycleQueue = new();
			m_LoadAssetCallbacks = new LoadAssetCallbacks(LoadUIFormSuccessCallback, LoadUIFormFailureCallback, LoadUIFormUpdateCallback, LoadUIFormDependencyAssetCallback);
			m_InstancePool = null;
			m_Serial = 0;
			m_OpenUIFormSuccessEventHandler = null;
			m_OpenUIFormFailureEventHandler = null;
			m_OpenUIFormUpdateEventHandler = null;
			m_OpenUIFormDependencyAssetEventHandler = null;
			m_CloseUIFormCompleteEventHandler = null;

        }

		/// <summary>
		/// 获取界面组数量。
		/// </summary>
		public int UIGroupCount
		{
			get
			{
				return m_UIGroups.Count;
			}
		}

        /// <summary>
        /// 获取或设置界面实例对象池自动释放可释放对象的间隔秒数。
        /// </summary>
        public float InstanceAutoReleaseInterval
        {
            get
            {
                return m_InstancePool.AutoReleaseInterval;
            }
            set
            {
                m_InstancePool.AutoReleaseInterval = value;
            }
        }

        /// <summary>
        /// 获取或设置界面实例对象池的容量。
        /// </summary>
        public int InstanceCapacity
		{
			get
			{
				return m_InstancePool.Capacity;
			}
			set
			{
				m_InstancePool.Capacity = value;
			}
		}

		/// <summary>
		/// 获取或设置界面实例对象池对象过期秒数。
		/// </summary>
		public float InstanceExpireTime
		{
			get
			{
				return m_InstancePool.ExpireTime;
			}
			set
			{
				m_InstancePool.ExpireTime = value;
			}
		}

		/// <summary>
		/// 获取或设置界面实例对象池的优先级。
		/// </summary>
		public int InstancePriority
		{
			get
			{
				return m_InstancePool.Priority;
			}
			set
			{
				m_InstancePool.Priority = value;
			}
		}

		/// <summary>
		/// 打开界面成功事件。
		/// </summary>
		public event EventHandler<OpenUIFormSuccessEventArgs> OpenUIFormSuccess
		{
			add
			{
				m_OpenUIFormSuccessEventHandler += value;
			}
			remove
			{
				m_OpenUIFormSuccessEventHandler -= value;
			}
		}

		/// <summary>
		/// 打开界面失败事件。
		/// </summary>
		public event EventHandler<OpenUIFormFailureEventArgs> OpenUIFormFailure
		{
			add
			{
				m_OpenUIFormFailureEventHandler += value;
			}
			remove
			{
				m_OpenUIFormFailureEventHandler -= value;
			}
		}

		/// <summary>
		/// 打开界面更新事件。
		/// </summary>
		public event EventHandler<OpenUIFormUpdateEventArgs> OpenUIFormUpdate
		{
			add
			{
				m_OpenUIFormUpdateEventHandler += value;
			}
			remove
			{
				m_OpenUIFormUpdateEventHandler -= value;
			}
		}

		/// <summary>
		/// 打开界面时加载依赖资源事件。
		/// </summary>
		public event EventHandler<OpenUIFormDependencyAssetEventArgs> OpenUIFormDependencyAsset
		{
			add
			{
				m_OpenUIFormDependencyAssetEventHandler += value;
			}
			remove
			{
				m_OpenUIFormDependencyAssetEventHandler -= value;
			}
		}

		/// <summary>
		/// 关闭界面完成事件。
		/// </summary>
		public event EventHandler<CloseUIFormCompleteEventArgs> CloseUIFormComplete
		{
			add
			{
				m_CloseUIFormCompleteEventHandler += value;
			}
			remove
			{
				m_CloseUIFormCompleteEventHandler -= value;
			}
		}

		/// <summary>
		/// 界面管理器轮询。
		/// </summary>
		/// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
		/// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
		public override void Update(float elapseSeconds, float realElapseSeconds)
		{
            while (m_RecycleQueue.Count > 0)
            {
				UIForm uiForm = m_RecycleQueue.Dequeue();
				if(null != uiForm)
                {
					uiForm.OnRecycle();

					if(null != uiForm.Handle)
						m_InstancePool.Unspawn(uiForm.Handle);
				}
            }

            foreach (KeyValuePair<string, UIGroup> uiGroup in m_UIGroups)
			{
				uiGroup.Value.Update(elapseSeconds, realElapseSeconds);
			}
		}

		/// <summary>
		/// 关闭并清理界面管理器。
		/// </summary>
		public override void Shutdown()
		{
			CloseAllLoadedUIForms();
			m_UIGroups.Clear();
			m_UIFormsBeingLoaded.Clear();
			m_UIFormsToReleaseOnLoad.Clear();
			m_UIFormOpenStack.Clear();
			//m_RecycleQueue.Clear();
		}

		/// <summary>
		/// 设置对象池管理器。
		/// </summary>
		/// <param name="objectPoolManager">对象池管理器。</param>
		public void CreateInstancePool()
		{
			m_InstancePool = ObjectPoolManager.Instance.CreateSingleSpawnObjectPool<UIFormInstanceObject>("UI Instance Pool");
		}

		/// <summary>
		/// 是否存在界面组。
		/// </summary>
		/// <param name="uiGroupName">界面组名称。</param>
		/// <returns>是否存在界面组。</returns>
		public bool HasUIGroup(string uiGroupName)
		{
			if (string.IsNullOrEmpty(uiGroupName))
			{
				throw new FrameworkException("UI group name is invalid.");
			}

			return m_UIGroups.ContainsKey(uiGroupName);
		}

		/// <summary>
		/// 获取界面组。
		/// </summary>
		/// <param name="uiGroupName">界面组名称。</param>
		/// <returns>要获取的界面组。</returns>
		public UIGroup GetUIGroup(string uiGroupName)
		{
			if (string.IsNullOrEmpty(uiGroupName))
			{
				throw new FrameworkException("UI group name is invalid.");
			}

			UIGroup uiGroup = null;
			if (m_UIGroups.TryGetValue(uiGroupName, out uiGroup))
			{
				return uiGroup;
			}

			return null;
		}

		/// <summary>
		/// 获取所有界面组。
		/// </summary>
		/// <returns>所有界面组。</returns>
		public UIGroup[] GetAllUIGroups()
		{
			int index = 0;
			UIGroup[] uiGroups = new UIGroup[m_UIGroups.Count];
			foreach (KeyValuePair<string, UIGroup> uiGroup in m_UIGroups)
			{
				uiGroups[index++] = uiGroup.Value;
			}

			return uiGroups;
		}

        /// <summary>
        /// 获取所有界面组。
        /// </summary>
        /// <param name="results">所有界面组。</param>
        public void GetAllUIGroups(List<UIGroup> results)
        {
            if (results == null)
            {
                throw new FrameworkException("Results is invalid.");
            }

            results.Clear();
            foreach (KeyValuePair<string, UIGroup> uiGroup in m_UIGroups)
            {
                results.Add(uiGroup.Value);
            }
        }

		/// <summary>
		/// 增加界面组。
		/// </summary>
		/// <param name="uiGroupName">界面组名称。</param>
		/// <param name="uiGroupDepth">界面组深度。</param>
		/// <param name="uiGroupHelper">界面组辅助器。</param>
		/// <returns>是否增加界面组成功。</returns>
		public bool AddUIGroup(string uiGroupName, int uiGroupDepth, Transform root)
		{
			if (string.IsNullOrEmpty(uiGroupName))
			{
				throw new FrameworkException("UI group name is invalid.");
			}

			if (HasUIGroup(uiGroupName))
			{
				return false;
			}

			m_UIGroups.Add(uiGroupName, new UIGroup(uiGroupName, uiGroupDepth, root));

			return true;
		}

		/// <summary>
		/// 是否存在界面。
		/// </summary>
		/// <param name="serialId">界面序列编号。</param>
		/// <returns>是否存在界面。</returns>
		public bool HasUIForm(int serialId)
		{
			foreach (KeyValuePair<string, UIGroup> uiGroup in m_UIGroups)
			{
				if (uiGroup.Value.HasUIForm(serialId))
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// 是否存在界面。
		/// </summary>
		/// <param name="uiFormAssetName">界面资源名称。</param>
		/// <returns>是否存在界面。</returns>
		public bool HasUIForm(string uiFormAssetName)
		{
			if (string.IsNullOrEmpty(uiFormAssetName))
			{
				throw new FrameworkException("UI form asset name is invalid.");
			}

			foreach (KeyValuePair<string, UIGroup> uiGroup in m_UIGroups)
			{
				if (uiGroup.Value.HasUIForm(uiFormAssetName))
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// 获取界面。
		/// </summary>
		/// <param name="serialId">界面序列编号。</param>
		/// <returns>要获取的界面。</returns>
		public UIForm GetUIForm(int serialId)
		{
			foreach (KeyValuePair<string, UIGroup> uiGroup in m_UIGroups)
			{
				UIForm uiForm = uiGroup.Value.GetUIForm(serialId);
				if (uiForm != null)
				{
					return uiForm;
				}
			}

			return null;
		}

		/// <summary>
		/// 获取界面。
		/// </summary>
		/// <param name="uiFormAssetName">界面资源名称。</param>
		/// <returns>要获取的界面。</returns>
		public UIForm GetUIForm(string uiFormAssetName)
		{
			if (string.IsNullOrEmpty(uiFormAssetName))
			{
				throw new FrameworkException("UI form asset name is invalid.");
			}

			foreach (KeyValuePair<string, UIGroup> uiGroup in m_UIGroups)
			{
				UIForm uiForm = uiGroup.Value.GetUIForm(uiFormAssetName);
				if (uiForm != null)
				{
					return uiForm;
				}
			}

			return null;
		}

		/// <summary>
		/// 获取界面。
		/// </summary>
		/// <param name="uiFormAssetName">界面资源名称。</param>
		/// <returns>要获取的界面。</returns>
		public UIForm[] GetUIForms(string uiFormAssetName)
		{
			if (string.IsNullOrEmpty(uiFormAssetName))
			{
				throw new FrameworkException("UI form asset name is invalid.");
			}

			List<UIForm> uiForms = new List<UIForm>();
			foreach (KeyValuePair<string, UIGroup> uiGroup in m_UIGroups)
			{
				uiForms.AddRange(uiGroup.Value.GetUIForms(uiFormAssetName));
			}

			return uiForms.ToArray();
		}

		/// <summary>
		/// 获取所有已加载的界面。
		/// </summary>
		/// <returns>所有已加载的界面。</returns>
		public UIForm[] GetAllLoadedUIForms()
		{
			List<UIForm> uiForms = new List<UIForm>();
			foreach (KeyValuePair<string, UIGroup> uiGroup in m_UIGroups)
			{
				uiForms.AddRange(uiGroup.Value.GetAllUIForms());
			}

			return uiForms.ToArray();
		}

		/// <summary>
		/// 获取所有正在加载界面的序列编号。
		/// </summary>
		/// <returns>所有正在加载界面的序列编号。</returns>
		public int[] GetAllLoadingUIFormSerialIds()
		{
            int index = 0;
            int[] results = new int[m_UIFormsBeingLoaded.Count];
            foreach (var uiFormBeingLoaded in m_UIFormsBeingLoaded)
            {
                results[index++] = uiFormBeingLoaded.Key;
            }

            return results;
        }

		/// <summary>
		/// 是否正在加载界面。
		/// </summary>
		/// <param name="serialId">界面序列编号。</param>
		/// <returns>是否正在加载界面。</returns>
		public bool IsLoadingUIForm(int serialId)
		{
			return m_UIFormsBeingLoaded.ContainsKey(serialId);
		}

		/// <summary>
		/// 是否正在加载界面。
		/// </summary>
		/// <param name="uiFormAssetName">界面资源名称。</param>
		/// <returns>是否正在加载界面。</returns>
		public bool IsLoadingUIForm(string uiFormAssetName)
		{
			if (string.IsNullOrEmpty(uiFormAssetName))
			{
				throw new FrameworkException("UI form asset name is invalid.");
			}

			return m_UIFormsBeingLoaded.ContainsValue(uiFormAssetName);
		}

		/// <summary>
		/// 是否是合法的界面。
		/// </summary>
		/// <param name="uiForm">界面。</param>
		/// <returns>界面是否合法。</returns>
		public bool IsValidUIForm(UIForm uiForm)
		{
			if (uiForm == null)
			{
				return false;
			}

			return HasUIForm(uiForm.SerialId);
		}

		/// <summary>
		/// 打开界面。
		/// </summary>
		/// <param name="uiFormAssetName">界面资源名称。</param>
		/// <param name="uiGroupName">界面组名称。</param>
		/// <returns>界面的序列编号。</returns>
		public int OpenUIForm(string uiFormAssetName, string uiGroupName)
		{
			return OpenUIForm(uiFormAssetName, uiGroupName, false, Constant.DefaultPriority, false, null, null);
		}

		/// <summary>
		/// 打开界面。
		/// </summary>
		/// <param name="uiFormAssetName">界面资源名称。</param>
		/// <param name="uiGroupName">界面组名称。</param>
		/// <param name="userData">用户自定义数据。</param>
		/// <returns>界面的序列编号。</returns>
		public int OpenUIForm(string uiFormAssetName, string uiGroupName, object userData)
		{
			return OpenUIForm(uiFormAssetName, uiGroupName, false, Constant.DefaultPriority, false, userData, null);
		}

		/// <summary>
		/// 打开界面。
		/// </summary>
		/// <param name="uiFormAssetName">界面资源名称。</param>
		/// <param name="uiGroupName">界面组名称。</param>
		/// <param name="priority">加载界面资源的优先级。</param>
		/// <param name="pauseCoveredUIForm">是否暂停被覆盖的界面。</param>
		/// <returns>界面的序列编号。</returns>
		public int OpenUIForm(string uiFormAssetName, string uiGroupName, bool IsMultipleInstance, int priority, bool pauseCoveredUIForm, object userData)
		{
			return OpenUIForm(uiFormAssetName, uiGroupName, IsMultipleInstance, priority, pauseCoveredUIForm, userData, null);
		}

		/// <summary>
		/// 打开界面。
		/// </summary>
		/// <param name="uiFormAssetName">界面资源名称。</param>
		/// <param name="uiGroupName">界面组名称。</param>
		/// <param name="priority">加载界面资源的优先级。</param>
		/// <param name="pauseCoveredUIForm">是否暂停被覆盖的界面。</param>
		/// <param name="userData">用户自定义数据。</param>
		/// <returns>界面的序列编号。</returns>
		public int OpenUIForm(string uiFormAssetName, string uiGroupName, bool IsMultipleInstance, int priority, bool pauseCoveredUIForm, object userData, Action<UIForm> OnComplete)
		{
			//if (m_ResourceManager == null)
			//{
			//    throw new FrameworkException("You must set resource manager first.");
			//}

			if (string.IsNullOrEmpty(uiFormAssetName))
			{
				throw new FrameworkException("UI form asset name is invalid.");
			}

			if (string.IsNullOrEmpty(uiGroupName))
			{
				throw new FrameworkException("UI group name is invalid.");
			}

			UIGroup uiGroup = (UIGroup)GetUIGroup(uiGroupName);
			if (uiGroup == null)
			{
				throw new FrameworkException(string.Format("UI group '{0}' is not exist.", uiGroupName));
			}

			//if (uiGroup.Name == GameDefines.UILayer.Default && !uiGroup.AddNewFormAsset(uiFormAssetName))
			//{
			//	//throw new FrameworkException(string.Format("Open duplicated UI form:{0} in group:{1}", uiFormAssetName, uiGroupName));
			//	Log.ErrorFormat("Open duplicated UI form:{0} in group:{1}", uiFormAssetName, uiGroupName);
			//}

			if(!IsMultipleInstance && (IsLoadingUIForm(uiFormAssetName) || HasUIForm(uiFormAssetName)))
            {
				Log.ErrorFormat("UI重复打开了, {0}", uiFormAssetName);
				return -1;
            }

			int serialId = m_Serial++;
			UIFormInstanceObject uiFormInstanceObject = m_InstancePool.Spawn(uiFormAssetName);
            if (uiFormInstanceObject == null)
            {
				m_UIFormsBeingLoaded.Add(serialId, uiFormAssetName);

				ParseAssetPath(uiFormAssetName, true, out string assetName, out string assetBundle);
                ResourceManager.Instance?.LoadAssetAsync<GameObject>(assetBundle, assetName, (key, asset, err) =>
                {
                    string uiKey = Path.GetFileNameWithoutExtension(uiFormAssetName);
                    var info = new OpenUIFormInfo(serialId, uiKey, uiGroup, pauseCoveredUIForm, userData, OnComplete);
                    if (string.IsNullOrEmpty(err))
                    {
                        LoadUIFormSuccessCallback(uiFormAssetName, asset, 0, info);
                    }
                    else
                    {
                        LoadUIFormFailureCallback(uiFormAssetName, err, info);
                    }
                });
            }
            else
            {
                InternalOpenUIForm(serialId, uiFormAssetName, uiGroup, uiFormInstanceObject.Target, pauseCoveredUIForm, 0f, userData, OnComplete);
            }

			return serialId;
		}

		/// <summary>
		/// 关闭界面。
		/// </summary>
		/// <param name="serialId">要关闭界面的序列编号。</param>
		public void CloseUIForm(int serialId)
		{
			CloseUIForm(serialId, null);
		}

		/// <summary>
		/// 关闭界面。
		/// </summary>
		/// <param name="serialId">要关闭界面的序列编号。</param>
		/// <param name="userData">用户自定义数据。</param>
		public void CloseUIForm(int serialId, object userData)
		{
			if (IsLoadingUIForm(serialId))
			{
				m_UIFormsToReleaseOnLoad.Add(serialId);
				return;
			}

			UIForm uiForm = GetUIForm(serialId);
			if (uiForm == null)
			{
				throw new FrameworkException(string.Format("Can not find UI form '{0}'.", serialId.ToString()));
			}

			CloseUIForm(uiForm, userData);
		}

		/// <summary>
		/// 关闭界面。
		/// </summary>
		/// <param name="uiForm">要关闭的界面。</param>
		public void CloseUIForm(UIForm uiForm)
		{
			CloseUIForm(uiForm, null);
		}

		/// <summary>
		/// 关闭界面。
		/// </summary>
		/// <param name="uiForm">要关闭的界面。</param>
		/// <param name="userData">用户自定义数据。</param>
		public void CloseUIForm(UIForm uiForm, object userData)
		{
			if (uiForm == null)
			{
				throw new FrameworkException("UI form is invalid.");
			}

			UIGroup uiGroup = (UIGroup)uiForm.UIGroup;
			if (uiGroup == null)
			{
				throw new FrameworkException("UI group is invalid.");
			}

            m_TempStack.Clear();
            while (m_UIFormOpenStack.Count > 0)
            {
                // Get last uiform
                var popUiform = m_UIFormOpenStack.Pop();

                if (popUiform == null)
                    continue;

                // 如果关闭的ui不是最后一个
                if (popUiform != uiForm)
                    m_TempStack.Push(popUiform);
                else
                    break;
            }
            while (m_TempStack.Count > 0)
            {
                m_UIFormOpenStack.Push(m_TempStack.Pop());
            }

            uiGroup.RemoveUIForm(uiForm);
			uiForm.OnClose(userData);
			uiGroup.Refresh();

            if (m_CloseUIFormCompleteEventHandler != null)
			{
				m_CloseUIFormCompleteEventHandler(this, new CloseUIFormCompleteEventArgs(uiForm.SerialId, uiForm.UIFormAssetName, uiGroup, userData));
			}

			m_RecycleQueue.Enqueue(uiForm);
		}

        public void CloseUIFormByStack()
        {
            while (m_UIFormOpenStack.Count > 0)
            {
                // Get last uiform
                var uiForm = m_UIFormOpenStack.Pop();

                if (uiForm == null)
                    continue;

                bool rs = uiForm.OnBack();
                if (!rs)
                    m_UIFormOpenStack.Push(uiForm);

                break;
            }
        }

        public UIForm[] GetAllUIFormInStack()
        {
            List<UIForm> forms = new List<UIForm>();
            foreach(var form in m_UIFormOpenStack.ToArray())
            {
                forms.Add(form);
            }

            return forms.ToArray();
        }

        /// <summary>
        /// 关闭所有已加载的界面。
        /// </summary>
        public void CloseAllLoadedUIForms()
		{
			CloseAllLoadedUIForms(null);
		}

		/// <summary>
		/// 关闭所有已加载的界面。
		/// </summary>
		/// <param name="userData">用户自定义数据。</param>
		public void CloseAllLoadedUIForms(object userData)
		{
			UIForm[] uiForms = GetAllLoadedUIForms();
			foreach (UIForm uiForm in uiForms)
			{
				CloseUIForm(uiForm, userData);
			}
		}

		/// <summary>
		/// 关闭所有正在加载的界面。
		/// </summary>
		public void CloseAllLoadingUIForms()
		{
			foreach (var iter in m_UIFormsBeingLoaded)
			{
				m_UIFormsToReleaseOnLoad.Add(iter.Key);
			}
		}

        /// <summary>
        /// 激活界面。
        /// </summary>
        /// <param name="uiForm">要激活的界面。</param>
        public void RefocusUIForm(UIForm uiForm)
		{
			RefocusUIForm(uiForm, null);
		}

		/// <summary>
		/// 激活界面。
		/// </summary>
		/// <param name="uiForm">要激活的界面。</param>
		/// <param name="userData">用户自定义数据。</param>
		public void RefocusUIForm(UIForm uiForm, object userData)
		{
			if (uiForm == null)
			{
				throw new FrameworkException("UI form is invalid.");
			}

			UIGroup uiGroup = (UIGroup)uiForm.UIGroup;
			if (uiGroup == null)
			{
				throw new FrameworkException("UI group is invalid.");
			}

			uiGroup.RefocusUIForm(uiForm, userData);
			uiGroup.Refresh();
			uiForm.OnRefocus(userData);
		}

		/// <summary>
		/// 设置界面实例是否被加锁。
		/// </summary>
		/// <param name="uiFormInstance">要设置是否被加锁的界面实例。</param>
		/// <param name="locked">界面实例是否被加锁。</param>
		public void SetUIFormInstanceLocked(object uiFormInstance, bool locked)
		{
			if (uiFormInstance == null)
			{
				throw new FrameworkException("UI form instance is invalid.");
			}

			m_InstancePool.SetLocked(uiFormInstance, locked);
		}

		/// <summary>
		/// 设置界面实例的优先级。
		/// </summary>
		/// <param name="uiFormInstance">要设置优先级的界面实例。</param>
		/// <param name="priority">界面实例优先级。</param>
		public void SetUIFormInstancePriority(object uiFormInstance, int priority)
		{
			if (uiFormInstance == null)
			{
				throw new FrameworkException("UI form instance is invalid.");
			}

			m_InstancePool.SetPriority(uiFormInstance, priority);
		}

		private void InternalOpenUIForm(int serialId, string uiFormAssetName, UIGroup uiGroup, object uiFormInstance, bool pauseCoveredUIForm, float duration, object userData, Action<UIForm> openCallback)
		{
			try
			{
				UIForm uiForm = CreateUIForm(uiFormInstance, uiGroup, userData);
				if (uiForm == null)
				{
					throw new FrameworkException("Can not create UI form in helper.");
				}

				uiForm.OnInit(serialId, uiFormAssetName, uiGroup, pauseCoveredUIForm, userData);
				uiGroup.AddUIForm(uiForm);
				uiForm.OnOpen(userData);
				uiGroup.Refresh();

                m_UIFormOpenStack.Push(uiForm);

				openCallback?.Invoke(uiForm as UIForm);

                if (m_OpenUIFormSuccessEventHandler != null)
				{
					m_OpenUIFormSuccessEventHandler(this, new OpenUIFormSuccessEventArgs(uiForm, duration, userData));
				}

				//m_IUFormOpenStack.Push();
				//RefreshInvisible(uiForm, uiGroup);
			}
			catch (Exception exception)
			{
				Log.Error(exception);
				if (m_OpenUIFormFailureEventHandler != null)
				{
					m_OpenUIFormFailureEventHandler(this, new OpenUIFormFailureEventArgs(serialId, uiFormAssetName, uiGroup.Name, pauseCoveredUIForm, exception.ToString(), userData));
					return;
				}

				throw;
			}
		}

        /// <summary>
        /// 创建界面。
        /// </summary>
        /// <param name="uiFormInstance">界面实例。</param>
        /// <param name="uiGroup">界面所属的界面组。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>界面。</returns>
        private UIForm CreateUIForm(object uiFormInstance, UIGroup uiGroup, object userData)
        {
            GameObject go = uiFormInstance as GameObject;
            if (go == null)
            {
                Log.Error("UI form instance is invalid.");
                return null;
            }

            RectTransform rectTrans = go.GetComponent<RectTransform>();
            if (rectTrans != null)
            {
                rectTrans.SetParent(uiGroup.Mono.transform, false);
                rectTrans.localScale = Vector3.one;
                rectTrans.offsetMin = Vector3.zero;
                rectTrans.offsetMax = Vector3.zero;
                rectTrans.anchorMin = Vector2.zero;
                rectTrans.anchorMax = Vector2.one;
                rectTrans.pivot = new Vector2(.5f, .5f);
                rectTrans.SetAsLastSibling();
            }
            else
            {
                var trans = go.transform;
                trans.SetParent(uiGroup.Mono.transform, false);
                trans.localScale = Vector3.one;
            }

            return go.GetOrAddComponent<UIForm>();
        }

        private void LoadUIFormSuccessCallback(string uiFormAssetName, object uiFormAsset, float duration, object userData)
		{
			OpenUIFormInfo openUIFormInfo = (OpenUIFormInfo)userData;
			if (openUIFormInfo == null)
			{
				throw new FrameworkException("Open UI form info is invalid.");
			}

			if (m_UIFormsToReleaseOnLoad.Contains(openUIFormInfo.SerialId))
			{
				//Log.Debug("Release UI form '{0}' on loading success.", openUIFormInfo.SerialId.ToString());
				m_UIFormsToReleaseOnLoad.Remove(openUIFormInfo.SerialId);
                ResourceHelper.UnloadAssetWithObject(uiFormAsset, true);
				return;
			}

			m_UIFormsBeingLoaded.Remove(openUIFormInfo.SerialId);
			UIFormInstanceObject uiFormInstanceObject = new UIFormInstanceObject(uiFormAssetName, uiFormAsset);
            m_InstancePool.Register(uiFormInstanceObject, true);

            InternalOpenUIForm(openUIFormInfo.SerialId, uiFormAssetName, openUIFormInfo.UIGroup, uiFormInstanceObject.Target, openUIFormInfo.PauseCoveredUIForm, 0, openUIFormInfo.UserData, openUIFormInfo.LoadFromSuccessAction);
		}

		private void LoadUIFormFailureCallback(string uiFormAssetName, string errorMessage, object userData)
		{
			OpenUIFormInfo openUIFormInfo = (OpenUIFormInfo)userData;
			if (openUIFormInfo == null)
			{
				throw new FrameworkException("Open UI form info is invalid.");
			}

			m_UIFormsBeingLoaded.Remove(openUIFormInfo.SerialId);
			m_UIFormsToReleaseOnLoad.Remove(openUIFormInfo.SerialId);
			string appendErrorMessage = string.Format("Load UI form failure, asset name '{0}', error message '{1}'.", uiFormAssetName, errorMessage);
			if (m_OpenUIFormFailureEventHandler != null)
			{
				m_OpenUIFormFailureEventHandler(this, new OpenUIFormFailureEventArgs(openUIFormInfo.SerialId, uiFormAssetName, openUIFormInfo.UIGroup.Name, openUIFormInfo.PauseCoveredUIForm, appendErrorMessage, openUIFormInfo.UserData));
				return;
			}

			throw new FrameworkException(appendErrorMessage);
		}

		private void LoadUIFormUpdateCallback(string uiFormAssetName, float progress, object userData)
		{
			OpenUIFormInfo openUIFormInfo = (OpenUIFormInfo)userData;
			if (openUIFormInfo == null)
			{
				throw new FrameworkException("Open UI form info is invalid.");
			}

			if (m_OpenUIFormUpdateEventHandler != null)
			{
				m_OpenUIFormUpdateEventHandler(this, new OpenUIFormUpdateEventArgs(openUIFormInfo.SerialId, uiFormAssetName, openUIFormInfo.UIGroup.Name, openUIFormInfo.PauseCoveredUIForm, progress, openUIFormInfo.UserData));
			}
		}

		private void LoadUIFormDependencyAssetCallback(string uiFormAssetName, string dependencyAssetName, int loadedCount, int totalCount, object userData)
		{
			OpenUIFormInfo openUIFormInfo = (OpenUIFormInfo)userData;
			if (openUIFormInfo == null)
			{
				throw new FrameworkException("Open UI form info is invalid.");
			}

			if (m_OpenUIFormDependencyAssetEventHandler != null)
			{
				m_OpenUIFormDependencyAssetEventHandler(this, new OpenUIFormDependencyAssetEventArgs(openUIFormInfo.SerialId, uiFormAssetName, openUIFormInfo.UIGroup.Name, openUIFormInfo.PauseCoveredUIForm, dependencyAssetName, loadedCount, totalCount, openUIFormInfo.UserData));
			}
		}

        private void ParseAssetPath(string assetPath, bool isSingleBundle, out string assetName, out string assetBundleName)
        {
            assetName = System.IO.Path.GetFileNameWithoutExtension(assetPath);
            int length = assetPath.LastIndexOf(isSingleBundle ? '.' : '/');
            if (length != -1)
                assetBundleName = assetPath.Substring(0, length).ToLower();
            else
                assetBundleName = assetPath.ToLower();
        }
    }
}
