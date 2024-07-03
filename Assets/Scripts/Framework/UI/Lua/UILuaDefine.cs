using System;
using UnityEngine;
using UnityEngine.UI;
using EnhancedUI.EnhancedScroller;
using Chanto;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

public enum LuaComType
{
    // 基础组件
    Unknown = 0,
    GameObject = 1,
    Transform = 2,
    RectTransform = 3,
    Toggle = 4,
    ToggleGroup = 5,
    Slider = 6,
    ParticleSystem = 7,
    RawImage = 8,
    ScrollRect = 9,
    InputField = 10,
    Material = 11,
    Animator = 12,
    Animation = 13,
    CanvasGroup = 14,
    TextMeshProUGUI = 15,
    TMP_InputField = 16,

    // 扩展的基础通用组件从100开始
    IMImage = 100,
    IMTextMeshProUGUI = 101,
    UIExpireTimer = 102,
    UIMultiScroller = 103,
    UIExtend = 104,
    UIRedDot = 105,
    CDButton = 106,
    CheckBoxButton = 107,
    ToggleButton = 108,
    GroupButton = 109,
    SwitchButton = 110,
    EnhancedScroller = 111,
    EnhancedScrollerCellView = 112,
    ToggleButtonGroup = 113,
    
    // 自定义通用业务组件从200开始
    UIItemIcon = 200,
}

[Serializable]
/// <summary>
/// Lua组件分组
/// </summary>
public class LuaComGroup
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name;

    /// <summary>
    /// Lua组件数组
    /// </summary>
    public LuaCom[] LuaComs;
}

[Serializable]
/// <summary>
/// Lua组件
/// </summary>
public class LuaCom
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name;

#if ODIN_INSPECTOR
    [OnValueChanged("OnTypeChange")]
#endif
    /// <summary>
    /// 类型
    /// </summary>
    public LuaComType Type;

#if ODIN_INSPECTOR
    [OnValueChanged("OnComObjChange")]
