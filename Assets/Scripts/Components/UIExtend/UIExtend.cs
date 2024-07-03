/**********************************************************************************************
 * @说    明: 动态加载子资源组件,可减少依赖,节省内存
 * @作    者: zhoumingfeng
 * @版 本 号: V1.00
 * @创建时间: 2021.8.6
 **********************************************************************************************/
using Framework;
using UnityEngine;
using Framework.UI;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace Chanto
{
    /// <summary>
    /// 资源类型
    /// </summary>
    public enum E_AssetType
    {
        UIForm,     // UI: 资源从UI表里加载
        Effect,     // 特效: 资源从Effect表里加载
        Entity      // 实体: 资源从Entity表里加载(此表可配置任何资源)
    }

    public class UIExtend : MonoBehaviour
    {
        #region 公有变量

        // 资源ID(UI或者特效);
        public string AssetId;

        // 资源类型;
        public E_AssetType AssetType = E_AssetType.UIForm;

        [Header("显示时是否激活")]
        public bool IsActiveOnEnable = false;

        [Header("是否检查特效等级")]
        public bool IsCheckEffectLevel = true;

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
            if (this.IsActiveOnEnable)
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
            this.LoadAsset(this.AssetId, this.AssetType, callback);
        }

        /// <summary>
        /// 指定打开某个子UI实体;
        /// </summary>
        /// <param>/param>
        /// <returns></returns>
        public void OpenNewUI(string assetId, OnLoadAssetCompeleteHandler callback = null)
        {
            if(this.IsShowLastAsset)
            {
                if(this._IsLoading && this.AssetId != assetId)
                {
                    this._waitingLoadAssetId = assetId;
                    return;
                }

                this.LoadAsset(assetId, E_AssetType.UIForm, callback:(obj, entityID, param) =>
                {
                    if(null != this)
                    {
                        if (!string.IsNullOrEmpty(this._waitingLoadAssetId) && this._waitingLoadAssetId != this.AssetId)
                        {
                            this.LoadAsset(this._waitingLoadAssetId, E_AssetType.UIForm, callback: (obj2, entityID2, param2) =>
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
                this.LoadAsset(assetId, E_AssetType.UIForm, callback);
            }
        }

        /// <summary>
        /// 指定打开某个特效实体;
        /// </summary>
        /// <param>/param>
        /// <returns></returns>
        public void OpenNewEffect(string assetId, OnLoadAssetCompeleteHandler callback = null)
        {
            if (this.IsShowLastAsset)
            {
                if (this._IsLoading && this.AssetId != assetId)
                {
                    this._waitingLoadAssetId = assetId;
                    return;
                }

                this.LoadAsset(assetId, E_AssetType.Effect, callback: (obj, entityID, param) =>
                {
                    if (null != this)
                    {
                        if (!string.IsNullOrEmpty(this._waitingLoadAssetId) && this._waitingLoadAssetId != this.AssetId)
                        {
                            this.LoadAsset(this._waitingLoadAssetId, E_AssetType.Effect, callback: (obj2, entityID2, param2) =>
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
                this.LoadAsset(assetId, E_AssetType.Effect, callback);
            }
        }

        /// <summary>
        /// 指定打开某个实体;
        /// </summary>
        /// <param>/param>
        /// <returns></returns>
        public void OpenNewEntity(string assetId, OnLoadAssetCompeleteHandler callback = null)
        {
            if (this.IsShowLastAsset)
            {
                if (this._IsLoading && this.AssetId != assetId)
                {
                    this._waitingLoadAssetId = assetId;
                    return;
                }

                this.LoadAsset(assetId, E_AssetType.Entity, callback: (obj, entityID, param) =>
                {
                    if (null != this)
                    {
                        if (!string.IsNullOrEmpty(this._waitingLoadAssetId) && this._waitingLoadAssetId != this.AssetId)
                        {
                            this.LoadAsset(this._waitingLoadAssetId, E_AssetType.Entity, callback: (obj2, entityID2, param2) =>
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
                this.LoadAsset(assetId, E_AssetType.Entity, callback);
            }
        }

        /// <summary>
        /// 打开一个指定类型的子实体;
        /// </summary>
        /// <param>/param>
        /// <returns></returns>
        public void OpenNewAsset(string assetId, E_AssetType assetType, OnLoadAssetCompeleteHandler callback = null)
        {
            if (this.IsShowLastAsset)
            {
                if (this._IsLoading && this.AssetId != assetId)
                {
                    this._waitingLoadAssetId = assetId;
                    return;
                }

                this.LoadAsset(assetId, assetType, callback: (obj, entityID, param) =>
                {
                    if (null != this)
                    {
                        if (!string.IsNullOrEmpty(this._waitingLoadAssetId) && this._waitingLoadAssetId != this.AssetId)
                        {
                            this.LoadAsset(this._waitingLoadAssetId, assetType, callback: (obj2, entityID2, param2) =>
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
                this.LoadAsset(assetId, assetType, callback);
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
        private void LoadAsset(string assetId, E_AssetType assetType, OnLoadAssetCompeleteHandler callback = null)
        {
            if (string.IsNullOrEmpty(assetId))
            {
                callback?.Invoke(_targetObject, assetId, this._Param);
                return;
            }

            if (_IsLoading)
            {
                Log.Warning($"UIExtend::LoadEntity ===> The last asset is not loading complete, entity id is {this.AssetId}");
                callback?.Invoke(_targetObject, assetId, this._Param);
                return;
            }

            if (callback != m_OnLoadAssetCompelete && null != callback)
                m_OnLoadAssetCompelete = callback;

            if (AssetId == assetId && null != _targetObject)
            {
                _targetObject.SetActiveEx(true);
                _IsLoading = false;
                
                OnLoadAssetCompelete?.Invoke(_targetObject, this.AssetId, this._Param);
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
                string assetFile = this.GetAssetFile(assetId, assetType);
                if(string.IsNullOrEmpty(assetFile))
                {
                    callback?.Invoke(_targetObject, assetId, this._Param);
                    return;
                }

                // 正在加载标记
                _IsLoading = true;

                // 加载界面资源;
                ResourceHelper.LoadAsset<GameObject>(assetFile, (assetPath, asset, error) =>
                {
                    _IsLoading = false;

                    // 由于是异步加载,回调过来的时候有可能自己被销毁了,所以需要判断一下自身
                    if (null == this || null == this.transform || null == this.gameObject)
                    {
                        if(null != asset)
                            ResourceHelper.UnloadAssetWithObject(asset, true);

                        OnLoadAssetCompelete?.Invoke(null, assetId, this._Param);
                        //Log.Error("load asset({0}) fail, error:{1}", assetFile, error);
                    }
                    else
                    {
                        var prefab = asset as GameObject;
                        if (prefab == null)
                        {
                            OnLoadAssetCompelete?.Invoke(null, assetId, this._Param);
                            Log.Error($"load asset({assetFile}) fail, error:{error}");
                        }
                        else
                        {
                            _cacheAsset = asset;
                            _targetObject = ObjectExtension.CreateInstClone(asset as Object, this.transform) as GameObject;
                            if (null != this && null != _targetObject)
                            {
                                _targetObject.SetActiveEx(true);

                                RectTransform rectTransform = _targetObject.GetComponent<RectTransform>();
                                if (null != rectTransform)
                                    rectTransform.anchoredPosition = Vector2.zero;
                                else
                                    _targetObject.transform.localPosition = Vector3.zero;

                                if(assetType == E_AssetType.Effect)
                                {// 加载特效时添加Canvas,以实现动静分离
                                    var canvas = _targetObject.GetComponent<Canvas>();
                                    if(null == canvas)
                                    {
                                        _targetObject.AddComponent<Canvas>();
                                        _targetObject.layer = this.gameObject.layer;
                                    }

                                    int layer = LayerMask.NameToLayer("UI");

                                    // 如果为UI层,那么强制改为UI层
                                    if (_targetObject.layer != layer && this.IsInUI())
                                        _targetObject.layer = layer;
                                }

                                OnLoadAssetCompelete?.Invoke(_targetObject, assetId, this._Param);
                            }
                            else
                            {
                                ResourceHelper.UnloadAssetWithObject(_cacheAsset, true);
                                _cacheAsset = null;

                                OnLoadAssetCompelete?.Invoke(null, assetId, this._Param);

                                Log.Error($"load asset({assetFile}) fail, object is null...");
                            }
                        }
                    }
                });

                AssetId = assetId;
            }
        }

        /// <summary>
        /// 获得资源文件
        /// </summary>
        /// <param name="assetType"></param>
        /// <returns></returns>
        private string GetAssetFile(string entityId, E_AssetType assetType)
        {
            string assetPath = string.Empty;

            if (string.IsNullOrEmpty(entityId))
                return assetPath;

            /*
            switch(assetType)
            {
                case E_AssetType.UIForm:
                    {
                        var datarow = GameEntry.Table.GetDataRow<LF.UiDataRow>(entityId);
                        if (null != datarow)
                            assetPath = datarow.AssetName;
                        else
                            Log.Error("UI配表中不存在这个ID:{0},请检查!", entityId);

                        break;
                    }
                case E_AssetType.Effect:
                    {
                        var datarow = GameEntry.Table.GetDataRow<LF.EffectDataRow>(entityId);
                        if (null != datarow)
                        {
                            if (this.IsCheckEffectLevel && datarow.Level > GameEntry.Adapter.EffectLevel)
                                Log.Info("特效{0}等级大于当前特效所要求的等级{1}, 将不予加载.", entityId, GameEntry.Adapter.EffectLevel);
                            else
                                assetPath = datarow.AssetName;
                        } 
                        else
                        {
                            Log.Error("特效effect配表中不存在这个ID:{0},请检查!", entityId);
                        }

                        break;
                    }
                case E_AssetType.Entity:
                    {
                        var datarow = GameEntry.Table.GetDataRow<LF.EntityDataRow>(entityId);
                        if (null != datarow)
                            assetPath = datarow.AssetName;
                        else
                            Log.Error("实体entity配表中不存在这个ID:{0},请检查!", entityId);

                        break;
                    }
                default:
                    break;
            }
            */

            return assetPath;
        }

        /// <summary>
        /// 是否为UI层
        /// </summary>
        /// <returns></returns>
        private bool IsInUI()
        {
            int layer = LayerMask.NameToLayer("UI");

            if (this.gameObject.layer == layer)
                return true;

            int count = 0;
            var parentTransform = this.transform.parent;
            while (null != parentTransform && count < 100)
            {
                var tmpForm = parentTransform.GetComponent<UIForm>();
                if (null != tmpForm)
                    return true;
                else
                    parentTransform = parentTransform.parent;
            }

            return false;
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
            if(string.IsNullOrEmpty(this.AssetId))
            {
                EditorUtility.DisplayDialog("提示", "没有配置资源ID,无法加载!", "确定");
                return;
            }

            string assetFile = this.GetAssetFile(this.AssetId, this.AssetType);
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

            var entity = ObjectExtension.CreateInstClone(target, this.transform);
            if(null != entity)
            {
                RectTransform rectTransform = entity.GetComponent<RectTransform>();
                if (null != rectTransform)
                    rectTransform.anchoredPosition = Vector2.zero;
                else
                    entity.transform.localPosition = Vector3.zero;

                if (this.AssetType == E_AssetType.Effect)
                {
                    var canvas = entity.GetComponent<Canvas>();
                    if (null == canvas)
                        entity.AddComponent<Canvas>();
                }
            }
        }

        #endregion

#endif

    }
}
