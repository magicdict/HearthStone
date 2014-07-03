using Engine.Action;
using Engine.Control;
using Engine.Utility;
using System;

namespace Engine.Effect
{
    /// <summary>
    /// 变形法术
    /// </summary>
    public class TransformEffect : IAtomicEffect
    {
        public String 变形目标卡牌编号;
        /// <summary>
        /// 对英雄动作
        /// </summary>
        /// <param name="game"></param>
        /// <param name="PlayInfo"></param>
        /// <returns></returns>
        String IAtomicEffect.DealHero(ActionStatus game, Client.PublicInfo PlayInfo)
        {
            return String.Empty;
        }
        /// <summary>
        /// 对随从动作
        /// </summary>
        /// <param name="game"></param>
        /// <param name="Minion"></param>
        /// <returns></returns>
        String IAtomicEffect.DealMinion(ActionStatus game, Card.MinionCard Minion)
        {
            var Summon = (Engine.Card.MinionCard)CardUtility.GetCardInfoBySN(变形目标卡牌编号);
            //一定要初始化，不然的话，生命值是0；
            Summon.初始化();
            Summon.战场位置 = Minion.战场位置;
            //战场位置的继承
            Minion = Summon;
            return Server.ActionCode.strStatus + CardUtility.strSplitMark + Minion.战场位置.ToString() + CardUtility.strSplitMark + 变形目标卡牌编号;
        }
        /// <summary>
        /// 对方复原操作
        /// </summary>
        /// <param name="game"></param>
        /// <param name="actField"></param>
        void IAtomicEffect.ReRunEffect(ActionStatus game, string[] actField)
        {
            if (actField[1] == CardUtility.strYou)
            {
                //MyInfo
                game.AllRole.MyPublicInfo.BattleField.BattleMinions[int.Parse(actField[2]) - 1] = (Engine.Card.MinionCard)CardUtility.GetCardInfoBySN(actField[3]);
            }
            else
            {
                game.AllRole.YourPublicInfo.BattleField.BattleMinions[int.Parse(actField[2]) - 1] = (Engine.Card.MinionCard)CardUtility.GetCardInfoBySN(actField[3]);
            }
        }
        /// <summary>
        /// 获得效果信息
        /// </summary>
        /// <param name="InfoArray"></param>
        void IAtomicEffect.GetField(System.Collections.Generic.List<string> InfoArray)
        {
            变形目标卡牌编号 = InfoArray[0];
        }
    }
}
