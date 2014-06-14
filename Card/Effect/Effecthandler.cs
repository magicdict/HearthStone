using System;
using System.Collections.Generic;

namespace Card.Effect
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
        public static List<string> GetTargetList(CardUtility.SelectOption SelectOpt, Client.GameManager game, int RandSeed)
        {
            //切记，这里的EffectCount都是1
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
                                    Result.Add(CardUtility.strMe + CardUtility.strSplitMark + Client.BattleFieldInfo.AllMinionPos.ToString("D1"));
                                    Result.Add(CardUtility.strMe + CardUtility.strSplitMark + Client.BattleFieldInfo.HeroPos.ToString("D1"));
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
                                    Result.Add(CardUtility.strYou + CardUtility.strSplitMark + Client.BattleFieldInfo.AllMinionPos.ToString("D1"));
                                    Result.Add(CardUtility.strYou + CardUtility.strSplitMark + Client.BattleFieldInfo.HeroPos.ToString("D1"));
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
                                    Result.Add(CardUtility.strMe + CardUtility.strSplitMark + Client.BattleFieldInfo.AllMinionPos.ToString("D1"));
                                    Result.Add(CardUtility.strYou + CardUtility.strSplitMark + Client.BattleFieldInfo.AllMinionPos.ToString("D1"));
                                    Result.Add(CardUtility.strMe + CardUtility.strSplitMark + Client.BattleFieldInfo.HeroPos.ToString("D1"));
                                    Result.Add(CardUtility.strYou + CardUtility.strSplitMark + Client.BattleFieldInfo.HeroPos.ToString("D1"));
                                    break;
                            }
                            break;
                    }
                    break;
                case CardUtility.TargetSelectModeEnum.指定:
                case CardUtility.TargetSelectModeEnum.继承:
                case CardUtility.TargetSelectModeEnum.横扫:
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
        public static List<String> RunSingleEffect(CardUtility.SelectOption AbliltyPosPicker, EffectDefine singleEffect, Card.Client.GameManager game, int RandomSeed)
        {
            List<String> Result = new List<string>();
            List<String> PosList = GetTargetList(AbliltyPosPicker, game, RandomSeed);
            foreach (String PosInfo in PosList)
            {
                var PosField = PosInfo.Split(CardUtility.strSplitMark.ToCharArray());
                var strResult = String.Empty;
                if (PosField[0] == CardUtility.strMe)
                {
                    strResult += CardUtility.strMe + Card.CardUtility.strSplitMark;
                    switch (int.Parse(PosField[1]))
                    {
                        case Card.Client.BattleFieldInfo.HeroPos:
                            GetEffectHandler(singleEffect, game, PosInfo).DealHero(game, singleEffect, true);
                            strResult += Card.Client.BattleFieldInfo.HeroPos.ToString();
                            break;
                        case Card.Client.BattleFieldInfo.AllMinionPos:
                            for (int i = 0; i < game.MyInfo.BattleField.MinionCount; i++)
                            {
                                GetEffectHandler(singleEffect, game, PosInfo).DealMinion(game, singleEffect, true, i);
                            }
                            strResult += Card.Client.BattleFieldInfo.AllMinionPos.ToString();
                            break;
                        default:
                            GetEffectHandler(singleEffect, game, PosInfo).DealMinion(game, singleEffect, true, int.Parse(PosField[1]) - 1);
                            strResult += PosField[1];
                            break;
                    }
                }
                else
                {
                    strResult += CardUtility.strYou + Card.CardUtility.strSplitMark;
                    switch (int.Parse(PosField[1]))
                    {
                        case Card.Client.BattleFieldInfo.HeroPos:
                            GetEffectHandler(singleEffect, game, PosInfo).DealHero(game, singleEffect, false);
                            strResult += Card.Client.BattleFieldInfo.HeroPos.ToString();
                            break;
                        case Card.Client.BattleFieldInfo.AllMinionPos:
                            for (int i = 0; i < game.YourInfo.BattleField.MinionCount; i++)
                            {
                                GetEffectHandler(singleEffect, game, PosInfo).DealMinion(game, singleEffect, false, i);
                            }
                            strResult += Card.Client.BattleFieldInfo.AllMinionPos.ToString();
                            break;
                        default:
                            GetEffectHandler(singleEffect, game, PosInfo).DealMinion(game, singleEffect, false, int.Parse(PosField[1]) - 1);
                            strResult += PosField[1];
                            break;
                    }
                    strResult += Card.CardUtility.strSplitMark;
                    Result.Add(strResult);
                }
            }
            return Result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="singleEffect"></param>
        /// <param name="game"></param>
        /// <param name="PosInfo"></param>
        /// <returns></returns>
        private static IEffectHandler GetEffectHandler(EffectDefine singleEffect, Card.Client.GameManager game, String PosInfo)
        {
            if (String.IsNullOrEmpty(singleEffect.效果条件)) return (IEffectHandler)singleEffect.TrueAtomicEffect;
            if (Client.ExpressHandler.BattleFieldCondition(game, PosInfo, singleEffect.效果条件))
            {
                return (IEffectHandler)singleEffect.TrueAtomicEffect;
            }
            else
            {
                return (IEffectHandler)singleEffect.FalseAtomicEffect;
            }
        }
    }
}
