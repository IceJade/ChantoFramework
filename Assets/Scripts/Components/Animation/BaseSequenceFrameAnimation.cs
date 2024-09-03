using Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

/// <summary>
/// 序列帧动画基类
/// </summary>
namespace Chanto
{
    public class BaseSequenceFrameAnimation : BaseMono
    {
        public bool IsLoop = true;
        public bool AutoPlay = true;
        public string ImageFormat;
        public int StartIndex = 0;
        public int EndIndex = 5;
        public float Interval = 0.1f;

        protected float _currentTime = 0;
        protected int _currentIndex = 0;

        protected List<object> _cacheAsset = new();
        protected Dictionary<int, Sprite> _cacheSprites = new();

        protected Action _completeCallback = null;

        protected bool _is_stop = false;

        public void SetCompleteCallback(Action callback)
        {
            _completeCallback = callback;
        }

        /// <summary>
        /// 播放动画
        /// </summary>
        public void Play()
        {
            this.AutoPlay = true;
            this._is_stop = false;
        }

        /// <summary>
        /// 重新播放动画(从第一张图片开始播放)
        /// </summary>
        public void Replay()
        {
            this.AutoPlay = true;
            this._is_stop = false;
            this._currentIndex = 0;
        }

        /// <summary>
        /// 逆序播放(倒播)
        /// </summary>
        public virtual void ReversePlay()
        {

        }

        protected virtual void Start()
        {
            this._is_stop = false;
        }

        protected virtual void Update()
        {
            if (this._is_stop || !this.AutoPlay) return;

            if (this.StartIndex < 0 || this.EndIndex <= 0 || this.Interval <= 0)
                return;

            if(this._currentTime <= 0)
            {
                this._currentTime = Time.time;
                this._currentIndex = this.StartIndex;

                SetSprite(this._currentIndex);
            }
            else
            {
                if (Time.time - this._currentTime < this.Interval)
                    return;

                this._currentTime = Time.time;

                this._currentIndex++;

                if (this._currentIndex > this.EndIndex)
                {
                    if(!this.IsLoop)
                    {
                        this._is_stop = true;
                        this._completeCallback?.Invoke();
                        return;
                    }
                    else
                    {
                        this._currentIndex = this.StartIndex;
                    }
                }

                SetSprite(this._currentIndex);
            }
        }

        protected virtual void OnDestroy()
        {
            this.UnloadAsset();
        }

        protected virtual void SetSprite(int index)
        {
            if (index < StartIndex || index > EndIndex)
                return;

            if (_cacheSprites.ContainsKey(index))
            {
                SetSprite(_cacheSprites[index]);
                return;
            }

            var spriteName = string.Format(ImageFormat, index);
            LoadImage(spriteName, index);
        }

        protected virtual void SetSprite(Sprite sprite)
        {

        }

        protected virtual void LoadImage(string spriteName, int index)
        {
            if (string.IsNullOrEmpty(spriteName))
                return;

            string tmpSpriteName = spriteName.Replace(Constant.Sprites.ImageExtension, "");

            string assetName = this.GetSpriteConfigName(tmpSpriteName);
            var loadType = SpriteConfigManager.Instance.GetLoadType(assetName);
            if (loadType == E_SpriteLoadType.None)
                return;

            // 获得Bundle名称
            string bundleName = SpriteConfigManager.Instance.GetBunldeName(assetName);

            if (loadType == E_SpriteLoadType.Atlas)
            {// 图集加载模式
                string atlasName = SpriteConfigManager.Instance.GetAtlasName(assetName);
                if (string.IsNullOrEmpty(bundleName) || string.IsNullOrEmpty(atlasName))
                    return;

                ResourceManager.Instance.LoadAssetAsync<SpriteAtlas>(bundleName, atlasName, (key, asset, err) =>
                {
                    if (null != this)
                        this.enabled = true;

                    if (string.IsNullOrEmpty(err))
                    {
                        if (null != this && null != asset)
                        {
                            Sprite sprite = null;
                            string tempSpriteName = string.Empty;
                            SpriteAtlas atlas = asset as SpriteAtlas;
                            if (null == atlas)
                            {
                                this.UnloadAssetWithObject(asset);
                                asset = null;

                                return;
                            }


                            sprite = AtlasUtils.GetOrAddCacheSprite(atlas, assetName);
                            if (null == sprite)
                            {
                                this.UnloadAssetWithObject(asset);
                                asset = null;

                                return;
                            }

                            if (this._currentIndex == index)
                                SetSprite(sprite);

                            this._cacheAsset.Add(asset);
                            this._cacheSprites.SetOrAdd(index, sprite);
                        }
                        else
                        {
                            this.UnloadAssetWithObject(asset);
                        }
                    }
                });
            }
            else
            {// AssetBundle加载模式
                if (string.IsNullOrEmpty(bundleName))
                {
                    int length = tmpSpriteName.LastIndexOf('.');
                    if (length >= 0)
                        bundleName = tmpSpriteName.Substring(0, length).ToLowerInvariant();
                }

                if (string.IsNullOrEmpty(bundleName))
                    return;

                ResourceManager.Instance.LoadAssetAsync<Sprite>(bundleName, assetName, (key, asset, err) =>
                {
                    if (null != this)
                        this.enabled = true;

                    if (string.IsNullOrEmpty(err))
                    {
                        if (null != this && null != asset)
                        {
                            var sprite = asset as Sprite;

                            if (null == sprite)
                            {
                                this.UnloadAssetWithObject(asset);
                                asset = null;
                                return;
                            }

                            if (this._currentIndex == index)
                                SetSprite(sprite);

                            this._cacheAsset.Add(asset);
                            this._cacheSprites.SetOrAdd(index, sprite);
                        }
                        else
                        {
                            this.UnloadAssetWithObject(asset);
                        }
                    }
                });
            }
        }

        protected string GetSpriteConfigName(string spriteName)
        {
            var tmpSpriteName = spriteName;
            if (spriteName.Contains(Constant.Sprites.ImageExtension))
                tmpSpriteName = spriteName.Replace(Constant.Sprites.ImageExtension, "");

            int start = tmpSpriteName.IndexOf("/");
            int last = tmpSpriteName.LastIndexOf("/");

            if (start == last)
                return tmpSpriteName;

            string spritePath = tmpSpriteName.Substring(0, last);
            last = spritePath.LastIndexOf("/");
            tmpSpriteName = tmpSpriteName.Substring(last + 1);

            return tmpSpriteName;
        }

        protected virtual void UnloadAsset()
        {
            this._cacheSprites.Clear();

            if (null != this._cacheAsset)
            {
                for(int i = 0; i < this._cacheAsset.Count; i++)
                    this.UnloadAssetWithObject(this._cacheAsset[i]);

                this._cacheAsset.Clear();
            }
        }

        protected virtual void UnloadAssetWithObject(object obj, bool immdiately = false)
        {
            if (ResourceManager.Instance != null)
                ResourceManager.Instance.UnloadAssetWithObject(obj, immdiately);
        }

        public void CleanCacheSprites()
        {
            this._cacheSprites.Clear();
        }
    }
}