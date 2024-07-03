
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

/// <summary>
/// 集合类扩展
/// </summary>
public static class CollectionsExtension
{
    #region 类型转换

    public static int asInt(this string text)
    {
        return text.ToInt();
    }

    public static int asInt(this object value)
    {
        return (int)value;
    }

    public static long asLong(this string text)
    {
        return text.ToLong();
    }

    public static float asFloat(this string text)
    {
        return text.ToFloat();
    }

    public static float asDouble(this string text)
    {
        return text.ToFloat();
    }

    public static uint toUInt(this int value)
    {
        return unchecked((uint)value);
    }

    public static int length(this string text)
    {
        return text.IsNotNullAndEmpty() ? text.Length : 0;
    }

    public static int size(this string text)
    {
        return text.IsNotNullAndEmpty() ? text.Length : 0;
    }

    public static string append(this string text, string add)
    {
        text += add;
        return text;
    }

    #endregion

    #region 集合类扩展

    public static int size(this string[] arr)
    {
        if (null == arr)
            return 0;

        return arr.Length;
    }

    public static int count<T>(this T[] arr)
    {
        if (null == arr)
            return 0;

        return arr.Length;
    }

    public static int _countof<T>(T[] arr)
    {
        if (null == arr)
            return 0;

        return arr.Length;
    }

    public static void reset<T>(this T[] array, T value = default(T))
    {
        if (null == array || array.Length <= 0)
            return;

        for (int i = 0; i < array.Length; i++)
            array[i] = value;
    }

    public static int size<T>(this List<T> list)
    {
        if (null == list)
            return 0;

        return list.Count;
    }

    public static int count<T>(this List<T> list)
    {
        if (null == list)
            return 0;

        return list.Count;
    }
    public delegate bool ConditionV<TV>(TV v);
    public delegate bool ConditionKV<T, TV>(T k, TV v);
    public static bool erase<T, TV>(this Dictionary<T, TV> dict, ConditionKV<T, TV> condition)
    {
        if (null == dict)
            return false;
        var tmpList = new List<T>();
        foreach (var it in dict)
            if (condition(it.Key, it.Value))
                tmpList.Add(it.Key);
        if (tmpList.Count == 0)
            return false;
        foreach (var k in tmpList)
            dict.Remove(k);
        return true;
    }

    public static bool erase<T, TV>(this Dictionary<T, TV> dict, ConditionV<TV> condition)
    {
        if (null == dict)
            return false;
        var tmpList = new List<T>();
        foreach (var it in dict)
            if (condition(it.Value))
                tmpList.Add(it.Key);
        if (tmpList.Count == 0)
            return false;
        foreach (var k in tmpList)
            dict.Remove(k);
        return true;
    }

    public static int size<T, TV>(this Dictionary<T, TV> dict)
    {
        if (null == dict)
            return 0;

        return dict.Count;
    }

    public static bool empty(this string[] arr)
    {
        if (null == arr)
            return true;

        return arr.Length <= 0;
    }

    public static bool empty<T>(this List<T> list)
    {
        if (null == list)
            return true;

        return list.Count <= 0;
    }

    public static bool empty<T, TV>(this Dictionary<T, TV> dict)
    {
        if (null == dict)
            return true;

        return dict.Count <= 0;
    }

    public static void push_back<T>(this List<T> list, T value)
    {
        if (null == list)
            return;

        list.Add(value);
    }

    public static T at<T>(this T[] array, int index, T defaultValue = default(T))
    {
        if (null == array || index >= array.Length)
            return defaultValue;

        return array[index];
    }

    public static T at<T>(this List<T> list, int index, T defaultValue = default(T))
    {
        if (null == list || index >= list.Count)
            return defaultValue;

        return list[index];
    }

    public static T objectAtIndex<T>(this List<T> list, int index, T defaultValue = default(T))
    {
        if (null == list || index >= list.Count)
            return defaultValue;

        return list[index];
    }

    public static T back<T>(this List<T> list, T defaultValue = default(T))
    {
        if (null == list)
            return defaultValue;

        return list.Last();
    }

    public static void erase<T>(this List<T> list, T element, bool removeAll = true)
    {
        if (null != list && list.Contains(element))
        {
            if (removeAll)
                list.RemoveAll((o) => o.Equals(element));
            else
                list.Remove(element);
        }
    }

