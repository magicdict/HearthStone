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
            string[] actField = item.Split(CardUtility.strSplitMark.ToCharArray());
            switch (Engine.Server.ActionCode.GetActionType(item))
            {
                case ActionCode.ActionType.Card:
                    CardEffect.ReRunEffect(game, actField);
                    break;
                case ActionCode.ActionType.UseMinion:
                    int Pos = int.Parse(actField[2]);
                    var minion = (Engine.Card.MinionCard)Engine.Utility.CardUtility.GetCardInfoBySN(actField[1]);
                    minion.Init();
                    game.GuestInfo.BattleField.PutToBattle(Pos, minion);
                    game.GuestInfo.BattleField.ResetBuff();
                    break;
                case ActionCode.ActionType.UseWeapon:
                    game.GuestInfo.Weapon = (Engine.Card.WeaponCard)Engine.Utility.CardUtility.GetCardInfoBySN(actField[1]);
                    break;
                case ActionCode.ActionType.UseSecret:
                    game.GuestInfo.SecretCount++; ;
                    break;
                case ActionCode.ActionType.UseAbility:
                    break;
                case ActionCode.ActionType.Fight:
                    //FIGHT#1#2
                    FightHandler.Fight(int.Parse(actField[2]), int.Parse(actField[1]), game, true);
                    break;
                case ActionCode.ActionType.Point:
                    IAtomicEffect point = new PointEffect();
                    point.ReRunEffect(game, actField);
                    break;
                case ActionCode.ActionType.Health:
                    IAtomicEffect health = new HealthEffect();
                    health.ReRunEffect(game, actField);
                    break;
                case ActionCode.ActionType.Status:
                    IAtomicEffect status = new StatusEffect();
                    status.ReRunEffect(game, actField);
                    break;
                case ActionCode.ActionType.Transform:
                    IAtomicEffect transform = new TransformEffect();
                    transform.ReRunEffect(game, actField);
                    break;
                case ActionCode.ActionType.Attack:
                    IAtomicEffect attack = new AttackEffect();
                    attack.ReRunEffect(game, actField);
                    break;
                case ActionCode.ActionType.HitSecret:
                    SecretCard.ReRunSecret(game, actField);
                    break;
                case ActionCode.ActionType.Control:
                    ControlEffect.ReRunEffect(game, actField);
                    break;
                case ActionCode.ActionType.Summon:
                    SummonEffect.ReRunEffect(game, actField);
                    break;
                case ActionCode.ActionType.Crystal:
                    CrystalEffect.ReRunEffect(game, actField);
                    break;
                case ActionCode.ActionType.WeaponPoint:
                    WeaponPointEffect.ReRunEffect(game, actField);
                    break;
                case ActionCode.ActionType.Settle:
                    game.Settle();
                    break;
            }
        }
        #endregion

    }
}
