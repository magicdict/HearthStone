using System;
using System.Collections.Generic;
using System.Linq;

namespace Card.Effect
{
    public static class PointEffect
    {
        /// <summary>
        /// 
        /// </summary>
        public const String strIgnore = "X";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="singleEffect"></param>
        /// <param name="game"></param>
        /// <returns></returns>
        public static List<string> RunEffect(EffectDefine singleEffect, Client.GameManager game, List<String> PosList)
        {
            List<String> Result = new List<string>();
            int AttackPoint = singleEffect.ActualEffectPoint;
            //处理对象
            foreach (var PosInfo in PosList)
            {
                var PosField = PosInfo.Split(CardUtility.strSplitMark.ToCharArray());
                if (PosField[0] == CardUtility.strMe)
                {
                    //位置从1开始，数组从0开始
                    RunPointEffect(game.MySelf.RoleInfo.BattleField.BattleMinions[int.Parse(PosField[1]) - 1],singleEffect.AddtionInfo);
                }
                else
                {
                    //位置从1开始，数组从0开始
                    RunPointEffect(game.YourInfo.BattleField.BattleMinions[int.Parse(PosField[1]) - 1], singleEffect.AddtionInfo);
                }
                Result.Add(Card.Server.ActionCode.strPoint + Card.CardUtility.strSplitMark + PosInfo + Card.CardUtility.strSplitMark + singleEffect.AddtionInfo);
            }
            return Result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Minion"></param>
        /// <param name="Addition"></param>
        public static void RunPointEffect(MinionCard Minion, String Addition)
        {
            var AttackHealth = Addition.Split("/".ToArray());
            if (AttackHealth[0] != strIgnore)
            {
                if (AttackHealth[0].Length != 1)
                {
                    Minion.实际攻击力 += int.Parse(AttackHealth[0]);
                }
                else
                {
                    Minion.实际攻击力 = int.Parse(AttackHealth[0]);
                }
            }
            if (AttackHealth[1] != strIgnore)
            {
                if (AttackHealth[1].Length != 1)
                {
                    Minion.实际生命值 += int.Parse(AttackHealth[1]);
                }
                else
                {
                    Minion.实际生命值 = int.Parse(AttackHealth[1]);
                }
            }
        }
    }
}
