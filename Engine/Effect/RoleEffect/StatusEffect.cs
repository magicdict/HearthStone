using Engine.Card;
using Engine.Utility;
using System;
using System.Collections.Generic;

namespace Engine.Effect
{
    /// <summary>
    /// 状态改变效果
    /// </summary>
    public class StatusEffect : IAtomicEffect
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
        /// <summary>
        /// 施加状态
        /// </summary>
        public String 施加状态;
        /// <summary>
        /// 对英雄施法
        /// </summary>
        /// <param name="game"></param>
        /// <param name="singleEffect"></param>
        /// <param name="本方对方标识"></param>
        void IAtomicEffect.DealHero(Client.GameManager game, Client.PublicInfo PlayInfo)
        {
            switch (施加状态)
            {
                case strFreeze:
                    PlayInfo.冰冻状态 = CardUtility.EffectTurn.效果命中;
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 对随从施法
        /// </summary>
        /// <param name="game"></param>
        /// <param name="singleEffect"></param>
        /// <param name="本方对方标识"></param>
        /// <param name="PosIndex"></param>
        void IAtomicEffect.DealMinion(Client.GameManager game, Card.MinionCard Minion)
        {
            switch (施加状态)
            {
                case strFreeze:
                    Minion.冰冻状态 = CardUtility.EffectTurn.效果命中;
                    break;
                case strSlience:
                    Minion.沉默状态 = true;
                    break;
                case strAngry:
                    Minion.风怒特性 = true;
                    break;
                case strCharge:
                    Minion.冲锋特性 = true;
                    if (Minion.AttactStatus == MinionCard.攻击状态.准备中)
                    {
                        Minion.AttactStatus = MinionCard.攻击状态.可攻击;
                    }
                    break;
                case strTaunt:
                    Minion.嘲讽特性 = true;
                    break;
                case strTurnEndDead:
                    Minion.特殊效果 = MinionCard.特殊效果列表.回合结束死亡;
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 获得效果信息
        /// </summary>
        /// <param name="InfoArray"></param>
        void IAtomicEffect.GetField(List<string> InfoArray)
        {
            施加状态 = InfoArray[0];
        }
    }
}
