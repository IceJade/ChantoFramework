using Framework.Event;

public class CommonEventArgs : GameEventArgs
{
    public bool ParamBool { get; set; }
    public int ParamInt { get; set; }
    public long ParamLong { get; set; }
    public float ParamFloat { get; set; }
    public string ParamString { get; set; }
    public object UserData { get; set; }

    public CommonEventArgs()
    {

    }

    public CommonEventArgs(EventId eventId, object userData = null) : base(eventId)
    {
        UserData = userData;
    }
    
    public CommonEventArgs(object userData) : base()
    {
        UserData = userData;
    }

    public override void SetParam(bool param) { ParamBool = param; }
    public override void SetParam(int param) { ParamInt = param; }
    public override void SetParam(long param) { ParamLong = param; }
    public override void SetParam(float param) { ParamFloat = param; }
    public override void SetParam(string param) { ParamString = param; }
    public override void SetParam(object param) { UserData = param; }

    public override void Clear()
    {
        base.Clear();

        ParamBool = false;
        ParamInt = 0;
        ParamFloat = 0.0f;
        ParamString = "";

        UserData = null;
    }
}