    public static void erase<T, TV>(this Dictionary<T, TV> dict, T key)
    {
        if (null != dict && dict.ContainsKey(key))
            dict.Remove(key);
    }

    public static void clear<T>(this List<T> list)
    {
        if (null == list)
            return;

        list.Clear();
    }

    public static void removeAllObjects<T>(this List<T> list)
    {
        if (null == list)
            return;

        list.Clear();
    }

    public static void addObject<T>(this List<T> list, T value)
    {
        if (null == list)
            return;

        list.Add(value);
    }

    public static void pop_back<T>(this List<T> list)
    {
        if (null == list)
            return;

        int count = list.Count;
        list.RemoveAt(count - 1);
    }

    public static void exchangeObjectAtIndex<T>(this List<T> list, int index1, int index2)
    {
        if (null == list)
            return;

        if (index1 < 0 || index1 >= list.Count)
            return;

        if (index2 < 0 || index2 >= list.Count)
            return;

        var temp = list[index1];
        list[index1] = list[index2];
        list[index2] = temp;
    }

    public static void insert<T>(this List<T> list, T[] values)
    {
        if (null == values || values.Length <= 0)
            return;

        for (int i = 0; i < values.Length; i++)
            list.Add(values[i]);
    }

    public static void clear<T, TV>(this Dictionary<T, TV> dict)
    {
        if (null == dict)
            return;

        dict.Clear();
    }

    public static TV SetOrAdd<T, TV>(this Dictionary<T, TV> dict, T key, TV value)
    {
        if (dict.ContainsKey(key))
        {
            dict[key] = value;
        }
        else
        {
            dict.Add(key, value);
        }

        return value;
    }

    public static TV SetOrAdd<T, TV>(this SortedDictionary<T, TV> dict, T key, TV value)
    {
        if (dict.ContainsKey(key))
        {
            dict[key] = value;
        }
        else
        {
            dict.Add(key, value);
        }

        return value;
    }

    public static bool insert<T, TV>(this Dictionary<T, TV> dict, T key, TV value)
    {
        if (dict.ContainsKey(key))
            return false;

        dict.Add(key, value);
        return true;
    }

    public static bool insertList<T, TV>(this Dictionary<T, List<TV>> dict, T key, TV value)
    {
        if (!dict.TryGetValue(key, out var v))
        {
            v = new List<TV>();
            v.Add(value);
            dict.Add(key, v);
        }
        else
        {
            v.Add(value);
        }

        return true;
    }

    public static TV Get<T, TV>(this Dictionary<T, TV> dict, T key)
    {
        if (null != dict && dict.ContainsKey(key))
            return dict[key];

        return default(TV);
    }

    public static bool SafeRemove<T, TV>(this Dictionary<T, TV> dict, T key)
    {
        if (null != dict && dict.ContainsKey(key))
            return dict.Remove(key);

        return false;
    }

    public static TV GetOrCreate<T, TV>(this Dictionary<T, TV> dict, T key) where TV : new()
    {
        if (!dict.TryGetValue(key, out var v))
        {
            v = new TV();
            dict.Add(key, v);
        }

        return v;
    }

    public static int compare(this string text, string other)
    {
        return string.Compare(text, other, StringComparison.Ordinal);
    }

    public static int find(this string text, string other)
    {
        return text.IndexOf(other, StringComparison.Ordinal);
    }

    #endregion

    #region 设置坐标

    public static void setPoint(this Vector2Int point, int x, int y)
    {
        point.x = x;
        point.y = y;
    }

    /// <summary>
    /// 设置坐标(优先设置锚点坐标)
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public static void setPosition(this Transform transform, float x, float y, float z = 0)
    {
        var rectTransform = transform.GetComponent<RectTransform>();
        if (null != rectTransform)
        {
            rectTransform.setAnchorPosition(x, y, z);
        }
        else
        {
            Vector3 v = transform.localPosition;
            v.x = x;
            v.y = y;
            v.z = z;
            transform.localPosition = v;
        }
    }

    public static void setPosition(this Transform transform, Vector2 point)
    {
        transform.setPosition(point.x, point.y, 0);
    }

    public static void setPosition(this Transform transform, Vector3 point)
    {
        transform.setPosition(point.x, point.y, point.z);
    }

