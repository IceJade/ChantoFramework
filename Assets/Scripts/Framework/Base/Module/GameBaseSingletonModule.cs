using System;

namespace Framework
{
    public abstract class GameBaseSingletonModule<T> : GameBaseModule where T : GameBaseModule, new()
    {
        protected static T _instance;
        private bool _isInit = false;
        private static readonly object _lock;

        static GameBaseSingletonModule()
        {
            _lock = new object();
        }

        protected GameBaseSingletonModule() { }

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
                            _instance = default(T) == null ? Activator.CreateInstance<T>() : default;
                        }
                    }
                }

                return _instance;
            }

            protected set
            {
                if (_instance != null)
                {
                    throw new Exception("Instance already exists.");
                }
                _instance = value;
            }
        }

        public sealed override void InitInstance()
        {
            if (_isInit)
                return;

            Init();
        }

        public override void Init()
        {
            _isInit = true;
        }

        public override void Reset()
        {
            _isInit = false;
        }
    }
}