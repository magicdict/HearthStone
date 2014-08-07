using Engine.Card;
using Engine.Client;
using Engine.Control;
using Newtonsoft.Json;
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
        public static string CardXmlFolder = string.Empty;
        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init(string mCardXmlFolder)
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
        public static Dictionary<string, string> ReadyCardDic = new Dictionary<string, string>();
        /// <summary>
        /// 通过卡牌序列号获得卡牌名称
        /// </summary>
        /// <param name="SN"></param>
        /// <returns></returns>
        public static string GetCardNameBySN(string SN)
        {
            if (ReadyCardDic.ContainsKey(SN)) return ReadyCardDic[SN];
            return "UnKnow";
        }
        /// <summary>
        /// 获得卡牌信息
        /// </summary>
        /// <param name="CardSn"></param>
        /// <returns></returns>
        public static string GetCardInfo(string CardSn)
        {
            StringBuilder Status = new StringBuilder();
            if (GetCardInfoBySN(CardSn) != null)
            {
                CardBasicInfo info = GetCardInfoBySN(CardSn);
                Status.AppendLine("==============");
                Status.AppendLine("Description" + info.描述);
                Status.AppendLine("StandardCostPoint" + info.使用成本);
                Status.AppendLine("Type：" + info.卡牌种类.ToString());
                switch (info.卡牌种类)
                {
                    case CardBasicInfo.资源类型枚举.随从:
                        Status.AppendLine("攻击力：" + ((MinionCard)info).攻击力.ToString());
                        Status.AppendLine("生命值：" + ((MinionCard)info).生命值上限.ToString());
                        break;
                    case CardBasicInfo.资源类型枚举.武器:
                        Status.AppendLine("攻击力：" + ((WeaponCard)info).攻击力.ToString());
                        Status.AppendLine("耐久度：" + ((WeaponCard)info).耐久度.ToString());
                        break;
                }
                Status.AppendLine("==============");
            }
            return Status.ToString();
        }
        /// <summary>
        /// 卡牌组合
        /// </summary>
        public static Dictionary<string, CardBasicInfo> CardCollections = new Dictionary<string, CardBasicInfo>();
        /// <summary>
        /// 通过卡牌序列号获得卡牌名称
        /// </summary>
        /// <param name="SN"></param>
        /// <returns></returns>
        public static CardBasicInfo GetCardInfoBySN(string SN)
        {
            if (CardCollections.ContainsKey(SN))
            {
                var c = CardCollections[SN].DeepCopy();
                if (c.卡牌种类 == CardBasicInfo.资源类型枚举.随从) ((MinionCard)c).初始化();
                return c;
            }
            return null;
        }
        /// <summary>
        /// 从XML文件读取
        /// </summary>
        public static void GetCardInfoFromXml()
        {
            CardCollections.Clear();
            switch (SystemManager.外部资料格式)
            {
                case SystemManager.ExportType.XML:
                    //法术
                    foreach (var AbilityXml in Directory.GetFiles(CardXmlFolder + "\\Ability\\"))
                    {
                        XmlSerializer xml = new XmlSerializer(typeof(SpellCard));
                        SpellCard ability = (SpellCard)xml.Deserialize(new StreamReader(AbilityXml));
                        CardCollections.Add(ability.序列号, ability);
                    }
                    //随从
                    foreach (var MinionXml in Directory.GetFiles(CardXmlFolder + "\\Minion\\"))
                    {
                        XmlSerializer xml = new XmlSerializer(typeof(MinionCard));
                        MinionCard Minio = (MinionCard)xml.Deserialize(new StreamReader(MinionXml));
                        CardCollections.Add(Minio.序列号, Minio);
                    }
                    //武器
                    foreach (var WeaponXml in Directory.GetFiles(CardXmlFolder + "\\Weapon\\"))
                    {
                        XmlSerializer xml = new XmlSerializer(typeof(WeaponCard));
                        WeaponCard Weapon = (WeaponCard)xml.Deserialize(new StreamReader(WeaponXml));
                        CardCollections.Add(Weapon.序列号, Weapon);
                    }
                    //奥秘
                    foreach (var SecretXml in Directory.GetFiles(CardXmlFolder + "\\Secret\\"))
                    {
                        XmlSerializer xml = new XmlSerializer(typeof(SecretCard));
                        SecretCard Secret = (SecretCard)xml.Deserialize(new StreamReader(SecretXml));
                        CardCollections.Add(Secret.序列号, Secret);
                    }
                    break;
                case SystemManager.ExportType.JSON:
                    //法术
                    foreach (var AbilityXml in Directory.GetFiles(CardXmlFolder + "\\Ability\\"))
                    {
                        SpellCard ability = (SpellCard)JsonSerializer.Create().Deserialize(new StreamReader(AbilityXml), typeof(SpellCard));
                        CardCollections.Add(ability.序列号, ability);
                    }
                    //随从
                    foreach (var MinionXml in Directory.GetFiles(CardXmlFolder + "\\Minion\\"))
                    {
                        MinionCard Minio = (MinionCard)JsonSerializer.Create().Deserialize(new StreamReader(MinionXml), typeof(MinionCard));
                        CardCollections.Add(Minio.序列号, Minio);
                    }
                    //武器
                    foreach (var WeaponXml in Directory.GetFiles(CardXmlFolder + "\\Weapon\\"))
                    {
                        WeaponCard Weapon = (WeaponCard)JsonSerializer.Create().Deserialize(new StreamReader(WeaponXml), typeof(WeaponCard));
                        CardCollections.Add(Weapon.序列号, Weapon);
                    }
                    //奥秘
                    foreach (var SecretXml in Directory.GetFiles(CardXmlFolder + "\\Secret\\"))
                    {
                        SecretCard Secret = (SecretCard)JsonSerializer.Create().Deserialize(new StreamReader(SecretXml), typeof(SecretCard));
                        CardCollections.Add(Secret.序列号, Secret);
                    }
                    break;
                default:
                    break;
            }
        }

        #region"常数"
        /// <summary>
        /// 真
        /// </summary>
        public const string strTrue = "1";
        /// <summary>
        /// 假
        /// </summary>
        public const string strFalse = "0";
        /// <summary>
        /// 表示本方
        /// </summary>
        public const string strMe = "ME";
        /// <summary>
        /// 表示对方
        /// </summary>
        public const string strYou = "YOU";
        /// <summary>
        /// 分隔符号
        /// </summary>
        public const string strSplitMark = "#";
        /// <summary>
        /// 分隔符号（不同内容）
        /// </summary>
        public const string strSplitDiffMark = "$";
        /// <summary>
        /// 分隔符号(数组)
        /// </summary>
        public const string strSplitArrayMark = "|";
        /// <summary>
        /// 分隔符号(键值对)
        /// </summary>
        public const string strSplitKeyValueMark = ":";
        /// <summary>
        /// 忽略
        /// </summary>
        public const string strIgnore = "X";
        /// <summary>
        /// 最大值
        /// </summary>
        public const int Max = 999;
        /// <summary>
        /// OK
        /// </summary>
        public const string strOK = "OK";
        #endregion

        #region"枚举值"
        /// <summary>
        /// 职业
        /// </summary>
        public enum 职业枚举
        {
            中立,
            猎人,
            盗贼,
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
        /// <remarks>
        /// 虽然可以将排除指定位置的情况转化为：
        /// TrueEffecct为未指定。
        /// 但是考虑到让所有其他鱼人增加X/+2的情况
        /// 这样的话变成复合条件了
        /// </remarks>
        public enum 目标选择模式枚举
        {
            /// <summary>
            /// 无需选择
            /// </summary>
            不用选择,
            /// <summary>
            /// 自身位置
            /// </summary>
            自身,
            /// <summary>
            /// 随机
            /// </summary>
            随机,
            /// <summary>
            /// 指定
            /// </summary>
            指定,
            /// <summary>
            /// 全体
            /// </summary>
            全体,
            /// <summary>
            /// 单个目标 + 全体的模式
            /// </summary>
            横扫,
            /// <summary>
            /// 指定目标及其相邻的模式
            /// </summary>
            相邻,
            /// <summary>
            /// 指定目标的相邻的模式
            /// 通古斯防御者：相邻的单位获得嘲讽
            /// </summary>
            相邻排除指定位置,
            /// <summary>
            /// 全体排除指定位置
            /// </summary>
            全体排除指定位置
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
            public 目标选择模式枚举 EffictTargetSelectMode;
            /// <summary>
            /// 法术对象选择角色
            /// </summary>
            public 目标选择角色枚举 EffectTargetSelectRole;
            /// <summary>
            /// 法术对象选择方向
            /// </summary>
            public 目标选择方向枚举 EffectTargetSelectDirect;
            /// <summary>
            /// 法术对象选择条件
            /// </summary>
            public string EffectTargetSelectCondition;
            /// <summary>
            /// 选定位置
            /// </summary>
            public 指定位置结构体 SelectedPos;
            /// <summary>
            /// 不能选择的位置
            /// </summary>
            public 指定位置结构体 CanNotSelectPos;
            /// <summary>
            /// 嘲讽限制
            /// </summary>
            public bool 嘲讽限制;
            /// <summary>
            /// 是否需要位置选择
            /// </summary>
            public bool IsNeedTargetSelect()
            {
                return EffictTargetSelectMode == 目标选择模式枚举.指定 ||
                       EffictTargetSelectMode == 目标选择模式枚举.横扫 ||
                       EffictTargetSelectMode == 目标选择模式枚举.相邻;
            }
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
        /// 用户指定位置
        /// </summary>
        [Serializable]
        public struct 指定位置结构体
        {
            /// <summary>
            /// 本方/对方
            /// HOST/GUEST
            /// </summary>
            public bool 本方对方标识;
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
            public static 指定位置结构体 FromString(string Info)
            {
                指定位置结构体 PosInfo = new 指定位置结构体();
                PosInfo.本方对方标识 = Info.Split(strSplitMark.ToCharArray())[0] == strMe;
                PosInfo.位置 = int.Parse(Info.Split(strSplitMark.ToCharArray())[1]);
                return PosInfo;
            }
        }
        #endregion



    }
}
