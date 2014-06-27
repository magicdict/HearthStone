using System;
using Newtonsoft.Json;

namespace Engine.Client
{
    /// <summary>
    /// 用来向Browser发送最新的战场情况
    /// </summary>
    public static class HtmlBattle
    {
        public class Minion
        {
            /// <summary>
            /// 攻击力
            /// </summary>
            public int 攻击力;
            /// <summary>
            /// 生命力
            /// </summary>
            public int 生命力;
            /// <summary>
            /// 状态列表
            /// </summary>
            public String 状态列表;
            /// <summary>
            /// InitByMinion
            /// </summary>
            /// <param name="minion"></param>
            public void InitByMinion(Card.MinionCard minion)
            {
                攻击力 = minion.实际攻击值;
                生命力 = minion.生命值;
                状态列表 = minion.状态;
            }
        }
        public static String GenerateBattleInfo
        {

        }
    }
}
