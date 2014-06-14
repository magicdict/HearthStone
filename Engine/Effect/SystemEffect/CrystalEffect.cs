using Engine.Client;
using Engine.Server;
using Engine.Utility;
using System.Collections.Generic;
namespace Engine.Effect
{
    public class CrystalEffect : EffectDefine
    {
        /// <summary>
        /// 获得法力水晶
        /// </summary>
        public int 获得法力水晶 = 0;
        /// <summary>
        /// 获得空法力水晶
        /// </summary>
        public int 获得空法力水晶 = 0;
        /// <summary>
        /// 对法力水晶的法术实施
        /// </summary>
        /// <param name="role"></param>
        /// <param name="Ability"></param>
        public List<string> RunEffect(GameManager game, Utility.CardUtility.TargetSelectDirectEnum Direct)
        {
            List<string> Result = new List<string>();

            switch (Direct)
            {
                case CardUtility.TargetSelectDirectEnum.本方:
                    game.MyInfo.crystal.AddCurrentPoint(获得法力水晶);
                    game.MyInfo.crystal.AddFullPoint(获得空法力水晶);
                    break;
                case CardUtility.TargetSelectDirectEnum.对方:
                    game.YourInfo.crystal.AddCurrentPoint(获得法力水晶);
                    game.YourInfo.crystal.AddFullPoint(获得空法力水晶);
                    break;
                case CardUtility.TargetSelectDirectEnum.双方:
                    game.MyInfo.crystal.AddCurrentPoint(获得法力水晶);
                    game.MyInfo.crystal.AddFullPoint(获得空法力水晶);
                    game.YourInfo.crystal.AddCurrentPoint(获得法力水晶);
                    game.YourInfo.crystal.AddFullPoint(获得空法力水晶);
                    break;
                default:
                    break;
            }
            //Crystal#ME#4#4
            if (Direct == CardUtility.TargetSelectDirectEnum.本方)
            {
                Result.Add(ActionCode.strCrystal + CardUtility.strSplitMark + CardUtility.strMe + CardUtility.strSplitMark +
                    game.MyInfo.crystal.CurrentRemainPoint + CardUtility.strSplitMark + game.MyInfo.crystal.CurrentFullPoint);
            }
            else
            {
                Result.Add(ActionCode.strCrystal + CardUtility.strSplitMark + CardUtility.strYou + CardUtility.strSplitMark +
                    game.YourInfo.crystal.CurrentRemainPoint + CardUtility.strSplitMark + game.YourInfo.crystal.CurrentFullPoint);
            }
            return Result;
        }
    }
}
