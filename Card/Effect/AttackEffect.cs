using Card.Server;
using System;
using System.Collections.Generic;

namespace Card.Effect
{
    public static class AttackEffect
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="singleEffect"></param>
        /// <param name="game"></param>
        /// <param name="Pos"></param>
        /// <param name="Seed">随机数种子</param>
        /// <returns></returns>
        public static List<string> RunEffect(EffectDefine singleEffect, Client.GameManager game, CardUtility.TargetPosition Pos, int Seed)
        {
            List<String> PosList = Card.Effect.EffectDefine.GetTargetList(singleEffect, game, Pos, Seed);
            List<String> Result = new List<string>();
            int AttackPoint = singleEffect.ActualEffectPoint;
            //处理对象
            //ME#POS
            foreach (var PosInfo in PosList)
            {
                var PosField = PosInfo.Split(CardUtility.strSplitMark.ToCharArray());
                if (PosField[0] == CardUtility.strMe)
                {
                    if (PosField[1] == Card.Client.BattleFieldInfo.HeroPos.ToString())
                    {
                        game.MySelf.RoleInfo.AfterBeAttack(AttackPoint);
                    }
                    else
                    {
                        //位置从1开始，数组从0开始
                        game.MySelf.RoleInfo.BattleField.BattleMinions[int.Parse(PosField[1]) - 1].AfterBeAttack(AttackPoint);
                    }
                }
                else
                {
                    if (PosField[1] == Card.Client.BattleFieldInfo.HeroPos.ToString())
                    {

                        game.YourInfo.AfterBeAttack(AttackPoint);
                    }
                    else
                    {
                        //位置从1开始，数组从0开始
                        game.YourInfo.BattleField.BattleMinions[int.Parse(PosField[1]) - 1].AfterBeAttack(AttackPoint);
                    }
                }
                Result.Add(Card.Server.ActionCode.strAttack + Card.CardUtility.strSplitMark + PosInfo + Card.CardUtility.strSplitMark + AttackPoint.ToString());
            }
            return Result;
        }
    }
}
