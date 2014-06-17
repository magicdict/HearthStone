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
        public static List<string> GetTargetList(CardUtility.PositionSelectOption SelectOpt, Client.GameManager game, int RandSeed)
        {
            List<string> Result = new List<string>();
            switch (SelectOpt.EffictTargetSelectMode)
            {
                case CardUtility.TargetSelectModeEnum.随机:
                    Random t = new Random(DateTime.Now.Millisecond + RandSeed);
                    switch (SelectOpt.EffectTargetSelectDirect)
                    {
                        case CardUtility.TargetSelectDirectEnum.本方:
                            switch (SelectOpt.EffectTargetSelectRole)
                            {
                                case CardUtility.TargetSelectRoleEnum.随从:
                                    SelectOpt.SelectedPos.Postion = t.Next(1, game.MyInfo.BattleField.MinionCount + 1);
                                    SelectOpt.SelectedPos.MeOrYou = true;
                                    break;
                                case CardUtility.TargetSelectRoleEnum.所有角色:
                                    SelectOpt.SelectedPos.Postion = t.Next(Client.BattleFieldInfo.HeroPos, game.MyInfo.BattleField.MinionCount + 1);
                                    SelectOpt.SelectedPos.MeOrYou = true;
                                    break;
                            }
                            //ME#POS
                            Result.Add(CardUtility.strMe + CardUtility.strSplitMark + SelectOpt.SelectedPos.Postion.ToString("D1"));
                            break;
                        case CardUtility.TargetSelectDirectEnum.对方:
                            switch (SelectOpt.EffectTargetSelectRole)
                            {
                                case CardUtility.TargetSelectRoleEnum.随从:
                                    SelectOpt.SelectedPos.Postion = t.Next(1, game.YourInfo.BattleField.MinionCount + 1);
                                    SelectOpt.SelectedPos.MeOrYou = false;
                                    break;
                                case CardUtility.TargetSelectRoleEnum.所有角色:
                                    SelectOpt.SelectedPos.Postion = t.Next(Client.BattleFieldInfo.HeroPos, game.YourInfo.BattleField.MinionCount + 1);
                                    SelectOpt.SelectedPos.MeOrYou = false;
                                    break;
                            }
                            //ME#POS
                            Result.Add(CardUtility.strYou + CardUtility.strSplitMark + SelectOpt.SelectedPos.Postion.ToString("D1"));
                            break;
                        case CardUtility.TargetSelectDirectEnum.双方:
                            //本方对方
                            int MinionCount;
                            if (t.Next(1, 3) == 1)
                            {
                                SelectOpt.SelectedPos.MeOrYou = true;
                                MinionCount = game.MyInfo.BattleField.MinionCount;
                            }
                            else
                            {
                                SelectOpt.SelectedPos.MeOrYou = false;
                                MinionCount = game.YourInfo.BattleField.MinionCount;
                            }
                            switch (SelectOpt.EffectTargetSelectRole)
                            {
                                case CardUtility.TargetSelectRoleEnum.随从:
                                    SelectOpt.SelectedPos.Postion = t.Next(1, MinionCount + 1);
                                    break;
                                case CardUtility.TargetSelectRoleEnum.所有角色:
                                    SelectOpt.SelectedPos.Postion = t.Next(Client.BattleFieldInfo.HeroPos, MinionCount + 1);
                                    break;
                            }
                            //ME#POS
                            Result.Add((SelectOpt.SelectedPos.MeOrYou ? CardUtility.strMe : CardUtility.strYou) + CardUtility.strSplitMark + SelectOpt.SelectedPos.Postion.ToString("D1"));
                            break;
                        default:
                            break;
                    }
                    break;
                case CardUtility.TargetSelectModeEnum.全体:
                case CardUtility.TargetSelectModeEnum.横扫:
                    switch (SelectOpt.EffectTargetSelectDirect)
                    {
                        case CardUtility.TargetSelectDirectEnum.本方:
                            switch (SelectOpt.EffectTargetSelectRole)
                            {
                                case CardUtility.TargetSelectRoleEnum.随从:
                                    Result.Add(CardUtility.strMe + CardUtility.strSplitMark + Client.BattleFieldInfo.AllMinionPos.ToString("D1"));
                                    break;
                                case CardUtility.TargetSelectRoleEnum.英雄:
                                    Result.Add(CardUtility.strMe + CardUtility.strSplitMark + Client.BattleFieldInfo.HeroPos.ToString("D1"));
                                    break;
                                case CardUtility.TargetSelectRoleEnum.所有角色:
                                    Result.Add(CardUtility.strMe + CardUtility.strSplitMark + Client.BattleFieldInfo.AllRolePos.ToString("D1"));
                                    break;
                            }
                            break;
                        case CardUtility.TargetSelectDirectEnum.对方:
                            switch (SelectOpt.EffectTargetSelectRole)
                            {
                                case CardUtility.TargetSelectRoleEnum.随从:
                                    Result.Add(CardUtility.strYou + CardUtility.strSplitMark + Client.BattleFieldInfo.AllMinionPos.ToString("D1"));
                                    break;
                                case CardUtility.TargetSelectRoleEnum.英雄:
                                    Result.Add(CardUtility.strYou + CardUtility.strSplitMark + Client.BattleFieldInfo.HeroPos.ToString("D1"));
                                    break;
                                case CardUtility.TargetSelectRoleEnum.所有角色:
                                    Result.Add(CardUtility.strYou + CardUtility.strSplitMark + Client.BattleFieldInfo.AllRolePos.ToString("D1"));
                                    break;
                            }
                            break;
                        case CardUtility.TargetSelectDirectEnum.双方:
                            switch (SelectOpt.EffectTargetSelectRole)
                            {
                                case CardUtility.TargetSelectRoleEnum.随从:
                                    Result.Add(CardUtility.strMe + CardUtility.strSplitMark + Client.BattleFieldInfo.AllMinionPos.ToString("D1"));
                                    Result.Add(CardUtility.strYou + CardUtility.strSplitMark + Client.BattleFieldInfo.AllMinionPos.ToString("D1"));
                                    break;
                                case CardUtility.TargetSelectRoleEnum.英雄:
                                    Result.Add(CardUtility.strMe + CardUtility.strSplitMark + Client.BattleFieldInfo.HeroPos.ToString("D1"));
                                    Result.Add(CardUtility.strYou + CardUtility.strSplitMark + Client.BattleFieldInfo.HeroPos.ToString("D1"));
                                    break;
                                case CardUtility.TargetSelectRoleEnum.所有角色:
                                    Result.Add(CardUtility.strMe + CardUtility.strSplitMark + Client.BattleFieldInfo.AllRolePos.ToString("D1"));
                                    Result.Add(CardUtility.strYou + CardUtility.strSplitMark + Client.BattleFieldInfo.AllRolePos.ToString("D1"));
                                    break;
                            }
                            break;
                    }
                    break;
                case CardUtility.TargetSelectModeEnum.指定:
                case CardUtility.TargetSelectModeEnum.继承:
                    Result.Add((SelectOpt.SelectedPos.MeOrYou ? CardUtility.strMe : CardUtility.strYou) + CardUtility.strSplitMark + SelectOpt.SelectedPos.Postion.ToString("D1"));
                    break;
                case CardUtility.TargetSelectModeEnum.不用选择:
                    if (SelectOpt.EffectTargetSelectRole == CardUtility.TargetSelectRoleEnum.英雄)
                    {
                        switch (SelectOpt.EffectTargetSelectDirect)
                        {
                            case CardUtility.TargetSelectDirectEnum.本方:
                                Result.Add(CardUtility.strMe + CardUtility.strSplitMark + Client.BattleFieldInfo.HeroPos.ToString("D1"));
                                break;
                            case CardUtility.TargetSelectDirectEnum.对方:
                                Result.Add(CardUtility.strYou + CardUtility.strSplitMark + Client.BattleFieldInfo.HeroPos.ToString("D1"));
                                break;
                            case CardUtility.TargetSelectDirectEnum.双方:
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
        /// <param name="singleEffect">效果</param>
        /// <param name="Field"></param>
        /// <param name="Pos">指定对象</param>
        /// <returns></returns>
        public static List<String> RunSingleEffect(EffectDefine singleEffect, Engine.Client.GameManager game, int RandomSeed)
        {
            List<String> Result = new List<string>();
            List<String> PosList = GetTargetList(singleEffect.AbliltyPosPicker, game, RandomSeed);
            foreach (String PosInfo in PosList)
            {
                var PosField = PosInfo.Split(CardUtility.strSplitMark.ToCharArray());
                var strResult = String.Empty;
                if (PosField[0] == CardUtility.strMe)
                {
                    strResult += CardUtility.strMe + Engine.Utility.CardUtility.strSplitMark;
                    switch (int.Parse(PosField[1]))
                    {
                        case Engine.Client.BattleFieldInfo.HeroPos:
                            GetEffectHandler(singleEffect, game, CardUtility.strMe + CardUtility.strSplitMark + Client.BattleFieldInfo.HeroPos.ToString("D1")).DealHero(game, singleEffect, true);
                            strResult += Engine.Client.BattleFieldInfo.HeroPos.ToString();
                            break;
                        case Engine.Client.BattleFieldInfo.AllMinionPos:
                            for (int i = 0; i < game.MyInfo.BattleField.MinionCount; i++)
                            {
                                GetEffectHandler(singleEffect, game, CardUtility.strMe + CardUtility.strSplitMark + (i + 1).ToString("D1")).DealMinion(game, singleEffect, true, i);
                                strResult += CardUtility.strMe + CardUtility.strSplitMark + (i + 1).ToString("D1");
                            }
                            break;
                        case Engine.Client.BattleFieldInfo.AllRolePos:
                            GetEffectHandler(singleEffect, game, CardUtility.strMe + CardUtility.strSplitMark + Client.BattleFieldInfo.HeroPos.ToString("D1")).DealHero(game, singleEffect, true);
                            strResult += Engine.Client.BattleFieldInfo.HeroPos.ToString();
                            for (int i = 0; i < game.MyInfo.BattleField.MinionCount; i++)
                            {
                                GetEffectHandler(singleEffect, game, CardUtility.strMe + CardUtility.strSplitMark + (i + 1).ToString("D1")).DealMinion(game, singleEffect, true, i);
                                strResult += CardUtility.strMe + CardUtility.strSplitMark + (i + 1).ToString("D1");
                            }
                            break;
                        default:
                            GetEffectHandler(singleEffect, game, PosInfo).DealMinion(game, singleEffect, true, int.Parse(PosField[1]) - 1);
                            strResult += PosField[1];
                            break;
                    }
                }
                else
                {
                    strResult += CardUtility.strYou + Engine.Utility.CardUtility.strSplitMark;
                    switch (int.Parse(PosField[1]))
                    {
                        case Engine.Client.BattleFieldInfo.HeroPos:
                            GetEffectHandler(singleEffect, game, CardUtility.strYou + CardUtility.strSplitMark + Client.BattleFieldInfo.HeroPos.ToString("D1")).DealHero(game, singleEffect, false);
                            strResult += Engine.Client.BattleFieldInfo.HeroPos.ToString();
                            break;
                        case Engine.Client.BattleFieldInfo.AllMinionPos:
                            for (int i = 0; i < game.YourInfo.BattleField.MinionCount; i++)
                            {
                                GetEffectHandler(singleEffect, game, CardUtility.strYou + CardUtility.strSplitMark + (i + 1).ToString("D1")).DealMinion(game, singleEffect, false, i);
                                strResult += CardUtility.strYou + CardUtility.strSplitMark + (i + 1).ToString("D1");
                            }
                            break;
                        case Engine.Client.BattleFieldInfo.AllRolePos:
                            GetEffectHandler(singleEffect, game, CardUtility.strYou + CardUtility.strSplitMark + Client.BattleFieldInfo.HeroPos.ToString("D1")).DealHero(game, singleEffect, false);
                            strResult += Engine.Client.BattleFieldInfo.HeroPos.ToString();
                            for (int i = 0; i < game.YourInfo.BattleField.MinionCount; i++)
                            {
                                GetEffectHandler(singleEffect, game, CardUtility.strYou + CardUtility.strSplitMark + (i + 1).ToString("D1")).DealMinion(game, singleEffect, false, i);
                                strResult += CardUtility.strYou + CardUtility.strSplitMark + (i + 1).ToString("D1");
                            }
                            break;
                        default:
                            GetEffectHandler(singleEffect, game, PosInfo).DealMinion(game, singleEffect, false, int.Parse(PosField[1]) - 1);
                            strResult += PosField[1];
                            break;
                    }
                    strResult += Engine.Utility.CardUtility.strSplitMark;
                    Result.Add(strResult);
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
        private static IAtomicEffect GetEffectHandler(EffectDefine singleEffect, Engine.Client.GameManager game, String PosInfo)
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
