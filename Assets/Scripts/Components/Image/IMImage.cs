using Chanto;
using Framework;
using System;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using SpriteAtlasManager = Framework.SpriteAtlasManager;

public class IMImage : Image
{
    private bool isLoading = false;
    private object cacheAsset = null;
    private string cacheSpriteName = null;
    private string waitingLoadSpriteName = null;

    /// <summary>
    /// 加载图片回调;
    /// </summary>
    /// <param name="spriteTarget">图片对象</param>
    /// <param name="result">返回结果:0-成功 1-加载失败 2-图片重复加载 3-图片路径为空 4-图片正在加载中 5-目标为null 6-表中没有配置此图片 7-表中没有配置图片所在的图集 8-图片配置错误 9-图片Bundle错误</param>
    [XLua.CSharpCallLua]
    public delegate void LoadSpriteCallback(object spriteTarget, int result);

    /// <summary>
    /// 加载图片
    /// </summary>
    /// <param name="atlasName">图集名称</param>
    /// <param name="spriteName">图片名称</param>
    /// <param name="defaultSpriteName">默认图片</param>
    /// <param name="loadCallback">加载回调</param>
    /// <param name="isNeedNativeSize">是否自动大小</param>
    /// <param name="loadingEnable">加载时是否可用</param>
    public void LoadAsyncWithAtlas(string atlasName, string spriteName, string defaultSpriteName = "", LoadSpriteCallback loadCallback = null, bool isNeedNativeSize = false)
    {
        this.LoadAsync("{atlasName}/{spriteName}", defaultSpriteName, loadCallback, isNeedNativeSize);
        //this.LoadAsync(spriteName, defaultSpriteName, null, isNeedNativeSize);
    }

