using Engine.Utility;
using System;
using System.Text;
using System.Xml.Serialization;
namespace Engine.Card
{
    [Serializable]
    public class HeroCard : CardBasicInfo
    {
        /// <summary>
        /// 英雄技能
        /// </summary>
        public string HeroSkillCardSN = string.Empty;
        /// <summary>
        /// 生命力
        /// </summary>
        public int LifePoint = 30;
        /// <summary>
        /// 护盾
        /// </summary>
        [XmlIgnore]
        public int ShieldPoint = 0;
        /// <summary>
        /// 武器
        /// </summary>
        [XmlIgnore]
        public WeaponCard Weapon = null;
        /// <summary>
        /// 当前奥秘数
        /// </summary>
        [XmlIgnore]
        public int SecretCount = 0;
        /// <summary>
        /// 战场位置
        /// </summary>
        public CardUtility.指定位置结构体 战场位置;
        /// <summary>
        /// 是否为冰冻状态
        /// </summary>
        public CardUtility.效果回合枚举 冰冻状态 = CardUtility.效果回合枚举.无效果;
        /// <summary>
        /// 英雄技能
        /// </summary>
        public SpellCard HeroSkill {
            get
            {
                return (SpellCard)CardUtility.GetCardInfoBySN(HeroSkillCardSN);
            }
        }
        /// <summary>
        /// 装备武器时候的剩余攻击次数
        /// </summary>
        public int RemainAttackTimes = 0;
        /// <summary>
        /// 某些效果带来的临时攻击力提升
        /// </summary>
        public int TempAttackPoint = 0;
        /// <summary>
        /// 英雄技能是否已经使用
        /// </summary>
        public bool IsUsedHeroAbility = true;

        /// <summary>
        /// 能否成为当前动作的对象
        /// </summary>
        public bool 能否成为动作对象 = false;
        /// <summary>
        /// 获得信息
        /// </summary>
        /// <returns></returns>
        public new string GetInfo()
        {
            StringBuilder Status = new StringBuilder();
            Status.AppendLine("Hero Info:");
            Status.AppendLine("HealthPoint：" + LifePoint);
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
}
