using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Card.Effect
{
    public static class HealthEffect
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
            int HealthPoint = singleEffect.ActualEffectPoint;
            //处理对象
            //ME#POS
            foreach (var PosInfo in PosList)
            {
                var PosField = PosInfo.Split(CardUtility.strSplitMark.ToCharArray());
                if (PosField[0] == CardUtility.strMe)
                {
                    if (PosField[1] == "0")
                    {
                        game.MySelf.RoleInfo.HealthPoint += HealthPoint;
                        if (game.MySelf.RoleInfo.HealthPoint > 30) game.MySelf.RoleInfo.HealthPoint = 30;
                        Result.Add(Card.Server.ActionCode.strHealth + Card.CardUtility.strSplitMark + PosInfo + Card.CardUtility.strSplitMark + game.MySelf.RoleInfo.HealthPoint.ToString());
                    }
                    else
                    {
                        //位置从1开始，数组从0开始
                        game.MySelf.RoleInfo.BattleField.BattleMinions[int.Parse(PosField[1]) - 1].ActualHealthPoint += HealthPoint;
                        if (game.MySelf.RoleInfo.BattleField.BattleMinions[int.Parse(PosField[1]) - 1].ActualHealthPoint > game.MySelf.RoleInfo.BattleField.BattleMinions[int.Parse(PosField[1]) - 1].StandardHealthPoint)
                            game.MySelf.RoleInfo.BattleField.BattleMinions[int.Parse(PosField[1]) - 1].ActualHealthPoint = game.MySelf.RoleInfo.BattleField.BattleMinions[int.Parse(PosField[1]) - 1].StandardHealthPoint;
                        Result.Add(Card.Server.ActionCode.strHealth + Card.CardUtility.strSplitMark + PosInfo + Card.CardUtility.strSplitMark + game.MySelf.RoleInfo.BattleField.BattleMinions[int.Parse(PosField[1]) - 1].ActualHealthPoint.ToString());
                    }
                }
                else
                {
                    if (PosField[1] == "0")
                    {
                        game.AgainstInfo.HealthPoint += HealthPoint;
                        if (game.AgainstInfo.HealthPoint > 30) game.AgainstInfo.HealthPoint = 30;
                        Result.Add(Card.Server.ActionCode.strHealth + Card.CardUtility.strSplitMark + PosInfo + Card.CardUtility.strSplitMark + game.AgainstInfo.HealthPoint.ToString());
                    }
                    else
                    {
                        //位置从1开始，数组从0开始
                        game.AgainstInfo.BattleField.BattleMinions[int.Parse(PosField[1]) - 1].ActualHealthPoint += HealthPoint;
                        if (game.AgainstInfo.BattleField.BattleMinions[int.Parse(PosField[1]) - 1].ActualHealthPoint > game.AgainstInfo.BattleField.BattleMinions[int.Parse(PosField[1]) - 1].StandardHealthPoint)
                            game.AgainstInfo.BattleField.BattleMinions[int.Parse(PosField[1]) - 1].ActualHealthPoint = game.AgainstInfo.BattleField.BattleMinions[int.Parse(PosField[1]) - 1].StandardHealthPoint;
                        Result.Add(Card.Server.ActionCode.strHealth + Card.CardUtility.strSplitMark + PosInfo + Card.CardUtility.strSplitMark + game.AgainstInfo.BattleField.BattleMinions[int.Parse(PosField[1]) - 1].ActualHealthPoint.ToString());
                    }
                }
            }
            return Result;
        }
    }
}
