using System;
using System.Collections.Generic;
using System.Linq;

namespace Card.Effect
{
    /// <summary>
    /// 增益效果
    /// </summary>
    public class PointEffect : AtomicEffectDefine, IEffectHandler
    {
        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="Minion"></param>
        /// <param name="Addition"></param>
        public static void RunPointEffect(MinionCard Minion, String Addition, int TurnCount = 999)
        {
            var PointInfo = Addition.Split("/".ToArray());
            if (TurnCount == 999)
            {
                Minion.实际攻击力 = CardUtility.PointProcess(Minion.实际攻击力, PointInfo[0]);
                Minion.实际生命值 = CardUtility.PointProcess(Minion.实际生命值, PointInfo[1]);
                Minion.实际生命值上限 = CardUtility.PointProcess(Minion.实际生命值上限, PointInfo[1]);
            }
            else
            {
                //本回合攻击力翻倍的对应
                Minion.本回合攻击力加成 = CardUtility.PointProcess(Minion.实际攻击力, PointInfo[0]) - Minion.实际攻击力;
                Minion.本回合生命力加成 = CardUtility.PointProcess(Minion.实际生命值上限, PointInfo[1]) - Minion.实际生命值上限;
            }
        }
        void IEffectHandler.DealHero(Client.GameManager game, AtomicEffectDefine singleEffect, bool MeOrYou)
        {
            throw new NotImplementedException();
        }
        void IEffectHandler.DealMinion(Client.GameManager game, AtomicEffectDefine singleEffect, bool MeOrYou, int PosIndex)
        {
            ///防止加成的干扰！
            int TurnCount = Effecthandler.GetEffectPoint(game,singleEffect.ActualEffectPoint); 
            if (TurnCount == 1)
            {
                if (MeOrYou)
                {
                    RunPointEffect(game.MyInfo.BattleField.BattleMinions[PosIndex], singleEffect.AdditionInfo, 1);
                }
                else
                {
                    RunPointEffect(game.YourInfo.BattleField.BattleMinions[PosIndex], singleEffect.AdditionInfo, 1);
                }
            }
            else
            {
                if (MeOrYou)
                {
                    RunPointEffect(game.MyInfo.BattleField.BattleMinions[PosIndex], singleEffect.AdditionInfo);
                }
                else
                {
                    RunPointEffect(game.YourInfo.BattleField.BattleMinions[PosIndex], singleEffect.AdditionInfo);
                }
            }
        }
    }
}
