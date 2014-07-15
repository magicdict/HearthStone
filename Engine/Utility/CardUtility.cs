using Engine.Action;
using Engine.Card;
using Engine.Client;
using Engine.Control;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Engine.Utility
{
    /// <summary>
    /// 系统管理
    /// </summary>
    public static class CardUtility
    {
        /// <summary>
        /// CardXML文件夹
        /// </summary>
        public static String CardXmlFolder = String.Empty;
        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init(String mCardXmlFolder)
        {
            CardXmlFolder = mCardXmlFolder;
            //从配置文件中获得卡牌的SN和名称的联系
            GetCardInfoFromXml();
            //序列号 名称
            ReadyCardDic.Clear();
            foreach (CardBasicInfo card in CardCollections.Values)
            {
                if (card.是否启用) ReadyCardDic.Add(card.序列号, card.名称);
            }
        }
        /// <summary>
        /// 序列号和卡牌名称对应关系表格(可用状态)
        /// </summary>
        public static Dictionary<String, String> ReadyCardDic = new Dictionary<string, string>();
        /// <summary>
        /// 通过卡牌序列号获得卡牌名称
        /// </summary>
        /// <param name="SN"></param>
        /// <returns></returns>
        public static String GetCardNameBySN(String SN)
        {
            if (ReadyCardDic.ContainsKey(SN)) return ReadyCardDic[SN];
            return "UnKnow";
        }
        /// <summary>
        /// 获得卡牌信息
        /// </summary>
        /// <param name="CardSn"></param>
        /// <returns></returns>
        public static String GetCardInfo(String CardSn)
        {
            StringBuilder Status = new StringBuilder();
            if (Engine.Utility.CardUtility.GetCardInfoBySN(CardSn) != null)
            {
                Engine.Card.CardBasicInfo info = Engine.Utility.CardUtility.GetCardInfoBySN(CardSn);
                Status.AppendLine("==============");
                Status.AppendLine("Description" + info.描述);
                Status.AppendLine("StandardCostPoint" + info.使用成本);
                Status.AppendLine("Type：" + info.卡牌种类.ToString());
                switch (info.卡牌种类)
                {
                    case CardBasicInfo.卡牌类型枚举.随从:
                        Status.AppendLine("攻击力：" + ((Engine.Card.MinionCard)info).攻击力.ToString());
                        Status.AppendLine("生命值：" + ((Engine.Card.MinionCard)info).生命值上限.ToString());
                        break;
                    case CardBasicInfo.卡牌类型枚举.武器:
                        Status.AppendLine("攻击力：" + ((Engine.Card.WeaponCard)info).攻击力.ToString());
                        Status.AppendLine("耐久度：" + ((Engine.Card.WeaponCard)info).耐久度.ToString());
                        break;
                }
                Status.AppendLine("==============");
            }
            return Status.ToString();
        }
        /// <summary>
        /// 卡牌组合
        /// </summary>
        public static Dictionary<String, CardBasicInfo> CardCollections = new Dictionary<String, CardBasicInfo>();
        /// <summary>
        /// 通过卡牌序列号获得卡牌名称
        /// </summary>
        /// <param name="SN"></param>
        /// <returns></returns>
        public static CardBasicInfo GetCardInfoBySN(String SN)
        {
            if (CardCollections.ContainsKey(SN))
            {
                var c = CardCollections[SN].DeepCopy();
                if (c.卡牌种类 == CardBasicInfo.卡牌类型枚举.随从) ((Engine.Card.MinionCard)c).初始化();
                return c;
            }
            return null;
        }
        /// <summary>
        /// 从XML文件读取
        /// </summary>
        public static void GetCardInfoFromXml()
        {
            //调用侧的NET版本3.5会引发错误。。。
            CardCollections.Clear();
            //法术
            foreach (var AbilityXml in Directory.GetFiles(CardXmlFolder + "\\Ability\\"))
            {
                XmlSerializer xml = new XmlSerializer(typeof(Engine.Card.SpellCard));
                Engine.Card.SpellCard ability = (SpellCard)xml.Deserialize(new StreamReader(AbilityXml));
                CardCollections.Add(ability.序列号, ability);
            }
            //随从
            foreach (var MinionXml in Directory.GetFiles(CardXmlFolder + "\\Minion\\"))
            {
                XmlSerializer xml = new XmlSerializer(typeof(Engine.Card.MinionCard));
                Engine.Card.MinionCard Minio = (MinionCard)xml.Deserialize(new StreamReader(MinionXml));
                Minio.使用成本 = Minio.使用成本;
                CardCollections.Add(Minio.序列号, Minio);
            }
            //武器
            foreach (var WeaponXml in Directory.GetFiles(CardXmlFolder + "\\Weapon\\"))
            {
                XmlSerializer xml = new XmlSerializer(typeof(Engine.Card.WeaponCard));
                Engine.Card.WeaponCard Weapon = (WeaponCard)xml.Deserialize(new StreamReader(WeaponXml));
                Weapon.使用成本 = Weapon.使用成本;
                CardCollections.Add(Weapon.序列号, Weapon);
            }
            //奥秘
            foreach (var SecretXml in Directory.GetFiles(CardXmlFolder + "\\Secret\\"))
            {
                XmlSerializer xml = new XmlSerializer(typeof(Engine.Card.SecretCard));
                Engine.Card.SecretCard Secret = (SecretCard)xml.Deserialize(new StreamReader(SecretXml));
                Secret.使用成本 = Secret.使用成本;
                CardCollections.Add(Secret.序列号, Secret);
            }
        }

        #region"常数"
        /// <summary>
        /// 原生法术
        /// </summary>
        public const String 原生卡牌标识 = "0";
        /// <summary>
        /// 真
        /// </summary>
        public const String strTrue = "1";
        /// <summary>
        /// 假
        /// </summary>
        public const String strFalse = "0";
        /// <summary>
        /// 表示本方
        /// </summary>
        public const String strMe = "ME";
        /// <summary>
        /// 表示对方
        /// </summary>
        public const String strYou = "YOU";
        /// <summary>
        /// 分隔符号
        /// </summary>
        public const String strSplitMark = "#";
        /// <summary>
        /// 分隔符号（不同内容）
        /// </summary>
        public const String strSplitDiffMark = "$";
        /// <summary>
        /// 分隔符号(数组)
        /// </summary>
        public const String strSplitArrayMark = "|";
        /// <summary>
        /// 忽略
        /// </summary>
        public const String strIgnore = "X";
        /// <summary>
        /// 最大值
        /// </summary>
        public const int Max = 999;
        /// <summary>
        /// OK
        /// </summary>
        public const String strOK = "OK";
        #endregion
        
        #region"枚举值"
        /// <summary>
        /// 职业
        /// </summary>
        public enum 职业枚举
        {
            猎人,
            盗贼,
            中立,
            德鲁伊,
            术士,
            圣骑士,
            萨满,
            牧师,
            法师,
            战士,
        }
        /// <summary>
        /// 种族
        /// </summary>
        public enum 种族枚举
        {
            无,
            恶魔,
            龙,
            海盗,
            鱼人,
            野兽,
        }
        /// <summary>
        /// 目标选择模式
        /// </summary>
        public enum 目标选择模式枚举
        {
            /// <summary>
            /// 无需选择
            /// </summary>
            不用选择,
            /// <summary>
            /// 继承第一效果的位置信息
            /// </summary>
            继承,
            /// <summary>
            /// 随机
            /// </summary>
            随机,
            /// <summary>
            /// 全体
            /// </summary>
            全体,
            /// <summary>
            /// 指定
            /// </summary>
            指定,
            /// <summary>
            /// 单个目标 + 全体的模式
            /// </summary>
            横扫,
            /// <summary>
            /// 单个目标 + 相邻的模式
            /// </summary>
            相邻
        }
        /// <summary>
        /// 目标选择方向
        /// </summary>
        public enum 目标选择方向枚举
        {
            /// <summary>
            /// 本方
            /// </summary>
            本方,
            /// <summary>
            /// 对方
            /// </summary>
            对方,
            /// <summary>
            /// 无限制
            /// </summary>
            双方
        }
        /// <summary>
        /// 选择器和选择结果
        /// </summary>
        [Serializable]
        public struct 位置选择用参数结构体
        {
            /// <summary>
            /// 法术对象选择模式
            /// </summary>
            public CardUtility.目标选择模式枚举 EffictTargetSelectMode;
            /// <summary>
            /// 法术对象选择角色
            /// </summary>
            public CardUtility.目标选择角色枚举 EffectTargetSelectRole;
            /// <summary>
            /// 法术对象选择方向
            /// </summary>
            public CardUtility.目标选择方向枚举 EffectTargetSelectDirect;
            /// <summary>
            /// 法术对象选择条件
            /// </summary>
            public String EffectTargetSelectCondition;
            /// <summary>
            /// 选定位置
            /// </summary>
            public 指定位置结构体 SelectedPos;
            /// <summary>
            /// 嘲讽限制
            /// </summary>
            public Boolean 嘲讽限制;
        }
        /// <summary>
        /// 符合种族条件
        /// </summary>
        /// <param name="minion"></param>
        /// <param name="SelectOpt"></param>
        /// <returns></returns>
        public static Boolean 符合选择条件(Engine.Card.MinionCard minion, String strCondition)
        {
            if (String.IsNullOrEmpty(strCondition) || strCondition == strIgnore) return true;
            foreach (var 种族名称 in Enum.GetNames(typeof(种族枚举)))
            {
                if (种族名称 == strCondition)
                {
                    return strCondition == minion.种族.ToString();
                }
                if (("非" + 种族名称) == strCondition)
                {
                    return strCondition != minion.种族.ToString();
                }
            }
            switch (strCondition.Substring(1, 1))
            {
                case "+":
                    return minion.攻击力 >= int.Parse(strCondition.Substring(0, 1));
                case "-":
                    return minion.攻击力 <= int.Parse(strCondition.Substring(0, 1));
            }
            return true;
        }
        /// <summary>
        /// 目标选择角色
        /// </summary>
        public enum 目标选择角色枚举
        {
            /// <summary>
            /// 随从
            /// </summary>
            随从,
            /// <summary>
            /// 英雄
            /// </summary>
            英雄,
            /// <summary>
            /// 全体（随从+英雄）
            /// </summary>
            所有角色,
            /// <summary>
            /// 武器，例如：潜行者，对武器喂毒
            /// </summary>
            武器
        }
        /// <summary>
        /// 返回值枚举
        /// </summary>
        public enum 返回值枚举
        {
            /// <summary>
            /// 正常
            /// </summary>
            正常,
            /// <summary>
            /// 异常
            /// </summary>
            异常
        }
        /// <summary>
        /// 抉择枚举
        /// </summary>
        public enum 抉择枚举
        {
            /// <summary>
            /// 第一效果
            /// </summary>
            第一效果,
            /// <summary>
            /// 第二效果
            /// </summary>
            第二效果,
            /// <summary>
            /// 取消
            /// </summary>
            取消
        }
        /// <summary>
        /// 效果回合[Turn Effect]
        /// </summary>
        public enum 效果回合枚举
        {
            /// <summary>
            /// 无效果[None]
            /// </summary>
            无效果,
            /// <summary>
            /// 效果命中[Hit Turn]
            /// </summary>
            效果命中,
            /// <summary>
            /// 效果作用[Effect Turn]
            /// </summary>
            效果作用
        }
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
            public 指定位置结构体 触发位置;
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
            public 目标选择方向枚举 触发效果事件方向;
            /// <summary>
            /// 效果编号
            /// </summary>
            public String 效果编号;
            /// <summary>
            /// 限制信息
            /// </summary>
            public String 限制信息;
        }
        /// <summary>
        /// 用户指定位置
        /// </summary>
        [Serializable]
        public struct 指定位置结构体
        {
            /// <summary>
            /// 本方/对方
            /// HOST/GUEST
            /// </summary>
            public Boolean 本方对方标识;
            /// <summary>
            /// 0 - 英雄，1-7 随从位置
            /// </summary>
            public int 位置;
            /// <summary>
            /// ToString(重载)
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return (本方对方标识 ? strMe : strYou) + strSplitMark + 位置.ToString("D1");
            }
            /// <summary>
            /// 将文字转换为位置信息
            /// </summary>
            /// <param name="Info"></param>
            /// <returns></returns>
            public static 指定位置结构体 FromString(String Info)
            {
                指定位置结构体 PosInfo = new 指定位置结构体();
                PosInfo.本方对方标识 = Info.Split(strSplitMark.ToCharArray())[0] == strMe;
                PosInfo.位置 = int.Parse(Info.Split(strSplitMark.ToCharArray())[1]);
                return PosInfo;
            }
        }
        #endregion

        #region"委托"
        /// <summary>
        /// 抉择
        /// </summary>
        /// <param name="First">第一效果</param>
        /// <param name="Second">第二效果</param>
        /// <returns>是否为第一效果</returns>
        public delegate 抉择枚举 delegatePickEffect(String First, String Second);
        /// <summary>
        /// 抽牌委托
        /// </summary>
        /// <param name="IsFirst">先后手区分</param>
        /// <param name="Ability">法术定义</param>
        public delegate List<String> delegateDrawCard(Boolean IsFirst, int DrawCount);
        /// <summary>
        /// 获得位置
        /// </summary>
        /// <returns></returns>
        public delegate 指定位置结构体 deleteGetTargetPosition(位置选择用参数结构体 选择参数);
        /// <summary>
        /// 随从进场位置
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public delegate int delegateGetMinionPos(BattleFieldInfo battleInfo);
        /// <summary>
        /// 中断处理
        /// </summary>
        /// <param name="interrupt"></param>
        public delegate void delegateInterrupt(FullServerManager.Interrupt interrupt);
        #endregion

    }
}
