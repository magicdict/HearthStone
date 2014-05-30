using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Card.Effect
{
    public static class StatusEffect
    {

        /// <summary>
        /// 冰冻状态
        /// </summary>
        public const String strFreeze = "FREEZE";
        /// <summary>
        /// 沉默
        /// </summary>
        public const String strSlience = "SLIENCE";
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
                        switch (singleEffect.AddtionInfo)
                        {
                            case strFreeze:
                                game.MySelf.RoleInfo.冰冻状态 = CardUtility.EffectTurn.效果命中;
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        //位置从1开始，数组从0开始
                        switch (singleEffect.AddtionInfo)
                        {
                            case strFreeze:
                                game.MySelf.RoleInfo.BattleField.BattleMinions[int.Parse(PosField[1]) - 1].冰冻状态 = CardUtility.EffectTurn.效果命中;
                                break;
                            case strSlience:
                                game.MySelf.RoleInfo.BattleField.BattleMinions[int.Parse(PosField[1]) - 1].被沉默();
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    if (PosField[1] == Card.Client.BattleFieldInfo.HeroPos.ToString())
                    {
                        switch (singleEffect.AddtionInfo)
                        {
                            case strFreeze:
                                game.YourInfo.冰冻状态 = CardUtility.EffectTurn.效果命中;
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        //位置从1开始，数组从0开始
                        switch (singleEffect.AddtionInfo)
                        {
                            case strFreeze:
                                game.YourInfo.BattleField.BattleMinions[int.Parse(PosField[1]) - 1].冰冻状态 = CardUtility.EffectTurn.效果命中;
                                break;
                            case strSlience:
                                game.YourInfo.BattleField.BattleMinions[int.Parse(PosField[1]) - 1].被沉默();
                                break;
                            default:
                                break;
                        }
                    }
                }
                //STATUS#ME#1#FREEZE
                Result.Add(Card.Server.ActionCode.strStatus + Card.CardUtility.strSplitMark + PosInfo + Card.CardUtility.strSplitMark + strFreeze);
            }
            return Result;
        }

    }
}
