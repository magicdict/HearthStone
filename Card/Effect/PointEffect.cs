using System;
using System.Collections.Generic;
using System.Linq;

namespace Card.Effect
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
                Minion.实际攻击力 = CardUtility.PointProcess(Minion.实际攻击力, 攻击力);
                Minion.实际生命值 = CardUtility.PointProcess(Minion.实际生命值, 生命值);
                Minion.实际生命值上限 = CardUtility.PointProcess(Minion.实际生命值上限, 生命值);
            }
            else
            {
                //本回合攻击力翻倍的对应
                Minion.本回合攻击力加成 = CardUtility.PointProcess(Minion.实际攻击力, 攻击力) - Minion.实际攻击力;
                Minion.本回合生命力加成 = CardUtility.PointProcess(Minion.实际生命值上限, 生命值) - Minion.实际生命值上限;
            }
        }
        void IEffectHandler.DealHero(Client.GameManager game, EffectDefine singleEffect, bool MeOrYou)
        {
            throw new NotImplementedException();
        }
        void IEffectHandler.DealMinion(Client.GameManager game, EffectDefine singleEffect, bool MeOrYou, int PosIndex)
        {
            ///防止加成的干扰！
            int TurnCount = Effecthandler.GetEffectPoint(game, 持续回合);
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
