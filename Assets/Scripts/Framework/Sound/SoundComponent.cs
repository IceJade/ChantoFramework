using Framework;
using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using Framework;

namespace Framework.Sound
{
    /// <summary>
    /// 声音组件。
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Game Framework/Sound")]
    public class SoundComponent : FrameworkComponent
    {
        private const int DefaultPriority = 0;

        private AudioListener m_AudioListener = null;

        [SerializeField]
        private Transform m_InstanceRoot = null;

        [SerializeField]
        private AudioMixer m_AudioMixer = null;

        [SerializeField]
        private SoundGroupNode[] m_SoundGroups = null;

        private EventHandler<PlaySoundSuccessEventArgs> m_PlaySoundSuccessEventHandler;
        private EventHandler<PlaySoundFailureEventArgs> m_PlaySoundFailureEventHandler;
        private EventHandler<PlaySoundUpdateEventArgs> m_PlaySoundUpdateEventHandler;
        private EventHandler<PlaySoundDependencyAssetEventArgs> m_PlaySoundDependencyAssetEventHandler;

        /// <summary>
        /// 获取声音组数量。
        /// </summary>
        public int SoundGroupCount
        {
            get
            {
                return SoundManager.Instance.SoundGroupCount;
            }
        }

        /// <summary>
        /// 获取声音混响器。
        /// </summary>
        public AudioMixer AudioMixer
        {
            get
            {
                return m_AudioMixer;
            }
        }

        /// <summary>
        /// 游戏框架组件初始化。
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            SoundManager.Instance.PlaySoundSuccess += OnPlaySoundSuccess;
            SoundManager.Instance.PlaySoundFailure += OnPlaySoundFailure;
            SoundManager.Instance.PlaySoundUpdate += OnPlaySoundUpdate;
            SoundManager.Instance.PlaySoundDependencyAsset += OnPlaySoundDependencyAsset;

            m_AudioListener = gameObject.GetOrAddComponent<AudioListener>();

// #if UNITY_5_4_OR_NEWER
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
// #else
//             ISceneManager sceneManager = FrameworkEntry.GetModule<ISceneManager>();
//             if (sceneManager == null)
//             {
//                 Log.Fatal("Scene manager is invalid.");
//                 return;
//             }

//             sceneManager.LoadSceneSuccess += OnLoadSceneSuccess;
//             sceneManager.LoadSceneFailure += OnLoadSceneFailure;
//             sceneManager.UnloadSceneSuccess += OnUnloadSceneSuccess;
//             sceneManager.UnloadSceneFailure += OnUnloadSceneFailure;
// #endif
        }

        private void Start()
        {
            BaseComponent baseComponent = GameEntry.GetComponent<BaseComponent>();
            if (baseComponent == null)
            {
                Log.Exception("Base component is invalid.");
                return;
            }

            if (m_InstanceRoot == null)
            {
                m_InstanceRoot = (new GameObject("Sound Instances")).transform;
                m_InstanceRoot.SetParent(gameObject.transform);
                m_InstanceRoot.localScale = Vector3.one;
            }

            for (int i = 0; i < m_SoundGroups.Length; i++)
            {
                if (!AddSoundGroup(m_SoundGroups[i].Name, m_SoundGroups[i].AvoidBeingReplacedBySamePriority, m_SoundGroups[i].Mute, m_SoundGroups[i].Volume, m_SoundGroups[i].AgentHelperCount))
                {
                    Log.WarningFormat("Add sound group '{0}' failure.", m_SoundGroups[i].Name);
                    continue;
                }
            }
        }

        private void OnDestroy()
        {
#if UNITY_5_4_OR_NEWER
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
#endif
        }

        /// <summary>
        /// 是否存在指定声音组。
        /// </summary>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <returns>指定声音组是否存在。</returns>
        public bool HasSoundGroup(string soundGroupName)
        {
            return SoundManager.Instance.HasSoundGroup(soundGroupName);
        }

        /// <summary>
        /// 获取指定声音组。
        /// </summary>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <returns>要获取的声音组。</returns>
        public SoundGroup GetSoundGroup(string soundGroupName)
        {
            return SoundManager.Instance.GetSoundGroup(soundGroupName);
        }

