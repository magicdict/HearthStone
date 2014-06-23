using Engine.Card;
using Engine.Server;
using Engine.Utility;
using System;
using System.Collections.Generic;

namespace Engine.Client
{
    public static class GameManager
    {
        /// <summary>
        /// 是否开始运行
        /// </summary>
        public static Boolean IsStart = false;
        /// <summary>
        /// 获得目标对象
        /// </summary>
        public static Engine.Utility.CardUtility.deleteGetTargetPosition GetSelectTarget;
        /// <summary>
        /// 抉择卡牌
        /// </summary>
        public static Engine.Utility.CardUtility.delegatePickEffect PickEffect;
        /// <summary>
        /// 
        /// </summary>
        public static GameStatus gameStatus = new GameStatus();
        /// <summary>
        /// 全局随机种子
        /// </summary>
        public static int RandomSeed = 1;
        /// <summary>
        /// 事件处理组件
        /// </summary>
        public static EventHandler 事件处理组件 = new EventHandler();
        /// <summary>
        /// 初始化
        /// </summary>
        public static void InitPlayInfo()
        {
            gameStatus.client.Init();
            gameStatus.server.Init();
            if (gameStatus.client.IsHost)
            {
                gameStatus.client.MyInfo = gameStatus.client.HostInfo;
                gameStatus.client.YourInfo = gameStatus.client.GuestInfo;
            }
            else
            {
                gameStatus.client.MyInfo = gameStatus.client.GuestInfo;
                gameStatus.client.YourInfo = gameStatus.client.HostInfo;
            }
            gameStatus.client.HostInfo.crystal.CurrentFullPoint = 0;
            gameStatus.client.HostInfo.crystal.CurrentRemainPoint = 0;
            gameStatus.client.GuestInfo.crystal.CurrentFullPoint = 0;
            gameStatus.client.GuestInfo.crystal.CurrentRemainPoint = 0;
            gameStatus.client.MyInfo.战场位置 = new CardUtility.TargetPosition() { 本方对方标识 = true, Postion = BattleFieldInfo.HeroPos };
            gameStatus.client.YourInfo.战场位置 = new CardUtility.TargetPosition() { 本方对方标识 = false, Postion = BattleFieldInfo.HeroPos };
            gameStatus.client.MyInfo.BattleField.本方对方标识 = true;
            gameStatus.client.YourInfo.BattleField.本方对方标识 = false;
            //英雄技能：奥术飞弹
            gameStatus.client.MyInfo.HeroAbility = (Engine.Card.AbilityCard)Engine.Utility.CardUtility.GetCardInfoBySN("A000056");
            gameStatus.client.YourInfo.HeroAbility = (Engine.Card.AbilityCard)Engine.Utility.CardUtility.GetCardInfoBySN("A000056");
        }
        /// <summary>
        /// 从服务器获得初始手牌
        /// </summary>
        public static void 获得初始手牌()
        {
            var HandCard = Engine.Client.ClientRequest.DrawCard(gameStatus.GameId.ToString(GameServer.GameIdFormat), gameStatus.client.IsFirst,
                           gameStatus.client.IsFirst ? PublicInfo.BasicHandCardCount : (PublicInfo.BasicHandCardCount + 1));
            foreach (var card in HandCard)
            {
                gameStatus.client.HostSelfInfo.handCards.Add(CardUtility.GetCardInfoBySN(card));
            }
            gameStatus.client.HostInfo.HandCardCount = HandCard.Count;
        }
        /// <summary>
        /// 初始化手牌
        /// </summary>
        /// <param name="IsFirst"></param>
        public static void InitHandCard(Boolean IsFirst)
        {
            if (IsFirst)
            {
                gameStatus.client.HostInfo.RemainCardDeckCount = Engine.Client.CardDeck.MaxCards - 3;
                gameStatus.client.GuestInfo.RemainCardDeckCount = Engine.Client.CardDeck.MaxCards - 4;
            }
            else
            {
                gameStatus.client.HostSelfInfo.handCards.Add(CardUtility.GetCardInfoBySN(Engine.Card.AbilityCard.SN幸运币));
                gameStatus.client.HostInfo.RemainCardDeckCount = Engine.Client.CardDeck.MaxCards - 4;
                gameStatus.client.GuestInfo.RemainCardDeckCount = Engine.Client.CardDeck.MaxCards - 3;
            }
        }
        /// <summary>
        /// 新的回合
        /// </summary>
        public static void TurnStart(PublicInfo PlayInfo)
        {
            //过载的清算
            if (PlayInfo.OverloadPoint != 0)
            {
                PlayInfo.crystal.ReduceCurrentPoint(PlayInfo.OverloadPoint);
                PlayInfo.OverloadPoint = 0;
            }
            //连击的重置
            gameStatus.client.HostInfo.连击状态 = false;
            //魔法水晶的增加
            gameStatus.client.HostInfo.crystal.NewTurn();
            PlayInfo.RemainAttactTimes = 1;
            PlayInfo.IsUsedHeroAbility = false;
            PlayInfo.BattleField.FreezeStatus();
            //重置攻击次数,必须放在状态变化之后！
            //原因是剩余攻击回数和状态有关！
            foreach (var minion in PlayInfo.BattleField.BattleMinions)
            {
                if (minion != null) minion.ResetAttackTimes();
            }
        }
        /// <summary>
        /// 对手回合结束的清场
        /// </summary>
        public static List<String> TurnEnd(PublicInfo PlayInfo)
        {
            List<String> ActionLst = new List<string>();
            //对手回合加成属性的去除
            int ExistMinionCount = PlayInfo.BattleField.MinionCount;
            for (int i = 0; i < ExistMinionCount; i++)
            {
                if (PlayInfo.BattleField.BattleMinions[i] != null)
                {
                    PlayInfo.BattleField.BattleMinions[i].本回合生命力加成 = 0;
                    PlayInfo.BattleField.BattleMinions[i].本回合攻击力加成 = 0;
                    if (PlayInfo.BattleField.BattleMinions[i].特殊效果 == MinionCard.特殊效果列表.回合结束死亡)
                    {
                        PlayInfo.BattleField.BattleMinions[i] = null;
                    }
                }
            }
            PlayInfo.BattleField.ClearDead(gameStatus, false);
            ActionLst.AddRange(Settle());
            ActionLst.AddRange(事件处理组件.事件处理(gameStatus));
            return ActionLst;
        }
        /// <summary>
        /// 清算(核心方法)
        /// </summary>
        /// <returns></returns>
        public static List<String> Settle()
        {
            //每次原子操作后进行一次清算
            //将亡语效果也发送给对方
            List<String> actionlst = new List<string>();
            //1.检查需要移除的对象
            var MyDeadMinion = gameStatus.client.HostInfo.BattleField.ClearDead(gameStatus, true);
            var YourDeadMinion = gameStatus.client.GuestInfo.BattleField.ClearDead(gameStatus, false);
            //2.重新计算Buff
            gameStatus.client.HostInfo.BattleField.ResetBuff();
            gameStatus.client.GuestInfo.BattleField.ResetBuff();
            //3.武器的移除
            if (gameStatus.client.HostInfo.Weapon != null && gameStatus.client.HostInfo.Weapon.耐久度 == 0) gameStatus.client.HostInfo.Weapon = null;
            if (gameStatus.client.GuestInfo.Weapon != null && gameStatus.client.GuestInfo.Weapon.耐久度 == 0) gameStatus.client.GuestInfo.Weapon = null;
            //发送结算同步信息
            actionlst.Add(Server.ActionCode.strSettle);
            foreach (var minion in MyDeadMinion)
            {
                //亡语的时候，本方无需倒置方向
                actionlst.AddRange(minion.发动亡语(gameStatus, false));
            }
            foreach (var minion in YourDeadMinion)
            {
                //亡语的时候，对方需要倒置方向
                //例如，亡语为 本方召唤一个随从，敌人亡语，变为敌方召唤一个随从
                actionlst.AddRange(minion.发动亡语(gameStatus, true));
            }
            return actionlst;
        }
    }
}
