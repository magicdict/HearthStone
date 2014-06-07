using Card.Server;
using System;
using System.Collections.Generic;

namespace Card.Effect
{
    /// <summary>
    /// 攻击效果
    /// </summary>
    public class AttackEffect : IEffectHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <param name="singleEffect"></param>
        /// <param name="MeOrYou"></param>
        void IEffectHandler.DealHero(Client.GameManager game, EffectDefine singleEffect, Boolean MeOrYou)
        {
            int AttackPoint = singleEffect.ActualEffectPoint;
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
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <param name="singleEffect"></param>
        /// <param name="MeOrYou"></param>
        /// <param name="PosIndex"></param>
        void IEffectHandler.DealMinion(Client.GameManager game, EffectDefine singleEffect, Boolean MeOrYou, int PosIndex)
        {
            int AttackPoint = singleEffect.ActualEffectPoint;
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
    }
}
