using Engine.Client;
using Engine.Utility;
using System;
using System.Collections.Generic;

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
        String IAtomicEffect.DealHero(Client.ClientPlayerInfo game, Client.PublicInfo PlayInfo)
        {
            int ShieldPoint = ExpressHandler.GetEffectPoint(game, 护甲回复表达式);
            int HealthPoint = ExpressHandler.GetEffectPoint(game, 生命值回复表达式);
            PlayInfo.AfterBeShield(ShieldPoint);
            if (PlayInfo.AfterBeHealth(HealthPoint))
            {
                ClientManager.事件处理组件.事件池.Add(new Engine.Utility.CardUtility.全局事件()
                {
                    触发事件类型 = CardUtility.事件类型枚举.治疗,
                    触发位置 = PlayInfo.战场位置
                });
            }
            return Server.ActionCode.strHealth + CardUtility.strSplitMark + PlayInfo.战场位置.ToString() + CardUtility.strSplitMark +
                        HealthPoint.ToString() + CardUtility.strSplitMark + ShieldPoint.ToString();
        }
        /// <summary>
        /// 对随从动作
        /// </summary>
        /// <param name="game"></param>
        /// <param name="Minion"></param>
        /// <returns></returns>
        String IAtomicEffect.DealMinion(Client.ClientPlayerInfo game, Card.MinionCard Minion)
        {
            int HealthPoint = ExpressHandler.GetEffectPoint(game, 生命值回复表达式);
            if (Minion.设置被治疗后状态(HealthPoint))
            {
                ClientManager.事件处理组件.事件池.Add(new Engine.Utility.CardUtility.全局事件()
                {
                    触发事件类型 = CardUtility.事件类型枚举.治疗,
                    触发位置 = Minion.战场位置
                });
            }
            return Server.ActionCode.strHealth + CardUtility.strSplitMark + Minion.战场位置.ToString() +
                                                 CardUtility.strSplitMark + HealthPoint.ToString();
        }
        /// <summary>
        /// 对方复原操作
        /// </summary>
        /// <param name="game"></param>
        /// <param name="actField"></param>
        void IAtomicEffect.ReRunEffect(Client.ClientPlayerInfo game, string[] actField)
        {
            int HealthPoint = int.Parse(actField[3]);
            if (actField[1] == CardUtility.strYou)
            {
                //MyInfo
                if (actField[2] == Client.BattleFieldInfo.HeroPos.ToString("D1"))
                {
                    game.BasicInfo.AfterBeHealth(HealthPoint);
                    if (actField.Length == 5)
                    {
                        game.BasicInfo.AfterBeShield(int.Parse(actField[4]));
                    }
                }
                else
                {
                    game.BasicInfo.BattleField.BattleMinions[int.Parse(actField[2]) - 1].设置被治疗后状态(HealthPoint);
                }
            }
            else
            {
                //YourInfo
                if (actField[2] == Client.BattleFieldInfo.HeroPos.ToString("D1"))
                {
                    game.YourInfo.AfterBeHealth(HealthPoint);
                    if (actField.Length == 5)
                    {
                        game.YourInfo.AfterBeShield(int.Parse(actField[4]));
                    }
                }
                else
                {
                    game.YourInfo.BattleField.BattleMinions[int.Parse(actField[2]) - 1].设置被治疗后状态(HealthPoint);
                }
            }
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
