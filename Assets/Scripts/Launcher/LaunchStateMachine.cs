using System;

public abstract class LaunchState : IState<LaunchStateEnum, LaunchContext>
{
    public virtual float ProgressValue => -1f;
    public static bool TureCondition(LaunchContext context) { return true; }
    public static bool FalseCondition(LaunchContext context) { return false; }
    
    protected Func<LaunchContext, bool> Condition = TureCondition;

    protected LaunchStateEnum FailurePassTo;
    
    public abstract LaunchStateEnum Name { get; }
    public virtual bool Updatable { get; protected set; } = false;
    public IStateMachine<LaunchContext, LaunchStateEnum> Parent { get; set; }
    public LaunchContext Context => Parent.Context;

    public void Init()
    {
        OnInit();
    }

    public void Enter(LaunchStateEnum from)
    {
        if (Condition.Invoke(Context))
        {
            OnEnter(from);
        }
        else
        {
            Transition(FailurePassTo);
        }
    }

    public void Update(float deltaTime)
    {
        if (Updatable)
        {
            OnUpdate(deltaTime);
        }
    }

    public void Exit(LaunchStateEnum to)
    {
        OnExit(to);
    }
    
    public void Destroy()
    {
        OnDestroy();
    }
    
    protected virtual void OnInit() {}
    protected abstract void OnEnter(LaunchStateEnum from);
    protected virtual void OnUpdate(float deltaTime) { }
    protected abstract void OnExit(LaunchStateEnum to);
    protected virtual void OnDestroy() { }

    protected void Transition(LaunchStateEnum to)
    {
        Parent.Transition(to);
    }
    
    public override bool Equals(object obj) => this.Equals(obj as LaunchState);

    public bool Equals(IState<LaunchStateEnum, LaunchContext> other)
    {
        if (other == null)
        {
            return false;
        }

        return other.Name == Name;
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }

    public static bool operator ==(LaunchState lhs, LaunchState rhs)
    {
        if (lhs is null)
        {
            if (rhs is null)
            {
                return true;
            }

            return false;
        }

        return lhs.Equals(rhs);
    }
    public static bool operator !=(LaunchState lhs, LaunchState rhs) => !(lhs == rhs);
}

/*
 *                                                                                                                             v--------------服务器状态正常-----------^
 *                                                                                                                             v                                      ^
 *                                           >>首次进入>> LuaState                                   >>没有服务器信息>> FetchServerListState       >>>>>连接失败>>> CheckServerStatusState
 *                                           ^               v                                      ^                          v                 ^
 * SplashScreenState >> InitGameModulesState >>重新加载>> LoadXMLState >> PrepareConnectServerState >>有服务器信息>>>>> ConnectServerState >>>>>>>>>>>>>连接成功>>> LoginState >>登陆成功>> AppUpdateState >> WaitingPushInit
 *          ^                                                                         ^                                                                                      v
 *          ^                                                                         ^---------------------------------------登陆失败----------------------------------------
 *          ^
 *          ^
 *          ----------------------------------------------------------------------------- ReloadGameState
 * 
 */
/// <summary>
/// 游戏流程状态机
/// </summary>
public class LaunchStateMachine : UStateMachine<LaunchContext, LaunchStateEnum>
{
    public LaunchStateMachine()
    {
        this.Context = new LaunchContext();

        // todo : 先屏蔽掉更新状态
        AddState<SplashScreenState>(true);
        AddState<AssetsUpdateState>();
        AddState<InitGameModuleState>();
        //AddState<PrepareConnectServerState>();
        AddState<FetchServerListState>();
        AddState<ConnectServerState>();
        //AddState<CheckServerStatusState>();
        AddState<LoginState>();
        //AddState<AppUpdateState>();
        //AddState<LoadXMLState>();
        //AddState<WaitingPushInitState>();
        //AddState<AuthState>();
        //AddState<PrepareGameState>();
        AddState<GameState>();
        //AddState<ReloadGameState>();
        AddState<LaunchErrorState>();
        //AddState<QuitGame>();
    }

    public bool AddState<T>(bool isDefault = false) where T : LaunchState
    {
        LaunchState state = System.Activator.CreateInstance<T>();
        return AddState(state, isDefault);
    }

    public new LaunchState FindState(LaunchStateEnum type)
    {
        return base.FindState(type) as LaunchState;
    }

    protected override void OnAnyStateEnter(LaunchStateEnum stateName)
    {
        LaunchState enterState = FindState(stateName);
        if (enterState.ProgressValue >= 0)
        {
            Context.SetLoadingBarProgress(enterState.ProgressValue);
        }
        UnityEngine.Debug.Log($"#StateMachine# [Enter] {stateName}");
    }

    protected override void OnAnyStateExit(LaunchStateEnum stateName)
    {
        UnityEngine.Debug.Log($"#StateMachine# [Exit] {stateName}");
    }

    public void LoginAuthSuccess()
    {
        if (m_currentState != null && m_currentState.Name == LaunchStateEnum.Auth)
        {
            Transition(LaunchStateEnum.PrepareGame);
        }
    }
}