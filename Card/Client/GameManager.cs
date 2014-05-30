using Card;
using Card.Effect;
using Card.Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace Card.Client
{
    /// <summary>
    /// 游戏管理
    /// </summary>
    public class GameManager
    {
        /// <summary>
        /// 游戏玩家名称
        /// </summary>
        public String PlayerNickName = "DARUMA";
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
        public Card.CardUtility.deleteGetTargetPosition GetSelectTarget;
        /// <summary>
        /// 抉择卡牌
        /// </summary>
        public Card.CardUtility.delegatePickEffect PickEffect;
        /// <summary>
        /// 
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
            var HandCard = Card.Server.ClientUtlity.DrawCard(GameId.ToString(GameServer.GameIdFormat), IsFirst, IsFirst ? 3 : 4);
            MySelf.RoleInfo.crystal.CurrentFullPoint = 0;
            MySelf.RoleInfo.crystal.CurrentRemainPoint = 0;
            //DEBUG START
            HandCard.Add("S000001");
            MySelf.RoleInfo.crystal.CurrentFullPoint = 5;
            MySelf.RoleInfo.crystal.CurrentRemainPoint = 5;
            //DEBUG END
            //英雄技能：奥术飞弹
            MySelf.RoleInfo.HeroAbility = (Card.AbilityCard)Card.CardUtility.GetCardInfoBySN("A000056");
            YourInfo.HeroAbility = (Card.AbilityCard)Card.CardUtility.GetCardInfoBySN("A000056");
            if (!IsFirst) HandCard.Add(Card.CardUtility.SN幸运币);
            foreach (var card in HandCard)
            {
                MySelf.handCards.Add(CardUtility.GetCardInfoBySN(card));
            }
            MySelf.RoleInfo.HandCardCount = HandCard.Count;
            if (IsFirst)
            {
                MySelf.RoleInfo.RemainCardDeckCount = Card.Client.CardDeck.MaxCards - 3;
                YourInfo.RemainCardDeckCount = Card.Client.CardDeck.MaxCards - 4;
            }
            else
            {
                MySelf.RoleInfo.RemainCardDeckCount = Card.Client.CardDeck.MaxCards - 4;
                YourInfo.RemainCardDeckCount = Card.Client.CardDeck.MaxCards - 3;
            }
        }
        /// <summary>
        /// 本方情报
        /// </summary>
        public PlayerDetailInfo MySelf = new PlayerDetailInfo();
        /// <summary>
        /// 对方情报
        /// </summary>
        public PlayerBasicInfo YourInfo = new PlayerBasicInfo();
        /// <summary>
        /// 新的回合
        /// </summary>
        public void TurnStart()
        {
            if (IsMyTurn)
            {
                //魔法水晶的增加
                MySelf.RoleInfo.crystal.NewTurn();
                //手牌
                var NewCardList = Card.Server.ClientUtlity.DrawCard(GameId.ToString(GameServer.GameIdFormat), IsFirst, 1);
                foreach (var card in NewCardList)
                {
                    MySelf.handCards.Add(CardUtility.GetCardInfoBySN(card));
                }
                MySelf.RoleInfo.HandCardCount++;
                MySelf.RoleInfo.RemainCardDeckCount--;
                MySelf.RoleInfo.RemainAttackCount = 1;
                MySelf.RoleInfo.IsUsedHeroAbility = false;
                foreach (var minion in MySelf.RoleInfo.BattleField.BattleMinions)
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
                foreach (var minion in MySelf.RoleInfo.BattleField.BattleMinions)
                {
                    if (minion != null) minion.ResetAttackTimes();
                }
            }
            else
            {
                YourInfo.crystal.NewTurn();
                YourInfo.HandCardCount++;
                YourInfo.RemainCardDeckCount--;
                YourInfo.RemainAttackCount = 1;
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
        public void TurnEnd()
        {
            //调用这个方法的时候，IsMyTurn肯定是True
            //本方的回合结束效果
        }

        /// <summary>
        /// 全局随机种子
        /// </summary>
        public static int Seed = 1;
        /// <summary>
        /// 使用法术
        /// </summary>
        /// <param name="card"></param>
        /// <param name="ConvertPosDirect">对象方向转换</param>
        public List<String> UseAbility(Card.AbilityCard card, Boolean ConvertPosDirect)
        {
            List<String> Result = new List<string>();
            Card.CardUtility.PickEffect PickEffectResult = CardUtility.PickEffect.第一效果;
            if (card.CardAbility.IsNeedSelect())
            {
                PickEffectResult = PickEffect(card.CardAbility.FirstAbilityDefine.Description, card.CardAbility.SecondAbilityDefine.Description);
                if (PickEffectResult == CardUtility.PickEffect.取消) return new List<string>();
            }
            var SingleEffectList = card.CardAbility.GetSingleEffectList(PickEffectResult == CardUtility.PickEffect.第一效果);
            for (int i = 0; i < SingleEffectList.Count; i++)
            {
                Card.CardUtility.TargetPosition Pos = new CardUtility.TargetPosition();
                //取消处理
                if (Pos.Postion == -1) return new List<string>();
                var singleEff = SingleEffectList[i];
                singleEff.EffectCount = 1;
                if (singleEff.IsNeedSelectTarget())
                {
                    Pos = GetSelectTarget(singleEff.EffectTargetSelectDirect, singleEff.EffectTargetSelectRole, false);
                }
                else
                {
                    if (ConvertPosDirect)
                    {
                        switch (singleEff.EffectTargetSelectDirect)
                        {
                            case CardUtility.TargetSelectDirectEnum.本方:
                                singleEff.EffectTargetSelectDirect = CardUtility.TargetSelectDirectEnum.对方;
                                break;
                            case CardUtility.TargetSelectDirectEnum.对方:
                                singleEff.EffectTargetSelectDirect = CardUtility.TargetSelectDirectEnum.本方;
                                break;
                            case CardUtility.TargetSelectDirectEnum.双方:
                                break;
                            default:
                                break;
                        }
                    }
                }
                Result.AddRange(EffectDefine.RunSingleEffect(singleEff, this, Pos, Seed));
                Seed++;
                //每次原子操作后进行一次清算
                //将亡语效果也发送给对方
                Result.AddRange(Settle());
            }
            return Result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionlst"></param>
        /// <returns></returns>
        public List<String> 奥秘计算(List<String> actionlst)
        {
            List<String> Result = new List<string>();
            //奥秘计算 START
            //本方（Fight也需要）
            if (MySelf.奥秘列表.Count != 0)
            {
                //本方的行动触发本方奥秘的检查
                for (int i = 0; i < actionlst.Count; i++)
                {
                    foreach (var secret in MySelf.奥秘列表)
                    {
                        if ((!secret.IsHitted) && Card.SecretCard.IsSecretHit(secret.SN, actionlst[i], true))
                        {
                            //奥秘执行
                            Result.AddRange(Card.SecretCard.RunSecretHit(secret.SN, actionlst[i], true, this));
                            secret.IsHitted = true;
                        }
                    }
                }
                //移除已经触发的奥秘
                MySelf.清除命中奥秘();
            }
            //对方（Fight也需要）
            if (YourInfo.SecretCount != 0)
            {
                var HitCard = Card.Server.ClientUtlity.IsSecretHit(GameId.ToString(GameServer.GameIdFormat), IsFirst, actionlst);
                if (!String.IsNullOrEmpty(HitCard))
                {
                    var HitCardList = HitCard.Split(Card.CardUtility.strSplitArrayMark.ToCharArray());
                    foreach (var hitCard in HitCardList)
                    {
                        Result.AddRange(Card.SecretCard.RunSecretHit(hitCard.Split(Card.CardUtility.strSplitDiffMark.ToCharArray())[0],
                                                                     hitCard.Split(Card.CardUtility.strSplitDiffMark.ToCharArray())[1], false, this));
                        YourInfo.SecretCount--;
                    }
                }
            }
            //奥秘计算 END
            return Result;

        }
        /// <summary>
        /// 战斗
        /// </summary>
        /// <param name="MyPos">本方</param>
        /// <param name="YourPos">对方</param>
        /// <param name="被动攻击">被动攻击</param>
        /// <returns></returns>
        public List<String> Fight(int MyPos, int YourPos, Boolean 被动攻击 = false)
        {
            List<String> Result = new List<string>();
            if (被动攻击)
            {
                if (YourPos != 0)
                {
                    YourInfo.BattleField.BattleMinions[YourPos - 1].RemainAttactTimes--;
                }
                else
                {
                    if (YourInfo.Weapon != null)
                    {
                        YourInfo.Weapon.实际耐久度--;
                        YourInfo.RemainAttackCount = 0;
                    }
                }
            }
            else
            {
                //攻击次数
                if (MyPos != 0)
                {
                    MySelf.RoleInfo.BattleField.BattleMinions[MyPos - 1].RemainAttactTimes--;
                }
                else
                {
                    if (MySelf.RoleInfo.Weapon != null)
                    {
                        MySelf.RoleInfo.Weapon.实际耐久度--;
                        MySelf.RoleInfo.RemainAttackCount = 0;
                    }
                }
            }
            //潜行等去除(如果不是被攻击方的处理)
            if (!被动攻击 && (MyPos != 0)) MySelf.RoleInfo.BattleField.BattleMinions[MyPos - 1].AfterAttack();
            //伤害计算(本方)
            var YourAttackPoint = 0;
            if (YourPos != 0)
            {
                YourAttackPoint = YourInfo.BattleField.BattleMinions[YourPos - 1].TotalAttack();
            }
            else
            {
                if (YourInfo.Weapon != null)
                {
                    YourAttackPoint = YourInfo.Weapon.ActualAttackPoint;
                }
            }
            if (MyPos != 0)
            {
                MySelf.RoleInfo.BattleField.BattleMinions[MyPos - 1].AfterBeAttack(YourAttackPoint);
            }
            else
            {
                MySelf.RoleInfo.HealthPoint -= YourAttackPoint;
            }
            //伤害计算(对方)
            var MyAttackPoint = 0;
            if (MyPos != 0)
            {
                MyAttackPoint = MySelf.RoleInfo.BattleField.BattleMinions[MyPos - 1].TotalAttack();
            }
            else
            {
                if (MySelf.RoleInfo.Weapon != null) MyAttackPoint = MySelf.RoleInfo.Weapon.ActualAttackPoint;
            }
            if (YourPos != 0)
            {
                YourInfo.BattleField.BattleMinions[YourPos - 1].AfterBeAttack(MyAttackPoint);
            }
            else
            {
                YourInfo.HealthPoint -= MyAttackPoint;
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
            List<String> actionlst = new List<string>();
            //1.检查需要移除的对象
            var MyDeadMinion = MySelf.RoleInfo.BattleField.ClearDead();
            foreach (var minion in MyDeadMinion)
            {
                actionlst.Add(Card.Server.ActionCode.strDead + Card.CardUtility.strSplitMark + CardUtility.strMe + Card.CardUtility.strSplitMark + minion.SN);
                //亡语的时候，需要倒置方向
                actionlst.AddRange(minion.发动亡语(this, false));
            }
            var YourDeadMinion = YourInfo.BattleField.ClearDead();
            foreach (var minion in YourDeadMinion)
            {
                actionlst.Add(Card.Server.ActionCode.strDead + Card.CardUtility.strSplitMark + CardUtility.strYou + Card.CardUtility.strSplitMark + minion.SN);
                //亡语的时候，需要倒置方向
                actionlst.AddRange(minion.发动亡语(this, true));
            }
            //2.重新计算Buff
            MySelf.RoleInfo.BattleField.ResetBuff();
            YourInfo.BattleField.ResetBuff();
            //3.武器的移除
            if (MySelf.RoleInfo.Weapon != null && MySelf.RoleInfo.Weapon.实际耐久度 == 0) MySelf.RoleInfo.Weapon = null;
            if (YourInfo.Weapon != null && YourInfo.Weapon.实际耐久度 == 0) YourInfo.Weapon = null;
            return actionlst;
        }
        public void RemoveUsedCard(String CardSn)
        {
            Card.CardBasicInfo removeCard = new CardBasicInfo();
            foreach (var Seekcard in MySelf.handCards)
            {
                if (Seekcard.SN == CardSn)
                {
                    removeCard = Seekcard;
                }
            }
            MySelf.handCards.Remove(removeCard);
            MySelf.RoleInfo.HandCardCount = MySelf.handCards.Count;
        }
        /// <summary>
        /// 武器是否可用
        /// </summary>
        /// <returns></returns>
        public Boolean IsWeaponEnable()
        {
            return MySelf.RoleInfo.RemainAttackCount != 0 &&
                   MySelf.RoleInfo.Weapon != null &&
                   MySelf.RoleInfo.Weapon.实际耐久度 > 0 &&
                   IsMyTurn;
        }
        /// <summary>
        /// 英雄技能是否可用
        /// </summary>
        /// <returns></returns>
        public Boolean IsHeroAblityEnable()
        {
            return (!MySelf.RoleInfo.IsUsedHeroAbility) && IsMyTurn &&
                    MySelf.RoleInfo.crystal.CurrentRemainPoint >= MySelf.RoleInfo.HeroAbility.ActualCostPoint;
        }
    }
}
