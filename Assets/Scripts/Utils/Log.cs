using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
public class Log
{
    public enum InfoType
    {
        None,
        Net,
        Load,
        Lua
    }
    static LogSetting LogSetting = null;
    static string StartupTime = string.Empty;
    static Thread ms_ThreadWriteFile;
    static Dictionary<string, Queue<string>> ms_LogQueue = new Dictionary<string, Queue<string>>();
    static string LogPath = string.Empty;

    static public void StartUp(LogSetting logSetting)
    {
        Application.logMessageReceived+=UnityLogCallback;
        StartupTime = System.DateTime.Now.ToString("MM-dd_HH-mm-ss");
        LogSetting = logSetting;
        SetLogPath(LogSetting.LogDir);
        ms_ThreadWriteFile = new Thread(WriteFileThread);
        ms_ThreadWriteFile.Start();
    }
    static public void Info(string sFormat, params object[] args)
    {
        //string sLog = StringUtils.Format("{0}\t{1}\t{2}", LogDataTime(), "[Info]", StringUtils.Format(sFormat, args));
        Debug.Log(StringUtils.Format(sFormat, args));
    }
    static public void Warning(string sFormat, params object[] args)
    {
        //string sLog = StringUtils.Format("{0}\t{1}\t{2}", LogDataTime(), "[Warning]", StringUtils.Format(sFormat, args));
        Debug.LogWarning(StringUtils.Format(sFormat, args));

    }
    static public void Error(string sFormat, params object[] args)
    {
        //string sLog = StringUtils.Format("{0}\t{1}\t{2}", LogDataTime(), "[Error]", StringUtils.Format(sFormat, args));
        Debug.LogError(StringUtils.Format(sFormat, args));
    }

    static void UnityLogCallback(string condition, string stackTrace, LogType type)
    {
        if (LogSetting.WriteFile)
        {
            WriteFile(StringUtils.Format("Log_{0}.txt", StartupTime), StringUtils.Format("[{0}]\t{1}\t{2}", type.ToString(), LogDataTime(), condition));
        }
    }
    static string LogDataTime()
    {
        return System.DateTime.Now.ToString("MM-dd HH:mm:ss.fff");
    }

    static void WriteFile(string sFile,string sContent)
    {
        lock(ms_LogQueue)
        {
            Queue<string> queue;
            if (!ms_LogQueue.TryGetValue(sFile, out queue))
            {
                queue = new Queue<string>();
                ms_LogQueue.Add(sFile, queue);
            }
            queue.Enqueue(sContent);
        }
    }

    static void WriteFileThread()
    {
        ExecuteWriteFile();
        Thread.Sleep(1000);
    }

    static void ExecuteWriteFile()
    {
        lock (ms_LogQueue)
        {
            string sLog;
            var itor = ms_LogQueue.GetEnumerator();
            while (itor.MoveNext())
            {
                var oper = itor.Current;
                var queue = oper.Value;
                while (queue.Count > 0)
                {
                    sLog = queue.Dequeue();
                    IOUtils.WriteFile(StringUtils.PathCombine(LogPath, oper.Key), sLog,true);
                }
            }
        }
    }

    static public void ShutDown()
    {
        Application.logMessageReceived -= UnityLogCallback;
        ExecuteWriteFile();
        ms_LogQueue.Clear();
        
    }

    static public void SetLogPath(string sDir)
    {
        LogPath = StringUtils.PathCombine(Application.persistentDataPath, sDir);
#if UNITY_EDITOR
        LogPath = StringUtils.PathCombine(UnityUtils.UnityProjectPath(), sDir);
#endif
    }
}
