using System;
using System.Collections.Generic;

public interface IState<TStateType, TContext> : IEquatable<IState<TStateType, TContext>>
{
    public TStateType Name { get; }
    public bool Updatable { get; }

    public IStateMachine<TContext, TStateType> Parent { get; set; }

    public void Init();
    public void Enter(TStateType from);
    public void Update(float deltaTime);
    public void Exit(TStateType to);
    public void Destroy();
}

public interface IStateMachine<TContext, TStateType>
{
    public bool IsRunning { get; }
    public TContext Context { get; set; }
    public IState<TStateType, TContext> CurrentState { get; }
    public IState<TStateType, TContext> DefaultState { get; }
    public void SetDefaultState(TStateType name);
    public bool AddState(IState<TStateType, TContext> state, bool isDefault);
    public IState<TStateType, TContext> FindState(TStateType name);
    public void Start();
    public bool Transition(TStateType to);
    public void Stop();
    public void Restart();
    public void Update(float deltaTime);
}

public abstract class UStateMachine<TContext, TStateType> : IStateMachine<TContext, TStateType>
{
    public bool IsRunning { get { return isRunning; } }
    public TContext Context { get; set; }
    public IState<TStateType, TContext> CurrentState { get { return m_currentState; } }
    public IState<TStateType, TContext> DefaultState { get { return m_defaultState; } }

    protected bool isRunning = false;
    protected IState<TStateType, TContext> m_currentState;
    protected IState<TStateType, TContext> m_defaultState;

    protected Dictionary<TStateType, IState<TStateType, TContext>> m_stateDict;

    public UStateMachine()
    {
        this.m_stateDict = new Dictionary<TStateType, IState<TStateType, TContext>>();
    }

    public bool AddState(IState<TStateType, TContext> state, bool isDefault)
    {
        if (state == null)
        {
            return false;
        }

        if (!m_stateDict.ContainsKey(state.Name))
        {
            m_stateDict.Add(state.Name, state);
            state.Parent = this;
            state.Init();

            if (isDefault)
            {
                m_defaultState = state;
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetDefaultState(TStateType name)
    {
        if (m_stateDict.TryGetValue(name, out IState<TStateType, TContext> state))
        {
            m_defaultState = state;
        }
        else
        {
            UnityEngine.Debug.Log("special default state is not exits!" + name);
        }
    }

    public IState<TStateType, TContext> FindState(TStateType name)
    {
        return m_stateDict.TryGetValue(name, out IState<TStateType, TContext> state) ? state : null;
    }
 
    public void Start()
    {
        if (m_defaultState == null)
        {
            UnityEngine.Debug.Log("UStateMachine: Default State Not Found When StateMachine Start");
            return;
        }

        if (isRunning)
        {
            UnityEngine.Debug.Log("UStateMachine: StateMachine Is Running, But Still Call Start Function");
            return;
        }

        InternalEnter(m_defaultState);
        isRunning = true;
    }

    public bool Transition(TStateType to)
    {
        if (m_stateDict.TryGetValue(to, out IState<TStateType, TContext> state))
        {
            return InternalEnter(state);
        }
        UnityEngine.Debug.LogError("Transition target not found: " + to);
        return false;
    }

    public void Update(float deltaTime)
    {
        if (!isRunning)
        {
            return;
        }

        m_currentState?.Update(deltaTime);
    }

    public void Stop()
    {
        isRunning = false;
    }

    public void Restart()
    {
        isRunning = false;
        Start();
    }

    protected bool InternalEnter(IState<TStateType, TContext> dstState)
    {
        if (dstState == null || m_currentState == dstState)
        {
            return false;
        }

        TStateType from = default(TStateType);
        if (m_currentState != null) 
        { 
            from = m_currentState.Name;
            m_currentState.Exit(dstState.Name);
            OnAnyStateExit(m_currentState.Name);
        }

        OnAnyStateEnter(dstState.Name);
        m_currentState = dstState;
        m_currentState.Enter(from);
        return true;
    }

    protected virtual void OnAnyStateEnter(TStateType stateName) {}
    protected virtual void OnAnyStateExit(TStateType stateName) {}
}