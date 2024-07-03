using UnityEngine;
using UnityEngine.EventSystems;

/********************************************************************************
 * 说    明: 切换按钮,点击后自动切换按钮状态, 配合ToggleButtonGroup脚本使用
 * 版    本: 1.0
 * 创建时间: 2021.06.09
 * 作    者: zhoumingfeng
 * 修改记录:
 * 
 ********************************************************************************/
namespace Chanto
{
    public class ToggleButton : BaseButton
    {
        public enum E_State
        {
            UnChecked = 0,
            Checked,
            Disable
        }

        // 选中时显示的节点
        [SerializeField] protected GameObject CheckedNode;

        // 不选中时显示的节点
        [SerializeField] protected GameObject UncheckNode;

        // 不可用时显示的节点
        [SerializeField] protected GameObject DisableNode;

        // Toggle按钮组
        [SerializeField] protected ToggleButtonGroup group;

        // 当前是否选中
        [SerializeField] protected E_State state = E_State.UnChecked;

        // 是否自动通知刷新组内按钮的状态
        [SerializeField] protected bool AutoNotify = true;

        protected override void Awake()
        {
            base.Awake();

            this.Init();

            this.SetState(state, false);
        }

        protected void Init()
        {
            if (this.IsRegisted())
                return;

            if (null != group)
                group.Register(this);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            this.Checked();

            base.OnPointerClick(eventData);
        }

        /// <summary>
        /// 是否注册
        /// </summary>
        /// <returns></returns>
        public bool IsRegisted()
        {
            if(null == group)
            {
                Log.Error("ToggleButtonGroup is not bind.");
                return false;
            }

            return group.Contains(this);
        }

        /// <summary>
        /// 选中(其它按钮状态会自动刷新)
        /// </summary>
        protected virtual void Checked()
        {
            // 如果不主动通知刷新,那么返回
            if (!this.AutoNotify)
                return;

            // 如果是选中状态和不可用状态,那么返回
            if (this.state == E_State.Checked 
                || this.state == E_State.Disable)
                return;

            this.SetState(E_State.Checked, true);
        }

        /// <summary>
        /// 获得当前状态
        /// </summary>
        /// <returns></returns>
        public E_State GetState()
        {
            return this.state;
        }

        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="_state">要设置的状态</param>
        /// <param name="isNotify">是否通知刷新组内其它按钮的状态</param>
        public void SetState(E_State _state, bool isNotify = true)
        {
            this.Init();

            if (null != CheckedNode)
                CheckedNode.SetActiveEx(_state == E_State.Checked);

            if (null != UncheckNode)
                UncheckNode.SetActiveEx(_state == E_State.UnChecked);

            if (null != DisableNode)
                DisableNode.SetActiveEx(_state == E_State.Disable);

            if (null != group && _state == E_State.Checked && isNotify)
                group.NotifyToggleOn(this);

            this.state = _state;
        }

        /// <summary>
        /// 刷新状态
        /// 说明:如果当前为不可用状态,那么忽略状态刷新,如果不希望忽略,那么请使用SetState接口
        /// </summary>
        /// <param name="_state">状态</param>
        public void RefreshState(E_State _state)
        {
            if (this.state == E_State.Disable)
                return;

            this.SetState(_state, true);
        }
    }
}
