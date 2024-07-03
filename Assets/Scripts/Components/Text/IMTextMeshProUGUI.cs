using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Framework;
using XLua;

/************************************************************************************
* @说    明: TextMeshProUGUI扩展,可根据不同语言设置字体属性,也可支持自动设置语言文本
* @作    者: zhoumingfeng
* @版 本 号: V1.00
* @创建时间: 2023.03.31
*************************************************************************************/
[ExecuteAlways]
[DisallowMultipleComponent]
public class IMTextMeshProUGUI : TextMeshProUGUI
{
    [Tooltip("全部转大写")]
    public bool wordUpper = false;

    #region 英文

    [Header("英文系字体")]
    public TMP_FontAsset fontEN;

    [LabelText("英文 字号")]
    public float fontSizeEN = 20;

    [LabelText("英文 样式")]
    public FontStyles styleEN;

    [LabelText("英文 行间距")]
    public float lineSpaceEN = -25.0f;

    [LabelText("英文 自动大小")]
    public bool autoFontSizeEN = false;

    [LabelText("英文 自动大小最小值")]
    public int minSizeEN = 10;

    [LabelText("英文 自动大小最大值")]
    public int maxSizeEN = 40;

    [Tooltip("是否不换行")]
    public bool isNonBreakingSpaceEN = false;

    #endregion

    #region 中文

    [Header("中文系字体")]
    public TMP_FontAsset fontCN;

    [LabelText("中文 字号")]
    public float fontSizeCN = 18;

    [LabelText("中文 样式")]
    public FontStyles styleCN;

    [LabelText("中文 行间距")]
    public float lineSpaceCN = -25.0f;

    [LabelText("中文 自动大小")]
    public bool autoFontSizeCN = false;

    [LabelText("中文 自动大小最小值")]
    public int minSizeCN = 10;

    [LabelText("中文 自动大小最大值")]
    public int maxSizeCN = 40;

    [Tooltip("是否不换行")]
    public bool isNonBreakingSpaceCN = false;

    #endregion 中文

    #region 日语

    [Header("日语字体")]
    public TMP_FontAsset fontJA;

    [LabelText("日语 字号")]
    public float fontSizeJA = 18;

    [LabelText("日语 样式")]
    public FontStyles styleJA;

    [LabelText("日语 行间距")]
    public float lineSpaceJA = 0.0f;

    [LabelText("日语 自动大小")]
    public bool autoFontSizeJA = false;

    [LabelText("日语 自动大小最小值")]
    public int minSizeJA = 10;

    [LabelText("日语 自动大小最大值")]
    public int maxSizeJA = 40;

    [Tooltip("是否不换行")]
    public bool isNonBreakingSpaceJA = false;

    #endregion 日语

    #region 韩语

    [Header("韩语字体")]
    public TMP_FontAsset fontKO;

    [LabelText("韩语 字号")]
    public float fontSizeKO = 16;

    [LabelText("韩语 样式")]
    public FontStyles styleKO;

    [LabelText("韩语 行间距")]
    public float lineSpaceKO = -25.0f;

    [LabelText("韩语 自动大小")]
    public bool autoFontSizeKO = false;

    [LabelText("韩语 自动大小最小值")]
    public int minSizeKO = 10;

    [LabelText("韩语 自动大小最大值")]
    public int maxSizeKO = 40;

    [Tooltip("是否不换行")]
    public bool isNonBreakingSpaceKO = false;

    #endregion 韩语

    #region 泰语

    [Header("泰语字体")]
    public TMP_FontAsset fontTH;

    [LabelText("泰语 字号")]
    public float fontSizeTH = 18;

    [LabelText("泰语 样式")]
    public FontStyles styleTH;

    [LabelText("泰语 行间距")]
    public float lineSpaceTH = -25.0f;

    [LabelText("泰语 自动大小")]
    public bool autoFontSizeTH = false;

    [LabelText("泰语 自动大小最小值")]
    public int minSizeTH = 10;

    [LabelText("泰语 自动大小最大值")]
    public int maxSizeTH = 40;

    [Tooltip("是否不换行")]
    public bool isNonBreakingSpaceTH = false;

    #endregion 泰语

    #region 阿拉伯语

    [Header("阿拉伯语字体")]
    public TMP_FontAsset fontAR;

    [LabelText("阿拉伯语 字号")]
    public float fontSizeAR = 20;

    [LabelText("阿拉伯语 样式")]
    public FontStyles styleAR;

