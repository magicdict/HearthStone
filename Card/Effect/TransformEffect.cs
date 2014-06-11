using System;

namespace Card.Effect
{
    /// <summary>
    /// 变形法术
    /// </summary>
    public class TransformEffect : AtomicEffectDefine, IEffectHandler
    {
        void IEffectHandler.DealHero(Client.GameManager game, AtomicEffectDefine singleEffect, bool MeOrYou)
        {
            throw new NotImplementedException();
        }
        public String 变形目标卡牌编号;
        void IEffectHandler.DealMinion(Client.GameManager game, AtomicEffectDefine singleEffect, bool MeOrYou, int PosIndex)
        {
            var Summon = (Card.MinionCard)CardUtility.GetCardInfoBySN(变形目标卡牌编号);
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

        public new void GetField()
        {
            throw new NotImplementedException();
        }
    }
}
