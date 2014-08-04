using Engine.Action;
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
        /// 攻击状态[Attack Status]
        /// </summary>
        public enum 攻击状态枚举
        {
            /// <summary>
            /// 新上场的随从[Sleeping]
            /// </summary>
            准备中,
            /// <summary>
            /// 可以攻击[Can Attack]
            /// </summary>
            可攻击,
            /// <summary>
            /// 已经攻击完毕[Already Attacked]
            /// </summary>
            攻击完毕
        }
        /// <summary>
        /// 特殊效果枚举[Special Effect Enum]
        /// </summary>
        public enum 特殊效果枚举
        {
            /// <summary>
            /// None
            /// </summary>
            无效果,
            /// <summary>
            /// 古拉巴什狂暴者:每当该随从受到伤害时，获得+3攻击力。
            /// [The Gurubashi Berserker: +3 When be attacked]
            /// </summary>
            持续激怒,
            /// <summary>
            /// 帝王眼镜蛇:消灭任何受到该随从伤害的随从。
            /// [King Cobra:Destory Minion]
            /// </summary>
            攻击必死,
            /// <summary>
            /// 回合结束时候死亡[Die at the end of turn]
            /// </summary>
            回合结束死亡
        }
        #endregion

        #region"属性"

        #region"设计时状态"
        /// <summary>
        /// 种族[]
        /// </summary>
        public CardUtility.种族枚举 种族 = CardUtility.种族枚举.无;
        /// <summary>
        /// 攻击力[Basic Attack Point]
        /// </summary>
        public int 攻击力 = 0;
        /// <summary>
        /// 生命值[Health Point]
        /// </summary>
        public int 生命值 = 0;
        /// <summary>
        /// 体力[Health Point Limit]
        /// </summary>
        public int 生命值上限 = 0;
        /// <summary>
        /// 嘲讽[Taunt]
        /// </summary>
        public bool 嘲讽特性 = false;
        /// <summary>
        /// 冲锋[Charge]
        /// </summary>
        public bool 冲锋特性 = false;
        /// <summary>
        /// 风怒[Windfury]
        /// </summary>
        public bool 风怒特性 = false;
        /// <summary>
        /// 潜行[Stealth]
        /// </summary>
        public bool 潜行特性 = false;
        /// <summary>
        /// 圣盾[Divine Shield]
        /// </summary>
        public bool 圣盾特性 = false;
        /// <summary>
        /// 法术免疫 [Ability Immune]
        /// </summary>
        public bool 法术免疫特性 = false;
        /// <summary>
        /// 英雄技能免疫[Hero Skill Immune]
        /// </summary>
        public bool 英雄技能免疫特性 = false;
        /// <summary>
        /// 不能攻击[Can't Attack]
        /// </summary>
        public bool 无法攻击特性 = false;
        #endregion

        #region"效果"
        /// <summary>
        /// 战吼(效果号码)[Battlecry Effect]
        /// </summary>
        public string 战吼效果 = string.Empty;
        /// <summary>
        /// 亡语(效果号码)[DeathRattle Effect]
        /// </summary>
        public string 亡语效果 = string.Empty;
        /// <summary>
        /// 激怒(效果号码)[Enrage Effect]
        /// </summary>
        public string 激怒效果 = string.Empty;
        /// <summary>
        /// 回合开始(效果号码)[Trun Start Effect]
        /// </summary>
        public string 回合开始效果 = string.Empty;
        /// <summary>
        /// 回合结束(效果号码)[Trun End Effect]
        /// </summary>
        public string 回合结束效果 = string.Empty;
        /// <summary>
        /// 光环效果 [Buff Effect]
        /// </summary>
        public Buff.光环结构体 光环效果;
        /// <summary>
        /// 特殊效果 [Specical Effect]
        /// </summary>
        public 特殊效果枚举 特殊效果 = 特殊效果枚举.无效果;
        /// <summary>
        /// 自身事件 [Event Effect]
        /// </summary>
        public CardUtility.事件效果结构体 自身事件效果 = new CardUtility.事件效果结构体();
        #endregion

        #region"运行时状态"
        /// <summary>
        /// 受过伤害[Damaged]
        /// </summary>
        [XmlIgnore]
        public bool 受过伤害 = false;
        /// <summary>
        /// 沉默状态[Slience Status]
        /// </summary>
        [XmlIgnore]
        public bool 沉默状态 = false;
        /// <summary>
        /// 激怒状态[Enrage Status]
        /// </summary>
        [XmlIgnore]
        public bool 激怒状态 = false;
        /// <summary>
        /// 剩余攻击次数[Remain Attack Count]
        /// </summary>
        /// <remarks>
        /// 风怒的时候，回合开始为2次
        /// 刚放到战场时，冲锋的单位为1次，其余为0次
        /// </remarks>
        [XmlIgnore]
        public int 剩余攻击次数 = 1;
        /// <summary>
        /// 战场位置[BattleFiled Position of the minion]
        /// </summary>
        [XmlIgnore]
        public CardUtility.指定位置结构体 战场位置;
        /// <summary>
        /// 该单位受到战地的效果[Get Buff Effect]
        /// </summary>
        [XmlIgnore]
        public List<Buff.光环结构体> 受战场效果 = new List<Buff.光环结构体>();
        /// <summary>
        /// 能否成为当前动作的对象[Can be the target of action]
        /// </summary>
        [XmlIgnore]
        public bool 能否成为动作对象 = false;
        #endregion

        #region"回合效果"
        /// <summary>
        /// 攻击状态
        /// </summary>
        [XmlIgnore]
        public 攻击状态枚举 攻击状态 = 攻击状态枚举.准备中;
        /// <summary>
        /// 是否为冰冻状态[Freeze Status]
        /// </summary>
        [XmlIgnore]
        public CardUtility.效果回合枚举 冰冻状态 = CardUtility.效果回合枚举.无效果;
        /// <summary>
        /// 
        /// </summary>
        public int 本回合攻击力加成 = 0;
        /// <summary>
        /// 
        /// </summary>
        public int 本回合生命力加成 = 0;
        #endregion

        #region"动态属性"
        /// <summary>
        /// 能否攻击
        /// </summary>
        /// <returns></returns>
        public bool 能否攻击
        {
            get
            {
                if (冰冻状态 != CardUtility.效果回合枚举.无效果) return false;
                if (无法攻击特性) return false;
                if (实际攻击值 == 0) return false;
                return 剩余攻击次数 > 0 && 攻击状态 == 攻击状态枚举.可攻击;
            }
        }
        /// <summary>
        /// 实际输出效果
        /// </summary>
        /// <returns>包含了光环/激怒效果</returns>
        public int 实际攻击值
        {
            get
            {
                int rtnAttack = 攻击力;
                foreach (var buff in 受战场效果)
                {
                    rtnAttack += int.Parse(buff.信息.Split("/".ToCharArray())[0]);
                }
                //激怒效果
                if (!沉默状态 && 激怒状态)
                {
                    if (!string.IsNullOrEmpty(激怒效果)) rtnAttack += int.Parse(激怒效果);
                }
                rtnAttack += 本回合攻击力加成;
                if (特殊效果 == 特殊效果枚举.攻击必死 && !沉默状态) rtnAttack = 999;
                return rtnAttack;
            }
        }
        /// <summary>
        /// 完整状态[未被使用]
        /// </summary>
        public string 完整状态
        {
            get
            {
                StringBuilder Status = new StringBuilder();
                Status.AppendLine(名称);
                Status.AppendLine("[状]" + (圣盾特性 ? "圣" : string.Empty) +
                                           (嘲讽特性 ? "|嘲" : string.Empty) +
                                           (风怒特性 ? "|风" : string.Empty) +
                                           (冲锋特性 ? "|冲" : string.Empty) +
                                           (潜行特性 ? "|潜" : string.Empty) +
                                           (冰冻状态 != CardUtility.效果回合枚举.无效果 ? "冻" : string.Empty));
                Status.AppendLine("[实]" + 攻击力.ToString() + "/" + 生命值.ToString() +
                                  "[总]" + 实际攻击值.ToString() + "/" + 生命值.ToString());
                return Status.ToString();
            }
        }
        /// <summary>
        /// 状态
        /// </summary>
        public string 状态
        {
            get
            {
                return ((圣盾特性 ? "圣" : string.Empty) +
                       (嘲讽特性 ? "|嘲" : string.Empty) +
                       (风怒特性 ? "|风" : string.Empty) +
                       (冲锋特性 ? "|冲" : string.Empty) +
                       (潜行特性 ? "|潜" : string.Empty) +
                       (冰冻状态 != CardUtility.效果回合枚举.无效果 ? "冻" : string.Empty)).TrimStart("|".ToCharArray());
            }
        }
        /// <summary>
        /// 生命值上限[未被使用]
        /// </summary>
        public int 合计生命值上限
        {
            get
            {
                int BuffLife = 0;
                foreach (var buff in 受战场效果)
                {
                    BuffLife += int.Parse(buff.信息.Split("/".ToCharArray())[1]);
                }
                return 生命值上限 + BuffLife + 本回合生命力加成;
            }
        }
        #endregion

        #endregion

        #region"方法"
        /// <summary>
        /// 设置随从初始状态
        /// </summary>
        public void 初始化()
        {
            //初始状态
            生命值 = 生命值上限;
            冰冻状态 = CardUtility.效果回合枚举.无效果;
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
                攻击状态 = 攻击状态枚举.可攻击;
            }
            else
            {
                攻击状态 = 攻击状态枚举.准备中;
            }
        }
        /// <summary>
        /// 重置可攻击次数
        /// </summary>
        public void 重置剩余攻击次数()
        {
            if (风怒特性)
            {
                剩余攻击次数 = 2;
            }
            else
            {
                剩余攻击次数 = 1;
            }
            攻击状态 = 攻击状态枚举.可攻击;
        }
        /// <summary>
        /// 发动战吼(暂时没有使用的方法)
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public List<string> 发动战吼(ActionStatus game)
        {
            List<string> ActionCodeLst = new List<string>();
            //战吼效果
            if (战吼效果 != string.Empty && !沉默状态)
            {
                game.Interrupt.Step = 1;
                var 战吼Result = RunAction.StartAction(game, 战吼效果);
                //第一条是使用了战吼卡牌的消息，如果不除去，对方客户端会认为使用了一张卡牌
                //如果战吼在召唤的时候无法成功，法术机能会误认为是取消
                if (战吼Result.Count > 0) 战吼Result.RemoveAt(0);
                ActionCodeLst.AddRange(战吼Result);
            }
            return ActionCodeLst;
        }
        /// <summary>
        /// 发动亡语（CS/BS)
        /// </summary>
        /// <param name="game"></param>
        /// <param name="IsMyAction"></param>
        /// <returns></returns>
        public List<string> 发动亡语(ActionStatus game)
        {
            List<string> ActionCodeLst = new List<string>();
            //亡语效果
            if (亡语效果 != string.Empty && !沉默状态)
            {
                //BS模式下面，必须设置Step
                game.Interrupt.Step = 1;
                var 亡语Result = RunAction.StartAction(game, 亡语效果);
                //第一条是使用了亡语卡牌的消息，如果不除去，对方客户端会认为使用了一张卡牌
                if (亡语Result.Count > 0) 亡语Result.RemoveAt(0);
                ActionCodeLst.AddRange(亡语Result);
            }
            return ActionCodeLst;
        }
        /// <summary>
        /// 回合开始效果
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public List<string> 回合开始(ActionStatus game)
        {
            List<string> ActionCodeLst = new List<string>();
            //回合开始效果
            if (回合开始效果 != string.Empty && !沉默状态)
            {
                var 回合开始Result = RunAction.StartAction(game, 回合开始效果);
                //第一条是使用了亡语卡牌的消息，如果不除去，对方客户端会认为使用了一张卡牌
                if (回合开始Result.Count > 0) 回合开始Result.RemoveAt(0);
                ActionCodeLst.AddRange(回合开始Result);
                潜行特性 = false;
            }
            return ActionCodeLst;
        }
        /// <summary>
        /// 回合结束效果
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public List<string> 回合结束(ActionStatus game)
        {
            List<string> ActionCodeLst = new List<string>();
            //回合结束效果
            if (回合结束效果 != string.Empty && !沉默状态)
            {
                var 回合结束Result = RunAction.StartAction(game, 回合结束效果);
                //第一条是使用了亡语卡牌的消息，如果不除去，对方客户端会认为使用了一张卡牌
                if (回合结束Result.Count > 0) 回合结束Result.RemoveAt(0);
                ActionCodeLst.AddRange(回合结束Result);
                潜行特性 = false;
            }
            本回合生命力加成 = 0;
            本回合攻击力加成 = 0;
            return ActionCodeLst;
        }
        /// <summary>
        /// 设置攻击后状态
        /// </summary>
        public void 设置攻击后状态()
        {
            //失去潜行
            潜行特性 = false;
            剩余攻击次数--;
            if (剩余攻击次数 == 0) 攻击状态 = 攻击状态枚举.攻击完毕;
        }
        /// <summary>
        /// 设置被攻击后状态
        /// </summary>
        /// <returns>是否产生实际伤害</returns>
        public bool 设置被攻击后状态(int 攻击点数)
        {
            if (圣盾特性)
            {
                圣盾特性 = false;
                return false;
            }
            else
            {
                生命值 -= 攻击点数;
                圣盾特性 = false;
            }
            //失去圣盾
            if (攻击点数 > 0)
            {
                受过伤害 = true;
                if (!string.IsNullOrEmpty(激怒效果)) 激怒状态 = true;
                if (特殊效果 == 特殊效果枚举.持续激怒 && !沉默状态) 攻击力 += 3;
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
        /// <param name="治疗点数"></param>
        /// <returns>是否产生实际治疗作用</returns>
        public bool 设置被治疗后状态(int 治疗点数)
        {
            if (生命值 == 生命值上限) return false;
            生命值 += 治疗点数;
            if (生命值 > 生命值上限) 生命值 = 生命值上限;
            //取消激怒
            if (生命值 == 生命值上限) 激怒状态 = false;
            return true;
        }
        /// <summary>
        /// 事件处理方法
        /// </summary>
        /// <param name="事件"></param>
        /// <param name="game"></param>
        /// <returns></returns>
        public List<string> 事件处理方法(CardUtility.全局事件 事件, ActionStatus game)
        {
            List<string> ActionLst = new List<string>();
            if (!沉默状态 && 自身事件效果.触发效果事件类型 == 事件.触发事件类型)
            {
                if (自身事件效果.触发效果事件方向 != CardUtility.目标选择方向枚举.双方)
                {
                    if (自身事件效果.触发效果事件方向 == CardUtility.目标选择方向枚举.本方 && (!事件.触发位置.本方对方标识)) return ActionLst;
                    if (自身事件效果.触发效果事件方向 == CardUtility.目标选择方向枚举.对方 && (事件.触发位置.本方对方标识)) return ActionLst;
                }
                if (!string.IsNullOrEmpty(自身事件效果.限制信息) && !CardUtility.符合选择条件(this, 自身事件效果.限制信息))
                {
                    return ActionLst;
                }
                ActionLst.Add(Server.ActionCode.strHitEvent + CardUtility.strSplitMark);
                //这里有可能是一个增益表达式！！
                if (自身事件效果.效果编号.StartsWith("A"))
                {
                    ActionLst.AddRange(((SpellCard)CardUtility.GetCardInfoBySN(自身事件效果.效果编号)).UseSpell(game));
                }
                else
                {
                    Effect.PointEffect t = new Effect.PointEffect();
                    string[] opt = 自身事件效果.效果编号.Split("/".ToCharArray());
                    t.持续回合 = CardUtility.Max.ToString();
                    t.攻击力 = opt[0];
                    t.生命值 = opt[1];
                    ((Effect.IAtomicEffect)t).DealMinion(game, this);
                }
                潜行特性 = false;
            }
            return ActionLst;
        }
        #endregion
    }
}
