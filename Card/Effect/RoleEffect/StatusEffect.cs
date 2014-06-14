using Engine.Card;
using Engine.Utility;
using System;
using System.Collections.Generic;

namespace Engine.Effect
{
    /// <summary>
    /// 状态改变效果
    /// </summary>
    public class StatusEffect : EffectDefine, IEffectHandler
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
        /// 回合结束死亡
        /// </summary>
        public const String strTurnEndDead = "TURNENDDEAD";

        public String 施加状态;

        /// <summary>
        /// 施法
        /// </summary>
        /// <param name="myMinion"></param>
        /// <param name="AddtionInfo"></param>
        public void RunStatusEffect(MinionCard myMinion)
        {
            switch (施加状态)
            {
                case strFreeze:
                    myMinion.冰冻状态 = CardUtility.EffectTurn.效果命中;
                    break;
                case strSlience:
                    myMinion.Is沉默Status = true;
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
                case strTurnEndDead:
                    myMinion.特殊效果 = MinionCard.特殊效果列表.回合结束死亡;
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
            if (MeOrYou)
            {
                RunStatusEffect(game.MyInfo.BattleField.BattleMinions[PosIndex]);
            }
            else
            {
                RunStatusEffect(game.YourInfo.BattleField.BattleMinions[PosIndex]);
            }
        }
    }
}
