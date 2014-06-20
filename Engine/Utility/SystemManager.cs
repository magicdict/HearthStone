using System;
using System.IO;

namespace Engine.Utility
{
    /// <summary>
    /// 日志
    /// </summary>
    public static class SystemManager
    {
        /// <summary>
        /// 游戏类型枚举
        /// </summary>
        public enum GameType
        {
            单机版,
            客户端服务器版,
            HTML版
        }
        public static GameType CurrentGameType = GameType.HTML版;
        /// <summary>
        /// 日志
        /// </summary>
        public static StreamWriter logfile;
        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            logfile = new StreamWriter("C:\\mlog.txt", true, System.Text.UnicodeEncoding.Unicode);
        }
        /// <summary>
        /// 终结化
        /// </summary>
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
