using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

namespace Framework
{
    public class Localization : GameBaseSingletonModule<Localization>
    {
        private static readonly string[] ColumnSplit = new string[] { "=" };
        private const int ColumnCount = 2;

        private Language m_Language;

        private readonly Dictionary<string, string> m_Dictionary = new();
        private readonly Dictionary<string, TMP_SpriteAsset> m_EmojiDic = new();

        public Localization()
        {
            m_Language = SystemLanguage;
        }

        public Localization(Language userLanguage, object data = null)
        {
            m_Language = SystemLanguage;

            if (userLanguage != Language.Unspecified)
            {
                m_Language = userLanguage;
            }
        }

        public void Preload()
        {
            // 默认用英文多语言文件
            string languageName = "en";
            //string languageName = "ChineseSimplified";
            if (IsSuported())
            {
                // languageName = Language.ToString();
                languageName = GetLanguageName();
            }

            Load(languageName);
            LoadTMPSettings();
        }

        private void LoadTMPSettings()
        {
            try
            {
                //var path = $"Assets/Art/Localization_bak/ChineseSimplified/Fonts/TMP_Settings2.asset";
                //var op = Addressables.LoadAssetAsync<TMP_Settings>(path);
                //var taTMPSettings = op.WaitForCompletion();
                //if (taTMPSettings == null)
                //{
                //    Debug.LogErrorFormat("load TMP_Settings2 assets failed");
                //    return;
                //}

                //TMP_Settings.instance = taTMPSettings;
            }
            catch (Exception e)
            {
                Debug.LogErrorFormat(e.ToString());
            }
        }

        private void Load(string language)
        {
            if (string.IsNullOrEmpty(language))
            {
                Debug.LogError("language is null or empty, set as default en");
                language = "en";
            }

            // var path = $"Assets/Main/DataTable/text_{language}.ini"; //$"Assets/Main/Localization/text_{language}.txt";
            // var op = Addressables.LoadAssetAsync<TextAsset>(path);
            // var ta = op.WaitForCompletion();
            // if (ta == null)
            // {
            //     Debug.LogErrorFormat("load [{0}] localization file failed", language);
            //     return;
            // }

            //var fileContent = UDatatableManager.LoadDatatable($"text_{language}.ini");
            //if (string.IsNullOrEmpty(fileContent))
            //{
            //    Debug.LogErrorFormat("load [{0}] localization file failed", language);
            //    return;
            //}
            //Parse(fileContent);
        }

        /// <summary>
        /// 解析字典。
        /// </summary>
        /// <param name="text">要解析的字典文本。</param>
        /// <returns>是否解析字典成功。</returns>
        public bool Parse(string text)
        {
            try
            {
                m_Dictionary.Clear();
                string[] rowTexts = Framework.Utility.Text.SplitToLines(text);
                for (int i = 0; i < rowTexts.Length; i++)
                {
                    if (rowTexts[i].Length <= 0 || rowTexts[i][0] == '#')
                    {
                        continue;
                    }

                    string[] splitLine = rowTexts[i].Split(ColumnSplit, 2, StringSplitOptions.None);
                    if (splitLine.Length != ColumnCount)
                    {
                        Log.WarningFormat("Can not parse dictionary '{0}'.", rowTexts[i]);
                        //return false;
                        continue;
                    }

                    string key = splitLine[0];
                    string value = splitLine[1];
                    if (string.IsNullOrEmpty(key))
                    {
                        //Log.Warning("Invalid Key at line:" + i);
                        continue;
                    }
                    if (!AddRawString(key, value))
                    {
                        Log.WarningFormat("Can not add raw string with key '{0}' which may be invalid or duplicate.", key);
                        //return false;
                        continue;
                    }
                }

                return true;
            }
            catch (Exception exception)
            {
                Log.WarningFormat("Can not parse dictionary '{0}' with exception '{1}'.", text, string.Format("{0}\n{1}", exception.Message, exception.StackTrace));
                return false;
            }
        }

        /// <summary>
        /// 支持语言列表。
        /// </summary>
        public List<Language> SuportedLanguages = new List<Language>(){
            Language.English,
            Language.French,
            Language.German,
            Language.Russian,
            Language.Korean,
            Language.Thai,
            Language.Japanese,
            Language.PortuguesePortugal,
            Language.Spanish,
            Language.Turkish,
            Language.Indonesian,
            Language.ChineseTraditional,
            Language.ChineseSimplified,
            Language.Italian,
            // Language.Polish,
            // Language.Dutch,
            Language.Arabic,
        };

        /// <summary>
        /// 获取或设置本地化语言。
        /// </summary>
        public Language Language
        {
            get
            {
                return m_Language;
            }
            set
            {
                if (value == Language.Unspecified)
                {
                    throw new Exception("Language is invalid.");
                }

                m_Language = value;
            }
        }