    public static void setPositionX(this Transform transform, float newValue)
    {
        var rectTransform = transform.GetComponent<RectTransform>();
        if (null != rectTransform)
        {
            rectTransform.setAnchorPositionX(newValue);
        }
        else
        {
            Vector3 v = transform.localPosition;
            v.x = newValue;
            transform.localPosition = v;
        }
    }

    public static void setPositionY(this Transform transform, float newValue)
    {
        var rectTransform = transform.GetComponent<RectTransform>();
        if (null != rectTransform)
        {
            rectTransform.setAnchorPositionY(newValue);
        }
        else
        {
            Vector3 v = transform.localPosition;
            v.y = newValue;
            transform.localPosition = v;
        }
    }

    public static void setPositionZ(this Transform transform, float newValue)
    {
        var rectTransform = transform.GetComponent<RectTransform>();
        if (null != rectTransform)
        {
            rectTransform.setAnchorPositionZ(newValue);
        }
        else
        {
            Vector3 v = transform.localPosition;
            v.z = newValue;
            transform.localPosition = v;
        }
    }

    public static void setPosition(this Component comp, Vector2 point)
    {
        if (comp != null)
        {
            setPosition(comp.transform, point);
        }
    }


    public static void setPositionX(this Component comp, float newValue)
    {
        if (comp != null)
        {
            setPositionX(comp.transform, newValue);
        }
    }

    public static void setPositionY(this Component comp, float newValue)
    {
        if (comp != null)
        {
            setPositionY(comp.transform, newValue);
        }
    }

    public static void setPositionZ(this Component comp, float newValue)
    {
        if (comp != null)
        {
            setPositionZ(comp.transform, newValue);
        }
    }

    public static void setAnchorPosition(this RectTransform transform, float x, float y, float z = 0)
    {
        Vector3 v = transform.anchoredPosition;
        v.x = x;
        v.y = y;
        v.z = z;
        transform.anchoredPosition = v;
    }

    public static void setAnchorPositionX(this RectTransform transform, float newValue)
    {
        Vector3 v = transform.anchoredPosition;
        v.x = newValue;
        transform.anchoredPosition = v;
    }

    public static void setAnchorPositionY(this RectTransform transform, float newValue)
    {
        Vector3 v = transform.anchoredPosition;
        v.y = newValue;
        transform.anchoredPosition = v;
    }

    public static void setAnchorPositionZ(this RectTransform transform, float newValue)
    {
        Vector3 v = transform.anchoredPosition;
        v.z = newValue;
        transform.anchoredPosition = v;
    }

    public static void setPositionX(this GameObject node, float x)
    {
        node.transform.setPositionX(x);
    }

    public static void setPositionY(this GameObject node, float y)
    {
        node.transform.setPositionY(y);
    }

    public static void setPosition(this GameObject node, float x, float y, float z = 0)
    {
        node.transform.setPosition(x, y, z);
    }

    public static void setPosition(this GameObject node, Vector2 pos)
    {
        node.transform.setPosition(pos.x, pos.y, 0);
    }

    public static void setPosition(this GameObject node, Vector3 pos)
    {
        node.transform.setPosition(pos.x, pos.y, pos.z);
    }

    public static void setPositionX(this Image image, float x)
    {
        image.rectTransform.setAnchorPositionX(x);
    }

    public static void setPositionY(this Image image, float y)
    {
        image.rectTransform.setAnchorPositionY(y);
    }

    public static void setPosition(this Image image, float x, float y, float z = 0)
    {
        image.rectTransform.setAnchorPosition(x, y, z);
    }

    public static void setPosition(this Image image, Vector2 vector)
    {
        image.rectTransform.setAnchorPosition(vector.x, vector.y, 0);
    }

    public static void setPosition(this Image image, Vector3 vector)
    {
        image.rectTransform.setAnchorPosition(vector.x, vector.y, vector.z);
    }

    public static void setPositionX(this TMP_Text text, float x)
    {
        text.rectTransform.setAnchorPositionX(x);
    }

    public static void setPositionY(this TMP_Text text, float y)
    {
        text.rectTransform.setAnchorPositionY(y);
    }

    public static void setPosition(this TMP_Text text, float x, float y, float z = 0)
    {
        text.rectTransform.setAnchorPosition(x, y, z);
    }

