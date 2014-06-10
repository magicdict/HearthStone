using System;
using System.Collections.Generic;

namespace Card.Effect
{
    public class Effecthandler
    {
        /// <summary>
        /// 效果点数的表达式计算
        /// </summary>
        /// <param name="strEffectPoint"></param>
        /// <returns></returns>
        public static int GetEffectPoint(Client.GameManager game,String strEffectPoint)
        {
            int point = 0;
            if (!String.IsNullOrEmpty(strEffectPoint))
            {
                if (strEffectPoint.StartsWith("="))
                {
                    switch (strEffectPoint.Substring(1))
                    {
                        case "MyWeaponAP":
                            if (game.MyInfo.Weapon != null) point = game.MyInfo.Weapon.实际攻击力;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    point = int.Parse(strEffectPoint);
                }
            }
            return point;
        }
        /// <summary>
        /// 施法对象列表
        /// </summary>
        /// <param name="singleEffect"></param>
        /// <param name="game"></param>
        /// <param name="PosInfo"></param>
        /// <param name="Seed"></param>
        /// <returns></returns>
        public static List<string> GetTargetList(CardUtility.SelectOption SelectOpt, Client.GameManager game, CardUtility.TargetPosition PosInfo, int Seed)
        {
            //切记，这里的EffectCount都是1
            List<string> Result = new List<string>();
            switch (SelectOpt.EffictTargetSelectMode)
            {
                case CardUtility.TargetSelectModeEnum.随机:
                    Random t = new Random(DateTime.Now.Millisecond + Seed);
                    switch (SelectOpt.EffectTargetSelectDirect)
                    {
                        case CardUtility.TargetSelectDirectEnum.本方:
                            switch (SelectOpt.EffectTargetSelectRole)
                            {
                                case CardUtility.TargetSelectRoleEnum.随从:
                                    PosInfo.Postion = t.Next(1, game.MyInfo.BattleField.MinionCount + 1);
                                    PosInfo.MeOrYou = true;
                                    break;
                                case CardUtility.TargetSelectRoleEnum.所有角色:
                                    PosInfo.Postion = t.Next(Client.BattleFieldInfo.HeroPos, game.MyInfo.BattleField.MinionCount + 1);
                                    PosInfo.MeOrYou = true;
                                    break;
                            }
                            //ME#POS
                            Result.Add(CardUtility.strMe + CardUtility.strSplitMark + PosInfo.Postion.ToString("D1"));
                            break;
                        case CardUtility.TargetSelectDirectEnum.对方:
                            switch (SelectOpt.EffectTargetSelectRole)
                            {
                                case CardUtility.TargetSelectRoleEnum.随从:
                                    PosInfo.Postion = t.Next(1, game.YourInfo.BattleField.MinionCount + 1);
                                    PosInfo.MeOrYou = false;
                                    break;
                                case CardUtility.TargetSelectRoleEnum.所有角色:
                                    PosInfo.Postion = t.Next(Client.BattleFieldInfo.HeroPos, game.YourInfo.BattleField.MinionCount + 1);
                                    PosInfo.MeOrYou = false;
                                    break;
                            }
                            //ME#POS
                            Result.Add(CardUtility.strYou + CardUtility.strSplitMark + PosInfo.Postion.ToString("D1"));
                            break;
                        case CardUtility.TargetSelectDirectEnum.双方:
                            //本方对方
                            int MinionCount;
                            if (t.Next(1, 3) == 1)
                            {
                                PosInfo.MeOrYou = true;
                                MinionCount = game.MyInfo.BattleField.MinionCount;
                            }
                            else
                            {
                                PosInfo.MeOrYou = false;
                                MinionCount = game.YourInfo.BattleField.MinionCount;
                            }
                            switch (SelectOpt.EffectTargetSelectRole)
                            {
                                case CardUtility.TargetSelectRoleEnum.随从:
                                    PosInfo.Postion = t.Next(1, MinionCount + 1);
                                    break;
                                case CardUtility.TargetSelectRoleEnum.所有角色:
                                    PosInfo.Postion = t.Next(Client.BattleFieldInfo.HeroPos, MinionCount + 1);
                                    break;
                            }
                            //ME#POS
                            Result.Add((PosInfo.MeOrYou ? CardUtility.strMe : CardUtility.strYou) + CardUtility.strSplitMark + PosInfo.Postion.ToString("D1"));
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
                                    Result.Add(CardUtility.strMe + CardUtility.strSplitMark + Client.BattleFieldInfo.AllPos.ToString("D1"));
                                    break;
                                case CardUtility.TargetSelectRoleEnum.英雄:
                                    Result.Add(CardUtility.strMe + CardUtility.strSplitMark + Client.BattleFieldInfo.HeroPos.ToString("D1"));
                                    break;
                                case CardUtility.TargetSelectRoleEnum.所有角色:
                                    Result.Add(CardUtility.strMe + CardUtility.strSplitMark + Client.BattleFieldInfo.AllPos.ToString("D1"));
                                    Result.Add(CardUtility.strMe + CardUtility.strSplitMark + Client.BattleFieldInfo.HeroPos.ToString("D1"));
                                    break;
                            }
                            break;
                        case CardUtility.TargetSelectDirectEnum.对方:
                            switch (SelectOpt.EffectTargetSelectRole)
                            {
                                case CardUtility.TargetSelectRoleEnum.随从:
                                    Result.Add(CardUtility.strYou + CardUtility.strSplitMark + Client.BattleFieldInfo.AllPos.ToString("D1"));
                                    break;
                                case CardUtility.TargetSelectRoleEnum.英雄:
                                    Result.Add(CardUtility.strYou + CardUtility.strSplitMark + Client.BattleFieldInfo.HeroPos.ToString("D1"));
                                    break;
                                case CardUtility.TargetSelectRoleEnum.所有角色:
                                    Result.Add(CardUtility.strYou + CardUtility.strSplitMark + Client.BattleFieldInfo.AllPos.ToString("D1"));
                                    Result.Add(CardUtility.strYou + CardUtility.strSplitMark + Client.BattleFieldInfo.HeroPos.ToString("D1"));
                                    break;
                            }
                            break;
                        case CardUtility.TargetSelectDirectEnum.双方:
                            switch (SelectOpt.EffectTargetSelectRole)
                            {
                                case CardUtility.TargetSelectRoleEnum.随从:
                                    Result.Add(CardUtility.strMe + CardUtility.strSplitMark + Client.BattleFieldInfo.AllPos.ToString("D1"));
                                    Result.Add(CardUtility.strYou + CardUtility.strSplitMark + Client.BattleFieldInfo.AllPos.ToString("D1"));
                                    break;
                                case CardUtility.TargetSelectRoleEnum.英雄:
                                    Result.Add(CardUtility.strMe + CardUtility.strSplitMark + Client.BattleFieldInfo.HeroPos.ToString("D1"));
                                    Result.Add(CardUtility.strYou + CardUtility.strSplitMark + Client.BattleFieldInfo.HeroPos.ToString("D1"));
                                    break;
                                case CardUtility.TargetSelectRoleEnum.所有角色:
                                    Result.Add(CardUtility.strMe + CardUtility.strSplitMark + Client.BattleFieldInfo.AllPos.ToString("D1"));
                                    Result.Add(CardUtility.strYou + CardUtility.strSplitMark + Client.BattleFieldInfo.AllPos.ToString("D1"));
                                    Result.Add(CardUtility.strMe + CardUtility.strSplitMark + Client.BattleFieldInfo.HeroPos.ToString("D1"));
                                    Result.Add(CardUtility.strYou + CardUtility.strSplitMark + Client.BattleFieldInfo.HeroPos.ToString("D1"));
                                    break;
                            }
                            break;
                    }
                    break;
                case CardUtility.TargetSelectModeEnum.指定:
                case CardUtility.TargetSelectModeEnum.继承:
                    Result.Add((PosInfo.MeOrYou ? CardUtility.strMe : CardUtility.strYou) + CardUtility.strSplitMark + PosInfo.Postion.ToString("D1"));
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
        public static List<String> RunSingleEffect(AtomicEffectDefine singleEffect, Card.Client.GameManager game, Card.CardUtility.TargetPosition Pos, int Seed)
        {
            List<String> Result = new List<string>();
            //List<String> PosList = GetTargetList(singleEffect, game, Pos, Seed);
            List<String> PosList = new List<string>();
            //切记，这里的EffectCount都是1
            switch (singleEffect.AbilityEffectType)
            {
                case Card.Effect.AtomicEffectDefine.AbilityEffectEnum.攻击:
                case Card.Effect.AtomicEffectDefine.AbilityEffectEnum.回复:
                case Card.Effect.AtomicEffectDefine.AbilityEffectEnum.状态:
                case Card.Effect.AtomicEffectDefine.AbilityEffectEnum.增益:
                case Card.Effect.AtomicEffectDefine.AbilityEffectEnum.变形:
                    Result.AddRange(RunNormalSingleEffect(singleEffect, game, PosList));
                    break;
                case Card.Effect.AtomicEffectDefine.AbilityEffectEnum.召唤:
                    Result.AddRange(((SummonEffect)singleEffect).RunEffect(game));
                    break;
                case Card.Effect.AtomicEffectDefine.AbilityEffectEnum.卡牌:
                    Result.AddRange(((CardEffect)singleEffect).RunEffect(game));
                    break;
                case Card.Effect.AtomicEffectDefine.AbilityEffectEnum.水晶:
                    Result.AddRange(((CrystalEffect)singleEffect).RunEffect(game));
                    break;
                case Card.Effect.AtomicEffectDefine.AbilityEffectEnum.控制:
                    Result.AddRange(((ControlEffect)singleEffect).RunEffect(game, PosList[0]));
                    break;
                case Card.Effect.AtomicEffectDefine.AbilityEffectEnum.武器:
                    Result.AddRange(((WeaponPointEffect)singleEffect).RunEffect(game));
                    break;
                default:
                    break;
            }
            return Result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="singleEffect"></param>
        /// <param name="game"></param>
        /// <param name="PosList"></param>
        /// <returns></returns>
        public static List<String> RunNormalSingleEffect(AtomicEffectDefine singleEffect, Client.GameManager game, List<String> PosList)
        {
            List<String> Result = new List<string>();
            String strResult = String.Empty;
            String strEffect = String.Empty;
            IEffectHandler handler = new AttackEffect();
            switch (singleEffect.AbilityEffectType)
            {
                case Card.Effect.AtomicEffectDefine.AbilityEffectEnum.攻击:
                    handler = new AttackEffect();
                    strResult = Card.Server.ActionCode.strAttack;
                    strEffect = singleEffect.ActualEffectPoint.ToString();
                    break;
                case Card.Effect.AtomicEffectDefine.AbilityEffectEnum.回复:
                    handler = new HealthEffect();
                    strResult = Card.Server.ActionCode.strHealth;
                    strEffect = singleEffect.ActualEffectPoint.ToString() + CardUtility.strSplitMark + singleEffect.AdditionInfo;
                    break;
                case Card.Effect.AtomicEffectDefine.AbilityEffectEnum.状态:
                    handler = new StatusEffect();
                    strResult = Card.Server.ActionCode.strStatus;
                    strEffect = singleEffect.AdditionInfo;
                    break;
                case Card.Effect.AtomicEffectDefine.AbilityEffectEnum.增益:
                    handler = new PointEffect();
                    strResult = Card.Server.ActionCode.strPoint;
                    strEffect = singleEffect.AdditionInfo + CardUtility.strSplitMark + singleEffect.StandardEffectPoint;
                    break;
                case Card.Effect.AtomicEffectDefine.AbilityEffectEnum.变形:
                    handler = new TransformEffect();
                    strResult = Card.Server.ActionCode.strTransform;
                    strEffect = singleEffect.AdditionInfo;
                    break;
            }
            strResult += Card.CardUtility.strSplitMark;
            foreach (var PosInfo in PosList)
            {
                var PosField = PosInfo.Split(CardUtility.strSplitMark.ToCharArray());
                if (PosField[0] == CardUtility.strMe)
                {
                    strResult += CardUtility.strMe + Card.CardUtility.strSplitMark;
                    switch (int.Parse(PosField[1]))
                    {
                        case Card.Client.BattleFieldInfo.HeroPos:
                            handler.DealHero(game, singleEffect, true);
                            strResult += Card.Client.BattleFieldInfo.HeroPos.ToString();
                            break;
                        case Card.Client.BattleFieldInfo.AllPos:
                            for (int i = 0; i < game.MyInfo.BattleField.MinionCount; i++)
                            {
                                handler.DealMinion(game, singleEffect, true, i);
                            }
                            strResult += Card.Client.BattleFieldInfo.AllPos.ToString();
                            break;
                        default:
                            handler.DealMinion(game, singleEffect, true, int.Parse(PosField[1]) - 1);
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
                            handler.DealHero(game, singleEffect, false);
                            strResult += Card.Client.BattleFieldInfo.HeroPos.ToString();
                            break;
                        case Card.Client.BattleFieldInfo.AllPos:
                            for (int i = 0; i < game.YourInfo.BattleField.MinionCount; i++)
                            {
                                handler.DealMinion(game, singleEffect, false, i);
                            }
                            strResult += Card.Client.BattleFieldInfo.AllPos.ToString();
                            break;
                        default:
                            handler.DealMinion(game, singleEffect, false, int.Parse(PosField[1]) - 1);
                            strResult += PosField[1];
                            break;
                    }
                }
                strResult += Card.CardUtility.strSplitMark + strEffect;
                Result.Add(strResult);
            }
            return Result;
        }
    }
}
