using Engine.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Control
{
    public static class GameManager
    {
        /// <summary>
        /// 获得目标对象
        /// </summary>
        public static Engine.Utility.CardUtility.deleteGetTargetPosition GetSelectTarget;
        /// <summary>
        /// 抉择卡牌
        /// </summary>
        public static Engine.Utility.CardUtility.delegatePickEffect PickEffect;
        /// <summary>
        /// 全局随机种子
        /// </summary>
        public static int RandomSeed = 1;
        /// <summary>
        /// 清算(核心方法)
        /// </summary>
        /// <returns></returns>
        public static List<String> Settle(PublicInfo HostInfo,PublicInfo GuestInfo)
        {
            //每次原子操作后进行一次清算
            //将亡语效果也发送给对方
            List<String> actionlst = new List<string>();
            //1.检查需要移除的对象
            var MyDeadMinion = HostInfo.BattleField.ClearDead(gameStatus, true);
            var YourDeadMinion = GuestInfo.BattleField.ClearDead(gameStatus, false);
            //2.重新计算Buff
            HostInfo.BattleField.ResetBuff();
            GuestInfo.BattleField.ResetBuff();
            //3.武器的移除
            if (HostInfo.Weapon != null && HostInfo.Weapon.耐久度 == 0) HostInfo.Weapon = null;
            if (GuestInfo.Weapon != null && GuestInfo.Weapon.耐久度 == 0) GuestInfo.Weapon = null;
            //发送结算同步信息
            actionlst.Add(Server.ActionCode.strSettle);
            foreach (var minion in MyDeadMinion)
            {
                //亡语的时候，本方无需倒置方向
                actionlst.AddRange(minion.发动亡语(gameStatus, false));
            }
            foreach (var minion in YourDeadMinion)
            {
                //亡语的时候，对方需要倒置方向
                //例如，亡语为 本方召唤一个随从，敌人亡语，变为敌方召唤一个随从
                actionlst.AddRange(minion.发动亡语(gameStatus, true));
            }
            return actionlst;
        }
    }
}
