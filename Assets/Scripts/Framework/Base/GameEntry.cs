using Chanto;
using Framework.Event;
using Framework.Sound;
using Framework.UI;
using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// 游戏入口。
    /// </summary>
    public static class GameEntry
    {
        private const string UnityFrameworkVersion = "3.1.3";
        private static readonly LinkedList<FrameworkComponent> s_FrameworkComponents = new LinkedList<FrameworkComponent>();

        /// <summary>
        /// 游戏框架所在的场景编号。
        /// </summary>
        internal const int FrameworkSceneId = 0;

        /// <summary>
        /// 获取 Unity 游戏框架版本号。
        /// </summary>
        public static string Version
        {
            get
            {
                return UnityFrameworkVersion;
            }
        }

        private static BaseComponent gameBase;
        public static BaseComponent GameBase
        {
            get
            {
                if (gameBase == null)
                {
                    gameBase = GetComponent<BaseComponent>();
                }
                return gameBase;
            }
        }

        private static EventComponent eventCom;
        public static EventComponent Event
        {
            get
            {
                if (eventCom == null)
                {
                    eventCom = GetComponent<EventComponent>();
                }
                return eventCom;
            }
        }

        private static UIComponent uiCom;
        public static UIComponent UI
        {
            get
            {
                if (uiCom == null)
                {
                    uiCom = GetComponent<UIComponent>();
                }
                return uiCom;
            }
        }

        static private Localization localization;
        public static Localization Localization
        {
            get
            {
                if (localization == null)
                    localization = Localization.Instance;
                return localization;
            }
        }

        private static SoundComponent sound;
        public static SoundComponent Sound
        {
            get
            {
                if (sound == null)
                {
                    sound = GetComponent<SoundComponent>();
                }
                return sound;
            }
        }

        // static private CustomDataProxy customDataProxy;
        // public static CustomDataProxy Data
        // {
        //     get
        //     {
        //         if (customDataProxy == null)
        //             customDataProxy = DataProxyManager.Instance.RetrieveProxy<CustomDataProxy>();
        //         return customDataProxy;
        //     }
        // }

        //static private CustomDataTableProxy customDataTableProxy;
        //public static CustomDataTableProxy DataTable
        //{
        //    get
        //    {
        //        if (customDataTableProxy == null)
        //            customDataTableProxy = DataProxyManager.Instance.RetrieveProxy<CustomDataTableProxy>();
        //        return customDataTableProxy;
        //    }
        //}

        //public static TimerComponent Timer => TimerComponent.Instance;

        //private static DataConfigComponent dataConfig;
        //public static DataConfigComponent DataConfig
        //{
        //    get
        //    {
        //        if (dataConfig == null)
        //        {
        //            dataConfig = GetComponent<DataConfigComponent>();
        //        }
        //        return dataConfig;
        //    }
        //}

        // static private GlobalDataProxy globalDataProxy;
        // public static GlobalDataProxy GlobalData
        // {
        //     get
        //     {
        //         if (globalDataProxy == null)
        //             globalDataProxy = DataProxyManager.Instance.RetrieveProxy<GlobalDataProxy>();
        //         return globalDataProxy;
        //     }
        // }

        // private static GuideControllerComponent guide;
        // public static GuideControllerComponent Guide
        // {
        //     get
        //     {
        //         if (guide == null)
        //         {
        //             guide = GetComponent<GuideControllerComponent>();
        //         }
        //         return guide;
        //     }
        // }

        // private static MailDBProxy mailDBHelp;
        // public static MailDBProxy MailDBHelp
        // {
        //     get
        //     {
        //         if (mailDBHelp == null)
        //         {
        //             mailDBHelp = DataProxyManager.Instance.RetrieveProxy<MailDBProxy>();
        //         }
        //         return mailDBHelp;
        //     }
        // }

        // private static PersonMailDBProxy personMailDBHelp;
        // public static PersonMailDBProxy PersonMailDBHelp
        // {
        //     get
        //     {
        //         if (personMailDBHelp == null)
        //         {
        //             personMailDBHelp = DataProxyManager.Instance.RetrieveProxy<PersonMailDBProxy>();
        //         }
        //         return personMailDBHelp;
        //     }
        // }

        // private static SDKProxy sdkProxy;
        // public static SDKProxy Sdk
        // {
        //     get
        //     {
        //         if (sdkProxy == null)
        //             sdkProxy = DataProxyManager.Instance.RetrieveProxy<SDKProxy>();
        //
        //         return sdkProxy;
        //     }
        // }

        static private TableManager table;
        public static TableManager Table
        {
            get
            {
                if (null == table)
                {
                    table = TableManager.Instance;
                }

                return table;
            }
        }

        /// <summary>
        /// 获取游戏框架组件。
        /// </summary>
        /// <typeparam name="T">要获取的游戏框架组件类型。</typeparam>
        /// <returns>要获取的游戏框架组件。</returns>
        public static T GetComponent<T>() where T : FrameworkComponent
        {
            return (T)GetComponent(typeof(T));
        }

        /// <summary>
        /// 获取游戏框架组件。
        /// </summary>
        /// <param name="type">要获取的游戏框架组件类型。</param>
        /// <returns>要获取的游戏框架组件。</returns>
        public static FrameworkComponent GetComponent(Type type)
        {
            LinkedListNode<FrameworkComponent> current = s_FrameworkComponents.First;
            while (current != null)
            {
                if (current.Value.GetType() == type)
                {
                    return current.Value;
                }

                current = current.Next;
            }

            return null;
        }

        /// <summary>
        /// 获取游戏框架组件。
        /// </summary>
        /// <param name="typeName">要获取的游戏框架组件类型名称。</param>
        /// <returns>要获取的游戏框架组件。</returns>
        public static FrameworkComponent GetComponent(string typeName)
        {
            LinkedListNode<FrameworkComponent> current = s_FrameworkComponents.First;
            while (current != null)
            {
                Type type = current.Value.GetType();
                if (type.FullName == typeName || type.Name == typeName)
                {
                    return current.Value;
                }

                current = current.Next;
            }

            return null;
        }

        /// <summary>
        /// 关闭游戏框架。
        /// </summary>
        /// <param name="shutdownType">关闭游戏框架类型。</param>
        public static void Shutdown(ShutdownType shutdownType)
        {
            Log.InfoFormat("Shutdown Game Framework ({0})...", shutdownType.ToString());
            BaseComponent baseComponent = GetComponent<BaseComponent>();
            if (baseComponent != null)
            {
                baseComponent.Shutdown();
                baseComponent = null;
            }

            s_FrameworkComponents.Clear();


            if (shutdownType == ShutdownType.None)
            {
                return;
            }

            if (shutdownType == ShutdownType.Restart)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(FrameworkSceneId);
                return;
            }

            if (shutdownType == ShutdownType.Quit)
            {
                Application.Quit();
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#endif
                return;
            }
        }

        /// <summary>
        /// 注册游戏框架组件。
        /// </summary>
        /// <param name="gameFrameworkComponent">要注册的游戏框架组件。</param>
        internal static void RegisterComponent(FrameworkComponent gameFrameworkComponent)
        {
            if (gameFrameworkComponent == null)
            {
                Log.Error("Game Framework component is invalid.");
                return;
            }

            Type type = gameFrameworkComponent.GetType();
            LinkedListNode<FrameworkComponent> current = s_FrameworkComponents.First;
            while (current != null)
            {
                if (current.Value.GetType() == type)
                {
                    Log.ErrorFormat("Game Framework component type '{0}' is already exist.", type.FullName);
                    return;
                }

                current = current.Next;
            }

            s_FrameworkComponents.AddLast(gameFrameworkComponent);
        }
    }
}
