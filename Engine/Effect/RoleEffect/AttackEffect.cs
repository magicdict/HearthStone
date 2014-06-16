using Engine.Server;
using Engine.Utility;
using System;
using System.Collections.Generic;

namespace Engine.Effect
{
    /// <summary>
    /// 攻击效果
    /// </summary>
    public class AttackEffect : AtomicEffectDefine, IEffectHandler
    {
        /// <summary>
        /// 效果表达式
        /// </summary>
        public String 伤害效果表达式 = String.Empty;
        /// <summary>
        /// 强化伤害效果表达式
        /// </summary>
        public String 强化伤害效果表达式 = String.Empty;
        /// <summary>
        /// 实际伤害点数
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public int 实际伤害点数 = 0;
        /// <summary>
        /// 实际强化伤害点数
        /// </summary>
        public int 实际强化伤害点数 = 0;
        /// <summary>
        /// 对英雄
        /// </summary>
        /// <param name="game"></param>
        /// <param name="singleEffect"></param>
        /// <param name="MeOrYou"></param>
        void IEffectHandler.DealHero(Client.GameManager game, EffectDefine singleEffect, Boolean MeOrYou)
        {
            //调整伤害值
            int AttackPoint = 实际伤害点数;
            if (MeOrYou)
            {
                game.MyInfo.AfterBeAttack(AttackPoint);
                game.事件处理组件.事件池.Add(new Engine.Utility.CardUtility.全局事件()
                {
                    事件类型 = CardUtility.事件类型列表.受伤,
                    触发方向 = CardUtility.TargetSelectDirectEnum.本方,
                    触发位置 = Engine.Client.BattleFieldInfo.HeroPos
                });
            }
            else
            {
                game.YourInfo.AfterBeAttack(AttackPoint);
                game.事件处理组件.事件池.Add(new Engine.Utility.CardUtility.全局事件()
                {
                    事件类型 = CardUtility.事件类型列表.受伤,
                    触发方向 = CardUtility.TargetSelectDirectEnum.对方,
                    触发位置 = Engine.Client.BattleFieldInfo.HeroPos
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
        void IEffectHandler.DealMinion(Client.GameManager game, EffectDefine singleEffect, Boolean MeOrYou, int PosIndex)
        {
            //调整伤害值
            int AttackPoint = 实际伤害点数;
            if (MeOrYou)
            {
                if (game.MyInfo.BattleField.BattleMinions[PosIndex].AfterBeAttack(AttackPoint))
                {
                    game.事件处理组件.事件池.Add(new Engine.Utility.CardUtility.全局事件()
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
                    game.事件处理组件.事件池.Add(new Engine.Utility.CardUtility.全局事件()
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
