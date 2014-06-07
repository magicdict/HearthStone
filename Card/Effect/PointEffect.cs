using System;
using System.Collections.Generic;
using System.Linq;

namespace Card.Effect
{
    public class PointEffect : IEffectHandler
    {
        /// <summary>
        /// 忽略
        /// </summary>
        public const String strIgnore = "X";
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
                Minion.实际攻击力 = PointProcess(Minion.实际攻击力, PointInfo[0]);
                Minion.实际生命值 = PointProcess(Minion.实际生命值, PointInfo[1]);
                Minion.实际生命值上限 = PointProcess(Minion.实际生命值上限, PointInfo[1]);
            }
            else
            {
                //本回合攻击力翻倍的对应
                Minion.本回合攻击力加成 = PointProcess(Minion.实际攻击力, PointInfo[0]) - Minion.实际攻击力;
                Minion.本回合生命力加成 = PointProcess(Minion.实际生命值上限, PointInfo[1]) - Minion.实际生命值上限;
            }
        }
        /// <summary>
        /// PointProcess
        /// </summary>
        /// <param name="oldPoint"></param>
        /// <param name="ModifyPoint"></param>
        /// <returns></returns>
        public static int PointProcess(int oldPoint, String ModifyPoint)
        {
            int newPoint = oldPoint;
            if (ModifyPoint != strIgnore)
            {
                if (ModifyPoint.Length != 1)
                {
                    switch (ModifyPoint.Substring(0, 1))
                    {
                        case "+":
                        case "-":
                            newPoint += int.Parse(ModifyPoint);
                            break;
                        case "*":
                            newPoint *= int.Parse(ModifyPoint.Substring(1, 1));
                            break;
                        default:
                            break;
                    }
                }
            }
            return newPoint;
        }

        void IEffectHandler.DealHero(Client.GameManager game, EffectDefine singleEffect, bool MeOrYou)
        {
            throw new NotImplementedException();
        }
        void IEffectHandler.DealMinion(Client.GameManager game, EffectDefine singleEffect, bool MeOrYou, int PosIndex)
        {
            ///防止加成的干扰！
            int TurnCount = singleEffect.StandardEffectPoint;
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