#endif
    public UnityEngine.Object ComObj;

    /// <summary>
    /// 原始物体的实例编号
    /// </summary>
    private int m_InstanceId;

    #region OnComObjChange 设置组件

    /// <summary>
    /// 设置组件
    /// </summary>
    private void OnComObjChange()
    {
#if UNITY_EDITOR
        if (ComObj == null)
        {
            Name = "";
            Type = LuaComType.Unknown;
            m_InstanceId = 0;
            return;
        }

        Transform Trans = null;
        Transform[] arr = UnityEditor.Selection.transforms[0].GetComponentsInChildren<Transform>(true);
        if (null != arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i].gameObject.GetInstanceID() == ComObj.GetInstanceID())
                {
                    Trans = arr[i];
                    m_InstanceId = ComObj.GetInstanceID();
                    break;
                }
            }
        }

        if (Trans != null)
        {
            Name = this.GetObjectName(Trans);

#region 设置类型

            if (Trans.GetComponent<CDButton>() != null)
            {
                Type = LuaComType.CDButton;
                ComObj = Trans.GetComponent<CDButton>();
            }
            else if (Trans.GetComponent<InputField>() != null)
            {
                Type = LuaComType.InputField;
                ComObj = Trans.GetComponent<InputField>();
            }
            else if (null != Trans.GetComponent<EnhancedScroller>())
            {
                Type = LuaComType.EnhancedScroller;
                ComObj = Trans.GetComponent<EnhancedScroller>();
            }
            else if (null != Trans.GetComponent<EnhancedScrollerCellView>())
            {
                Type = LuaComType.EnhancedScrollerCellView;
                ComObj = Trans.GetComponent<EnhancedScrollerCellView>();
            }
            else if (Trans.GetComponent<ScrollRect>() != null)
            {
                Type = LuaComType.ScrollRect;
                ComObj = Trans.GetComponent<ScrollRect>();
            }
            else if (Trans.GetComponent<Slider>() != null)
            {
                Type = LuaComType.Slider;
                ComObj = Trans.GetComponent<Slider>();
            }
            else if (Trans.GetComponent<IMImage>() != null)
            {
                Type = LuaComType.IMImage;
                ComObj = Trans.GetComponent<IMImage>();
            }
            else if (Trans.GetComponent<RawImage>() != null)
            {
                Type = LuaComType.RawImage;
                ComObj = Trans.GetComponent<RawImage>();
            }
            else if (Trans.GetComponent<UIExpireTimer>() != null)
            {
                Type = LuaComType.UIExpireTimer;
                ComObj = Trans.GetComponent<UIExpireTimer>();
            }
            else if (Trans.GetComponent<IMTextMeshProUGUI>() != null)
            {
                Type = LuaComType.IMTextMeshProUGUI;
                ComObj = Trans.GetComponent<IMTextMeshProUGUI>();
            }
            else if (null != Trans.GetComponent<UIMultiScroller>())
            {
                Type = LuaComType.UIMultiScroller;
                ComObj = Trans.GetComponent<UIMultiScroller>();
            }
            else if(null != Trans.GetComponent<TMPro.TextMeshProUGUI>())
            {
                Type = LuaComType.TextMeshProUGUI;
                ComObj = Trans.GetComponent<TMPro.TextMeshProUGUI>();
            }
            else if (null != Trans.GetComponent<TMPro.TMP_InputField>())
            {
                Type = LuaComType.TMP_InputField;
                ComObj = Trans.GetComponent<TMPro.TMP_InputField>();
            }
            else if (null != Trans.GetComponent<UIExtend>())
            {
                Type = LuaComType.UIExtend;
                ComObj = Trans.GetComponent<UIExtend>();
            }
            else if (null != Trans.GetComponent<CheckBoxButton>())
            {
                Type = LuaComType.CheckBoxButton;
                ComObj = Trans.GetComponent<CheckBoxButton>();
            }
            else if (null != Trans.GetComponent<ToggleButton>())
            {
                Type = LuaComType.ToggleButton;
                ComObj = Trans.GetComponent<ToggleButton>();
            }
            else if(null != Trans.GetComponent<GroupButton>())
            {
                Type = LuaComType.GroupButton;
                ComObj = Trans.GetComponent<GroupButton>();
            }
            else if (null != Trans.GetComponent<ToggleButtonGroup>())
            {
                Type = LuaComType.ToggleButtonGroup;
                ComObj = Trans.GetComponent<ToggleButtonGroup>();
            }
            else if (null != Trans.GetComponent<SwitchButton>())
            {
                Type = LuaComType.SwitchButton;
                ComObj = Trans.GetComponent<SwitchButton>();
            }
            else if (Trans.GetComponent<RectTransform>() != null)
            {
                Type = LuaComType.RectTransform;
                ComObj = Trans.GetComponent<RectTransform>();
            }
            else if (Trans.GetComponent<Transform>() != null)
            {
                Type = LuaComType.Transform;
                ComObj = Trans.GetComponent<Transform>();
            }
            else if (Trans.GetComponent<Toggle>() != null)
            {
                Type = LuaComType.Toggle;
                ComObj = Trans.GetComponent<Toggle>();
            }
            else if (Trans.GetComponent<ToggleGroup>() != null)
            {
                Type = LuaComType.ToggleGroup;
                ComObj = Trans.GetComponent<ToggleGroup>();
            }
            else if (Trans.GetComponent<Animator>() != null)
            {
                Type = LuaComType.Animator;
                ComObj = Trans.GetComponent<Animator>();
            }
            else if (Trans.GetComponent<ParticleSystem>() != null)
            {
                Type = LuaComType.ParticleSystem;
                ComObj = Trans.GetComponent<ParticleSystem>();
            }
            else if (Trans.GetComponent<ParticleSystem>() != null)
            {
                Type = LuaComType.ParticleSystem;
                ComObj = Trans.GetComponent<ParticleSystem>();
            }
            else if (Trans.GetComponent<Material>() != null)
            {
                Type = LuaComType.Material;
                ComObj = Trans.GetComponent<Material>();
            }
            else if (Trans.GetComponent<CanvasGroup>() != null)
            {
                Type = LuaComType.CanvasGroup;
                ComObj = Trans.GetComponent<CanvasGroup>();
            }
#endregion
        }
#endif
    }

    private string GetObjectName(Transform Trans)
    {
        string name = Trans.name;
        if (this.IsExistName(name)) 
            name = name + "_" + ComObj.GetInstanceID().ToString();

        return name;
    }

    private bool IsExistName(string name)
    {
#if UNITY_EDITOR
        GameObject root = Selection.activeObject as GameObject;
        if (null == root)
            return false;

        UILuaForm luaForm = root.GetComponent<UILuaForm>();
        if (null == luaForm)
            return false;

        for (int i = 0; i < luaForm.LuaComGroups.Length; i++)
        {
            for(int j = 0; j < luaForm.LuaComGroups[i].LuaComs.Length; j++)
            {
                if (name == luaForm.LuaComGroups[i].LuaComs[j].Name)
                    return true;
            }
        }
#endif

        return false;
    }

