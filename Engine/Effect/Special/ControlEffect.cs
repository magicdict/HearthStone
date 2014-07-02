using Engine.Client;
using Engine.Utility;
using System;
using System.Collections.Generic;
namespace Engine.Effect
{
    /// <summary>
    /// 控制
    /// </summary>
    public class ControlEffect
    {
        public static List<string> RunEffect(Client.ClientPlayerInfo game, String PosField)
        {
            List<String> Result = new List<string>();
            if (game.BasicInfo.BattleField.MinionCount != SystemManager.MaxMinionCount)
            {
                game.BasicInfo.BattleField.AppendToBattle(game.YourInfo.BattleField.BattleMinions[int.Parse(PosField) - 1].DeepCopy());
                game.YourInfo.BattleField.BattleMinions[int.Parse(PosField) - 1] = null;
                //CONTROL#1
                Result.Add(Engine.Server.ActionCode.strControl + Engine.Utility.CardUtility.strSplitMark + PosField[1]);
            }
            return Result;
        }
        /// <summary>
        /// 对方复原操作
        /// </summary>
        /// <param name="game"></param>
        /// <param name="actField"></param>
        public static void ReRunEffect(ClientPlayerInfo game, String[] actField)
        {
            game.YourInfo.BattleField.AppendToBattle(game.BasicInfo.BattleField.BattleMinions[int.Parse(actField[1]) - 1].DeepCopy());
            game.BasicInfo.BattleField.BattleMinions[int.Parse(actField[1]) - 1] = null;
        }
    }
}
