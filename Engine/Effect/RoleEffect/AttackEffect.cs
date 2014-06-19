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
        String IAtomicEffect.DealHero(Client.GameManager game, Client.PublicInfo PlayInfo)
        {
            int AttackPoint = ExpressHandler.GetEffectPoint(game, 伤害效果表达式);
            //调整伤害值
            if (伤害加成) AttackPoint += game.MyInfo.BattleField.AbilityDamagePlus;
            if (PlayInfo.AfterBeAttack(AttackPoint))
            {
                game.事件处理组件.事件池.Add(new Engine.Utility.CardUtility.全局事件()
                {
                    触发事件类型 = CardUtility.事件类型列表.受伤,
                    触发位置 = PlayInfo.战场位置
                });
            }
            return Server.ActionCode.strAttack + CardUtility.strSplitMark + PlayInfo.战场位置.ToString() + CardUtility.strSplitMark + AttackPoint.ToString();
        }
        /// <summary>
        /// 对随从动作
        /// </summary>
        /// <param name="game"></param>
        /// <param name="Minion"></param>
        /// <returns></returns>
        String IAtomicEffect.DealMinion(Client.GameManager game, Card.MinionCard Minion)
        {
            int AttackPoint = ExpressHandler.GetEffectPoint(game, 伤害效果表达式);
            //调整伤害值
            if (伤害加成) AttackPoint += game.MyInfo.BattleField.AbilityDamagePlus;
            if (Minion.AfterBeAttack(AttackPoint))
            {
                game.事件处理组件.事件池.Add(new Engine.Utility.CardUtility.全局事件()
                {
                    触发事件类型 = CardUtility.事件类型列表.受伤,
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
        void IAtomicEffect.ReRunEffect(Client.GameManager game, string[] actField)
        {
            int AttackPoint = int.Parse(actField[3]);
            if (actField[1] == CardUtility.strYou)
            { 
                //MyInfo
                if (actField[2] == Client.BattleFieldInfo.HeroPos.ToString("D1"))
                {
                    game.MyInfo.AfterBeAttack(AttackPoint);
                }
                else
                {
                    game.MyInfo.BattleField.BattleMinions[int.Parse(actField[2]) - 1].AfterBeAttack(AttackPoint);
                }
            }
            else
            {
                //YourInfo
                if (actField[2] == Client.BattleFieldInfo.HeroPos.ToString("D1"))
                {
                    game.YourInfo.AfterBeAttack(AttackPoint);
                }
                else
                {
                    game.YourInfo.BattleField.BattleMinions[int.Parse(actField[2]) - 1].AfterBeAttack(AttackPoint);
                }
            }
        }
    }
}