#endregion

    #region OnTypeChange 类型修改

    /// <summary>
    /// 类型修改
    /// </summary>
    private void OnTypeChange()
    {
#if UNITY_EDITOR
        if (ComObj == null)
        {
            Name = "";
            Type = LuaComType.Unknown;
            return;
        }

        LuaComType oldType = Type;

        Transform Trans = null;
        Transform[] arr = UnityEditor.Selection.transforms[0].GetComponentsInChildren<Transform>(true);
        if (null != arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i].gameObject.GetInstanceID() == m_InstanceId)
                {
                    Trans = arr[i];
                    break;
                }
            }
        }

        UnityEngine.Object tempObj = null;
#region 设置类型
        switch(Type)
        {
            case LuaComType.CDButton:
                {
                    tempObj = Trans.GetComponent<CDButton>();
                    break;
                }
            case LuaComType.InputField:
                {
                    tempObj = Trans.GetComponent<InputField>();
                    break;
                }
            case LuaComType.ScrollRect:
                {
                    tempObj = Trans.GetComponent<ScrollRect>();
                    break;
                }
            case LuaComType.Slider:
                {
                    tempObj = Trans.GetComponent<Slider>();
                    break;
                }
            case LuaComType.RectTransform:
                {
                    tempObj = Trans.GetComponent<RectTransform>();
                    break;
                }
            case LuaComType.Transform:
                {
                    tempObj = Trans.GetComponent<Transform>();
                    break;
                }
            case LuaComType.Toggle:
                {
                    tempObj = Trans.GetComponent<Toggle>();
                    break;
                }
            case LuaComType.ToggleGroup:
                {
                    tempObj = Trans.GetComponent<ToggleGroup>();
                    break;
                }
            case LuaComType.Animator:
                {
                    tempObj = Trans.GetComponent<Animator>();
                    break;
                }
            case LuaComType.GameObject:
                {
                    tempObj = Trans.gameObject;
                    break;
                }
            case LuaComType.UIMultiScroller:
                {
                    tempObj = Trans.gameObject.GetComponent<UIMultiScroller>();
                    break;
                }
            case LuaComType.TextMeshProUGUI:
                {
                    tempObj = Trans.gameObject.GetComponent<TMPro.TextMeshProUGUI>();
                    break;
                }
            case LuaComType.UIExtend:
                {
                    tempObj = Trans.gameObject.GetComponent<UIExtend>();
                    break;
                }
            case LuaComType.CheckBoxButton:
                {
                    tempObj = Trans.gameObject.GetComponent<CheckBoxButton>();
                    break;
                }
            case LuaComType.ToggleButton:
                {
                    tempObj = Trans.gameObject.GetComponent<ToggleButton>();
                    break;
                }
            case LuaComType.GroupButton:
                {
                    tempObj = Trans.gameObject.GetComponent<GroupButton>();
                    break;
                }
            case LuaComType.EnhancedScroller:
            {
                tempObj = Trans.gameObject.GetComponent<EnhancedScroller>();
                break;
            }
            case LuaComType.EnhancedScrollerCellView:
            {
                tempObj = Trans.gameObject.GetComponent<EnhancedScrollerCellView>();
                break;
            }
            case LuaComType.ToggleButtonGroup:
            {
                tempObj = Trans.gameObject.GetComponent<ToggleButtonGroup>();
                break;
            }
            case LuaComType.TMP_InputField:
                {
                    tempObj = Trans.gameObject.GetComponent<TMPro.TMP_InputField>();
                    break;
                }
            case LuaComType.SwitchButton:
                {
                    tempObj = Trans.gameObject.GetComponent<SwitchButton>();
                    break;
                }
            case LuaComType.IMImage:
                {
                    tempObj = Trans.gameObject.GetComponent<IMImage>();
                    break;
                }
            case LuaComType.IMTextMeshProUGUI:
                {
                    tempObj = Trans.gameObject.GetComponent<IMTextMeshProUGUI>();
                    break;
                }
            case LuaComType.UIExpireTimer:
                {
                    tempObj = Trans.gameObject.GetComponent<UIExpireTimer>();
                    break;
                }
            default:
                break;
        }
 
#endregion

        if (tempObj != null)
        {
            ComObj = tempObj;
        }
        else
        {
            Type = oldType;
            Debug.LogError("您选择类型在当前组件上不存在 ComObj=" + ComObj.name);
        }
