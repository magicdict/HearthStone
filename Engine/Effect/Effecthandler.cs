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
        /// 实施效果
        /// </summary>
        /// <param name="singleEffect"></param>
        /// <param name="game"></param>
        /// <param name="RandomSeed"></param>
        /// <returns></returns>
        public static List<String> RunSingleEffect(EffectDefine singleEffect, ActionStatus game, int RandomSeed)
        {
            List<String> Result = new List<string>();
            List<String> PosList = SelectUtility.GetTargetList(singleEffect.AbliltyPosPicker, game, RandomSeed);
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