    public static void setPosition(this TMP_Text text, Vector2 vector)
    {
        text.rectTransform.setAnchorPosition(vector.x, vector.y, 0);
    }

    public static void setPosition(this TMP_Text text, Vector3 vector)
    {
        text.rectTransform.setAnchorPosition(vector.x, vector.y, vector.z);
    }

    #endregion

    #region 获得坐标

    /// <summary>
    /// 设置坐标(优先设置锚点坐标)
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public static Vector2 getPosition(this Transform transform)
    {
        var rectTransform = transform.GetComponent<RectTransform>();
        if (null != rectTransform)
        {
            return rectTransform.getAnchorPosition();
        }
        else
        {
            return transform.localPosition;
        }
    }

    public static float getPositionX(this Transform transform)
    {
        var rectTransform = transform.GetComponent<RectTransform>();
        if (null != rectTransform)
        {
            return rectTransform.getAnchorPositionX();
        }
        else
        {
            return transform.localPosition.x;
        }
    }

    public static float getPositionY(this Transform transform)
    {
        var rectTransform = transform.GetComponent<RectTransform>();
        if (null != rectTransform)
        {
            return rectTransform.getAnchorPositionY();
        }
        else
        {
            return transform.localPosition.y;
        }
    }

    public static Vector2 getAnchorPosition(this RectTransform transform)
    {
        return transform.anchoredPosition;
    }

    public static float getAnchorPositionX(this RectTransform transform)
    {
        return transform.anchoredPosition.x;
    }

    public static float getAnchorPositionY(this RectTransform transform)
    {
        return transform.anchoredPosition.y;
    }

    public static float getPositionX(this GameObject node)
    {
        return node.transform.getPositionX();
    }

    public static float getPositionY(this GameObject node)
    {
        return node.transform.getPositionY();
    }

    public static Vector2 getPosition(this GameObject node)
    {
        return node.transform.getPosition();
    }

    public static Vector2 getPosition(this Image image)
    {
        return image.rectTransform.getAnchorPosition();
    }

    public static float getPositionX(this Image image)
    {
        return image.rectTransform.getAnchorPositionX();
    }

    public static float getPositionY(this Image image)
    {
        return image.rectTransform.getAnchorPositionY();
    }

    public static Vector2 getPosition(this TMP_Text text)
    {
        return text.rectTransform.getPosition();
    }

    public static float getPositionX(this TMP_Text text)
    {
        return text.rectTransform.getAnchorPositionX();
    }

    public static float getPositionY(this TMP_Text text)
    {
        return text.rectTransform.getAnchorPositionY();
    }

    #endregion

    #region 设置透明度

    /// <summary>
    /// 设置透明度
    /// </summary>
    /// <param name="image"></param>
    /// <param name="alpha">0到255之间的浮点数</param>
    public static void setOpacity(this Image image, float alpha)
    {
        float _alpha = alpha;

        if (alpha < 0) _alpha = 0;
        if (alpha > 255.0f) _alpha = 255.0f;

        var color = image.color;
        color.a = _alpha / 255.0f;
        image.color = color;
    }

    public static void setOpacity(this TMP_Text text, float alpha)
    {
        float _alpha = alpha;

        if (alpha < 0) _alpha = 0;
        if (alpha > 255.0f) _alpha = 255.0f;

        var color = text.color;
        color.a = _alpha / 255.0f;
        text.color = color;
    }

    public static void setOpacity(this SpriteRenderer spr, float alpha)
    {
        float _alpha = alpha;

        if (alpha < 0) _alpha = 0;
        if (alpha > 255.0f) _alpha = 255.0f;

        var color = spr.color;
        color.a = _alpha / 255.0f;
        spr.color = color;
    }

    public static void setOpacity(this CanvasGroup canvasGroup, float alpha)
    {
        float _alpha = alpha;

        if (alpha < 0) _alpha = 0;
        if (alpha > 255.0f) _alpha = 255.0f;

        canvasGroup.alpha = _alpha / 255.0f;
    }

