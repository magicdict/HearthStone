using System;
using System.Collections.Generic;

namespace Engine.Effect
{
    [Serializable]
    public class AtomicEffectDefine
    {
        /// <summary>
        /// 魔法效果
        /// </summary>
        public enum AtomicEffectEnum
        {
            /// <summary>
            /// 未定义
            /// </summary>
            未定义,
            /// <summary>
            /// 攻击类
            /// </summary>
            攻击,
            /// <summary>
            /// 治疗回复
            /// </summary>
            回复,
            /// <summary>
            /// 改变状态
            /// </summary>
            状态,
            /// <summary>
            /// 召唤
            /// </summary>
            召唤,
            /// <summary>
            /// 增益（点数）
            /// </summary>
            增益,
            /// <summary>
            /// 抽牌/弃牌
            /// </summary>
            卡牌,
            /// <summary>
            /// 变形
            /// 变羊，变青蛙
            /// </summary>
            变形,
            /// <summary>
            /// 获得水晶
            /// </summary>
            水晶,
            /// <summary>
            /// 控制权
            /// </summary>
            控制,
            /// <summary>
            /// 奥秘
            /// </summary>
            奥秘,
            /// <summary>
            /// 武器
            /// </summary>
            武器,
        }
        /// <summary>
        /// 
        /// </summary>
        public AtomicEffectEnum AtomicEffectType = AtomicEffectEnum.未定义;
        /// <summary>
        /// 信息组
        /// </summary>
        public List<String> InfoArray = new List<string>();
        /// <summary>
        /// 初始化值
        /// </summary>
        public virtual void GetField() { }
    }
}
