using System;
using System.IO;

namespace Card
{
    /// <summary>
    /// 日志
    /// </summary>
    public static class logger
    {
        public static StreamWriter logfile;
        public static void Init()
        {
            logfile = new StreamWriter("C:\\mlog.txt", true, System.Text.UnicodeEncoding.Unicode);
        }
        public static void Terminate()
        {
            logfile.Close();
        }
        /// <summary>
        /// LOG的记录（长间隔）
        /// </summary>
        /// <param name="Info"></param>
        public static void TextLog(String Info)
        {
            logfile.WriteLine(DateTime.Now.ToString() + ":" + Info);
            logfile.Flush();
        }
    }
}
