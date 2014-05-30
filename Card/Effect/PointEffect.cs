using System;
using System.Linq;

namespace Card.Effect
{
    public static class PointEffect
    {
        /// <summary>
        /// 
        /// </summary>
        public const String strIgnore = "X";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Minion"></param>
        /// <param name="Addition"></param>
        public static void RunPointEffect(MinionCard Minion, String Addition)
        {
            var AttackHealth = Addition.Split("/".ToArray());
            if (AttackHealth[0] != strIgnore)
            {
                if (AttackHealth[0].Length != 1)
                {
                    Minion.实际攻击力 += int.Parse(AttackHealth[0]);
                }
                else
                {
                    Minion.实际攻击力 = int.Parse(AttackHealth[0]);
                }
            }
            if (AttackHealth[1] != strIgnore)
            {
                if (AttackHealth[1].Length != 1)
                {
                    Minion.实际生命值 += int.Parse(AttackHealth[1]);
                }
                else
                {
                    Minion.实际生命值 = int.Parse(AttackHealth[1]);
                }
            }
        }
    }
}
