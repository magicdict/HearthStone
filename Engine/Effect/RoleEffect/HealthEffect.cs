using Engine.Client;
using Engine.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Effect
{
    /// <summary>
    /// 治疗效果
    /// </summary>
    public class HealthEffect : IAtomicEffect
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
        /// 对英雄动作
        /// </summary>
        /// <param name="game"></param>
        /// <param name="PlayInfo"></param>
        /// <returns></returns>
        String IAtomicEffect.DealHero(Client.GameManager game, Client.PublicInfo PlayInfo)
        {
            int ShieldPoint = ExpressHandler.GetEffectPoint(game, 护甲回复表达式);
            int HealthPoint = ExpressHandler.GetEffectPoint(game, 生命值回复表达式);
            PlayInfo.AfterBeShield(ShieldPoint);
            if (PlayInfo.AfterBeHealth(HealthPoint))
            {
                game.事件处理组件.事件池.Add(new Engine.Utility.CardUtility.全局事件()
                {
                    触发事件类型 = CardUtility.事件类型列表.治疗,
                    触发位置 = PlayInfo.战场位置
                });
            }
            return Server.ActionCode.strHealth + CardUtility.strSplitMark + PlayInfo.战场位置.ToString() + CardUtility.strSplitMark +
                        ShieldPoint.ToString() + CardUtility.strSplitMark + HealthPoint.ToString();
        }
        /// <summary>
        /// 对随从动作
        /// </summary>
        /// <param name="game"></param>
        /// <param name="Minion"></param>
        /// <returns></returns>
        String IAtomicEffect.DealMinion(Client.GameManager game, Card.MinionCard Minion)
        {
            int HealthPoint = ExpressHandler.GetEffectPoint(game, 生命值回复表达式);
            if (Minion.AfterBeHealth(HealthPoint))
            {
                game.事件处理组件.事件池.Add(new Engine.Utility.CardUtility.全局事件()
                {
                    触发事件类型 = CardUtility.事件类型列表.治疗,
                    触发位置 = Minion.战场位置
                });
            }
            return Server.ActionCode.strHealth + CardUtility.strSplitMark + Minion.战场位置.ToString() + 
                                                 CardUtility.strSplitMark + HealthPoint.ToString();
        }
        /// <summary>
        /// 获得效果信息
        /// </summary>
        /// <param name="InfoArray"></param>
        void IAtomicEffect.GetField(List<string> InfoArray)
        {
            生命值回复表达式 = InfoArray[0];
            护甲回复表达式 = InfoArray[1];
        }
    }
}
