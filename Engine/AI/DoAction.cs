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
            foreach (var card in PlaySelfInfo.handCards)
            {
                if (card.CardType == Card.CardBasicInfo.CardTypeEnum.随从 &&
                    PlayInfo.BattleField.MinionCount != BattleFieldInfo.MaxMinionCount)
                {
                    if (card.使用成本 <= PlayInfo.crystal.CurrentRemainPoint)
                    {
                        int newPos = PlayInfo.BattleField.MinionCount + 1;
                        RunAction.StartAction(GameManager.gameStatus, card.序列号, false, new string[] { newPos.ToString() });
                        PlayInfo.crystal.CurrentRemainPoint -= card.使用成本;
                    }
                }
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
