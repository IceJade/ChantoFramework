using Framework;
using Framework.Resource;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Sound
{
    /// <summary>
    /// 声音管理器。
    /// </summary>
    public class SoundManager : GameBaseSingletonModule<SoundManager>
    {
        private readonly Dictionary<string, SoundGroup> m_SoundGroups;
        private readonly List<int> m_SoundsBeingLoaded;
        private readonly HashSet<int> m_SoundsToReleaseOnLoad;
        private readonly LoadAssetCallbacks m_LoadAssetCallbacks;
        private int m_Serial;
        private EventHandler<PlaySoundSuccessEventArgs> m_PlaySoundSuccessEventHandler;
        private EventHandler<PlaySoundFailureEventArgs> m_PlaySoundFailureEventHandler;
        private EventHandler<PlaySoundUpdateEventArgs> m_PlaySoundUpdateEventHandler;
        private EventHandler<PlaySoundDependencyAssetEventArgs> m_PlaySoundDependencyAssetEventHandler;

        /// <summary>
        /// 初始化声音管理器的新实例。
        /// </summary>
        public SoundManager()
        {
            m_SoundGroups = new Dictionary<string, SoundGroup>();
            m_SoundsBeingLoaded = new List<int>();
            m_SoundsToReleaseOnLoad = new HashSet<int>();
            m_LoadAssetCallbacks = new LoadAssetCallbacks(LoadSoundSuccessCallback, LoadSoundFailureCallback, LoadSoundUpdateCallback, LoadSoundDependencyAssetCallback);
            m_Serial = 1;
            m_PlaySoundSuccessEventHandler = null;
            m_PlaySoundFailureEventHandler = null;
            m_PlaySoundUpdateEventHandler = null;
            m_PlaySoundDependencyAssetEventHandler = null;
        }

        /// <summary>
        /// 获取声音组数量。
        /// </summary>
        public int SoundGroupCount
        {
            get
            {
                return m_SoundGroups.Count;
            }
        }

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

        /// <summary>
        /// 声音管理器轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        public override void Update(float elapseSeconds, float realElapseSeconds)
        {

        }

        /// <summary>
        /// 关闭并清理声音管理器。
        /// </summary>
        public void Shutdown()
        {
            StopAllLoadedSounds();
            m_SoundGroups.Clear();
            m_SoundsBeingLoaded.Clear();
            m_SoundsToReleaseOnLoad.Clear();
        }

        /// <summary>
        /// 是否存在指定声音组。
        /// </summary>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <returns>指定声音组是否存在。</returns>
        public bool HasSoundGroup(string soundGroupName)
        {
            if (string.IsNullOrEmpty(soundGroupName))
            {
                throw new FrameworkException("Sound group name is invalid.");
            }

            return m_SoundGroups.ContainsKey(soundGroupName);
        }

        /// <summary>
        /// 获取指定声音组。
        /// </summary>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <returns>要获取的声音组。</returns>
        public SoundGroup GetSoundGroup(string soundGroupName)
        {
            if (string.IsNullOrEmpty(soundGroupName))
            {
                throw new FrameworkException("Sound group name is invalid.");
            }

            SoundGroup soundGroup = null;
            if (m_SoundGroups.TryGetValue(soundGroupName, out soundGroup))
            {
                return soundGroup;
            }

            return null;
        }

        /// <summary>
        /// 获取所有声音组。
        /// </summary>
        /// <returns>所有声音组。</returns>
        public SoundGroup[] GetAllSoundGroups()
        {
            int index = 0;
            SoundGroup[] soundGroups = new SoundGroup[m_SoundGroups.Count];
            foreach (KeyValuePair<string, SoundGroup> soundGroup in m_SoundGroups)
            {
                soundGroups[index++] = soundGroup.Value;
            }

            return soundGroups;
        }

        /// <summary>
        /// 增加声音组。
        /// </summary>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <param name="soundGroupAvoidBeingReplacedBySamePriority">声音组中的声音是否避免被同优先级声音替换。</param>
        /// <param name="soundGroupMute">声音组是否静音。</param>
        /// <param name="soundGroupVolume">声音组音量。</param>
        /// <param name="soundGroupHelper">声音组辅助器。</param>
        /// <returns>是否增加声音组成功。</returns>
        public bool AddSoundGroup(string soundGroupName, bool soundGroupAvoidBeingReplacedBySamePriority, bool soundGroupMute, float soundGroupVolume)
        {
            if (string.IsNullOrEmpty(soundGroupName))
            {
                throw new FrameworkException("Sound group name is invalid.");
            }

            if (HasSoundGroup(soundGroupName))
            {
                return false;
            }

            SoundGroup soundGroup = new SoundGroup(soundGroupName);
            soundGroup.AvoidBeingReplacedBySamePriority = soundGroupAvoidBeingReplacedBySamePriority;
            soundGroup.Mute = soundGroupMute;
            soundGroup.Volume = soundGroupVolume;
            m_SoundGroups.Add(soundGroupName, soundGroup);

            return true;
        }

        /// <summary>
        /// 增加声音代理辅助器。
        /// </summary>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <param name="soundAgentHelper">要增加的声音代理辅助器。</param>
        public void AddSoundAgentHelper(string soundGroupName, SoundAgentHelper soundAgentHelper)
        {
            SoundGroup soundGroup = (SoundGroup)GetSoundGroup(soundGroupName);
            if (soundGroup == null)
            {
                throw new FrameworkException(string.Format("Sound group '{0}' is not exist.", soundGroupName));
            }

            soundGroup.AddSoundAgentHelper(soundAgentHelper);
        }

        /// <summary>
        /// 获取所有正在加载声音的序列编号。
        /// </summary>
        /// <returns>所有正在加载声音的序列编号。</returns>
        public int[] GetAllLoadingSoundSerialIds()
        {
            return m_SoundsBeingLoaded.ToArray();
        }

        /// <summary>
        /// 是否正在加载声音。
        /// </summary>
        /// <param name="serialId">声音序列编号。</param>
        /// <returns>是否正在加载声音。</returns>
        public bool IsLoadingSound(int serialId)
        {
            return m_SoundsBeingLoaded.Contains(serialId);
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="soundAssetName">声音资源名称。</param>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlaySound(string soundAssetName, string soundGroupName)
        {
            return PlaySound(soundAssetName, soundGroupName, Constant.DefaultPriority, null, null);
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
            return PlaySound(soundAssetName, soundGroupName, priority, null, null);
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
            return PlaySound(soundAssetName, soundGroupName, Constant.DefaultPriority, playSoundParams, null);
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
            return PlaySound(soundAssetName, soundGroupName, Constant.DefaultPriority, null, userData);
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
            return PlaySound(soundAssetName, soundGroupName, priority, playSoundParams, null);
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="soundAssetName">声音资源名称。</param>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <param name="priority">加载声音资源的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlaySound(string soundAssetName, string soundGroupName, int priority, object userData)
        {
            return PlaySound(soundAssetName, soundGroupName, priority, null, userData);
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="soundAssetName">声音资源名称。</param>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <param name="playSoundParams">播放声音参数。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlaySound(string soundAssetName, string soundGroupName, PlaySoundParams playSoundParams, object userData)
        {
            return PlaySound(soundAssetName, soundGroupName, Constant.DefaultPriority, playSoundParams, userData);
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
            if (playSoundParams == null)
            {
                playSoundParams = new PlaySoundParams();
            }

            int serialId = m_Serial++;
            PlaySoundErrorCode? errorCode = null;
            string errorMessage = null;
            SoundGroup soundGroup = (SoundGroup)GetSoundGroup(soundGroupName);
            if (soundGroup == null)
            {
                errorCode = PlaySoundErrorCode.SoundGroupNotExist;
                errorMessage = string.Format("Sound group '{0}' is not exist.", soundGroupName);
            }
            else if (soundGroup.SoundAgentCount <= 0)
            {
                errorCode = PlaySoundErrorCode.SoundGroupHasNoAgent;
                errorMessage = string.Format("Sound group '{0}' is have no sound agent.", soundGroupName);
            }

            if (errorCode.HasValue)
            {
                if (m_PlaySoundFailureEventHandler != null)
                {
                    m_PlaySoundFailureEventHandler(this, new PlaySoundFailureEventArgs(serialId, soundAssetName, soundGroupName, playSoundParams, errorCode.Value, errorMessage, userData));
                    return serialId;
                }

                throw new FrameworkException(errorMessage);
            }

            m_SoundsBeingLoaded.Add(serialId);
            // ParseAssetPath(soundAssetName, true, out string assetName, out string assetBundle);
            // Debug.Log("play sound : " + soundAssetName);
            ResourceHelper.LoadAsset<AudioClip>(soundAssetName, (key, asset, error) => 
            {
                if (string.IsNullOrEmpty(error))
                {
                    m_LoadAssetCallbacks?.LoadAssetSuccessCallback?.Invoke(soundAssetName, asset, 0, new PlaySoundEventParams(serialId, soundGroup, playSoundParams, userData));
                }
                else
                {
                    m_LoadAssetCallbacks?.LoadAssetFailureCallback?.Invoke(soundAssetName, error, new PlaySoundEventParams(serialId, soundGroup, playSoundParams, userData));
                }
            });
     

            // Framework.ResourceManager.Instance?.LoadAssetAsync(assetBundle, assetName, typeof(UnityEngine.Object), (key, asset, err) =>
            // {
            //     if (string.IsNullOrEmpty(err))
            //         m_LoadAssetCallbacks?.LoadAssetSuccessCallback?.Invoke(soundAssetName, asset, 0, new PlaySoundInfo(serialId, soundGroup, playSoundParams, userData));
            //     else
            //         m_LoadAssetCallbacks?.LoadAssetFailureCallback?.Invoke(soundAssetName, LoadResourceStatus.NotExist, err, new PlaySoundInfo(serialId, soundGroup, playSoundParams, userData));

            // });

            //GameEntry.Resource.LoadAssetWithPath(soundAssetName, m_LoadAssetCallbacks, new PlaySoundInfo(serialId, soundGroup, playSoundParams, userData));
            //m_ResourceManager.LoadAsset(soundAssetName, priority, m_LoadAssetCallbacks, new PlaySoundInfo(serialId, soundGroup, playSoundParams, userData));
            return serialId;
        }

        /// <summary>
        /// 停止播放声音。
        /// </summary>
        /// <param name="serialId">要停止播放声音的序列编号。</param>
        /// <returns>是否停止播放声音成功。</returns>
        public bool StopSound(int serialId)
        {
            return StopSound(serialId, SoundDefine.DefaultFadeOutSeconds);
        }

        /// <summary>
        /// 停止播放声音。
        /// </summary>
        /// <param name="serialId">要停止播放声音的序列编号。</param>
        /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位。</param>
        /// <returns>是否停止播放声音成功。</returns>
        public bool StopSound(int serialId, float fadeOutSeconds)
        {
            if (IsLoadingSound(serialId))
            {
                m_SoundsToReleaseOnLoad.Add(serialId);
                return true;
            }

            foreach (KeyValuePair<string, SoundGroup> soundGroup in m_SoundGroups)
            {
                if (soundGroup.Value.StopSound(serialId, fadeOutSeconds))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 停止所有已加载的声音。
        /// </summary>
        public void StopAllLoadedSounds()
        {
            StopAllLoadedSounds(SoundDefine.DefaultFadeOutSeconds);
        }

        /// <summary>
        /// 停止所有已加载的声音。
        /// </summary>
        /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位。</param>
        public void StopAllLoadedSounds(float fadeOutSeconds)
        {
            foreach (KeyValuePair<string, SoundGroup> soundGroup in m_SoundGroups)
            {
                soundGroup.Value.StopAllLoadedSounds(fadeOutSeconds);
            }
        }

        /// <summary>
        /// 停止所有正在加载的声音。
        /// </summary>
        public void StopAllLoadingSounds()
        {
            foreach (int serialId in m_SoundsBeingLoaded)
            {
                m_SoundsToReleaseOnLoad.Add(serialId);
            }
        }

        /// <summary>
        /// 暂停播放声音。
        /// </summary>
        /// <param name="serialId">要暂停播放声音的序列编号。</param>
        public void PauseSound(int serialId)
        {
            PauseSound(serialId, SoundDefine.DefaultFadeOutSeconds);
        }

        /// <summary>
        /// 暂停播放声音。
        /// </summary>
        /// <param name="serialId">要暂停播放声音的序列编号。</param>
        /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位。</param>
        public void PauseSound(int serialId, float fadeOutSeconds)
        {
            foreach (KeyValuePair<string, SoundGroup> soundGroup in m_SoundGroups)
            {
                if (soundGroup.Value.PauseSound(serialId, fadeOutSeconds))
                {
                    return;
                }
            }

            throw new FrameworkException(string.Format("Can not find sound '{0}'.", serialId.ToString()));
        }

        /// <summary>
        /// 恢复播放声音。
        /// </summary>
        /// <param name="serialId">要恢复播放声音的序列编号。</param>
        public void ResumeSound(int serialId)
        {
            ResumeSound(serialId, SoundDefine.DefaultFadeInSeconds);
        }

        /// <summary>
        /// 恢复播放声音。
        /// </summary>
        /// <param name="serialId">要恢复播放声音的序列编号。</param>
        /// <param name="fadeInSeconds">声音淡入时间，以秒为单位。</param>
        public void ResumeSound(int serialId, float fadeInSeconds)
        {
            foreach (KeyValuePair<string, SoundGroup> soundGroup in m_SoundGroups)
            {
                if (soundGroup.Value.ResumeSound(serialId, fadeInSeconds))
                {
                    return;
                }
            }

            throw new FrameworkException(string.Format("Can not find sound '{0}'.", serialId.ToString()));
        }

        private void LoadSoundSuccessCallback(string soundAssetName, object soundAsset, float duration, object userData)
        {
            PlaySoundEventParams playSoundInfo = (PlaySoundEventParams)userData;
            if (playSoundInfo == null)
            {
                throw new FrameworkException("Play sound info is invalid.");
            }

            m_SoundsBeingLoaded.Remove(playSoundInfo.SerialId);
            if (m_SoundsToReleaseOnLoad.Contains(playSoundInfo.SerialId))
            {
                //Log.Debug("Release sound '{0}' on loading success.", playSoundInfo.SerialId.ToString());
                m_SoundsToReleaseOnLoad.Remove(playSoundInfo.SerialId);
                ResourceHelper.UnloadAssetWithObject(soundAsset);
                return;
            }

            PlaySoundErrorCode? errorCode = null;
            SoundAgent soundAgent = playSoundInfo.SoundGroup.PlaySound(playSoundInfo.SerialId, soundAsset, playSoundInfo.PlaySoundParams, out errorCode);
            if (soundAgent != null)
            {
                if (m_PlaySoundSuccessEventHandler != null)
                {
                    m_PlaySoundSuccessEventHandler(this, new PlaySoundSuccessEventArgs(playSoundInfo.SerialId, soundAssetName, soundAgent, duration, playSoundInfo.UserData));
                }
            }
            else
            {
                m_SoundsToReleaseOnLoad.Remove(playSoundInfo.SerialId);
                ResourceHelper.UnloadAssetWithObject(soundAsset);
                string errorMessage = string.Format("Sound group '{0}' play sound '{1}' failure.", playSoundInfo.SoundGroup.Name, soundAssetName);
                if (m_PlaySoundFailureEventHandler != null)
                {
                    m_PlaySoundFailureEventHandler(this, new PlaySoundFailureEventArgs(playSoundInfo.SerialId, soundAssetName, playSoundInfo.SoundGroup.Name, playSoundInfo.PlaySoundParams, errorCode.Value, errorMessage, playSoundInfo.UserData));
                    return;
                }

                throw new FrameworkException(errorMessage);
            }
        }

        private void LoadSoundFailureCallback(string soundAssetName, string errorMessage, object userData)
        {
            PlaySoundEventParams playSoundInfo = (PlaySoundEventParams)userData;
            if (playSoundInfo == null)
            {
                throw new FrameworkException("Play sound info is invalid.");
            }

            m_SoundsBeingLoaded.Remove(playSoundInfo.SerialId);
            m_SoundsToReleaseOnLoad.Remove(playSoundInfo.SerialId);
            string appendErrorMessage = string.Format("Load sound failure, asset name '{0}', error message '{1}'.", soundAssetName, errorMessage);
            if (m_PlaySoundFailureEventHandler != null)
            {
                m_PlaySoundFailureEventHandler(this, new PlaySoundFailureEventArgs(playSoundInfo.SerialId, soundAssetName, playSoundInfo.SoundGroup.Name, playSoundInfo.PlaySoundParams, PlaySoundErrorCode.LoadAssetFailure, appendErrorMessage, playSoundInfo.UserData));
                return;
            }

            throw new FrameworkException(appendErrorMessage);
        }

        private void LoadSoundUpdateCallback(string soundAssetName, float progress, object userData)
        {
            PlaySoundEventParams playSoundInfo = (PlaySoundEventParams)userData;
            if (playSoundInfo == null)
            {
                throw new FrameworkException("Play sound info is invalid.");
            }

            if (m_PlaySoundUpdateEventHandler != null)
            {
                m_PlaySoundUpdateEventHandler(this, new PlaySoundUpdateEventArgs(playSoundInfo.SerialId, soundAssetName, playSoundInfo.SoundGroup.Name, playSoundInfo.PlaySoundParams, progress, playSoundInfo.UserData));
            }
        }

        private void LoadSoundDependencyAssetCallback(string soundAssetName, string dependencyAssetName, int loadedCount, int totalCount, object userData)
        {
            PlaySoundEventParams playSoundInfo = (PlaySoundEventParams)userData;
            if (playSoundInfo == null)
            {
                throw new FrameworkException("Play sound info is invalid.");
            }

            if (m_PlaySoundDependencyAssetEventHandler != null)
            {
                m_PlaySoundDependencyAssetEventHandler(this, new PlaySoundDependencyAssetEventArgs(playSoundInfo.SerialId, soundAssetName, playSoundInfo.SoundGroup.Name, playSoundInfo.PlaySoundParams, dependencyAssetName, loadedCount, totalCount, playSoundInfo.UserData));
            }
        }
    }
}
