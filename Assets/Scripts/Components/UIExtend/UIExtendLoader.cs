/**********************************************************************************************
 * @说    明: 动态加载子资源组件,可减少依赖,节省内存
 * @作    者: zhoumingfeng
 * @版 本 号: V1.00
 * @创建时间: 2024.05.30
 **********************************************************************************************/
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace Chanto
{
    public class UIExtendLoader : BaseMono
    {
        #region 公有变量

        // 资源路径;
        public string AssetFile;

        [Header("显示时是否激活")]
        public bool IsLoadOnEnable = false;

        [Header("多资源加载时是否以最后一个资源作为显示对象")]
        public bool IsShowLastAsset = false;

        // 加载资源完成回调处理函数;
        [XLua.CSharpCallLua]
        public delegate void OnLoadAssetCompeleteHandler(GameObject obj, string entityID, int param);

        private OnLoadAssetCompeleteHandler m_OnLoadAssetCompelete = null;

        // 加载资源的完成回调;
        public OnLoadAssetCompeleteHandler OnLoadAssetCompelete
        {
            get { return m_OnLoadAssetCompelete; }
            set { m_OnLoadAssetCompelete = value; }
        }

        #endregion

        #region 私有变量

        // 参数;
        private int _Param = -1;

        private GameObject _targetObject = null;

        private object _cacheAsset = null;

        private bool _IsLoading = false;

        private string _waitingLoadAssetId = null;

        #endregion

        #region 框架接口

        private void OnEnable()
        {
            if (this.IsLoadOnEnable)
                this.Open();
        }

        /// <summary>
        /// UI销毁时释放资源;
        /// </summary>
        private void OnDestroy()
        {
            this.Close();
        }

        #endregion

        #region 公有接口

        /// <summary>
        /// 使用默认配置打开子实体(UI或者Effect);
        /// </summary>
        /// <param name="callback">加载资源成功的回调</param>
        public void Open(OnLoadAssetCompeleteHandler callback = null)
        {
            this.LoadAsset(this.AssetFile, callback);
        }

        /// <summary>
        /// 指定打开某个子UI实体;
        /// </summary>
        /// <param>/param>
        /// <returns></returns>
        public void OpenNew(string assetPath, OnLoadAssetCompeleteHandler callback = null)
        {
#if UNITY_EDITOR
            if(!File.Exists(assetPath))
            {
                callback?.Invoke(null, null, -1);
                Log.Error($"文件不存在, 请检查! {assetPath}");
                return;
            }
#endif
            if(this.IsShowLastAsset)
            {
                if(this._IsLoading && this.AssetFile != assetPath)
                {
                    this._waitingLoadAssetId = assetPath;
                    return;
                }

                this.LoadAsset(assetPath, (obj, entityID, param) =>
                {
                    if(null != this)
                    {
                        if (!string.IsNullOrEmpty(this._waitingLoadAssetId) && this._waitingLoadAssetId != this.AssetFile)
                        {
                            this.LoadAsset(this._waitingLoadAssetId, (obj2, entityID2, param2) =>
                            {
                                if (null != this)
                                    this._waitingLoadAssetId = string.Empty;

                                callback?.Invoke(obj2, entityID2, param2);
                            });
                        }
                        else
                        {
                            callback?.Invoke(obj, entityID, param);
                        }
                    }
                    else
                    {
                        callback?.Invoke(obj, entityID, param);
                    }
                });
            }
            else
            {
                this.LoadAsset(assetPath, callback);
            }
        }

        /// <summary>
        /// 重置子实体(某些特效循环播放可以调用这个接口)
        /// </summary>
        /// <param name="callback"></param>
        public void Reset(OnLoadAssetCompeleteHandler callback = null)
        {
            this.Hide();
            this.Open(callback);
        }
        
        
        public void SetScale(float scale)
        {
            if (_targetObject)
                _targetObject.transform.localScale = Vector3.one * scale;
        }

        /// <summary>
        /// 隐藏子实体;
        /// </summary>
        /// <param>/param>
        /// <returns></returns>
        public void Hide()
        {
            if (null != _targetObject)
                _targetObject.SetActiveEx(false);
        }

        /// <summary>
        /// 关闭子实体;
        /// </summary>
        /// <param>/param>
        /// <returns></returns>
        public void Close()
        {
            if (null != _targetObject)
            {
                Object.DestroyImmediate(_targetObject);
                _targetObject = null;
            }

            if (null != _cacheAsset)
            {
                ResourceHelper.UnloadAssetWithObject(_cacheAsset, true);
                _cacheAsset = null;
            }

            this.m_OnLoadAssetCompelete = null;
        }

        /// <summary>
        /// 是否已经打开子实体;
        /// </summary>
        /// <param>/param>
        /// <returns></returns>
        public bool IsOpened()
        {
            return null != _targetObject && _targetObject.activeSelf == true;
        }

        /// <summary>
        /// 获取子实体的组件;
        /// </summary>
        /// <param>/param>
        /// <returns></returns>
        public T GetExtendComponent<T>()
        {
            return gameObject.GetComponentInChildren<T>();
        }

        /// <summary>
        /// 设置参数(可以给多个子实体设置参数以用于区分);
        /// </summary>
        /// <param name="param">参数</param>
        public void SetParam(int param)
        {
            this._Param = param;
        }

        #endregion

        #region 私有接口

        /// <summary>
        /// 打开一个指定类型的实体;
        /// </summary>
        /// <param>/param>
        /// <returns></returns>
        private void LoadAsset(string assetFile, OnLoadAssetCompeleteHandler callback = null)
        {
            if (string.IsNullOrEmpty(assetFile))
            {
                callback?.Invoke(_targetObject, assetFile, this._Param);
                return;
            }

            if (_IsLoading)
            {
                Log.Warning($"UIExtend::LoadEntity ===> The last asset is not loading complete, entity id is {this.AssetFile}");
                callback?.Invoke(_targetObject, assetFile, this._Param);
                return;
            }

            if (callback != m_OnLoadAssetCompelete && null != callback)
                m_OnLoadAssetCompelete = callback;

            if (this.AssetFile == assetFile && null != _targetObject)
            {
                _targetObject.SetActiveEx(true);
                _IsLoading = false;
                
                OnLoadAssetCompelete?.Invoke(_targetObject, this.AssetFile, this._Param);
            }
            else
            {
                if (null != _targetObject)
                {
                    _targetObject.SetActiveEx(false);
                    Object.Destroy(_targetObject);
                    _targetObject = null;
                }

                if (null != _cacheAsset)
                {
                    ResourceHelper.UnloadAssetWithObject(_cacheAsset, true);
                    _cacheAsset = null;
                }

                // 获得资源路径
                if(string.IsNullOrEmpty(assetFile))
                {
                    callback?.Invoke(_targetObject, assetFile, this._Param);
                    return;
                }

                // 正在加载标记
                _IsLoading = true;

                this.AssetFile = assetFile;

                // 加载界面资源;
                ResourceHelper.LoadAsset<GameObject>(assetFile, (assetFile, asset, error) =>
                {
                    _IsLoading = false;

                    // 由于是异步加载,回调过来的时候有可能自己被销毁了,所以需要判断一下自身
                    if (null == this || null == this.transform || null == this.gameObject)
                    {
                        if(null != asset)
                            ResourceHelper.UnloadAssetWithObject(asset, true);

                        OnLoadAssetCompelete?.Invoke(null, assetFile, this._Param);
                        //Log.Error("load asset({0}) fail, error:{1}", assetFile, error);
                    }
                    else
                    {
                        var prefab = asset as GameObject;
                        if (prefab == null)
                        {
                            OnLoadAssetCompelete?.Invoke(null, assetFile, this._Param);
                            Log.Error($"load asset({assetFile}) fail, error:{error}");
                        }
                        else
                        {
                            _cacheAsset = asset;
                            _targetObject = Object.Instantiate(asset as Object, this.transform) as GameObject;
                            if (null != this && null != _targetObject)
                            {
                                _targetObject.SetActiveEx(true);
                                _targetObject.transform.localPosition = Vector3.zero;

                                OnLoadAssetCompelete?.Invoke(_targetObject, assetFile, this._Param);
                            }
                            else
                            {
                                ResourceHelper.UnloadAssetWithObject(_cacheAsset, true);
                                _cacheAsset = null;

                                OnLoadAssetCompelete?.Invoke(null, assetFile, this._Param);

                                Log.Error($"load asset({assetFile}) fail, object is null...");
                            }
                        }
                    }
                });
            }
        }

        #endregion

#if UNITY_EDITOR

        #region UNITY_EDITOR 模式下的代码

#if ODIN_INSPECTOR
        [Button("加载资源", ButtonSizes.Large)]
#endif
        private void LoadAsset()
        {
            this.DoLoadAsset();
        }

        /// <summary>
        /// 加载特效文件
        /// </summary>
        private void DoLoadAsset()
        {
            if(string.IsNullOrEmpty(this.AssetFile))
            {
                EditorUtility.DisplayDialog("提示", "没有配置资源ID,无法加载!", "确定");
                return;
            }

            string assetFile = AssetFile;
            if(string.IsNullOrEmpty(assetFile))
            {
                EditorUtility.DisplayDialog("提示", "资源不存在,请检查!", "确定");
                return;
            }

            if(!assetFile.EndsWith(".prefab"))
                assetFile += ".prefab";

            if(!File.Exists(assetFile))
            {
                EditorUtility.DisplayDialog("提示", "资源不存在,请检查!", "确定");
                return;
            }

            if(this.transform.childCount > 0)
            {
                EditorUtility.DisplayDialog("提示", "是否已经加载,请检查!", "确定");
                return;
            }

            var target = AssetDatabase.LoadMainAssetAtPath(assetFile) as GameObject;
            if(null == target)
            {
                EditorUtility.DisplayDialog("提示", "资源加载失败!", "确定");
                return;
            }

            //var entity = ObjectExtension.CreateInstClone(target, this.transform);
            var entity = Object.Instantiate(target, this.transform);
            if (null != entity)
            {
                RectTransform rectTransform = entity.GetComponent<RectTransform>();
                if (null != rectTransform)
                    rectTransform.anchoredPosition = Vector2.zero;
                else
                    entity.transform.localPosition = Vector3.zero;
            }
        }

        #endregion

#endif

    }
}
