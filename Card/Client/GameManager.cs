using Engine.Card;
using Engine.Effect;
using Engine.Effect.Server;
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
            //DEBUG START
            MyInfo.crystal.CurrentFullPoint = 5;
            MyInfo.crystal.CurrentRemainPoint = 5;
            YourInfo.crystal.CurrentFullPoint = 5;
            YourInfo.crystal.CurrentRemainPoint = 5;
            HandCard.Add("A000106");
            HandCard.Add("W000002");
            //DEBUG END
            //英雄技能：奥术飞弹
            MyInfo.HeroAbility = (Engine.Card.AbilityCard)Engine.Utility.CardUtility.GetCardInfoBySN("A200001");
            YourInfo.HeroAbility = (Engine.Card.AbilityCard)Engine.Utility.CardUtility.GetCardInfoBySN("A200001");
            if (!IsFirst) HandCard.Add(Engine.Utility.CardUtility.SN幸运币);
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
            if (card.Overload > 0 && OverloadPoint > 0)
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
            if (MyInfo.crystal.CurrentRemainPoint < card.ActualCostPoint)
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
                        MyInfo.BattleField.BattleMinions[i] = null;
                        事件池.Add(new CardUtility.全局事件()
                        {
                            事件类型 = CardUtility.事件类型列表.死亡,
                            触发方向 = CardUtility.TargetSelectDirectEnum.本方,
                            触发位置 = i + 1
                        });
                    }
                }
            }
            ActionLst.AddRange(Settle());
            ActionLst.AddRange(事件处理());
            return ActionLst;
        }
        /// <summary>
        /// 奥秘计算
        /// </summary>
        /// <param name="actionlst"></param>
        /// <returns></returns>
        public List<String> 奥秘计算(List<String> actionlst)
        {
            List<String> Result = new List<string>();
            //奥秘计算 START
            //本方（Fight也需要）
            if (MySelfInfo.奥秘列表.Count != 0)
            {
                //本方的行动触发本方奥秘的检查
                for (int i = 0; i < actionlst.Count; i++)
                {
                    foreach (var secret in MySelfInfo.奥秘列表)
                    {
                        if ((!secret.IsHitted) && Engine.Card.SecretCard.IsSecretHit(secret.SN, actionlst[i], true))
                        {
                            //奥秘执行
                            Result.AddRange(Engine.Card.SecretCard.RunSecretHit(secret.SN, actionlst[i], true, this));
                            secret.IsHitted = true;
                        }
                    }
                }
                //移除已经触发的奥秘
                MySelfInfo.清除命中奥秘();
            }
            //对方（Fight也需要）
            if (YourInfo.SecretCount != 0)
            {
                var HitCard = Engine.Client.ClientRequest.IsSecretHit(GameId.ToString(GameServer.GameIdFormat), IsFirst, actionlst);
                if (!String.IsNullOrEmpty(HitCard))
                {
                    var HitCardList = HitCard.Split(Engine.Utility.CardUtility.strSplitArrayMark.ToCharArray());
                    foreach (var hitCard in HitCardList)
                    {
                        Result.AddRange(Engine.Card.SecretCard.RunSecretHit(hitCard.Split(Engine.Utility.CardUtility.strSplitDiffMark.ToCharArray())[0],
                                                                     hitCard.Split(Engine.Utility.CardUtility.strSplitDiffMark.ToCharArray())[1], false, this));
                        YourInfo.SecretCount--;
                    }
                }
            }
            //奥秘计算 END
            Result.AddRange(Settle());
            return Result;
        }
        /// <summary>
        /// 战斗
        /// </summary>
        /// <param name="攻击方Pos">本方</param>
        /// <param name="被攻击方Pos">对方</param>
        /// <param name="被动攻击">被动攻击</param>
        /// <returns></returns>
        public List<String> Fight(int 攻击方Pos, int 被攻击方Pos, Boolean 被动攻击 = false)
        {
            List<String> Result = new List<string>();
            //主动攻击方的状态变化
            if (!被动攻击)
            {
                //攻击次数
                if (攻击方Pos == BattleFieldInfo.HeroPos)
                {
                    if (MyInfo.Weapon != null)
                    {
                        MyInfo.Weapon.实际耐久度--;
                        MyInfo.RemainAttactTimes = 0;
                    }
                }
                else
                {
                    //攻击次数的清算,潜行等去除(如果不是被攻击方的处理)
                    MyInfo.BattleField.BattleMinions[攻击方Pos - 1].AfterDoAttack(被动攻击);
                }
            }
            //伤害计算(本方)
            //被攻击方如果是英雄，则认为这个时候是攻击方的回合，
            //则被攻击方的英雄是关闭武器状态的，本方不会受到伤害
            var YourAttackPoint = 0;
            if (被攻击方Pos != BattleFieldInfo.HeroPos)
            {
                YourAttackPoint = YourInfo.BattleField.BattleMinions[被攻击方Pos - 1].TotalAttack();
            }
            if (攻击方Pos != BattleFieldInfo.HeroPos)
            {
                //圣盾不引发伤害事件
                if (MyInfo.BattleField.BattleMinions[攻击方Pos - 1].AfterBeAttack(YourAttackPoint))
                {
                    事件池.Add(new Engine.Utility.CardUtility.全局事件()
                    {
                        事件类型 = CardUtility.事件类型列表.受伤,
                        触发方向 = CardUtility.TargetSelectDirectEnum.本方,
                        触发位置 = 攻击方Pos
                    });
                }
            }
            else
            {
                //护甲不引发伤害事件
                if (MyInfo.AfterBeAttack(YourAttackPoint))
                {
                    事件池.Add(new Engine.Utility.CardUtility.全局事件()
                    {
                        事件类型 = CardUtility.事件类型列表.受伤,
                        触发方向 = CardUtility.TargetSelectDirectEnum.本方,
                        触发位置 = BattleFieldInfo.HeroPos
                    });
                }
            }
            //伤害计算(对方)
            var MyAttackPoint = 0;
            if (攻击方Pos != BattleFieldInfo.HeroPos)
            {
                MyAttackPoint = MyInfo.BattleField.BattleMinions[攻击方Pos - 1].TotalAttack();
            }
            else
            {
                if (MyInfo.Weapon != null) MyAttackPoint = MyInfo.Weapon.实际攻击力;
            }
            if (被攻击方Pos != BattleFieldInfo.HeroPos)
            {
                if (YourInfo.BattleField.BattleMinions[被攻击方Pos - 1].AfterBeAttack(MyAttackPoint))
                {
                    事件池.Add(new Engine.Utility.CardUtility.全局事件()
                    {
                        事件类型 = CardUtility.事件类型列表.受伤,
                        触发方向 = CardUtility.TargetSelectDirectEnum.对方,
                        触发位置 = 被攻击方Pos
                    });
                }
            }
            else
            {
                //护甲不引发伤害事件
                if (YourInfo.AfterBeAttack(MyAttackPoint))
                {
                    事件池.Add(new Engine.Utility.CardUtility.全局事件()
                    {
                        事件类型 = CardUtility.事件类型列表.受伤,
                        触发方向 = CardUtility.TargetSelectDirectEnum.对方,
                        触发位置 = BattleFieldInfo.HeroPos
                    });
                }
            }

            //每次操作后进行一次清算
            if (!被动攻击)
            {
                //将亡语效果放入结果
                Result.AddRange(Settle());
            }
            else
            {
                //对方已经发送亡语效果，本方不用重复模拟了
                Settle();
            }
            return Result;
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
            if (MyInfo.Weapon != null && MyInfo.Weapon.实际耐久度 == 0) MyInfo.Weapon = null;
            if (YourInfo.Weapon != null && YourInfo.Weapon.实际耐久度 == 0) YourInfo.Weapon = null;
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
                if (Seekcard.SN == CardSn)
                {
                    removeCard = Seekcard;
                }
            }
            MySelfInfo.handCards.Remove(removeCard);
            MyInfo.HandCardCount = MySelfInfo.handCards.Count;
        }
        /// <summary>
        /// 武器是否可用
        /// </summary>
        /// <returns></returns>
        public Boolean IsWeaponEnable()
        {
            return MyInfo.RemainAttactTimes != 0 &&
                   MyInfo.Weapon != null &&
                   MyInfo.Weapon.实际耐久度 > 0 &&
                   IsMyTurn;
        }
        /// <summary>
        /// 英雄技能是否可用
        /// </summary>
        /// <returns></returns>
        public Boolean IsHeroAblityEnable()
        {
            return (!MyInfo.IsUsedHeroAbility) && IsMyTurn &&
                    MyInfo.crystal.CurrentRemainPoint >= MyInfo.HeroAbility.ActualCostPoint;
        }

        #region "Event"
        /// <summary>
        /// 事件池
        /// </summary>
        public List<CardUtility.全局事件> 事件池 = new List<CardUtility.全局事件>();
        /// <summary>
        /// 事件处理
        /// </summary>
        /// <returns></returns>
        public List<String> 事件处理()
        {
            List<String> Result = new List<string>();
            for (int j = 0; j < 事件池.Count; j++)
            {
                CardUtility.全局事件 事件 = 事件池[j];
                for (int i = 0; i < MyInfo.BattleField.MinionCount; i++)
                {
                    if (事件.事件类型 == CardUtility.事件类型列表.召唤 &&
                        事件.触发位置 == (i + 1) &&
                        事件.触发方向 == CardUtility.TargetSelectDirectEnum.本方)
                    {
                        continue;
                    }
                    else
                    {
                        Result.AddRange(MyInfo.BattleField.BattleMinions[i].事件处理方法(事件, this, CardUtility.strMe + CardUtility.strSplitMark + (i + 1).ToString()));
                    }
                }
                //转换触发方向，对方触发事件？结果是否传送？传送时候要改变strMe和strYou！
                if (事件.触发方向 == CardUtility.TargetSelectDirectEnum.本方)
                {
                    事件.触发方向 = CardUtility.TargetSelectDirectEnum.对方;
                }
                else
                {
                    事件.触发方向 = CardUtility.TargetSelectDirectEnum.本方;
                }
                for (int i = 0; i < YourInfo.BattleField.MinionCount; i++)
                {
                    YourInfo.BattleField.BattleMinions[i].事件处理方法(事件, this, CardUtility.strYou + CardUtility.strSplitMark + (i + 1).ToString());
                }
            }
            return Result;
        }
        #endregion
    }
}
