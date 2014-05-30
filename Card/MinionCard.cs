using Card.Client;
using Card.Effect;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
namespace Card
{
    /// <summary>
    /// 随从卡牌
    /// </summary>
    [Serializable]
    public class MinionCard : CardBasicInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public enum 光环范围
        {
            /// <summary>
            /// 随从全体
            /// </summary>
            随从全体,
            /// <summary>
            /// 相邻随从
            /// </summary>
            相邻随从,
            /// <summary>
            /// 英雄
            /// </summary>
            英雄
        }
        /// <summary>
        /// 光环类型
        /// </summary>
        public enum 光环类型
        {
            /// <summary>
            /// 增加攻防
            /// </summary>
            增加攻防,
            /// <summary>
            /// 减少施法成本
            /// </summary>
            减少施法成本,
            /// <summary>
            /// 增加法术效果
            /// </summary>
            增加法术效果
        }
        /// <summary>
        /// 效果
        /// </summary>
        [Serializable]
        public struct Buff
        {
            /// <summary>
            /// 范围
            /// </summary>
            public 光环范围 Scope;
            /// <summary>
            /// 效果
            /// </summary>
            public 光环类型 EffectType;
            /// <summary>
            /// 信息
            /// </summary>
            public String BuffInfo;
            /// <summary>
            /// 效果来源
            /// </summary>
            public String Name;
        }

        #region"属性"

        #region"设计时状态"
        /// <summary>
        /// 攻击力（标准）
        /// </summary>
        public int StandardAttackPoint = -1;
        /// <summary>
        /// 体力（标准）
        /// </summary>
        public int 标准生命值上限 = -1;
        /// <summary>
        /// 嘲讽(标准)
        /// </summary>
        public Boolean Standard嘲讽 = false;
        /// <summary>
        /// 冲锋(标准)
        /// </summary>
        public Boolean Standard冲锋 = false;
        /// <summary>
        /// 风怒(标准)
        /// </summary>
        public Boolean Standard风怒 = false;
        /// <summary>
        /// 是否初始为潜行状态
        /// </summary>
        public Boolean 潜行特性 = false;
        /// <summary>
        /// 是否初始为圣盾状态
        /// </summary>
        public Boolean 圣盾特性 = false;
        /// <summary>
        /// 法术免疫
        /// </summary>
        public Boolean 法术免疫特性 = false;
        /// <summary>
        /// 英雄技能免疫
        /// </summary>
        public Boolean 英雄技能免疫特性 = false;
        /// <summary>
        /// 不能攻击
        /// </summary>
        public Boolean Standard不能攻击 = false;
        /// <summary>
        /// 战吼(效果号码)
        /// </summary>
        public String 战吼效果 = String.Empty;
        /// <summary>
        /// 亡语(效果号码)
        /// </summary>
        public String 亡语效果 = String.Empty;
        /// <summary>
        /// 激怒(效果号码)
        /// </summary>
        public String 激怒效果 = String.Empty;
        /// <summary>
        /// 连击(效果号码)
        /// </summary>
        public String 连击效果 = String.Empty;
        /// <summary>
        /// 回合开始(效果号码)
        /// </summary>
        public String 回合开始效果 = String.Empty;
        /// <summary>
        /// 回合结束(效果号码)
        /// </summary>
        public String 回合结束效果 = String.Empty;
        /// <summary>
        /// 该单位在战地时的效果
        /// </summary>
        public Buff 光环效果;
        #endregion

