using System;
using System.Collections.Generic;
using System.Text;

namespace Card.Client
{
    /// <summary>
    /// 共通情报
    /// </summary>
    public class PlayerBasicInfo
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
        /// 护盾点数
        /// </summary>
        public int Sheild = 0;
        /// <summary>
        /// 生命力
        /// </summary>
        public int HealthPoint = 30;
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
        /// 手牌数
        /// </summary>
        public int HandCardCount = 0;
        /// <summary>
        /// 装备武器时候的剩余攻击次数
        /// </summary>
        public int RemainAttackCount = 0;
        /// <summary>
        /// 英雄技能
        /// </summary>
        public AbilityCard HeroAbility = new AbilityCard();
        /// <summary>
        /// 
        /// </summary>
        public Boolean IsUsedHeroAbility = true;
        /// <summary>
        /// 奥秘数
        /// </summary>
        public int SecretCount = 0;
        /// <summary>
        /// 
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
    }
    /// <summary>
    /// 本方情报
    /// </summary>
    public class PlayerDetailInfo
    {
        /// <summary>
        /// 基本情报
        /// </summary>
        public PlayerBasicInfo RoleInfo = new PlayerBasicInfo();
        /// <summary>
        /// 手牌
        /// </summary>
        public List<Card.CardBasicInfo> handCards = new List<Card.CardBasicInfo>();
        /// <summary>
        /// 奥秘
        /// </summary>
        public List<SecretCard> 奥秘 = new List<SecretCard>();
    }
}
