using Engine.Card;
using Engine.Effect;
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
        /// 游戏玩家名称
        /// </summary>
        public String PlayerNickName = "NickName";
        /// <summary>
        /// 是否主机
        /// </summary>
        public Boolean IsHost;
        /// <summary>
        /// 是否为先手
        /// </summary>
        public Boolean IsFirst;
        /// <summary>
        /// 游戏编号
        /// </summary>
        public int GameId;
        /// <summary>
        /// 是否为我的回合
        /// </summary>
        public Boolean IsMyTurn;
        /// <summary>
        /// 获得目标对象
        /// </summary>
        public Engine.Utility.CardUtility.deleteGetTargetPosition GetSelectTarget;
        /// <summary>
        /// 抉择卡牌
        /// </summary>
        public Engine.Utility.CardUtility.delegatePickEffect PickEffect;
        /// <summary>
        /// 上回合过载
        /// </summary>
        public int OverloadPoint = 0;
        /// <summary>
        /// 全局随机种子
        /// </summary>
        public static int RandomSeed = 1;
        /// <summary>
        /// 
        /// </summary>
        public EventHandler 事件处理组件 = new EventHandler();
        /// <summary>
        /// 游戏信息
        /// </summary>
        /// <returns></returns>
        public string GetGameInfo()
        {
            StringBuilder Status = new StringBuilder();
            Status.AppendLine("==============");
            Status.AppendLine("System：");
            Status.AppendLine("GameId：" + GameId);
            Status.AppendLine("PlayerNickName：" + PlayerNickName);
            Status.AppendLine("IsHost：" + IsHost);
            Status.AppendLine("IsFirst：" + IsFirst);
            Status.AppendLine("==============");
            return Status.ToString();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            //手牌设定
            var HandCard = Engine.Client.ClientRequest.DrawCard(GameId.ToString(GameServer.GameIdFormat), IsFirst,
                           IsFirst ? PublicInfo.BasicHandCardCount : (PublicInfo.BasicHandCardCount + 1));
            MyInfo.crystal.CurrentFullPoint = 0;
            MyInfo.crystal.CurrentRemainPoint = 0;
            YourInfo.crystal.CurrentFullPoint = 0;
            YourInfo.crystal.CurrentRemainPoint = 0;
            MyInfo.战场位置 = new CardUtility.TargetPosition() { 本方对方标识 =true, Postion = BattleFieldInfo.HeroPos };
            YourInfo.战场位置 = new CardUtility.TargetPosition() { 本方对方标识 = false, Postion = BattleFieldInfo.HeroPos };
            MyInfo.BattleField.本方对方标识 = true;
            YourInfo.BattleField.本方对方标识 = false;
            //DEBUG START
            MyInfo.crystal.CurrentFullPoint = 5;
            MyInfo.crystal.CurrentRemainPoint = 5;
            YourInfo.crystal.CurrentFullPoint = 5;
            YourInfo.crystal.CurrentRemainPoint = 5;
            HandCard.Add("M000059");
            //DEBUG END
            //英雄技能：奥术飞弹
            MyInfo.HeroAbility = (Engine.Card.AbilityCard)Engine.Utility.CardUtility.GetCardInfoBySN("A000056");
            YourInfo.HeroAbility = (Engine.Card.AbilityCard)Engine.Utility.CardUtility.GetCardInfoBySN("A000056");
            if (!IsFirst) HandCard.Add(Engine.Card.AbilityCard.SN幸运币);
            foreach (var card in HandCard)
            {
                MySelfInfo.handCards.Add(CardUtility.GetCardInfoBySN(card));
            }
            MyInfo.HandCardCount = HandCard.Count;
            if (IsFirst)
            {
                MyInfo.RemainCardDeckCount = Engine.Client.CardDeck.MaxCards - 3;
                YourInfo.RemainCardDeckCount = Engine.Client.CardDeck.MaxCards - 4;
            }
            else
            {
                MyInfo.RemainCardDeckCount = Engine.Client.CardDeck.MaxCards - 4;
                YourInfo.RemainCardDeckCount = Engine.Client.CardDeck.MaxCards - 3;
            }
        }
        /// <summary>
        /// 本方私有情报
        /// </summary>
        public PrivateInfo MySelfInfo = new PrivateInfo();
        /// <summary>
        /// 本方情报
        /// </summary>
        public PublicInfo MyInfo = new PublicInfo();
        /// <summary>
        /// 对方情报
        /// </summary>
        public PublicInfo YourInfo = new PublicInfo();
        /// <summary>
        /// 对方私有情报（单机版有效）
        /// </summary>
        public PrivateInfo YourSelfInfo = new PrivateInfo();
        /// <summary>
        /// 检查是否可以使用
        /// </summary>
        /// <returns></returns>
        public String CheckCondition(Engine.Card.CardBasicInfo card)
        {
            //剩余的法力是否足够实际召唤的法力
            String Message = String.Empty;
            if (card.过载 > 0 && OverloadPoint > 0)
            {
                Message = "已经使用过载";
                return Message;
            }
            //if (card.CardType == CardBasicInfo.CardTypeEnum.法术)
            //{
            //    if (((Card.AbilityCard)card).CheckCondition(this) == false)
            //    {
            //        Message = "没有法术使用对象";
            //        return Message;
            //    }
            //}
            if (card.CardType == CardBasicInfo.CardTypeEnum.随从)
            {
                if (MyInfo.BattleField.MinionCount == Engine.Client.BattleFieldInfo.MaxMinionCount)
                {
                    Message = "随从已经满员";
                    return Message;
                }
            }
            if (MyInfo.crystal.CurrentRemainPoint < card.使用成本)
            {
                Message = "法力水晶不足";
            }
            return Message;
        }
        /// <summary>
        /// 新的回合
        /// </summary>
        public void TurnStart()
        {
            if (IsMyTurn)
            {
                //对手回合加成属性的去除
                int ExistMinionCount = YourInfo.BattleField.MinionCount;
                for (int i = 0; i < ExistMinionCount; i++)
                {
                    if (YourInfo.BattleField.BattleMinions[i] != null)
                    {
                        YourInfo.BattleField.BattleMinions[i].本回合生命力加成 = 0;
                        YourInfo.BattleField.BattleMinions[i].本回合攻击力加成 = 0;
                        if (YourInfo.BattleField.BattleMinions[i].特殊效果 == MinionCard.特殊效果列表.回合结束死亡)
                        {
                            YourInfo.BattleField.BattleMinions[i] = null;
                        }
                    }
                }
                YourInfo.BattleField.ClearDead(this, false);

                //魔法水晶的增加
                MyInfo.crystal.NewTurn();
                //过载的清算
                if (OverloadPoint != 0)
                {
                    MyInfo.crystal.ReduceCurrentPoint(OverloadPoint);
                    OverloadPoint = 0;
                }
                //连击的重置
                MyInfo.连击状态 = false;
                //手牌
                var NewCardList = Engine.Client.ClientRequest.DrawCard(GameId.ToString(GameServer.GameIdFormat), IsFirst, 1);
                foreach (var card in NewCardList)
                {
                    if (MySelfInfo.handCards.Count < PublicInfo.MaxHandCardCount) MySelfInfo.handCards.Add(CardUtility.GetCardInfoBySN(card));
                }
                MyInfo.HandCardCount++;
                MyInfo.RemainCardDeckCount--;
                MyInfo.RemainAttactTimes = 1;
                MyInfo.IsUsedHeroAbility = false;
                foreach (var minion in MyInfo.BattleField.BattleMinions)
                {
                    if (minion != null)
                    {
                        switch (minion.冰冻状态)
                        {
                            case CardUtility.EffectTurn.效果命中:
                                //如果上回合被命中的，这回合就是作用中
                                minion.冰冻状态 = CardUtility.EffectTurn.效果作用;
                                break;
                            case CardUtility.EffectTurn.效果作用:
                                //如果上回合作用中的，这回合就是解除
                                minion.冰冻状态 = CardUtility.EffectTurn.无效果;
                                break;
                        }
                    }
                }
                //重置攻击次数,必须放在状态变化之后！
                //原因是剩余攻击回数和状态有关！
                foreach (var minion in MyInfo.BattleField.BattleMinions)
                {
                    if (minion != null) minion.ResetAttackTimes();
                }
                //手牌消耗的计算
                MySelfInfo.ResetHandCardCost(this);
            }
            else
            {
                YourInfo.crystal.NewTurn();
                if (YourInfo.HandCardCount < PublicInfo.MaxHandCardCount) YourInfo.HandCardCount++;
                YourInfo.RemainCardDeckCount--;
                YourInfo.RemainAttactTimes = 1;
                YourInfo.IsUsedHeroAbility = false;
                //重置攻击次数
                foreach (var minion in YourInfo.BattleField.BattleMinions)
                {
                    if (minion != null) minion.ResetAttackTimes();
                }
                //如果对手有可以解除冰冻的，解除冰冻
                foreach (var minion in YourInfo.BattleField.BattleMinions)
                {
                    if (minion != null)
                    {
                        switch (minion.冰冻状态)
                        {
                            case CardUtility.EffectTurn.效果命中:
                                //如果上回合被命中的，这回合就是作用中
                                minion.冰冻状态 = CardUtility.EffectTurn.效果作用;
                                break;
                            case CardUtility.EffectTurn.效果作用:
                                //如果上回合作用中的，这回合就是解除
                                minion.冰冻状态 = CardUtility.EffectTurn.无效果;
                                break;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 结束回合
        /// </summary>
        public List<String> TurnEnd()
        {
            //调用这个方法的时候，IsMyTurn肯定是True
            //回合结束效果
            List<String> ActionLst = new List<string>();
            int ExistMinionCount = MyInfo.BattleField.MinionCount;
            //不能直接使用 MinionCount ，这个会在下面的逻辑里面变化的！
            for (int i = 0; i < ExistMinionCount; i++)
            {
                if (MyInfo.BattleField.BattleMinions[i] != null)
                {
                    ActionLst.AddRange(MyInfo.BattleField.BattleMinions[i].回合结束(this));
                    if (MyInfo.BattleField.BattleMinions[i].特殊效果 == MinionCard.特殊效果列表.回合结束死亡)
                    {
                        事件处理组件.事件池.Add(new CardUtility.全局事件()
                        {
                            触发事件类型 = CardUtility.事件类型列表.死亡,
                            触发位置 = MyInfo.BattleField.BattleMinions[i].战场位置
                        });
                        MyInfo.BattleField.BattleMinions[i] = null;
                    }
                }
            }
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
            var MyDeadMinion = MyInfo.BattleField.ClearDead(this, true);
            foreach (var minion in MyDeadMinion)
            {
                //亡语的时候，需要倒置方向
                actionlst.AddRange(minion.发动亡语(this, false));
            }
            var YourDeadMinion = YourInfo.BattleField.ClearDead(this, false);
            foreach (var minion in YourDeadMinion)
            {
                //亡语的时候，需要倒置方向
                actionlst.AddRange(minion.发动亡语(this, true));
            }
            //2.重新计算Buff
            MyInfo.BattleField.ResetBuff();
            YourInfo.BattleField.ResetBuff();
            //3.武器的移除
            if (MyInfo.Weapon != null && MyInfo.Weapon.耐久度 == 0) MyInfo.Weapon = null;
            if (YourInfo.Weapon != null && YourInfo.Weapon.耐久度 == 0) YourInfo.Weapon = null;
            return actionlst;
        }
        /// <summary>
        /// 去掉使用过的手牌
        /// </summary>
        /// <param name="CardSn"></param>
        public void RemoveUsedCard(String CardSn)
        {
            Engine.Card.CardBasicInfo removeCard = new CardBasicInfo();
            foreach (var Seekcard in MySelfInfo.handCards)
            {
                if (Seekcard.序列号 == CardSn)
                {
                    removeCard = Seekcard;
                }
            }
            MySelfInfo.handCards.Remove(removeCard);
            MyInfo.HandCardCount = MySelfInfo.handCards.Count;
        }
    }
}
