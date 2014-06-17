using System;

namespace Engine.Utility
{
    /// <summary>
    /// 表达式处理器
    /// </summary>
    public static class ExpressHandler
    {
        /// <summary>
        /// 增益计算
        /// </summary>
        /// <param name="oldPoint"></param>
        /// <param name="ModifyPoint"></param>
        /// <returns></returns>
        public static int PointProcess(int oldPoint, String ModifyPoint)
        {
            int newPoint = oldPoint;
            if (ModifyPoint != CardUtility.strIgnore)
            {
                if (ModifyPoint.Length != 1)
                {
                    switch (ModifyPoint.Substring(0, 1))
                    {
                        case "+":
                        case "-":
                            newPoint += int.Parse(ModifyPoint);
                            break;
                        case "*":
                            newPoint *= int.Parse(ModifyPoint.Substring(1, 1));
                            break;
                        default:
                            newPoint = int.Parse(ModifyPoint);
                            break;
                    }
                }
            }
            return newPoint;
        }
        /// <summary>
        /// 效果点数的表达式计算
        /// </summary>
        /// <param name="strEffectPoint"></param>
        /// <returns></returns>
        public static int GetEffectPoint(Client.GameManager game, String strEffectPoint)
        {
            int point = 0;
            strEffectPoint = strEffectPoint.ToUpper();
            if (!String.IsNullOrEmpty(strEffectPoint))
            {
                if (strEffectPoint.StartsWith("="))
                {
                    switch (strEffectPoint.Substring(1))
                    {
                        case "MYWEAPONAP":
                            //本方武器攻击力
                            if (game.MyInfo.Weapon != null) point = game.MyInfo.Weapon.攻击力;
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
        /// 目标效果表达式判断
        /// </summary>
        /// <param name="game"></param>
        /// <param name="PosInfo"></param>
        /// <param name="singleEffect"></param>
        /// <returns></returns>
        public static bool AtomicEffectPickCondition(Client.GameManager game, string PosInfo, Effect.EffectDefine singleEffect)
        {
            var 效果条件 = singleEffect.效果条件.ToUpper();
            String YouOrMe = PosInfo.Split(CardUtility.strSplitMark.ToCharArray())[0];
            String Position = PosInfo.Split(CardUtility.strSplitMark.ToCharArray())[1];
            switch (效果条件)
            {
                case "POSITION":
                    return PosInfo == singleEffect.AbliltyPosPicker.SelectedPos.ToString();
                case "ISFREEZE":
                    if (YouOrMe == CardUtility.strMe)
                    {
                        if (Position == Client.BattleFieldInfo.HeroPos.ToString("D1"))
                        {
                            return game.MyInfo.冰冻状态 != CardUtility.EffectTurn.无效果;
                        }
                        else
                        {
                            return game.MyInfo.BattleField.BattleMinions[int.Parse(Position) - 1].冰冻状态 != CardUtility.EffectTurn.无效果;
                        }
                    }
                    else
                    {
                        if (Position == Client.BattleFieldInfo.HeroPos.ToString("D1"))
                        {
                            return game.YourInfo.冰冻状态 != CardUtility.EffectTurn.无效果;
                        }
                        else
                        {
                            return game.YourInfo.BattleField.BattleMinions[int.Parse(Position) - 1].冰冻状态 != CardUtility.EffectTurn.无效果;
                        }
                    }
                default:
                    break;
            }
            return true;
        }
        /// <summary>
        /// 自动计算战场情况
        /// </summary>
        /// <param name="game"></param>
        /// <param name="效果选择条件"></param>
        /// <returns></returns>
        public static bool AbilityPickCondition(Client.GameManager game, string 效果选择条件)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 追加法术条件计算
        /// </summary>
        /// <param name="game"></param>
        /// <param name="Ability"></param>
        /// <returns></returns>
        internal static bool AppendAbilityCondition(Client.GameManager game, Card.AbilityCard.AbilityDefine Ability)
        {
            if (Ability.AppendEffectCondition == CardUtility.strIgnore)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
