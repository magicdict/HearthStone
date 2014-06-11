using Card.Client;
using Card.Effect;
using System;
using System.Collections.Generic;

namespace Card
{
    /// <summary>
    /// 法术卡牌
    /// </summary>
    [Serializable]
    public class AbilityCard : CardBasicInfo
    {
        /// <summary>
        /// 原生法术
        /// </summary>
        public const String 原生法术 = "0";
        /// <summary>
        /// 效果选择类型枚举
        /// </summary>
        public enum 效果选择类型枚举
        {
            /// <summary>
            /// 无需选择
            /// </summary>
            无需选择,
            /// <summary>
            /// 用户主动
            /// </summary>
            主动选择,
            /// <summary>
            /// 自动判定
            /// </summary>
            自动判定
        }
        /// <summary>
        /// 效果选择类型枚举
        /// </summary>
        public 效果选择类型枚举 效果选择类型 = 效果选择类型枚举.无需选择;
        /// <summary>
        /// 是否需要抉择
        /// </summary>
        /// <returns></returns>
        public Boolean IsNeedSelect()
        {
            return 效果选择类型 != 效果选择类型枚举.无需选择;
        }
        /// <summary>
        /// 第一效果
        /// </summary>
        public EffectDefine FirstAbilityDefine = new EffectDefine();
        /// <summary>
        /// 第二效果
        /// </summary>
        public EffectDefine SecondAbilityDefine = new EffectDefine();
        /// <summary>
        /// 效果定义
        /// </summary>
        public struct EffectDefine
        {
            /// <summary>
            /// 主效果定义
            /// </summary>
            public AtomicEffectDefine MainAbilityDefine;
            /// <summary>
            /// 主效果定义(连击)
            /// </summary>
            public AtomicEffectDefine MainAbilityDefineCombit;
            /// <summary>
            /// 追加效果定义
            /// </summary>
            public AtomicEffectDefine AppendAbilityDefine;
            /// <summary>
            /// 追加效果启动条件
            /// </summary>
            public String AppendEffectCondition;
            /// <summary>
            /// 初始化
            /// </summary>
            public void Init()
            {
                MainAbilityDefine = new AtomicEffectDefine();
                AppendAbilityDefine = new AtomicEffectDefine();
                MainAbilityDefineCombit = new AtomicEffectDefine();
            }
            /// <summary>
            /// 用具体的类替换
            /// </summary>
            public void GetField()
            {
                MainAbilityDefine.GetField();
                AppendAbilityDefine.GetField();
                MainAbilityDefineCombit.GetField();
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public new void Init()
        {
            FirstAbilityDefine.GetField();
            SecondAbilityDefine.GetField();
        }
        /// <summary>
        /// 分解获得效果列表
        /// </summary>
        /// <param name="game"></param>
        /// <param name="atomic"></param>
        /// <returns></returns>
        public List<Card.Effect.AtomicEffectDefine> GetSingleEffectList(GameManager game,AtomicEffectDefine atomic)
        {
            //攻击，抽牌，召唤
            //多次攻击需要考虑法术伤害加成
            List<Card.Effect.AtomicEffectDefine> EffectLst = new List<Card.Effect.AtomicEffectDefine>();
            switch (atomic.AbilityEffectType)
            {
                case AtomicEffectDefine.AbilityEffectEnum.攻击:
                    int ActualAttackCount =  Effecthandler.GetEffectPoint(game,((AttackEffect)atomic).标准效果回数表达式);
                    if (ActualAttackCount > 1) ActualAttackCount += game.MyInfo.BattleField.AttackEffectPlus;
                    for (int i = 0; i < ActualAttackCount; i++)
                    {
                        EffectLst.Add(atomic);
                    }
                    break;
                default:
                    EffectLst.Add(atomic);
                    break;
            }
            return EffectLst;
        }
        /// <summary>
        /// 简单检查
        /// </summary>
        /// <param name="gameManager"></param>
        /// <returns></returns>
        public bool CheckCondition(Client.GameManager gameManager)
        {
            return true;
        }
    }
}
