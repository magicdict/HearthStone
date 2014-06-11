using Card.Server;
using System;
using System.Collections.Generic;

namespace Card.Effect
{
    /// <summary>
    /// 攻击效果
    /// </summary>
    public class AttackEffect : AtomicEffectDefine, IEffectHandler
    {
        /// <summary>
        /// 法术方向
        /// </summary>
        public CardUtility.TargetSelectDirectEnum 法术方向 = CardUtility.TargetSelectDirectEnum.双方;
        /// <summary>
        /// 标准效果表达式
        /// </summary>
        public String 标准效果表达式 = String.Empty;
        /// <summary>
        /// 标准效果回数表达式
        /// </summary>
        public String 标准效果回数表达式 = String.Empty;
        /// <summary>
        /// 实际伤害点数
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public int 实际伤害点数 = 0;
        /// <summary>
        /// 对英雄
        /// </summary>
        /// <param name="game"></param>
        /// <param name="singleEffect"></param>
        /// <param name="MeOrYou"></param>
        void IEffectHandler.DealHero(Client.GameManager game, AtomicEffectDefine singleEffect, Boolean MeOrYou)
        {
            //调整伤害值
            int AttackPoint = 实际伤害点数;
            if (MeOrYou)
            {
                game.MyInfo.AfterBeAttack(AttackPoint);
                game.事件池.Add(new Card.CardUtility.全局事件()
                {
                    事件类型 = CardUtility.事件类型列表.受伤,
                    触发方向 = CardUtility.TargetSelectDirectEnum.本方,
                    触发位置 = Card.Client.BattleFieldInfo.HeroPos
                });
            }
            else
            {
                game.YourInfo.AfterBeAttack(AttackPoint);
                game.事件池.Add(new Card.CardUtility.全局事件()
                {
                    事件类型 = CardUtility.事件类型列表.受伤,
                    触发方向 = CardUtility.TargetSelectDirectEnum.对方,
                    触发位置 = Card.Client.BattleFieldInfo.HeroPos
                });
            }
        }
        /// <summary>
        /// 对随从
        /// </summary>
        /// <param name="game"></param>
        /// <param name="singleEffect"></param>
        /// <param name="MeOrYou"></param>
        /// <param name="PosIndex"></param>
        void IEffectHandler.DealMinion(Client.GameManager game, AtomicEffectDefine singleEffect, Boolean MeOrYou, int PosIndex)
        {
            //调整伤害值
            int AttackPoint = 实际伤害点数;
            if (MeOrYou)
            {
                if (game.MyInfo.BattleField.BattleMinions[PosIndex].AfterBeAttack(AttackPoint))
                {
                    game.事件池.Add(new Card.CardUtility.全局事件()
                    {
                        事件类型 = CardUtility.事件类型列表.受伤,
                        触发方向 = CardUtility.TargetSelectDirectEnum.本方,
                        触发位置 = PosIndex + 1
                    });
                }
            }
            else
            {
                if (game.YourInfo.BattleField.BattleMinions[PosIndex].AfterBeAttack(AttackPoint))
                {
                    game.事件池.Add(new Card.CardUtility.全局事件()
                    {
                        事件类型 = CardUtility.事件类型列表.受伤,
                        触发方向 = CardUtility.TargetSelectDirectEnum.对方,
                        触发位置 = PosIndex + 1
                    });
                }
            }
        }
        /// <summary>
        /// 初始化值
        /// </summary>
        public new void GetField()
        {
            法术方向 = CardUtility.GetEnum<CardUtility.TargetSelectDirectEnum>(InfoArray[1], CardUtility.TargetSelectDirectEnum.双方);
        }
    }
}
