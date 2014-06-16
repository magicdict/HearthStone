using Engine.Card;
using System;
using System.Collections.Generic;
using System.Drawing;
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
                Status.AppendLine("Type：" + info.CardType.ToString());
                switch (info.CardType)
                {
                    case CardBasicInfo.CardTypeEnum.随从:
                        Status.AppendLine("攻击力：" + ((Engine.Card.MinionCard)info).攻击力.ToString());
                        Status.AppendLine("生命值：" + ((Engine.Card.MinionCard)info).生命值上限.ToString());
                        break;
                    case CardBasicInfo.CardTypeEnum.武器:
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
                var c = CardCollections[SN].深拷贝();
                c.Init();
                if (c.CardType == CardBasicInfo.CardTypeEnum.随从) ((Engine.Card.MinionCard)c).Init();
                if (c.CardType == CardBasicInfo.CardTypeEnum.法术) ((Engine.Card.AbilityCard)c).Init();
                if (c.CardType == CardBasicInfo.CardTypeEnum.武器) ((Engine.Card.WeaponCard)c).Init();
                if (c.CardType == CardBasicInfo.CardTypeEnum.奥秘) ((Engine.Card.SecretCard)c).Init();
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
                XmlSerializer xml = new XmlSerializer(typeof(Engine.Card.AbilityCard));
                Engine.Card.AbilityCard ability = (AbilityCard)xml.Deserialize(new StreamReader(AbilityXml));
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
        #endregion
        #region"枚举值"
        /// <summary>
        /// 职业
        /// </summary>
        public enum ClassEnum
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
        public enum 种族Enum
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
        public enum TargetSelectModeEnum
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
        public enum TargetSelectDirectEnum
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
        public struct PositionSelectOption
        {
            /// <summary>
            /// 法术对象选择模式
            /// </summary>
            public CardUtility.TargetSelectModeEnum EffictTargetSelectMode;
            /// <summary>
            /// 法术对象选择角色
            /// </summary>
            public CardUtility.TargetSelectRoleEnum EffectTargetSelectRole;
            /// <summary>
            /// 法术对象选择方向
            /// </summary>
            public CardUtility.TargetSelectDirectEnum EffectTargetSelectDirect;
            /// <summary>
            /// 法术对象选择条件
            /// </summary>
            public String EffectTargetSelectCondition;
            /// <summary>
            /// 选定位置
            /// </summary>
            public TargetPosition SelectedPos;
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
        public static Boolean 符合选择条件(Engine.Card.MinionCard minion, PositionSelectOption SelectOpt)
        {
            String strCondition = SelectOpt.EffectTargetSelectCondition;
            if (String.IsNullOrEmpty(strCondition)) return true;
            foreach (var 种族名称 in Enum.GetNames(typeof(种族Enum)))
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
            switch (strCondition.Substring(1,1))
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
        public enum TargetSelectRoleEnum
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
        /// 返回值
        /// </summary>
        public enum CommandResult
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
        public enum PickEffect
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
        /// 效果回合
        /// </summary>
        public enum EffectTurn
        {
            /// <summary>
            /// 无效果
            /// </summary>
            无效果,
            /// <summary>
            /// 效果命中
            /// </summary>
            效果命中,
            /// <summary>
            /// 效果作用
            /// </summary>
            效果作用
        }
        /// <summary>
        /// 事件类型列表
        /// </summary>
        public enum 事件类型列表
        {
            无,
            施法,
            治疗,
            死亡,
            奥秘命中,
            受伤,
            召唤,
            卡牌,
        }
        /// <summary>
        /// 全局事件
        /// </summary>
        [Serializable]
        public struct 全局事件
        {
            /// <summary>
            /// 事件名称
            /// </summary>
            public 事件类型列表 事件类型;
            /// <summary>
            /// 事件效果
            /// </summary>
            public String 事件效果;
            /// <summary>
            /// 触发方向
            /// </summary>
            public TargetSelectDirectEnum 触发方向;
            /// <summary>
            /// 触发位置
            /// </summary>
            public int 触发位置;
            /// <summary>
            /// 附加信息
            /// </summary>
            public String 附加信息;
        }
        #endregion
        /// <summary>
        /// 随机打算数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static T[] RandomSort<T>(T[] array, int Seed)
        {
            int len = array.Length;
            System.Collections.Generic.List<int> list = new System.Collections.Generic.List<int>();
            T[] ret = new T[len];
            Random rand = Seed == 0 ? new Random() : new Random(DateTime.Now.Millisecond + Seed);
            int i = 0;
            while (list.Count < len)
            {
                int iter = rand.Next(0, len);
                if (!list.Contains(iter))
                {
                    list.Add(iter);
                    ret[i] = array[iter];
                    i++;
                }
            }
            return ret;
        }
        /// <summary>
        /// 获得字符枚举值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strEnum"></param>
        /// <returns></returns>
        public static T GetEnum<T>(String strEnum, T Default)
        {
            if (String.IsNullOrEmpty(strEnum)) return Default;
            try
            {
                T EnumValue = (T)Enum.Parse(typeof(T), strEnum);
                return EnumValue;
            }
            catch (Exception)
            {
                return Default;
            }
        }
        /// <summary>
        /// 数字字符转数字，错误则返回默认值
        /// </summary>
        /// <param name="StringInt"></param>
        /// <param name="DefaultValue">默认值</param>
        /// <returns></returns>
        public static int GetInt(String StringInt, int DefaultValue = 0)
        {
            if (String.IsNullOrEmpty(StringInt)) return DefaultValue;
            return int.Parse(StringInt);
        }
        /// <summary>
        /// 用户指定位置
        /// </summary>
        [Serializable]
        public struct TargetPosition
        {
            /// <summary>
            /// 本方/对方
            /// </summary>
            public Boolean MeOrYou;
            /// <summary>
            /// 0 - 英雄，1-7 随从位置
            /// </summary>
            public int Postion;
        }
        #region"委托"
        /// <summary>
        /// 抉择
        /// </summary>
        /// <param name="First">第一效果</param>
        /// <param name="Second">第二效果</param>
        /// <returns>是否为第一效果</returns>
        public delegate PickEffect delegatePickEffect(String First, String Second);
        /// <summary>
        /// 抽牌委托
        /// </summary>
        /// <param name="IsFirst">先后手区分</param>
        /// <param name="Ability">法术定义</param>
        public delegate List<String> delegateDrawCard(Boolean IsFirst, int DrawCount);
        /// <summary>
        /// 获得图片
        /// </summary>
        /// <returns></returns>
        public delegate Image delegateGetImage(String ImageKey);
        /// <summary>
        /// 获得位置
        /// </summary>
        /// <returns></returns>
        public delegate TargetPosition deleteGetTargetPosition(PositionSelectOption 选择参数);
        /// <summary>
        /// 随从进场位置
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public delegate int delegateGetPutPos(Engine.Client.GameManager game);
        #endregion
        #region"扩展方法"

        /// <summary>
        /// 深拷贝对象副本
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T 深拷贝<T>(this T obj)
        {
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            T copy = default(T);
            try
            {
                formatter.Serialize(ms, obj);
                ms.Seek(0, System.IO.SeekOrigin.Begin);
                copy = (T)formatter.Deserialize(ms);
            }
            catch (Exception ex)
            {
                throw new Exception("深拷贝对象实例出错", ex);
            }
            finally
            {
                ms.Close();
            }
            return copy;
        }
        #endregion
    }
}
