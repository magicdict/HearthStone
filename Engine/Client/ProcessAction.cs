using Engine.Card;
using Engine.Effect;
using Engine.Server;
using Engine.Utility;

namespace Engine.Client
{
    public static class ProcessAction
    {
        #region"处理对方的动作"
        /// <summary>
        /// 处理对方的动作
        /// </summary>
        /// <param name="item"></param>
        /// <param name="game"></param>
        public static void Process(string item, GameManager game)
        {
            var actField = item.Split(CardUtility.strSplitMark.ToCharArray());
            switch (Engine.Server.ActionCode.GetActionType(item))
            {
                case ActionCode.ActionType.Card:
                    if (actField[1] == CardUtility.strYou)
                    {
                        if (actField.Length == 3)
                        {
                            //如果有第三参数，则获得指定手牌
                            game.MySelfInfo.handCards.Add(Engine.Utility.CardUtility.GetCardInfoBySN(actField[2]));
                            game.MyInfo.HandCardCount++;
                        }
                        else
                        {
                            var drawCards = Engine.Client.ClientRequest.DrawCard(game.GameId.ToString(GameServer.GameIdFormat), game.IsFirst, 1);
                            game.MySelfInfo.handCards.Add(Engine.Utility.CardUtility.GetCardInfoBySN(drawCards[0]));
                            game.MyInfo.HandCardCount++;
                            game.MyInfo.RemainCardDeckCount--;
                        }
                    }
                    else
                    {
                        game.YourInfo.HandCardCount++;
                        game.YourInfo.RemainCardDeckCount--;
                    }
                    break;
                case ActionCode.ActionType.UseMinion:
                    int Pos = int.Parse(actField[2]);
                    var minion = (Engine.Card.MinionCard)Engine.Utility.CardUtility.GetCardInfoBySN(actField[1]);
                    minion.Init();
                    game.YourInfo.BattleField.PutToBattle(Pos, minion);
                    game.YourInfo.BattleField.ResetBuff();
                    break;
                case ActionCode.ActionType.UseWeapon:
                    game.YourInfo.Weapon = (Engine.Card.WeaponCard)Engine.Utility.CardUtility.GetCardInfoBySN(actField[1]);
                    break;
                case ActionCode.ActionType.UseSecret:
                    game.YourInfo.SecretCount++; ;
                    break;
                case ActionCode.ActionType.UseAbility:
                    break;
                case ActionCode.ActionType.Fight:
                    //FIGHT#1#2
                    game.Fight(int.Parse(actField[2]), int.Parse(actField[1]), true);
                    break;
                case ActionCode.ActionType.Point:
                case ActionCode.ActionType.Health:
                case ActionCode.ActionType.Status:
                case ActionCode.ActionType.Transform:
                case ActionCode.ActionType.Attack:
                    RunNormalEffect(game, actField);
                    break;
                case ActionCode.ActionType.HitSecret:
                    if (actField[1] == CardUtility.strYou)
                    {
                        Engine.Card.SecretCard Hit = new SecretCard();
                        foreach (var secret in game.MySelfInfo.奥秘列表)
                        {
                            if (secret.SN == actField[2])
                            {
                                Hit = secret;
                                break;
                            }
                        }
                        game.MySelfInfo.奥秘列表.Remove(Hit);
                    }
                    else
                    {
                        game.YourInfo.SecretCount--;
                    }
                    break;
                case ActionCode.ActionType.Control:
                    game.YourInfo.BattleField.AppendToBattle(game.MyInfo.BattleField.BattleMinions[int.Parse(actField[1]) - 1].深拷贝());
                    game.MyInfo.BattleField.BattleMinions[int.Parse(actField[1]) - 1] = null;
                    break;
                case ActionCode.ActionType.Summon:
                    //不会出现溢出的问题，溢出在Effect里面处理过了
                    //SUMMON#YOU#M000001
                    //Me代表对方 YOU代表自己，必须反过来
                    if (actField[1] == CardUtility.strYou)
                    {
                        game.MyInfo.BattleField.AppendToBattle(actField[2]);
                    }
                    else
                    {
                        game.YourInfo.BattleField.AppendToBattle(actField[2]);
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
                        game.MyInfo.crystal.CurrentRemainPoint = int.Parse(actField[2]);
                        game.MyInfo.crystal.CurrentFullPoint = int.Parse(actField[3]);
                    }
                    break;
                case ActionCode.ActionType.WeaponPoint:
                    //WeaponPoint#ME#+0/+0
                    //Me代表对方 YOU代表自己，必须反过来
                    string[] Op = actField[2].Split("/".ToCharArray());
                    if (actField[1] == CardUtility.strMe)
                    {
                        game.MyInfo.Weapon.实际攻击力 += int.Parse(Op[0]);
                        game.MyInfo.Weapon.实际耐久度 += int.Parse(Op[1]);
                    }
                    else
                    {
                        game.YourInfo.Weapon.实际攻击力 += int.Parse(Op[0]);
                        game.YourInfo.Weapon.实际耐久度 += int.Parse(Op[1]);
                    }
                    break;
                case ActionCode.ActionType.UnKnown:
                    break;
            }
            //这里不需要发送亡语效果，
            //由法术或者攻击发动放将结果发送给接受方
            game.Settle();
        }
        /// <summary>
        /// 运行效果
        /// </summary>
        /// <param name="game"></param>
        /// <param name="actField"></param>
        private static void RunNormalEffect(GameManager game, string[] actField)
        {
            //ATTACK#ME#POS#AP
            //Me代表对方 YOU代表自己，必须反过来
            IEffectHandler handler = new AttackEffect();
            //AtomicEffectDefine SingleEffect = new AtomicEffectDefine();
            //switch (actField[0])
            //{
            //case Card.Server.ActionCode.strAttack:
            //    handler = new AttackEffect();
            //    SingleEffect.ActualEffectPoint = actField[3];
            //    break;
            //case Card.Server.ActionCode.strHealth:
            //    handler = new HealthEffect();
            //    SingleEffect.ActualEffectPoint = actField[3];
            //    if (actField.Length == 5) SingleEffect.AdditionInfo = actField[4];
            //    break;
            //case Card.Server.ActionCode.strStatus:
            //    handler = new StatusEffect();
            //    SingleEffect.AdditionInfo = actField[3];
            //    break;
            //case Card.Server.ActionCode.strPoint:
            //    handler = new PointEffect();
            //    SingleEffect.AdditionInfo = actField[3];
            //    SingleEffect.StandardEffectPoint = actField[4];
            //    break;
            //case Card.Server.ActionCode.strTransform:
            //    handler = new TransformEffect();
            //    SingleEffect.AdditionInfo = actField[3];
            //    break;
            //}
            //if (actField[1] == CardUtility.strYou)
            //{
            //    switch (int.Parse(actField[2]))
            //    {
            //        case BattleFieldInfo.HeroPos:
            //            handler.DealHero(game, SingleEffect, true);
            //            break;
            //        case BattleFieldInfo.AllPos:
            //            for (int i = 0; i < game.MyInfo.BattleField.MinionCount; i++)
            //            {
            //                handler.DealMinion(game, SingleEffect, true, i);
            //            }
            //            break;
            //        default:
            //            handler.DealMinion(game, SingleEffect, true, int.Parse(actField[2]) - 1);
            //            break;
            //    }
            //}
            //else
            //{
            //    switch (int.Parse(actField[2]))
            //    {
            //        case BattleFieldInfo.HeroPos:
            //            handler.DealHero(game, SingleEffect, false);
            //            break;
            //        case BattleFieldInfo.AllPos:
            //            for (int i = 0; i < game.YourInfo.BattleField.MinionCount; i++)
            //            {
            //                handler.DealMinion(game, SingleEffect, false, i);
            //            }
            //            break;
            //        default:
            //            handler.DealMinion(game, SingleEffect, false, int.Parse(actField[2]) - 1);
            //            break;
            //    }
            //}
        }
        #endregion

    }
}
