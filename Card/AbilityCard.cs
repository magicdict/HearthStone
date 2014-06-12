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
        /// 对象选择器
        /// </summary>
        public CardUtility.SelectOption 对象选择器 = new CardUtility.SelectOption();
        /// <summary>
        /// 效果选择类型枚举
        /// </summary>
        public 效果选择类型枚举 效果选择类型 = 效果选择类型枚举.无需选择;
        /// <summary>
        /// 标准效果回数表达式
        /// </summary>
        public String 标准效果回数表达式 = String.Empty;
        /// <summary>
        /// 第一效果
        /// </summary>
        public AbilityDefine FirstAbilityDefine = new AbilityDefine();
        /// <summary>
        /// 第二效果
        /// </summary>
        public AbilityDefine SecondAbilityDefine = new AbilityDefine();
        /// <summary>
        /// 效果定义
        /// </summary>
        public struct AbilityDefine
        {
            /// <summary>
            /// 主效果定义
            /// </summary>
            public EffectDefine MainAbilityDefine;
            /// <summary>
            /// 追加效果定义
            /// </summary>
            public EffectDefine AppendAbilityDefine;
            /// <summary>
            /// 追加效果启动条件
            /// </summary>
            public String AppendEffectCondition;
            /// <summary>
            /// 初始化
            /// </summary>
            public void Init()
            {
                MainAbilityDefine = new EffectDefine();
                AppendAbilityDefine = new EffectDefine();
            }
            /// <summary>
            /// 用具体的类替换
            /// </summary>
            public void GetField()
            {
                MainAbilityDefine.TrueAtomicEffect.GetField();
                MainAbilityDefine.FalseAtomicEffect.GetField();
                AppendAbilityDefine.TrueAtomicEffect.GetField();
                AppendAbilityDefine.FalseAtomicEffect.GetField();
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
        public List<Card.Effect.EffectDefine> GetSingleEffectList(GameManager game,EffectDefine atomic)
        {
            //攻击，抽牌，召唤
            //多次攻击需要考虑法术伤害加成
            List<Card.Effect.EffectDefine> EffectLst = new List<Card.Effect.EffectDefine>();

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
