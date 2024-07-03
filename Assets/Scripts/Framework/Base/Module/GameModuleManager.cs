using System.Collections.Generic;

namespace Framework
{
    public class GameModuleManager : IGameLifeCircle
    {
        private List<IGameModule> updateList;
        private List<IGameModule> lateUpdateList;
        private List<IGameModule> fixedUpdateList;
        private Dictionary<System.Type, IGameModule> modules;

        public GameModuleManager()
        {
            updateList = new List<IGameModule>();
            lateUpdateList = new List<IGameModule>();
            fixedUpdateList = new List<IGameModule>();

            modules = new Dictionary<System.Type, IGameModule>();
        }

        /// <summary>
        /// 添加指定类型模块 
        /// </summary>
        /// <typeparam name="T">IGameModule</typeparam>
        /// <returns></returns>
        public T Add<T>() where T : GameBaseModule
        {
            System.Type moduleType = typeof(T);
            IGameModule module;
            if (!modules.TryGetValue(moduleType, out module))
            {
                module = System.Activator.CreateInstance<T>();
                Internal_Add(moduleType, module);              
            }
            return (T)module;
        }

        public void Add(IGameModule instance)
        {
            System.Type moduleType = instance.GetType();
            if (!modules.ContainsKey(moduleType))
            {
                Internal_Add(moduleType, instance);
            }
        }

        private void Internal_Add(System.Type moduleType, IGameModule module)
        {
            if (module.Updatable)
            {
                updateList.Add(module);
            }

            if (module.LateUpdatable)
            {
                lateUpdateList.Add(module);
            }

            if (module.FixedUpdatable)
            {
                fixedUpdateList.Add(module);
            }
            modules.Add(moduleType, module);
            module.Initialize();
        }

        /// <summary>
        /// 获取指定类型的游戏模块 
        /// </summary>
        /// <typeparam name="T">IGameModule</typeparam>
        /// <returns></returns>
        public T Get<T>() where T : IGameModule
        {
            IGameModule module;
            if (modules.TryGetValue(typeof(T), out module))
            {
                return (T)module;
            }
            return default(T);
        }

        public void Initialize()
        {
            foreach (var pair in modules)
            {
                pair.Value.Initialize();
            }
        }

        /// <summary>
        /// 每帧调用(只针对开启Updatable的模块生效)
        /// </summary>
        /// <param name="elapsedTime"> The interval in seconds from the last frame to the current one </param>
        /// <param name="realElapsedTime"> The real time in seconds since the game started </param>
        public void Update(float elapsedTime, float realElapsedTime)
        {
            int moduleCount = updateList.Count;
            if (moduleCount > 0)
            {
                IGameModule module;
                for (int i = 0; i < moduleCount; ++i)
                {
                    module = updateList[i];
                    module.Update(elapsedTime, realElapsedTime);
                }
            }
        }

        /// <summary>
        /// 每帧调用(只针对开启FixedUpdatable的模块生效)
        /// </summary>
        public void FixedUpdate()
        {
            int moduleCount = fixedUpdateList.Count;
            if (moduleCount > 0)
            {
                IGameModule module;
                for (int i = 0; i < moduleCount; ++i)
                {
                    module = fixedUpdateList[i];
                    module.FixedUpdate();
                }
            }
        }

        /// <summary>
        /// 每帧调用(只针对开启LateUpdatable的模块生效)
        /// </summary>
        public void LateUpdate()
        {
            int moduleCount = lateUpdateList.Count;
            if (moduleCount > 0)
            {
                IGameModule module;
                for (int i = 0; i < moduleCount; ++i)
                {
                    module = lateUpdateList[i];
                    module.LateUpdate();
                }
            }
        }

        public void OnApplicationFocus(bool focus)
        {
            foreach (var pair in modules)
            {
                pair.Value.OnApplicationFocus(focus);
            }
        }

        public void OnApplicationPause(bool paused)
        {
            foreach (var pair in modules)
            {
                pair.Value.OnApplicationPause(paused);
            }
        }

        public void OnApplicationQuit()
        {
            foreach (var pair in modules)
            {
                pair.Value.OnApplicationQuit();
            }
        }
    }
}