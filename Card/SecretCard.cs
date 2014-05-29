using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Card
{
    /// <summary>
    /// 奥秘卡牌
    /// </summary>
    [Serializable]
    public class SecretCard:CardBasicInfo
    {
        /// <summary>
        /// 触发条件类型
        /// </summary>
        public enum SecretCondition
        {
            对方召唤随从
        }
        public SecretCondition Condition = SecretCondition.对方召唤随从;
        /// <summary>
        /// 附加信息
        /// </summary>
        public String AdditionInfo = String.Empty;
    }
}
