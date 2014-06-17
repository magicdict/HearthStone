using Engine.Client;
using Engine.Server;
using Engine.Utility;
using System.Collections.Generic;

namespace Engine.Effect
{
    public class WeaponPointEffect
    {
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
        public List<string> RunEffect(GameManager game, Utility.CardUtility.TargetSelectDirectEnum Direct)
        {
            List<string> Result = new List<string>();
            if (Direct == CardUtility.TargetSelectDirectEnum.本方)
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
        /// <summary>
        /// 获得效果信息
        /// </summary>
        /// <param name="InfoArray"></param>
        public void GetField(List<string> InfoArray)
        {
            攻击力 = InfoArray[0];
            耐久度 = InfoArray[1];
        }
    }
}
