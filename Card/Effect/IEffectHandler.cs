using System;
using System.Collections.Generic;

namespace Engine.Effect
{
    public interface IEffectHandler
    {
        void DealHero(Client.GameManager game, EffectDefine singleEffect, Boolean MeOrYou);
        void DealMinion(Client.GameManager game, EffectDefine singleEffect, Boolean MeOrYou, int PosIndex);
    }
}
