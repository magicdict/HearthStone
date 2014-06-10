using System;
using System.Collections.Generic;

namespace Card.Effect
{
    public class ControlEffect : AtomicEffectDefine
    {
        public List<string> RunEffect(Client.GameManager game, String PosField)
        {
            List<String> Result = new List<string>();
            if (game.MyInfo.BattleField.MinionCount != Card.Client.BattleFieldInfo.MaxMinionCount)
            {
                game.MyInfo.BattleField.AppendToBattle(game.YourInfo.BattleField.BattleMinions[int.Parse(PosField) - 1].深拷贝());
                game.YourInfo.BattleField.BattleMinions[int.Parse(PosField) - 1] = null;
                //CONTROL#1
                Result.Add(Card.Server.ActionCode.strControl + Card.CardUtility.strSplitMark + PosField[1]);
            }
            return Result;
        }
    }
}
