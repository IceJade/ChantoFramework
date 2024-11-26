using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Framework
{
    public class GameModuleManager : IGame
    {
        private List<GameBaseModule> updateList = new();
        private List<GameBaseModule> lateUpdateList = new();
        private List<GameBaseModule> fixedUpdateList = new();
        private Dictionary<System.Type, GameBaseModule> modules = new();

        /// <summary>
        /// 添加指定类型模块 
        /// </summary>
        /// <typeparam name="T">GameBaseModule</typeparam>
        /// <returns></returns>
        public T Add<T>() where T : GameBaseModule
        {
            System.Type moduleType = typeof(T);
            GameBaseModule module;
            if (!modules.TryGetValue(moduleType, out module))
            {
                module = System.Activator.CreateInstance<T>();
                Internal_Add(moduleType, module);              
            }
            return (T)module;
        }

        public void Add(GameBaseModule instance)
        {
            System.Type moduleType = instance.GetType();
            if (!modules.ContainsKey(moduleType))
            {
                Internal_Add(moduleType, instance);
            }
        }

        private void Internal_Add(System.Type moduleType, GameBaseModule module)
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
            module.Init();
        }

        /// <summary>
        /// 获取指定类型的游戏模块 
        /// </summary>
        /// <typeparam name="T">GameBaseModule</typeparam>
        /// <returns></returns>
        public T Get<T>() where T : GameBaseModule
        {
            GameBaseModule module;
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
                pair.Value.Init();
            }
        }

        public void CreateInstance()
        {
            // 获取当前程序集
            var assembly = Assembly.GetExecutingAssembly();

            // 查找所有继承自 ControllerBase 的类型
            var moduleTypes = assembly.GetTypes()
                                           .Where(t => t.IsSubclassOf(typeof(GameBaseModule)) && !t.IsAbstract)
                                           .ToList();

            // 实例化这些类型
            foreach (var moduleType in moduleTypes)
            {
                if (modules.ContainsKey(moduleType))
                    continue;

                try
                {
                    var moduleInstance = Activator.CreateInstance(moduleType);
                    Log.Info($"实例化了 {moduleType.Name} 类");
                }
                catch (Exception ex)
                {
                    Log.Info($"实例化 {moduleType.Name} 时出错: {ex.Message}");
                }
            }
        }

        public void Init()
        {
            CreateInstance();

            foreach (var pair in modules)
            {
                pair.Value.InitInstance();
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
                GameBaseModule module;
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
                GameBaseModule module;
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
                GameBaseModule module;
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

        public void Shutdown()
        {
            foreach (var pair in modules)
            {
                pair.Value.Shutdown();
            }
        }
    }
}