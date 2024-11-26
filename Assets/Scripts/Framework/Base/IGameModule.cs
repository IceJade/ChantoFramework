namespace Framework
{
    public interface IGameModule : System.IComparable, IGame
    {
        int Priority { get; }
        bool Updatable { get; }
        bool FixedUpdatable { get; }
        bool LateUpdatable { get; }
        void Init();
    }
}