    [LabelText("阿拉伯语 行间距")]
    public float lineSpaceAR = -25.0f;

    [LabelText("阿拉伯语 自动大小")]
    public bool autoFontSizeAR = false;

    [LabelText("阿拉伯语 自动大小最小值")]
    public int minSizeAR = 10;

    [LabelText("阿拉伯语 自动大小最大值")]
    public int maxSizeAR = 40;

    [Tooltip("是否不换行")]
    public bool isNonBreakingSpaceAR = false;

    [LabelText("阿拉伯语 右对齐")]
    public bool isAlignmentRight = false;

    #endregion 阿拉伯语

    #region 设置语言ID

    // 语言ID
    public int languageId = 0;

    public bool setPara1 = false;
    public string para1 = "";

    public bool setPara2 = false;
    public string para2 = "";

    public bool setPara3 = false;
    public string para3 = "";

    #endregion 设置语言ID

    //设置描边
    public bool isUseOutline = false;
    public Color outLineColor = Color.black;
    [Range(0, 1)] public float outLineSize = 0.1f;

    //设置阴影
    public bool isUseShadow = false;
    public Color shadowColor = Color.black;
    [Range(-1, 1)] public float offsetX = 0.5f;
    [Range(-1, 1)] public float offsetY = -0.5f;

    private Material _material;
    private TMP_FontAsset _font;

    //private static readonly int s_DiffuseMaskTex_ID = Shader.PropertyToID("_DiffuseMaskTex");
    //private static readonly int s_Softness_ID = Shader.PropertyToID("_OutlineSoftness");
    //private static readonly int s_Dilate_ID = Shader.PropertyToID("_FaceDilate");
    //private static readonly int s_FaceTex_ID = Shader.PropertyToID("_FaceTex");

    private static readonly int s_OutlineColor_ID = Shader.PropertyToID("_OutlineColor");
    private static readonly int s_OutlineThickness_ID = Shader.PropertyToID("_OutlineWidth");

    private static readonly int s_UnderlayColor_ID = Shader.PropertyToID("_UnderlayColor");
    private static readonly int s_UnderlayOffsetX_ID = Shader.PropertyToID("_UnderlayOffsetX");
    private static readonly int s_UnderlayOffsetY_ID = Shader.PropertyToID("_UnderlayOffsetY");
    //private static readonly int s_UnderlayDilate_ID = Shader.PropertyToID("_UnderlayDilate");
    //private static readonly int s_UnderlaySoftness_ID = Shader.PropertyToID("_UnderlaySoftness");

    private string nobr = "<nobr>{0}</nobr>";

    #region 框架接口

    protected override void OnEnable()
    {
        base.OnEnable();

        if (!Application.isPlaying)
        {
            this.CheckFontChange();
        }
        else
        {
            this.SetFont();

            this.CheckFontChange();

            this.AutoFillText();
        }

        

        
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        ReleaseMaterial();
    }

    #endregion

    #region 公有接口

    public void SetDialogId(int dialogId, params object[] args)
    {
        this.SetText(GameEntry.Localization.GetString(dialogId, args));
    }

    public void SetDialogId(string dialogId, params object[] args)
    {
        this.SetText(GameEntry.Localization.GetString(dialogId, args));
    }

    public void SetText(int value)
    {
        this.SetTextEx(value);
    }

    public void SetText(params object[] args)
    {
        this.SetTextEx(args);
    }

    public void SetText(string content)
    {
        this.SetTextEx(content);
    }

    public void SetTextEx(int value)
    {
        this.SetBaseText(value.ToString());
    }

    public void SetTextEx(string content)
    {
        if (string.IsNullOrEmpty(content))
        {
            this.SetBaseText("");
            return;
        }

        string text = content;
        text = ReplaceRichText(text);
        //if (GameEntry.Localization.Language == Language.Arabic)
        //{// 阿拉伯语从右往左读, 需要将文本反转一下
        //    // 查找是否存在自适应文本宽度的组件,否则需要自行控制换行
        //    var fitter = this.GetComponent<ContentSizeFitter>();
        //    if (null == fitter || fitter.horizontalFit != ContentSizeFitter.FitMode.PreferredSize)
        //    {
        //        var arabic = ArabicSupport.ArabicFixer.FixEx(content, true, false);
        //        this.SetArabicTextSupportWrap(arabic);
        //    }
        //    else
        //    {
        //        text = ArabicSupport.ArabicFixer.FixEx(content, true, false);
        //        this.SetBaseText(text);
        //    }

        //    // 设置文本右对齐
        //    //this.SetAlignmentForArabic();
        //}
        //else
        {
            this.SetBaseText(text);
        }
    }

