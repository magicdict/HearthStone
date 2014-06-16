using System;
using System.Collections.Generic;

namespace Engine.Effect
{
    public interface IAtomicEffect
    {
        /// <summary>
        /// 对英雄动作
        /// </summary>
        /// <param name="game"></param>
        /// <param name="singleEffect"></param>
        /// <param name="MeOrYou"></param>
        void DealHero(Client.GameManager game, EffectDefine singleEffect, Boolean MeOrYou);
        /// <summary>
        /// 对随从动作
        /// </summary>
        /// <param name="game"></param>
        /// <param name="singleEffect"></param>
        /// <param name="MeOrYou"></param>
        /// <param name="PosIndex"></param>
        void DealMinion(Client.GameManager game, EffectDefine singleEffect, Boolean MeOrYou, int PosIndex);
        /// <summary>
        /// 获得效果信息
        /// </summary>
        void GetField(List<String> InfoArray);
    }
}
