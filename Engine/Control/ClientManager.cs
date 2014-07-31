using Engine.Action;
using Engine.Card;
using Engine.Client;
using Engine.Server;
using Engine.Utility;
using System;
using System.Collections.Generic;

namespace Engine.Control
{
    public class ClientManager
    {
        /// <summary>
        /// 游戏编号
        /// </summary>
        public int GameId;
        /// <summary>
        /// 是否开始运行
        /// </summary>
        public bool IsStart = false;
        /// <summary>
        /// 游戏玩家名称
        /// </summary>
        public bool IsHost;
        /// <summary>
        /// 是否为先手
        /// </summary>
        public bool IsFirst;
        /// <summary>
        /// 本方回合
        /// </summary>
        public bool IsMyTurn;
        /// <summary>
        /// 
        /// </summary>
        public string PlayerNickName;
        /// <summary>
        /// 事件处理组件
        /// </summary>
        public BattleEventHandler 事件处理组件 = new BattleEventHandler();
        /// <summary>
        /// 游戏状态
        /// Client端已经原生将本方对方设定好了
        /// </summary>
        public ActionStatus actionStatus = new ActionStatus();
        /// <summary>
        /// 当前卡牌
        /// </summary>
        public CardBasicInfo CurrentActiveCard;
        /// <summary>
        /// 初始化
        /// </summary>
        public void InitPlayInfo()
        {
            actionStatus.AllRole.MyPublicInfo.crystal.CurrentFullPoint = 0;
            actionStatus.AllRole.MyPublicInfo.crystal.CurrentRemainPoint = 0;
            actionStatus.AllRole.YourPublicInfo.crystal.CurrentFullPoint = 0;
            actionStatus.AllRole.YourPublicInfo.crystal.CurrentRemainPoint = 0;

            actionStatus.AllRole.MyPublicInfo.战场位置 = new CardUtility.指定位置结构体() { 本方对方标识 = true, 位置 = BattleFieldInfo.HeroPos };
            actionStatus.AllRole.YourPublicInfo.战场位置 = new CardUtility.指定位置结构体() { 本方对方标识 = false, 位置 = BattleFieldInfo.HeroPos };
            actionStatus.AllRole.MyPublicInfo.BattleField.本方对方标识 = true;
            actionStatus.AllRole.YourPublicInfo.BattleField.本方对方标识 = false;
            //英雄技能：奥术飞弹
            actionStatus.AllRole.MyPublicInfo.HeroAbility = (SpellCard)CardUtility.GetCardInfoBySN("A000056");
            actionStatus.AllRole.YourPublicInfo.HeroAbility = (SpellCard)CardUtility.GetCardInfoBySN("A000056");

            if (SystemManager.游戏模式 == SystemManager.GameMode.塔防)
            {
                actionStatus.AllRole.YourPublicInfo.LifePoint = CardUtility.Max;
                actionStatus.AllRole.MyPublicInfo.crystal.CurrentFullPoint = 10;
                actionStatus.AllRole.MyPublicInfo.crystal.CurrentRemainPoint = 10;
            }
        }
        /// <summary>
        /// 初始化手牌(CS)
        /// </summary>
        /// <param name="IsHost"></param>
        public void InitHandCard()
        {
            int DrawCardCnt = IsFirst ? PublicInfo.BasicHandCardCount : (PublicInfo.BasicHandCardCount + 1);
            foreach (var card in ClientRequest.DrawCard(GameId.ToString(GameServer.GameIdFormat), IsHost, DrawCardCnt))
            {
                actionStatus.AllRole.MyPrivateInfo.handCards.Add(CardUtility.GetCardInfoBySN(card));
            }
            if (IsFirst)
            {
                actionStatus.AllRole.MyPublicInfo.RemainCardDeckCount = CardDeck.MaxCards - 3;
                actionStatus.AllRole.YourPublicInfo.RemainCardDeckCount = CardDeck.MaxCards - 4;
                actionStatus.AllRole.MyPublicInfo.HandCardCount = PublicInfo.BasicHandCardCount;
                actionStatus.AllRole.YourPublicInfo.HandCardCount = PublicInfo.BasicHandCardCount + 1 + 1;
            }
            else
            {
                actionStatus.AllRole.MyPrivateInfo.handCards.Add(CardUtility.GetCardInfoBySN(SpellCard.SN幸运币));
                actionStatus.AllRole.MyPublicInfo.RemainCardDeckCount = CardDeck.MaxCards - 4;
                actionStatus.AllRole.YourPublicInfo.RemainCardDeckCount = CardDeck.MaxCards - 3;
                actionStatus.AllRole.MyPublicInfo.HandCardCount = PublicInfo.BasicHandCardCount + 1 + 1;
                actionStatus.AllRole.YourPublicInfo.HandCardCount = PublicInfo.BasicHandCardCount;
            }
        }
        /// <summary>
        /// 新的回合
        /// </summary>
        public void TurnStart(bool IsMyTurn)
        {
            PublicInfo PlayInfo = IsMyTurn ? actionStatus.AllRole.MyPublicInfo : actionStatus.AllRole.YourPublicInfo;
            if (IsMyTurn)
            {
                actionStatus.AllRole.MyPrivateInfo.handCards.Add(CardUtility.GetCardInfoBySN(ClientRequest.DrawCard(GameId.ToString(GameServer.GameIdFormat), IsHost, 1)[0]));
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
            PlayInfo.RemainAttackTimes = 1;
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
        public List<string> TurnEnd(bool IsMyTurn)
        {
            PublicInfo PlayInfo = IsMyTurn ? actionStatus.AllRole.MyPublicInfo : actionStatus.AllRole.YourPublicInfo;
            List<string> ActionLst = new List<string>();
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
            PlayInfo.BattleField.ClearDead(事件处理组件, false);
            ActionLst.AddRange(ActionStatus.Settle(actionStatus));
            ActionLst.AddRange(事件处理组件.事件处理(actionStatus));
            return ActionLst;
        }
    }
}
