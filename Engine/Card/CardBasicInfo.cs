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
        /// 序列号
        /// </summary>
        /// <remarks>
        /// 该卡牌的统一序列号
        /// 该卡牌在整个系统中的唯一序号
        /// </remarks>
        public string SN;
        /// <summary>
        /// 名称
        /// </summary>
        /// <remarks>
        /// 该卡牌的名称。例如：“山岭巨人”。
        /// 人文方面的一个名称
        /// </remarks>
        public string Name;
        /// <summary>
        /// 描述
        /// </summary>
        /// <remarks>
        /// 该卡牌的描述。例如：“女猎手是暗夜精灵的卫士，她们出没于黑夜中”
        ///人文方面的一个描述
        /// </remarks>
        public string Description;
        /// <summary>
        /// 稀有度
        /// </summary>
        public enum 稀有程度 : byte
        {
            白色,
            绿色,
            蓝色,
            紫色,
            橙色
        }
        /// <summary>
        /// 稀有度
        /// </summary>
        ///<remarks>
        /// 该卡牌的稀有度
        /// </remarks>
        public 稀有程度 Rare;
        /// <summary>
        /// 获得卡牌种类
        /// </summary>
        public CardTypeEnum CardType {
            get {
                switch (SN.Substring(0,1))
                {
                    case "A":
                        return CardTypeEnum.法术;
                    case "W":
                        return CardTypeEnum.武器;
                    case "M":
                        return CardTypeEnum.随从;
                    case "S":
                        return CardTypeEnum.奥秘;
                    default:
                        return CardTypeEnum.其他;
                }
            }
        }
        /// <summary>
        /// 是否启用
        /// </summary>
        public Boolean IsCardReady = false;
        #endregion

        #region "炉石专用"
        public enum CardTypeEnum
        {
            /// <summary>
            /// 随从
            /// </summary>
            随从,
            /// <summary>
            /// 法术
            /// </summary>
            法术,
            /// <summary>
            /// 武器
            /// </summary>
            武器,
            /// <summary>
            /// 奥秘
            /// </summary>
            奥秘,
            /// <summary>
            /// 其他
            /// </summary>
            其他
        }
        /// <summary>
        /// 运行时的卡牌号码
        /// </summary>
        public int RuntimeId;
        /// <summary>
        /// 过载
        /// </summary>
        public int Overload;
        /// <summary>
        /// 连击(效果号码)
        /// </summary>
        public String 连击效果 = String.Empty;
        /// <summary>
        /// 职业
        /// </summary>
        public CardUtility.ClassEnum Class;
        /// <summary>
        /// 的使用成本
        /// </summary>
        public int 使用成本;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String GetInfo()
        {
            return "[" + CardType.ToString() + "]" + Name + "[" + 使用成本 + "]";
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
        }
        #endregion

    }
}
