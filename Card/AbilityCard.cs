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
        }
        /// <summary>
        /// 分解获得效果列表
        /// </summary>
        /// <param name="IsFirstEffect">需要抉择的时候，是否选择第一项目</param>
        /// <returns>最小效果列表</returns>
        public List<Card.Effect.AtomicEffectDefine> GetSingleEffectList(Boolean IsFirstEffect)
        {
            //这里都转化为1次效果
            //例如：奥术飞弹的3次工具这里将转为3次效果
            //这样做的原因是，每次奥术飞弹攻击之后，必须要进行一次清算，是否有目标已经被摧毁
            //如果被摧毁的话，无法攻击这个目标了，
            //同时，如果出现亡语的话，亡语可能召唤出新的可攻击目标
            List<Card.Effect.AtomicEffectDefine> EffectLst = new List<Card.Effect.AtomicEffectDefine>();
            return EffectLst;
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public new void Init()
        {
            FirstAbilityDefine.Init();
            if (IsNeedSelect()) SecondAbilityDefine.Init();
        }
        /// <summary>
        /// 调整法术效果
        /// </summary>
        /// <param name="AbilityEffect"></param>
        public void JustfyEffectPoint(int AbilityEffect)
        {
            //法术强度本意是增加法术卡的总伤。以奥术飞弹为例，法术强度+1会令奥术飞弹多1发伤害，而非单发伤害+1。法术强度不影响治疗效果。
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
