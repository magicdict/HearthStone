using System;

namespace Card.Effect
{
    public interface IEffectHandler
    {
        void DealHero(Client.GameManager game, EffectDefine singleEffect, Boolean MeOrYou);
        void DealMinion(Client.GameManager game, EffectDefine singleEffect, Boolean MeOrYou, int PosIndex);
    }
}
