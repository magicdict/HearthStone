using Engine.Server;
using Engine.Utility;
using System;
using System.Collections.Generic;

namespace Engine.Effect
{
    /// <summary>
    /// 攻击效果
    /// </summary>
    public class AttackEffect : IAtomicEffect
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
        public String 实际伤害点数 = String.Empty;
        /// <summary>
        /// 获得效果信息
        /// </summary>
        /// <param name="InfoArray"></param>
        void IAtomicEffect.GetField(List<string> InfoArray)
        {
            实际伤害点数 = InfoArray[0];
        }
        void IAtomicEffect.DealHero(Client.GameManager game, Client.PublicInfo PlayInfo)
        {
            //调整伤害值
            int AttackPoint = ExpressHandler.GetEffectPoint(game, 实际伤害点数);
            if (PlayInfo.AfterBeAttack(AttackPoint))
            {
                game.事件处理组件.事件池.Add(new Engine.Utility.CardUtility.全局事件()
                {
                    触发事件类型 = CardUtility.事件类型列表.受伤,
                    触发位置 = PlayInfo.战场位置
                });
            }
        }
        void IAtomicEffect.DealMinion(Client.GameManager game, Card.MinionCard Minion)
        {
            //调整伤害值
            int AttackPoint = ExpressHandler.GetEffectPoint(game, 实际伤害点数);
            if (Minion.AfterBeAttack(AttackPoint))
            {
                game.事件处理组件.事件池.Add(new Engine.Utility.CardUtility.全局事件()
                {
                    触发事件类型 = CardUtility.事件类型列表.受伤,
                    触发位置 = Minion.战场位置
                });
            }
        }
    }
}
