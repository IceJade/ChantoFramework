using UnityEngine;

/// <summary>
/// 序列帧动画
/// </summary>
namespace Chanto
{
    [RequireComponent(typeof(IMImage))]
    public class UISequenceFrameAnimation : ReverseSequenceFrameAnimation
    {
        private IMImage imgFragment;

        protected override void Start()
        {
            base.Start();

            imgFragment = this.GetComponent<IMImage>();
        }

        protected override void Update()
        {
            if (null == imgFragment)
                return;

            base.Update();
        }

        protected override void SetSprite(Sprite sprite)
        {
            imgFragment.sprite = sprite;
        }
    }
}