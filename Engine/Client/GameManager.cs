using Engine.Card;
using Engine.Server;
using Engine.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Client
{
    /// <summary>
    /// 游戏管理
    /// </summary>
    public class GameManager
    {
        /// <summary>
        /// 游戏编号
        /// </summary>
        public int GameId;
        /// <summary>
        /// 主机私有情报
        /// </summary>
        public PrivateInfo HostSelfInfo = new PrivateInfo();
        /// <summary>
        /// 主机情报
        /// </summary>
        public PublicInfo HostInfo = new PublicInfo();
        /// <summary>
        /// 从属情报
        /// </summary>
        public PublicInfo GuestInfo = new PublicInfo();
        /// <summary>
        /// 从属私有情报
        /// </summary>
        public PrivateInfo GuestSelfInfo = new PrivateInfo();
        /// <summary>
        /// 获得目标对象
        /// </summary>
        public Engine.Utility.CardUtility.deleteGetTargetPosition GetSelectTarget;
        /// <summary>
        /// 抉择卡牌
        /// </summary>
        public Engine.Utility.CardUtility.delegatePickEffect PickEffect;
        /// <summary>
        /// 全局随机种子
        /// </summary>
        public static int RandomSeed = 1;
        /// <summary>
        /// 事件处理组件
        /// </summary>
        public EventHandler 事件处理组件 = new EventHandler();
        /// <summary>
        /// 游戏类型
        /// </summary>
        public SystemManager.GameType 游戏类型 = SystemManager.GameType.客户端服务器版;
        /// <summary>
        /// 初始化
        /// </summary>
        public void InitPlayInfo()
        {
            //暂时从外部注入
            HostInfo.crystal.CurrentFullPoint = 0;
            HostInfo.crystal.CurrentRemainPoint = 0;
            GuestInfo.crystal.CurrentFullPoint = 0;
            GuestInfo.crystal.CurrentRemainPoint = 0;
            HostInfo.战场位置 = new CardUtility.TargetPosition() { 本方对方标识 = true, Postion = BattleFieldInfo.HeroPos };
            GuestInfo.战场位置 = new CardUtility.TargetPosition() { 本方对方标识 = false, Postion = BattleFieldInfo.HeroPos };
            HostInfo.BattleField.本方对方标识 = true;
            GuestInfo.BattleField.本方对方标识 = false;
            //英雄技能：奥术飞弹
            HostInfo.HeroAbility = (Engine.Card.AbilityCard)Engine.Utility.CardUtility.GetCardInfoBySN("A000056");
            GuestInfo.HeroAbility = (Engine.Card.AbilityCard)Engine.Utility.CardUtility.GetCardInfoBySN("A000056");
        }
        public void InitHandCard(Boolean IsFirst)
        {
            //手牌设定
            var HandCard = Engine.Client.ClientRequest.DrawCard(GameId.ToString(GameServer.GameIdFormat), IsFirst,
                           IsFirst ? PublicInfo.BasicHandCardCount : (PublicInfo.BasicHandCardCount + 1));
            if (!IsFirst) HandCard.Add(Engine.Card.AbilityCard.SN幸运币);
            foreach (var card in HandCard)
            {
                HostSelfInfo.handCards.Add(CardUtility.GetCardInfoBySN(card));
            }
            HostInfo.HandCardCount = HandCard.Count;
            if (IsFirst)
            {
                HostInfo.RemainCardDeckCount = Engine.Client.CardDeck.MaxCards - 3;
                GuestInfo.RemainCardDeckCount = Engine.Client.CardDeck.MaxCards - 4;
            }
            else
            {
                HostInfo.RemainCardDeckCount = Engine.Client.CardDeck.MaxCards - 4;
                GuestInfo.RemainCardDeckCount = Engine.Client.CardDeck.MaxCards - 3;
            }
        }
        /// <summary>
        /// 新的回合
        /// </summary>
        public void TurnStart(PublicInfo PlayInfo)
        {
            //过载的清算
            if (PlayInfo.OverloadPoint != 0)
            {
                PlayInfo.crystal.ReduceCurrentPoint(PlayInfo.OverloadPoint);
                PlayInfo.OverloadPoint = 0;
            }
            //连击的重置
            HostInfo.连击状态 = false;
            //魔法水晶的增加
            HostInfo.crystal.NewTurn();
            PlayInfo.RemainAttactTimes = 1;
            PlayInfo.IsUsedHeroAbility = false;
            PlayInfo.BattleField.FreezeStatus();
            //重置攻击次数,必须放在状态变化之后！
            //原因是剩余攻击回数和状态有关！
            foreach (var minion in PlayInfo.BattleField.BattleMinions)
            {
                if (minion != null) minion.ResetAttackTimes();
            }
        }
        /// <summary>
        /// 对手回合结束的清场
        /// </summary>
        public List<String> TurnEnd(PublicInfo PlayInfo)
        {
            List<String> ActionLst = new List<string>();
            //对手回合加成属性的去除
            int ExistMinionCount = PlayInfo.BattleField.MinionCount;
            for (int i = 0; i < ExistMinionCount; i++)
            {
                if (PlayInfo.BattleField.BattleMinions[i] != null)
                {
                    PlayInfo.BattleField.BattleMinions[i].本回合生命力加成 = 0;
                    PlayInfo.BattleField.BattleMinions[i].本回合攻击力加成 = 0;
                    if (PlayInfo.BattleField.BattleMinions[i].特殊效果 == MinionCard.特殊效果列表.回合结束死亡)
                    {
                        PlayInfo.BattleField.BattleMinions[i] = null;
                    }
                }
            }
            PlayInfo.BattleField.ClearDead(this, false);
            ActionLst.AddRange(Settle());
            ActionLst.AddRange(事件处理组件.事件处理(this));
            return ActionLst;
        }
        /// <summary>
        /// 清算(核心方法)
        /// </summary>
        /// <returns></returns>
        public List<String> Settle()
        {
            //每次原子操作后进行一次清算
            //将亡语效果也发送给对方
            List<String> actionlst = new List<string>();
            //1.检查需要移除的对象
            var MyDeadMinion = HostInfo.BattleField.ClearDead(this, true);
            var YourDeadMinion = GuestInfo.BattleField.ClearDead(this, false);
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
                actionlst.AddRange(minion.发动亡语(this, false));
            }
            foreach (var minion in YourDeadMinion)
            {
                //亡语的时候，对方需要倒置方向
                //例如，亡语为 本方召唤一个随从，敌人亡语，变为敌方召唤一个随从
                actionlst.AddRange(minion.发动亡语(this, true));
            }
            return actionlst;
        }
    }
}
