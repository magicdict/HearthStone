using Engine.Action;
using Engine.Client;
using Engine.Control;
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
        String IAtomicEffect.DealHero(ActionStatus game, Client.PublicInfo PlayInfo)
        {
            int ShieldPoint = ExpressHandler.GetEffectPoint(game, 护甲回复表达式);
            int HealthPoint = ExpressHandler.GetEffectPoint(game, 生命值回复表达式);
            PlayInfo.AfterBeShield(ShieldPoint);
            if (PlayInfo.AfterBeHealth(HealthPoint))
            {
                game.eventhandler.事件池.Add(new Engine.Utility.CardUtility.全局事件()
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
        String IAtomicEffect.DealMinion(ActionStatus game, Card.MinionCard Minion)
        {
            int HealthPoint = ExpressHandler.GetEffectPoint(game, 生命值回复表达式);
            if (Minion.设置被治疗后状态(HealthPoint))
            {
                game.eventhandler.事件池.Add(new Engine.Utility.CardUtility.全局事件()
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
        void IAtomicEffect.ReRunEffect(ActionStatus game, string[] actField)
        {
            int HealthPoint = int.Parse(actField[3]);
            if (actField[1] == CardUtility.strYou)
            {
                //MyInfo
                if (actField[2] == Client.BattleFieldInfo.HeroPos.ToString("D1"))
                {
                    game.AllRole.MyPublicInfo.AfterBeHealth(HealthPoint);
                    if (actField.Length == 5)
                    {
                        game.AllRole.MyPublicInfo.AfterBeShield(int.Parse(actField[4]));
                    }
                }
                else
                {
                    game.AllRole.MyPublicInfo.BattleField.BattleMinions[int.Parse(actField[2]) - 1].设置被治疗后状态(HealthPoint);
                }
            }
            else
            {
                //YourInfo
                if (actField[2] == Client.BattleFieldInfo.HeroPos.ToString("D1"))
                {
                    game.AllRole.YourPublicInfo.AfterBeHealth(HealthPoint);
                    if (actField.Length == 5)
                    {
                        game.AllRole.YourPublicInfo.AfterBeShield(int.Parse(actField[4]));
                    }
                }
                else
                {
                    game.AllRole.YourPublicInfo.BattleField.BattleMinions[int.Parse(actField[2]) - 1].设置被治疗后状态(HealthPoint);
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