    public static void setOpacity(this Transform transform, float alpha, bool isApplyAllChilds = true)
    {
        if (!isApplyAllChilds)
        {
            var image = transform.GetComponent<Image>();
            if (null != image)
                image.setOpacity(alpha);

            var text = transform.GetComponent<TMP_Text>();
            if (null != image)
                text.setOpacity(alpha);
        }
        else
        {
            var canvasGroup = transform.GetComponent<CanvasGroup>();
            if (null != canvasGroup)
            {
                canvasGroup.setOpacity(alpha);
            }
            else
            {
                var images = transform.GetComponentsInChildren<Image>(true);
                foreach (var image in images)
                {
                    if (image.CompareTag("ignore"))
                        continue;

                    image.setOpacity(alpha);
                }

                var tmpTexts = transform.GetComponentsInChildren<TMP_Text>(true);
                foreach (var text in tmpTexts)
                {
                    if (text.CompareTag("ignore"))
                        continue;

                    text.setOpacity(alpha);
                }
            }
        }
    }

    public static void setOpacity(this GameObject node, float alpha, bool isApplyAllChilds = true)
    {
        node.transform.setOpacity(alpha, isApplyAllChilds);
    }

    #endregion

    #region 设置缩放

    public static void setScaleX(this Transform transform, float scale)
    {
        transform.SetLocalScaleX(scale);
    }

    public static void setScaleY(this Transform transform, float scale)
    {
        transform.SetLocalScaleY(scale);
    }
    public static float getScale(this Transform transform)
    {
        Vector3 v = transform.localScale;
        Assert.IsTrue(Math.Abs(v.x - v.y) < 0.00001, "localScale");
        return v.x;
    }
    public static void setScale(this Transform transform, float scale)
    {
        Vector3 v = transform.localScale;
        v.x = scale;
        v.y = scale;
        transform.localScale = v;
    }
    public static float getScale(this GameObject node)
    {
        Vector3 v = node.transform.localScale;
        Assert.IsTrue(Math.Abs(v.x - v.y) < 0.00001, "localScale");
        return v.x;
    }
    public static float getScaleY(this GameObject node)
    {
        return node.transform.localScale.y;
    }
    public static void setScale(this GameObject node, float scale)
    {
        Vector3 v = node.transform.localScale;
        v.x = scale;
        v.y = scale;
        node.transform.localScale = v;
    }
    public static void setScaleX(this GameObject node, float scale)
    {
        node.transform.setScaleX(scale);
    }

    public static void setScaleY(this GameObject node, float scale)
    {
        node.transform.setScaleY(scale);
    }

    public static void setFlipX(this GameObject node, bool flipX)
    {
        var scaleX = flipX ? -1 : 1;
        node.transform.setScaleX(scaleX);
    }

    public static void setFlipY(this GameObject node, bool flipY)
    {
        var scaleY = flipY ? -1 : 1;
        node.transform.setScaleY(scaleY);
    }

    public static void setScaleX(this Image image, float scale)
    {
        image.transform.setScaleX(scale);
    }

    public static void setScaleY(this Image image, float scale)
    {
        image.transform.setScaleY(scale);
    }

    public static void setScale(this Image image, float scale)
    {
        image.transform.setScale(scale);
    }

    public static void setScaleX(this TMP_Text text, float scale)
    {
        text.transform.setScaleX(scale);
    }

    public static void setScaleY(this TMP_Text text, float scale)
    {
        text.transform.setScaleY(scale);
    }

    public static void setScale(this TMP_Text text, float scale)
    {
        text.transform.setScale(scale);
    }

    public static void setFlipX(this Image image, bool flipX)
    {
        var scaleX = flipX ? -1 : 1;
        image.transform.setScaleX(scaleX);
    }

    public static void setFlipY(this Image image, bool flipY)
    {
        var scaleY = flipY ? -1 : 1;
        image.transform.setScaleX(scaleY);
    }

    #endregion

    #region 设置旋转
    public static void setRotation(this Transform transform, float rotation)
    {
        var localRotation = transform.localRotation;
        transform.localEulerAngles = new Vector3(localRotation.x, localRotation.y, -rotation);
    }

    public static void setRotation(this GameObject gameObject, float rotation)
    {
        gameObject.transform.setRotation(rotation);
    }

    public static void setRotation(this TMP_Text text, float rotation)
    {
        text.transform.setRotation(rotation);
    }

    public static void setRotation(this Image image, float rotation)
    {
        image.transform.setRotation(rotation);
    }
    #endregion

    #region 设置锚点

