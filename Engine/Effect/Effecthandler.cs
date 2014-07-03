using Engine.Action;
using Engine.Client;
using Engine.Control;
using Engine.Utility;
using System;
using System.Collections.Generic;

namespace Engine.Effect
{
    public class Effecthandler
    {
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
                                    SelectOpt.SelectedPos.Postion = t.Next(1,game.AllRole.MyPublicInfo.BattleField.MinionCount + 1);
                                    SelectOpt.SelectedPos.本方对方标识 = true;
                                    break;
                                case CardUtility.目标选择角色枚举.所有角色:
                                    SelectOpt.SelectedPos.Postion = t.Next(Client.BattleFieldInfo.HeroPos,game.AllRole.MyPublicInfo.BattleField.MinionCount + 1);
                                    SelectOpt.SelectedPos.本方对方标识 = true;
                                    break;
                            }
                            //ME#POS
                            Result.Add(CardUtility.strMe + CardUtility.strSplitMark + SelectOpt.SelectedPos.Postion.ToString("D1"));
                            break;
                        case CardUtility.目标选择方向枚举.对方:
                            switch (SelectOpt.EffectTargetSelectRole)
                            {
                                case CardUtility.目标选择角色枚举.随从:
                                    SelectOpt.SelectedPos.Postion = t.Next(1,game.AllRole.YourPublicInfo.BattleField.MinionCount + 1);
                                    SelectOpt.SelectedPos.本方对方标识 = false;
                                    break;
                                case CardUtility.目标选择角色枚举.所有角色:
                                    SelectOpt.SelectedPos.Postion = t.Next(Client.BattleFieldInfo.HeroPos,game.AllRole.YourPublicInfo.BattleField.MinionCount + 1);
                                    SelectOpt.SelectedPos.本方对方标识 = false;
                                    break;
                            }
                            //ME#POS
                            Result.Add(CardUtility.strYou + CardUtility.strSplitMark + SelectOpt.SelectedPos.Postion.ToString("D1"));
                            break;
                        case CardUtility.目标选择方向枚举.双方:
                            //本方对方
                            int MinionCount;
                            if (t.Next(1, 3) == 1)
                            {
                                SelectOpt.SelectedPos.本方对方标识 = true;
                                MinionCount =game.AllRole.MyPublicInfo.BattleField.MinionCount;
                            }
                            else
                            {
                                SelectOpt.SelectedPos.本方对方标识 = false;
                                MinionCount =game.AllRole.YourPublicInfo.BattleField.MinionCount;
                            }
                            switch (SelectOpt.EffectTargetSelectRole)
                            {
                                case CardUtility.目标选择角色枚举.随从:
                                    SelectOpt.SelectedPos.Postion = t.Next(1, MinionCount + 1);
                                    break;
                                case CardUtility.目标选择角色枚举.所有角色:
                                    SelectOpt.SelectedPos.Postion = t.Next(Client.BattleFieldInfo.HeroPos, MinionCount + 1);
                                    break;
                            }
                            //ME#POS
                            Result.Add((SelectOpt.SelectedPos.本方对方标识 ? CardUtility.strMe : CardUtility.strYou) + CardUtility.strSplitMark + SelectOpt.SelectedPos.Postion.ToString("D1"));
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
                                    Result.Add(CardUtility.strMe + CardUtility.strSplitMark + Client.BattleFieldInfo.AllMinionPos.ToString("D1"));
                                    break;
                                case CardUtility.目标选择角色枚举.英雄:
                                    Result.Add(CardUtility.strMe + CardUtility.strSplitMark + Client.BattleFieldInfo.HeroPos.ToString("D1"));
                                    break;
                                case CardUtility.目标选择角色枚举.所有角色:
                                    Result.Add(CardUtility.strMe + CardUtility.strSplitMark + Client.BattleFieldInfo.AllRolePos.ToString("D1"));
                                    break;
                            }
                            break;
                        case CardUtility.目标选择方向枚举.对方:
                            switch (SelectOpt.EffectTargetSelectRole)
                            {
                                case CardUtility.目标选择角色枚举.随从:
                                    Result.Add(CardUtility.strYou + CardUtility.strSplitMark + Client.BattleFieldInfo.AllMinionPos.ToString("D1"));
                                    break;
                                case CardUtility.目标选择角色枚举.英雄:
                                    Result.Add(CardUtility.strYou + CardUtility.strSplitMark + Client.BattleFieldInfo.HeroPos.ToString("D1"));
                                    break;
                                case CardUtility.目标选择角色枚举.所有角色:
                                    Result.Add(CardUtility.strYou + CardUtility.strSplitMark + Client.BattleFieldInfo.AllRolePos.ToString("D1"));
                                    break;
                            }
                            break;
                        case CardUtility.目标选择方向枚举.双方:
                            switch (SelectOpt.EffectTargetSelectRole)
                            {
                                case CardUtility.目标选择角色枚举.随从:
                                    Result.Add(CardUtility.strMe + CardUtility.strSplitMark + Client.BattleFieldInfo.AllMinionPos.ToString("D1"));
                                    Result.Add(CardUtility.strYou + CardUtility.strSplitMark + Client.BattleFieldInfo.AllMinionPos.ToString("D1"));
                                    break;
                                case CardUtility.目标选择角色枚举.英雄:
                                    Result.Add(CardUtility.strMe + CardUtility.strSplitMark + Client.BattleFieldInfo.HeroPos.ToString("D1"));
                                    Result.Add(CardUtility.strYou + CardUtility.strSplitMark + Client.BattleFieldInfo.HeroPos.ToString("D1"));
                                    break;
                                case CardUtility.目标选择角色枚举.所有角色:
                                    Result.Add(CardUtility.strMe + CardUtility.strSplitMark + Client.BattleFieldInfo.AllRolePos.ToString("D1"));
                                    Result.Add(CardUtility.strYou + CardUtility.strSplitMark + Client.BattleFieldInfo.AllRolePos.ToString("D1"));
                                    break;
                            }
                            break;
                    }
                    break;
                case CardUtility.目标选择模式枚举.指定:
                case CardUtility.目标选择模式枚举.继承:
                    Result.Add((SelectOpt.SelectedPos.本方对方标识 ? CardUtility.strMe : CardUtility.strYou) + CardUtility.strSplitMark + SelectOpt.SelectedPos.Postion.ToString("D1"));
                    break;
                case CardUtility.目标选择模式枚举.相邻:
                    Result.Add((SelectOpt.SelectedPos.本方对方标识 ? CardUtility.strMe : CardUtility.strYou) + CardUtility.strSplitMark + SelectOpt.SelectedPos.Postion.ToString("D1"));
                    //左侧追加
                    if (SelectOpt.SelectedPos.Postion != 1)
                        Result.Add((SelectOpt.SelectedPos.本方对方标识 ? CardUtility.strMe : CardUtility.strYou) + CardUtility.strSplitMark + (SelectOpt.SelectedPos.Postion - 1).ToString("D1"));
                    //右侧追加
                    if (SelectOpt.SelectedPos.本方对方标识)
                    {
                        if (SelectOpt.SelectedPos.Postion !=game.AllRole.MyPublicInfo.BattleField.MinionCount)
                            Result.Add((SelectOpt.SelectedPos.本方对方标识 ? CardUtility.strMe : CardUtility.strYou) + CardUtility.strSplitMark + (SelectOpt.SelectedPos.Postion + 1).ToString("D1"));
                    }
                    else
                    {
                        if (SelectOpt.SelectedPos.Postion !=game.AllRole.YourPublicInfo.BattleField.MinionCount)
                            Result.Add((SelectOpt.SelectedPos.本方对方标识 ? CardUtility.strMe : CardUtility.strYou) + CardUtility.strSplitMark + (SelectOpt.SelectedPos.Postion + 1).ToString("D1"));
                    }
                    break;
                case CardUtility.目标选择模式枚举.不用选择:
                    if (SelectOpt.EffectTargetSelectRole == CardUtility.目标选择角色枚举.英雄)
                    {
                        switch (SelectOpt.EffectTargetSelectDirect)
                        {
                            case CardUtility.目标选择方向枚举.本方:
                                Result.Add(CardUtility.strMe + CardUtility.strSplitMark + Client.BattleFieldInfo.HeroPos.ToString("D1"));
                                break;
                            case CardUtility.目标选择方向枚举.对方:
                                Result.Add(CardUtility.strYou + CardUtility.strSplitMark + Client.BattleFieldInfo.HeroPos.ToString("D1"));
                                break;
                            case CardUtility.目标选择方向枚举.双方:
                                Result.Add(CardUtility.strMe + CardUtility.strSplitMark + Client.BattleFieldInfo.HeroPos.ToString("D1"));
                                Result.Add(CardUtility.strYou + CardUtility.strSplitMark + Client.BattleFieldInfo.HeroPos.ToString("D1"));
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
        /// 实施效果
        /// </summary>
        /// <param name="singleEffect"></param>
        /// <param name="game"></param>
        /// <param name="RandomSeed"></param>
        /// <returns></returns>
        public static List<String> RunSingleEffect(EffectDefine singleEffect, ActionStatus game, int RandomSeed)
        {
            List<String> Result = new List<string>();
            List<String> PosList = GetTargetList(singleEffect.AbliltyPosPicker, game, RandomSeed);
            foreach (String PosInfo in PosList)
            {
                var PosField = PosInfo.Split(CardUtility.strSplitMark.ToCharArray());
                var strResult = String.Empty;
                if (PosField[0] == CardUtility.strMe)
                {
                    switch (int.Parse(PosField[1]))
                    {
                        case BattleFieldInfo.HeroPos:
                            Result.Add(GetEffectHandler(singleEffect, game, CardUtility.strMe + CardUtility.strSplitMark + Client.BattleFieldInfo.HeroPos.ToString("D1")).DealHero(game, game.AllRole.MyPublicInfo));
                            break;
                        case BattleFieldInfo.AllMinionPos:
                            for (int i = 0; i <game.AllRole.MyPublicInfo.BattleField.MinionCount; i++)
                            {
                                Result.Add(GetEffectHandler(singleEffect, game, CardUtility.strMe + CardUtility.strSplitMark + (i + 1).ToString("D1")).DealMinion(game, game.AllRole.MyPublicInfo.BattleField.BattleMinions[i]));
                            }
                            break;
                        case BattleFieldInfo.AllRolePos:
                            Result.Add(GetEffectHandler(singleEffect, game, CardUtility.strMe + CardUtility.strSplitMark + Client.BattleFieldInfo.HeroPos.ToString("D1")).DealHero(game, game.AllRole.MyPublicInfo));
                            for (int i = 0; i <game.AllRole.MyPublicInfo.BattleField.MinionCount; i++)
                            {
                                Result.Add(GetEffectHandler(singleEffect, game, CardUtility.strMe + CardUtility.strSplitMark + (i + 1).ToString("D1")).DealMinion(game, game.AllRole.MyPublicInfo.BattleField.BattleMinions[i]));
                            }
                            break;
                        default:
                            Result.Add(GetEffectHandler(singleEffect, game, PosInfo).DealMinion(game, game.AllRole.MyPublicInfo.BattleField.BattleMinions[int.Parse(PosField[1]) - 1]));
                            break;
                    }
                }
                else
                {
                    switch (int.Parse(PosField[1]))
                    {
                        case BattleFieldInfo.HeroPos:
                            Result.Add(GetEffectHandler(singleEffect, game, CardUtility.strYou + CardUtility.strSplitMark + Client.BattleFieldInfo.HeroPos.ToString("D1")).DealHero(game, game.AllRole.YourPublicInfo));
                            break;
                        case BattleFieldInfo.AllMinionPos:
                            for (int i = 0; i <game.AllRole.YourPublicInfo.BattleField.MinionCount; i++)
                            {
                                Result.Add(GetEffectHandler(singleEffect, game, CardUtility.strYou + CardUtility.strSplitMark + (i + 1).ToString("D1")).DealMinion(game, game.AllRole.YourPublicInfo.BattleField.BattleMinions[i]));
                            }
                            break;
                        case BattleFieldInfo.AllRolePos:
                            Result.Add(GetEffectHandler(singleEffect, game, CardUtility.strYou + CardUtility.strSplitMark + Client.BattleFieldInfo.HeroPos.ToString("D1")).DealHero(game, game.AllRole.YourPublicInfo));
                            for (int i = 0; i <game.AllRole.YourPublicInfo.BattleField.MinionCount; i++)
                            {
                                Result.Add(GetEffectHandler(singleEffect, game, CardUtility.strYou + CardUtility.strSplitMark + (i + 1).ToString("D1")).DealMinion(game, game.AllRole.YourPublicInfo.BattleField.BattleMinions[i]));
                            }
                            break;
                        default:
                            Result.Add(GetEffectHandler(singleEffect, game, PosInfo).DealMinion(game, game.AllRole.YourPublicInfo.BattleField.BattleMinions[int.Parse(PosField[1]) - 1]));
                            break;
                    }
                }
            }
            return Result;
        }
        /// <summary>
        /// 根据施法对象获得不同法术
        /// </summary>
        /// <param name="singleEffect"></param>
        /// <param name="game"></param>
        /// <param name="PosInfo"></param>
        /// <returns></returns>
        private static IAtomicEffect GetEffectHandler(EffectDefine singleEffect, ActionStatus game, String PosInfo)
        {
            AtomicEffectDefine atomic;
            if (String.IsNullOrEmpty(singleEffect.效果条件) ||
                singleEffect.效果条件 == CardUtility.strIgnore ||
                ExpressHandler.AtomicEffectPickCondition(game, PosInfo, singleEffect))
            {
                atomic = singleEffect.TrueAtomicEffect;
            }
            else
            {
                atomic = singleEffect.FalseAtomicEffect;
            }
            IAtomicEffect IAtomic = new AttackEffect();
            switch (atomic.AtomicEffectType)
            {
                case AtomicEffectDefine.AtomicEffectEnum.攻击:
                    IAtomic = new AttackEffect();
                    break;
                case AtomicEffectDefine.AtomicEffectEnum.回复:
                    IAtomic = new HealthEffect();
                    break;
                case AtomicEffectDefine.AtomicEffectEnum.状态:
                    IAtomic = new StatusEffect();
                    break;
                case AtomicEffectDefine.AtomicEffectEnum.增益:
                    IAtomic = new PointEffect();
                    break;
                case AtomicEffectDefine.AtomicEffectEnum.变形:
                    IAtomic = new TransformEffect();
                    break;
            }
            IAtomic.GetField(atomic.InfoArray);
            return (IAtomicEffect)IAtomic;
        }
    }
}
