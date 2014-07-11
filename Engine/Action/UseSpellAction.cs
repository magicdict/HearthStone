using Engine.Card;
using Engine.Client;
using Engine.Effect;
using Engine.Utility;
using System;
using System.Collections.Generic;

namespace Engine.Action
{
    public static class UseSpellAction
    {
        /// <summary>
        /// 使用法术
        /// </summary>
        /// <param name="game"></param>
        /// <param name="IsMyAction">对象方向转换</param>
        public static void RunBS(ActionStatus game, String CardSn)
        {
            List<String> Result = new List<string>();
            SpellCard spell = (SpellCard)CardUtility.GetCardInfoBySN(CardSn);
            //Step1
            Engine.Utility.CardUtility.抉择枚举 PickAbilityResult = CardUtility.抉择枚举.第一效果;
            if (game.Interrupt.Step == 1)
            {
                switch (spell.效果选择类型)
                {
                    case Engine.Card.SpellCard.效果选择类型枚举.无需选择:
                        break;
                    case Engine.Card.SpellCard.效果选择类型枚举.主动选择:
                        if (game.AllRole.MyPublicInfo.BattleField.MinionCount != 0)
                        {
                            game.Interrupt.Step = 2;
                            game.Interrupt.ActionName = "SPELLDECIDE";
                            return;
                        }
                        break;
                    case Engine.Card.SpellCard.效果选择类型枚举.自动判定:
                        if (!ExpressHandler.AbilityPickCondition(game, spell.效果选择条件)) PickAbilityResult = CardUtility.抉择枚举.第二效果;
                        break;
                    default:
                        break;
                }
                game.Interrupt.Step = 2;
            }
            //Step2
            if (PickAbilityResult != CardUtility.抉择枚举.取消)
            {
                List<EffectDefine> SingleEffectList = new List<EffectDefine>();
                SpellCard.AbilityDefine ability;
                if (PickAbilityResult == CardUtility.抉择枚举.第一效果)
                {
                    ability = spell.FirstAbilityDefine;
                }
                else
                {
                    ability = spell.SecondAbilityDefine;
                }
                SpellCard.RunAbility(game, ability);
            }

            if (spell.原生卡牌)
                game.battleEvenetHandler.事件池.Add(new Engine.Utility.CardUtility.全局事件()
                {
                    触发事件类型 = CardUtility.事件类型枚举.施法,
                    触发位置 = new CardUtility.指定位置结构体()
                    {
                        Postion = BattleFieldInfo.HeroPos,
                        本方对方标识 = true
                    }
                });
            game.Interrupt.Step = 99;
            game.Interrupt.ActionName = CardUtility.strOK;
        }
    }
}