    public static void setAnchorPoint(this GameObject node, Vector2 v)
    {
        var rectTrans = node.GetComponent<RectTransform>();
        if (null == rectTrans)
            return;

        rectTrans.setAnchorPosition(v.x, v.y);
    }

    public static void setAnchorPoint(this GameObject node, float x, float y)
    {
        var rectTrans = node.GetComponent<RectTransform>();
        if (null == rectTrans)
            return;

        rectTrans.setAnchorPosition(x, y);
    }

    #endregion

    #region visible
    public static void setVisible(this GameObject gameObject, bool value)
    {
        gameObject.SetActiveEx(value);
    }
    public static void setVisible(this Transform transform, bool value)
    {
        transform.gameObject.SetActiveEx(value);
    }
    public static void setVisible(this MonoBehaviour behaviour, bool value)
    {
        behaviour.gameObject.setVisible(value);
    }
    public static void setVisible(this Image image, bool value)
    {
        image.gameObject.setVisible(value);
    }

    public static void setVisible(this TMP_Text image, bool value)
    {
        image.gameObject.setVisible(value);
    }

    public static void setVisible(this Button image, bool value)
    {
        image.gameObject.setVisible(value);
    }

    public static bool isVisible(this GameObject gameObject)
    {
        return gameObject.activeSelf;
    }
    public static bool isVisible(this MonoBehaviour behaviour)
    {
        return behaviour.gameObject.isVisible();
    }
    public static bool isVisible(this Image text)
    {
        return text.gameObject.isVisible();
    }
    public static bool isVisible(this TMP_Text text)
    {
        return text.gameObject.isVisible();
    }

    public static bool isVisible(this Button text)
    {
        return text.gameObject.isVisible();
    }

    #endregion

    #region 设置颜色

    public static void setColor(this IMImage img, int r, int g, int b)
    {
        img.color = new Color(r / 255.0f, g / 255.0f, b / 255.0f);
    }

    public static void setColor(this IMImage img, Color color)
    {
        img.color = color;
    }

    public static void setColor(this SpriteRenderer spr, Color color)
    {
        spr.color = color;
    }

    #endregion

    #region GameObject child

    public static void removeAllChildren(this GameObject gameObject, bool immediate = false)
    {
        if (null == gameObject)
            return;

        gameObject.transform.RemoveAllChildren(immediate);
    }
    public static void removeFromParent(this GameObject gameObject)
    {
        GameObject.Destroy(gameObject);
    }

    public static GameObject getParent(this GameObject gameObject)
    {
        return gameObject.transform.parent.gameObject;
    }

    public static void removeChildByName(this GameObject gameObject, string name)
    {
        // return gameObject.transform.parent.gameObject;
        Transform tf;
        if ((tf = gameObject.transform.Find(name)) != null)
        {
            GameObject.Destroy(tf.gameObject);
        }
    }

    public static Transform getChildByName(this GameObject gameObject, string nodeName)
    {
        return gameObject.transform.Find(nodeName);
    }

    public static int getChildrenCount(this GameObject node)
    {
        return node.transform.childCount;
    }

    public static void addChild(this GameObject node, Transform trans)
    {
        if (null == trans || null == node)
            return;

        trans.SetParent(node.transform);
    }

    public static void addChild(this GameObject node, Transform trans, bool worldPositionStays)
    {
        if (null == trans || null == node)
            return;

        trans.SetParent(node.transform, worldPositionStays);
    }
    #endregion

    #region TMP_InputField

    public static string getText(this TMP_InputField inputField)
    {
        return inputField.text;
    }

    public static void setText(this TMP_InputField inputField, string text)
    {
        inputField.SetTextWithoutNotify(text);
    }

    public static void setString(this TMP_InputField inputField, string text)
    {
        inputField.SetTextWithoutNotify(text);
    }

    public static void setMaxLength(this TMP_InputField inputField, int maxLength)
    {
        inputField.characterLimit = maxLength;
    }

    public static void setMaxChars(this TMP_InputField inputField, int maxLength)
    {
        inputField.characterLimit = maxLength;
    }

    #endregion

    #region STL

    public static int find_first_not_of(this string text, string match)
    {
        if (text.IsNullOrEmpty() || match.IsNullOrEmpty())
            return -1;

        for (int i = 0; i < text.Length; i++)
        {
            if (!match.Contains(text[i]))
                return i;
        }

        return -1;
    }

    #endregion
}
