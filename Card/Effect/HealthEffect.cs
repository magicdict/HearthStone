using Card.Client;
using System;
using System.Collections.Generic;

namespace Card.Effect
{
    public class HealthEffect : IEffectHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <param name="singleEffect"></param>
        /// <param name="MeOrYou"></param>
        void IEffectHandler.DealHero(GameManager game, EffectDefine singleEffect, bool MeOrYou)
        {
            int HealthPoint = singleEffect.ActualEffectPoint;
            if (MeOrYou)
            {
                game.MyInfo.AfterBeHealth(HealthPoint);
            }
            else
            {
                game.YourInfo.AfterBeHealth(HealthPoint);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <param name="singleEffect"></param>
        /// <param name="MeOrYou"></param>
        /// <param name="PosIndex"></param>
        void IEffectHandler.DealMinion(GameManager game, EffectDefine singleEffect, bool MeOrYou, int PosIndex)
        {
            int HealthPoint = singleEffect.ActualEffectPoint;
            if (MeOrYou)
            {
                game.MyInfo.BattleField.BattleMinions[PosIndex].AfterBeHealth(HealthPoint);
            }
            else
            {
                game.YourInfo.BattleField.BattleMinions[PosIndex].AfterBeHealth(HealthPoint);
            }
        }
    }
}