    /// <summary>
    /// 异步加载图片资源;
    /// 当加载多个图片且都未加载完毕时,以第一个图片作为显示对象
    /// </summary>
    /// <param name="spriteName">图片名称</param>
    /// <param name="defaultSpriteName">默认图片名称</param>
    /// <param name="loadCallback">图片加载回调</param>
    /// <param name="isNeedNativeSize">是否设置原大小尺寸</param>
    /// <param name="loadingEnable">加载时是否可用</param>
    public void LoadAsync(string spriteName, string defaultSpriteName = "", LoadSpriteCallback loadCallback = null, bool isNeedNativeSize = false, bool loadingEnable = false)
    {
        if (string.IsNullOrEmpty(spriteName))
        {
            loadCallback?.Invoke(this, 3);

            return;
        }

        //#if UNITY_EDITOR
        //        var _sprite = SpriteConfigManager.Instance.GetSprite(spriteName);
        //        if(null == _sprite)
        //        {
        //            _sprite = SpriteConfigManager.Instance.GetSprite(defaultSpriteName);
        //        }

        //        if(null != _sprite)
        //            this.sprite = _sprite;

        //        return;
        //#endif

        string tmpSpriteName = spriteName.Replace(Constant.Sprites.ImageExtension, "");
        if (!string.IsNullOrEmpty(cacheSpriteName)
            && cacheSpriteName == tmpSpriteName)
        {
            loadCallback?.Invoke(this, 2);

            return;
        }

        // 正在加载,那么返回;
        if (!loadingEnable && this.isLoading)
        {
            loadCallback?.Invoke(this, 4);

            return;
        }

        string assetName = this.GetSpriteConfigName(tmpSpriteName);
        var loadType = SpriteConfigManager.Instance.GetLoadType(assetName);
        if (loadType == E_SpriteLoadType.None)
        {
            // 如果找不到图片,那么使用默认图片
            if (!string.IsNullOrEmpty(defaultSpriteName))
            {
                assetName = this.GetSpriteConfigName(defaultSpriteName);
                loadType = SpriteConfigManager.Instance.GetLoadType(assetName);
                if (loadType == E_SpriteLoadType.None)
                {
                    Log.ErrorFormat("找不到图片: {0} 和 {1} , 请检查是否导入...", tmpSpriteName, defaultSpriteName);

                    this.enabled = true;
                    this.cacheSpriteName = string.Empty;

                    loadCallback?.Invoke(this, 6);

                    return;
                }
            }
            else
            {
                Log.ErrorFormat("Can not find {0} in sprites config...", tmpSpriteName);

                this.enabled = true;
                this.cacheSpriteName = string.Empty;

                loadCallback?.Invoke(this, 6);

                return;
            }
        }

        // 解决资源加载完毕之前显示白图的问题;
        this.enabled = loadingEnable;

        // 正在加载标记;
        this.isLoading = true;

        // 缓存图片名称;
        this.cacheSpriteName = tmpSpriteName;

        // 获得Bundle名称
        string bundleName = SpriteConfigManager.Instance.GetBunldeName(assetName);

        if (loadType == E_SpriteLoadType.Atlas)
        {// 图集加载模式
            string atlasName = SpriteConfigManager.Instance.GetAtlasName(assetName);
            if (string.IsNullOrEmpty(bundleName) || string.IsNullOrEmpty(atlasName))
            {
                this.enabled = true;
                this.UnloadAsset();
                this.SetDefaultSprite();

                loadCallback?.Invoke(this, 8);

                Log.ErrorFormat($"图片{tmpSpriteName}或者其对应的图集AssetBunle可能没有设置，请检查!");

                return;
            }

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

                            sprite = SpriteAtlasManager.Instance.DefaultSprite;
                            tempSpriteName = Constant.Sprites.DefaultSpriteName;
                        }
                        else
                        {
                            sprite = AtlasUtils.GetOrAddCacheSprite(atlas, assetName);
                            if (null == sprite)
                            {
                                this.UnloadAssetWithObject(asset);
                                asset = null;

                                sprite = SpriteAtlasManager.Instance.DefaultSprite;
                                tempSpriteName = Constant.Sprites.DefaultSpriteName;
                            }
                            else
                            {
                                tempSpriteName = tmpSpriteName;
                            }
                        }

                        // 如果存在缓存,那么先释放掉;
                        this.UnloadAsset();

                        // 赋值新图片
                        this.sprite = sprite;

                        // 缓存图片名称
                        this.cacheSpriteName = tempSpriteName;

                        // 动态加载的图片赋一个名称, 否则分析内存时图片名称为null;
                        //#if (_DEBUG || UNITY_EDITOR)
                        //                        this.sprite.name = assetName + Constant.Tag.Name_Dynamic_load_Sprite;
                        //#endif

                        if (isNeedNativeSize)
                            this.SetNativeSize();

                        this.cacheAsset = asset;

                        loadCallback?.Invoke(this, 0);
                    }
                    else
                    {
                        this.UnloadAssetWithObject(asset);
                        this.cacheSpriteName = string.Empty;

                        loadCallback?.Invoke(this, 5);
                    }
                }
                else
                {
                    this.cacheSpriteName = string.Empty;
                    loadCallback?.Invoke(this, 1);

                    Log.Error($"Load sprite fail, {tmpSpriteName}, error:{err}");
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
            {
                this.enabled = true;
                this.cacheSpriteName = string.Empty;
                loadCallback?.Invoke(this, 9);

                return;
            }

            ResourceManager.Instance.LoadAssetAsync<Sprite>(bundleName, assetName, (key, asset, err) =>
            {
                if (null != this)
                    this.enabled = true;

                if (string.IsNullOrEmpty(err))
                {
                    if (null != this && null != asset)
                    {
                        sprite = asset as Sprite;
                        string tempSpriteName = string.Empty;

                        if (null == sprite)
                        {
                            this.UnloadAssetWithObject(asset);
                            asset = null;

                            sprite = SpriteAtlasManager.Instance.DefaultSprite;
                            tempSpriteName = Constant.Sprites.DefaultSpriteName;
                        }
                        else
                        {
                            tempSpriteName = tmpSpriteName;
                        }

                        // 如果存在缓存,那么先释放掉;
                        this.UnloadAsset();

                        // 赋值新图片
                        this.sprite = sprite;

                        // 缓存图片名称
                        this.cacheSpriteName = tempSpriteName;

                        // 动态加载的图片赋一个名称, 否则分析内存时图片名称为null;
                        //#if (_DEBUG || UNITY_EDITOR)
                        //                        this.sprite.name = assetName + Constant.Tag.Name_Dynamic_load_Sprite;
                        //#endif

                        if (isNeedNativeSize)
                            this.SetNativeSize();

                        this.cacheAsset = asset;

                        loadCallback?.Invoke(this, 0);
                    }
                    else
                    {
                        this.UnloadAssetWithObject(asset);
                        this.cacheSpriteName = string.Empty;

                        loadCallback?.Invoke(this, 5);
                    }
                }
                else
                {
                    this.UnloadAssetWithObject(asset);

                    if (null != this)
                    {
                        this.UnloadAsset();
                        this.SetDefaultSprite();

                        if (isNeedNativeSize)
                            this.SetNativeSize();

                        loadCallback?.Invoke(this, 1);
                    }
                }
            });
        }
    }

    /// <summary>
    /// 异步加载图片资源
    /// 当加载多个图片且都未加载完毕时,以最后一个图片作为显示对象
    /// </summary>
    /// <param name="spriteName"></param>
    /// <param name="defaultSpriteName"></param>
    /// <param name="loadCallback"></param>
    /// <param name="isNeedNativeSize"></param>
    /// <param name="loadingEnable"></param>
    public void LoadAsyncEx(string spriteName, string defaultSpriteName = "", LoadSpriteCallback loadCallback = null, bool isNeedNativeSize = false)
    {
        if (spriteName.IsNullOrEmpty())
            return;

        string tmpSpriteName = spriteName.Replace(".png", "");
        if (this.isLoading && this.cacheSpriteName != tmpSpriteName)
        {
            this.waitingLoadSpriteName = tmpSpriteName;
            return;
        }

        this.LoadAsync(tmpSpriteName, defaultSpriteName, loadCallback: (obj, result) =>
        {
            if (this)
            {
                if (!string.IsNullOrEmpty(this.waitingLoadSpriteName) && this.waitingLoadSpriteName != this.cacheSpriteName)
                    this.LoadAsync(this.waitingLoadSpriteName, defaultSpriteName, loadCallback: (obj2, result2) =>
                    {
                        if (null != this)
                            this.waitingLoadSpriteName = string.Empty;

                        loadCallback?.Invoke(obj2, result2);
                    }, isNeedNativeSize, true);
                else
                    loadCallback?.Invoke(obj, result);
            }
            else
            {
                loadCallback?.Invoke(obj, result);
            }
        }, isNeedNativeSize, false);
    }

    public void SetSprite(Sprite sprite)
    {
        this.UnloadAsset();

        this.sprite = sprite;
    }

    protected override void OnDestroy()
    {
        this.UnloadAsset();
    }

    private void UnloadAsset()
    {
        if (null != this.cacheAsset)
        {
            this.UnloadAssetWithObject(this.cacheAsset);
            this.cacheAsset = null;
        }

        this.isLoading = false;
        this.cacheSpriteName = null;
    }

    private void UnloadAssetWithObject(object obj, bool immdiately = false)
    {
        if (ResourceManager.Instance != null)
            ResourceManager.Instance.UnloadAssetWithObject(obj, immdiately);
    }

    /// <summary>
    /// 设置默认图片
    /// </summary>
    private void SetDefaultSprite()
    {
        this.sprite = SpriteAtlasManager.Instance.DefaultSprite;
        this.cacheSpriteName = Constant.Sprites.DefaultSpriteName;
    }

    private string GetSpriteConfigName(string spriteName)
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
}