        #region"运行时状态"
        /// <summary>
        /// 攻击力（实际、不包含光环效果）
        /// </summary>
        [XmlIgnore]
        public int 实际攻击力 = -1;
        /// <summary>
        /// 体力（实际）
        /// </summary>
        [XmlIgnore]
        public int 实际生命值上限 = -1;
        /// <summary>
        /// 体力（实际）
        /// </summary>
        [XmlIgnore]
        public int 实际生命值 = -1;
        [XmlIgnore]
        public Boolean 受过伤害 = false;
        /// <summary>
        /// 嘲讽(实际)
        /// </summary>
        [XmlIgnore]
        public Boolean Actual嘲讽 = false;
        /// <summary>
        /// 冲锋(实际)
        /// </summary>
        [XmlIgnore]
        public Boolean Actual冲锋 = false;
        /// <summary>
        /// 风怒(实际)
        /// </summary>
        [XmlIgnore]
        public Boolean Actual风怒 = false;
        /// <summary>
        /// 
        /// </summary>
        public Boolean Actual不能攻击 = false;
        /// <summary>
        /// 是否为潜行状态
        /// </summary>
        [XmlIgnore]
        public Boolean Is潜行Status = false;
        /// <summary>
        /// 是否为圣盾状态
        /// </summary>
        [XmlIgnore]
        public Boolean Is圣盾Status = false;
        /// <summary>
        /// 是否为法术免疫状态
        /// </summary>
        [XmlIgnore]
        public Boolean Is法术免疫Status = false;
        /// <summary>
        /// 是否为英雄技能免疫状态
        /// </summary>
        [XmlIgnore]
        public Boolean Is英雄技能免疫Status = false;
        /// <summary>
        /// 是否为激怒状态
        /// </summary>
        [XmlIgnore]
        public Boolean Is激怒Status = false;
        /// <summary>
        /// 是否为沉默状态
        /// </summary>
        [XmlIgnore]
        public Boolean Is沉默Status = false;
        /// <summary>
        /// 是否为冰冻状态
        /// </summary>
        [XmlIgnore]
        public Card.CardUtility.EffectTurn 冰冻状态 = CardUtility.EffectTurn.无效果;
        /// <summary>
        /// 剩余攻击次数
        /// </summary>
        /// <remarks>
        /// 风怒的时候，回合开始为2次
        /// 刚放到战场时，冲锋的单位为1次，其余为0次
        /// </remarks>
        [XmlIgnore]
        public int RemainAttactTimes = 1;
        /// <summary>
        /// 该单位受到战地的效果
        /// </summary>
        [XmlIgnore]
        public List<Buff> 受战地效果 = new List<Buff>();
        #endregion
        /// <summary>
        /// 设置初始状态
        /// </summary>
        public new void Init()
        {
            //将运行时状态设置为设计时状态
            this.实际攻击力 = this.StandardAttackPoint;
            this.ActualCostPoint = this.StandardCostPoint;
            this.实际生命值上限 = this.标准生命值上限;
            this.实际生命值 = this.标准生命值上限;
            this.Actual冲锋 = this.Standard冲锋;
            this.Actual嘲讽 = this.Standard嘲讽;
            this.Actual风怒 = this.Standard风怒;
            this.Actual不能攻击 = this.Standard不能攻击;
            this.Is潜行Status = this.潜行特性;
            this.Is圣盾Status = this.圣盾特性;
            this.Is英雄技能免疫Status = this.英雄技能免疫特性;
            this.Is法术免疫Status = this.法术免疫特性;
            //初始状态
            this.冰冻状态 = CardUtility.EffectTurn.无效果;
            this.Is沉默Status = false;
            this.Is激怒Status = false;
            //攻击次数
            if (Actual冲锋)
            {
                if (Actual风怒)
                {
                    RemainAttactTimes = 2;
                }
                else
                {
                    RemainAttactTimes = 1;
                }
            }
            else
            {
                RemainAttactTimes = 0;
            }
        }
        /// <summary>
        /// 重置可攻击次数
        /// </summary>
        public void ResetAttackTimes()
        {
            if (Actual风怒)
            {
                RemainAttactTimes = 2;
            }
            else
            {
                RemainAttactTimes = 1;
            }
        }
        /// <summary>
        /// 能否攻击
        /// </summary>
        /// <returns></returns>
        public Boolean CanAttack()
        {
            if (冰冻状态 != CardUtility.EffectTurn.无效果) return false;
            if (Actual不能攻击) return false;
            return RemainAttactTimes > 0;
        }

