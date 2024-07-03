using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Framework;
using XLua;
using Framework.UI;

/// <summary>
/// Unity 扩展。
/// </summary>
public static class UnityExtension
{

    public static void TryAddElement<T>(this Dictionary<long, List<T>> dic, long key, T t)
    {
        if (dic.ContainsKey(key))
        {
            dic[key].Add(t);
        }
        else
        {
            dic[key] = new List<T>();
            dic[key].Add(t);
        }
    }

    public static List<string> ToStrList(this string str, char splitChar)
    {
        List<string> strList = new List<string>();
        if (!string.IsNullOrEmpty(str))
        {
            string[] strs = str.Split(new char[] { splitChar }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < strs.Length; i++)
            {
                strList.Add(strs[i]);
            }
        }
        return strList;
    }
    public static List<int> ToIntList(this string str, char splitChar)
    {
        List<int> iList = new List<int>();
        if (!string.IsNullOrEmpty(str))
        {
            string[] strs = str.Split(new char[] { splitChar }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < strs.Length; i++)
            {
                iList.Add(strs[i].ToInt());
            }
        }
        return iList;
    }

    public static int ToInt(this string str)
    {
        int i = 0;
        int.TryParse(str, out i);
        return i;
    }
    public static int ToInt(this object str)
    {
        int i = 0;
        int.TryParse(str.ToString(), out i);
        return i;
    }
    public static long ToLong(this object str)
    {
        long i = 0;
        long.TryParse(str.ToString(), out i);
        return i;
    }
    public static float ToFloat(this string str)
    {
        float i = 0f;
        float.TryParse(str, out i);
        return i;
    }

    public static float ToFloat(this object obj)
    {
        float i = 0.0f;
        float.TryParse(obj.ToString(), out i);
        return i;
    }
    
    public static long ToLong(this string str)
    {
        long i = 0;
        if (str.Contains("."))
        {
            List <string> strVec = new List<string>();
            StringExtension.SplitString(str, '.', ref strVec);
            long.TryParse(strVec[0], out i);
        }
        else
        {
            long.TryParse(str, out i);
        }

        return i;
    }

    public static GameObject Instantiate(this GameObject go)
    {
        if (go != null)
        {
            return GameObject.Instantiate(go);
        }
        return go;
    }

    public static void Destroy(this GameObject go)
    {
        if (go != null)
        {
            GameObject.Destroy(go);
        }
    }
    /// <summary>
    /// 获取或增加组件。
    /// </summary>
    /// <typeparam name="T">要获取或增加的组件。</typeparam>
    /// <param name="gameObject">目标对象。</param>
    /// <returns>获取或增加的组件。</returns>
    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        T component = gameObject.GetComponent<T>();
        if (component == null)
        {
            component = gameObject.AddComponent<T>();
        }

