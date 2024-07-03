using System;
using Framework;

namespace Framework.Sound
{
    /// <summary>
    /// 声音代理。
    /// </summary>
    public class SoundAgent
    {
        private readonly SoundGroup m_SoundGroup;
        private readonly SoundAgentHelper m_SoundAgentHelper;
        private int m_SerialId;
        private object m_SoundAsset;
        private DateTime m_SetSoundAssetTime;
        private bool m_MuteInSoundGroup;
        private float m_VolumeInSoundGroup;

        /// <summary>
        /// 初始化声音代理的新实例。
        /// </summary>
        /// <param name="soundGroup">所在的声音组。</param>
        /// <param name="soundAgentHelper">声音代理辅助器接口。</param>
        public SoundAgent(SoundGroup soundGroup, SoundAgentHelper soundAgentHelper)
        {
            if (soundGroup == null)
            {
                throw new Exception("Sound group is invalid.");
            }

            if (soundAgentHelper == null)
            {
                throw new Exception("Sound agent helper is invalid.");
            }

            m_SoundGroup = soundGroup;
            m_SoundAgentHelper = soundAgentHelper;
            m_SoundAgentHelper.ResetSoundAgent += OnResetSoundAgent;
            m_SerialId = 0;
            m_SoundAsset = null;
            Reset();
        }

        /// <summary>
        /// 获取所在的声音组。
        /// </summary>
        public SoundGroup SoundGroup
        {
            get
            {
                return m_SoundGroup;
            }
        }

        /// <summary>
        /// 获取或设置声音的序列编号。
        /// </summary>
        public int SerialId
        {
            get
            {
                return m_SerialId;
            }
            set
            {
                m_SerialId = value;
            }
        }

        /// <summary>
        /// 获取当前是否正在播放。
        /// </summary>
        public bool IsPlaying
        {
            get
            {
                return m_SoundAgentHelper.IsPlaying;
            }
        }

        /// <summary>
        /// 获取或设置播放位置。
        /// </summary>
        public float Time
        {
            get
            {
                return m_SoundAgentHelper.Time;
            }
            set
            {
                m_SoundAgentHelper.Time = value;
            }
        }

        /// <summary>
        /// 获取是否静音。
        /// </summary>
        public bool Mute
        {
            get
            {
                return m_SoundAgentHelper.Mute;
            }
        }

        /// <summary>
        /// 获取或设置在声音组内是否静音。
        /// </summary>
        public bool MuteInSoundGroup
        {
            get
            {
                return m_MuteInSoundGroup;
            }
            set
            {
                m_MuteInSoundGroup = value;
                RefreshMute();
            }
        }

        /// <summary>
        /// 获取或设置是否循环播放。
        /// </summary>
        public bool Loop
        {
            get
            {
                return m_SoundAgentHelper.Loop;
            }
            set
            {
                m_SoundAgentHelper.Loop = value;
            }
        }

        /// <summary>
        /// 获取或设置声音优先级。
        /// </summary>
        public int Priority
        {
            get
            {
                return m_SoundAgentHelper.Priority;
            }
            set
            {
                m_SoundAgentHelper.Priority = value;
            }
        }

        /// <summary>
        /// 获取音量大小。
        /// </summary>
        public float Volume
        {
            get
            {
                return m_SoundAgentHelper.Volume;
            }
        }

        /// <summary>
        /// 获取或设置在声音组内音量大小。
        /// </summary>
        public float VolumeInSoundGroup
        {
            get
            {
                return m_VolumeInSoundGroup;
            }
            set
            {
                m_VolumeInSoundGroup = value;
                RefreshVolume();
            }
        }

        /// <summary>
        /// 获取或设置声音音调。
        /// </summary>
        public float Pitch
        {
            get
            {
                return m_SoundAgentHelper.Pitch;
            }
            set
            {
                m_SoundAgentHelper.Pitch = value;
            }
        }

        /// <summary>
        /// 获取或设置声音立体声声相。
        /// </summary>
        public float PanStereo
        {
            get
            {
                return m_SoundAgentHelper.PanStereo;
            }
            set
            {
                m_SoundAgentHelper.PanStereo = value;
            }
        }

        /// <summary>
        /// 获取或设置声音空间混合量。
        /// </summary>
        public float SpatialBlend
        {
            get
            {
                return m_SoundAgentHelper.SpatialBlend;
            }
            set
            {
                m_SoundAgentHelper.SpatialBlend = value;
            }
        }

