using Engine.Action;
using Engine.Card;
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
        /// 伤害加成
        /// </summary>
        public Boolean 伤害加成 = false;
        /// <summary>
        /// 获得效果信息
        /// </summary>
        /// <param name="InfoArray"></param>
        void IAtomicEffect.GetField(List<string> InfoArray)
        {
            伤害效果表达式 = InfoArray[0];
            伤害加成 = ExpressHandler.GetBooleanExpress(InfoArray[1]);
        }
        /// <summary>
        /// 对英雄动作
        /// </summary>
        /// <param name="game"></param>
        /// <param name="PlayInfo"></param>
        /// <returns></returns>
        String IAtomicEffect.DealHero(ActionStatus game, Client.PublicInfo PlayInfo)
        {
            int AttackPoint = ExpressHandler.GetEffectPoint(game, 伤害效果表达式);
            //调整伤害值
            if (伤害加成) AttackPoint += game.AllRole.MyPublicInfo.BattleField.AbilityDamagePlus;
            if (PlayInfo.Hero.AfterBeAttack(AttackPoint))
            {
                game.battleEvenetHandler.事件池.Add(new EventCard.全局事件()
                {
                    触发事件类型 = EventCard.事件类型枚举.受伤,
                    触发位置 = PlayInfo.Hero.战场位置
                });
            }
            return Server.ActionCode.strAttack + CardUtility.strSplitMark + PlayInfo.Hero.战场位置.ToString() + CardUtility.strSplitMark + AttackPoint.ToString();
        }
        /// <summary>
        /// 对随从动作
        /// </summary>
        /// <param name="game"></param>
        /// <param name="Minion"></param>
        /// <returns></returns>
        String IAtomicEffect.DealMinion(ActionStatus game, Card.MinionCard Minion)
        {
            int AttackPoint = ExpressHandler.GetEffectPoint(game, 伤害效果表达式);
            //调整伤害值
            if (伤害加成) AttackPoint += game.AllRole.MyPublicInfo.BattleField.AbilityDamagePlus;
            if (Minion.设置被攻击后状态(AttackPoint))
            {
                game.battleEvenetHandler.事件池.Add(new Engine.Card.EventCard.全局事件()
                {
                    触发事件类型 = EventCard.事件类型枚举.受伤,
                    触发位置 = Minion.战场位置
                });
            }
            return Server.ActionCode.strAttack + CardUtility.strSplitMark + Minion.战场位置.ToString() + CardUtility.strSplitMark + AttackPoint.ToString();
        }
        /// <summary>
        /// 对方复原操作
        /// </summary>
        /// <param name="game"></param>
        /// <param name="actField"></param>
        void IAtomicEffect.ReRunEffect(ActionStatus game, string[] actField)
        {
            int AttackPoint = int.Parse(actField[3]);
            if (actField[1] == CardUtility.strYou)
            { 
                //MyInfo
                if (actField[2] == Client.BattleFieldInfo.HeroPos.ToString("D1"))
                {
                    game.AllRole.MyPublicInfo.Hero.AfterBeAttack(AttackPoint);
                }
                else
                {
                    game.AllRole.MyPublicInfo.BattleField.BattleMinions[int.Parse(actField[2]) - 1].设置被攻击后状态(AttackPoint);
                }
            }
            else
            {
                //YourInfo
                if (actField[2] == Client.BattleFieldInfo.HeroPos.ToString("D1"))
                {
                    game.AllRole.MyPublicInfo.Hero.AfterBeAttack(AttackPoint);
                }
                else
                {
                    game.AllRole.MyPublicInfo.BattleField.BattleMinions[int.Parse(actField[2]) - 1].设置被攻击后状态(AttackPoint);
                }
            }
        }
    }
}
