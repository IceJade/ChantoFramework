using Framework;

namespace Chanto
{
    /// <summary>
    /// 全局数据管理器
    /// </summary>
    public class GlobalController : ControllerBaseSingleton<GlobalController>
    {
        public PlayerData Player { get; set; } = new();

        protected override void OnInit()
        {

        }

        protected override void OnLoginSuccess()
        {

        }

        protected override void OnDestroy()
        {

        }
    }
}