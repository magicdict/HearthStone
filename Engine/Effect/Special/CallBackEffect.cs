using Engine.Action;
using Engine.Utility;
using System.Collections.Generic;

namespace Engine.Effect
{
    /// <summary>
    /// 召回随从
    /// </summary>
    public class CallBackEffect
    {
        public static List<string> RunEffect(ActionStatus game, string PosField)
        {
            List<string> Result = new List<string>();
            var Pos = CardUtility.指定位置结构体.FromString(PosField);
            if (Pos.本方对方标识)
            {
                if (game.AllRole.MyPrivateInfo.handCards.Count != SystemManager.MaxHandCardCount) { 
                    game.AllRole.MyPrivateInfo.handCards.Add(
                        CardUtility.GetCardInfoBySN(game.AllRole.MyPublicInfo.BattleField.BattleMinions[Pos.位置 - 1].序列号)
                    );
                }
                game.AllRole.MyPublicInfo.BattleField.BattleMinions[Pos.位置 - 1] = null;
            }
            else
            {
                if (game.AllRole.YourPrivateInfo.handCards.Count != SystemManager.MaxHandCardCount)
                {
                    game.AllRole.YourPrivateInfo.handCards.Add(
                        CardUtility.GetCardInfoBySN(game.AllRole.YourPublicInfo.BattleField.BattleMinions[Pos.位置 - 1].序列号)
                    );
                }
                game.AllRole.YourPublicInfo.BattleField.BattleMinions[Pos.位置 - 1] = null;
            }
            return Result;
        }
    }
}
