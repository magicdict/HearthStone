using Engine.Card;
using Engine.Utility;
using System.Collections.Generic;

namespace Engine.Client
{
    /// <summary>
    /// 战场信息（炉石专用）
    /// </summary>
    public class BattleFieldInfo
    {
        /// <summary>
        /// 未知
        /// </summary>
        public const int UnknowPos = -1;
        /// <summary>
        /// 英雄
        /// </summary>
        public const int HeroPos = 0;
        /// <summary>
        /// 全体随从
        /// </summary>
        public const int AllMinionPos = 8;
        /// <summary>
        /// 全体角色
        /// </summary>
        public const int AllRolePos = 9;
        /// <summary>
        /// 法术消耗
        /// </summary>
        public int AbilityCost = 0;
        /// <summary>
        /// 随从消耗
        /// </summary>
        public int MinionCost = 0;
        /// <summary>
        /// 法术效果
        /// 多次伤害法术，则施法次数 + 1
        /// 单次伤害法术，则施法强度 + 1
        /// </summary>
        public int AbilityDamagePlus = 0;
        /// <summary>
        /// 方向标志
        /// </summary>
        public bool 本方对方标识;
        /// <summary>
        /// 当前随从数量
        /// </summary>
        public int MinionCount
        {
            get
            {
                int t = 0;
                for (int i = 0; i < SystemManager.MaxMinionCount; i++)
                {
                    if (BattleMinions[i] != null) t++;
                }
                return t;
            }
        }
        /// <summary>
        /// 随从位置
        /// </summary>
        ///<remarks>
        ///7个位置的注意事项
        ///[0][1][2][3][4][5][6]
        ///有些卡牌涉及到对于左右位置的加成问题，所以，位置是很敏感的
        ///</remarks>
        public MinionCard[] BattleMinions = new MinionCard[SystemManager.MaxMinionCount];
        /// <summary>
        /// 卡牌入战场
        /// </summary>
        /// <param name="CardSn"></param>
        public void AppendToBattle(string CardSn)
        {
            int Position = MinionCount + 1;
            PutToBattle(Position, CardSn);
        }
        /// <summary>
        /// 卡牌入战场
        /// </summary>
        /// <param name="CardSn"></param>
        public void AppendToBattle(MinionCard Minion)
        {
            int Position = MinionCount + 1;
            PutToBattle(Position, Minion);
        }
        /// <summary>
        /// 卡牌入战场
        /// </summary>
        /// <param name="Position"></param>
        /// <param name="CardSn"></param>
        public void PutToBattle(int Position, string CardSn)
        {
            CardBasicInfo card = CardUtility.GetCardInfoBySN(CardSn);
            PutToBattle(Position, (MinionCard)card);
        }
        /// <summary>
        /// 卡牌入战场
        /// </summary>
        /// <param name="Position">从1开始的位置</param>
        /// <param name="Minion">随从</param>
        /// <remarks>不涉及到战吼等计算</remarks>
        public void PutToBattle(int Position, MinionCard Minion)
        {
            //战场满了
            if (MinionCount == SystemManager.MaxMinionCount) return;
            //无效的位置
            if ((Position < 1) || (Position > MinionCount + 1) || Position > SystemManager.MaxMinionCount) return;
            //插入操作
            if (BattleMinions[Position - 1] == null)
            {
                //添加到最右边
                BattleMinions[Position - 1] = Minion;
            }
            else
            {
                //Position右边的全体移位，腾出地方
                for (int i = SystemManager.MaxMinionCount - 1; i >= Position; i--)
                {
                    BattleMinions[i] = BattleMinions[i - 1];
                }
                BattleMinions[Position - 1] = Minion;
            }
            for (int i = 0; i < MinionCount; i++)
            {
                BattleMinions[i].战场位置.位置 = i + 1;
                BattleMinions[i].战场位置.本方对方标识 = 本方对方标识;
            }
        }
        /// <summary>
        /// 从战场移除单位
        /// </summary>
        /// <param name="Position">从1开始的位置</param>
        /// <remarks>不涉及到亡语等计算</remarks>
        public void GetOutFromBattle(int Position)
        {
            for (int i = Position - 1; i < SystemManager.MaxMinionCount - 1; i++)
            {
                BattleMinions[i] = BattleMinions[i + 1];
            }
            BattleMinions[SystemManager.MaxMinionCount - 1] = null;
            for (int i = 0; i < MinionCount; i++)
            {
                BattleMinions[i].战场位置.位置 = i + 1;
                BattleMinions[i].战场位置.本方对方标识 = 本方对方标识;
            }
        }
        /// <summary>
        /// 冰冻状态的更新
        /// </summary>
        /// <param name="battle"></param>
        public void FreezeStatus()
        {
            foreach (var minion in BattleMinions)
            {
                if (minion != null)
                {
                    switch (minion.冰冻状态)
                    {
                        case CardUtility.效果回合枚举.效果命中:
                            //如果上回合被命中的，这回合就是作用中
                            minion.冰冻状态 = CardUtility.效果回合枚举.效果作用;
                            break;
                        case CardUtility.效果回合枚举.效果作用:
                            //如果上回合作用中的，这回合就是解除
                            minion.冰冻状态 = CardUtility.效果回合枚举.无效果;
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// 去除死去随从
        /// </summary>
        /// <param name="evt"></param>
        /// <param name="本方对方标识"></param>
        /// <returns></returns>
        public List<MinionCard> ClearDead(BattleEventHandler evt, bool 本方对方标识)
        {
            //必须是当前的随从，不能使编号
            //如果是沉默状态的随从，无亡语效果！
            List<MinionCard> DeadList = new List<MinionCard>();
            var CloneMinions = new MinionCard[SystemManager.MaxMinionCount];
            int ALive = 0;
            for (int i = 0; i < SystemManager.MaxMinionCount; i++)
            {
                if (BattleMinions[i] != null)
                {
                    if (BattleMinions[i].生命值 > 0)
                    {
                        CloneMinions[ALive] = BattleMinions[i];
                        ALive++;
                    }
                    else
                    {
                        DeadList.Add(BattleMinions[i]);
                        evt.事件池.Add(new CardUtility.全局事件()
                        {
                            触发事件类型 = CardUtility.事件类型枚举.死亡,
                            触发位置 = BattleMinions[i].战场位置,
                        });
                    }
                }
            }
            BattleMinions = CloneMinions;
            for (int i = 0; i < MinionCount; i++)
            {
                BattleMinions[i].战场位置.位置 = i + 1;
                BattleMinions[i].战场位置.本方对方标识 = 本方对方标识;
            }
            return DeadList;
        }
    }
}
