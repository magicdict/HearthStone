using Engine.Action;
using Engine.Client;
using Engine.Control;
using Engine.Server;
using Engine.Utility;
using System;
using System.Collections.Generic;

namespace Engine.AI
{
    public static class DoAction
    {
        public static List<String> Run(ActionStatus gameStatus)
        {
            PublicInfo PlayInfo = gameStatus.AllRole.YourPublicInfo;
            PrivateInfo PlaySelfInfo = gameStatus.AllRole.YourPrivateInfo;

            List<String> Result = new List<string>();
            //能上场的随从都上场
            int HandCardIndex = HasBattleMinion(gameStatus);
            while (HandCardIndex != -1)
            {
                int newPos = PlayInfo.BattleField.MinionCount + 1;
                var card = PlaySelfInfo.handCards[HandCardIndex];
                gameStatus.Interrupt.SessionData = newPos.ToString();
                RunAction.StartAction(gameStatus, card.序列号);
                PlayInfo.crystal.CurrentRemainPoint -= card.使用成本;
                PlaySelfInfo.RemoveUsedCard(card.序列号);
                PlayInfo.HandCardCount = PlaySelfInfo.handCards.Count;
                HandCardIndex = HasBattleMinion(gameStatus);
            }
            //能攻击的随从都攻击，优先攻击英雄
            int AttackPos = HasAttackMinion(gameStatus);
            while (AttackPos != -1)
            {
                int BeAttackedPos = GetAttackTarget(gameStatus);
                RunAction.Fight(gameStatus, AttackPos, BeAttackedPos, false);
                AttackPos = HasAttackMinion(gameStatus);
            }
            Result.Add(ActionCode.strEndTurn);
            return Result;
        }
        /// <summary>
        /// 能上场的随从
        /// </summary>
        /// <returns></returns>
        private static int HasBattleMinion(ActionStatus gameStatus)
        {
            PrivateInfo PlaySelfInfo = gameStatus.AllRole.YourPrivateInfo;
            PublicInfo PlayInfo = gameStatus.AllRole.YourPublicInfo;
            for (int i = 0; i < PlaySelfInfo.handCards.Count; i++)
            {
                var card = PlaySelfInfo.handCards[i];
                if (card.卡牌种类 == Card.CardBasicInfo.卡牌类型枚举.随从 &&
                    PlayInfo.BattleField.MinionCount != SystemManager.MaxMinionCount)
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
            PublicInfo PlayInfo = gameStatus.AllRole.YourPublicInfo;
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
            PublicInfo PlayInfo = gameStatus.AllRole.MyPublicInfo;
            for (int i = 0; i < PlayInfo.BattleField.MinionCount; i++)
            {
                if (PlayInfo.BattleField.BattleMinions[i].嘲讽特性) return i + 1;
            }
            return PlayInfo.BattleField.MinionCount;
        }
    }
}
