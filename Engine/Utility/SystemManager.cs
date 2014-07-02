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
        /// <summary>
        /// 游戏模式枚举
        /// </summary>
        public enum GameMode
        {
            标准,
            塔防
        }
        /// <summary>
        /// 最大生命值
        /// </summary>
        public static int MaxHealthPoint = 30;
        /// <summary>
        /// 最大手牌数
        /// </summary>
        public const int MaxHandCardCount = 10;
        /// <summary>
        /// 最多7个随从的位置
        /// </summary>
        public const int MaxMinionCount = 7;
        /// <summary>
        /// 最大水晶数
        /// </summary>
        public const int MaxCrystalPoint = 10;
        /// <summary>
        /// 游戏类型
        /// </summary>
        public static SystemManager.GameType 游戏类型 = SystemManager.GameType.HTML版;
        /// <summary>
        /// 游戏模式
        /// </summary>
        public static SystemManager.GameMode 游戏模式 = SystemManager.GameMode.标准;
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
