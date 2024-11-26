using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class GameLauncher : MonoBehaviour
    {
        public static GameLauncher Instance { get; private set; }

        [SerializeField]
        public ThreadPriority BackgroundLoadingPriority = ThreadPriority.High;

        [SerializeField]
        private GameObject m_ReporterPrefab;

        [SerializeField]
        private List<GameObject> m_DontDestroyOnLoadGameObjects;

        private GameObject m_Reporter;

        private ChantoEntry XXGameEntry = new();

        public float DeltaTime { get; private set; }
        public float RealtimeSinceStartup { get; private set; }

        public const int MAX_SCREEN_WIDTH = 1080;

        void Awake()
        {
            DontDestroyOnLoad(gameObject);

            Application.backgroundLoadingPriority = BackgroundLoadingPriority;
            Application.lowMemory += OnLowMemoryCallback;

            Instance = this;

            //#if GAME_DEBUG || UNITY_EDITOR
            //            ShowReporter();

            //            if(isShowDebugInfo)
            //                gameObject.AddComponent<FPSUI>();
            //#endif

            InitScreenWH();

            if (m_DontDestroyOnLoadGameObjects != null && m_DontDestroyOnLoadGameObjects.Count > 0)
            {
                foreach (var go in m_DontDestroyOnLoadGameObjects)
                {
                    if (!go) continue;
                    DontDestroyOnLoad(go);
                }
            }
        }

        private void InitScreenWH()
        {
            if (Screen.width > MAX_SCREEN_WIDTH)
            {
                int originWidth = Screen.width;
                int originHeight = Screen.height;
                int scaleHeight = (int)((Screen.height / (float)Screen.width) * MAX_SCREEN_WIDTH);
                Screen.SetResolution(MAX_SCREEN_WIDTH, scaleHeight, true);
                Log.InfoFormat("InitScreenWH , originWidth ={0},   originHeight = {1} , ChangeScreenWidth = {2},  ChangeScreenHeight = {3}", originWidth, originHeight, MAX_SCREEN_WIDTH, scaleHeight);
            }
        }

        private void Start()
        {
            InitBuindInModules();
            XXGameEntry.StartGame();
        }

        public void ShowReporter()
        {
            if (!m_ReporterPrefab || m_Reporter)
            {
                return;
            }

            m_Reporter = Instantiate(m_ReporterPrefab);
            DontDestroyOnLoad(m_Reporter);
        }

        private void InitBuindInModules()
        {
        }

        private void Update()
        {
            DeltaTime = Time.deltaTime;
            RealtimeSinceStartup = Time.realtimeSinceStartup;

            XXGameEntry.BeforeUpdate();
            XXGameEntry.Update(DeltaTime, RealtimeSinceStartup);

            //UResourceManager.Tick();
        }

        private void LateUpdate()
        {
            XXGameEntry.LateUpdate();
        }

        private void FixedUpdate()
        {
            XXGameEntry.FixedUpdate();
        }

        private void OnApplicationFocus(bool focus)
        {
            XXGameEntry.OnApplicationFocus(focus);
        }

        private void OnApplicationPause(bool pause)
        {
            XXGameEntry.OnApplicationPause(pause);
        }

        private void OnApplicationQuit()
        {
            Application.lowMemory -= OnLowMemoryCallback;
            XXGameEntry.Shutdown();
        }

        private void OnLowMemoryCallback()
        {
            Debug.Log("[OnLowMemory]");
            Debug.Log("Total Reserved memory by Unity: " + UnityEngine.Profiling.Profiler.GetTotalReservedMemoryLong() + " Bytes");
            Debug.Log("- Allocated memory by Unity: " + UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong() + " Bytes");
            Debug.Log("- Reserved but not allocated: " + UnityEngine.Profiling.Profiler.GetTotalUnusedReservedMemoryLong() + " Bytes");

            XXGameEntry.OnLowMemoryCallback();

            System.GC.Collect();
            //System.GC.WaitForPendingFinalizers();
        }
    }
}