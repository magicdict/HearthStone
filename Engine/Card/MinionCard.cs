using Engine.Client;
using Engine.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
namespace Engine.Card
{
    /// <summary>
    /// 随从卡牌
    /// </summary>
    [Serializable]
    public class MinionCard : CardBasicInfo
    {
        #region"结构枚举"
        /// <summary>
        /// 光环范围
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
            /// 全局
            /// </summary>
            全局
        }
        /// <summary>
        /// 攻击状态
        /// </summary>
        public enum 攻击状态
        {
            /// <summary>
            /// 新上场的随从
            /// </summary>
            准备中,
            /// <summary>
            /// 可以攻击
            /// </summary>
            可攻击,
            /// <summary>
            /// 已经攻击完毕
            /// </summary>
            攻击完毕
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
            /// 施法成本
            /// </summary>
            施法成本,
            /// <summary>
            /// 随从成本
            /// </summary>
            随从成本,
            /// <summary>
            /// 增加法术效果
            /// </summary>
            法术效果
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
        /// <summary>
        /// 战吼类型列表
        /// </summary>
        public enum 战吼类型列表
        {
            /// <summary>
            /// 默认
            /// </summary>
            默认,
            /// <summary>
            /// 入场之前进行结算
            /// </summary>
            抢先,
            /// <summary>
            /// 入场后相邻随从
            /// </summary>
            相邻,
            /// <summary>
            /// 自身
            /// </summary>
            自身,
        }
        /// <summary>
        /// 特殊效果列表
        /// </summary>
        public enum 特殊效果列表
        {
            无效果,
            /// <summary>
            /// 古拉巴什狂暴者:每当该随从受到伤害时，获得+3攻击力。
            /// </summary>
            持续激怒,
            /// <summary>
            /// 帝王眼镜蛇:消灭任何受到该随从伤害的随从。
            /// </summary>
            攻击必死,
            /// <summary>
            /// 回合结束时候死亡
            /// </summary>
            回合结束死亡
        }
        #endregion

        #region"属性"
        #region"设计时状态"
        /// <summary>
        /// 
        /// </summary>
        public 特殊效果列表 特殊效果 = 特殊效果列表.无效果;
        /// <summary>
        /// 种族
        /// </summary>
        public CardUtility.种族Enum 种族 = CardUtility.种族Enum.无;
        /// <summary>
        /// 攻击力
        /// </summary>
        public int 攻击力 = 0;
        /// <summary>
        /// 生命值
        /// </summary>
        public int 生命值 = 0;
        /// <summary>
        /// 体力
        /// </summary>
        public int 生命值上限 = 0;
        /// <summary>
        /// 嘲讽
        /// </summary>
        public Boolean 嘲讽特性 = false;
        /// <summary>
        /// 冲锋
        /// </summary>
        public Boolean 冲锋特性 = false;
        /// <summary>
        /// 风怒
        /// </summary>
        public Boolean 风怒特性 = false;
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
        public Boolean 无法攻击特性 = false;
        /// <summary>
        /// 战吼(效果号码)
        /// </summary>
        public String 战吼效果 = String.Empty;
        /// <summary>
        /// 战吼类型
        /// </summary>
        public 战吼类型列表 战吼类型 = 战吼类型列表.默认;
        /// <summary>
        /// 亡语(效果号码)
        /// </summary>
        public String 亡语效果 = String.Empty;
        /// <summary>
        /// 激怒(效果号码)
        /// </summary>
        public String 激怒效果 = String.Empty;
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
        /// <summary>
        /// 自身事件
        /// </summary>
        public Engine.Utility.CardUtility.事件效果 自身事件效果 = new CardUtility.事件效果();
        #endregion

        #region"运行时状态"
        /// <summary>
        /// 受过伤害
        /// </summary>
        [XmlIgnore]
        public Boolean 受过伤害 = false;
        /// <summary>
        /// 沉默状态
        /// </summary>
        [XmlIgnore]
        public Boolean 沉默状态 = false;
        /// <summary>
        /// 激怒状态
        /// </summary>
        [XmlIgnore]
        public Boolean 激怒状态 = false;
        /// <summary>
        /// 是否为冰冻状态
        /// </summary>
        [XmlIgnore]
        public Engine.Utility.CardUtility.EffectTurn 冰冻状态 = CardUtility.EffectTurn.无效果;
        /// <summary>
        /// 剩余攻击次数
        /// </summary>
        /// <remarks>
        /// 风怒的时候，回合开始为2次
        /// 刚放到战场时，冲锋的单位为1次，其余为0次
        /// </remarks>
        [XmlIgnore]
        public int 剩余攻击次数 = 1;
        /// <summary>
        /// 攻击状态
        /// </summary>
        [XmlIgnore]
        public 攻击状态 AttactStatus = 攻击状态.准备中;
        /// <summary>
        /// 战场位置
        /// </summary>
        [XmlIgnore]
        public Engine.Utility.CardUtility.TargetPosition 战场位置;
        /// <summary>
        /// 该单位受到战地的效果
        /// </summary>
        [XmlIgnore]
        public List<Buff> 受战场效果 = new List<Buff>();
        /// <summary>
        /// 能否成为当前动作的对象
        /// </summary>
        [XmlIgnore]
        public Boolean 能否成为动作对象 = false;
        #endregion