        /// <summary>
        /// 实际输出效果
        /// </summary>
        /// <returns>包含了光环效果</returns>
        public int TotalAttack()
        {
            int BuffAct = 0;
            foreach (var buff in 受战地效果)
            {
                BuffAct += int.Parse(buff.BuffInfo.Split("/".ToCharArray())[0]);
            }
            //激怒效果
            if (!Is沉默Status)
            {

            }
            return 实际攻击力 + BuffAct;
        }
        /// <summary>
        /// 实际生命值上限
        /// </summary>
        /// <returns>包含了光环效果</returns>
        public int 合计生命值上限()
        {
            int BuffAct = 0;
            foreach (var buff in 受战地效果)
            {
                BuffAct += int.Parse(buff.BuffInfo.Split("/".ToCharArray())[1]);
            }
            return 标准生命值上限 + BuffAct;
        }
        /// <summary>
        /// 生存状态
        /// </summary>
        /// <returns></returns>
        public bool IsLive()
        {
            return 实际生命值 > 0;
        }
        /// <summary>
        /// 发动战吼
        /// </summary>
        /// <returns></returns>
        public List<String> 发动战吼(GameManager game)
        {
            List<String> ActionCodeLst = new List<string>();
            //战吼效果
            if (战吼效果 != String.Empty)
            {
                var 战吼Result = RunAction.StartAction(game, 战吼效果);
                //第一条是使用了战吼卡牌的消息，如果不除去，对方客户端会认为使用了一张卡牌
                战吼Result.RemoveAt(0);
                ActionCodeLst.AddRange(战吼Result);
            }
            return ActionCodeLst;
        }
        /// <summary>
        /// 发动亡语
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public List<String> 发动亡语(GameManager game, Boolean IsNeedConvertPosDirect)
        {
            List<String> ActionCodeLst = new List<string>();
            //亡语效果
            if (亡语效果 != String.Empty && !Is沉默Status)
            {
                var 战吼Result = RunAction.StartAction(game, 亡语效果, IsNeedConvertPosDirect);
                //第一条是使用了亡语卡牌的消息，如果不除去，对方客户端会认为使用了一张卡牌
                战吼Result.RemoveAt(0);
                ActionCodeLst.AddRange(战吼Result);
            }
            return ActionCodeLst;
        }
        /// <summary>
        /// 回合开始效果
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public List<String> 回合开始(GameManager game)
        {
            List<String> ActionCodeLst = new List<string>();
            //回合开始效果
            if (回合开始效果 != String.Empty && !Is沉默Status)
            {
                var 战吼Result = RunAction.StartAction(game, 回合开始效果);
                //第一条是使用了亡语卡牌的消息，如果不除去，对方客户端会认为使用了一张卡牌
                战吼Result.RemoveAt(0);
                ActionCodeLst.AddRange(战吼Result);
            }
            return ActionCodeLst;
        }
        /// <summary>
        /// 回合结束效果
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public List<String> 回合结束(GameManager game)
        {
            List<String> ActionCodeLst = new List<string>();
            //回合结束效果
            if (回合结束效果 != String.Empty && !Is沉默Status)
            {
                var 战吼Result = RunAction.StartAction(game, 回合结束效果);
                //第一条是使用了亡语卡牌的消息，如果不除去，对方客户端会认为使用了一张卡牌
                战吼Result.RemoveAt(0);
                ActionCodeLst.AddRange(战吼Result);
            }
            return ActionCodeLst;
        }
        /// <summary>
        /// 沉默
        /// </summary>
        public void 被沉默()
        {
            Is沉默Status = true;
            Actual嘲讽 = false;
            Actual冲锋 = false;
            Actual风怒 = false;
            Actual不能攻击 = false;
        }
        /// <summary>
        /// 攻击后
        /// </summary>
        public void AfterAttack()
        {
            //失去潜行
            Is潜行Status = false;
        }
        /// <summary>
        /// 被攻击后
        /// </summary>
        public void AfterBeAttack(int AttackPoint)
        {
            if (!Is圣盾Status) 实际生命值 -= AttackPoint;
            //失去圣盾
            Is圣盾Status = false;
        }
        /// <summary>
        /// 获得信息
        /// </summary>
        /// <returns></returns>
        public new String GetInfo()
        {
            StringBuilder Status = new StringBuilder();
            Status.AppendLine(Name);
            Status.AppendLine("[状]" + (Is圣盾Status ? "圣" : String.Empty) + 
                                       (Actual嘲讽 ? "|嘲" : String.Empty) + 
                                       (Actual风怒 ? "|风" : String.Empty) +
                                       (Actual冲锋 ? "|冲" : String.Empty) + 
                                       (冰冻状态!= CardUtility.EffectTurn.无效果?"冻":String.Empty));
            Status.AppendLine("[实]" + 实际攻击力.ToString() + "/" + 实际生命值.ToString() +
                              "[总]" + TotalAttack().ToString() + "/" + 实际生命值.ToString());
            return Status.ToString();
        }
        #endregion
    }
}
