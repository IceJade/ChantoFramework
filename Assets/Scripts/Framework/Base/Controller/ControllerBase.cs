using System;

namespace Framework
{

    public interface IControllerBaseInit
    {
        void Init();
        void InitData();
    }

    public abstract class ControllerBase
    {
        protected ControllerBase()
        {
            ControllerContainer.Instance.AddControllerBase(this);
        }
        // OnInit 与  OnPurge 配对使用
        public abstract void InitInstance();
        // 构造时调用
        protected virtual void OnInit() { }

        public abstract void DestroyInstance();
        // 销毁时调用
        protected virtual void OnDestroy() { }

        // 重新登录、断线重连时调用
        // 一般用于清理数据
        public void Reset()
        {
            OnReset();
        }

        protected virtual void OnReset() { }

        // 自动开启定时器
        private bool isUpdateOpen = false;

        public void OpenUpdateSecond()
        {
            if (!isUpdateOpen)
            {
                ControllerContainer.Instance.AddUpdateSecond(this);
                isUpdateOpen = true;
            }

           
        }
        public void CloseUpdateSecond()
        {
            if (isUpdateOpen)
            {
                ControllerContainer.Instance.RemoveUpdateSecond(this);
                isUpdateOpen = false;
            }
        }
        public bool IsUpdateOpen
        {
            get => isUpdateOpen;
        }
        // 固定1s
        public void UpdateSecond(float elapsedTime)
        {
            OnUpdateSecond(elapsedTime);
            onEnterFrame(elapsedTime);
        }
        // 定时器 1秒跑一次
        protected virtual void OnUpdateSecond(float elapsedTime) { }

        //  Cocos 惯用定时器
        public virtual void onEnterFrame(float elapsedTime) { }

        public virtual void OnApplicationFocus(bool focus) { }

        public virtual void OnApplicationPause(bool paused) { }

    }

    public abstract class ControllerBaseSingleton<T> : ControllerBase where T : ControllerBase, new()
    {
        private static readonly object _lock = new object();
        
        public static bool IsInstance()
        {
            return _instance != null;
        }
        public static T getInstance()
        {
            return Instance;
        }

        protected static T _instance = null;
        public static T Instance
        {
            get
            {
                // Double-Checked Locking
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = Activator.CreateInstance<T>();
                            _instance.InitInstance();
                        }
                    }
                }

                return _instance;
            }
        }
        
     
        public sealed override void InitInstance()
        {
            OnInit();
        }
        public sealed override void DestroyInstance()
        {
            OnDestroy();
            _instance = null;
        }
        public static bool isInst()
        {
            return null != Instance ? true : false;
        }
    }
}