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
        public static List<string> RunEffect(Client.GameManager game, String PosField)
        {
            List<String> Result = new List<string>();
            if (game.HostInfo.BattleField.MinionCount != Engine.Client.BattleFieldInfo.MaxMinionCount)
            {
                game.HostInfo.BattleField.AppendToBattle(game.GuestInfo.BattleField.BattleMinions[int.Parse(PosField) - 1].深拷贝());
                game.GuestInfo.BattleField.BattleMinions[int.Parse(PosField) - 1] = null;
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
        public static void ReRunEffect(GameManager game, String[] actField)
        {
            game.GuestInfo.BattleField.AppendToBattle(game.HostInfo.BattleField.BattleMinions[int.Parse(actField[1]) - 1].深拷贝());
            game.HostInfo.BattleField.BattleMinions[int.Parse(actField[1]) - 1] = null;
        }
    }
}
