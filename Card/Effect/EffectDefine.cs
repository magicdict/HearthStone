using System;
using System.Collections.Generic;

namespace Card.Effect
{
    [Serializable]
    public class EffectDefine
    {
        /// <summary>
        /// 描述
        /// </summary>
        public String Description = String.Empty;
        /// <summary>
        /// 魔法效果
        /// </summary>
        public enum AbilityEffectEnum
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
            /// 改变卡牌点数
            /// </summary>
            点数,
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
        }
        /// <summary>
        /// 法术类型
        /// </summary>
        public AbilityEffectEnum AbilityEffectType;
        /// <summary>
        /// 选择
        /// </summary>
        public CardUtility.SelectOption SelectOpt = new CardUtility.SelectOption();
        /// <summary>
        /// 是否需要选择目标
        /// </summary>
        /// <returns></returns>
        public Boolean IsNeedSelectTarget()
        {
            return SelectOpt.EffictTargetSelectMode == CardUtility.TargetSelectModeEnum.指定;
        }
        /// 攻击的时候：99表示消灭一个单位
        /// 治疗的时候：99表示完全回复一个单位
        /// 抽牌的时候：表示抽牌的数量
        /// <summary>
        /// 效果点数(标准)
        /// </summary>
        public int StandardEffectPoint;
        /// <summary>
        /// 效果点数(实际)
        /// </summary>
        public int ActualEffectPoint;
        /// <summary>
        /// 效果回数
        /// </summary>
        public int StandardEffectCount;
        /// <summary>
        /// 效果回数(实际)
        /// </summary>
        public int ActualEffectCount;
        /// <summary>
        /// 附加信息
        /// </summary>
        public String AddtionInfo;
        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            ActualEffectPoint = StandardEffectPoint;
            ActualEffectCount = StandardEffectCount;
        }
    }
}