        /// <summary>
        /// 获取系统语言。
        /// </summary>
        public static Language SystemLanguage
        {
            get
            {
                switch (Application.systemLanguage)
                {
                    case UnityEngine.SystemLanguage.Afrikaans: return Language.Afrikaans;
                    case UnityEngine.SystemLanguage.Arabic: return Language.Arabic;
                    case UnityEngine.SystemLanguage.Basque: return Language.Basque;
                    case UnityEngine.SystemLanguage.Belarusian: return Language.Belarusian;
                    case UnityEngine.SystemLanguage.Bulgarian: return Language.Bulgarian;
                    case UnityEngine.SystemLanguage.Catalan: return Language.Catalan;
                    case UnityEngine.SystemLanguage.Chinese: return Language.ChineseSimplified;
                    case UnityEngine.SystemLanguage.ChineseSimplified: return Language.ChineseSimplified;
                    case UnityEngine.SystemLanguage.ChineseTraditional: return Language.ChineseTraditional;
                    case UnityEngine.SystemLanguage.Czech: return Language.Czech;
                    case UnityEngine.SystemLanguage.Danish: return Language.Danish;
                    case UnityEngine.SystemLanguage.Dutch: return Language.Dutch;
                    case UnityEngine.SystemLanguage.English: return Language.English;
                    case UnityEngine.SystemLanguage.Estonian: return Language.Estonian;
                    case UnityEngine.SystemLanguage.Faroese: return Language.Faroese;
                    case UnityEngine.SystemLanguage.Finnish: return Language.Finnish;
                    case UnityEngine.SystemLanguage.French: return Language.French;
                    case UnityEngine.SystemLanguage.German: return Language.German;
                    case UnityEngine.SystemLanguage.Greek: return Language.Greek;
                    case UnityEngine.SystemLanguage.Hebrew: return Language.Hebrew;
                    case UnityEngine.SystemLanguage.Hungarian: return Language.Hungarian;
                    case UnityEngine.SystemLanguage.Icelandic: return Language.Icelandic;
                    case UnityEngine.SystemLanguage.Indonesian: return Language.Indonesian;
                    case UnityEngine.SystemLanguage.Italian: return Language.Italian;
                    case UnityEngine.SystemLanguage.Japanese: return Language.Japanese;
                    case UnityEngine.SystemLanguage.Korean: return Language.Korean;
                    case UnityEngine.SystemLanguage.Latvian: return Language.Latvian;
                    case UnityEngine.SystemLanguage.Lithuanian: return Language.Lithuanian;
                    case UnityEngine.SystemLanguage.Norwegian: return Language.Norwegian;
                    case UnityEngine.SystemLanguage.Polish: return Language.Polish;
                    case UnityEngine.SystemLanguage.Portuguese: return Language.PortuguesePortugal;
                    case UnityEngine.SystemLanguage.Romanian: return Language.Romanian;
                    case UnityEngine.SystemLanguage.Russian: return Language.Russian;
                    case UnityEngine.SystemLanguage.SerboCroatian: return Language.SerboCroatian;
                    case UnityEngine.SystemLanguage.Slovak: return Language.Slovak;
                    case UnityEngine.SystemLanguage.Slovenian: return Language.Slovenian;
                    case UnityEngine.SystemLanguage.Spanish: return Language.Spanish;
                    case UnityEngine.SystemLanguage.Swedish: return Language.Swedish;
                    case UnityEngine.SystemLanguage.Thai: return Language.Thai;
                    case UnityEngine.SystemLanguage.Turkish: return Language.Turkish;
                    case UnityEngine.SystemLanguage.Ukrainian: return Language.Ukrainian;
                    case UnityEngine.SystemLanguage.Unknown: return Language.Unspecified;
                    case UnityEngine.SystemLanguage.Vietnamese: return Language.Vietnamese;
                    default: return Language.Unspecified;
                }
            }
        }