        /// <summary>
        /// 获取所有声音组。
        /// </summary>
        /// <returns>所有声音组。</returns>
        public SoundGroup[] GetAllSoundGroups()
        {
            return SoundManager.Instance.GetAllSoundGroups();
        }

        /// <summary>
        /// 设置声音组的静音。
        /// </summary>
        public void SetSoundGroupMute(string soundGroupName, bool mute)
        {
            if (HasSoundGroup(soundGroupName))
            {
                SoundGroup soundGroup = GetSoundGroup(soundGroupName);
                soundGroup.Mute = mute;
            }
        }

        /// <summary>
        /// 增加声音组。
        /// </summary>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <param name="soundAgentHelperCount">声音代理辅助器数量。</param>
        /// <returns>是否增加声音组成功。</returns>
        public bool AddSoundGroup(string soundGroupName, int soundAgentHelperCount)
        {
            return AddSoundGroup(soundGroupName, false, false, 1f, soundAgentHelperCount);
        }

        /// <summary>
        /// 增加声音组。
        /// </summary>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <param name="soundGroupAvoidBeingReplacedBySamePriority">声音组中的声音是否避免被同优先级声音替换。</param>
        /// <param name="soundGroupMute">声音组是否静音。</param>
        /// <param name="soundGroupVolume">声音组音量。</param>
        /// <param name="soundAgentHelperCount">声音代理辅助器数量。</param>
        /// <returns>是否增加声音组成功。</returns>
        public bool AddSoundGroup(string soundGroupName, bool soundGroupAvoidBeingReplacedBySamePriority, bool soundGroupMute, float soundGroupVolume, int soundAgentHelperCount)
        {
            if (SoundManager.Instance.HasSoundGroup(soundGroupName))
            {
                return false;
            }

            var soundGroupObject = new GameObject();
            soundGroupObject.name = string.Format("Sound Group - {0}", soundGroupName);
            Transform transform = soundGroupObject.transform;
            transform.SetParent(m_InstanceRoot);
            transform.localScale = Vector3.one;

            if (!SoundManager.Instance.AddSoundGroup(soundGroupName, soundGroupAvoidBeingReplacedBySamePriority, soundGroupMute, soundGroupVolume))
            {
                return false;
            }

            for (int i = 0; i < soundAgentHelperCount; i++)
            {
                if (!AddSoundAgentHelper(soundGroupName, transform, i))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 获取所有正在加载声音的序列编号。
        /// </summary>
        /// <returns>所有正在加载声音的序列编号。</returns>
        public int[] GetAllLoadingSoundSerialIds()
        {
            return SoundManager.Instance.GetAllLoadingSoundSerialIds();
        }

        /// <summary>
        /// 是否正在加载声音。
        /// </summary>
        /// <param name="serialId">声音序列编号。</param>
        /// <returns>是否正在加载声音。</returns>
        public bool IsLoadingSound(int serialId)
        {
            return SoundManager.Instance.IsLoadingSound(serialId);
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="soundAssetName">声音资源名称。</param>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlaySound(string soundAssetName, string soundGroupName)
        {
            return PlaySound(soundAssetName, soundGroupName, DefaultPriority, null, null, null);
        }

        /// <summary>
        /// 播放音乐。
        /// </summary>
        /// <param name="assetPath">声音资源名称。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlayMusic(string name, bool loop = true)
        {
            var assetPath = string.Format("Assets/Main/Arts/Sound/Music/{0}.ogg", name);
            return PlaySound(assetPath, "Music", DefaultPriority, new PlaySoundParams()
            {
                Loop = loop
            }, null, null);
        }

        /// <summary>
        /// 播放音效。
        /// </summary>
        /// <param name="assetPath">声音资源名称。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlayEffect(string name)
        {
            var assetPath = string.Format("Assets/Main/Arts/Sound/Effect/{0}.ogg", name);
            return PlaySound(assetPath, "Effect", DefaultPriority, null, null, null);
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="name">声音资源名称。</param>
        /// <param name="soundVolume">音量大小</param>
        /// <returns></returns>
        public int PlayEffect(string name, float soundVolume)
        {
            var assetPath = string.Format("Assets/Main/Arts/Sound/Effect/{0}.ogg", name);
            PlaySoundParams soundParams = new PlaySoundParams();
            soundParams.VolumeInSoundGroup = 0.5f;
            return PlaySound(assetPath, "Effect", DefaultPriority, soundParams, null, null);
        }

        /// <summary>
        /// 播放音效。
        /// </summary>
        /// <param name="assetPath">声音资源名称。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlayEffect(string name, bool loop)
        {
            var assetPath = string.Format("Assets/Main/Arts/Sound/Effect/{0}.ogg", name);
            return PlaySound(assetPath, "Effect", DefaultPriority, new PlaySoundParams()
            {
                Loop = loop
            }, null, null);
        }

        /// <summary>
        /// 播放配音
        /// </summary>
        /// <param name="assetPath">声音资源名称。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlayDub(string name, string soundGroup = "Dub")
        {
            string lang = GameEntry.Localization.GetGuideSoundLangName();
            var assetPath = string.Format("Assets/Main/Arts/Sound/Guide/{1}/{0}.ogg", name, lang);
            return PlaySound(assetPath, soundGroup, DefaultPriority, null, null, null);
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="soundAssetName">声音资源名称。</param>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <param name="priority">加载声音资源的优先级。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlaySound(string soundAssetName, string soundGroupName, int priority)
        {
            return PlaySound(soundAssetName, soundGroupName, priority, null, null, null);
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="soundAssetName">声音资源名称。</param>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <param name="playSoundParams">播放声音参数。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlaySound(string soundAssetName, string soundGroupName, PlaySoundParams playSoundParams)
        {
            return PlaySound(soundAssetName, soundGroupName, DefaultPriority, playSoundParams, null, null);
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="soundAssetName">声音资源名称。</param>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <param name="worldPosition">声音所在的世界坐标。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlaySound(string soundAssetName, string soundGroupName, Vector3 worldPosition)
        {
            return PlaySound(soundAssetName, soundGroupName, DefaultPriority, null, worldPosition, null);
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="soundAssetName">声音资源名称。</param>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlaySound(string soundAssetName, string soundGroupName, object userData)
        {
            return PlaySound(soundAssetName, soundGroupName, DefaultPriority, null, null, userData);
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="soundAssetName">声音资源名称。</param>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <param name="priority">加载声音资源的优先级。</param>
        /// <param name="playSoundParams">播放声音参数。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlaySound(string soundAssetName, string soundGroupName, int priority, PlaySoundParams playSoundParams)
        {
            return PlaySound(soundAssetName, soundGroupName, priority, playSoundParams, null, null);
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="soundAssetName">声音资源名称。</param>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <param name="priority">加载声音资源的优先级。</param>
        /// <param name="playSoundParams">播放声音参数。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlaySound(string soundAssetName, string soundGroupName, int priority, PlaySoundParams playSoundParams, object userData)
        {
            return PlaySound(soundAssetName, soundGroupName, priority, playSoundParams, null, userData);
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="soundAssetName">声音资源名称。</param>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <param name="priority">加载声音资源的优先级。</param>
        /// <param name="playSoundParams">播放声音参数。</param>
        /// <param name="bindingEntity">声音绑定的实体。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlaySound(string soundAssetName, string soundGroupName, int priority, PlaySoundParams playSoundParams, object bindingEntity, object userData)
        {
            return SoundManager.Instance.PlaySound(soundAssetName, soundGroupName, priority, playSoundParams, new PlaySoundInfo(Vector3.zero, userData));
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="soundAssetName">声音资源名称。</param>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <param name="priority">加载声音资源的优先级。</param>
        /// <param name="playSoundParams">播放声音参数。</param>
        /// <param name="worldPosition">声音所在的世界坐标。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlaySound(string soundAssetName, string soundGroupName, int priority, PlaySoundParams playSoundParams, Vector3 worldPosition)
        {
            return PlaySound(soundAssetName, soundGroupName, priority, playSoundParams, worldPosition, null);
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="soundAssetName">声音资源名称。</param>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <param name="priority">加载声音资源的优先级。</param>
        /// <param name="playSoundParams">播放声音参数。</param>
        /// <param name="worldPosition">声音所在的世界坐标。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlaySound(string soundAssetName, string soundGroupName, int priority, PlaySoundParams playSoundParams, Vector3 worldPosition, object userData)
        {
            return SoundManager.Instance.PlaySound(soundAssetName, soundGroupName, priority, playSoundParams, new PlaySoundInfo(worldPosition, userData));
        }

        /// <summary>
        /// 停止播放声音。
        /// </summary>
        /// <param name="serialId">要停止播放声音的序列编号。</param>
        /// <returns>是否停止播放声音成功。</returns>
        public bool StopSound(int serialId)
        {
            return SoundManager.Instance.StopSound(serialId);
        }

        /// <summary>
        /// 停止播放声音。
        /// </summary>
        /// <param name="serialId">要停止播放声音的序列编号。</param>
        /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位。</param>
        /// <returns>是否停止播放声音成功。</returns>
        public bool StopSound(int serialId, float fadeOutSeconds)
        {
            return SoundManager.Instance.StopSound(serialId, fadeOutSeconds);
        }

        /// <summary>
        /// 停止所有已加载的声音。
        /// </summary>
        public void StopAllLoadedSounds()
        {
            SoundManager.Instance.StopAllLoadedSounds();
        }

        /// <summary>
        /// 停止所有已加载的声音。
        /// </summary>
        /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位。</param>
        public void StopAllLoadedSounds(float fadeOutSeconds)
        {
            SoundManager.Instance.StopAllLoadedSounds(fadeOutSeconds);
        }

        /// <summary>
        /// 停止所有正在加载的声音。
        /// </summary>
        public void StopAllLoadingSounds()
        {
            SoundManager.Instance.StopAllLoadingSounds();
        }

        /// <summary>
        /// 暂停播放声音。
        /// </summary>
        /// <param name="serialId">要暂停播放声音的序列编号。</param>
        public void PauseSound(int serialId)
        {
            SoundManager.Instance.PauseSound(serialId);
        }

        /// <summary>
        /// 暂停播放声音。
        /// </summary>
        /// <param name="serialId">要暂停播放声音的序列编号。</param>
        /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位。</param>
        public void PauseSound(int serialId, float fadeOutSeconds)
        {
            SoundManager.Instance.PauseSound(serialId, fadeOutSeconds);
        }

        /// <summary>
        /// 恢复播放声音。
        /// </summary>
        /// <param name="serialId">要恢复播放声音的序列编号。</param>
        public void ResumeSound(int serialId)
        {
            SoundManager.Instance.ResumeSound(serialId);
        }

        /// <summary>
        /// 恢复播放声音。
        /// </summary>
        /// <param name="serialId">要恢复播放声音的序列编号。</param>
        /// <param name="fadeInSeconds">声音淡入时间，以秒为单位。</param>
        public void ResumeSound(int serialId, float fadeInSeconds)
        {
            SoundManager.Instance.ResumeSound(serialId, fadeInSeconds);
        }

        /// <summary>
        /// 增加声音代理辅助器。
        /// </summary>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <param name="soundGroupHelper">声音组辅助器。</param>
        /// <param name="index">声音代理辅助器索引。</param>
        /// <returns>是否增加声音代理辅助器成功。</returns>
        private bool AddSoundAgentHelper(string soundGroupName, Transform parent, int index)
        {
            var soundAgentObject = new GameObject();
            var soundAgentHelper = soundAgentObject.GetOrAddComponent<SoundAgentHelper>();
            soundAgentHelper.name = string.Format("Sound Agent Helper - {0} - {1}", soundGroupName, index.ToString());
            Transform transform = soundAgentHelper.transform;
            transform.SetParent(parent);
            transform.localScale = Vector3.one;

            if (m_AudioMixer != null)
            {
                AudioMixerGroup[] audioMixerGroups = m_AudioMixer.FindMatchingGroups(string.Format("Master/{0}/{1}", soundGroupName, index.ToString()));
                if (audioMixerGroups.Length > 0)
                {
                    soundAgentHelper.AudioMixerGroup = audioMixerGroups[0];
                }
                else
                {
                    audioMixerGroups = m_AudioMixer.FindMatchingGroups(string.Format("Master/{0}", soundGroupName));
                    if (audioMixerGroups.Length > 0)
                    {
                        soundAgentHelper.AudioMixerGroup = audioMixerGroups[0];
                    }
                    else
                    {
                        soundAgentHelper.AudioMixerGroup = m_AudioMixer.FindMatchingGroups("Master")[0];
                    }
                }
            }

            SoundManager.Instance.AddSoundAgentHelper(soundGroupName, soundAgentHelper);

            return true;
        }

        #region 事件

        /// <summary>
        /// 播放声音成功事件。
        /// </summary>
        public event EventHandler<PlaySoundSuccessEventArgs> PlaySoundSuccess
        {
            add
            {
                m_PlaySoundSuccessEventHandler += value;
            }
            remove
            {
                m_PlaySoundSuccessEventHandler -= value;
            }
        }

        /// <summary>
        /// 播放声音失败事件。
        /// </summary>
        public event EventHandler<PlaySoundFailureEventArgs> PlaySoundFailure
        {
            add
            {
                m_PlaySoundFailureEventHandler += value;
            }
            remove
            {
                m_PlaySoundFailureEventHandler -= value;
            }
        }

        /// <summary>
        /// 播放声音更新事件。
        /// </summary>
        public event EventHandler<PlaySoundUpdateEventArgs> PlaySoundUpdate
        {
            add
            {
                m_PlaySoundUpdateEventHandler += value;
            }
            remove
            {
                m_PlaySoundUpdateEventHandler -= value;
            }
        }

        /// <summary>
        /// 播放声音时加载依赖资源事件。
        /// </summary>
        public event EventHandler<PlaySoundDependencyAssetEventArgs> PlaySoundDependencyAsset
        {
            add
            {
                m_PlaySoundDependencyAssetEventHandler += value;
            }
            remove
            {
                m_PlaySoundDependencyAssetEventHandler -= value;
            }
        }

        private void OnPlaySoundSuccess(object sender, PlaySoundSuccessEventArgs e)
        {
            PlaySoundInfo playSoundInfo = e.UserData as PlaySoundInfo;
            if (playSoundInfo != null)
            {
                var soundAgentHelper = e.SoundAgent.Helper;
                soundAgentHelper.SetWorldPosition(playSoundInfo.WorldPosition);
            }

            m_PlaySoundSuccessEventHandler?.Invoke(sender, e);
        }

        private void OnPlaySoundFailure(object sender, PlaySoundFailureEventArgs e)
        {
            string logMessage = string.Format("Play sound failure, asset name '{0}', sound group name '{1}', error code '{2}', error message '{3}'.", e.SoundAssetName, e.SoundGroupName, e.ErrorCode.ToString(), e.ErrorMessage);
            if (e.ErrorCode == PlaySoundErrorCode.IgnoredDueToLowPriority)
            {
                Log.Info(logMessage);
            }
            else
            {
                Log.Warning(logMessage);
            }

            m_PlaySoundFailureEventHandler?.Invoke(sender, e);
        }

        private void OnPlaySoundUpdate(object sender, PlaySoundUpdateEventArgs e)
        {
            m_PlaySoundUpdateEventHandler?.Invoke(sender, e);
        }

        private void OnPlaySoundDependencyAsset(object sender, PlaySoundDependencyAssetEventArgs e)
        {
            m_PlaySoundDependencyAssetEventHandler?.Invoke(sender, e);
        }

        // private void OnLoadSceneSuccess(object sender, Framework.Scene.LoadSceneSuccessEventArgs e)
        // {
        //     RefreshAudioListener();
        // }

        // private void OnLoadSceneFailure(object sender, Framework.Scene.LoadSceneFailureEventArgs e)
        // {
        //     RefreshAudioListener();
        // }

        // private void OnUnloadSceneSuccess(object sender, Framework.Scene.UnloadSceneSuccessEventArgs e)
        // {
        //     RefreshAudioListener();
        // }

        // private void OnUnloadSceneFailure(object sender, Framework.Scene.UnloadSceneFailureEventArgs e)
        // {
        //     RefreshAudioListener();
        // }

        #endregion

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            RefreshAudioListener();
        }

        private void OnSceneUnloaded(Scene scene)
        {
            RefreshAudioListener();
        }

        private void RefreshAudioListener()
        {
            m_AudioListener.enabled = FindObjectsOfType<AudioListener>().Length <= 1;
        }
    }
}
