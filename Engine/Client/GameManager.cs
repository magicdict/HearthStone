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
        /// 游戏编号
        /// </summary>
        public static int GameId;
        /// <summary>
        /// 游戏类型
        /// </summary>
        public static SystemManager.GameType 游戏类型 = SystemManager.GameType.客户端服务器版;
        /// <summary>
        /// 游戏模式
        /// </summary>
        public static SystemManager.GameMode 游戏模式 = SystemManager.GameMode.标准;
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
        /// 游戏状态
        /// </summary>
        public static GameStatus gameStatus = new GameStatus();
        /// <summary>
        /// 单机版的模拟服务器
        /// </summary>
        public static RemoteGameManager SimulateServer;
        /// <summary>
        /// 全局随机种子
        /// </summary>
        public static int RandomSeed = 1;
        /// <summary>
        /// 
        /// </summary>
        public static CardBasicInfo CurrentActiveCard;
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
            if (gameStatus.client.IsHost)
            {
                gameStatus.client.MyInfo = gameStatus.client.HostInfo;
                gameStatus.client.YourInfo = gameStatus.client.GuestInfo;
                gameStatus.client.MySelfInfo = gameStatus.client.HostSelfInfo;
                gameStatus.client.YourSelfInfo = gameStatus.client.GuestSelfInfo;
            }
            else
            {
                gameStatus.client.MyInfo = gameStatus.client.GuestInfo;
                gameStatus.client.YourInfo = gameStatus.client.HostInfo;
                gameStatus.client.MySelfInfo = gameStatus.client.GuestSelfInfo;
                gameStatus.client.YourSelfInfo = gameStatus.client.HostSelfInfo;
            }
            gameStatus.client.HostInfo.crystal.CurrentFullPoint = 0;
            gameStatus.client.HostInfo.crystal.CurrentRemainPoint = 0;
            gameStatus.client.GuestInfo.crystal.CurrentFullPoint = 0;
            gameStatus.client.GuestInfo.crystal.CurrentRemainPoint = 0;
            gameStatus.client.MyInfo.战场位置 = new CardUtility.指定位置结构体() { 本方对方标识 = true, Postion = BattleFieldInfo.HeroPos };
            gameStatus.client.YourInfo.战场位置 = new CardUtility.指定位置结构体() { 本方对方标识 = false, Postion = BattleFieldInfo.HeroPos };
            gameStatus.client.MyInfo.BattleField.本方对方标识 = true;
            gameStatus.client.YourInfo.BattleField.本方对方标识 = false;
            //英雄技能：奥术飞弹
            gameStatus.client.MyInfo.HeroAbility = (Engine.Card.AbilityCard)Engine.Utility.CardUtility.GetCardInfoBySN("A000056");
            gameStatus.client.YourInfo.HeroAbility = (Engine.Card.AbilityCard)Engine.Utility.CardUtility.GetCardInfoBySN("A000056");

            if (游戏模式 == SystemManager.GameMode.塔防)
            {
                gameStatus.client.YourInfo.LifePoint = CardUtility.Max;
                gameStatus.client.HostInfo.crystal.CurrentFullPoint = 10;
                gameStatus.client.HostInfo.crystal.CurrentRemainPoint = 10;
            }
        }
        /// <summary>
        /// 初始化手牌(CS)
        /// </summary>
        /// <param name="IsHost"></param>
        public static void InitHandCard()
        {
            int DrawCardCnt = gameStatus.client.IsFirst ? PublicInfo.BasicHandCardCount : (PublicInfo.BasicHandCardCount + 1);
            foreach (var card in Engine.Client.ClientRequest.DrawCard(GameId.ToString(GameServer.GameIdFormat), gameStatus.client.IsFirst, DrawCardCnt))
            {
                gameStatus.client.MySelfInfo.handCards.Add(CardUtility.GetCardInfoBySN(card));
            }
            if (gameStatus.client.IsFirst)
            {
                gameStatus.client.MyInfo.RemainCardDeckCount = Engine.Client.CardDeck.MaxCards - 3;
                gameStatus.client.YourInfo.RemainCardDeckCount = Engine.Client.CardDeck.MaxCards - 4;
                gameStatus.client.MyInfo.HandCardCount = PublicInfo.BasicHandCardCount;
                gameStatus.client.YourInfo.HandCardCount = PublicInfo.BasicHandCardCount + 1 + 1;
            }
            else
            {
                gameStatus.client.MySelfInfo.handCards.Add(CardUtility.GetCardInfoBySN(Engine.Card.AbilityCard.SN幸运币));
                gameStatus.client.MyInfo.RemainCardDeckCount = Engine.Client.CardDeck.MaxCards - 4;
                gameStatus.client.YourInfo.RemainCardDeckCount = Engine.Client.CardDeck.MaxCards - 3;
                gameStatus.client.MyInfo.HandCardCount = PublicInfo.BasicHandCardCount + 1 + 1;
                gameStatus.client.YourInfo.HandCardCount = PublicInfo.BasicHandCardCount;
            }
        }
        /// <summary>
        /// 初始化手牌(Single)
        /// </summary>
        /// <param name="IsHost"></param>
        public static void InitHandCard(Boolean IsHost)
        {
            if (!IsHost && 游戏模式 == SystemManager.GameMode.塔防) return;

            int DrawCardCnt = SimulateServer.serverinfo.IsFirst(IsHost) ? PublicInfo.BasicHandCardCount : (PublicInfo.BasicHandCardCount + 1);
            if (IsHost)
            {
                foreach (var card in SimulateServer.DrawCard(IsHost, DrawCardCnt))
                {
                    gameStatus.client.HostSelfInfo.handCards.Add(CardUtility.GetCardInfoBySN(card));
                    gameStatus.client.HostInfo.HandCardCount++;
                    gameStatus.client.HostInfo.RemainCardDeckCount--;
                }
                if (!SimulateServer.serverinfo.IsFirst(IsHost))
                {
                    gameStatus.client.HostSelfInfo.handCards.Add(CardUtility.GetCardInfoBySN(Engine.Card.AbilityCard.SN幸运币));
                    gameStatus.client.HostInfo.HandCardCount++;
                }
            }
            else
            {
                foreach (var card in SimulateServer.DrawCard(IsHost, DrawCardCnt))
                {
                    gameStatus.client.GuestSelfInfo.handCards.Add(CardUtility.GetCardInfoBySN(card));
                    gameStatus.client.GuestInfo.HandCardCount++;
                    gameStatus.client.GuestInfo.RemainCardDeckCount--;
                }
                if (!SimulateServer.serverinfo.IsFirst(IsHost))
                {
                    gameStatus.client.GuestSelfInfo.handCards.Add(CardUtility.GetCardInfoBySN(Engine.Card.AbilityCard.SN幸运币));
                    gameStatus.client.GuestInfo.HandCardCount++;
                }
            }
        }
        /// <summary>
        /// 新的回合
        /// </summary>
        public static void TurnStart(Boolean IsMyTurn)
        {
            PublicInfo PlayInfo = IsMyTurn ? gameStatus.client.MyInfo : gameStatus.client.YourInfo;
            PrivateInfo SelfInfo = IsMyTurn ? gameStatus.client.MySelfInfo : gameStatus.client.YourSelfInfo;
            //抽一张手牌
            switch (游戏类型)
            {
                case SystemManager.GameType.单机版:
                    if (游戏模式 == SystemManager.GameMode.塔防)
                    {
                        if (!IsMyTurn)
                        {
                            for (int i = 0; i < SystemManager.MaxHandCardCount - PlayInfo.HandCardCount; i++)
                            {
                                var t = SimulateServer.DrawCard(IsMyTurn, 1);
                                if (t.Count == 1)
                                {
                                    SelfInfo.handCards.Add(CardUtility.GetCardInfoBySN(t[0]));
                                    PlayInfo.HandCardCount++;
                                }
                            }
                        }
                        else
                        {
                            if (SelfInfo.handCards.Count < SystemManager.MaxHandCardCount)
                            {
                                var t = SimulateServer.DrawCard(IsMyTurn, 1);
                                if (t.Count == 1) SelfInfo.handCards.Add(CardUtility.GetCardInfoBySN(t[0]));
                                PlayInfo.HandCardCount++;
                            }
                        }
                    }
                    else
                    {
                        if (SelfInfo.handCards.Count < SystemManager.MaxHandCardCount)
                        {
                            var t = SimulateServer.DrawCard(IsMyTurn, 1);
                            if (t.Count == 1) SelfInfo.handCards.Add(CardUtility.GetCardInfoBySN(t[0]));
                            PlayInfo.HandCardCount++;
                        }
                    }
                    break;
                case SystemManager.GameType.客户端服务器版:
                    SelfInfo.handCards.Add(CardUtility.GetCardInfoBySN(Engine.Client.ClientRequest.DrawCard(GameId.ToString(GameServer.GameIdFormat), gameStatus.client.IsFirst, 1)[0]));
                    break;
                case SystemManager.GameType.HTML版:
                    break;
                default:
                    break;
            }
            //过载的清算
            if (PlayInfo.OverloadPoint != 0)
            {
                PlayInfo.crystal.ReduceCurrentPoint(PlayInfo.OverloadPoint);
                PlayInfo.OverloadPoint = 0;
            }
            //连击的重置
            PlayInfo.连击状态 = false;
            //魔法水晶的增加
            PlayInfo.crystal.NewTurn();
            PlayInfo.RemainAttactTimes = 1;
            PlayInfo.IsUsedHeroAbility = false;
            PlayInfo.BattleField.FreezeStatus();
            //重置攻击次数,必须放在状态变化之后！
            //原因是剩余攻击回数和状态有关！
            foreach (var minion in PlayInfo.BattleField.BattleMinions)
            {
                if (minion != null) minion.重置剩余攻击次数();
            }
        }
        /// <summary>
        /// 对手回合结束的清场
        /// </summary>
        public static List<String> TurnEnd(Boolean IsMyTurn)
        {
            PublicInfo PlayInfo = IsMyTurn ? gameStatus.client.MyInfo : gameStatus.client.YourInfo;
            List<String> ActionLst = new List<string>();
            //对手回合加成属性的去除
            int ExistMinionCount = PlayInfo.BattleField.MinionCount;
            for (int i = 0; i < ExistMinionCount; i++)
            {
                if (PlayInfo.BattleField.BattleMinions[i] != null)
                {
                    PlayInfo.BattleField.BattleMinions[i].本回合生命力加成 = 0;
                    PlayInfo.BattleField.BattleMinions[i].本回合攻击力加成 = 0;
                    if (PlayInfo.BattleField.BattleMinions[i].特殊效果 == MinionCard.特殊效果枚举.回合结束死亡)
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
