using System;
using System.Web;
using System.Diagnostics;
using System.IO;
using System.Threading;

/// <summary>
/// Summary description for Logger
/// </summary>
public class Logger
{
    public static string LogPath = @"C:\LogFiles\";

    public Logger()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static void Log(string str)
    {
        try
        {
            SaveToDisk(str);
        }
        catch
        {
            // Do nothing.
        }
    }

    /// <summary>
    /// Log an exception.
    /// </summary>
    /// <param name="ex"></param>
    public static void Log(Exception ex)
    {
        try
        {
            SaveToDisk(ex.Message + ": " + ex.ToString());
        }
        catch
        {
            // Do nothing.
        }
    }

    /// <summary>
    /// Log a string.
    /// </summary>
    /// <param name="str"></param>
    private static void SaveToDisk(string str)
    {
        StackTrace st = new StackTrace(0, true);
        StackFrame sf = st.GetFrame(2);
        string filePath = "";
        string fileDirectory;
        string fileName = "";
        try
        {
            fileName = string.Format("[{0}]_{1}",
                sf.GetFileName().ToLower().Replace(HttpContext.Current.Request.PhysicalApplicationPath.ToLower(), "").Replace(":", "：").Replace("\\", " - "),
                DateTime.Now.ToString("yyyyMMdd"));
        }
        catch
        {
            fileName = "[unknown]" + "_" + DateTime.Now.ToString("yyyyMMdd");
        }
        string fileExt = ".log";
        string mutexId = "";

        str = "[" + DateTime.Now.ToString("s") + "]\t" + str;

        fileDirectory = LogPath + DateTime.Now.ToString("yyyy-MM-dd") + "\\";
        if (!Directory.Exists(fileDirectory))
        {
            Directory.CreateDirectory(fileDirectory);
        }

        filePath = fileDirectory + fileName + fileExt;
        mutexId = filePath.Substring(3).Replace("\\", " - ");

        using (Mutex mutex = new Mutex(false, mutexId))
        {
            try
            {
                mutex.WaitOne();

                using (StreamWriter sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine(str);
                }
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }
    }
}
