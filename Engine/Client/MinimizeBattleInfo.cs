using Engine.Card;
using Engine.Action;

namespace Engine.Client
{
    public class MinimizeBattleInfo
    {
        /// <summary>
        /// 随从最小化数据
        /// </summary>
        public struct Minion
        {
            /// <summary>
            /// 名称
            /// </summary>
            public string 名称;
            /// <summary>
            /// 攻击力
            /// </summary>
            public int 攻击力;
            /// <summary>
            /// 生命力
            /// </summary>
            public int 生命值;
            /// <summary>
            /// 生命力
            /// </summary>
            public int 使用成本;
            /// <summary>
            /// 状态列表
            /// </summary>
            public string 状态列表;
            /// <summary>
            /// 能否攻击
            /// </summary>
            public bool 能否攻击;
            /// <summary>
            /// 描述
            /// </summary>
            public string 描述;
            /// <summary>
            /// InitByMinion
            /// </summary>
            /// <param name="minion"></param>
            public void Init(MinionCard minion)
            {
                名称 = minion.名称;
                攻击力 = minion.实际攻击值;
                生命值 = minion.生命值;
                使用成本 = minion.使用成本;
                状态列表 = minion.状态;
                能否攻击 = minion.能否攻击;
                描述 = minion.描述;
            }
        }
        /// <summary>
        /// 公开信息
        /// </summary>
        public struct PlayerInfo
        {
            /// <summary>
            /// 护盾值
            /// </summary>
            public int 护盾值;
            /// <summary>
            /// 生命力
            /// </summary>
            public int 生命力;
            /// <summary>
            /// 攻击力
            /// </summary>
            public int 攻击力;
            /// <summary>
            /// 可用水晶
            /// </summary>
            public int 可用水晶;
            /// <summary>
            /// 总体水晶
            /// </summary>
            public int 总体水晶;
            /// <summary>
            /// 英雄技能
            /// </summary>
            public string 英雄技能;
            /// <summary>
            /// 
            /// </summary>
            public string 英雄技能描述;
            /// <summary>
            /// 能否使用
            /// </summary>
            public bool 使用英雄技能;
            /// <summary>
            /// 是否可以攻击
            /// </summary>
            public bool 可以攻击;
            /// <summary>
            /// 初始化
            /// </summary>
            /// <param name="pubInfo"></param>
            public void Init(PublicInfo pubInfo)
            {
                攻击力 = pubInfo.Hero.实际攻击值;
                护盾值 = pubInfo.Hero.ShieldPoint;
                生命力 = pubInfo.Hero.LifePoint;
                可用水晶 = pubInfo.crystal.CurrentRemainPoint;
                总体水晶 = pubInfo.crystal.CurrentFullPoint;
                英雄技能 = pubInfo.Hero.HeroSkill.序列号;
                英雄技能描述 = pubInfo.Hero.HeroSkill.描述;
                使用英雄技能 = pubInfo.IsHeroSkillEnable(true);
                可以攻击 = pubInfo.Hero.IsAttackEnable(true);
            }
        }
        /// <summary>
        /// 手牌
        /// </summary>
        public struct HandCardInfo
        {
            /// <summary>
            /// 序列号
            /// </summary>
            public string 序列号;
            /// <summary>
            /// 名称
            /// </summary>
            public string 名称;
            /// <summary>
            /// 成本
            /// </summary>
            public int 使用成本;
            /// <summary>
            /// 攻击力
            /// </summary>
            public int 攻击力;
            /// <summary>
            /// 生命力
            /// </summary>
            public int 生命值;
            /// <summary>
            /// 状态列表
            /// </summary>
            public string 状态列表;
            /// <summary>
            /// 描述
            /// </summary>
            public string 描述;
            /// <summary>
            /// 初始化
            /// </summary>
            /// <param name="card"></param>
            public void Init(CardBasicInfo card)
            {
                序列号 = card.序列号;
                名称 = card.名称;
                使用成本 = card.使用成本;
                描述 = card.描述;
                switch (card.卡牌种类)
                {
                    case CardBasicInfo.资源类型枚举.随从:
                        攻击力 = ((MinionCard)card).攻击力;
                        生命值 = ((MinionCard)card).生命值;
                        break;
                    case CardBasicInfo.资源类型枚举.法术:
                        break;
                    case CardBasicInfo.资源类型枚举.武器:
                        攻击力 = ((WeaponCard)card).攻击力;
                        生命值 = ((WeaponCard)card).耐久度;
                        break;
                    case CardBasicInfo.资源类型枚举.奥秘:
                        break;
                    case CardBasicInfo.资源类型枚举.其他:
                        break;
                    default:
                        break;
                }
            }
        }
        public Minion[] MyBattle;
        public Minion[] YourBattle;
        public HandCardInfo[] HandCard;
        public PlayerInfo MyInfo = new PlayerInfo();
        public PlayerInfo YourInfo = new PlayerInfo();
        public void Init(ActionStatus status)
        {
            //ActionStatus在获取的过程中，已经知道IsHost信息，所以这里的无需做Host到My的转换了
            MyInfo.Init(status.AllRole.MyPublicInfo);
            YourInfo.Init(status.AllRole.YourPublicInfo);
            HandCard = new HandCardInfo[status.AllRole.MyPrivateInfo.handCards.Count];
            for (int i = 0; i < status.AllRole.MyPrivateInfo.handCards.Count; i++)
            {
                HandCardInfo t = new HandCardInfo();
                t.Init(status.AllRole.MyPrivateInfo.handCards[i]);
                HandCard[i] = t;
            }
            MyBattle = new Minion[status.AllRole.MyPublicInfo.BattleField.MinionCount];
            for (int i = 0; i < status.AllRole.MyPublicInfo.BattleField.MinionCount; i++)
            {
                Minion t = new Minion();
                t.Init(status.AllRole.MyPublicInfo.BattleField.BattleMinions[i]);
                MyBattle[i] = t;
            }
            YourBattle = new Minion[status.AllRole.YourPublicInfo.BattleField.MinionCount];
            for (int i = 0; i < status.AllRole.YourPublicInfo.BattleField.MinionCount; i++)
            {
                Minion t = new Minion();
                t.Init(status.AllRole.YourPublicInfo.BattleField.BattleMinions[i]);
                YourBattle[i] = t;
            }
        }
    }
}
