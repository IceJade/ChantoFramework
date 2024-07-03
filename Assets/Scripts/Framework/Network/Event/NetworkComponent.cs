﻿using Framework.Event;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Network
{
    /// <summary>
    /// 网络组件。
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Game Framework/Network")]
    public sealed class NetworkComponent : FrameworkComponent
    {
        private INetworkManager m_NetworkManager = null;
        private EventComponent m_EventComponent = null;

        /// <summary>
        /// 获取网络频道数量。
        /// </summary>
        public int NetworkChannelCount
        {
            get
            {
                return m_NetworkManager.NetworkChannelCount;
            }
        }

        /// <summary>
        /// 游戏框架组件初始化。
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            m_NetworkManager = FrameworkEntry.GetModule<INetworkManager>();
            if (m_NetworkManager == null)
            {
                Log.Info("Network manager is invalid.");
                return;
            }

            m_NetworkManager.NetworkConnected += OnNetworkConnected;
            m_NetworkManager.NetworkClosed += OnNetworkClosed;
            m_NetworkManager.NetworkMissHeartBeat += OnNetworkMissHeartBeat;
            m_NetworkManager.NetworkError += OnNetworkError;
            m_NetworkManager.NetworkCustomError += OnNetworkCustomError;
        }

        private void Start()
        {
            m_EventComponent = GameEntry.GetComponent<EventComponent>();
            if (m_EventComponent == null)
            {
                Log.Info("Event component is invalid.");
                return;
            }
        }

        /// <summary>
        /// 检查是否存在网络频道。
        /// </summary>
        /// <param name="name">网络频道名称。</param>
        /// <returns>是否存在网络频道。</returns>
        public bool HasNetworkChannel(string name)
        {
            return m_NetworkManager.HasNetworkChannel(name);
        }

        /// <summary>
        /// 获取网络频道。
        /// </summary>
        /// <param name="name">网络频道名称。</param>
        /// <returns>要获取的网络频道。</returns>
        public INetworkChannel GetNetworkChannel(string name)
        {
            return m_NetworkManager.GetNetworkChannel(name);
        }

        /// <summary>
        /// 获取所有网络频道。
        /// </summary>
        /// <returns>所有网络频道。</returns>
        public INetworkChannel[] GetAllNetworkChannels()
        {
            return m_NetworkManager.GetAllNetworkChannels();
        }

        /// <summary>
        /// 获取所有网络频道。
        /// </summary>
        /// <param name="results">所有网络频道。</param>
        public void GetAllNetworkChannels(List<INetworkChannel> results)
        {
            m_NetworkManager.GetAllNetworkChannels(results);
        }

        /// <summary>
        /// 创建网络频道。
        /// </summary>
        /// <param name="name">网络频道名称。</param>
        /// <param name="serviceType">网络服务类型。</param>
        /// <param name="networkChannelHelper">网络频道辅助器。</param>
        /// <returns>要创建的网络频道。</returns>
        public INetworkChannel CreateNetworkChannel(string name, ServiceType serviceType, INetworkChannelHelper networkChannelHelper)
        {
            return m_NetworkManager.CreateNetworkChannel(name, serviceType, networkChannelHelper);
        }

        /// <summary>
        /// 销毁网络频道。
        /// </summary>
        /// <param name="name">网络频道名称。</param>
        /// <returns>是否销毁网络频道成功。</returns>
        public bool DestroyNetworkChannel(string name)
        {
            return m_NetworkManager.DestroyNetworkChannel(name);
        }

        private void OnNetworkConnected(object sender, Framework.Network.NetworkConnectedEventArgs e)
        {
            //m_EventComponent.Fire(this, NetworkConnectedEventArgs.Create(e));
        }

        private void OnNetworkClosed(object sender, Framework.Network.NetworkClosedEventArgs e)
        {
            //m_EventComponent.Fire(this, NetworkClosedEventArgs.Create(e));
        }

        private void OnNetworkMissHeartBeat(object sender, Framework.Network.NetworkMissHeartBeatEventArgs e)
        {
            //m_EventComponent.Fire(this, NetworkMissHeartBeatEventArgs.Create(e));
        }

        private void OnNetworkError(object sender, Framework.Network.NetworkErrorEventArgs e)
        {
            //m_EventComponent.Fire(this, NetworkErrorEventArgs.Create(e));
        }

        private void OnNetworkCustomError(object sender, Framework.Network.NetworkCustomErrorEventArgs e)
        {
            //m_EventComponent.Fire(this, NetworkCustomErrorEventArgs.Create(e));
        }
    }
}