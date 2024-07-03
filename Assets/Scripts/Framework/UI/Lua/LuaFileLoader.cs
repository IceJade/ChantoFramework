using Framework;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LuaFileLoader : Singleton<LuaFileLoader>
{
    private AssetBundle m_luaBundle;

    private Dictionary<string, byte[]> luaFiles = new Dictionary<string, byte[]>();

    private void InitBundle()
    {
        if (m_luaBundle == null)
        {
            string innerBundlePath = $"{AssetBundles.Utility.GetStreamingAssetsDirectory()}/Assets/Main/Lua/luascript.bundle";
            string sanboxBundlePath = Application.persistentDataPath + "/assets/shelter/luascript.bundle";
            // 开启吃饭时. 先沙盒,后包体
            //if (ChiConfig.OpenChiFanState && ChiConfig.OpenChiFan) 
            //{
            //    if (File.Exists(sanboxBundlePath)) 
            //    {
            //        m_luaBundle = AssetBundle.LoadFromFile(sanboxBundlePath);
            //        if (m_luaBundle != null) return;
            //    } 
            //} 
            m_luaBundle = AssetBundle.LoadFromFile(innerBundlePath);
        }
    }

    public void LoadLuaScript(string luaName, out byte[] contents)
    {
        InitBundle();

        TextAsset tmp = m_luaBundle.LoadAsset<TextAsset>(luaName);
        if (tmp == null)
            contents = null;

        contents = m_luaBundle.LoadAsset<TextAsset>(luaName)?.bytes;
    }

    public byte[] GetLuaString(string luaName)
    {
        if (Application.isEditor)
        {
            if (!luaFiles.ContainsKey(luaName))
            {
                byte[] luaBytes = System.Text.Encoding.UTF8.GetBytes(File.ReadAllText(luaName));
                luaFiles.Add(luaName, luaBytes);
            }

            return luaFiles[luaName];
        }
        else
        {
            InitBundle();

            TextAsset textAsset = m_luaBundle.LoadAsset<TextAsset>(luaName);
            if (null == textAsset)
            {
                textAsset = m_luaBundle.LoadAsset<TextAsset>(Path.GetFileNameWithoutExtension(luaName));
                if (null == textAsset)
                {
                    Log.Error("have on {0} assetbundle file, please check...");
                    return null;
                }
            }

            return textAsset.bytes;
        }
    }
}
