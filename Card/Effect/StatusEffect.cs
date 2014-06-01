using System;
using System.Collections.Generic;

namespace Card.Effect
{
    /// <summary>
    /// 状态改变效果
    /// </summary>
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
        /// 圣盾
        /// </summary>
        public const String strShield = "SHIELD";
        /// <summary>
        /// 嘲讽
        /// </summary>
        public const String strTaunt = "TAUNT";
        /// <summary>
        /// 风怒
        /// </summary>
        public const String strAngry = "ANGRY";
        /// <summary>
        /// 冲锋
        /// </summary>
        public const String strCharge = "CHARGE";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="singleEffect"></param>
        /// <param name="game"></param>
        /// <returns></returns>
        public static List<string> RunEffect(EffectDefine singleEffect, Client.GameManager game, List<String> PosList)
        {
            List<String> Result = new List<string>();
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
                        var myMinion = game.MySelf.RoleInfo.BattleField.BattleMinions[int.Parse(PosField[1]) - 1];
                        switch (singleEffect.AddtionInfo)
                        {
                            case strFreeze:
                                myMinion.冰冻状态 = CardUtility.EffectTurn.效果命中;
                                break;
                            case strSlience:
                                myMinion.被沉默();
                                break;
                            case strAngry:
                                myMinion.Actual风怒 = true;
                                break;
                            case strCharge:
                                myMinion.Actual冲锋 = true;
                                if (myMinion.AttactStatus == MinionCard.攻击状态.准备中)
                                {
                                    myMinion.AttactStatus = MinionCard.攻击状态.可攻击;
                                }
                                break;
                            case strTaunt:
                                myMinion.Actual嘲讽 = true;
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
                        var yourMinion = game.YourInfo.BattleField.BattleMinions[int.Parse(PosField[1]) - 1];
                        switch (singleEffect.AddtionInfo)
                        {
                            case strFreeze:
                                yourMinion.冰冻状态 = CardUtility.EffectTurn.效果命中;
                                break;
                            case strSlience:
                                yourMinion.被沉默();
                                break;
                            case strAngry:
                                yourMinion.Actual风怒 = true;
                                break;
                            case strCharge:
                                yourMinion.Actual冲锋 = true;
                                break;
                            case strTaunt:
                                yourMinion.Actual嘲讽 = true;
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
