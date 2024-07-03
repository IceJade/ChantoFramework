using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

public static class GameApplication
{
    /// <summary>
    /// 是否是debug模式
    /// </summary>
#if GAME_NET_PRIVATE
    public static bool isUseOnline = false;
    public static string onlineUid = "389462107001013";
    public static string onlineUuid = "867db81bab8cecacbb4062917142f01382e27d1dFB881592186862100";
    public static int onlineZone = 2910;
    public static string onlineDeviceId = "emula_b06b8b3c9e844d49a9b89c5a359aee9bFB88";
#endif
    public static bool isDebug
    {
        get
        {
#if GAME_DEBUG
            return true;
#else
            return false;
#endif
        }
    }

    /// <summary>
    /// 是否是内网包
    /// </summary>
    public static bool isNetPrivate
    {
        get
        {
#if GAME_NET_PRIVATE
            if (isUseOnline) return false;
            return true;
#else
            return false;
#endif
        }
    }

    /// <summary>
    /// 安装包标识符
    /// </summary>
    public static string packageName => Application.identifier;
    
    //发送给服务器的包名，由于服务器现在没有内外网包名的判断，客户端需要区分包名，这里特殊处理下
    public static string serverPackageName => "com.im30.lsu3d.gp";

    private static string s_PackageSign;

    public static string packageSign
    {
        get
        {
            if (s_PackageSign == null)
            {
                SHA1 sha1 = new SHA1CryptoServiceProvider();
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(packageName));
                s_PackageSign = BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant();
            }

            return s_PackageSign;
        }
    }

    /// <summary>
    /// 应用版本号
    /// </summary>
    public static string version => Application.version;

    //public static string version = "2.0.4";

    private static string s_BuildVersion;

    /// <summary>
    /// 应用构建版本号
    /// </summary>
    public static string buildVersion
    {
        get
        {
            //return "200";
            if (s_BuildVersion != null)
            {
                return s_BuildVersion;
            }

#if UNITY_EDITOR
#if UNITY_ANDROID
            s_BuildVersion = PlayerSettings.Android.bundleVersionCode.ToString(CultureInfo.InvariantCulture);
#elif UNITY_IOS
            s_BuildVersion = PlayerSettings.iOS.buildNumber;
#else
            s_BuildVersion = "";
#endif
#elif UNITY_ANDROID
            using (AndroidJavaClass contextCls = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject context = contextCls.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    using (AndroidJavaObject packageMngr = context.Call<AndroidJavaObject>("getPackageManager"))
                    {
                        using (AndroidJavaObject packageInfo =
packageMngr.Call<AndroidJavaObject>("getPackageInfo", Application.identifier, 0))
                        {
                            s_BuildVersion =
packageInfo.Get<int>("versionCode").ToString(CultureInfo.InvariantCulture);
                        }
                    }
                }
            }
#elif UNITY_IOS
            // todo : 获取ios的构建版本号
#else
            s_BuildVersion = "";
#endif
            return s_BuildVersion;
        }
    }

    public static bool isEditor => Application.isEditor;

    public static bool isAndroid
    {
        get
        {
#if UNITY_ANDROID
            return true;
#else
            return false;
#endif
        }
    }
    
    public static bool isIOS    
    {
        get
        {
#if UNITY_IOS || UNITY_IPHONE
            return true;
#else
            return false;
#endif
        }
    }

    public static bool useFacebook
    {
        get
        {
#if USE_FACEBOOK
            return true;
#else
            return false;
#endif
        }
    }
    
    /// <summary>
    /// 获取平台名称
    /// </summary>
    public static string platformName
    {
        get
        {
            if (isAndroid)
            {
                return "android";
            }
            else if (isIOS)
            {
                return "ios";
            }
            else
            {
                return "unknown";
            }
        }
    }

    #region 渠道标识

    public const string ChannelUnknown = "unknown";
    public const string ChannelGoogle = "market_global";
    public const string ChannelAppStore = "AppStore";
    
    /// <summary>
    /// 获取渠道名称
    /// Google渠道：market_global
    /// </summary>
    public static string channelName
    {
        get
        {
            if (isAndroid)
            {
#if CHANNEL_GOOGLE
                return ChannelGoogle;
#else
                // TODO : 安卓平台其他渠道拓展
                return ChannelGoogle;
#endif
            }
            else if (isIOS)
            {
                return ChannelAppStore;
            }
            else
            {
                return ChannelUnknown;
            }
        }
    }
    #endregion

    public static string currentRegionName => RegionInfo.CurrentRegion.Name;

    public static bool isChina => currentRegionName is "CN" or "cn";
    
    //获取虚拟键盘高度
    public static int keyboardHeight
    {
        get
        {
#if UNITY_EDITOR
            return 0;
#elif UNITY_ANDROID
            return AndroidUtils.GetKeyboardHeight();
#elif UNITY_IOS
            return (int)TouchScreenKeyboard.area.height;
#else
            return 0;
#endif
        }
    }
}