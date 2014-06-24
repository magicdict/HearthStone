using Engine.Client;
using Engine.Server;
using System;
using System.Collections.Generic;

namespace Engine.AI
{
    public static class DoAction
    {
        public static List<String> Run()
        {
            PublicInfo PlayInfo = GameManager.gameStatus.client.YourInfo;
            PrivateInfo PlaySelfInfo = GameManager.gameStatus.client.YourSelfInfo;

            List<String> Result = new List<string>();
            //能上场的随从都上场
            int HandCardIndex = HasBattleMinion();
            while (HandCardIndex != -1)
            {
                int newPos = PlayInfo.BattleField.MinionCount + 1;
                var card = PlaySelfInfo.handCards[HandCardIndex];
                RunAction.StartAction(GameManager.gameStatus, card.序列号, false, new string[] { newPos.ToString() });
                PlayInfo.crystal.CurrentRemainPoint -= card.使用成本;
                PlaySelfInfo.RemoveUsedCard(card.序列号);
                PlayInfo.HandCardCount = PlaySelfInfo.handCards.Count;
                HandCardIndex = HasBattleMinion();
            }
            //能攻击的随从都攻击，优先攻击英雄
            int AttackPos = HasAttackMinion();
            while (AttackPos != -1)
            {
                int BeAttackedPos = GetAttackTarget();
                RunAction.Fight(GameManager.gameStatus, AttackPos, BeAttackedPos, false);
                AttackPos = HasAttackMinion();
            }
            Result.Add(ActionCode.strEndTurn);
            return Result;
        }
        /// <summary>
        /// 能上场的随从
        /// </summary>
        /// <returns></returns>
        private static int HasBattleMinion()
        {
            PrivateInfo PlaySelfInfo = GameManager.gameStatus.client.YourSelfInfo;
            PublicInfo PlayInfo = GameManager.gameStatus.client.YourInfo;
            for (int i = 0; i < PlaySelfInfo.handCards.Count; i++)
            {
                var card = PlaySelfInfo.handCards[i];
                if (card.CardType == Card.CardBasicInfo.CardTypeEnum.随从 &&
                    PlayInfo.BattleField.MinionCount != BattleFieldInfo.MaxMinionCount)
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
        private static int HasAttackMinion()
        {
            PublicInfo PlayInfo = GameManager.gameStatus.client.YourInfo;
            //能攻击的随从都攻击，优先攻击英雄
            for (int i = 0; i < PlayInfo.BattleField.MinionCount; i++)
            {
                if (PlayInfo.BattleField.BattleMinions[i].CanAttack())
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
        private static int GetAttackTarget()
        {
            PublicInfo PlayInfo = GameManager.gameStatus.client.MyInfo;
            for (int i = 0; i < PlayInfo.BattleField.MinionCount; i++)
            {
                if (PlayInfo.BattleField.BattleMinions[i].嘲讽特性) return i + 1;
            }
            return PlayInfo.BattleField.MinionCount;
        }
    }
}
