using Card.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Card.Effect
{
    public static class HealthEffect
    {
        /// <summary>
        /// 治疗
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
                    if (PosField[1] == Card.Client.BattleFieldInfo.HeroPos.ToString())
                    {
                        game.MySelf.RoleInfo.HealthPoint += HealthPoint;
                        if (game.MySelf.RoleInfo.HealthPoint > PlayerBasicInfo.MaxHealthPoint) game.MySelf.RoleInfo.HealthPoint = PlayerBasicInfo.MaxHealthPoint;
                        Result.Add(Card.Server.ActionCode.strHealth + Card.CardUtility.strSplitMark + PosInfo + Card.CardUtility.strSplitMark + game.MySelf.RoleInfo.HealthPoint.ToString());
                    }
                    else
                    {
                        //位置从1开始，数组从0开始
                        game.MySelf.RoleInfo.BattleField.BattleMinions[int.Parse(PosField[1]) - 1].实际生命值 += HealthPoint;
                        if (game.MySelf.RoleInfo.BattleField.BattleMinions[int.Parse(PosField[1]) - 1].实际生命值 > game.MySelf.RoleInfo.BattleField.BattleMinions[int.Parse(PosField[1]) - 1].实际生命值上限)
                            game.MySelf.RoleInfo.BattleField.BattleMinions[int.Parse(PosField[1]) - 1].实际生命值 = game.MySelf.RoleInfo.BattleField.BattleMinions[int.Parse(PosField[1]) - 1].实际生命值上限;
                        Result.Add(Card.Server.ActionCode.strHealth + Card.CardUtility.strSplitMark + PosInfo + Card.CardUtility.strSplitMark + game.MySelf.RoleInfo.BattleField.BattleMinions[int.Parse(PosField[1]) - 1].实际生命值.ToString());
                    }
                }
                else
                {
                    if (PosField[1] == Card.Client.BattleFieldInfo.HeroPos.ToString())
                    {
                        game.YourInfo.HealthPoint += HealthPoint;
                        if (game.YourInfo.HealthPoint > PlayerBasicInfo.MaxHealthPoint) game.YourInfo.HealthPoint = PlayerBasicInfo.MaxHealthPoint;
                        Result.Add(Card.Server.ActionCode.strHealth + Card.CardUtility.strSplitMark + PosInfo + Card.CardUtility.strSplitMark + game.YourInfo.HealthPoint.ToString());
                    }
                    else
                    {
                        //位置从1开始，数组从0开始
                        game.YourInfo.BattleField.BattleMinions[int.Parse(PosField[1]) - 1].实际生命值 += HealthPoint;
                        if (game.YourInfo.BattleField.BattleMinions[int.Parse(PosField[1]) - 1].实际生命值 > game.YourInfo.BattleField.BattleMinions[int.Parse(PosField[1]) - 1].实际生命值上限)
                            game.YourInfo.BattleField.BattleMinions[int.Parse(PosField[1]) - 1].实际生命值 = game.YourInfo.BattleField.BattleMinions[int.Parse(PosField[1]) - 1].实际生命值上限;
                        Result.Add(Card.Server.ActionCode.strHealth + Card.CardUtility.strSplitMark + PosInfo + Card.CardUtility.strSplitMark + game.YourInfo.BattleField.BattleMinions[int.Parse(PosField[1]) - 1].实际生命值.ToString());
                    }
                }
            }
            return Result;
        }
    }
}