#endif
    }

    #endregion

    #region 获得组件的实例信息

    // 获得组件的GameObject
    public GameObject GetGameObject()
    {
        GameObject obj = null;

        switch (this.Type)
        {
            case LuaComType.Unknown:
                {
                    break;
                }
            case LuaComType.GameObject:
                {
                    obj = ComObj as GameObject;
                    break;
                }
            case LuaComType.Transform:
                {
                    Transform temp = ComObj as Transform;
                    if (null != temp)
                        obj = temp.gameObject;

                    break;
                }
            case LuaComType.Toggle:
                {
                    Toggle temp = ComObj as Toggle;
                    obj = temp?.gameObject;

                    break;
                }
            case LuaComType.ToggleGroup:
                {
                    ToggleGroup temp = ComObj as ToggleGroup;
                    obj = temp?.gameObject;

                    break;
                }
            case LuaComType.Slider:
                {
                    Slider temp = ComObj as Slider;
                    obj = temp?.gameObject;

                    break;
                }
            case LuaComType.ParticleSystem:
                {
                    ParticleSystem temp = ComObj as ParticleSystem;
                    obj = temp?.gameObject;

                    break;
                }
            case LuaComType.RawImage:
                {
                    RawImage temp = ComObj as RawImage;
                    obj = temp?.gameObject;

                    break;
                }
            case LuaComType.ScrollRect:
                {
                    ScrollRect temp = ComObj as ScrollRect;
                    obj = temp?.gameObject;

                    break;
                }
            case LuaComType.InputField:
                {
                    InputField temp = ComObj as InputField;
                    obj = temp?.gameObject;

                    break;
                }
            case LuaComType.Material:
                {
                    obj = null;

                    break;
                }
            case LuaComType.Animator:
                {
                    Animator temp = ComObj as Animator;
                    obj = temp?.gameObject;

                    break;
                }
            case LuaComType.Animation:
                {
                    Animation temp = ComObj as Animation;
                    obj = temp?.gameObject;

                    break;
                }
            case LuaComType.CanvasGroup:
                {
                    CanvasGroup temp = ComObj as CanvasGroup;
                    obj = temp?.gameObject;

                    break;
                }
            case LuaComType.UIMultiScroller:
                {
                    UIMultiScroller temp = ComObj as UIMultiScroller;
                    obj = temp?.gameObject;

                    break;
                }
            case LuaComType.TextMeshProUGUI:
                {
                    TMPro.TextMeshProUGUI temp = ComObj as TMPro.TextMeshProUGUI;
                    obj = temp?.gameObject;

                    break;
                }
            case LuaComType.RectTransform:
                {
                    RectTransform temp = ComObj as RectTransform;
                    obj = temp?.gameObject;

                    break;
                }
            case LuaComType.UIExtend:
                {
                    UIExtend temp = ComObj as UIExtend;
                    obj = temp?.gameObject;

                    break;
                }
            case LuaComType.CDButton:
                {
                    CDButton temp = ComObj as CDButton;
                    obj = temp?.gameObject;

                    break;
                }
            case LuaComType.CheckBoxButton:
                {
                    CheckBoxButton temp = ComObj as CheckBoxButton;
                    obj = temp?.gameObject;

                    break;
                }
            case LuaComType.ToggleButton:
                {
                    ToggleButton temp = ComObj as ToggleButton;
                    obj = temp?.gameObject;

                    break;
                }
            case LuaComType.GroupButton:
                {
                    GroupButton temp = ComObj as GroupButton;
                    obj = temp?.gameObject;

                    break;
                }
            case LuaComType.TMP_InputField:
                {
                    TMPro.TMP_InputField temp = ComObj as TMPro.TMP_InputField;
                    obj = temp?.gameObject;

                    break;
                }
            case LuaComType.SwitchButton:
                {
                    SwitchButton temp = ComObj as SwitchButton;
                    obj = temp?.gameObject;

                    break;
                }
            case LuaComType.IMImage:
                {
                    IMImage temp = ComObj as IMImage;
                    obj = temp?.gameObject;

                    break;
                }
            case LuaComType.IMTextMeshProUGUI:
                {
                    var temp = ComObj as IMTextMeshProUGUI;
                    obj = temp?.gameObject;

                    break;
                }
            case LuaComType.UIExpireTimer:
                {
                    UIExpireTimer temp = ComObj as UIExpireTimer;
                    obj = temp?.gameObject;

                    break;
                }
            case LuaComType.EnhancedScroller:
            {
                var temp = ComObj as EnhancedScroller;
                obj = temp?.gameObject;

                break;
            }
            case LuaComType.EnhancedScrollerCellView:
            {
                var temp = ComObj as EnhancedScrollerCellView;
                obj = temp?.gameObject;

                break;
            }
            case LuaComType.ToggleButtonGroup:
            {
                var temp = ComObj as ToggleButtonGroup;
                obj = temp?.gameObject;

                break;
            }
            default:
                break;
        }

        return obj;
    }

    // 获得绑定组件的实例ID;
    public int GetGameObjectInstanceId()
    {
        int instanceId = -1;

        GameObject temp = this.GetGameObject();
        if (null != temp)
            instanceId = temp.GetInstanceID();

        return instanceId;
    }

    #endregion
}