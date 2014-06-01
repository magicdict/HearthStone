using System;
using System.IO;

namespace Card
{
    /// <summary>
    /// 日志
    /// </summary>
    public static class logger
    {
        /// <summary>
        /// LOG的记录（长间隔）
        /// </summary>
        /// <param name="Info"></param>
        public static void TextLog(String Info)
        {
            StreamWriter logfile = new StreamWriter("C:\\mlog.txt", true, System.Text.UnicodeEncoding.Unicode);
            logfile.WriteLine(DateTime.Now.ToString() + ":" + Info);
            logfile.Flush();
            logfile.Close();
        }
    }
}
