using Framework.Event;
using Framework.Network;
using Framework.Pool;

namespace UnityFramework.Runtime
{
    /// <summary>
    /// 网络连接成功事件。
    /// </summary>
    public sealed class NetworkConnectedEventArgs : GameEventArgs
    {
        /// <summary>
        /// 网络连接成功事件编号。
        /// </summary>
        public static readonly int EventId = typeof(NetworkConnectedEventArgs).GetHashCode();

        /// <summary>
        /// 初始化网络连接成功事件的新实例。
        /// </summary>
        public NetworkConnectedEventArgs()
        {
            NetworkChannel = null;
            UserData = null;
        }

        /// <summary>
        /// 获取网络频道。
        /// </summary>
        public INetworkChannel NetworkChannel
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取用户自定义数据。
        /// </summary>
        public object UserData
        {
            get;
            private set;
        }

        /// <summary>
        /// 创建网络连接成功事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的网络连接成功事件。</returns>
        public static NetworkConnectedEventArgs Create(Framework.Network.NetworkConnectedEventArgs e)
        {
            NetworkConnectedEventArgs networkConnectedEventArgs = LogicPoolManager.Instance.Spawn<NetworkConnectedEventArgs>();
            networkConnectedEventArgs.NetworkChannel = e.NetworkChannel;
            networkConnectedEventArgs.UserData = e.UserData;
            return networkConnectedEventArgs;
        }

        /// <summary>
        /// 清理网络连接成功事件。
        /// </summary>
        public override void Clear()
        {
            NetworkChannel = null;
            UserData = null;
        }
    }
}
