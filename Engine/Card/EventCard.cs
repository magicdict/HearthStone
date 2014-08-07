using System;

namespace Engine.Card
{
    public class EventCard : CardBasicInfo
    {
        /// <summary>
        /// 事件类型枚举[Event Enum]
        /// </summary>
        public enum 事件类型枚举
        {
            /// <summary>
            /// None
            /// </summary>
            无,
            /// <summary>
            /// Run Ability
            /// </summary>
            施法,
            /// <summary>
            /// Recover
            /// </summary>
            治疗,
            /// <summary>
            /// Die
            /// </summary>
            死亡,
            /// <summary>
            /// Hit Secret
            /// </summary>
            奥秘命中,
            /// <summary>
            /// Damage
            /// </summary>
            受伤,
            /// <summary>
            /// Summon
            /// </summary>
            召唤,
            /// <summary>
            /// Draw Card
            /// </summary>
            卡牌,
        }
        /// <summary>
        /// 全局事件[Event]
        /// </summary>
        [Serializable]
        public struct 全局事件
        {
            /// <summary>
            /// 触发事件类型[Event Type]
            /// </summary>
            public 事件类型枚举 触发事件类型;
            /// <summary>
            /// 触发位置[Evnet Position]
            /// </summary>
            public Utility.CardUtility.指定位置结构体 触发位置;
        }
        /// <summary>
        /// 事件效果结构体[Evnet Effect Struct]
        /// </summary>
        [Serializable]
        public struct 事件效果结构体
        {
            /// <summary>
            /// 事件名称
            /// </summary>
            public 事件类型枚举 触发效果事件类型;
            /// <summary>
            /// 触发位置
            /// </summary>
            public Utility.CardUtility.目标选择方向枚举 触发效果事件方向;
            /// <summary>
            /// 效果编号
            /// </summary>
            public string 效果编号;
            /// <summary>
            /// 限制信息
            /// </summary>
            public string 限制信息;
        }
    }
}
