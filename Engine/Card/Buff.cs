using Engine.Action;
using Engine.Utility;
using System;

namespace Engine.Card
{
    /// <summary>
    /// 光环
    /// </summary>
    public class Buff
    {
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
            public CardUtility.位置选择用参数结构体 范围;
            /// <summary>
            /// 类型[Type]
            /// </summary>
            public 光环类型枚举 类型;
            /// <summary>
            /// 信息[Information]
            /// </summary>
            public string 信息;
            /// <summary>
            /// 来源[Buff Source]
            /// </summary>
            public string 来源;
        }
        /// <summary>
        /// Buff的设置
        /// </summary>
        public static void ResetBuff(ActionStatus game)
        {
            //去除所有光环效果
            //Buff不但是对本方战场产生效果，也可以对对方产生效果，所以必须放到全局的高度
            for (int i = 0; i < game.AllRole.MyPublicInfo.BattleField.MinionCount; i++)
            {
                if (game.AllRole.MyPublicInfo.BattleField.BattleMinions[i] != null) game.AllRole.MyPublicInfo.BattleField.BattleMinions[i].受战场效果.Clear();
            }
            for (int i = 0; i < game.AllRole.YourPublicInfo.BattleField.MinionCount; i++)
            {
                if (game.AllRole.YourPublicInfo.BattleField.BattleMinions[i] != null) game.AllRole.YourPublicInfo.BattleField.BattleMinions[i].受战场效果.Clear();
            }
            game.AllRole.MyPublicInfo.BattleField.AbilityCost = 0;
            game.AllRole.MyPublicInfo.BattleField.AbilityDamagePlus = 0;
            game.AllRole.MyPublicInfo.BattleField.MinionCost = 0;

            game.AllRole.YourPublicInfo.BattleField.AbilityCost = 0;
            game.AllRole.YourPublicInfo.BattleField.AbilityDamagePlus = 0;
            game.AllRole.YourPublicInfo.BattleField.MinionCost = 0;

            //设置光环效果
            for (int i = 0; i < game.AllRole.MyPublicInfo.BattleField.MinionCount; i++)
            {
                var minion = game.AllRole.MyPublicInfo.BattleField.BattleMinions[i];
                if (minion != null)
                {
                    if (!string.IsNullOrEmpty(minion.光环效果.信息))
                    {
                        switch (minion.光环效果.类型)
                        {
                            case 光环类型枚举.增加攻防:
                                switch (minion.光环效果.范围.EffictTargetSelectMode)
                                {
                                    case CardUtility.目标选择模式枚举.全体:
                                        for (int j = 0; j < game.AllRole.MyPublicInfo.BattleField.BattleMinions.Length; j++)
                                        {
                                            if (game.AllRole.MyPublicInfo.BattleField.BattleMinions[j] != null)
                                                game.AllRole.MyPublicInfo.BattleField.BattleMinions[j].受战场效果.Add(minion.光环效果);
                                        }
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case 光环类型枚举.施法成本:
                                switch (minion.光环效果.范围.EffectTargetSelectDirect)
                                {
                                    case CardUtility.目标选择方向枚举.本方:
                                        game.AllRole.MyPublicInfo.BattleField.AbilityCost += int.Parse(minion.光环效果.信息);
                                        break;
                                    case CardUtility.目标选择方向枚举.对方:
                                        game.AllRole.YourPublicInfo.BattleField.AbilityCost += int.Parse(minion.光环效果.信息);
                                        break;
                                    case CardUtility.目标选择方向枚举.双方:
                                        game.AllRole.MyPublicInfo.BattleField.AbilityCost += int.Parse(minion.光环效果.信息);
                                        game.AllRole.YourPublicInfo.BattleField.AbilityCost += int.Parse(minion.光环效果.信息);
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case 光环类型枚举.法术效果:
                                switch (minion.光环效果.范围.EffectTargetSelectDirect)
                                {
                                    case CardUtility.目标选择方向枚举.本方:
                                        game.AllRole.MyPublicInfo.BattleField.AbilityDamagePlus += int.Parse(minion.光环效果.信息);
                                        break;
                                    case CardUtility.目标选择方向枚举.对方:
                                        game.AllRole.YourPublicInfo.BattleField.AbilityDamagePlus += int.Parse(minion.光环效果.信息);
                                        break;
                                    case CardUtility.目标选择方向枚举.双方:
                                        game.AllRole.MyPublicInfo.BattleField.AbilityDamagePlus += int.Parse(minion.光环效果.信息);
                                        game.AllRole.YourPublicInfo.BattleField.AbilityDamagePlus += int.Parse(minion.光环效果.信息);
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case 光环类型枚举.随从成本:
                                switch (minion.光环效果.范围.EffectTargetSelectDirect)
                                {
                                    case CardUtility.目标选择方向枚举.本方:
                                        game.AllRole.MyPublicInfo.BattleField.MinionCost += int.Parse(minion.光环效果.信息);
                                        break;
                                    case CardUtility.目标选择方向枚举.对方:
                                        game.AllRole.YourPublicInfo.BattleField.MinionCost += int.Parse(minion.光环效果.信息);
                                        break;
                                    case CardUtility.目标选择方向枚举.双方:
                                        game.AllRole.MyPublicInfo.BattleField.MinionCost += int.Parse(minion.光环效果.信息);
                                        game.AllRole.YourPublicInfo.BattleField.MinionCost += int.Parse(minion.光环效果.信息);
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
    }
}
