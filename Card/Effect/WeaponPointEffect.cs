using Card.Client;
using Card.Server;
using System.Collections.Generic;

namespace Card.Effect
{
    public class WeaponPointEffect : AtomicEffectDefine
    {
        /// <summary>
        /// 法术方向
        /// </summary>
        public CardUtility.TargetSelectDirectEnum 法术方向 = CardUtility.TargetSelectDirectEnum.双方;
        /// <summary>
        /// 对法力水晶的法术实施
        /// </summary>
        /// <param name="role"></param>
        /// <param name="Ability"></param>
        public List<string> RunEffect(GameManager game)
        {
            List<string> Result = new List<string>();
            string[] Op = AdditionInfo.Split("/".ToCharArray());
            if (法术方向 == CardUtility.TargetSelectDirectEnum.本方)
            {
                if (game.MyInfo.Weapon != null)
                {
                    game.MyInfo.Weapon.实际攻击力 += int.Parse(Op[0]);
                    game.MyInfo.Weapon.实际耐久度 += int.Parse(Op[1]);
                    Result.Add(ActionCode.strWeaponPoint + CardUtility.strSplitMark + CardUtility.strMe + CardUtility.strSplitMark + AdditionInfo);
                }
            }
            else
            {
                if (game.YourInfo.Weapon != null)
                {
                    game.YourInfo.Weapon.实际攻击力 += int.Parse(Op[0]);
                    game.YourInfo.Weapon.实际耐久度 += int.Parse(Op[1]);
                    Result.Add(ActionCode.strWeaponPoint + CardUtility.strSplitMark + CardUtility.strYou + CardUtility.strSplitMark + AdditionInfo);
                }
            }
            return Result;
        }
    }
}
