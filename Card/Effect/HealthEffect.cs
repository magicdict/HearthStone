using Card.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Card.Effect
{
    /// <summary>
    /// 治疗效果
    /// </summary>
    public class HealthEffect : AtomicEffectDefine, IEffectHandler
    {
        /// <summary>
        /// 生命值回复表达式
        /// </summary>
        public String 生命值回复表达式 = String.Empty;
        /// <summary>
        /// 护甲回复表达式
        /// </summary>
        public String 护甲回复表达式 = String.Empty;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <param name="singleEffect"></param>
        /// <param name="MeOrYou"></param>
        void IEffectHandler.DealHero(GameManager game, AtomicEffectDefine singleEffect, bool MeOrYou)
        {
            int ShieldPoint = Effect.Effecthandler.GetEffectPoint(game,护甲回复表达式);
            int HealthPoint = Effect.Effecthandler.GetEffectPoint(game,生命值回复表达式);
            if (MeOrYou)
            {
                game.MyInfo.AfterBeShield(ShieldPoint);
                if (game.MyInfo.AfterBeHealth(HealthPoint))
                {
                    game.事件池.Add(new Card.CardUtility.全局事件()
                    {
                        事件类型 = CardUtility.事件类型列表.治疗,
                        触发方向 = CardUtility.TargetSelectDirectEnum.本方,
                        触发位置 = Card.Client.BattleFieldInfo.HeroPos
                    });
                }
            }
            else
            {
                game.YourInfo.AfterBeShield(ShieldPoint);
                if (game.YourInfo.AfterBeHealth(HealthPoint))
                {
                    game.事件池.Add(new Card.CardUtility.全局事件()
                    {
                        事件类型 = CardUtility.事件类型列表.治疗,
                        触发方向 = CardUtility.TargetSelectDirectEnum.对方,
                        触发位置 = Card.Client.BattleFieldInfo.HeroPos
                    });
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <param name="singleEffect"></param>
        /// <param name="MeOrYou"></param>
        /// <param name="PosIndex"></param>
        void IEffectHandler.DealMinion(GameManager game, AtomicEffectDefine singleEffect, bool MeOrYou, int PosIndex)
        {
            int HealthPoint = Effect.Effecthandler.GetEffectPoint(game, 生命值回复表达式);
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

        public new void GetField()
        {
            throw new NotImplementedException();
        }
    }
}