        #region"回合效果"
        /// <summary>
        /// 
        /// </summary>
        public int 本回合攻击力加成 = 0;
        /// <summary>
        /// 
        /// </summary>
        public int 本回合生命力加成 = 0;
        #endregion
        #endregion

        #region"方法"
        /// <summary>
        /// 设置随从初始状态
        /// </summary>
        public new void Init()
        {
            //初始状态
            生命值 = 生命值上限;
            this.冰冻状态 = CardUtility.EffectTurn.无效果;
            //攻击次数
            if (风怒特性)
            {
                剩余攻击次数 = 2;
            }
            else
            {
                剩余攻击次数 = 1;
            }
            if (冲锋特性)
            {
                this.AttactStatus = 攻击状态.可攻击;
            }
            else
            {
                this.AttactStatus = 攻击状态.准备中;
            }
        }
        /// <summary>
        /// 重置可攻击次数
        /// </summary>
        public void ResetAttackTimes()
        {
            if (风怒特性)
            {
                剩余攻击次数 = 2;
            }
            else
            {
                剩余攻击次数 = 1;
            }
            AttactStatus = 攻击状态.可攻击;
        }
        /// <summary>
        /// 能否攻击
        /// </summary>
        /// <returns></returns>
        public Boolean CanAttack()
        {
            if (冰冻状态 != CardUtility.EffectTurn.无效果) return false;
            if (无法攻击特性) return false;
            if (TotalAttack() == 0) return false;
            return 剩余攻击次数 > 0 && AttactStatus == 攻击状态.可攻击;
        }
        /// <summary>
        /// 实际输出效果
        /// </summary>
        /// <returns>包含了光环/激怒效果</returns>
        public int TotalAttack()
        {
            int rtnAttack = 攻击力;
            foreach (var buff in 受战场效果)
            {
                rtnAttack += int.Parse(buff.BuffInfo.Split("/".ToCharArray())[0]);
            }
            //激怒效果
            if (!沉默状态 && 激怒状态)
            {
                if (!String.IsNullOrEmpty(激怒效果)) rtnAttack += int.Parse(激怒效果);
            }
            rtnAttack += 本回合攻击力加成;
            if (特殊效果 == 特殊效果列表.攻击必死 && !沉默状态) rtnAttack = 999;
            return rtnAttack;
        }
        /// <summary>
        /// 生命值上限
        /// </summary>
        /// <returns>包含了光环效果</returns>
        public int 合计生命值上限()
        {
            int BuffLife = 0;
            foreach (var buff in 受战场效果)
            {
                BuffLife += int.Parse(buff.BuffInfo.Split("/".ToCharArray())[1]);
            }
            return 生命值上限 + BuffLife + 本回合生命力加成;
        }
        /// <summary>
        /// 发动战吼(默认)
        /// </summary>
        /// <param name="game"></param>
        /// <param name="SelfPosition"></param>
        /// <returns></returns>
        public List<String> 发动战吼(GameManager game)
        {
            List<String> ActionCodeLst = new List<string>();
            //战吼效果
            if (战吼效果 != String.Empty && !沉默状态)
            {
                var 战吼Result = RunAction.StartAction(game, 战吼效果);
                //第一条是使用了战吼卡牌的消息，如果不除去，对方客户端会认为使用了一张卡牌
                //如果战吼在召唤的时候无法成功，法术机能会误认为是取消
                if (战吼Result.Count > 0) 战吼Result.RemoveAt(0);
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
            if (亡语效果 != String.Empty && !沉默状态)
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
            if (回合开始效果 != String.Empty && !沉默状态)
            {
                var 回合开始Result = RunAction.StartAction(game, 回合开始效果);
                //第一条是使用了亡语卡牌的消息，如果不除去，对方客户端会认为使用了一张卡牌
                回合开始Result.RemoveAt(0);
                ActionCodeLst.AddRange(回合开始Result);
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
            if (回合结束效果 != String.Empty && !沉默状态)
            {
                var 回合结束Result = RunAction.StartAction(game, 回合结束效果);
                //第一条是使用了亡语卡牌的消息，如果不除去，对方客户端会认为使用了一张卡牌
                回合结束Result.RemoveAt(0);
                ActionCodeLst.AddRange(回合结束Result);
            }
            本回合生命力加成 = 0;
            本回合攻击力加成 = 0;
            return ActionCodeLst;
        }
        /// <summary>
        /// 攻击后
        /// </summary>
        public void AfterDoAttack(Boolean 被动攻击)
        {
            //失去潜行
            if (!被动攻击)
            {
                潜行特性 = false;
                剩余攻击次数--;
                if (剩余攻击次数 == 0) AttactStatus = MinionCard.攻击状态.攻击完毕;
            }
        }
        /// <summary>
        /// 被攻击
        /// </summary>
        /// <returns>是否产生实际伤害</returns>
        public Boolean AfterBeAttack(int AttackPoint)
        {
            if (圣盾特性)
            {
                圣盾特性 = false;
                return false;
            }
            else
            {
                生命值 -= AttackPoint;
                圣盾特性 = false;
            }
            //失去圣盾
            if (AttackPoint > 0)
            {
                受过伤害 = true;
                if (!String.IsNullOrEmpty(激怒效果)) 激怒状态 = true;
                if (特殊效果 == 特殊效果列表.持续激怒 && !沉默状态) 攻击力 += 3;
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 被治疗
        /// </summary>
        /// <param name="HealthPoint"></param>
        /// <returns>是否产生实际治疗作用</returns>
        public Boolean AfterBeHealth(int HealthPoint)
        {
            if (生命值 == 生命值上限) return false;
            生命值 += HealthPoint;
            if (生命值 > 生命值上限) 生命值 = 生命值上限;
            //取消风怒
            if (生命值 == 生命值上限) 激怒状态 = false;
            return true;
        }
        /// <summary>
        /// 事件处理方法
        /// </summary>
        /// <param name="事件"></param>
        /// <param name="game"></param>
        /// <param name="MyPos"></param>
        /// <returns></returns>
        public List<String> 事件处理方法(Engine.Utility.CardUtility.全局事件 事件, GameManager game, String MyPos)
        {
            List<String> ActionLst = new List<string>();
            if (!沉默状态 && 自身事件效果.触发效果事件类型 == 事件.触发事件类型)
            {
                //if (自身事件.触发位置.本方对方标识 == CardUtility.TargetSelectDirectEnum.双方)
                //{
                //    if (自身事件.触发方向 != 事件.触发方向) return ActionLst;
                //}
                //if (!String.IsNullOrEmpty(自身事件.附加信息) && (事件.附加信息 != 自身事件.附加信息)) return ActionLst;
                //ActionLst.Add(Engine.Server.ActionCode.strHitEvent + CardUtility.strSplitMark);
                //if (自身事件.事件效果.StartsWith("A"))
                //{
                //    //ActionLst.AddRange(((Card.AbilityCard)Card.CardUtility.GetCardInfoBySN(自身事件.事件效果)).UseAbility(gmae, false));
                //}
                //else
                //{
                //    //Card.Effect.PointEffect.RunPointEffect(this, 自身事件.事件效果);
                //    ActionLst.Add(Engine.Server.ActionCode.strPoint + CardUtility.strSplitMark + MyPos + CardUtility.strSplitMark + 自身事件.事件效果);
                //}
            }
            return ActionLst;
        }
        /// <summary>
        /// 获得信息
        /// </summary>
        /// <returns></returns>
        public new String GetInfo()
        {
            StringBuilder Status = new StringBuilder();
            Status.AppendLine(名称);
            Status.AppendLine("[状]" + (圣盾特性 ? "圣" : String.Empty) +
                                       (嘲讽特性 ? "|嘲" : String.Empty) +
                                       (风怒特性 ? "|风" : String.Empty) +
                                       (冲锋特性 ? "|冲" : String.Empty) +
                                       (冰冻状态 != CardUtility.EffectTurn.无效果 ? "冻" : String.Empty));
            Status.AppendLine("[实]" + 攻击力.ToString() + "/" + 生命值.ToString() +
                              "[总]" + TotalAttack().ToString() + "/" + 生命值.ToString());
            return Status.ToString();
        }
        #endregion
    }
}
