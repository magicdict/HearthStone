using Engine.Action;
using Engine.Client;
using Engine.Control;
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
        public static List<string> RunEffect(ActionStatus game, String PosField)
        {
            List<String> Result = new List<string>();
            if (game.AllRole.MyPublicInfo.BattleField.MinionCount != SystemManager.MaxMinionCount)
            {
                game.AllRole.MyPublicInfo.BattleField.AppendToBattle(game.AllRole.YourPublicInfo.BattleField.BattleMinions[int.Parse(PosField) - 1].DeepCopy());
                game.AllRole.YourPublicInfo.BattleField.BattleMinions[int.Parse(PosField) - 1] = null;
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
        public static void ReRunEffect(ActionStatus game, String[] actField)
        {
            game.AllRole.YourPublicInfo.BattleField.AppendToBattle(game.AllRole.MyPublicInfo.BattleField.BattleMinions[int.Parse(actField[1]) - 1].DeepCopy());
            game.AllRole.MyPublicInfo.BattleField.BattleMinions[int.Parse(actField[1]) - 1] = null;
        }
    }
}
