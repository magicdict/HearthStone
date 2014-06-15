using Engine.Card;
using Engine.Client;
using Engine.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Effect
{
    /// <summary>
    /// 增益效果
    /// </summary>
    public class PointEffect : EffectDefine, IEffectHandler
    {
        /// <summary>
        /// 
        /// </summary>
        public String 攻击力;
        /// <summary>
        /// 
        /// </summary>
        public String 生命值;

        /// <summary>
        /// 持续回合
        /// </summary>
        public String 持续回合;
        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="Minion"></param>
        /// <param name="Addition"></param>
        public void RunPointEffect(MinionCard Minion, int TurnCount = 999)
        {
            if (TurnCount == CardUtility.Max)
            {
                Minion.攻击力 = ExpressHandler.PointProcess(Minion.攻击力, 攻击力);
                Minion.生命值 = ExpressHandler.PointProcess(Minion.生命值, 生命值);
                Minion.生命值上限 = ExpressHandler.PointProcess(Minion.生命值上限, 生命值);
            }
            else
            {
                //本回合攻击力翻倍的对应
                Minion.本回合攻击力加成 = ExpressHandler.PointProcess(Minion.攻击力, 攻击力) - Minion.攻击力;
                Minion.本回合生命力加成 = ExpressHandler.PointProcess(Minion.生命值上限, 生命值) - Minion.生命值上限;
            }
        }
        void IEffectHandler.DealHero(Client.GameManager game, EffectDefine singleEffect, bool MeOrYou)
        {
            throw new NotImplementedException();
        }
        void IEffectHandler.DealMinion(Client.GameManager game, EffectDefine singleEffect, bool MeOrYou, int PosIndex)
        {
            ///防止加成的干扰！
            int TurnCount = ExpressHandler.GetEffectPoint(game, 持续回合);
            if (TurnCount == 1)
            {
                if (MeOrYou)
                {
                    RunPointEffect(game.MyInfo.BattleField.BattleMinions[PosIndex], 1);
                }
                else
                {
                    RunPointEffect(game.YourInfo.BattleField.BattleMinions[PosIndex], 1);
                }
            }
            else
            {
                if (MeOrYou)
                {
                    RunPointEffect(game.MyInfo.BattleField.BattleMinions[PosIndex]);
                }
                else
                {
                    RunPointEffect(game.YourInfo.BattleField.BattleMinions[PosIndex]);
                }
            }
        }

    }
}
