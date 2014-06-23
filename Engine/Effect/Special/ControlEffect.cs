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
        public static List<string> RunEffect(Client.GameStatus game, String PosField)
        {
            List<String> Result = new List<string>();
            if (game.client.MyInfo.BattleField.MinionCount != Engine.Client.BattleFieldInfo.MaxMinionCount)
            {
                game.client.MyInfo.BattleField.AppendToBattle(game.client.YourInfo.BattleField.BattleMinions[int.Parse(PosField) - 1].深拷贝());
                game.client.YourInfo.BattleField.BattleMinions[int.Parse(PosField) - 1] = null;
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
        public static void ReRunEffect(GameStatus game, String[] actField)
        {
            game.client.YourInfo.BattleField.AppendToBattle(game.client.MyInfo.BattleField.BattleMinions[int.Parse(actField[1]) - 1].深拷贝());
            game.client.MyInfo.BattleField.BattleMinions[int.Parse(actField[1]) - 1] = null;
        }
    }
}