        return component;
    }

    /// <summary>
    /// 获取或增加组件。
    /// </summary>
    /// <param name="gameObject">目标对象。</param>
    /// <param name="type">要获取或增加的组件类型。</param>
    /// <returns>获取或增加的组件。</returns>
    public static Component GetOrAddComponent(this GameObject gameObject, Type type)
    {
        Component component = gameObject.GetComponent(type);
        if (component == null)
        {
            component = gameObject.AddComponent(type);
        }

        return component;
    }

    /// <summary>
    /// 获取 GameObject 是否在场景中。
    /// </summary>
    /// <param name="gameObject">目标对象。</param>
    /// <returns>GameObject 是否在场景中。</returns>
    /// <remarks>若返回 true，表明此 GameObject 是一个场景中的实例对象；若返回 false，表明此 GameObject 是一个 Prefab。</remarks>
    public static bool InScene(this GameObject gameObject)
    {
        return gameObject.scene.name != null;
    }

    /// <summary>
    /// 递归设置游戏对象的层次。
    /// </summary>
    /// <param name="gameObject"><see cref="UnityEngine.GameObject" /> 对象。</param>
    /// <param name="layer">目标层次的编号。</param>
    public static void SetLayerRecursively(this GameObject gameObject, int layer)
    {
        Transform[] transforms = gameObject.GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < transforms.Length; i++)
        {
            transforms[i].gameObject.layer = layer;
        }
    }

    /// <summary>
    /// 取 <see cref="UnityEngine.Vector3" /> 的 (x, y, z) 转换为 <see cref="UnityEngine.Vector2" /> 的 (x, z)。
    /// </summary>
    /// <param name="vector3">要转换的 Vector3。</param>
    /// <returns>转换后的 Vector2。</returns>
    public static Vector2 ToVector2(this Vector3 vector3)
    {
        return new Vector2(vector3.x, vector3.z);
    }

    /// <summary>
    /// 取 <see cref="UnityEngine.Vector2" /> 的 (x, y) 转换为 <see cref="UnityEngine.Vector3" /> 的 (x, 0, y)。
    /// </summary>
    /// <param name="vector2">要转换的 Vector2。</param>
    /// <returns>转换后的 Vector3。</returns>
    public static Vector3 ToVector3(this Vector2 vector2)
    {
        return new Vector3(vector2.x, 0f, vector2.y);
    }

    /// <summary>
    /// 取 <see cref="UnityEngine.Vector2" /> 的 (x, y) 和给定参数 y 转换为 <see cref="UnityEngine.Vector3" /> 的 (x, 参数 y, y)。
    /// </summary>
    /// <param name="vector2">要转换的 Vector2。</param>
    /// <param name="y">Vector3 的 y 值。</param>
    /// <returns>转换后的 Vector3。</returns>
    public static Vector3 ToVector3(this Vector2 vector2, float y)
    {
        return new Vector3(vector2.x, y, vector2.y);
    }

    public static Vector2Int Clone(this Vector2Int vector2Int)
    {
        return new Vector2Int(vector2Int.x, vector2Int.y);
    }
    
    public static Vector2 ToVector2(this Vector2Int vector2Int)
    {
        return new Vector2(vector2Int.x, vector2Int.y);
    }

    #region Transform

    /// <summary>
    /// 设置绝对位置的 x 坐标。
    /// </summary>
    /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
    /// <param name="newValue">x 坐标值。</param>
    public static void SetPositionX(this Transform transform, float newValue)
    {
        Vector3 v = transform.position;
        v.x = newValue;
        transform.position = v;
    }

    /// <summary>
    /// 设置绝对位置的 y 坐标。
    /// </summary>
    /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
    /// <param name="newValue">y 坐标值。</param>
    public static void SetPositionY(this Transform transform, float newValue)
    {
        Vector3 v = transform.position;
        v.y = newValue;
        transform.position = v;
    }

    /// <summary>
    /// 设置绝对位置的 z 坐标。
    /// </summary>
    /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
    /// <param name="newValue">z 坐标值。</param>
    public static void SetPositionZ(this Transform transform, float newValue)
    {
        Vector3 v = transform.position;
        v.z = newValue;
        transform.position = v;
    }

    /// <summary>
    /// 增加绝对位置的 x 坐标。
    /// </summary>
    /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
    /// <param name="deltaValue">x 坐标值增量。</param>
    public static void AddPositionX(this Transform transform, float deltaValue)
    {
        Vector3 v = transform.position;
        v.x += deltaValue;
        transform.position = v;
    }

    /// <summary>
    /// 增加绝对位置的 y 坐标。
    /// </summary>
    /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
    /// <param name="deltaValue">y 坐标值增量。</param>
    public static void AddPositionY(this Transform transform, float deltaValue)
    {
        Vector3 v = transform.position;
        v.y += deltaValue;
        transform.position = v;
    }

    /// <summary>
    /// 增加绝对位置的 z 坐标。
    /// </summary>
    /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
    /// <param name="deltaValue">z 坐标值增量。</param>
    public static void AddPositionZ(this Transform transform, float deltaValue)
    {
        Vector3 v = transform.position;
        v.z += deltaValue;
        transform.position = v;
    }

    /// <summary>
    /// 设置相对位置的 x 坐标。
    /// </summary>
    /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
    /// <param name="newValue">x 坐标值。</param>
    public static void SetLocalPositionX(this Transform transform, float newValue)
    {
        Vector3 v = transform.localPosition;
        v.x = newValue;
        transform.localPosition = v;
    }

    /// <summary>
    /// 设置相对位置的 y 坐标。
    /// </summary>
    /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
    /// <param name="newValue">y 坐标值。</param>
    public static void SetLocalPositionY(this Transform transform, float newValue)
    {
        Vector3 v = transform.localPosition;
        v.y = newValue;
        transform.localPosition = v;
    }

    /// <summary>
    /// 设置相对位置的 z 坐标。
    /// </summary>
    /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
    /// <param name="newValue">z 坐标值。</param>
    public static void SetLocalPositionZ(this Transform transform, float newValue)
    {
        Vector3 v = transform.localPosition;
        v.z = newValue;
        transform.localPosition = v;
    }

    /// <summary>
    /// 增加相对位置的 x 坐标。
    /// </summary>
    /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
    /// <param name="deltaValue">x 坐标值。</param>
    public static void AddLocalPositionX(this Transform transform, float deltaValue)
    {
        Vector3 v = transform.localPosition;
        v.x += deltaValue;
        transform.localPosition = v;
    }

    /// <summary>
    /// 增加相对位置的 y 坐标。
    /// </summary>
    /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
    /// <param name="deltaValue">y 坐标值。</param>
    public static void AddLocalPositionY(this Transform transform, float deltaValue)
    {
        Vector3 v = transform.localPosition;
        v.y += deltaValue;
        transform.localPosition = v;
    }

    /// <summary>
    /// 增加相对位置的 z 坐标。
    /// </summary>
    /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
    /// <param name="deltaValue">z 坐标值。</param>
    public static void AddLocalPositionZ(this Transform transform, float deltaValue)
    {
        Vector3 v = transform.localPosition;
        v.z += deltaValue;
        transform.localPosition = v;
    }

    /// <summary>
    /// 设置相对尺寸。
    /// </summary>
    /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
    public static void SetLocalScale(this Transform transform, float newValueX, float newValueY, float newValueZ)
    {
        Vector3 v = new Vector3(newValueX,newValueY,newValueZ);
        transform.localScale = v;
    }

    /// <summary>
    /// 设置相对尺寸的 x 分量。
    /// </summary>
    /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
    /// <param name="newValue">x 分量值。</param>
    public static void SetLocalScaleX(this Transform transform, float newValue)
    {
        Vector3 v = transform.localScale;
        v.x = newValue;
        transform.localScale = v;
    }

    /// <summary>
    /// 设置相对尺寸的 y 分量。
    /// </summary>
    /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
    /// <param name="newValue">y 分量值。</param>
    public static void SetLocalScaleY(this Transform transform, float newValue)
    {
        Vector3 v = transform.localScale;
        v.y = newValue;
        transform.localScale = v;
    }

    /// <summary>
    /// 设置相对尺寸的 z 分量。
    /// </summary>
    /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
    /// <param name="newValue">z 分量值。</param>
    public static void SetLocalScaleZ(this Transform transform, float newValue)
    {
        Vector3 v = transform.localScale;
        v.z = newValue;
        transform.localScale = v;
    }

    /// <summary>
    /// 增加相对尺寸的 x 分量。
    /// </summary>
    /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
    /// <param name="deltaValue">x 分量增量。</param>
    public static void AddLocalScaleX(this Transform transform, float deltaValue)
    {
        Vector3 v = transform.localScale;
        v.x += deltaValue;
        transform.localScale = v;
    }

    /// <summary>
    /// 增加相对尺寸的 y 分量。
    /// </summary>
    /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
    /// <param name="deltaValue">y 分量增量。</param>
    public static void AddLocalScaleY(this Transform transform, float deltaValue)
    {
        Vector3 v = transform.localScale;
        v.y += deltaValue;
        transform.localScale = v;
    }

    /// <summary>
    /// 增加相对尺寸的 z 分量。
    /// </summary>
    /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
    /// <param name="deltaValue">z 分量增量。</param>
    public static void AddLocalScaleZ(this Transform transform, float deltaValue)
    {
        Vector3 v = transform.localScale;
        v.z += deltaValue;
        transform.localScale = v;
    }

    /// <summary>
    /// 二维空间下使 <see cref="UnityEngine.Transform" /> 指向指向目标点的算法，使用世界坐标。
    /// </summary>
    /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
    /// <param name="lookAtPoint2D">要朝向的二维坐标点。</param>
    /// <remarks>假定其 forward 向量为 <see cref="UnityEngine.Vector3.up" />。</remarks>
    public static void LookAt2D(this Transform transform, Vector2 lookAtPoint2D)
    {
        Vector3 vector = lookAtPoint2D.ToVector3() - transform.position;
        vector.y = 0f;

        if (vector.magnitude > 0f)
        {
            transform.rotation = Quaternion.LookRotation(vector.normalized, Vector3.up);
        }
    }

    /// <summary>
    /// 移除所有子节点
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="immediate"></param>
    public static void RemoveAllChildren(this Transform transform, bool immediate = false)
    {
        if (null == transform || transform.childCount <= 0)
            return;

        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            var child = transform.GetChild(i);

            if (!immediate)
                GameObject.Destroy(child.gameObject);
            else
                GameObject.DestroyImmediate(child.gameObject);
        }
    }

    #endregion Transform

    /// <summary>
    /// 查找该物体在层级面板中的路径
    /// </summary>
    private static StringBuilder s_PathStringBuilder;
    public static string GetPath(this Transform transform)
    {
        string resultPath = null;
        if (transform == null) return resultPath;
        if (s_PathStringBuilder == null)
        {
            s_PathStringBuilder = new StringBuilder(transform.name);
        }
        else
        {
            s_PathStringBuilder.Clear();
            s_PathStringBuilder.Append(transform.name);
        }
        Transform parent = transform.parent;
        string conStr = "/";
        while (parent != null)
        {
            s_PathStringBuilder.Insert(0, parent.name + conStr);
            parent = parent.parent;
        }
        resultPath = s_PathStringBuilder.ToString();
        return resultPath;
    }

    public static void SetActiveEx(this GameObject gameObject, bool value)
    {
        if (gameObject.activeSelf == value)
        {
            return;
        }
        
        gameObject.SetActive(value);
    }


    public static (int, string) GetBaseSortingOrderAndLayer(this Component ui)
    {
        var (order, layer) = (0, string.Empty);
        Canvas uiLayerRoot = null;
        var canvasList = ui.GetComponentsInParent<Canvas>();
        for (int i = 0; i < canvasList.Length; i++)
        {
            var parent = canvasList[i].transform.parent;
            if (null != parent && null != parent.GetComponent<UIGroupMono>())
            {
                uiLayerRoot = canvasList[i];

                break;
            }
        }

        if (null == uiLayerRoot)
        {
            Log.Error($"UI没有挂载到Layer层下,请检查! Name:{ui.name}");
            return (0, "Default");
        }

        order = uiLayerRoot.sortingOrder;
        layer = uiLayerRoot.sortingLayerName;

        return (order, layer);
    }

    #region ParseLuaTable

    public static void AddDialogId(ref string dialogId, string dialog)
    {
        if (dialog.IsNullOrEmpty())
            return;
        if (dialogId.IsNullOrEmpty())
            dialogId = dialog;
        else
        {
            dialogId = dialogId + "|" + dialog;
        }
    }

    public static string ParseTuple(object args, ref string dialogId)
    {
        var strResult = ParseTupleStr(args, ref dialogId);
        if (strResult.IsNullOrEmpty())
            strResult = ParseTupleInt(args, ref dialogId);
        return strResult;
    }

    public static string ParseTupleStr(object args, ref string dialogId)
    {
        if (args is Tuple<string>)
        {
            var tmp = args as Tuple<string>;
            AddDialogId(ref dialogId, tmp.Item1);
            return GameEntry.Localization.GetString(tmp.Item1);
        }
        else if (args is ValueTuple<string, string>)
        {
            var tmp = (ValueTuple<string, string>)args;
            AddDialogId(ref dialogId, tmp.Item1);
            return GameEntry.Localization.GetString(tmp.Item1, tmp.Item2);
        }
        else if (args is ValueTuple<string, string, string>)
        {
            var tmp = (ValueTuple<string, string, string>)args;
            AddDialogId(ref dialogId, tmp.Item1);
            return GameEntry.Localization.GetString(tmp.Item1, tmp.Item2, tmp.Item3);
        }
        else if (args is ValueTuple<string, string, string, string>)
        {
            var tmp = (ValueTuple<string, string, string, string>)args;
            AddDialogId(ref dialogId, tmp.Item1);
            return GameEntry.Localization.GetString(tmp.Item1, tmp.Item2, tmp.Item3, tmp.Item4);
        }
        else if (args is ValueTuple<string, string, string, string, string>)
        {
            var tmp = (ValueTuple<string, string, string, string, string>)args;
            AddDialogId(ref dialogId, tmp.Item1);
            return GameEntry.Localization.GetString(tmp.Item1, tmp.Item2, tmp.Item3, tmp.Item4, tmp.Item5);
        }
        return "";
    }

    public static string ParseTupleInt(object args, ref string dialogId)
    {
        if (args is Tuple<int>)
        {
            var tmp = args as Tuple<int>;
            AddDialogId(ref dialogId, tmp.Item1.ToString());
            return GameEntry.Localization.GetString(tmp.Item1.ToString());
        }
        else if (args is ValueTuple<string, int>)
        {
            var tmp = (ValueTuple<string, int>)args;
            AddDialogId(ref dialogId, tmp.Item1);
            return GameEntry.Localization.GetString(tmp.Item1, tmp.Item2);
        }
        else if (args is ValueTuple<string, int, int>)
        {
            var tmp = (ValueTuple<string, int, int>)args;
            AddDialogId(ref dialogId, tmp.Item1);
            return GameEntry.Localization.GetString(tmp.Item1, tmp.Item2, tmp.Item3);
        }
        else if (args is ValueTuple<string, int, int, int>)
        {
            var tmp = (ValueTuple<string, int, int, int>)args;
            AddDialogId(ref dialogId, tmp.Item1);
            return GameEntry.Localization.GetString(tmp.Item1, tmp.Item2, tmp.Item3, tmp.Item4);
        }
        else if (args is ValueTuple<string, int, int, int, int>)
        {
            var tmp = (ValueTuple<string, int, int, int, int>)args;
            AddDialogId(ref dialogId, tmp.Item1);
            return GameEntry.Localization.GetString(tmp.Item1, tmp.Item2, tmp.Item3, tmp.Item4, tmp.Item5);
        }
        return "";
    }

    public static string ParseLuaTable(object args, ref string dialogId)
    {
        if (args is LuaTable)
        {
            var tmp = args as LuaTable;
            return ParseLuaTableStr(tmp, ref dialogId);
        }
        return "";
    }

    public static string ParseLuaTableStr(LuaTable tmp, ref string dialogId)
    {
        //设置dialogId
        if (tmp.Length > 0)
        {
            AddDialogId(ref dialogId, tmp.Get<int, string>(1));
        }
        switch (tmp.Length)
        {
            case 1:
                return GameEntry.Localization.GetString(tmp.Get<int, string>(1));
            case 2:
                return GameEntry.Localization.GetString(tmp.Get<int, string>(1), tmp.Get<int, string>(2));
            case 3:
                return GameEntry.Localization.GetString(tmp.Get<int, string>(1), tmp.Get<int, string>(2), tmp.Get<int, string>(3));
            case 4:
                return GameEntry.Localization.GetString(tmp.Get<int, string>(1), tmp.Get<int, string>(2), tmp.Get<int, string>(3), tmp.Get<int, string>(4));
            case 5:
                return GameEntry.Localization.GetString(tmp.Get<int, string>(1), tmp.Get<int, string>(2), tmp.Get<int, string>(3), tmp.Get<int, string>(4), tmp.Get<int, string>(5));
        }

        return "";
    }

    #endregion

    #region Text Extend

    public static void SetDialogId(this Text text, string dialogId)
    {
        text.SetText((dialogId, ""));
    }

    public static void SetText(this Text text, params object[] args)
    {
        string result = "";
        string refDialogId = "";

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] is string)
            {
                result += args[i];
            }
            else if (args[i] is int || args[i] is long || args[i] is double || args[i] is float)
            {
                result = args[i].ToString();
            }
            else if (args[i] is LuaTable)
            {
                result += ParseLuaTable(args[i], ref refDialogId);
            }
            else
            {
                result += ParseTuple(args[i], ref refDialogId);
            }
        }

        //if (GameEntry.Localization.Language == GameFramework.Localization.Language.Arabic)
        //{// 阿拉伯语从右往左读, 需要将文本反转一下
        //    // 查找是否存在自适应文本宽度的组件,否则需要自行控制换行
        //    var fitter = text.GetComponent<ContentSizeFitter>();
        //    if (null == fitter || !fitter.enabled || fitter.horizontalFit != ContentSizeFitter.FitMode.PreferredSize)
        //    {
        //        var arabic = ArabicSupport.ArabicFixer.FixEx(result, true, false);
        //        text.SetArabicText(arabic);
        //    }
        //    else
        //    {
        //        text.text = ArabicSupport.ArabicFixer.FixEx(result, true, false);
        //    }

        //    // 设置文本右对齐
        //    //text.SetAlignmentForArabic();
        //}
        //else
        {
            text.text = result;
        }

        // 字符串比较短的就不去送检了
        if (result.Length <= 4 || refDialogId.IsNullOrEmpty())
        {
            return;
        }

        return;
    }

    #endregion
}
