using System;
using System.Text;

namespace Engine.Client
{
    public class ClientInfo
    {
        /// <summary>
        /// 游戏玩家名称
        /// </summary>
        public String PlayerNickName = "NickName";
        /// <summary>
        /// 是否主机
        /// </summary>
        public Boolean IsHost;
        /// <summary>
        /// 是否为先手
        /// </summary>
        public Boolean IsFirst;
        /// <summary>
        /// 本方回合
        /// </summary>
        public Boolean IsMyTurn;
        /// <summary>
        /// 游戏信息
        /// </summary>
        /// <returns></returns>
        public string GetClientInfo()
        {
            StringBuilder Status = new StringBuilder();
            Status.AppendLine("==============");
            Status.AppendLine("ClientInfo：");
            Status.AppendLine("PlayerNickName：" + PlayerNickName);
            Status.AppendLine("IsHost：" + IsHost);
            Status.AppendLine("IsFirst：" + IsFirst);
            Status.AppendLine("==============");
            return Status.ToString();
        }
        /// <summary>
        /// 游戏控制器
        /// </summary>
        public GameManager game = new GameManager();
        /// <summary>
        /// 初始化
        /// </summary>
        public void Init(){
            game.游戏类型 = Utility.SystemManager.GameType.客户端服务器版;
        }
    }
}
