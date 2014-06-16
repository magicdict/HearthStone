using System;
using System.Collections.Generic;
using Engine.Utility;
namespace Engine.Effect
{
    public class ControlEffect
    {
        public List<string> RunEffect(Client.GameManager game, String PosField)
        {
            List<String> Result = new List<string>();
            if (game.MyInfo.BattleField.MinionCount != Engine.Client.BattleFieldInfo.MaxMinionCount)
            {
                game.MyInfo.BattleField.AppendToBattle(game.YourInfo.BattleField.BattleMinions[int.Parse(PosField) - 1].深拷贝());
                game.YourInfo.BattleField.BattleMinions[int.Parse(PosField) - 1] = null;
                //CONTROL#1
                Result.Add(Engine.Server.ActionCode.strControl + Engine.Utility.CardUtility.strSplitMark + PosField[1]);
            }
            return Result;
        }
    }
}
