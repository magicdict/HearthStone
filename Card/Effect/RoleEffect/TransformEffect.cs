using Engine.Utility;
using System;

namespace Engine.Effect
{
    /// <summary>
    /// 变形法术
    /// </summary>
    public class TransformEffect : EffectDefine, IEffectHandler
    {
        void IEffectHandler.DealHero(Client.GameManager game, EffectDefine singleEffect, bool MeOrYou)
        {
            throw new NotImplementedException();
        }
        public String 变形目标卡牌编号;
        void IEffectHandler.DealMinion(Client.GameManager game, EffectDefine singleEffect, bool MeOrYou, int PosIndex)
        {
            var Summon = (Engine.Card.MinionCard)CardUtility.GetCardInfoBySN(变形目标卡牌编号);
            //一定要初始化，不然的话，生命值是-1；
            Summon.Init();
            if (MeOrYou)
            {
                game.MyInfo.BattleField.BattleMinions[PosIndex] = Summon;
            }
            else
            {
                game.YourInfo.BattleField.BattleMinions[PosIndex] = Summon;
            }
        }
    }
}
