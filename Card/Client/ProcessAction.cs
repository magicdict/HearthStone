using Card.Server;
using System;

namespace Card.Client
{
    public static class ProcessAction
    {
        #region"处理对方的动作"
        /// <summary>
        /// 攻击位置修正
        /// 全体伤害的时候，如果有目标被杀死，
        /// 则清算的时候，攻击目标会出现偏移
        /// </summary>
        private static int AttackTargetOffsetMe = 0;
        private static int AttackTargetOffsetYou = 0;

        /// <summary>
        /// 处理对方的动作
        /// </summary>
        /// <param name="item"></param>
        /// <param name="game"></param>
        public static void Process(string item, GameManager game)
        {
            var actField = item.Split(CardUtility.strSplitMark.ToCharArray());
            //AttackTargetOffset 在使用手牌或者进行卡牌直接攻击的时候，清零！
            switch (Card.Server.ActionCode.GetActionType(item))
            {
                case ActionCode.ActionType.UseMinion:
                    AttackTargetOffsetMe = 0;
                    AttackTargetOffsetYou = 0;
                    int Pos = int.Parse(actField[2]);
                    var minion = (Card.MinionCard)Card.CardUtility.GetCardInfoBySN(actField[1]);
                    minion.Init();
                    game.YourInfo.BattleField.PutToBattle(Pos, minion);
                    game.YourInfo.BattleField.ResetBuff();
                    break;
                case ActionCode.ActionType.UseWeapon:
                    AttackTargetOffsetMe = 0;
                    AttackTargetOffsetYou = 0;
                    game.YourInfo.Weapon = (Card.WeaponCard)Card.CardUtility.GetCardInfoBySN(actField[1]);
                    break;
                case ActionCode.ActionType.UseSecret:
                    AttackTargetOffsetMe = 0;
                    AttackTargetOffsetYou = 0;
                    game.YourInfo.SecretCount++; ;
                    break;
                case ActionCode.ActionType.UseAbility:
                    AttackTargetOffsetMe = 0;
                    AttackTargetOffsetYou = 0;
                    break;
                case ActionCode.ActionType.Fight:
                    //FIGHT#1#2
                    AttackTargetOffsetMe = 0;
                    AttackTargetOffsetYou = 0;
                    game.Fight(int.Parse(actField[2]), int.Parse(actField[1]), true);
                    break;
                case ActionCode.ActionType.HitSecret:
                    AttackTargetOffsetMe = 0;
                    AttackTargetOffsetYou = 0;
                    if (actField[1] == CardUtility.strYou)
                    {
                        //
                        Card.SecretCard Hit = new SecretCard();
                        foreach (var secret in game.MySelf.奥秘列表)
                        {
                            if (secret.SN == actField[2])
                            {
                                Hit = secret;
                                break;
                            }
                        }
                        game.MySelf.奥秘列表.Remove(Hit);
                    }
                    else
                    {
                        game.YourInfo.SecretCount--;
                    }
                    break;
                case ActionCode.ActionType.Control:
                    game.YourInfo.BattleField.AppendToBattle(game.MySelf.RoleInfo.BattleField.BattleMinions[int.Parse(actField[1]) - 1].深拷贝());
                    game.MySelf.RoleInfo.BattleField.BattleMinions[int.Parse(actField[1]) - 1] = null;
                    break;
                case ActionCode.ActionType.Point:
                    if (actField[1] == CardUtility.strYou)
                    {
                        Card.Effect.PointEffect.RunPointEffect(game.MySelf.RoleInfo.BattleField.BattleMinions[int.Parse(actField[2]) - 1], actField[3]);
                    }
                    else
                    {
                        Card.Effect.PointEffect.RunPointEffect(game.YourInfo.BattleField.BattleMinions[int.Parse(actField[2]) - 1], actField[3]);
                    }
                    break;
                case ActionCode.ActionType.Card:
                    if (actField[1] == CardUtility.strYou)
                    {
                        var drawCards = Card.Client.ClientRequest.DrawCard(game.GameId.ToString(GameServer.GameIdFormat), game.IsFirst, 1);
                        game.MySelf.handCards.Add(Card.CardUtility.GetCardInfoBySN(drawCards[0]));
                        game.MySelf.RoleInfo.HandCardCount++;
                        game.MySelf.RoleInfo.RemainCardDeckCount--;
                    }
                    else
                    {
                        game.YourInfo.HandCardCount++;
                        game.YourInfo.RemainCardDeckCount--;
                    }
                    break;
                case ActionCode.ActionType.Summon:
                    //不会出现溢出的问题，溢出在Effect里面处理过了
                    //SUMMON#YOU#M000001
                    //Me代表对方 YOU代表自己，必须反过来
                    if (actField[1] == CardUtility.strYou)
                    {
                        game.MySelf.RoleInfo.BattleField.AppendToBattle(actField[2]);
                    }
                    else
                    {
                        game.YourInfo.BattleField.AppendToBattle(actField[2]);
                    }
                    break;
                case ActionCode.ActionType.Health:
                    //HEALTH#ME#1#2
                    //Me代表对方 YOU代表自己，必须反过来
                    if (actField[1] == CardUtility.strYou)
                    {
                        if (actField[2] == Card.Client.BattleFieldInfo.HeroPos.ToString())
                        {
                            game.MySelf.RoleInfo.HealthPoint = int.Parse(actField[3]);
                        }
                        else
                        {
                            //位置从1开始，数组从0开始
                            game.MySelf.RoleInfo.BattleField.BattleMinions[int.Parse(actField[2]) - 1].实际生命值 = int.Parse(actField[3]);
                        }
                    }
                    else
                    {
                        if (actField[2] == Card.Client.BattleFieldInfo.HeroPos.ToString())
                        {
                            game.YourInfo.HealthPoint = int.Parse(actField[3]);
                        }
                        else
                        {
                            //位置从1开始，数组从0开始
                            game.YourInfo.BattleField.BattleMinions[int.Parse(actField[2]) - 1].实际生命值 = int.Parse(actField[3]);
                        }
                    }
                    break;
                case ActionCode.ActionType.Status:
                    //STATUS#ME#1#FREEZE
                    //Me代表对方 YOU代表自己，必须反过来
                    if (actField[1] == CardUtility.strYou)
                    {
                        if (actField[2] == Card.Client.BattleFieldInfo.HeroPos.ToString())
                        {
                            switch (actField[3])
                            {
                                case Card.Effect.StatusEffect.strFreeze:
                                    game.MySelf.RoleInfo.冰冻状态 = CardUtility.EffectTurn.效果命中;
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            //位置从1开始，数组从0开始
                            switch (actField[3])
                            {
                                case Card.Effect.StatusEffect.strFreeze:
                                    game.MySelf.RoleInfo.BattleField.BattleMinions[int.Parse(actField[2]) - 1].冰冻状态 = CardUtility.EffectTurn.效果命中;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    else
                    {
                        if (actField[2] == Card.Client.BattleFieldInfo.HeroPos.ToString())
                        {
                            switch (actField[3])
                            {
                                case Card.Effect.StatusEffect.strFreeze:
                                    game.YourInfo.冰冻状态 = CardUtility.EffectTurn.效果命中;
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            //位置从1开始，数组从0开始
                            switch (actField[3])
                            {
                                case Card.Effect.StatusEffect.strFreeze:
                                    game.YourInfo.BattleField.BattleMinions[int.Parse(actField[2]) - 1].冰冻状态 = CardUtility.EffectTurn.效果命中;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    break;
                case ActionCode.ActionType.Crystal:
                    //Crystal#ME#4#4
                    //Me代表对方 YOU代表自己，必须反过来
                    if (actField[1] == CardUtility.strMe)
                    {
                        game.YourInfo.crystal.CurrentRemainPoint = int.Parse(actField[2]);
                        game.YourInfo.crystal.CurrentFullPoint = int.Parse(actField[3]);
                    }
                    else
                    {
                        game.MySelf.RoleInfo.crystal.CurrentRemainPoint = int.Parse(actField[2]);
                        game.MySelf.RoleInfo.crystal.CurrentFullPoint = int.Parse(actField[3]);
                    }
                    break;
                case ActionCode.ActionType.Transform:
                    //TRANSFORM#ME#1#M9000001
                    //Me代表对方 YOU代表自己，必须反过来
                    if (actField[1] == CardUtility.strYou)
                    {
                        game.MySelf.RoleInfo.BattleField.BattleMinions[int.Parse(actField[2]) - 1] = (Card.MinionCard)Card.CardUtility.GetCardInfoBySN(actField[3]);
                    }
                    else
                    {
                        game.MySelf.RoleInfo.BattleField.BattleMinions[int.Parse(actField[2]) - 1] = (Card.MinionCard)Card.CardUtility.GetCardInfoBySN(actField[3]);
                    }
                    break;
                case ActionCode.ActionType.Attack:
                    //ATTACK#ME#POS#AP
                    //Me代表对方 YOU代表自己，必须反过来
                    int AttackPoint = int.Parse(actField[3]);
                    if (actField[1] == CardUtility.strYou)
                    {
                        if (actField[2] == Card.Client.BattleFieldInfo.HeroPos.ToString())
                        {
                            game.MySelf.RoleInfo.AfterBeAttack(AttackPoint);
                        }
                        else
                        {
                            //位置从1开始，数组从0开始
                            game.MySelf.RoleInfo.BattleField.BattleMinions[int.Parse(actField[2]) - 1 - AttackTargetOffsetMe].AfterBeAttack(AttackPoint);
                        }
                    }
                    else
                    {
                        if (actField[2] == Card.Client.BattleFieldInfo.HeroPos.ToString())
                        {
                            game.YourInfo.AfterBeAttack(AttackPoint);
                        }
                        else
                        {
                            //位置从1开始，数组从0开始
                            game.YourInfo.BattleField.BattleMinions[int.Parse(actField[2]) - 1 - AttackTargetOffsetYou].AfterBeAttack(AttackPoint);
                        }
                    }
                    break;
                case ActionCode.ActionType.UnKnown:
                    break;
            }
            //这里不需要发送亡语效果，
            //由法术或者攻击发动放将结果发送给接受方
            var resultLst = game.Settle();
            foreach (var result in resultLst)
            {
                if (result.StartsWith(Card.Server.ActionCode.strDead + Card.CardUtility.strSplitMark + CardUtility.strMe)) AttackTargetOffsetMe++;
                if (result.StartsWith(Card.Server.ActionCode.strDead + Card.CardUtility.strSplitMark + CardUtility.strYou)) AttackTargetOffsetYou++;
            }
        }
        #endregion

    }
}
