using Engine.Utility;
using System;
using System.Collections.Generic;

namespace Engine.Client
{
    public class FightHandler
    {
        /// <summary>
        /// 战斗
        /// </summary>
        /// <param name="攻击方Pos">本方</param>
        /// <param name="被攻击方Pos">对方</param>
        /// <param name="IsMyAction">被动攻击</param>
        /// <returns></returns>
        public static List<String> Fight(int 攻击方Pos, int 被攻击方Pos, GameStatus game, Boolean IsMyAction)
        {
            PublicInfo AttackInfo;
            PublicInfo AttackedInfo;
            if (IsMyAction)
            {
                AttackInfo = GameManager.gameStatus.client.MyInfo;
                AttackedInfo = GameManager.gameStatus.client.YourInfo;
            }
            else
            {
                AttackInfo = GameManager.gameStatus.client.YourInfo;
                AttackedInfo = GameManager.gameStatus.client.MyInfo;
            }
            List<String> Result = new List<string>();
            //主动攻击方的状态变化
            if (攻击方Pos == BattleFieldInfo.HeroPos)
            {
                //武器状态
                if (AttackInfo.Weapon != null)
                {
                    AttackInfo.Weapon.耐久度--;
                    AttackInfo.RemainAttactTimes = 0;
                }
            }
            else
            {
                //攻击次数的清算,潜行等去除(如果不是被攻击方的处理)
                AttackInfo.BattleField.BattleMinions[攻击方Pos - 1].设置攻击后状态();
            }

            //伤害计算(攻击方)
            var AttackedPoint = 0;
            if (被攻击方Pos != BattleFieldInfo.HeroPos)
            {
                AttackedPoint = AttackedInfo.BattleField.BattleMinions[被攻击方Pos - 1].实际攻击值;
            }

            if (攻击方Pos != BattleFieldInfo.HeroPos)
            {
                //圣盾不引发伤害事件
                if (AttackInfo.BattleField.BattleMinions[攻击方Pos - 1].设置被攻击后状态(AttackedPoint))
                {
                    GameManager.事件处理组件.事件池.Add(new Engine.Utility.CardUtility.全局事件()
                    {
                        触发事件类型 = CardUtility.事件类型枚举.受伤,
                        触发位置 = AttackInfo.BattleField.BattleMinions[攻击方Pos - 1].战场位置
                    });
                }
            }
            else
            {
                //护甲不引发伤害事件
                if (AttackInfo.AfterBeAttack(AttackedPoint))
                {
                    GameManager.事件处理组件.事件池.Add(new Engine.Utility.CardUtility.全局事件()
                    {
                        触发事件类型 = CardUtility.事件类型枚举.受伤,
                        触发位置 = AttackInfo.战场位置
                    });
                }
            }

            //伤害计算(被攻击方)
            var AttackPoint = 0;
            if (攻击方Pos != BattleFieldInfo.HeroPos)
            {
                AttackPoint = AttackInfo.BattleField.BattleMinions[攻击方Pos - 1].实际攻击值;
            }
            else
            {
                //其实除了武器以外，其他方法也可使英雄有攻击力！
                if (AttackInfo.Weapon != null) AttackPoint = AttackInfo.Weapon.攻击力;
            }
            if (被攻击方Pos != BattleFieldInfo.HeroPos)
            {
                if (AttackedInfo.BattleField.BattleMinions[被攻击方Pos - 1].设置被攻击后状态(AttackPoint))
                {
                    GameManager.事件处理组件.事件池.Add(new Engine.Utility.CardUtility.全局事件()
                    {
                        触发事件类型 = CardUtility.事件类型枚举.受伤,
                        触发位置 = AttackedInfo.BattleField.BattleMinions[被攻击方Pos - 1].战场位置
                    });
                }
            }
            else
            {
                //护甲不引发伤害事件
                if (AttackedInfo.AfterBeAttack(AttackPoint))
                {
                    GameManager.事件处理组件.事件池.Add(new Engine.Utility.CardUtility.全局事件()
                    {
                        触发事件类型 = CardUtility.事件类型枚举.受伤,
                        触发位置 = AttackedInfo.战场位置
                    });
                }
            }

            //每次操作后进行一次清算
            if (IsMyAction)
            {
                //将亡语效果放入结果
                Result.AddRange(GameManager.Settle());
            }
            else
            {
                //对方已经发送亡语效果，本方不用重复模拟了
                GameManager.Settle();
            }
            return Result;
        }
    }
}
