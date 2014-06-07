using Card.Client;
using System;
using System.Collections.Generic;

namespace Card.Effect
{
    /// <summary>
    /// 治疗效果
    /// </summary>
    public class HealthEffect : IEffectHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <param name="singleEffect"></param>
        /// <param name="MeOrYou"></param>
        void IEffectHandler.DealHero(GameManager game, EffectDefine singleEffect, bool MeOrYou)
        {
            int HealthPoint = singleEffect.ActualEffectPoint;
            if (MeOrYou)
            {
                game.MyInfo.AfterBeHealth(HealthPoint);
                game.事件池.Add(new Card.CardUtility.全局事件()
                {
                    事件类型 = CardUtility.事件类型列表.治疗,
                    触发方向 = CardUtility.TargetSelectDirectEnum.本方,
                    触发位置 = Card.Client.BattleFieldInfo.HeroPos
                });
            }
            else
            {
                game.YourInfo.AfterBeHealth(HealthPoint);
                game.事件池.Add(new Card.CardUtility.全局事件()
                {
                    事件类型 = CardUtility.事件类型列表.治疗,
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
        void IEffectHandler.DealMinion(GameManager game, EffectDefine singleEffect, bool MeOrYou, int PosIndex)
        {
            int HealthPoint = singleEffect.ActualEffectPoint;
            if (MeOrYou)
            {
                if (game.MyInfo.BattleField.BattleMinions[PosIndex].AfterBeHealth(HealthPoint))
                {
                    game.事件池.Add(new Card.CardUtility.全局事件()
                    {
                        事件类型 = CardUtility.事件类型列表.治疗,
                        触发方向 = CardUtility.TargetSelectDirectEnum.本方,
                        触发位置 = PosIndex + 1
                    });
                }
            }
            else
            {
                if (game.YourInfo.BattleField.BattleMinions[PosIndex].AfterBeHealth(HealthPoint))
                {
                    game.事件池.Add(new Card.CardUtility.全局事件()
                    {
                        事件类型 = CardUtility.事件类型列表.治疗,
                        触发方向 = CardUtility.TargetSelectDirectEnum.对方,
                        触发位置 = PosIndex + 1
                    });
                }
            }
        }
    }
}
