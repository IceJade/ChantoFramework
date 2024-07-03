//------------------------------------------------------------
// Game Framework v3.x
using Framework.Event;
using Framework.Pool;
using Framework.UI;
using System;
using System.Collections.Generic;

namespace Framework
{
    /// <summary>
    /// 游戏框架入口。
    /// </summary>
    public static class FrameworkEntry
    {
        private const string FrameworkVersion = "3.1.3";
        private static readonly LinkedList<FrameworkModule> s_FrameworkModules = new LinkedList<FrameworkModule>();
        private static LinkedList<FrameworkModule>.Enumerator enumerator;

        /// <summary>
        /// 获取游戏框架版本号。
        /// </summary>
        public static string Version
        {
            get
            {
                return FrameworkVersion;
            }
        }

        /// <summary>
        /// 所有游戏框架模块轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        public static void Update(float elapseSeconds, float realElapseSeconds)
        {
            UIManager.Instance.Update(elapseSeconds, realElapseSeconds);
            EventManager.Instance.Update(elapseSeconds, realElapseSeconds);
            ObjectPoolManager.Instance.Update(elapseSeconds, realElapseSeconds);

            enumerator = s_FrameworkModules.GetEnumerator();
            while (enumerator.MoveNext())
            {
                enumerator.Current.Update(elapseSeconds, realElapseSeconds);
            }

            //foreach (FrameworkModule module in s_FrameworkModules)
            //{
            //    module.Update(elapseSeconds, realElapseSeconds);
            //}
        }

        /// <summary>
        /// 关闭并清理所有游戏框架模块。
        /// </summary>
        public static void Shutdown()
        {
            UIManager.Instance.Shutdown();
            EventManager.Instance.Shutdown();
            LogicPoolManager.Instance.Shutdown();
            ObjectPoolManager.Instance.Shutdown();

            for (LinkedListNode<FrameworkModule> current = s_FrameworkModules.Last; current != null; current = current.Previous)
            {
                current.Value.Shutdown();
            }

            s_FrameworkModules.Clear();
        }

        /// <summary>
        /// 获取游戏框架模块。
        /// </summary>
        /// <typeparam name="T">要获取的游戏框架模块类型。</typeparam>
        /// <returns>要获取的游戏框架模块。</returns>
        /// <remarks>如果要获取的游戏框架模块不存在，则自动创建该游戏框架模块。</remarks>
        public static T GetModule<T>() where T : class
        {
            Type interfaceType = typeof(T);
            if (!interfaceType.IsInterface)
            {
                throw new FrameworkException(string.Format("You must get module by interface, but '{0}' is not.", interfaceType.FullName));
            }

            if (!interfaceType.FullName.StartsWith("Framework."))
            {
                throw new FrameworkException(string.Format("You must get a Game Framework module, but '{0}' is not.", interfaceType.FullName));
            }

            string moduleName = string.Format("{0}.{1}", interfaceType.Namespace, interfaceType.Name.Substring(1));
            Type moduleType = Type.GetType(moduleName);
            if (moduleType == null)
            {
                throw new FrameworkException(string.Format("Can not find Game Framework module type '{0}'.", moduleName));
            }

            return GetModule(moduleType) as T;
        }

        /// <summary>
        /// 获取游戏框架模块。
        /// </summary>
        /// <param name="moduleType">要获取的游戏框架模块类型。</param>
        /// <returns>要获取的游戏框架模块。</returns>
        /// <remarks>如果要获取的游戏框架模块不存在，则自动创建该游戏框架模块。</remarks>
        private static FrameworkModule GetModule(Type moduleType)
        {
            foreach (FrameworkModule module in s_FrameworkModules)
            {
                if (module.GetType() == moduleType)
                {
                    return module;
                }
            }

            return CreateModule(moduleType);
        }

        /// <summary>
        /// 创建游戏框架模块。
        /// </summary>
        /// <param name="moduleType">要创建的游戏框架模块类型。</param>
        /// <returns>要创建的游戏框架模块。</returns>
        private static FrameworkModule CreateModule(Type moduleType)
        {
            FrameworkModule module = (FrameworkModule)Activator.CreateInstance(moduleType);
            if (module == null)
            {
                throw new FrameworkException(string.Format("Can not create module '{0}'.", module.GetType().FullName));
            }

            LinkedListNode<FrameworkModule> current = s_FrameworkModules.First;
            while (current != null)
            {
                if (module.Priority > current.Value.Priority)
                {
                    break;
                }

                current = current.Next;
            }

            if (current != null)
            {
                s_FrameworkModules.AddBefore(current, module);
            }
            else
            {
                s_FrameworkModules.AddLast(module);
            }

            return module;
        }
    }
}
