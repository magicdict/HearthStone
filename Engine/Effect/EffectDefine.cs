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
        public CardUtility.PositionSelectOption AbliltyPosPicker = new CardUtility.PositionSelectOption();
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
    }
}