        /// <summary>
        /// 获取或设置声音最大距离。
        /// </summary>
        public float MaxDistance
        {
            get
            {
                return m_SoundAgentHelper.MaxDistance;
            }
            set
            {
                m_SoundAgentHelper.MaxDistance = value;
            }
        }

        /// <summary>
        /// 获取或设置声音多普勒等级。
        /// </summary>
        public float DopplerLevel
        {
            get
            {
                return m_SoundAgentHelper.DopplerLevel;
            }
            set
            {
                m_SoundAgentHelper.DopplerLevel = value;
            }
        }

        /// <summary>
        /// 获取声音代理辅助器。
        /// </summary>
        public SoundAgentHelper Helper
        {
            get
            {
                return m_SoundAgentHelper;
            }
        }

        /// <summary>
        /// 获取声音创建时间。
        /// </summary>
        internal DateTime SetSoundAssetTime
        {
            get
            {
                return m_SetSoundAssetTime;
            }
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        public void Play()
        {
            m_SoundAgentHelper.Play(SoundDefine.DefaultFadeInSeconds);
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="fadeInSeconds">声音淡入时间，以秒为单位。</param>
        public void Play(float fadeInSeconds)
        {
            m_SoundAgentHelper.Play(fadeInSeconds);
        }

        /// <summary>
        /// 停止播放声音。
        /// </summary>
        public void Stop()
        {
            m_SoundAgentHelper.Stop(SoundDefine.DefaultFadeOutSeconds);
        }

        /// <summary>
        /// 停止播放声音。
        /// </summary>
        /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位。</param>
        public void Stop(float fadeOutSeconds)
        {
            m_SoundAgentHelper.Stop(fadeOutSeconds);
        }

        /// <summary>
        /// 暂停播放声音。
        /// </summary>
        public void Pause()
        {
            m_SoundAgentHelper.Pause(SoundDefine.DefaultFadeOutSeconds);
        }

        /// <summary>
        /// 暂停播放声音。
        /// </summary>
        /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位。</param>
        public void Pause(float fadeOutSeconds)
        {
            m_SoundAgentHelper.Pause(fadeOutSeconds);
        }

        /// <summary>
        /// 恢复播放声音。
        /// </summary>
        public void Resume()
        {
            m_SoundAgentHelper.Resume(SoundDefine.DefaultFadeInSeconds);
        }

        /// <summary>
        /// 恢复播放声音。
        /// </summary>
        /// <param name="fadeInSeconds">声音淡入时间，以秒为单位。</param>
        public void Resume(float fadeInSeconds)
        {
            m_SoundAgentHelper.Resume(fadeInSeconds);
        }

        /// <summary>
        /// 重置声音代理。
        /// </summary>
        public void Reset()
        {
            ResourceHelper.UnloadAssetWithObject(m_SoundAsset);

            m_SetSoundAssetTime = DateTime.MinValue;
            Time = SoundDefine.DefaultTime;
            MuteInSoundGroup = SoundDefine.DefaultMute;
            Loop = SoundDefine.DefaultLoop;
            Priority = SoundDefine.DefaultPriority;
            VolumeInSoundGroup = SoundDefine.DefaultVolume;
            Pitch = SoundDefine.DefaultPitch;
            PanStereo = SoundDefine.DefaultPanStereo;
            SpatialBlend = SoundDefine.DefaultSpatialBlend;
            MaxDistance = SoundDefine.DefaultMaxDistance;
            DopplerLevel = SoundDefine.DefaultDopplerLevel;
            m_SoundAgentHelper.Reset();
        }

        internal bool SetSoundAsset(object soundAsset)
        {
            Reset();
            m_SoundAsset = soundAsset;
            m_SetSoundAssetTime = DateTime.Now;
            return m_SoundAgentHelper.SetSoundAsset(soundAsset);
        }

        internal void RefreshMute()
        {
            m_SoundAgentHelper.Mute = m_SoundGroup.Mute || m_MuteInSoundGroup;
        }

        internal void RefreshVolume()
        {
            m_SoundAgentHelper.Volume = m_SoundGroup.Volume * m_VolumeInSoundGroup;
        }

        private void OnResetSoundAgent(object sender, ResetSoundAgentEventArgs e)
        {
            Reset();
        }
    }
}
