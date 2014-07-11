using Engine.Utility;
using System;

namespace Engine.Effect
{
    [Serializable]
    public class EffectDefine
    {
        /// <summary>
        /// 对象选择器
        /// </summary>
        public CardUtility.位置选择用参数结构体 AbliltyPosPicker = new CardUtility.位置选择用参数结构体();
        /// <summary>
        /// 效果条件
        /// </summary>
        public String 效果条件 = String.Empty;
        /// <summary>
        /// 效果条件为 真
        /// </summary>
        public AtomicEffectDefine TrueAtomicEffect = new AtomicEffectDefine();
        /// <summary>
        /// 效果条件为 伪
        /// </summary>
        public AtomicEffectDefine FalseAtomicEffect = new AtomicEffectDefine();
        /// <summary>
        /// 效果回数
        /// </summary>
        public int EffectCount = 1;
        /// <summary>
        /// 是否需要位置选择
        /// </summary>
        public Boolean IsNeedTargetSelect
        {
            get{
                return AbliltyPosPicker.EffictTargetSelectMode == CardUtility.目标选择模式枚举.指定 ||
                       AbliltyPosPicker.EffictTargetSelectMode == CardUtility.目标选择模式枚举.横扫 ||
                       AbliltyPosPicker.EffictTargetSelectMode == CardUtility.目标选择模式枚举.相邻;
            }
        }
    }
}
