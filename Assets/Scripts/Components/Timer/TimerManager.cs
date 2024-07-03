using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public enum TimerType
    {
        FIXED_DURATION,
        FIXED_REALTIME_DURATION,
        EVERY_FRAME,
    }

    public class TimerManager : GameBaseSingletonModule<TimerManager>
    {
        private int curIndex = 0;
        private int MaxNum = 100000;
        private Dictionary<string, int> m_TaskKeyCache = new();
        private readonly Dictionary<int,TimerTask> m_TaskDic = new Dictionary<int,TimerTask>();
        private readonly Dictionary<int,TimerTask> m_TaskDicCathe = new Dictionary<int,TimerTask>();//设立缓冲区，避免remove同时add
        private readonly List<int> m_RemoveCathe = new List <int>();//设立缓冲区，避免remove同时add
        private readonly List<int> m_RemoveList = new List <int>();

        public override bool Updatable => true;

        public override void Initialize()
        {
        }

        public override void OnApplicationQuit()
        {
            m_TaskDic.Clear();
        }

        public void RemoveAllTask()
        {
            m_TaskDic.Clear();
        }

        public override void Update(float delta, float realElapsedTime)
        {
            foreach (var item in m_RemoveCathe)
            {
                if (m_TaskDicCathe.ContainsKey(item))
                {
                    m_TaskDicCathe.Remove(item);
                }
                if (m_TaskDic.ContainsKey(item))
                {
                    m_TaskDic.Remove(item);
                }
            }
            m_RemoveCathe.Clear();
            
            
            foreach (var item in m_TaskDic)
            {
                if (item.Value != null && item.Value.Update(delta) && item.Value.Execute())
                {
                    m_RemoveList.Add(item.Key);
                }
            }

            for (int i = 0; i < m_RemoveList.Count; ++i)
            {
                m_TaskDic.Remove(m_RemoveList[i]);
               
            }
            m_RemoveList.Clear();

            foreach (var item in m_TaskDicCathe)
            {
                m_TaskDic[item.Key] = item.Value;
            }
            m_TaskDicCathe.Clear();
            
           
            
        }

        /// <summary>
        /// Adds the one shot task.
        /// </summary>
        /// <param name="startTime">Start delay.</param>
        /// <param name="executable">Executable.</param>
        public int AddOneShotTask(float startTime, Func<bool> executable)
        {
            return AddTask(TimerType.FIXED_DURATION, startTime, 0, 1, executable);
        }

        /// <summary>
        /// Adds the realtime one shot task.
        /// </summary>
        /// <param name="startTime">Start delay.</param>
        /// <param name="executable">Executable.</param>
        public int AddRealtimeOneShotTask(float startTime, Func<bool> executable)
        {
            return AddTask(TimerType.FIXED_REALTIME_DURATION, Time.realtimeSinceStartup + startTime, 0, 1, executable);
        }

        /// <summary>
        /// Adds the repeat task.
        /// </summary>
        /// <param name="startTime">Start delay.</param>
        /// <param name="interval">Interval.</param>
        /// <param name="repeatTimes">Repeat times, -1 means always.</param>
        /// <param name="executable">Executable.</param>
        public int AddRepeatTask(float startTime, float interval, int repeatTimes, Func<bool> executable, string key = null)
        {
            return AddTask(TimerType.FIXED_DURATION, startTime, interval, repeatTimes, executable, key);
        }
        
        public int AddRepeatTask(float startTime, float interval, int repeatTimes, Func<float,bool> executable, string key = null)
        {
            return AddTask(TimerType.FIXED_DURATION, startTime, interval, repeatTimes, executable, key);
        }

        /// <summary>
        /// Adds the realtime repeat task.
        /// </summary>
        /// <param name="startTime">Start delay.</param>
        /// <param name="interval">Interval.</param>
        /// <param name="repeatTimes">Repeat times, -1 means always.</param>
        /// <param name="executable">Executable.</param>
        public int AddRealtimeRepeatTask(float startTime, float interval, int repeatTimes, Func<bool> executable)
        {
            return AddTask(TimerType.FIXED_REALTIME_DURATION, Time.realtimeSinceStartup + startTime, interval,
                repeatTimes, executable);
        }

        /// <summary>
        /// Adds the frame execute task.
        /// </summary>
        /// <param name="executable">Executable.</param>
        public int AddFrameExecuteTask(Func<bool> executable)
        {
            return AddTask(TimerType.EVERY_FRAME, 0, 0, 0, executable);
        }

        private int AddTask(TimerType timerType, float startTime, float interval, int repeatTimes, Func<bool> executable, string key = null)
        {
            curIndex = curIndex + 1;
            TimerTask timeTask = new TimerTask
            {
                timerType = timerType, startTime = startTime, interval = interval, repeatTimes = repeatTimes, executable = executable, uid = curIndex
            };
            m_TaskDicCathe[curIndex] = timeTask;
            if(key.IsNotNullAndEmpty()) m_TaskKeyCache.SetOrAdd(key, curIndex);
            return curIndex;
        }
        
        private int AddTask(TimerType timerType, float startTime, float interval, int repeatTimes, Func<float, bool> executable, string key = null)
        {
            curIndex = curIndex + 1;
            TimerTask timeTask = new TimerTask
            {
                timerType = timerType, startTime = startTime, interval = interval, repeatTimes = repeatTimes, deltExecutable = executable, uid = curIndex
            };
            m_TaskDicCathe[curIndex] = timeTask;
            if (key.IsNotNullAndEmpty()) m_TaskKeyCache.SetOrAdd(key, curIndex);
            return curIndex;
        }

        public void RemoveTask(int taskIndex)
        {
            if (taskIndex <= 0)
                return;

            m_RemoveCathe.Add(taskIndex);
        }

        public void RemoveTaskByKey(string taskKey)
        {
            if (taskKey.IsNullOrEmpty())
                return;

            int taskIndex = 0;
            if (!m_TaskKeyCache.TryGetValue(taskKey, out taskIndex))
                return;

            m_RemoveCathe.Add(taskIndex);
            m_TaskKeyCache.Remove(taskKey);
        }

        public bool HasTaskByKey(string taskKey)
        {
            if (taskKey.IsNullOrEmpty())
                return false;
            return  m_TaskKeyCache.ContainsKey(taskKey);
        }
    }

    public class TimerTask
    {
        public TimerType timerType;
        public float startTime;
        public float interval;
        public int repeatTimes;
        public Func<bool> executable;
        public Func<float, bool> deltExecutable;
        private int executeTimes;
        private float deltaTime;
        private float curDelta;
        private bool started;
        public int uid;

        public bool Update(float delta)
        {
            curDelta = delta;
            if (timerType == TimerType.FIXED_DURATION)
                deltaTime += delta;

            if (!started)
            {
                if (timerType == TimerType.FIXED_REALTIME_DURATION)
                {
                    if (Time.realtimeSinceStartup >= startTime)
                    {
                        started = true;
                        deltaTime = startTime + interval;
                        return true;
                    }
                }
                else
                {
                    if (deltaTime >= startTime)
                    {
                        started = true;
                        deltaTime -= startTime;
                        return true;
                    }
                }
            }
            else
            {
                if (timerType == TimerType.EVERY_FRAME)
                {
                    deltaTime = 0;
                    return true;
                }

                if (timerType == TimerType.FIXED_REALTIME_DURATION)
                {
                    if (Time.realtimeSinceStartup >= deltaTime)
                    {
                        deltaTime += interval;
                        return true;
                    }
                }
                else
                {
                    if (deltaTime >= interval)
                    {
                        deltaTime -= interval;
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Execute this instance.
        /// </summary>
        /// <returns>if <see langword="true"/>, remove this task</returns>
        public bool Execute()
        {
            executeTimes++;
            if (executable != null)
                return executable() || executeTimes == repeatTimes;
            else if (deltExecutable != null)
                return deltExecutable(curDelta) || executeTimes == repeatTimes;
            else
                return true;
        }
    }
}