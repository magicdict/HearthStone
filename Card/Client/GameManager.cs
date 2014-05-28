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
            //抽牌的具体方法
            CardUtility.DrawCard += DrawCardAtServer;
            //图片请求
            CardUtility.GetCardImage += GetCardImageAtServer;
            //属性
            var HandCard = Card.Server.ClientUtlity.DrawCard(GameId.ToString(GameServer.GameIdFormat), IsFirst, IsFirst ? 3 : 4);
            //DEBUG
            //烈日行者
            //HandCard.Add("M000099");
            ////森林狼
            //HandCard.Add("M000090");
            ////银色侍从
            //HandCard.Add("M000077");
            ////岩浆暴怒者
            //HandCard.Add("M000097");
            ////虚空行者
            //HandCard.Add("M000084");
            //野性成长
            //HandCard.Add("A000091");
            //冰霜新星
            HandCard.Add("A000038");

            MySelf.RoleInfo.crystal.CurrentFullPoint = 5;
            MySelf.RoleInfo.crystal.CurrentRemainPoint = 5;
            //英雄技能：奥术飞弹
            MySelf.RoleInfo.HeroAbility = (Card.AbilityCard)Card.CardUtility.GetCardInfoBySN("A000065");
            AgainstInfo.HeroAbility = (Card.AbilityCard)Card.CardUtility.GetCardInfoBySN("A000065");
            //DEBUG
            if (!IsFirst) HandCard.Add(Card.CardUtility.SN幸运币);
            foreach (var card in HandCard)
            {
                MySelf.handCards.Add(CardUtility.GetCardInfoBySN(card));
            }
            MySelf.RoleInfo.HandCardCount = HandCard.Count;
            if (IsFirst)
            {
                MySelf.RoleInfo.RemainCardDeckCount = Card.Client.CardDeck.MaxCards - 3;
                AgainstInfo.RemainCardDeckCount = Card.Client.CardDeck.MaxCards - 4;
            }
            else
            {
                MySelf.RoleInfo.RemainCardDeckCount = Card.Client.CardDeck.MaxCards - 4;
                AgainstInfo.RemainCardDeckCount = Card.Client.CardDeck.MaxCards - 3;
            }
        }
        /// <summary>
        /// 本方情报
        /// </summary>
        public PlayerDetailInfo MySelf = new PlayerDetailInfo();
        /// <summary>
        /// 对方情报
        /// </summary>
        public PlayerBasicInfo AgainstInfo = new PlayerBasicInfo();
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
                AgainstInfo.crystal.NewTurn();
                AgainstInfo.HandCardCount++;
                AgainstInfo.RemainCardDeckCount--;
                AgainstInfo.RemainAttackCount = 1;
                AgainstInfo.IsUsedHeroAbility = false;
                //重置攻击次数
                foreach (var minion in AgainstInfo.BattleField.BattleMinions)
                {
                    if (minion != null) minion.ResetAttackTimes();
                }
                //如果对手有可以解除冰冻的，解除冰冻
                foreach (var minion in AgainstInfo.BattleField.BattleMinions)
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
        /// <param name="CardSn"></param>
        public List<String> UseAbility(Card.AbilityCard card)
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
                Result.AddRange(EffectDefine.RunSingleEffect(singleEff, this, Pos, Seed));
                Seed++;
                //每次原子操作后进行一次清算
                Settle();
            }
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
            //潜行等去除(如果不是被攻击方的处理)
            if (!被动攻击 && (MyPos != 0)) MySelf.RoleInfo.BattleField.BattleMinions[MyPos - 1].AfterAttack();
            //伤害计算(本方)
            var YourAttackPoint = 0;
            if (YourPos != 0)
            {
                YourAttackPoint = AgainstInfo.BattleField.BattleMinions[YourPos - 1].TotalAttack();
            }
            else
            {
                if (AgainstInfo.Weapon != null) YourAttackPoint = AgainstInfo.Weapon.ActualAttackPoint;
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
                AgainstInfo.BattleField.BattleMinions[YourPos - 1].AfterBeAttack(MyAttackPoint);
            }
            else
            {
                AgainstInfo.HealthPoint -= MyAttackPoint;
            }
            //每次操作后进行一次清算
            Settle();
            return Result;
        }
        /// <summary>
        /// 清算(核心方法)
        /// </summary>
        /// <returns></returns>
        public List<String> Settle()
        {
            List<String> Result = new List<string>();
            //1.检查需要移除的对象
            MySelf.RoleInfo.BattleField.ClearDead();
            AgainstInfo.BattleField.ClearDead();
            //2.重新计算Buff
            MySelf.RoleInfo.BattleField.ResetBuff();
            AgainstInfo.BattleField.ResetBuff();
            //3.武器的移除
            if (MySelf.RoleInfo.Weapon != null && MySelf.RoleInfo.Weapon.实际耐久度 == 0) MySelf.RoleInfo.Weapon = null;
            if (AgainstInfo.Weapon != null && AgainstInfo.Weapon.实际耐久度 == 0) AgainstInfo.Weapon = null;
            return Result;
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
        /// <summary>
        /// 抽牌（服务器方法）
        /// </summary>
        /// <returns></returns>
        public List<String> DrawCardAtServer(Boolean IsFirst, int Count)
        {
            //向服务器提出请求，获得牌
            return GameServer.DrawCard(GameId, IsFirst, Count);
        }
        /// <summary>
        /// 获得图片（服务器方法）
        /// </summary>
        /// <param name="ImageKey"></param>
        /// <returns></returns>
        private static System.Drawing.Image GetCardImageAtServer(string ImageKey)
        {
            throw new NotImplementedException();
        }
    }
}