    public void SetTextEx(params object[] args)
    {
        string result = "";
        string refDialogId = "";
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] is string)
            {
                result += args[i];
            }
            else if (args[i] is int || args[i] is long || args[i] is float || args[i] is double)
            {
                result += args[i].ToString();
            }
            else if (args[i] is LuaTable)
            {
                result += UnityExtension.ParseLuaTable(args[i], ref refDialogId);
            }
            else
            {
                result += UnityExtension.ParseTuple(args[i], ref refDialogId);
            }
        }

        this.SetTextEx(result);
    }

    public void SetAutoFontSizeRange(float minSize, float maxSize)
    {
        this.fontSizeMin = minSize;
        this.fontSizeMax = maxSize;
    }

    public void SetNonBreakingSpace(bool isNonBreakingSpace)
    {
        this.m_isNonBreakingSpace = isNonBreakingSpace;
    }

    public void Refresh()
    {
        OnDisable();
        OnEnable();
    }

    #endregion

    #region 扩展cocos接口

    public void setString(string text)
    {
        this.SetText(text);
    }

    public void setString(int text)
    {
        this.SetText(text.ToString());
    }

    public void setVisible(bool value)
    {
        this.gameObject.setVisible(value);
    }

    public bool isVisible()
    {
        return this.gameObject.activeSelf;
    }

    public void setColor(int r, int g, int b)
    {
        this.color = new Color(r / 255.0f, g / 255.0f, b / 255.0f);
    }

    public void setColor(Color color)
    {
        this.color = color;
    }

    public Color getColor()
    {
        return this.color;
    }

    public float getFontSize()
    {
        return this.fontSize;
    }

    public void setFontSize(float size)
    {
        this.setSystemFontSize(size);
    }

    public void setSystemFontSize(float fontSize)
    {
        this.fontSize = fontSize;
        this.fontSizeEN = this.fontSize;
        this.fontSizeCN = this.fontSize;
        this.fontSizeJA = this.fontSize;
        this.fontSizeKO = this.fontSize;
        this.fontSizeTH = this.fontSize;
        this.fontSizeAR = this.fontSize;
    }

    public void enableOutline(Color outLineColor, float outLineSize)
    {
        this.isUseOutline = true;
        this.outLineColor = outLineColor;
        this.outLineSize = outLineSize / 10;
    }

    #endregion

    #region 私有接口

    private void SetBaseText(string content)
    {
        string target = content;

        // 是否换行
        if(!string.IsNullOrEmpty(content) && IsNonBreakingSpace())
            target = string.Format(nobr, content);

        //if (wordUpper)
        //    target = target.ToUpperInvariant();

        base.SetText(target);
    }

    /// <summary>
    /// 设置字体
    /// </summary>
    private void SetFont()
    {
        if (null == GameEntry.Localization)
            return;

        switch (GameEntry.Localization.Language)
        {
            case Language.ChineseSimplified:
            case Language.ChineseTraditional:
            case Language.Turkish:
                {
                    if (null == this.fontCN)
                        return;

                    if (this.font != this.fontCN)
                        this.font = this.fontCN;

                    this.fontStyle = this.styleCN;
                    this.fontSize = this.fontSizeCN;
                    this.lineSpacing = this.lineSpaceCN;

                    if (this.autoFontSizeCN)
                    {
                        this.enableAutoSizing = this.autoFontSizeCN;
                        this.SetAutoFontSizeRange(this.minSizeCN, this.maxSizeCN);
                    }

                    break;
                }
            case Language.Japanese:
                {
                    if (null == this.fontJA)
                        return;

                    if (this.font != this.fontJA)
                        this.font = this.fontJA;

                    this.fontStyle = this.styleJA;
                    this.fontSize = this.fontSizeJA;
                    this.lineSpacing = this.lineSpaceJA;
                    
                    if (this.autoFontSizeJA)
                    {
                        this.enableAutoSizing = this.autoFontSizeJA;
                        this.SetAutoFontSizeRange(this.minSizeJA, this.maxSizeJA);
                    }

                    break;
                }
            case Language.Korean:
                {
                    if (null == this.fontKO)
                        return;

                    if (this.font != this.fontKO)
                        this.font = this.fontKO;

                    this.fontStyle = this.styleKO;
                    this.fontSize = this.fontSizeKO;
                    this.lineSpacing = this.lineSpaceKO;

                    if (this.autoFontSizeKO)
                    {
                        this.enableAutoSizing = this.autoFontSizeKO;
                        this.SetAutoFontSizeRange(this.minSizeKO, this.maxSizeKO);
                    }

                    break;
                }
            case Language.Thai:
                {
                    if (null == this.fontTH)
                        return;

                    if (this.font != this.fontTH)
                        this.font = this.fontTH;

                    this.fontStyle = this.styleTH;
                    this.fontSize = this.fontSizeTH;
                    this.lineSpacing = this.lineSpaceTH;
                    
                    if (this.autoFontSizeTH)
                    {
                        this.enableAutoSizing = this.autoFontSizeTH;
                        this.SetAutoFontSizeRange(this.minSizeTH, this.maxSizeTH);
                    }

                    break;
                }
            case Language.Arabic:
                {
                    if (null == this.fontAR)
                        return;

                    if (this.font != this.fontAR)
                        this.font = this.fontAR;

                    if (this.isAlignmentRight)
                        this.SetAlignmentForArabic();

                    this.fontStyle = this.styleAR;
                    this.fontSize = this.fontSizeAR;
                    this.lineSpacing = this.lineSpaceAR;
                    
                    if (this.autoFontSizeAR)
                    {
                        this.enableAutoSizing = this.autoFontSizeAR;
                        this.SetAutoFontSizeRange(this.minSizeAR, this.maxSizeAR);
                    }

                    break;
                }
            default:
                {
                    if (null == this.fontEN)
                        return;

                    if (this.font != this.fontEN)
                        this.font = this.fontEN;

                    this.fontStyle = this.styleEN;
                    this.fontSize = this.fontSizeEN;
                    this.lineSpacing = this.lineSpaceEN;

                    if(this.autoFontSizeEN)
                    {
                        this.enableAutoSizing = this.autoFontSizeEN;
                        this.SetAutoFontSizeRange(this.minSizeEN, this.maxSizeEN);
                    }

                    break;
                }
        }

        if (wordUpper)
        {
            if ((this.fontStyle & FontStyles.UpperCase) != FontStyles.UpperCase)
                this.fontStyle |= FontStyles.UpperCase;
        }
    }

    /// <summary>
    /// 自动填充文本
    /// </summary>
    private void AutoFillText()
    {
        if (this.languageId <= 0)
            return;

        string text = string.Empty;
        if(setPara1 && setPara2 && setPara3)
            text = GameEntry.Localization.GetString(languageId, para1, para2, para3);
        else if (setPara1 && setPara2)
            text = GameEntry.Localization.GetString(languageId, para1, para2);
        else if(setPara1)
            text = GameEntry.Localization.GetString(languageId, para1);
        else
            text = GameEntry.Localization.GetString(languageId);

        this.SetTextEx(text);
    }

    private bool IsNonBreakingSpace()
    {
        bool isNonBreakingSpace = false;

        if (null == GameEntry.Localization)
            return isNonBreakingSpace;

        switch (GameEntry.Localization.Language)
        {
            case Language.ChineseSimplified:
            case Language.ChineseTraditional:
            case Language.Turkish:
                {
                    isNonBreakingSpace = this.isNonBreakingSpaceCN;

                    break;
                }
            case Language.Japanese:
                {
                    isNonBreakingSpace = this.isNonBreakingSpaceJA;

                    break;
                }
            case Language.Korean:
                {
                    isNonBreakingSpace = this.isNonBreakingSpaceKO;

                    break;
                }
            case Language.Thai:
                {
                    isNonBreakingSpace = this.isNonBreakingSpaceTH;

                    break;
                }
            case Language.Arabic:
                {
                    isNonBreakingSpace = this.isNonBreakingSpaceAR;

                    break;
                }
            default:
                {
                    isNonBreakingSpace = this.isNonBreakingSpaceEN;

                    break;
                }
        }

        return isNonBreakingSpace;
    }

    private void RefreshText()
    {
        if (!IsNonBreakingSpace())
            return;

        string content = this.text;
        if (content.StartsWith("<nobr>") && content.EndsWith("</nobr>"))
            return;

        this.SetBaseText(content);
    }

    private void CheckFontChange()
    {
        bool changed = false;
        if (_font != font)
        {
            _font = font;
            changed = true;
        }

        if (changed)
            ReleaseMaterial();

        if (_material == null && font.material != null)
        {
            _material = new Material(font.material) { hideFlags = HideFlags.HideAndDontSave };
            fontMaterial = _material;
        }

        RefreshProperty();
    }

    private void RefreshProperty()
    {
        if (_material == null)
            return;


        SetKeyword(_material, "OUTLINE_ON", isUseOutline);
        if (isUseOutline)
        {
            _material.SetColor(s_OutlineColor_ID, outLineColor);
            _material.SetFloat(s_OutlineThickness_ID, outLineSize);
        }
        else
        {
            _material.SetFloat(s_OutlineThickness_ID, 0);
        }

        if (isUseShadow)
        {

            SetKeyword(_material, "UNDERLAY_ON", true);
             SetKeyword(_material, "UNDERLAY_INNER", true);

            _material.SetColor(s_UnderlayColor_ID, shadowColor);
            _material.SetFloat(s_UnderlayOffsetX_ID, offsetX);
            _material.SetFloat(s_UnderlayOffsetY_ID, offsetY);
        }
        else
        {
            SetKeyword(_material, "UNDERLAY_ON", false);
            SetKeyword(_material, "UNDERLAY_INNER", false);
        }

        UpdateMeshPadding();
        fontMaterial = _material;
    }

    private void SetKeyword(Material mat, string key, bool enable)
    {
        if (enable)
            mat.EnableKeyword(key);
        else
            mat.DisableKeyword(key);
    }

    private void ReleaseMaterial()
    {
        if (_material != null)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                UnityEngine.Object.DestroyImmediate(_material, false);
            else
#endif
                UnityEngine.Object.Destroy(_material);
            _material = null;
        }
    }

    #endregion

    #region 字符串处理

    private void AddDialogId(ref string dialogId, string dialog)
    {
        if (dialog.IsNullOrEmpty())
            return;
        if (dialogId.IsNullOrEmpty())
            dialogId = dialog;
        else
            dialogId = dialogId + "|" + dialog;
    }

    private string ParseLuaTable(object args, ref string dialogId)
    {
        if (args is LuaTable)
        {
            var tmp = args as LuaTable;
            return ParseLuaTableStr(tmp, ref dialogId);
        }
        return "";
    }

    private string ParseTuple(object args, ref string dialogId)
    {
        var strResult = ParseTupleStr(args, ref dialogId);
        if (strResult.IsNullOrEmpty())
            strResult = ParseTupleInt(args, ref dialogId);
        return strResult;
    }

    private string ParseTupleStr(object args, ref string dialogId)
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

    private string ParseTupleInt(object args, ref string dialogId)
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

    private string ParseLuaTableStr(LuaTable tmp, ref string dialogId)
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
    
    private string ReplaceRichText(string originalStr)
    {
        List<Match> matches = new List<Match>();
        string pattern = @"\[(color=|u|b|link|/color|/u|/b|/link)([^\]]*)\]";
        foreach (Match match in Regex.Matches(originalStr, pattern))
        {
            matches.Add(match);
        }

        foreach (Match match in matches)
        {
            string tag = match.Groups[1].Value;
            string value = match.Groups[2].Value;

            switch (tag)
            {
                case "color=":
                    if (value.Length == 8)
                    {
                        value = value.Substring(2, 6);
                    }
                    originalStr = originalStr.Replace(match.Value, $"<color=#{value}>");
                    break;

                case "/color":
                    originalStr = originalStr.Replace(match.Value, "</color>");
                    break;

                case "u":
                    originalStr = originalStr.Replace(match.Value, "<u>");
                    break;

                case "/u":
                    originalStr = originalStr.Replace(match.Value, "</u>");
                    break;

                case "b":
                    originalStr = originalStr.Replace(match.Value, "<b>");
                    break;

                case "/b":
                    originalStr = originalStr.Replace(match.Value, "</b>");
                    break;

                case "link":
                    string[] parameters = value.Split(' ');
                    string bg = "";
                    string bgClick = "";

                    foreach (string param in parameters)
                    {
                        string[] keyValue = param.Split('=');
                        if (keyValue.Length == 2)
                        {
                            string key = keyValue[0].Trim();
                            string val = keyValue[1].Trim();
                            if (key.Equals("bg", System.StringComparison.OrdinalIgnoreCase))
                            {
                                bg = val;
                                if (bg.Length == 8)
                                {
                                    bg = bg.Substring(2, 6);
                                }
                            }
                            else if (key.Equals("bg_click", System.StringComparison.OrdinalIgnoreCase))
                            {
                                bgClick = val;
                                if (bgClick.Length == 8)
                                {
                                    bgClick = bgClick.Substring(2, 6);
                                }
                            }
                        }
                    }

                    originalStr = originalStr.Replace(match.Value, $"<link bg={bg} bg_click={bgClick}>");
                    break;

                case "/link":
                    originalStr = originalStr.Replace(match.Value, "</link>");
                    break;
            }
        }

        return originalStr;
    }

    #endregion

    #region 阿拉伯语显示处理

    /// <summary>
    /// 按照阿拉伯语规则设置文本
    /// </summary>
    /// <param name="tmp"></param>
    /// <param name="content"></param>
    //public void SetArabicText(string content)
    //{
    //    if (string.IsNullOrEmpty(content))
    //    {
    //        this.SetBaseText("");
    //        return;
    //    }

    //    string text = ArabicSupport.ArabicFixer.FixEx(content, true, false);
    //    this.SetBaseText(text);
    //}

    /// <summary>
    /// 为阿拉伯语设置文字对齐方式
    /// 由于阿拉伯语是从右往左阅读, 左对齐需要改成右对齐
    /// </summary>
    private void SetAlignmentForArabic()
    {
        if (GameEntry.Localization.Language != Language.Arabic)
            return;

        if (this.alignment == TextAlignmentOptions.MidlineLeft)
            this.alignment = TextAlignmentOptions.MidlineRight;
        else if (this.alignment == TextAlignmentOptions.BottomLeft)
            this.alignment = TextAlignmentOptions.BottomRight;
        else if (this.alignment == TextAlignmentOptions.TopLeft)
            this.alignment = TextAlignmentOptions.TopRight;
        else if (this.alignment == TextAlignmentOptions.Left)
            this.alignment = TextAlignmentOptions.Right;
    }

    /*
    private void SetArabicTextSupportWrap(string arabic)
    {
        if (string.IsNullOrEmpty(arabic))
        {
            this.SetBaseText("");
            return;
        }

        if (this.enableWordWrapping)
        {// 换行处理
            var rect = this.GetComponent<RectTransform>();
            float width = rect.sizeDelta.x;
            float length = this.GetTextLeng(arabic);
            if (width > 0 && width < length)
            {
                string content = arabic;

                RichLabelRanges richLabelRanges = new RichLabelRanges();
                richLabelRanges.InitRangeInfos(content);
                if (this.richText && richLabelRanges.Count > 0)
                    content = RichLabelUtils.RemoveLabels(content);

                if (content.Contains("\n"))
                    content = content.Replace("\n", Environment.NewLine);

                if (content.Contains("\r\r\n"))
                    content = content.Replace("\r\r\n", Environment.NewLine);

                if (content.Contains(Environment.NewLine))
                {
                    string[] stringSeparators = new string[] { Environment.NewLine };
                    string[] strLine = content.Split(stringSeparators, StringSplitOptions.None);

                    string arabicLine = this.GetArabicLineText(width, strLine[0]);
                    for (int i = 1; i < strLine.Length; i++)
                        arabicLine += Environment.NewLine + this.GetArabicLineText(width, strLine[i]);

                    this.SetBaseText(arabicLine);
                }
                else
                {
                    var array = content.Split(new char[] { ' ' });
                    if (array.Length <= 1)
                        this.SetText(arabic);
                    else
                        this.SetBaseText(this.GetArabicLineText(width, content));
                }
            }
            else
            {
                this.SetBaseText(arabic);
            }
        }
        else
        {
            this.SetBaseText(arabic);
        }
    }

    private string GetArabicLineText(float width, string content = null)
    {
        // split之后是反序的
        var array = content.Split(new char[] { ' ' });
        if (array.Length <= 1)
            return content;

        StringBuilder stringBuilder = new StringBuilder();

        int start = array.Length - 1;
        for (int i = array.Length - 1; i >= 0; i--)
        {
            if (!string.IsNullOrEmpty(array[i]))
            {
                start = i;
                stringBuilder.Append(array[i]);

                break;
            }
        }

        float length = 0;
        string part = stringBuilder.ToString();
        for (int i = start - 1; i >= 0; i--)
        {
            if (string.IsNullOrEmpty(array[i]))
                continue;

            part += " ";
            stringBuilder.Append(" ");

            if (i > 0)
                length = this.GetTextLeng(part + " " + array[i] + " ");
            else
                length = this.GetTextLeng(part + " " + array[i]);

            // 如果长度超过了文本组件宽度,那么添加换行符
            if (length > width)
            {
                part = string.Empty;

                stringBuilder.Append(Environment.NewLine);
            }

            part += array[i];
            stringBuilder.Append(array[i]);
        }

        string temp = stringBuilder.ToString();
        string[] stringSeparators = new string[] { Environment.NewLine };
        string[] strSplit = temp.Split(stringSeparators, StringSplitOptions.None);

        stringBuilder.Clear();
        for (int i = 0; i < strSplit.Length; i++)
        {
            var tmpArray = strSplit[i].Split(new char[] { ' ' });
            if (tmpArray.Length <= 1)
            {
                stringBuilder.Append(tmpArray[0]);
            }
            else
            {
                for (int j = tmpArray.Length - 1; j >= 0; j--)
                {
                    if (string.IsNullOrEmpty(tmpArray[j]))
                        continue;

                    stringBuilder.Append(tmpArray[j]);

                    if (j > 0)
                        stringBuilder.Append(" ");
                }
            }

            if (i < strSplit.Length - 1)
            {
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append(" ");
            }
        }

        return stringBuilder.ToString();
    }

    private float GetTextLeng(string str = null)
    {
        string content = string.IsNullOrEmpty(str) ? this.text : str;
        if (string.IsNullOrEmpty(content))
            return 0;

        RichLabelRanges richLabelRanges = new RichLabelRanges();
        richLabelRanges.InitRangeInfos(content);
        if (this.richText && richLabelRanges.Count > 0)
            content = RichLabelUtils.RemoveLabels(content);

        TMP_TextInfo info;

        try
        {
            info = this.GetTextInfo(content);
        }
        catch (Exception e)
        {
            Log.Error("TMP_Text.GetTextInfo {0} throw exception, error:{1}", string.IsNullOrEmpty(str) ? "" : str, e == null ? "" : e.Message);
            return 0;
        }

        float totalTextLeng = 0;
        for (int i = 0; i < info.characterCount; i++)
            totalTextLeng += Mathf.Abs(info.characterInfo[i].topLeft.x - info.characterInfo[i].topRight.x);

        return totalTextLeng;
    }
    */

    #endregion

    #region Editor处理逻辑
    
