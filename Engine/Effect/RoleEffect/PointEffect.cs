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
    public class PointEffect : AtomicEffectDefine, IAtomicEffect
    {
        /// <summary>
        /// 攻击力
        /// </summary>
        public String 攻击力;
        /// <summary>
        /// 生命值
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
        public void RunPointEffect(MinionCard Minion)
        {
            int TurnCount = int.Parse(持续回合);
            if (TurnCount == CardUtility.Max)
            {
                Minion.攻击力 = ExpressHandler.PointProcess(Minion.攻击力, 攻击力);
                Minion.生命值上限 = ExpressHandler.PointProcess(Minion.生命值上限, 生命值);
                Minion.生命值 = ExpressHandler.PointProcess(Minion.生命值, 生命值);
            }
            else
            {
                //本回合攻击力翻倍的对应
                Minion.本回合攻击力加成 = ExpressHandler.PointProcess(Minion.攻击力, 攻击力) - Minion.攻击力;
                Minion.本回合生命力加成 = ExpressHandler.PointProcess(Minion.生命值上限, 生命值) - Minion.生命值上限;
            }
        }
        void IAtomicEffect.DealHero(Client.GameManager game, EffectDefine singleEffect, bool MeOrYou)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 对随从施法
        /// </summary>
        /// <param name="game"></param>
        /// <param name="singleEffect"></param>
        /// <param name="MeOrYou"></param>
        /// <param name="PosIndex"></param>
        void IAtomicEffect.DealMinion(Client.GameManager game, EffectDefine singleEffect, bool MeOrYou, int PosIndex)
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
        /// <summary>
        /// 获得效果信息
        /// </summary>
        /// <param name="InfoArray"></param>
        void IAtomicEffect.GetField(List<string> InfoArray)
        {
            攻击力 = InfoArray[0].Split("/".ToCharArray())[0];
            生命值 = InfoArray[0].Split("/".ToCharArray())[1];
            持续回合 = InfoArray[1];
        }
    }
}
