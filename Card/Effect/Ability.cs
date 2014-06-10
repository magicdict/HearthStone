using System;
using System.Collections.Generic;

namespace Card.Effect
{
    [Serializable]
    public class Ability
    {
        /// <summary>
        /// 效果选择类型枚举
        /// </summary>
        public enum 效果选择类型枚举{
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
            public String SecondStartCondition;
            /// <summary>
            /// 初始化
            /// </summary>
            public void Init()
            {
                MainAbilityDefine.Init();
                if (AppendAbilityDefine != null) AppendAbilityDefine.Init();
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
            if (IsNeedSelect())
            {
                if (IsFirstEffect)
                {
                    for (int i = 0; i < FirstAbilityDefine.MainAbilityDefine.ActualEffectCount; i++)
                    {
                        EffectLst.Add(FirstAbilityDefine.MainAbilityDefine);
                    }
                }
                else
                {
                    for (int i = 0; i < SecondAbilityDefine.MainAbilityDefine.ActualEffectCount; i++)
                    {
                        EffectLst.Add(SecondAbilityDefine.MainAbilityDefine);
                    }
                }
            }
            else
            {
                for (int i = 0; i < FirstAbilityDefine.MainAbilityDefine.ActualEffectCount; i++)
                {
                    EffectLst.Add(FirstAbilityDefine.MainAbilityDefine);
                }
                if (SecondAbilityDefine.MainAbilityDefine.AbilityEffectType != AtomicEffectDefine.AbilityEffectEnum.未定义)
                {
                    for (int i = 0; i < SecondAbilityDefine.MainAbilityDefine.ActualEffectCount; i++)
                    {
                        EffectLst.Add(SecondAbilityDefine.MainAbilityDefine);
                    }
                }
            }
            return EffectLst;
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            FirstAbilityDefine.Init();
            if (IsNeedSelect()) SecondAbilityDefine.Init();            
        }
    }
}
