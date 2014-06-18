using Engine.Card;
using Engine.Client;
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
        /// <param name="本方对方标识"></param>
        void DealHero(Client.GameManager game, PublicInfo PlayInfo);
        /// <summary>
        /// 对随从动作
        /// </summary>
        /// <param name="game"></param>
        /// <param name="singleEffect"></param>
        /// <param name="本方对方标识"></param>
        /// <param name="PosIndex"></param>
        void DealMinion(Client.GameManager game, MinionCard Minion);
        /// <summary>
        /// 获得效果信息
        /// </summary>
        void GetField(List<String> InfoArray);
    }
}
