using Card.Server;
using System;
using System.Collections.Generic;

namespace Card.Effect
{
    public class AttackEffect : IEffectHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <param name="singleEffect"></param>
        /// <param name="MeOrYou"></param>
        void IEffectHandler.DealHero(Client.GameManager game, EffectDefine singleEffect, Boolean MeOrYou)
        {
            int AttackPoint = singleEffect.ActualEffectPoint;
            if (MeOrYou)
            {
                game.MySelf.RoleInfo.AfterBeAttack(AttackPoint);
            }
            else
            {
                game.YourInfo.AfterBeAttack(AttackPoint);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <param name="singleEffect"></param>
        /// <param name="MeOrYou"></param>
        /// <param name="PosIndex"></param>
        void IEffectHandler.DealMinion(Client.GameManager game, EffectDefine singleEffect, Boolean MeOrYou, int PosIndex)
        {
            int AttackPoint = singleEffect.ActualEffectPoint;
            if (MeOrYou)
            {
                game.MySelf.RoleInfo.BattleField.BattleMinions[PosIndex].AfterBeAttack(AttackPoint);
            }
            else
            {
                game.YourInfo.BattleField.BattleMinions[PosIndex].AfterBeAttack(AttackPoint);
            }
        }
    }
}
