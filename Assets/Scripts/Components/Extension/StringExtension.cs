using System.Security.Cryptography;
using System;
using Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

public static class StringExtension
{
    public static bool IsNullOrEmpty(this string str)
    {
        return string.IsNullOrEmpty(str);
    }

    public static bool IsNotNullAndEmpty(this string str)
    {
        return !string.IsNullOrEmpty(str);
    }
    
    public static bool IsInt(this string value)
    {
        return Regex.IsMatch(value, @"^[+-]?\d*$");
    }

    public static string FixNewLine(this string str)
    {
        return str.Replace("\\n", "\n");
    }

    public static string GetMD5(string msg)
    {
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] data = System.Text.Encoding.UTF8.GetBytes(msg);
        byte[] md5Data = md5.ComputeHash(data, 0, data.Length);
        md5.Clear();

        string destString = "";
        for (int i = 0; i < md5Data.Length; i++)
        {
            destString += System.Convert.ToString(md5Data[i], 16).PadLeft(2, '0');
        }
        destString = destString.PadLeft(32, '0');
        return destString;
    }

    public static string GetFileNameNoExtension(this string path, char separator = '.')
    {
        if (path.IsNullOrEmpty())
        {
            return "";
        }
        return path.Substring(0, path.LastIndexOf(separator));
    }
    public static string GetFileName(this string path, char separator = '/')
    {
        if (path.IsNullOrEmpty())
        {
            return "";
        }
        return path.Substring(path.LastIndexOf(separator) + 1);
    }
    public static string GetExtensionName(this string path)
    {
        return System.IO.Path.GetExtension(path);
    }

    public static string GetDirectoryName(this string fileName)
    {
        if (fileName.IsNullOrEmpty() || !fileName.Contains("/"))
        {
            return "";
        }
        return fileName.Substring(0, fileName.LastIndexOf("/"));
    }
    //千位分隔符
    public static string GetFormattedSeperatorNum(int value)
    {
        return value.ToString("N0");
    }
    public static string GetFormattedSeperatorNum(long value)
    {
        return value.ToString("N0");
    }
    public static string GetFormattedSeperatorNum(string value)
    {
        //小数点和百分值不做分隔
        if (value.Contains(".") || value.Contains("%"))
        {
            return value;
        }
        return value.ToLong().ToString("N0");
    }

    public static string GetFormattedInt(int value)
    {
        //string unit = "";
        var kVal = (float)value / 1000f;
        var mVal = (float)value / 1000000f;
        if (mVal >= 1f)
        {
            return mVal.ToString("F1") + "M";
        }
        if (kVal >= 1f)
        {
            return kVal.ToString("F1") + "K";
        }
        return value.ToString("");
    }
     
    public static string GetFormattedLong(long value)
    {
        //string unit = "";
        var kVal = (float)value / 1000f;
        var mVal = (float)value / 1000000f;
        if (mVal >= 1f)
        {
            return mVal.ToString("F1") + "M";
        }
        if (kVal >= 1f)
        {
            return kVal.ToString("F1") + "K";
        }
        return value.ToString("");
    }

    public static string GetFormattedStr(double value)
    {
        var kVal = value / 1000f;
        var mVal = value / 1000000f;
        var gVal = value / 1000000000f;
        if(gVal >= 1f)
        {
            return gVal.ToString("F1") + "G";
        }
        if (mVal >= 1f)
        {
            return mVal.ToString("F1") + "M";
        }
        if (kVal >= 1f)
        {
            return kVal.ToString("F1") + "K";
        }
        return value.ToString("");
    }

    /// <summary>
    /// 生成随机字符串
    /// </summary>
    /// <param name="_charCount">生成的字符数</param>
    /// <returns></returns>
    private static int rep = 0;
    public static string GenerateRandomStr(int _codeCount)
    {
        string str = string.Empty;
        long num2 = DateTime.Now.Ticks + rep;
        rep++;
        Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> rep)));
        for (int i = 0; i < _codeCount; i++)
        {
            int num = random.Next();
            str = str + ((char)(0x30 + ((ushort)(num % 10)))).ToString();
        }
        return str;
    }

    public static string FormatDBString(string str)
    {
        return "'" + str + "'";
    }

    public static string FormatPassedTime(double passed)
    {
        string strRet = "";
        int secondsPerMinute = 60;
        int secondsPerHour = 3600;
        int secondsPerDay = secondsPerHour * 24;

        int tmp = 0;
        if (passed >= secondsPerDay)
        {
            tmp = (int)(passed / secondsPerDay);
            strRet += tmp.ToString();
            strRet += GameEntry.Localization.GetString("105592"); //day
        }
        else if (passed >= secondsPerHour)
        {
            tmp = (int)(passed / secondsPerHour);
            strRet += tmp.ToString();
            strRet += GameEntry.Localization.GetString("105591"); //hour
        }
        else if (passed >= secondsPerMinute)
        {
            tmp = (int)(passed / secondsPerMinute);
            strRet += tmp.ToString();
            strRet += GameEntry.Localization.GetString("105590"); //minute
        }
        else
        {
            strRet += "1";
            strRet += GameEntry.Localization.GetString("105590"); //minute
        }

        strRet += " ";
        strRet += GameEntry.Localization.GetString("105593"); // ago

        return strRet;
    }

    public static void SplitString(string str, char key, ref List<string> list)
    {
        list = str.Split(key).ToList();
    }

    public static string[] SplitString(string str, char key)
    {
        return str.Split(key);
    }

    public static string FormatStringMaxLength(string str, int maxLen = 18)
    {
        int strLen = str.Length;
        if (strLen > maxLen)
        {
            return str.Substring(0, maxLen);
        }
        return str;
    }

    /// <summary>
    /// string to section 逗号分段
    /// </summary>
    /// <returns>The s.</returns>
    /// <param name="rawStr">Raw string.</param>
    public static string S2Sec(string rawStr)
    {
        if (rawStr == null || rawStr == "")
            return "";

        if (rawStr.Length <= 3)
            return rawStr;

        string retStr = rawStr;
        int curPos = rawStr.Length;
        while (curPos > 3)
        {
            curPos -= 3;
            retStr = retStr.Insert(curPos, ",");
        }

        return retStr;
    }

    /// <summary>
    /// string to section 逗号分段
    /// </summary>
    /// <returns>The s.</returns>
    /// <param name="rawNum">Raw int.</param>
    public static string S2Sec(int rawNum)
    {
        return S2Sec(rawNum.ToString());
    }

    public static int TryParseInt(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return 0;
        }
        else
        {
            return int.Parse(str);
        }
    }

    public static byte[] GetBytesUTF8(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return null;
        }

        return Encoding.UTF8.GetBytes(value);
    }

    public static string GetFormattedPercentStr(float num, string format = "p0")
    {
        return num.ToString(format);
    }

    public static string GenerateMD5(string input)
    {
        // 将输入字符串转换为字节数组
        byte[] inputBytes = Encoding.UTF8.GetBytes(input);

        // 创建一个 MD5 实例
        using (MD5 md5 = MD5.Create())
        {
            // 计算输入数据的 MD5 哈希值
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // 将字节数组转换为十六进制字符串
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2")); // 使用 "x2" 格式将每个字节转换为十六进制表示的字符串
            }
            return sb.ToString();
        }
    }
}