using System;
using System.Collections.Generic;

namespace Card.Client
{
    /// <summary>
    /// 战场信息（炉石专用）
    /// </summary>
    public class BattleFieldInfo
    {
        /// <summary>
        /// 最多7个随从的位置
        /// </summary>
        public const int MaxMinionCount = 7;
        /// <summary>
        /// 
        /// </summary>
        public const int HeroPos = 0;
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
        /// </summary>
        public int AbilityEffect = 0;
        /// <summary>
        /// 当前随从数量
        /// </summary>
        public int MinionCount
        {
            get
            {
                int t = 0;
                for (int i = 0; i < MaxMinionCount; i++)
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
        public MinionCard[] BattleMinions = new MinionCard[MaxMinionCount];
        /// <summary>
        /// 卡牌入战场
        /// </summary>
        /// <param name="CardSn"></param>
        public void AppendToBattle(String CardSn)
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
        public void PutToBattle(int Position, String CardSn)
        {
            Card.CardBasicInfo card = Card.CardUtility.GetCardInfoBySN(CardSn);
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
            if (MinionCount == MaxMinionCount) return;
            //无效的位置
            if ((Position < 1) || (Position > MinionCount + 1) || Position > MaxMinionCount) return;
            //插入操作
            if (BattleMinions[Position - 1] == null)
            {
                //添加到最右边
                BattleMinions[Position - 1] = Minion;
            }
            else
            {
                //Position右边的全体移位，腾出地方
                for (int i = MaxMinionCount - 1; i >= Position; i--)
                {
                    BattleMinions[i] = BattleMinions[i - 1];
                }
                BattleMinions[Position - 1] = Minion;
            }
        }
        /// <summary>
        /// 发动战吼
        /// 自身/相邻
        /// </summary>
        /// <param name="MinionPos"></param>
        public List<String> 发动战吼(int MinionPos)
        {
            List<String> ActionCodeLst = new List<string>();
            if (!String.IsNullOrEmpty(BattleMinions[MinionPos - 1].战吼效果))
            {
                List<int> PosList = new List<int>();
                if (BattleMinions[MinionPos - 1].战吼类型 == MinionCard.战吼类型列表.相邻)
                {
                    //相邻
                    //左边随从存在？
                    if (MinionPos != 1) PosList.Add(MinionPos - 1);
                    if (MinionPos != MinionCount) PosList.Add(MinionPos + 1);
                }
                else
                {
                    //自身
                    PosList.Add(MinionPos);
                }
                //处理状态和数值变化
                var 战吼 = (Card.AbilityCard)Card.CardUtility.GetCardInfoBySN(BattleMinions[MinionPos - 1].战吼效果);

                foreach (int PosInfo in PosList)
                {
                    switch (战吼.CardAbility.FirstAbilityDefine.AbilityEffectType)
                    {
                        case Card.Effect.EffectDefine.AbilityEffectEnum.点数:
                            Card.Effect.PointEffect.RunPointEffect(BattleMinions[PosInfo - 1], 战吼.CardAbility.FirstAbilityDefine.AddtionInfo);
                            ActionCodeLst.Add(Card.Server.ActionCode.strPoint + Card.CardUtility.strSplitMark + Card.CardUtility.strMe + Card.CardUtility.strSplitMark +
                            PosInfo + Card.CardUtility.strSplitMark + 战吼.CardAbility.FirstAbilityDefine.AddtionInfo);
                            break;
                        case Card.Effect.EffectDefine.AbilityEffectEnum.状态:
                            Card.Effect.StatusEffect.RunStatusEffect(BattleMinions[PosInfo - 1], 战吼.CardAbility.FirstAbilityDefine.AddtionInfo);
                            ActionCodeLst.Add(Card.Server.ActionCode.strStatus + Card.CardUtility.strSplitMark + Card.CardUtility.strMe + Card.CardUtility.strSplitMark +
                            PosInfo + Card.CardUtility.strSplitMark + 战吼.CardAbility.FirstAbilityDefine.AddtionInfo);
                            break;
                    }
                    switch (战吼.CardAbility.SecondAbilityDefine.AbilityEffectType)
                    {
                        case Card.Effect.EffectDefine.AbilityEffectEnum.点数:
                            Card.Effect.PointEffect.RunPointEffect(BattleMinions[PosInfo - 1], 战吼.CardAbility.SecondAbilityDefine.AddtionInfo);
                            ActionCodeLst.Add(Card.Server.ActionCode.strPoint + Card.CardUtility.strSplitMark + Card.CardUtility.strMe + Card.CardUtility.strSplitMark +
                            PosInfo + Card.CardUtility.strSplitMark + 战吼.CardAbility.SecondAbilityDefine.AddtionInfo);
                            break;
                        case Card.Effect.EffectDefine.AbilityEffectEnum.状态:
                            Card.Effect.StatusEffect.RunStatusEffect(BattleMinions[PosInfo - 1], 战吼.CardAbility.SecondAbilityDefine.AddtionInfo);
                            ActionCodeLst.Add(Card.Server.ActionCode.strStatus + Card.CardUtility.strSplitMark + Card.CardUtility.strMe + Card.CardUtility.strSplitMark +
                            PosInfo + Card.CardUtility.strSplitMark + 战吼.CardAbility.SecondAbilityDefine.AddtionInfo);
                            break;
                    }
                }
            }
            return ActionCodeLst;
        }
        /// <summary>
        /// 从战场移除单位
        /// </summary>
        /// <param name="Position">从1开始的位置</param>
        /// <remarks>不涉及到亡语等计算</remarks>
        public void GetOutFromBattle(int Position)
        {
            for (int i = Position - 1; i < MaxMinionCount - 1; i++)
            {
                BattleMinions[i] = BattleMinions[i + 1];
            }
            BattleMinions[MaxMinionCount - 1] = null;
        }

        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="事件"></param>
        /// <param name="game"></param>
        /// <returns></returns>
        public List<String> 触发事件(CardUtility.全局事件 事件, GameManager game)
        {
            List<String> ActionLst = new List<string>();
            for (int i = 0; i < BattleMinions.Length; i++)
            {
                var minion = BattleMinions[i];
                if (minion != null)
                {
                    ActionLst.AddRange(minion.触发事件(事件, game));
                }
            }
            return ActionLst;
        }

        /// <summary>
        /// Buff的设置
        /// </summary>
        /// <param name="game"></param>
        public void ResetBuff()
        {
            //去除所有光环效果
            for (int i = 0; i < BattleMinions.Length; i++)
            {
                if (BattleMinions[i] != null) BattleMinions[i].受战地效果.Clear();
            }
            AbilityCost = 0;
            AbilityEffect = 0;
            MinionCost = 0;
            //设置光环效果
            for (int i = 0; i < BattleMinions.Length; i++)
            {
                var minion = BattleMinions[i];
                if (minion != null)
                {
                    if (!String.IsNullOrEmpty(minion.光环效果.BuffInfo))
                    {
                        switch (minion.光环效果.EffectType)
                        {
                            case MinionCard.光环类型.增加攻防:
                                switch (minion.光环效果.Scope)
                                {
                                    case MinionCard.光环范围.随从全体:
                                        for (int j = 0; j < BattleMinions.Length; j++)
                                        {
                                            if (BattleMinions[j] != null) BattleMinions[j].受战地效果.Add(minion.光环效果);
                                        }
                                        break;
                                    case MinionCard.光环范围.相邻随从:
                                        break;
                                }
                                break;
                            case MinionCard.光环类型.施法成本:
                                AbilityCost += int.Parse(minion.光环效果.BuffInfo);
                                break;
                            case MinionCard.光环类型.法术效果:
                                AbilityEffect += int.Parse(minion.光环效果.BuffInfo);
                                break;
                            case MinionCard.光环类型.随从成本:
                                MinionCost += int.Parse(minion.光环效果.BuffInfo);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 去除死去随从
        /// </summary>
        public List<MinionCard> ClearDead()
        {
            //必须是当前的随从，不能使编号
            //如果是沉默状态的随从，无亡语效果！
            List<MinionCard> DeadList = new List<MinionCard>();
            var CloneMinions = new MinionCard[BattleFieldInfo.MaxMinionCount];
            int ALive = 0;
            for (int i = 0; i < BattleFieldInfo.MaxMinionCount; i++)
            {
                if (BattleMinions[i] != null)
                {
                    if (BattleMinions[i].IsLive())
                    {
                        CloneMinions[ALive] = BattleMinions[i];
                        ALive++;
                    }
                    else
                    {
                        DeadList.Add(BattleMinions[i]);
                    }
                }
            }
            BattleMinions = CloneMinions;
            return DeadList;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<String> ShowMinions()
        {
            List<String> InfoList = new List<string>();
            for (int i = 0; i < MinionCount; i++)
            {
                InfoList.Add("[" + BattleMinions[i].Name + "]" + BattleMinions[i].实际生命值 + "/" + BattleMinions[i].TotalAttack());
            }
            return InfoList;
        }
    }
}
