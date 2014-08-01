using Engine.Action;
using Engine.Card;
using System.Collections.Generic;

namespace Engine.Client
{
    /// <summary>
    /// 公共情报
    /// </summary>
    public class PublicInfo
    {
        /// <summary>
        /// 英雄
        /// </summary>
        public HeroCard Hero = new HeroCard();
        /// <summary>
        /// 水晶
        /// </summary>
        public Crystal crystal = new Crystal();
        /// <summary>
        /// 是否连击
        /// </summary>
        public bool 连击状态 = false;
        /// <summary>
        /// 上回合过载
        /// </summary>
        public int OverloadPoint = 0;
        /// <summary>
        /// 战场信息
        /// </summary>
        public BattleFieldInfo BattleField = new BattleFieldInfo();
        /// <summary>
        /// 剩余牌数
        /// </summary>
        public int RemainCardDeckCount = 30;
        /// <summary>
        /// 初始手牌数
        /// </summary>
        public const int BasicHandCardCount = 3;
        /// <summary>
        /// 手牌数
        /// </summary>
        public int HandCardCount = 0;
        /// <summary>
        /// 英雄技能是否可用
        /// </summary>
        /// <returns></returns>
        public bool IsHeroSkillEnable(bool IsMyTurn)
        {
            return (!Hero.IsUsedHeroAbility) && crystal.CurrentRemainPoint >= Hero.HeroSkill.使用成本 && IsMyTurn;
        }
    }
    /// <summary>
    /// 私有情报
    /// </summary>
    public class PrivateInfo
    {
        /// <summary>
        /// 手牌
        /// </summary>
        public List<CardBasicInfo> handCards = new List<CardBasicInfo>();
        /// <summary>
        /// 奥秘
        /// </summary>
        public List<SecretCard> 奥秘列表 = new List<SecretCard>();
        /// <summary>
        /// 手牌消耗重置
        /// </summary>
        public void ResetHandCardCost(ActionStatus game)
        {
            foreach (var card in handCards)
            {
                if (card.卡牌种类 == CardBasicInfo.卡牌类型枚举.法术)
                {
                    card.使用成本 = card.使用成本 + game.AllRole.YourPublicInfo.BattleField.AbilityCost;
                    if (card.使用成本 < 0) card.使用成本 = 0;
                }
            }
        }
        /// <summary>
        /// 去掉使用过的手牌
        /// </summary>
        /// <param name="CardSn"></param>
        public void RemoveUsedCard(string CardSn)
        {
            CardBasicInfo removeCard = new CardBasicInfo();
            foreach (var Seekcard in handCards)
            {
                if (Seekcard.序列号 == CardSn)
                {
                    removeCard = Seekcard;
                }
            }
            handCards.Remove(removeCard);
        }
        /// <summary>
        /// 清除命中奥秘
        /// </summary>
        public void 清除命中奥秘()
        {
            List<SecretCard> 奥秘副本 = new List<SecretCard>();
            foreach (var item in 奥秘列表)
            {
                if (!item.IsHitted) 奥秘副本.Add(item);
            }
            奥秘列表 = 奥秘副本;
        }
    }
}
