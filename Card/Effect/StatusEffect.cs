using System;
using System.Collections.Generic;

namespace Card.Effect
{
    /// <summary>
    /// 状态改变效果
    /// </summary>
    public class StatusEffect : IEffectHandler
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
        /// <param name="myMinion"></param>
        /// <param name="AddtionInfo"></param>
        public static void RunStatusEffect(MinionCard myMinion, String AddtionInfo)
        {
            switch (AddtionInfo)
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <param name="singleEffect"></param>
        /// <param name="MeOrYou"></param>
        void IEffectHandler.DealHero(Client.GameManager game, EffectDefine singleEffect, bool MeOrYou)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <param name="singleEffect"></param>
        /// <param name="MeOrYou"></param>
        /// <param name="PosIndex"></param>
        void IEffectHandler.DealMinion(Client.GameManager game, EffectDefine singleEffect, bool MeOrYou, int PosIndex)
        {
            int HealthPoint = singleEffect.ActualEffectPoint;
            if (MeOrYou)
            {
                RunStatusEffect(game.MySelf.RoleInfo.BattleField.BattleMinions[PosIndex], singleEffect.AddtionInfo);
            }
            else
            {
                RunStatusEffect(game.YourInfo.BattleField.BattleMinions[PosIndex], singleEffect.AddtionInfo);
            }
        }
    }
}
