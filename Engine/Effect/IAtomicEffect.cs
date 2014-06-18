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
        /// <param name="PlayInfo"></param>
        /// <returns></returns>
        String DealHero(Client.GameManager game, PublicInfo PlayInfo);
        /// <summary>
        /// 对随从动作
        /// </summary>
        /// <param name="game"></param>
        /// <param name="Minion"></param>
        /// <returns></returns>
        String DealMinion(Client.GameManager game, MinionCard Minion);
        /// <summary>
        /// 获得效果信息
        /// </summary>
        /// <param name="InfoArray"></param>
        void GetField(List<String> InfoArray);
    }
}