        /// <summary>
        /// 当前语言是否支持
        /// </summary>
        /// <returns>是否支持。</returns>
        public bool IsSuported()
        {
            return SuportedLanguages.Contains(Language);
        }

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <param name="key">字典主键。</param>
        /// <param name="args">字典参数。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        public string GetString(string key, params object[] args)
        {
            if (string.IsNullOrEmpty(key))
            {
                // Log.Info("Key is invalid.");
#if GAME_DEBUG
                return string.Empty;//"Key is invalid"; //QA提个bug,先注释，有需求可以打开
#else
            return "";
#endif
            }

            if (!m_Dictionary.TryGetValue(key, out string value))
            {
#if GAME_DEBUG
                return string.Format("<NoKey>{0}", key);
#else
            Log.Error(string.Format("<NoKey>{0}", key));
            return "";
#endif
            }

            try
            {
                if (value.Contains("\\n"))
                {
                    value = value.Replace("\\n", "\n");
                }

                if (args != null && args.Length > 0)
                {
                    if (args[0] is object[] data)  //lua传过来的数据会在封装一层，所以需要在解析一下
                    {
                        return string.Format(value, data);
                    }
                    else
                    {
                        return string.Format(value, args);
                    }

                }
                else
                    return value;
            }
            catch (Exception exception)
            {
                string errorString = string.Format("<Error>{0},{1}", key, value);
                if (args != null)
                {
                    foreach (object arg in args)
                    {
                        errorString += "," + arg.ToString();
                    }
                }

                errorString += "," + exception.Message;
                return errorString;
            }
        }

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <param name="key">字典主键。</param>
        /// <param name="args">字典参数。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        public string GetString(int key, params object[] args)
        {
            return this.GetString(key.ToString(), args);
        }

        /// <summary>
        /// 是否存在字典。
        /// </summary>
        /// <param name="key">字典主键。</param>
        /// <returns>是否存在字典。</returns>
        public bool HasRawString(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                Debug.LogError("找不到key 返回空字符串 key:" + key);
                return false;
                //throw new Exception("Key is invalid.");总是抛异常逻辑没法继续跑
            }

            return m_Dictionary.ContainsKey(key);
        }

        /// <summary>
        /// 根据字典主键获取字典值。
        /// </summary>
        /// <param name="key">字典主键。</param>
        /// <returns>字典值。</returns>
        public string GetRawString(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new Exception("Key is invalid.");
            }

            string value = null;
            if (m_Dictionary.TryGetValue(key, out value))
            {
                return value;
            }

#if GAME_DEBUG
            return string.Format("<NoKey>{0}", key);
#else
        Log.Error(string.Format("<NoKey>{0}", key));
        return string.Empty;
#endif
        }

        /// <summary>
        /// 增加字典。
        /// </summary>
        /// <param name="key">字典主键。</param>
        /// <param name="value">字典内容。</param>
        /// <returns>是否增加字典成功。</returns>
        public bool AddRawString(string key, string value)
        {
            if (HasRawString(key))
            {
                return false;
            }

            m_Dictionary.Add(key, value ?? string.Empty);
            return true;
        }

        /// <summary>
        /// 移除字典。
        /// </summary>
        /// <param name="key">字典主键。</param>
        /// <returns>是否移除字典成功。</returns>
        public bool RemoveRawString(string key)
        {
            if (!HasRawString(key))
            {
                return false;
            }

            return m_Dictionary.Remove(key);
        }

        public string GetSystemLanguageName()
        {
            return GetLanguageName(SystemLanguage);
        }

        //获取语言名称
        public string GetLanguageName()
        {
            return GetLanguageName(Language);
        }

        public string GetLanguageName(Language lang)
        {
            switch (lang)
            {
                case Language.ChineseSimplified:
                    return "zh_CN";
                case Language.ChineseTraditional:
                    return "zh_TW";
                case Language.English:
                    return "en";
                case Language.German:
                    return "de";
                case Language.French:
                    return "fr";
                case Language.Russian:
                    return "ru";
                case Language.Spanish:
                    return "es";
                case Language.Japanese:
                    return "ja";
                case Language.Korean:
                    return "ko";
                case Language.Thai:
                    return "th";
                case Language.Turkish:
                    return "tr";
                case Language.PortuguesePortugal:
                    return "pt";
                case Language.Italian:
                    return "it";
                case Language.Vietnamese:
                    return "vn";
                case Language.Polish:
                    return "pl";
                case Language.Indonesian:
                    return "id";
                case Language.Arabic:
                    return "ar";
                case Language.Unspecified:
                default:
                    return "en";
            }
        }

        public string GetGuideSoundLangName()
        {
            switch (Language)
            {
                case Language.ChineseSimplified:
                case Language.ChineseTraditional:
                    return "cn";
                case Language.German:
                    return "de";
                case Language.Japanese:
                    return "ja";
                case Language.Korean:
                    return "ko";
                default:
                    return "en";
            }
        }

        public string GetDefaultFontByLanguage(Language language)
        {
            switch (language)
            {
                case Language.ChineseSimplified:
                case Language.ChineseTraditional:
                    return "MSYH_Dynamic";
                case Language.Japanese:
                    return "yzgothic_Dynamic";
                case Language.Korean:
                    return "SourceHanSansKR-Bold_Dynamic";
                case Language.Thai:
                    return "tahoma_Dynamic";
                default:
                    return "ARIAL_Dynamic";
            }
        }

        public void Reset()
        {
            m_EmojiDic.Clear();
        }

        public static string ReplaceRichText(string originalStr)
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
    }
}