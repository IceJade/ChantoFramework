using UnityEngine;

/// <summary>
/// 逆序播放的序列帧动画类
/// </summary>
namespace Chanto
{
    public class ReverseSequenceFrameAnimation : BaseSequenceFrameAnimation
    {
        protected bool _reverse_play = false;

        /// <summary>
        /// 逆序播放(倒播)
        /// </summary>
        public override void ReversePlay()
        {
            this.AutoPlay = true;
            this._is_stop = false;
            this._reverse_play = true;
            this._currentIndex = this.EndIndex;
        }

        protected override void Update()
        {
            if (this._reverse_play)
            {
                if (this._is_stop || !this.AutoPlay) return;

                if (this.StartIndex < 0 || this.EndIndex <= 0 || this.Interval <= 0)
                    return;

                if (Time.time - this._currentTime < this.Interval)
                    return;

                this._currentTime = Time.time;

                if (this._currentIndex < this.StartIndex)
                {
                    if (!this.IsLoop)
                    {
                        this._is_stop = true;
                        this._completeCallback?.Invoke();
                        return;
                    }
                    else
                    {
                        this._currentIndex = this.EndIndex;
                    }
                }

                this.SetSprite(this._currentIndex);

                this._currentIndex--;
            }
            else
            {
                base.Update();
            }
        }
    }
}