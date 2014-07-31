using Engine.Action;
using Engine.Utility;
using System.Collections.Generic;
using Engine.Card;
using Engine.Client;
using System;

namespace Engine.Effect
{
    /// <summary>
    /// 召回随从
    /// </summary>
    public class CallBackEffect : IAtomicEffect
    {
        public string DealHero(ActionStatus game, PublicInfo PlayInfo)
        {
            throw new NotImplementedException();
        }
        public string DealMinion(ActionStatus game, MinionCard Minion)
        {
            var Pos = Minion.战场位置;
            if (Pos.本方对方标识)
            {
                if (game.AllRole.MyPrivateInfo.handCards.Count != SystemManager.MaxHandCardCount)
                {
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
            return String.Empty;
        }

        public void GetField(List<string> InfoArray)
        {
            //throw new NotImplementedException();
        }

        public void ReRunEffect(ActionStatus game, string[] actField)
        {
            throw new NotImplementedException();
        }
    }
}
