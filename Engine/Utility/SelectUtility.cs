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
        public static string GetTargetListString(ActionStatus game)
        {
            List<string> Target = new List<string>();
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
        /// 施法对象列表
        /// </summary>
        /// <param name="singleEffect"></param>
        /// <param name="game"></param>
        /// <param name="PosInfo"></param>
        /// <param name="RandSeed"></param>
        /// <returns></returns>
        public static List<string> GetTargetList(CardUtility.位置选择用参数结构体 SelectOpt, ActionStatus game, int RandSeed)
        {
            List<string> Result = new List<string>();
            switch (SelectOpt.EffictTargetSelectMode)
            {
                case CardUtility.目标选择模式枚举.随机:
                    Random t = new Random(DateTime.Now.Millisecond + RandSeed);
                    switch (SelectOpt.EffectTargetSelectDirect)
                    {
                        case CardUtility.目标选择方向枚举.本方:
                            switch (SelectOpt.EffectTargetSelectRole)
                            {
                                case CardUtility.目标选择角色枚举.随从:
                                    SelectOpt.SelectedPos.位置 = t.Next(1, game.AllRole.MyPublicInfo.BattleField.MinionCount + 1);
                                    SelectOpt.SelectedPos.本方对方标识 = true;
                                    break;
                                case CardUtility.目标选择角色枚举.所有角色:
                                    SelectOpt.SelectedPos.位置 = t.Next(BattleFieldInfo.HeroPos, game.AllRole.MyPublicInfo.BattleField.MinionCount + 1);
                                    SelectOpt.SelectedPos.本方对方标识 = true;
                                    break;
                            }
                            //ME#POS
                            Result.Add(CardUtility.strMe + CardUtility.strSplitMark + SelectOpt.SelectedPos.位置.ToString("D1"));
                            break;
                        case CardUtility.目标选择方向枚举.对方:
                            switch (SelectOpt.EffectTargetSelectRole)
                            {
                                case CardUtility.目标选择角色枚举.随从:
                                    SelectOpt.SelectedPos.位置 = t.Next(1, game.AllRole.YourPublicInfo.BattleField.MinionCount + 1);
                                    SelectOpt.SelectedPos.本方对方标识 = false;
                                    break;
                                case CardUtility.目标选择角色枚举.所有角色:
                                    SelectOpt.SelectedPos.位置 = t.Next(BattleFieldInfo.HeroPos, game.AllRole.YourPublicInfo.BattleField.MinionCount + 1);
                                    SelectOpt.SelectedPos.本方对方标识 = false;
                                    break;
                            }
                            //ME#POS
                            Result.Add(CardUtility.strYou + CardUtility.strSplitMark + SelectOpt.SelectedPos.位置.ToString("D1"));
                            break;
                        case CardUtility.目标选择方向枚举.双方:
                            //本方对方
                            int MinionCount;
                            if (t.Next(1, 3) == 1)
                            {
                                SelectOpt.SelectedPos.本方对方标识 = true;
                                MinionCount = game.AllRole.MyPublicInfo.BattleField.MinionCount;
                            }
                            else
                            {
                                SelectOpt.SelectedPos.本方对方标识 = false;
                                MinionCount = game.AllRole.YourPublicInfo.BattleField.MinionCount;
                            }
                            switch (SelectOpt.EffectTargetSelectRole)
                            {
                                case CardUtility.目标选择角色枚举.随从:
                                    SelectOpt.SelectedPos.位置 = t.Next(1, MinionCount + 1);
                                    break;
                                case CardUtility.目标选择角色枚举.所有角色:
                                    SelectOpt.SelectedPos.位置 = t.Next(BattleFieldInfo.HeroPos, MinionCount + 1);
                                    break;
                            }
                            //ME#POS
                            Result.Add((SelectOpt.SelectedPos.本方对方标识 ? CardUtility.strMe : CardUtility.strYou) + CardUtility.strSplitMark + SelectOpt.SelectedPos.位置.ToString("D1"));
                            break;
                        default:
                            break;
                    }
                    break;
                case CardUtility.目标选择模式枚举.全体:
                case CardUtility.目标选择模式枚举.横扫:
                    switch (SelectOpt.EffectTargetSelectDirect)
                    {
                        case CardUtility.目标选择方向枚举.本方:
                            switch (SelectOpt.EffectTargetSelectRole)
                            {
                                case CardUtility.目标选择角色枚举.随从:
                                    Result.Add(CardUtility.strMe + CardUtility.strSplitMark + BattleFieldInfo.AllMinionPos.ToString("D1"));
                                    break;
                                case CardUtility.目标选择角色枚举.英雄:
                                    Result.Add(CardUtility.strMe + CardUtility.strSplitMark + BattleFieldInfo.HeroPos.ToString("D1"));
                                    break;
                                case CardUtility.目标选择角色枚举.所有角色:
                                    Result.Add(CardUtility.strMe + CardUtility.strSplitMark + BattleFieldInfo.AllRolePos.ToString("D1"));
                                    break;
                            }
                            break;
                        case CardUtility.目标选择方向枚举.对方:
                            switch (SelectOpt.EffectTargetSelectRole)
                            {
                                case CardUtility.目标选择角色枚举.随从:
                                    Result.Add(CardUtility.strYou + CardUtility.strSplitMark + BattleFieldInfo.AllMinionPos.ToString("D1"));
                                    break;
                                case CardUtility.目标选择角色枚举.英雄:
                                    Result.Add(CardUtility.strYou + CardUtility.strSplitMark + BattleFieldInfo.HeroPos.ToString("D1"));
                                    break;
                                case CardUtility.目标选择角色枚举.所有角色:
                                    Result.Add(CardUtility.strYou + CardUtility.strSplitMark + BattleFieldInfo.AllRolePos.ToString("D1"));
                                    break;
                            }
                            break;
                        case CardUtility.目标选择方向枚举.双方:
                            switch (SelectOpt.EffectTargetSelectRole)
                            {
                                case CardUtility.目标选择角色枚举.随从:
                                    Result.Add(CardUtility.strMe + CardUtility.strSplitMark + BattleFieldInfo.AllMinionPos.ToString("D1"));
                                    Result.Add(CardUtility.strYou + CardUtility.strSplitMark + BattleFieldInfo.AllMinionPos.ToString("D1"));
                                    break;
                                case CardUtility.目标选择角色枚举.英雄:
                                    Result.Add(CardUtility.strMe + CardUtility.strSplitMark + BattleFieldInfo.HeroPos.ToString("D1"));
                                    Result.Add(CardUtility.strYou + CardUtility.strSplitMark + BattleFieldInfo.HeroPos.ToString("D1"));
                                    break;
                                case CardUtility.目标选择角色枚举.所有角色:
                                    Result.Add(CardUtility.strMe + CardUtility.strSplitMark + BattleFieldInfo.AllRolePos.ToString("D1"));
                                    Result.Add(CardUtility.strYou + CardUtility.strSplitMark + BattleFieldInfo.AllRolePos.ToString("D1"));
                                    break;
                            }
                            break;
                    }
                    break;
                case CardUtility.目标选择模式枚举.指定:
                    Result.Add((SelectOpt.SelectedPos.本方对方标识 ? CardUtility.strMe : CardUtility.strYou) + CardUtility.strSplitMark + SelectOpt.SelectedPos.位置.ToString("D1"));
                    break;
                case CardUtility.目标选择模式枚举.相邻:
                case CardUtility.目标选择模式枚举.相邻排除指定位置:
                    if (SelectOpt.EffictTargetSelectMode == CardUtility.目标选择模式枚举.相邻)
                    {
                        Result.Add((SelectOpt.SelectedPos.本方对方标识 ? CardUtility.strMe : CardUtility.strYou) + CardUtility.strSplitMark + SelectOpt.SelectedPos.位置.ToString("D1"));
                    }
                    //左侧追加
                    if (SelectOpt.SelectedPos.位置 != 1)
                        Result.Add((SelectOpt.SelectedPos.本方对方标识 ? CardUtility.strMe : CardUtility.strYou) + CardUtility.strSplitMark + (SelectOpt.SelectedPos.位置 - 1).ToString("D1"));
                    //右侧追加
                    if (SelectOpt.SelectedPos.本方对方标识)
                    {
                        if (SelectOpt.SelectedPos.位置 != game.AllRole.MyPublicInfo.BattleField.MinionCount)
                            Result.Add((SelectOpt.SelectedPos.本方对方标识 ? CardUtility.strMe : CardUtility.strYou) + CardUtility.strSplitMark + (SelectOpt.SelectedPos.位置 + 1).ToString("D1"));
                    }
                    else
                    {
                        if (SelectOpt.SelectedPos.位置 != game.AllRole.YourPublicInfo.BattleField.MinionCount)
                            Result.Add((SelectOpt.SelectedPos.本方对方标识 ? CardUtility.strMe : CardUtility.strYou) + CardUtility.strSplitMark + (SelectOpt.SelectedPos.位置 + 1).ToString("D1"));
                    }
                    break;
                case CardUtility.目标选择模式枚举.不用选择:
                    if (SelectOpt.EffectTargetSelectRole == CardUtility.目标选择角色枚举.英雄)
                    {
                        switch (SelectOpt.EffectTargetSelectDirect)
                        {
                            case CardUtility.目标选择方向枚举.本方:
                                Result.Add(CardUtility.strMe + CardUtility.strSplitMark + BattleFieldInfo.HeroPos.ToString("D1"));
                                break;
                            case CardUtility.目标选择方向枚举.对方:
                                Result.Add(CardUtility.strYou + CardUtility.strSplitMark + BattleFieldInfo.HeroPos.ToString("D1"));
                                break;
                            case CardUtility.目标选择方向枚举.双方:
                                Result.Add(CardUtility.strMe + CardUtility.strSplitMark + BattleFieldInfo.HeroPos.ToString("D1"));
                                Result.Add(CardUtility.strYou + CardUtility.strSplitMark + BattleFieldInfo.HeroPos.ToString("D1"));
                                break;
                            default:
                                break;
                        }
                    }
                    break;
            }
            return Result;
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
                                if (CardUtility.符合选择条件(game.AllRole.MyPublicInfo.BattleField.BattleMinions[i], SelectOption.EffectTargetSelectCondition))
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
                    bool Has嘲讽 = false;
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
                                    if (CardUtility.符合选择条件(game.AllRole.YourPublicInfo.BattleField.BattleMinions[i], SelectOption.EffectTargetSelectCondition))
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
                                if (CardUtility.符合选择条件(game.AllRole.MyPublicInfo.BattleField.BattleMinions[i], SelectOption.EffectTargetSelectCondition))
                                    game.AllRole.MyPublicInfo.BattleField.BattleMinions[i].能否成为动作对象 = !game.AllRole.MyPublicInfo.BattleField.BattleMinions[i].潜行特性;
                            }
                            for (int i = 0; i < game.AllRole.YourPublicInfo.BattleField.MinionCount; i++)
                            {
                                if (CardUtility.符合选择条件(game.AllRole.YourPublicInfo.BattleField.BattleMinions[i], SelectOption.EffectTargetSelectCondition))
                                    game.AllRole.YourPublicInfo.BattleField.BattleMinions[i].能否成为动作对象 = !game.AllRole.YourPublicInfo.BattleField.BattleMinions[i].潜行特性;
                            }
                            break;
                    }
                    break;
            }
            //战吼等时候，不能选择自己
            if (SelectOption.CanNotSelectPos.位置 != BattleFieldInfo.UnknowPos)
            {
                if (SelectOption.CanNotSelectPos.本方对方标识)
                {
                    if (SelectOption.CanNotSelectPos.位置 == BattleFieldInfo.HeroPos)
                    {
                        game.AllRole.MyPublicInfo.能否成为动作对象 = false;
                    }
                    else
                    {
                        game.AllRole.MyPublicInfo.BattleField.BattleMinions[SelectOption.CanNotSelectPos.位置 - 1].能否成为动作对象 = false;
                    }
                }
                else
                {
                    if (SelectOption.CanNotSelectPos.位置 == BattleFieldInfo.HeroPos)
                    {
                        game.AllRole.YourPublicInfo.能否成为动作对象 = false;
                    }
                    else
                    {
                        game.AllRole.YourPublicInfo.BattleField.BattleMinions[SelectOption.CanNotSelectPos.位置 - 1].能否成为动作对象 = false;
                    }
                }
            }
        }

    }
}
