using Engine.Action;
using Engine.Card;
using Engine.Utility;
using System.Collections.Generic;
using System.Text;

namespace Engine.Client
{
    /// <summary>
    /// 公共情报
    /// </summary>
    public class PublicInfo
    {
        /// <summary>
        /// 战场位置
        /// </summary>
        public CardUtility.指定位置结构体 战场位置;
        /// <summary>
        /// 是否为冰冻状态
        /// </summary>
        public CardUtility.效果回合枚举 冰冻状态 = CardUtility.效果回合枚举.无效果;
        /// <summary>
        /// 生命力
        /// </summary>
        public int LifePoint = 30;
        /// <summary>
        /// 护盾
        /// </summary>
        public int ShieldPoint = 0;
        /// <summary>
        /// 水晶
        /// </summary>
        public Crystal crystal = new Crystal();
        /// <summary>
        /// 上回合过载
        /// </summary>
        public int OverloadPoint = 0;
        /// <summary>
        /// 武器
        /// </summary>
        public WeaponCard Weapon;
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
        /// 装备武器时候的剩余攻击次数
        /// </summary>
        public int RemainAttackTimes = 0;
        /// <summary>
        /// 某些效果带来的临时攻击力提升
        /// </summary>
        public int TempAttackPoint = 0;
        /// <summary>
        /// 英雄技能
        /// </summary>
        public SpellCard HeroAbility = new SpellCard();
        /// <summary>
        /// 英雄技能是否已经使用
        /// </summary>
        public bool IsUsedHeroAbility = true;
        /// <summary>
        /// 当前奥秘数
        /// </summary>
        public int SecretCount = 0;
        /// <summary>
        /// 是否连击
        /// </summary>
        public bool 连击状态 = false;
        /// <summary>
        /// 能否成为当前动作的对象
        /// </summary>
        public bool 能否成为动作对象 = false;
        /// <summary>
        /// 获得信息
        /// </summary>
        /// <returns></returns>
        public string GetInfo()
        {
            StringBuilder Status = new StringBuilder();
            Status.AppendLine("Hero Info:");
            Status.AppendLine("Crystal：" + crystal.CurrentRemainPoint + "/" + crystal.CurrentFullPoint);
            Status.AppendLine("HealthPoint：" + LifePoint);
            Status.AppendLine("RemainCardDeckCount：" + RemainCardDeckCount);
            return Status.ToString();
        }
        /// <summary>
        /// 实际输出效果
        /// </summary>
        /// <returns>包含了光环/激怒效果</returns>
        public int 实际攻击值
        {
            get
            {
                int rtnAttack = 0;
                if (Weapon != null && Weapon.耐久度 > 0) rtnAttack += Weapon.攻击力;
                rtnAttack += TempAttackPoint;
                return rtnAttack;
            }
        }
        /// <summary>
        /// 是否可用攻击
        /// </summary>
        /// <returns></returns>
        public bool IsAttackEnable(bool IsMyTurn)
        {
            return RemainAttackTimes != 0 && 实际攻击值 > 0 && IsMyTurn;
        }
        /// <summary>
        /// 英雄技能是否可用
        /// </summary>
        /// <returns></returns>
        public bool IsHeroAblityEnable(bool IsMyTurn)
        {
            return (!IsUsedHeroAbility) && crystal.CurrentRemainPoint >= HeroAbility.使用成本 && IsMyTurn;
        }
        /// <summary>
        /// 攻击
        /// </summary>
        /// <param name="AttackPoint"></param>
        public bool AfterBeAttack(int AttackPoint)
        {
            if (ShieldPoint > 0)
            {
                if (ShieldPoint >= AttackPoint)
                {
                    ShieldPoint -= AttackPoint;
                    return false;
                }
                else
                {
                    LifePoint -= (AttackPoint - ShieldPoint);
                    ShieldPoint = 0;
                    return true;
                }
            }
            else
            {
                LifePoint -= AttackPoint;
                return true;
            }
        }
        /// <summary>
        /// 治疗
        /// </summary>
        /// <param name="HealthPoint"></param>
        public bool AfterBeHealth(int HealthPoint)
        {
            if (LifePoint == SystemManager.MaxHealthPoint) return false;
            LifePoint += HealthPoint;
            if (LifePoint > SystemManager.MaxHealthPoint) LifePoint = SystemManager.MaxHealthPoint;
            return true;
        }
        /// <summary>
        /// 护甲
        /// </summary>
        /// <param name="PlusShieldPoint"></param>
        public bool AfterBeShield(int PlusShieldPoint)
        {
            ShieldPoint += PlusShieldPoint;
            return true;
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
