using Engine.Utility;
using System;

namespace Engine.Effect
{
    /// <summary>
    /// 变形法术
    /// </summary>
    public class TransformEffect :  IAtomicEffect
    {
        public String 变形目标卡牌编号;
        void IAtomicEffect.DealHero(Client.GameManager game, Client.PublicInfo PlayInfo)
        {
            throw new NotImplementedException();
        }
        void IAtomicEffect.DealMinion(Client.GameManager game, Card.MinionCard Minion)
        {
            var Summon = (Engine.Card.MinionCard)CardUtility.GetCardInfoBySN(变形目标卡牌编号);
            //一定要初始化，不然的话，生命值是-1；
            Summon.Init();
            Summon.战场位置 = Minion.战场位置;
            Minion = Summon;
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
