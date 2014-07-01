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
        /// 光环范围[Buff Scope]
        /// </summary>
        public enum 光环范围枚举
        {
            /// <summary>
            /// 随从全体[All Minions]
            /// </summary>
            随从全体,
            /// <summary>
            /// 相邻随从[]
            /// </summary>
            相邻随从,
            /// <summary>
            /// 全局[]
            /// </summary>
            全局
        }

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
        /// 光环类型[Buff Effect]
        /// </summary>
        public enum 光环类型枚举
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
        /// 光环结构体[Buff struct]
        /// </summary>
        [Serializable]
        public struct 光环结构体
        {
            /// <summary>
            /// 范围[Scope]
            /// </summary>
            public 光环范围枚举 范围;
            /// <summary>
            /// 类型[Type]
            /// </summary>
            public 光环类型枚举 类型;
            /// <summary>
            /// 信息[Information]
            /// </summary>
            public String 信息;
            /// <summary>
            /// 来源[Buff Source]
            /// </summary>
            public String 来源;
        }

        /// <summary>
        /// 战吼类型枚举[Battlecry Type Enum]
        /// </summary>
        public enum 战吼类型枚举
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
        public Boolean 嘲讽特性 = false;
        /// <summary>
        /// 冲锋[Charge]
        /// </summary>
        public Boolean 冲锋特性 = false;
        /// <summary>
        /// 风怒[Windfury]
        /// </summary>
        public Boolean 风怒特性 = false;
        /// <summary>
        /// 潜行[Stealth]
        /// </summary>
        public Boolean 潜行特性 = false;
        /// <summary>
        /// 圣盾[Divine Shield]
        /// </summary>
        public Boolean 圣盾特性 = false;
        /// <summary>
        /// 法术免疫 [Ability Immune]
        /// </summary>
        public Boolean 法术免疫特性 = false;
        /// <summary>
        /// 英雄技能免疫[Hero Skill Immune]
        /// </summary>
        public Boolean 英雄技能免疫特性 = false;
        /// <summary>
        /// 不能攻击[Can't Attack]
        /// </summary>
        public Boolean 无法攻击特性 = false;
        #endregion

        #region"效果"
        /// <summary>
        /// 战吼(效果号码)[Battlecry Effect]
        /// </summary>
        public String 战吼效果 = String.Empty;
        /// <summary>
        /// 战吼类型[Battlecry Type]
        /// </summary>
        public 战吼类型枚举 战吼类型 = 战吼类型枚举.默认;
        /// <summary>
        /// 亡语(效果号码)[DeathRattle Effect]
        /// </summary>
        public String 亡语效果 = String.Empty;
        /// <summary>
        /// 激怒(效果号码)[Enrage Effect]
        /// </summary>
        public String 激怒效果 = String.Empty;
        /// <summary>
        /// 回合开始(效果号码)[Trun Start Effect]
        /// </summary>
        public String 回合开始效果 = String.Empty;
        /// <summary>
        /// 回合结束(效果号码)[Trun End Effect]
        /// </summary>
        public String 回合结束效果 = String.Empty;
        /// <summary>
        /// 光环效果 [Buff Effect]
        /// </summary>
        public 光环结构体 光环效果;
        /// <summary>
        /// 特殊效果 [Specical Effect]
        /// </summary>
        public 特殊效果枚举 特殊效果 = 特殊效果枚举.无效果;
        /// <summary>
        /// 自身事件 [Event Effect]
        /// </summary>
        public Engine.Utility.CardUtility.事件效果结构体 自身事件效果 = new CardUtility.事件效果结构体();
        #endregion

        #region"运行时状态"
        /// <summary>
        /// 受过伤害[Damaged]
        /// </summary>
        [XmlIgnore]
        public Boolean 受过伤害 = false;
        /// <summary>
        /// 沉默状态[Slience Status]
        /// </summary>
        [XmlIgnore]
        public Boolean 沉默状态 = false;
        /// <summary>
        /// 激怒状态[Enrage Status]
        /// </summary>
        [XmlIgnore]
        public Boolean 激怒状态 = false;
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
        public Engine.Utility.CardUtility.指定位置结构体 战场位置;
        /// <summary>
        /// 该单位受到战地的效果[Get Buff Effect]
        /// </summary>
        [XmlIgnore]
        public List<光环结构体> 受战场效果 = new List<光环结构体>();
        /// <summary>
        /// 能否成为当前动作的对象[Can be the target of action]
        /// </summary>
        [XmlIgnore]
        public Boolean 能否成为动作对象 = false;
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
        public Engine.Utility.CardUtility.效果回合枚举 冰冻状态 = CardUtility.效果回合枚举.无效果;
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
        public Boolean 能否攻击
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
                    if (!String.IsNullOrEmpty(激怒效果)) rtnAttack += int.Parse(激怒效果);
                }
                rtnAttack += 本回合攻击力加成;
                if (特殊效果 == 特殊效果枚举.攻击必死 && !沉默状态) rtnAttack = 999;
                return rtnAttack;
            }
        }
        /// <summary>
        /// 状态[未被使用]
        /// </summary>
        public String 状态
        {
            get
            {
                StringBuilder Status = new StringBuilder();
                Status.AppendLine(名称);
                Status.AppendLine("[状]" + (圣盾特性 ? "圣" : String.Empty) +
                                           (嘲讽特性 ? "|嘲" : String.Empty) +
                                           (风怒特性 ? "|风" : String.Empty) +
                                           (冲锋特性 ? "|冲" : String.Empty) +
                                           (潜行特性 ? "|潜" : String.Empty) +
                                           (冰冻状态 != CardUtility.效果回合枚举.无效果 ? "冻" : String.Empty));
                Status.AppendLine("[实]" + 攻击力.ToString() + "/" + 生命值.ToString() +
                                  "[总]" + 实际攻击值.ToString() + "/" + 生命值.ToString());
                return Status.ToString();
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
            this.冰冻状态 = CardUtility.效果回合枚举.无效果;
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
                this.攻击状态 = 攻击状态枚举.可攻击;
            }
            else
            {
                this.攻击状态 = 攻击状态枚举.准备中;
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
        /// 发动战吼(默认)
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public List<String> 发动战吼(GameStatus game, Boolean IsMyAction)
        {
            List<String> ActionCodeLst = new List<string>();
            //战吼效果
            if (战吼效果 != String.Empty && !沉默状态)
            {
                var 战吼Result = RunAction.StartAction(game, 战吼效果, IsMyAction);
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
        /// <param name="IsMyAction"></param>
        /// <returns></returns>
        public List<String> 发动亡语(GameStatus game, Boolean IsMyAction)
        {
            List<String> ActionCodeLst = new List<string>();
            //亡语效果
            if (亡语效果 != String.Empty && !沉默状态)
            {
                var 亡语Result = RunAction.StartAction(game, 亡语效果, IsMyAction);
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
        public List<String> 回合开始(GameStatus game)
        {
            List<String> ActionCodeLst = new List<string>();
            //回合开始效果
            if (回合开始效果 != String.Empty && !沉默状态)
            {
                var 回合开始Result = RunAction.StartAction(game, 回合开始效果, true);
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
        public List<String> 回合结束(GameStatus game)
        {
            List<String> ActionCodeLst = new List<string>();
            //回合结束效果
            if (回合结束效果 != String.Empty && !沉默状态)
            {
                var 回合结束Result = RunAction.StartAction(game, 回合结束效果, true);
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
            if (剩余攻击次数 == 0) 攻击状态 = MinionCard.攻击状态枚举.攻击完毕;
        }
        /// <summary>
        /// 设置被攻击后状态
        /// </summary>
        /// <returns>是否产生实际伤害</returns>
        public Boolean 设置被攻击后状态(int 攻击点数)
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
                if (!String.IsNullOrEmpty(激怒效果)) 激怒状态 = true;
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
        public Boolean 设置被治疗后状态(int 治疗点数)
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
        public List<String> 事件处理方法(Engine.Utility.CardUtility.全局事件 事件, GameStatus game)
        {
            List<String> ActionLst = new List<string>();
            if (!沉默状态 && 自身事件效果.触发效果事件类型 == 事件.触发事件类型)
            {
                if (自身事件效果.触发效果事件方向 != CardUtility.目标选择方向枚举.双方)
                {
                    if (自身事件效果.触发效果事件方向 == CardUtility.目标选择方向枚举.本方 && (!事件.触发位置.本方对方标识)) return ActionLst;
                    if (自身事件效果.触发效果事件方向 == CardUtility.目标选择方向枚举.对方 && (事件.触发位置.本方对方标识)) return ActionLst;
                }
                if (!String.IsNullOrEmpty(自身事件效果.限制信息) && !Engine.Utility.CardUtility.符合选择条件(this, 自身事件效果.限制信息))
                {
                    return ActionLst;
                }
                ActionLst.Add(Engine.Server.ActionCode.strHitEvent + CardUtility.strSplitMark);
                ActionLst.AddRange(((Card.SpellCard)CardUtility.GetCardInfoBySN(自身事件效果.效果编号)).UseAbility(game, false));
                潜行特性 = false;
            }
            return ActionLst;
        }
        #endregion
    }
}
