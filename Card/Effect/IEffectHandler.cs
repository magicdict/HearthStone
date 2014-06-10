using System;
using System.Collections.Generic;

namespace Card.Effect
{
    public interface IEffectHandler
    {
        void DealHero(Client.GameManager game, AtomicEffectDefine singleEffect, Boolean MeOrYou);
        void DealMinion(Client.GameManager game, AtomicEffectDefine singleEffect, Boolean MeOrYou, int PosIndex);
    }
}
