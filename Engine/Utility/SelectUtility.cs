using Engine.Action;
using Engine.Client;
using System;
using System.Collections.Generic;

namespace Engine.Utility
{
    /// <summary>
    /// 对象选择
    /// </summary>
    public static class SelectUtility
    {
        /// <summary>
        /// 获得对象选择列表
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public static String GetTargetList(ActionStatus game)
        {
            List<String> Target = new List<string>();
            if (game.AllRole.MyPublicInfo.能否成为动作对象) Target.Add(CardUtility.strMe + CardUtility.strSplitMark + BattleFieldInfo.HeroPos);
            for (int i = 0; i < game.AllRole.MyPublicInfo.BattleField.MinionCount; i++)
            {
                if (game.AllRole.MyPublicInfo.BattleField.BattleMinions[i].能否成为动作对象) Target.Add(CardUtility.strMe + CardUtility.strSplitMark + (i + 1).ToString("D1"));
            }
            if (game.AllRole.YourPublicInfo.能否成为动作对象) Target.Add(CardUtility.strYou + CardUtility.strSplitMark + BattleFieldInfo.HeroPos);
            for (int i = 0; i < game.AllRole.YourPublicInfo.BattleField.MinionCount; i++)
            {
                if (game.AllRole.YourPublicInfo.BattleField.BattleMinions[i].能否成为动作对象) Target.Add(CardUtility.strYou + CardUtility.strSplitMark + (i + 1).ToString("D1"));
            }
            return Target.ToListString();
        }
        /// <summary>
        /// 设置是否能成为当前动作的对象
        /// </summary>
        /// <param name="SelectOption"></param>
        /// <param name="game"></param>
        public static void SetTargetSelectEnable(CardUtility.位置选择用参数结构体 SelectOption, ActionStatus game)
        {
            game.AllRole.MyPublicInfo.能否成为动作对象 = false;
            for (int i = 0; i < game.AllRole.MyPublicInfo.BattleField.MinionCount; i++)
            {
                game.AllRole.MyPublicInfo.BattleField.BattleMinions[i].能否成为动作对象 = false;
            }
            game.AllRole.YourPublicInfo.能否成为动作对象 = false;
            for (int i = 0; i < game.AllRole.YourPublicInfo.BattleField.MinionCount; i++)
            {
                game.AllRole.YourPublicInfo.BattleField.BattleMinions[i].能否成为动作对象 = false;
            }
            switch (SelectOption.EffectTargetSelectDirect)
            {
                case CardUtility.目标选择方向枚举.本方:
                    switch (SelectOption.EffectTargetSelectRole)
                    {
                        case CardUtility.目标选择角色枚举.随从:
                        case CardUtility.目标选择角色枚举.所有角色:
                            for (int i = 0; i < game.AllRole.MyPublicInfo.BattleField.MinionCount; i++)
                            {
                                if (Engine.Utility.CardUtility.符合选择条件(game.AllRole.MyPublicInfo.BattleField.BattleMinions[i], SelectOption.EffectTargetSelectCondition))
                                {
                                    game.AllRole.MyPublicInfo.BattleField.BattleMinions[i].能否成为动作对象 = !game.AllRole.MyPublicInfo.BattleField.BattleMinions[i].潜行特性;
                                }
                            }
                            if (SelectOption.EffectTargetSelectRole == CardUtility.目标选择角色枚举.所有角色) game.AllRole.MyPublicInfo.能否成为动作对象 = true;
                            break;
                        case CardUtility.目标选择角色枚举.英雄:
                            game.AllRole.MyPublicInfo.能否成为动作对象 = true;
                            break;
                    }
                    break;
                case CardUtility.目标选择方向枚举.对方:
                    Boolean Has嘲讽 = false;
                    for (int i = 0; i < game.AllRole.YourPublicInfo.BattleField.MinionCount; i++)
                    {
                        if (game.AllRole.YourPublicInfo.BattleField.BattleMinions[i].嘲讽特性 &&
                            (!game.AllRole.YourPublicInfo.BattleField.BattleMinions[i].潜行特性))
                        {
                            //嘲讽特性的时候，如果潜行特性，则潜行特性无效
                            Has嘲讽 = true;
                            break;
                        }
                    }
                    switch (SelectOption.EffectTargetSelectRole)
                    {
                        case CardUtility.目标选择角色枚举.英雄:
                            game.AllRole.YourPublicInfo.能否成为动作对象 = true;
                            break;
                        case CardUtility.目标选择角色枚举.随从:
                        case CardUtility.目标选择角色枚举.所有角色:
                            if (SelectOption.嘲讽限制 && Has嘲讽)
                            {
                                game.AllRole.YourPublicInfo.能否成为动作对象 = false;
                                for (int i = 0; i < game.AllRole.YourPublicInfo.BattleField.MinionCount; i++)
                                {
                                    //只能选择嘲讽对象
                                    if (game.AllRole.YourPublicInfo.BattleField.BattleMinions[i].嘲讽特性)
                                        game.AllRole.YourPublicInfo.BattleField.BattleMinions[i].能否成为动作对象 = !game.AllRole.YourPublicInfo.BattleField.BattleMinions[i].潜行特性;
                                }
                            }
                            else
                            {
                                game.AllRole.YourPublicInfo.能否成为动作对象 = true;
                                for (int i = 0; i < game.AllRole.YourPublicInfo.BattleField.MinionCount; i++)
                                {
                                    if (Engine.Utility.CardUtility.符合选择条件(game.AllRole.YourPublicInfo.BattleField.BattleMinions[i], SelectOption.EffectTargetSelectCondition))
                                        game.AllRole.YourPublicInfo.BattleField.BattleMinions[i].能否成为动作对象 = !game.AllRole.YourPublicInfo.BattleField.BattleMinions[i].潜行特性;
                                }
                            }
                            if (SelectOption.EffectTargetSelectRole == CardUtility.目标选择角色枚举.所有角色) game.AllRole.YourPublicInfo.能否成为动作对象 = true;
                            break;
                    }
                    break;
                case CardUtility.目标选择方向枚举.双方:
                    switch (SelectOption.EffectTargetSelectRole)
                    {
                        case CardUtility.目标选择角色枚举.英雄:
                            game.AllRole.MyPublicInfo.能否成为动作对象 = true;
                            game.AllRole.YourPublicInfo.能否成为动作对象 = true;
                            break;
                        case CardUtility.目标选择角色枚举.所有角色:
                        case CardUtility.目标选择角色枚举.随从:
                            if (SelectOption.EffectTargetSelectRole == CardUtility.目标选择角色枚举.所有角色)
                            {
                                game.AllRole.MyPublicInfo.能否成为动作对象 = true;
                                game.AllRole.YourPublicInfo.能否成为动作对象 = true;
                            }
                            for (int i = 0; i < game.AllRole.MyPublicInfo.BattleField.MinionCount; i++)
                            {
                                if (Engine.Utility.CardUtility.符合选择条件(game.AllRole.MyPublicInfo.BattleField.BattleMinions[i], SelectOption.EffectTargetSelectCondition))
                                    game.AllRole.MyPublicInfo.BattleField.BattleMinions[i].能否成为动作对象 = !game.AllRole.MyPublicInfo.BattleField.BattleMinions[i].潜行特性;
                            }
                            for (int i = 0; i < game.AllRole.YourPublicInfo.BattleField.MinionCount; i++)
                            {
                                if (Engine.Utility.CardUtility.符合选择条件(game.AllRole.YourPublicInfo.BattleField.BattleMinions[i], SelectOption.EffectTargetSelectCondition))
                                    game.AllRole.YourPublicInfo.BattleField.BattleMinions[i].能否成为动作对象 = !game.AllRole.YourPublicInfo.BattleField.BattleMinions[i].潜行特性;
                            }
                            break;
                    }
                    break;
            }
        }
    }
}
