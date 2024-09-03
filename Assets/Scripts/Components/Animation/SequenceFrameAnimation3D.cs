using UnityEngine;

/// <summary>
/// 序列帧动画
/// </summary>
namespace Chanto
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SequenceFrameAnimation3D : ReverseSequenceFrameAnimation
    {
        private SpriteRenderer fragment;

        protected override void Start()
        {
            base.Start();

            fragment = this.GetComponent<SpriteRenderer>();
        }

        protected override void Update()
        {
            if (null == fragment)
                return;

            base.Update();
        }

        protected override void SetSprite(Sprite sprite)
        {
            fragment.sprite = sprite;
        }
    }
}