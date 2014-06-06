using System;
using System.Collections.Generic;
using System.Text;

namespace Card.Client
{
    /// <summary>
    /// 共通情报
    /// </summary>
    public class PublicInfo
    {
        /// <summary>
        /// 最大生命值
        /// </summary>
        public const int MaxHealthPoint = 30;
        /// <summary>
        /// 是否为先手
        /// </summary>
        public Boolean IsFirst;
        /// <summary>
        /// 是否为冰冻状态
        /// </summary>
        public Card.CardUtility.EffectTurn 冰冻状态 = CardUtility.EffectTurn.无效果;
        /// <summary>
        /// 生命力
        /// </summary>
        public int HealthPoint = 30;
        /// <summary>
        /// 护盾
        /// </summary>
        public int ShieldPoint = 0;
        /// <summary>
        /// 水晶
        /// </summary>
        public Crystal crystal = new Crystal();
        /// <summary>
        /// 武器
        /// </summary>
        public Card.WeaponCard Weapon;
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
        /// 最大手牌数
        /// </summary>
        public const int MaxHandCardCount = 10;
        /// <summary>
        /// 手牌数
        /// </summary>
        public int HandCardCount = 0;
        /// <summary>
        /// 装备武器时候的剩余攻击次数
        /// </summary>
        public int RemainAttactTimes = 0;
        /// <summary>
        /// 英雄技能
        /// </summary>
        public AbilityCard HeroAbility = new AbilityCard();
        /// <summary>
        /// 
        /// </summary>
        public Boolean IsUsedHeroAbility = true;
        /// <summary>
        /// 当前奥秘数
        /// </summary>
        public int SecretCount = 0;
        /// <summary>
        /// 是否连击
        /// </summary>
        public Boolean 连击状态 = false;
        /// <summary>
        /// 获得信息
        /// </summary>
        /// <returns></returns>
        public string GetInfo()
        {
            StringBuilder Status = new StringBuilder();
            Status.AppendLine("Hero Info:");
            Status.AppendLine("Crystal：" + crystal.CurrentRemainPoint + "/" + crystal.CurrentFullPoint);
            Status.AppendLine("HealthPoint：" + HealthPoint);
            Status.AppendLine("RemainCardDeckCount：" + RemainCardDeckCount);
            return Status.ToString();
        }
        /// <summary>
        /// 遇到攻击
        /// </summary>
        /// <param name="AttackPoint"></param>
        public void AfterBeAttack(int AttackPoint)
        {
            if (ShieldPoint > 0)
            {
                if (ShieldPoint >= AttackPoint)
                {
                    ShieldPoint -= AttackPoint;
                }
                else
                {
                    HealthPoint -= (AttackPoint - ShieldPoint);
                    ShieldPoint = 0;
                }
            }
            else
            {
                HealthPoint -= AttackPoint;
            }
        }
        /// <summary>
        /// 遇到攻击
        /// </summary>
        /// <param name="HealthPoint"></param>
        public void AfterBeHealth(int HealthPoint)
        {
            HealthPoint += HealthPoint;
            if (HealthPoint > PublicInfo.MaxHealthPoint) HealthPoint = PublicInfo.MaxHealthPoint;
        }
    }
    /// <summary>
    /// 本方情报
    /// </summary>
    public class PrivateInfo
    {
        /// <summary>
        /// 手牌
        /// </summary>
        public List<Card.CardBasicInfo> handCards = new List<Card.CardBasicInfo>();
        /// <summary>
        /// 奥秘
        /// </summary>
        public List<SecretCard> 奥秘列表 = new List<SecretCard>();
        /// <summary>
        /// 手牌消耗重置
        /// </summary>
        public void ResetHandCardCost(GameManager game)
        {
            foreach (var card in handCards)
            {
                if (card.CardType == CardBasicInfo.CardTypeEnum.法术)
                {
                    card.ActualCostPoint = card.StandardCostPoint + game.MyInfo.BattleField.AbilityCost;
                    if (card.ActualCostPoint < 0) card.ActualCostPoint = 0;
                }
            }
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
