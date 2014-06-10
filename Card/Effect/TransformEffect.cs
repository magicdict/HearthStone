using System;

namespace Card.Effect
{
    public class TransformEffect : AtomicEffectDefine, IEffectHandler
    {
        void IEffectHandler.DealHero(Client.GameManager game, AtomicEffectDefine singleEffect, bool MeOrYou)
        {
            throw new NotImplementedException();
        }

        void IEffectHandler.DealMinion(Client.GameManager game, AtomicEffectDefine singleEffect, bool MeOrYou, int PosIndex)
        {
            var Summon = (Card.MinionCard)CardUtility.GetCardInfoBySN(singleEffect.AdditionInfo);
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
