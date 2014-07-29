using Engine.Client;
using Engine.Utility;
using System;
namespace Engine.Card
{
    /// <summary>
    /// 卡牌共通
    /// </summary>
    [Serializable]
    public class CardBasicInfo
    {
        #region"基本"
        /// <summary>
        /// 序列号[Serial Number]
        /// </summary>
        /// <remarks>
        /// 该卡牌的统一序列号
        /// 该卡牌在整个系统中的唯一序号
        /// </remarks>
        public string 序列号;
        /// <summary>
        /// 名称[Name]
        /// </summary>
        /// <remarks>
        /// 该卡牌的名称。例如：“山岭巨人”。
        /// 人文方面的一个名称
        /// </remarks>
        public string 名称;
        /// <summary>
        /// 描述[Descrpition]
        /// </summary>
        /// <remarks>
        /// 该卡牌的描述。例如：“女猎手是暗夜精灵的卫士，她们出没于黑夜中”
        ///人文方面的一个描述
        /// </remarks>
        public string 描述;
        /// <summary>
        /// 稀有度[Rare]
        /// </summary>
        public enum 稀有程度枚举 : byte
        {
            /// <summary>
            /// White
            /// </summary>
            白色,
            /// <summary>
            /// Green
            /// </summary>
            绿色,
            /// <summary>
            /// Blue
            /// </summary>
            蓝色,
            /// <summary>
            /// Puple
            /// </summary>
            紫色,
            /// <summary>
            /// Orange
            /// </summary>
            橙色
        }
        /// <summary>
        /// 稀有度[Rare]
        /// </summary>
        public 稀有程度枚举 稀有程度;
        /// <summary>
        /// 卡牌种类
        /// </summary>
        public 卡牌类型枚举 卡牌种类 {
            get {
                switch (序列号.Substring(0,1))
                {
                    case "A":
                        return 卡牌类型枚举.法术;
                    case "W":
                        return 卡牌类型枚举.武器;
                    case "M":
                        return 卡牌类型枚举.随从;
                    case "S":
                        return 卡牌类型枚举.奥秘;
                    default:
                        return 卡牌类型枚举.其他;
                }
            }
        }
        /// <summary>
        /// 原生卡牌
        /// </summary>
        public 法术卡牌类型枚举 法术卡牌类型
        {
            get {
                switch (序列号.Substring(1, 1))
                {
                    case "0":
                        return 法术卡牌类型枚举.原生卡牌;
                    case "1":
                        return 法术卡牌类型枚举.英雄技能;
                    default:
                        return 法术卡牌类型枚举.战吼亡语;
                }
            }
        }
        /// <summary>
        /// 是否启用[Ready To Use]
        /// </summary>
        public Boolean 是否启用 = false;
        #endregion

        #region "炉石专用"
        public enum 法术卡牌类型枚举
        {
            原生卡牌,

            英雄技能,

            战吼亡语
        }
        /// <summary>
        /// 卡牌类型枚举[Card Type Enum]
        /// </summary>
        public enum 卡牌类型枚举
        {
            /// <summary>
            /// 随从[Minion]
            /// </summary>
            随从,
            /// <summary>
            /// 法术[Ability]
            /// </summary>
            法术,
            /// <summary>
            /// 武器[Weapon]
            /// </summary>
            武器,
            /// <summary>
            /// 奥秘[Secret]
            /// </summary>
            奥秘,
            /// <summary>
            /// 其他[Other]
            /// </summary>
            其他
        }
        /// <summary>
        /// 运行时的卡牌号码
        /// </summary>
        public int 运行时的卡牌号码;
        /// <summary>
        /// 过载[Overload]
        /// </summary>
        public int 过载;
        /// <summary>
        /// 连击(效果号码)[Combo Effect]
        /// </summary>
        public String 连击效果 = String.Empty;
        /// <summary>
        /// 职业[Class]
        /// </summary>
        public CardUtility.职业枚举 职业;
        /// <summary>
        /// 使用成本[Cost]
        /// </summary>
        public int 使用成本;
        /// <summary>
        /// 检查是否可以使用
        /// </summary>
        /// <returns></returns>
        public static String CheckCondition(Engine.Card.CardBasicInfo card,Client.PublicInfo MyInfo)
        {
            //剩余的法力是否足够实际召唤的法力
            String Message = String.Empty;
            if (card.过载 > 0 && MyInfo.OverloadPoint > 0)
            {
                Message = "已经使用过载";
                return Message;
            }
            //if (card.CardType == CardBasicInfo.CardTypeEnum.法术)
            //{
            //    if (((Card.AbilityCard)card).CheckCondition(this) == false)
            //    {
            //        Message = "没有法术使用对象";
            //        return Message;
            //    }
            //}
            if (card.卡牌种类 == CardBasicInfo.卡牌类型枚举.随从)
            {
                if (MyInfo.BattleField.MinionCount == SystemManager.MaxMinionCount)
                {
                    Message = "随从已经满员";
                    return Message;
                }
            }
            if (MyInfo.crystal.CurrentRemainPoint < card.使用成本)
            {
                Message = "法力水晶不足";
            }
            return Message;
        }
        /// <summary>
        /// 获得信息
        /// </summary>
        /// <returns></returns>
        public String GetInfo
        {
            get
            {
                return "[" + 卡牌种类.ToString() + "]" + 名称 + "[" + 使用成本 + "]";
            }
        }
        #endregion
    }
}