#if UNITY_EDITOR

    protected override void OnValidate()
    {
        base.OnValidate();

        RefreshProperty();
    }

    [BlackList]
    public void AutoFillFont()
    {
        this.font = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/Main/Arts/Fonts/ArialMT_SDF.asset");
        this.fontEN = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/Main/Arts/Fonts/ArialMT_SDF.asset");
        this.fontCN = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/Main/Arts/Fonts/SourceHanSansSC-Regular_SDF.asset");
        this.fontJA = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/Main/Arts/Fonts/SourceHanSansSC-Regular_SDF.asset");
        this.fontKO = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/Main/Arts/Fonts/SourceHanSansSC-Regular_SDF.asset");
        this.fontTH = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/Main/Arts/Fonts/ArialMT_SDF.asset");
        this.fontAR = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/Main/Arts/Fonts/ArialMT_SDF.asset");

        this.fontSize = this.fontSizeEN;
        this.fontStyle = this.styleEN;
        this.lineSpacing = this.lineSpaceEN;
        this.enableAutoSizing = this.autoFontSizeEN;
        if(this.enableAutoSizing)
            this.SetAutoFontSizeRange(this.minSizeEN, this.maxSizeEN);

        EditorUtility.SetDirty(this.gameObject);
    }


    /// <summary>
    /// 设置字体
    /// </summary>
    [BlackList]
    public void SetLanguageFont(Language language)
    {
        switch (language)
        {
            case Language.ChineseSimplified:
            case Language.ChineseTraditional:
            case Language.Turkish:
                {
                    if (null == this.fontCN)
                        return;

                    if (this.font != this.fontCN)
                        this.font = this.fontCN;

                    this.fontStyle = this.styleCN;
                    this.fontSize = this.fontSizeCN;
                    this.lineSpacing = this.lineSpaceCN;
                    this.enableAutoSizing = this.autoFontSizeCN;

                    if (this.enableAutoSizing)
                        this.SetAutoFontSizeRange(this.minSizeCN, this.maxSizeCN);

                    break;
                }
            case Language.Japanese:
                {
                    if (null == this.fontJA)
                        return;

                    if (this.font != this.fontJA)
                        this.font = this.fontJA;

                    this.fontStyle = this.styleJA;
                    this.fontSize = this.fontSizeJA;
                    this.lineSpacing = this.lineSpaceJA;
                    this.enableAutoSizing = this.autoFontSizeJA;

                    if (this.enableAutoSizing)
                        this.SetAutoFontSizeRange(this.minSizeJA, this.maxSizeJA);

                    break;
                }
            case Language.Korean:
                {
                    if (null == this.fontKO)
                        return;

                    if (this.font != this.fontKO)
                        this.font = this.fontKO;

                    this.fontStyle = this.styleKO;
                    this.fontSize = this.fontSizeKO;
                    this.lineSpacing = this.lineSpaceKO;
                    this.enableAutoSizing = this.autoFontSizeKO;

                    if (this.enableAutoSizing)
                        this.SetAutoFontSizeRange(this.minSizeKO, this.maxSizeKO);

                    break;
                }
            case Language.Thai:
                {
                    if (null == this.fontTH)
                        return;

                    if (this.font != this.fontTH)
                        this.font = this.fontTH;

                    this.fontStyle = this.styleTH;
                    this.fontSize = this.fontSizeTH;
                    this.lineSpacing = this.lineSpaceTH;
                    this.enableAutoSizing = this.autoFontSizeTH;

                    if (this.enableAutoSizing)
                        this.SetAutoFontSizeRange(this.minSizeTH, this.maxSizeTH);

                    break;
                }
            case Language.Arabic:
                {
                    if (null == this.fontAR)
                        return;

                    if (this.font != this.fontAR)
                        this.font = this.fontAR;

                    if (this.isAlignmentRight)
                        this.SetAlignmentForArabic();

                    this.fontStyle = this.styleAR;
                    this.fontSize = this.fontSizeAR;
                    this.lineSpacing = this.lineSpaceAR;
                    this.enableAutoSizing = this.autoFontSizeAR;

                    if (this.enableAutoSizing)
                        this.SetAutoFontSizeRange(this.minSizeAR, this.maxSizeAR);

                    break;
                }
            default:
                {
                    if (null == this.fontEN)
                        return;

                    if (this.font != this.fontEN)
                        this.font = this.fontEN;

                    this.fontStyle = this.styleEN;
                    this.fontSize = this.fontSizeEN;
                    this.lineSpacing = this.lineSpaceEN;
                    this.enableAutoSizing = this.autoFontSizeEN;

                    if (this.enableAutoSizing)
                        this.SetAutoFontSizeRange(this.minSizeEN, this.maxSizeEN);

                    break;
                }
        }

        if (wordUpper)
        {
            if ((this.fontStyle & FontStyles.UpperCase) != FontStyles.UpperCase)
                this.fontStyle |= FontStyles.UpperCase;
        }

        this.RefreshText(language);

#if UNITY_EDITOR
        this.gameObject.SetActive(false);
        this.gameObject.SetActive(true);
#endif

    }

    private void RefreshText(Language language)
    {
        string content = this.text;
        if (string.IsNullOrEmpty(content))
            return;

        string target = content;

        if (!IsNonBreakingSpace(language))
        {
            if (content.StartsWith("<nobr>") || content.EndsWith("</nobr>"))
            {
                target = target.Replace("<nobr>", "");
                target = target.Replace("</nobr>", "");
            }
        }
        else
        {
            if (content.StartsWith("<nobr>") && content.EndsWith("</nobr>"))
                return;

            // 是否换行
            if (!string.IsNullOrEmpty(content))
                target = string.Format(nobr, content);
        }

        base.SetText(target);
    }

    private bool IsNonBreakingSpace(Language language)
    {
        bool isNonBreakingSpace = false;

        switch (language)
        {
            case Language.ChineseSimplified:
            case Language.ChineseTraditional:
            case Language.Turkish:
                {
                    isNonBreakingSpace = this.isNonBreakingSpaceCN;

                    break;
                }
            case Language.Japanese:
                {
                    isNonBreakingSpace = this.isNonBreakingSpaceJA;

                    break;
                }
            case Language.Korean:
                {
                    isNonBreakingSpace = this.isNonBreakingSpaceKO;

                    break;
                }
            case Language.Thai:
                {
                    isNonBreakingSpace = this.isNonBreakingSpaceTH;

                    break;
                }
            case Language.Arabic:
                {
                    isNonBreakingSpace = this.isNonBreakingSpaceAR;

                    break;
                }
            default:
                {
                    isNonBreakingSpace = this.isNonBreakingSpaceEN;

                    break;
                }
        }

        return isNonBreakingSpace;
    }

#endif

    #endregion
}
