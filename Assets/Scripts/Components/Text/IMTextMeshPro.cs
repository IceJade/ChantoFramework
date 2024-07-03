using Sirenix.OdinInspector;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Framework;
using XLua;

/************************************************************************************
* @说    明: TextMeshPro扩展,可根据不同语言设置字体属性,也可支持自动设置语言文本
* @作    者: zhoumingfeng
* @版 本 号: V1.00
* @创建时间: 2023.03.31
*************************************************************************************/

[DisallowMultipleComponent]
public class IMTextMeshPro : TextMeshPro
{
    [Header("是否使用本地字体")]
    public bool useLocalFont = false;

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
    public float minSizeEN = 10;

    [LabelText("英文 自动大小最大值")]
    public float maxSizeEN = 40;

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
    public float minSizeCN = 10;

    [LabelText("中文 自动大小最大值")]
    public float maxSizeCN = 40;

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
    public float minSizeJA = 10;

    [LabelText("日语 自动大小最大值")]
    public float maxSizeJA = 40;

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
    public float minSizeKO = 10;

    [LabelText("韩语 自动大小最大值")]
    public float maxSizeKO = 40;

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
    public float minSizeTH = 10;

    [LabelText("泰语 自动大小最大值")]
    public float maxSizeTH = 40;

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
    public float minSizeAR = 10;

    [LabelText("阿拉伯语 自动大小最大值")]
    public float maxSizeAR = 40;

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

    private static readonly int s_DiffuseMaskTex_ID = Shader.PropertyToID("_DiffuseMaskTex");
    private static readonly int s_Softness_ID = Shader.PropertyToID("_OutlineSoftness");
    private static readonly int s_Dilate_ID = Shader.PropertyToID("_FaceDilate");
    private static readonly int s_FaceTex_ID = Shader.PropertyToID("_FaceTex");

    private static readonly int s_OutlineColor_ID = Shader.PropertyToID("_OutlineColor");
    private static readonly int s_OutlineThickness_ID = Shader.PropertyToID("_OutlineWidth");

    private static readonly int s_UnderlayColor_ID = Shader.PropertyToID("_UnderlayColor");
    private static readonly int s_UnderlayOffsetX_ID = Shader.PropertyToID("_UnderlayOffsetX");
    private static readonly int s_UnderlayOffsetY_ID = Shader.PropertyToID("_UnderlayOffsetY");
    private static readonly int s_UnderlayDilate_ID = Shader.PropertyToID("_UnderlayDilate");
    private static readonly int s_UnderlaySoftness_ID = Shader.PropertyToID("_UnderlaySoftness");


    protected override void OnEnable()
    {
        base.OnEnable();

        this.SetFont();

        this.AutoFillText();

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

    /// <summary>
    /// 设置字体
    /// </summary>
    private void SetFont()
    {
        if (!useLocalFont)
            return;

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
                    this.enableAutoSizing = this.autoFontSizeCN;

                    if (this.enableAutoSizing)
                    {
                        this.fontSizeMin = this.minSizeCN;
                        this.fontSizeMax = this.maxSizeCN;
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
                    this.enableAutoSizing = this.autoFontSizeJA;

                    if (this.enableAutoSizing)
                    {
                        this.fontSizeMin = this.minSizeJA;
                        this.fontSizeMax = this.maxSizeJA;
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
                    this.enableAutoSizing = this.autoFontSizeKO;

                    if (this.enableAutoSizing)
                    {
                        this.fontSizeMin = this.minSizeKO;
                        this.fontSizeMax = this.maxSizeKO;
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
                    this.enableAutoSizing = this.autoFontSizeTH;

                    if (this.enableAutoSizing)
                    {
                        this.fontSizeMin = this.minSizeTH;
                        this.fontSizeMax = this.maxSizeTH;
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
                    {
                        if (this.alignment == TextAlignmentOptions.MidlineLeft)
                            this.alignment = TextAlignmentOptions.MidlineRight;
                        else if (this.alignment == TextAlignmentOptions.BottomLeft)
                            this.alignment = TextAlignmentOptions.BottomRight;
                        else if (this.alignment == TextAlignmentOptions.TopLeft)
                            this.alignment = TextAlignmentOptions.TopRight;
                        else if (this.alignment == TextAlignmentOptions.Left)
                            this.alignment = TextAlignmentOptions.Right;
                    }

                    this.fontStyle = this.styleAR;
                    this.fontSize = this.fontSizeAR;
                    this.lineSpacing = this.lineSpaceAR;
                    this.enableAutoSizing = this.autoFontSizeAR;

                    if (this.enableAutoSizing)
                    {
                        this.fontSizeMin = this.minSizeAR;
                        this.fontSizeMax = this.maxSizeAR;
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
                    this.enableAutoSizing = this.autoFontSizeEN;

                    if (this.enableAutoSizing)
                    {
                        this.fontSizeMin = this.minSizeEN;
                        this.fontSizeMax = this.maxSizeEN;
                    }

                    break;
                }
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
        if (setPara1 && setPara2 && setPara3)
            text = GameEntry.Localization.GetString(languageId, para1, para2, para3);
        else if (setPara1 && setPara2)
            text = GameEntry.Localization.GetString(languageId, para1, para2);
        else if (setPara1)
            text = GameEntry.Localization.GetString(languageId, para1);
        else
            text = GameEntry.Localization.GetString(languageId);

        this.SetTextEx(text);
    }

    #region 公共接口

    public void SetDialogId(int dialogId)
    {
        this.SetText(GameEntry.Localization.GetString(dialogId));
    }

    public void SetDialogId(string dialogId)
    {
        this.SetText(GameEntry.Localization.GetString(dialogId));
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

    public void RefreshProperty()
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

    void SetKeyword(Material mat, string key, bool enable)
    {
        if (enable)
            mat.EnableKeyword(key);
        else
            mat.DisableKeyword(key);
    }


    void ReleaseMaterial()
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

    public void Refresh()
    {
        OnDisable();
        OnEnable();
    }

    protected override void OnDisable()
    {
        ReleaseMaterial();
    }

    #endregion

    #region 私有接口

    private void SetBaseText(string content)
    {
        string target = content;

        // 是否换行
        //if (!string.IsNullOrEmpty(content) && IsNonBreakingSpace())
        //    target = string.Format(nobr, content);

        //if (wordUpper)
        //    target = target.ToUpperInvariant();

        base.SetText(target);
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

#if UNITY_EDITOR

    protected override void OnValidate()
    {
        base.OnValidate();

        RefreshProperty();
    }

    public void AutoFillFont()
    {
        this.font = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/Main/Arts/Fonts/ArialMT_SDF.asset");
        this.fontEN = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/Main/Arts/Fonts/ArialMT_SDF.asset");
        this.fontCN = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/Main/Arts/Fonts/SourceHanSansSC-Regular_SDF.asset");
        this.fontJA = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/Main/Arts/Fonts/SourceHanSansSC-Regular_SDF.asset");
        this.fontKO = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/Main/Arts/Fonts/SourceHanSansSC-Regular_SDF.asset");
        this.fontTH = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/Main/Arts/Fonts/ArialMT_SDF.asset");
        this.fontAR = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/Main/Arts/Fonts/ArialMT_SDF.asset");
    }
#endif
}
