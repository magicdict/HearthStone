using Engine.Action;
using Engine.Client;
using Engine.Server;
using Engine.Utility;
using System;
using System.Collections.Generic;

namespace Engine.AI
{
    public static class DoAction
    {
        /// <summary>
        /// 获得一个动作
        /// </summary>
        /// <param name="gameStatus"></param>
        /// <returns></returns>
        public static void GetAction(ActionStatus gameStatus)
        {
            String Action = String.Empty;
            PublicInfo PlayInfo = gameStatus.AllRole.MyPublicInfo;
            PrivateInfo PlaySelfInfo = gameStatus.AllRole.MyPrivateInfo;
            List<String> Result = new List<string>();
            //优先使用技能
            if (PlayInfo.IsHeroSkillEnable(true))
            {
                //召唤
                if (PlayInfo.Hero.HeroSkill.FirstAbilityDefine.MainAbilityDefine.TrueAtomicEffect.AtomicEffectType ==
                    Effect.AtomicEffectDefine.AtomicEffectEnum.召唤 && 
                    PlayInfo.BattleField.MinionCount != SystemManager.MaxMinionCount)
                {
                    gameStatus.Interrupt.ActionCard = new MinimizeBattleInfo.HandCardInfo();
                    gameStatus.Interrupt.ActionCard.Init(PlayInfo.Hero.HeroSkill);
                    gameStatus.ActionName = "USEHEROSKILL";
                    gameStatus.Interrupt.Step = 1;
                    gameStatus.Interrupt.ActionName = "SPELL";
                    PlayInfo.crystal.CurrentRemainPoint -= PlayInfo.Hero.HeroSkill.使用成本;
                    PlayInfo.Hero.IsUsedHeroAbility = true;
                    RunAction.StartActionCS(gameStatus, PlayInfo.Hero.HeroSkill.序列号, new string[] { });
                    gameStatus.Interrupt.ActionName = "SPELL";
                    return;
                }
            }
            //能上场的随从都上场
            int HandCardIndex = HasBattleMinion(gameStatus);
            if (HandCardIndex != -1)
            {
                int newPos = PlayInfo.BattleField.MinionCount + 1;
                var card = PlaySelfInfo.handCards[HandCardIndex];
                RunAction.StartActionCS(gameStatus, card.序列号, new string[] { newPos.ToString() });
                PlayInfo.crystal.CurrentRemainPoint -= card.使用成本;
                PlaySelfInfo.RemoveUsedCard(card.序列号);
                PlayInfo.HandCardCount = PlaySelfInfo.handCards.Count;
                gameStatus.Interrupt.ActionCard = new MinimizeBattleInfo.HandCardInfo();
                gameStatus.Interrupt.ActionCard.Init(card);
                gameStatus.Interrupt.ActionName = "MINION";
                return;
            }
            //能攻击的随从都攻击，优先攻击英雄
            int AttackPos = HasAttackMinion(gameStatus);
            if (AttackPos != -1)
            {
                int BeAttackedPos = GetAttackTarget(gameStatus);
                RunAction.Fight(gameStatus, AttackPos, BeAttackedPos, true);
                gameStatus.Interrupt.ActionName = "FIGHT";
                gameStatus.Interrupt.ExternalInfo = AttackPos.ToString() + BeAttackedPos.ToString();
                return;
            }
            gameStatus.Interrupt.ActionName = ActionCode.strEndTurn;
        }
        /// <summary>
        /// 能上场的随从
        /// </summary>
        /// <returns></returns>
        private static int HasBattleMinion(ActionStatus gameStatus)
        {
            PrivateInfo PlaySelfInfo = gameStatus.AllRole.MyPrivateInfo;
            PublicInfo PlayInfo = gameStatus.AllRole.MyPublicInfo;
            if (PlayInfo.BattleField.MinionCount == SystemManager.MaxMinionCount) return -1;
            for (int i = 0; i < PlaySelfInfo.handCards.Count; i++)
            {
                var card = PlaySelfInfo.handCards[i];
                if (card.卡牌种类 == Card.CardBasicInfo.资源类型枚举.随从)
                {
                    if (card.使用成本 <= PlayInfo.crystal.CurrentRemainPoint)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
        /// <summary>
        /// 是否拥有可以攻击的随从
        /// </summary>
        /// <returns></returns>
        private static int HasAttackMinion(ActionStatus gameStatus)
        {
            PublicInfo PlayInfo = gameStatus.AllRole.MyPublicInfo;
            //能攻击的随从都攻击，优先攻击英雄
            for (int i = 0; i < PlayInfo.BattleField.MinionCount; i++)
            {
                if (PlayInfo.BattleField.BattleMinions[i].能否攻击)
                {
                    return i + 1;
                }
            }
            return -1;
        }
        /// <summary>
        /// 获得打击目标
        /// </summary>
        /// <returns></returns>
        private static int GetAttackTarget(ActionStatus gameStatus)
        {
            PublicInfo PlayInfo = gameStatus.AllRole.YourPublicInfo;
            for (int i = 0; i < PlayInfo.BattleField.MinionCount; i++)
            {
                if (PlayInfo.BattleField.BattleMinions[i].嘲讽特性) return i + 1;
            }
            return PlayInfo.BattleField.MinionCount;
        }
    }
}
