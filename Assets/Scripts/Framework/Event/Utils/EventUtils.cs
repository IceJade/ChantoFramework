using Framework.Event;
using Framework.Pool;

/// <summary>
/// 事件工具
/// </summary>
public static class EventUtils
{
    public static T CreateArgs<T>() where T : BaseEventArgs, new()
    {
        var eventArgs = LogicPoolManager.Instance.Spawn<T>();
        return eventArgs;
    }

    public static T CreateArgs<T>(EventId eventId) where T : BaseEventArgs, new()
    {
        var eventArgs = CreateArgs<T>();
        eventArgs.SetEventId(eventId);
        return eventArgs;
    }

    public static T CreateArgs<T>(EventId eventId, bool param) where T : BaseEventArgs, new()
    {
        var eventArgs = CreateArgs<T>(eventId);
        eventArgs.SetParam(param);
        return eventArgs;
    }

    public static T CreateArgs<T>(EventId eventId, int param) where T : BaseEventArgs, new()
    {
        var eventArgs = CreateArgs<T>(eventId);
        eventArgs.SetParam(param);
        return eventArgs;
    }

    public static T CreateArgs<T>(EventId eventId, float param) where T : BaseEventArgs, new()
    {
        var eventArgs = CreateArgs<T>(eventId);
        eventArgs.SetParam(param);
        return eventArgs;
    }

    public static T CreateArgs<T>(EventId eventId, string param) where T : BaseEventArgs, new()
    {
        var eventArgs = CreateArgs<T>(eventId);
        eventArgs.SetParam(param);
        return eventArgs;
    }

    public static T CreateArgs<T>(EventId eventId, object param) where T : BaseEventArgs, new()
    {
        var eventArgs = CreateArgs<T>(eventId);
        eventArgs.SetParam(param);
        return eventArgs;
    }
}
