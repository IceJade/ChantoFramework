using System.Collections;

namespace Framework
{
    public interface IGameEntry : IGameLifeCircle
    {
        void StartGame();
        void BeforeUpdate();
        void OnLowMemoryCallback();
    }
}