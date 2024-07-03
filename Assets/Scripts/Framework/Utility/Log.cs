using System;
using System.Text.RegularExpressions;
using UnityEngine;

/// <summary>
/// 日志类。
/// </summary>
public static class Log
{
    public static bool IsLoad()
    {
        return false;
    }

    public static string FilePathToLink(string file, string line)
    {
        if (file.IsNullOrEmpty())
        {
            return "";
        }

        if (line.IsNullOrEmpty())
        {
            string replacement = "<a href=\"%1\">%1</a>";
            return replacement.Replace("$1", file);
        }
        else
        {
            string replacement = "<a href=\"$1\" line=\"$2\">$1:$2</a>";
            return replacement.Replace("$1", file).Replace("$2", line);
        }
    }

    public static string FixFileLink(string input)
    {
        string result = input.Replace('\\', '/');
        string projectPath = Application.dataPath.Replace("Assets", "");
        result = result.Replace(projectPath, "");
        // Define the regular expression pattern
        string pattern = @"(?<!>)(Assets/[A-Za-z0-9_/]+?\.[\.A-z]+?):(\d+)";

        // Create the replacement string
        string replacement = "<a href=\"$1\" line=\"$2\">$1:$2</a>";

        // Perform the match and replace
        result = Regex.Replace(result, pattern, replacement);

        return result;
    }

    /// <summary>
    /// 记录调试级别日志，仅在带有 GAME_DEBUG 预编译选项时产生。
    /// </summary>
    [System.Diagnostics.Conditional("GAME_DEBUG")]
    public static void Debug(object message)
    {
        UnityEngine.Debug.Log(message);
    }

    /// <summary>
    /// 记录调试级别日志，仅在带有 GAME_DEBUG 预编译选项时产生。
    /// </summary>
    [System.Diagnostics.Conditional("GAME_DEBUG")]
    public static void Debug(object message, UnityEngine.Object context)
    {
        UnityEngine.Debug.Log(message, context);
    }

    /// <summary>
    /// 记录调试级别日志，仅在带有 GAME_DEBUG 预编译选项时产生。
    /// </summary>
    [System.Diagnostics.Conditional("GAME_DEBUG")]
    public static void DebugFormat(string format, params object[] args)
    {
        UnityEngine.Debug.LogFormat(format, args);
    }

    /// <summary>
    /// 记录调试级别日志，仅在带有 GAME_DEBUG 预编译选项时产生。
    /// </summary>
    [System.Diagnostics.Conditional("GAME_DEBUG")]
    public static void DebugFormat(UnityEngine.Object context, string format, params object[] args)
    {
        UnityEngine.Debug.LogFormat(context, format, args);
    }

    /// <summary>
    /// 打印信息级别日志，用于记录程序正常运行日志信息。
    /// </summary>
    public static void Info(object message)
    {
        UnityEngine.Debug.Log(message);
    }

    /// <summary>
    /// 打印信息级别日志，用于记录程序正常运行日志信息。
    /// </summary>
    public static void Info(object message, UnityEngine.Object context)
    {
        UnityEngine.Debug.Log(message, context);
    }

    /// <summary>
    /// 打印信息级别日志，用于记录程序正常运行日志信息。
    /// </summary>
    public static void InfoFormat(string format, params object[] args)
    {
        UnityEngine.Debug.LogFormat(format, args);
    }

    /// <summary>
    /// 打印信息级别日志，用于记录程序正常运行日志信息。
    /// </summary>
    public static void InfoFormat(UnityEngine.Object context, string format, params object[] args)
    {
        UnityEngine.Debug.LogFormat(context, format, args);
    }

    /// <summary>
    /// 打印警告级别日志，建议在发生局部功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
    /// </summary>
    public static void Warning(object message)
    {
        UnityEngine.Debug.LogWarning(message);
    }

    /// <summary>
    /// 打印警告级别日志，建议在发生局部功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
    /// </summary>
    public static void Warning(object message, UnityEngine.Object context)
    {
        UnityEngine.Debug.LogWarning(message, context);
    }

    /// <summary>
    /// 打印警告级别日志，建议在发生局部功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
    /// </summary>
    public static void WarningFormat(string format, params object[] args)
    {
        UnityEngine.Debug.LogWarningFormat(format, args);
    }

    /// <summary>
    /// 打印警告级别日志，建议在发生局部功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
    /// </summary>
    public static void WarningFormat(UnityEngine.Object context, string format, params object[] args)
    {
        UnityEngine.Debug.LogWarningFormat(context, format, args);
    }

    /// <summary>
    /// 打印错误级别日志，建议在发生功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
    /// </summary>
    /// <param name="message">日志内容。</param>
    public static void Error(object message)
    {
        string e = FixFileLink(message.ToString());
        UnityEngine.Debug.LogError(e);
    }

    /// <summary>
    /// 打印错误级别日志，建议在发生功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
    /// </summary>
    /// <param name="message">日志内容。</param>
    public static void Error(object message, UnityEngine.Object context)
    {
        string e = FixFileLink(message.ToString());
        UnityEngine.Debug.LogError(e, context);
    }

    /// <summary>
    /// 打印错误级别日志，建议在发生功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
    /// </summary>
    public static void ErrorFormat(string format, params object[] args)
    {

        string e = FixFileLink(String.Format(format, args));
        UnityEngine.Debug.LogError(e);
    }

    /// <summary>
    /// 打印错误级别日志，建议在发生功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
    /// </summary>
    public static void ErrorFormat(UnityEngine.Object context, string format, params object[] args)
    {
        string e = FixFileLink(String.Format(format, args));
        UnityEngine.Debug.LogError(e);
    }

    /// <summary>
    /// 打印异常级别日志，建议在发生严重错误，可能导致游戏崩溃或异常时使用，此时应尝试重启进程或重建游戏。
    /// </summary>
    public static void Exception(string exception)
    {
        Exception(new System.Exception(exception));
    }

    /// <summary>
    /// 打印异常级别日志，建议在发生严重错误，可能导致游戏崩溃或异常时使用，此时应尝试重启进程或重建游戏。
    /// </summary>
    public static void Exception(System.Exception exception)
    {
        UnityEngine.Debug.LogException(exception);
    }

    /// <summary>
    /// 打印异常级别日志，建议在发生严重错误，可能导致游戏崩溃或异常时使用，此时应尝试重启进程或重建游戏。
    /// </summary>
    public static void Exception(System.Exception exception, UnityEngine.Object context)
    {
        UnityEngine.Debug.LogException(exception, context);
    }
}
