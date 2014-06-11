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
                    game.MyInfo.Weapon.实际攻击力 += int.Parse(攻击力);
                    game.MyInfo.Weapon.实际耐久度 += int.Parse(耐久度);
                    Result.Add(ActionCode.strWeaponPoint + CardUtility.strSplitMark + CardUtility.strMe + CardUtility.strSplitMark);
                }
            }
            else
            {
                if (game.YourInfo.Weapon != null)
                {
                    game.YourInfo.Weapon.实际攻击力 += int.Parse(攻击力);
                    game.YourInfo.Weapon.实际耐久度 += int.Parse(耐久度);
                    Result.Add(ActionCode.strWeaponPoint + CardUtility.strSplitMark + CardUtility.strYou + CardUtility.strSplitMark);
                }
            }
            return Result;
        }

        public new void GetField()
        {
            throw new System.NotImplementedException();
        }
    }
}
