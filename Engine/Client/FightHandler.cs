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
        /// <param name="被动攻击">被动攻击</param>
        /// <returns></returns>
        public static List<String> Fight(int 攻击方Pos, int 被攻击方Pos, GameManager game,Boolean 被动攻击 = false)
        {
            List<String> Result = new List<string>();
            //主动攻击方的状态变化
            if (!被动攻击)
            {
                //攻击次数
                if (攻击方Pos == BattleFieldInfo.HeroPos)
                {
                    if (game.MyInfo.Weapon != null)
                    {
                        game.MyInfo.Weapon.耐久度--;
                        game.MyInfo.RemainAttactTimes = 0;
                    }
                }
                else
                {
                    //攻击次数的清算,潜行等去除(如果不是被攻击方的处理)
                    game.MyInfo.BattleField.BattleMinions[攻击方Pos - 1].AfterDoAttack(被动攻击);
                }
            }
            //伤害计算(本方)
            //被攻击方如果是英雄，则认为这个时候是攻击方的回合，
            //则被攻击方的英雄是关闭武器状态的，本方不会受到伤害
            var YourAttackPoint = 0;
            if (被攻击方Pos != BattleFieldInfo.HeroPos)
            {
                YourAttackPoint = game.YourInfo.BattleField.BattleMinions[被攻击方Pos - 1].TotalAttack();
            }
            if (攻击方Pos != BattleFieldInfo.HeroPos)
            {
                //圣盾不引发伤害事件
                if (game.MyInfo.BattleField.BattleMinions[攻击方Pos - 1].AfterBeAttack(YourAttackPoint))
                {
                    game.事件处理组件.事件池.Add(new Engine.Utility.CardUtility.全局事件()
                    {
                        事件类型 = CardUtility.事件类型列表.受伤,
                        触发方向 = CardUtility.TargetSelectDirectEnum.本方,
                        触发位置 = 攻击方Pos
                    });
                }
            }
            else
            {
                //护甲不引发伤害事件
                if (game.MyInfo.AfterBeAttack(YourAttackPoint))
                {
                    game.事件处理组件.事件池.Add(new Engine.Utility.CardUtility.全局事件()
                    {
                        事件类型 = CardUtility.事件类型列表.受伤,
                        触发方向 = CardUtility.TargetSelectDirectEnum.本方,
                        触发位置 = BattleFieldInfo.HeroPos
                    });
                }
            }
            //伤害计算(对方)
            var MyAttackPoint = 0;
            if (攻击方Pos != BattleFieldInfo.HeroPos)
            {
                MyAttackPoint = game.MyInfo.BattleField.BattleMinions[攻击方Pos - 1].TotalAttack();
            }
            else
            {
                if (game.MyInfo.Weapon != null) MyAttackPoint = game.MyInfo.Weapon.攻击力;
            }
            if (被攻击方Pos != BattleFieldInfo.HeroPos)
            {
                if (game.YourInfo.BattleField.BattleMinions[被攻击方Pos - 1].AfterBeAttack(MyAttackPoint))
                {
                    game.事件处理组件.事件池.Add(new Engine.Utility.CardUtility.全局事件()
                    {
                        事件类型 = CardUtility.事件类型列表.受伤,
                        触发方向 = CardUtility.TargetSelectDirectEnum.对方,
                        触发位置 = 被攻击方Pos
                    });
                }
            }
            else
            {
                //护甲不引发伤害事件
                if (game.YourInfo.AfterBeAttack(MyAttackPoint))
                {
                    game.事件处理组件.事件池.Add(new Engine.Utility.CardUtility.全局事件()
                    {
                        事件类型 = CardUtility.事件类型列表.受伤,
                        触发方向 = CardUtility.TargetSelectDirectEnum.对方,
                        触发位置 = BattleFieldInfo.HeroPos
                    });
                }
            }

            //每次操作后进行一次清算
            if (!被动攻击)
            {
                //将亡语效果放入结果
                Result.AddRange(game.Settle());
            }
            else
            {
                //对方已经发送亡语效果，本方不用重复模拟了
                game.Settle();
            }
            return Result;
        }
    }
}
