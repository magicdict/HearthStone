using System;

namespace Engine.Utility
{
    /// <summary>
    /// 表达式处理器
    /// </summary>
    public static class ExpressHandler
    {
        /// <summary>
        /// PointProcess
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
            if (!String.IsNullOrEmpty(strEffectPoint))
            {
                if (strEffectPoint.StartsWith("="))
                {
                    switch (strEffectPoint.Substring(1))
                    {
                        case "MyWeaponAP":
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
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <param name="PosInfo"></param>
        /// <param name="singleEffect"></param>
        /// <returns></returns>
        public static bool AtomicEffectPickCondition(Client.GameManager game, string PosInfo, Effect.EffectDefine singleEffect)
        {
            var 效果条件 = singleEffect.效果条件.ToUpper();
            switch (效果条件)
            {
                case "POSITION":
                    return PosInfo == singleEffect.AbliltyPosPicker.SelectedPos.ToString();
                default:
                    break;
            }
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <param name="效果选择条件"></param>
        /// <returns></returns>
        public static bool AbilityPickCondition(Client.GameManager game, string 效果选择条件)
        {
            throw new NotImplementedException();
        }
    }
}
