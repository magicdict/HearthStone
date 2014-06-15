using Engine.Client;
using Engine.Server;
using Engine.Utility;
using System.Collections.Generic;

namespace Engine.Effect
{
    public class WeaponPointEffect : EffectDefine
    {
        /// <summary>
        /// 法术方向
        /// </summary>
        public CardUtility.TargetSelectDirectEnum 法术方向 = CardUtility.TargetSelectDirectEnum.双方;
        /// <summary>
        /// 攻击力
        /// </summary>
        public string 攻击力;
        /// <summary>
        /// 耐久度
        /// </summary>
        public string 耐久度;
        /// <summary>
        /// 对武器增益的法术实施
        /// </summary>
        /// <param name="role"></param>
        /// <param name="Ability"></param>
        public List<string> RunEffect(GameManager game)
        {
            List<string> Result = new List<string>();
            if (法术方向 == CardUtility.TargetSelectDirectEnum.本方)
            {
                if (game.MyInfo.Weapon != null)
                {
                    game.MyInfo.Weapon.攻击力 += int.Parse(攻击力);
                    game.MyInfo.Weapon.耐久度 += int.Parse(耐久度);
                    Result.Add(ActionCode.strWeaponPoint + CardUtility.strSplitMark + CardUtility.strMe + CardUtility.strSplitMark);
                }
            }
            else
            {
                if (game.YourInfo.Weapon != null)
                {
                    game.YourInfo.Weapon.攻击力 += int.Parse(攻击力);
                    game.YourInfo.Weapon.耐久度 += int.Parse(耐久度);
                    Result.Add(ActionCode.strWeaponPoint + CardUtility.strSplitMark + CardUtility.strYou + CardUtility.strSplitMark);
                }
            }
            return Result;
        }
    }
}
