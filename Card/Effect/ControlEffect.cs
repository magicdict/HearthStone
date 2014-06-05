using System;
using System.Collections.Generic;

namespace Card.Effect
{
    public static class ControlEffect
    {
        public static List<string> RunEffect(EffectDefine singleEffect, Client.GameManager game, List<String> PosList)
        {
            List<String> Result = new List<string>();
            var PosField = PosList[0].Split(CardUtility.strSplitMark.ToCharArray());
            if (game.MySelf.RoleInfo.BattleField.MinionCount != Card.Client.BattleFieldInfo.MaxMinionCount)
            {
                game.MySelf.RoleInfo.BattleField.AppendToBattle(game.YourInfo.BattleField.BattleMinions[int.Parse(PosField[1]) - 1].深拷贝());
                game.YourInfo.BattleField.BattleMinions[int.Parse(PosField[1]) - 1] = null;
                //CONTROL#1
                Result.Add(Card.Server.ActionCode.strControl + Card.CardUtility.strSplitMark + PosField[1]);
            }
            return Result;
        }
    }
}
