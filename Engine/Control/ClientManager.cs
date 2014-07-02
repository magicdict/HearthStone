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
        public Boolean IsStart = false;
        /// <summary>
        /// 游戏玩家名称
        /// </summary>
        public Boolean IsHost;
        /// <summary>
        /// 是否为先手
        /// </summary>
        public Boolean IsFirst;
        /// <summary>
        /// 本方回合
        /// </summary>
        public Boolean IsMyTurn;
        /// <summary>
        /// 事件处理组件
        /// </summary>
        public Engine.Client.EventHandler 事件处理组件 = new Engine.Client.EventHandler();
        /// <summary>
        /// 游戏状态[本方]
        /// </summary>
        public ClientPlayerInfo myStatus = new ClientPlayerInfo();
        /// <summary>
        /// 游戏状态[对方]
        /// </summary>
        public PublicInfo yourBasicStatus = new PublicInfo();
        /// <summary>
        /// 当前卡牌
        /// </summary>
        public static CardBasicInfo CurrentActiveCard;
        /// <summary>
        /// 初始化
        /// </summary>
        public void InitPlayInfo()
        {
            myStatus.BasicInfo.crystal.CurrentFullPoint = 0;
            myStatus.BasicInfo.crystal.CurrentRemainPoint = 0;
            yourBasicStatus.crystal.CurrentFullPoint = 0;
            yourBasicStatus.crystal.CurrentRemainPoint = 0;

            myStatus.BasicInfo.战场位置 = new CardUtility.指定位置结构体() { 本方对方标识 = true, Postion = BattleFieldInfo.HeroPos };
            yourBasicStatus.战场位置 = new CardUtility.指定位置结构体() { 本方对方标识 = false, Postion = BattleFieldInfo.HeroPos };
            myStatus.BasicInfo.BattleField.本方对方标识 = true;
            yourBasicStatus.BattleField.本方对方标识 = false;
            //英雄技能：奥术飞弹
            myStatus.BasicInfo.HeroAbility = (Engine.Card.SpellCard)Engine.Utility.CardUtility.GetCardInfoBySN("A000056");
            yourBasicStatus.HeroAbility = (Engine.Card.SpellCard)Engine.Utility.CardUtility.GetCardInfoBySN("A000056");

            if (SystemManager.游戏模式 == SystemManager.GameMode.塔防)
            {
                yourBasicStatus.LifePoint = CardUtility.Max;
                myStatus.BasicInfo.crystal.CurrentFullPoint = 10;
                myStatus.BasicInfo.crystal.CurrentRemainPoint = 10;
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
                myStatus.SelfInfo.handCards.Add(CardUtility.GetCardInfoBySN(card));
            }
            if (IsFirst)
            {
                myStatus.BasicInfo.RemainCardDeckCount = CardDeck.MaxCards - 3;
                yourBasicStatus.RemainCardDeckCount = CardDeck.MaxCards - 4;
                myStatus.BasicInfo.HandCardCount = PublicInfo.BasicHandCardCount;
                yourBasicStatus.HandCardCount = PublicInfo.BasicHandCardCount + 1 + 1;
            }
            else
            {
                myStatus.SelfInfo.handCards.Add(CardUtility.GetCardInfoBySN(Engine.Card.SpellCard.SN幸运币));
                myStatus.BasicInfo.RemainCardDeckCount = CardDeck.MaxCards - 4;
                yourBasicStatus.RemainCardDeckCount = CardDeck.MaxCards - 3;
                myStatus.BasicInfo.HandCardCount = PublicInfo.BasicHandCardCount + 1 + 1;
                yourBasicStatus.HandCardCount = PublicInfo.BasicHandCardCount;
            }
        }
        /// <summary>
        /// 新的回合
        /// </summary>
        public void TurnStart(Boolean IsMyTurn)
        {
            PublicInfo PlayInfo = IsMyTurn ? myStatus.BasicInfo : yourBasicStatus;
            if (IsMyTurn) {
                myStatus.SelfInfo.handCards.Add(CardUtility.GetCardInfoBySN(ClientRequest.DrawCard(GameId.ToString(GameServer.GameIdFormat), IsHost, 1)[0]));
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
        public List<String> TurnEnd(Boolean IsMyTurn)
        {
            PublicInfo PlayInfo = IsMyTurn ? myStatus.BasicInfo : yourBasicStatus;
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
            PlayInfo.BattleField.ClearDead(myStatus, false);
            //ActionLst.AddRange(GameManager.Settle(gameStatus));
            ActionLst.AddRange(事件处理组件.事件处理(myStatus));
            return ActionLst;
        }
    }
}
