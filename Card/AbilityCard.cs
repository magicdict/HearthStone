using Card.Effect;
using System;
using System.Collections.Generic;

namespace Card
{
    /// <summary>
    /// 法术卡牌
    /// </summary>
    [Serializable]
    public class AbilityCard : CardBasicInfo
    {
        /// <summary>
        /// 法术
        /// </summary>
        public Ability CardAbility = new Ability();
        /// <summary>
        /// 设置初始状态
        /// </summary>
        public new void Init()
        {
            CardAbility.Init();
        }
        /// <summary>
        /// 原生法术
        /// </summary>
        public const String 原生法术 = "0";
        /// <summary>
        /// 调整法术效果
        /// </summary>
        /// <param name="AbilityEffect"></param>
        public void JustfyEffectPoint(int AbilityEffect)
        {
            //法术强度本意是增加法术卡的总伤。以奥术飞弹为例，法术强度+1会令奥术飞弹多1发伤害，而非单发伤害+1。法术强度不影响治疗效果。
            switch (CardAbility.FirstAbilityDefine.AbilityEffectType)
            {
                case EffectDefine.AbilityEffectEnum.攻击:
                    if (CardAbility.FirstAbilityDefine.StandardEffectCount == 1)
                    {
                        CardAbility.FirstAbilityDefine.ActualEffectPoint = CardAbility.FirstAbilityDefine.StandardEffectPoint + AbilityEffect;
                    }
                    else
                    {
                        CardAbility.FirstAbilityDefine.ActualEffectCount = CardAbility.FirstAbilityDefine.StandardEffectCount + AbilityEffect;
                    }
                    break;
                case EffectDefine.AbilityEffectEnum.回复:
                    CardAbility.FirstAbilityDefine.ActualEffectPoint = CardAbility.FirstAbilityDefine.StandardEffectPoint + AbilityEffect;
                    break;
            }
            if (CardAbility.SecondAbilityDefine.AbilityEffectType != EffectDefine.AbilityEffectEnum.未定义)
            {
                switch (CardAbility.SecondAbilityDefine.AbilityEffectType)
                {
                    case EffectDefine.AbilityEffectEnum.攻击:
                        if (CardAbility.SecondAbilityDefine.StandardEffectCount == 1)
                        {
                            CardAbility.SecondAbilityDefine.ActualEffectPoint = CardAbility.SecondAbilityDefine.StandardEffectPoint + AbilityEffect;
                        }
                        else
                        {
                            CardAbility.SecondAbilityDefine.ActualEffectCount = CardAbility.SecondAbilityDefine.StandardEffectCount + AbilityEffect;
                        }
                        break;
                    case EffectDefine.AbilityEffectEnum.回复:
                        CardAbility.SecondAbilityDefine.ActualEffectPoint = CardAbility.SecondAbilityDefine.StandardEffectPoint + AbilityEffect;
                        break;
                }
            }
        }
        /// <summary>
        /// 简单检查
        /// </summary>
        /// <param name="gameManager"></param>
        /// <returns></returns>
        public bool CheckCondition(Client.GameManager gameManager)
        {
            if (CardAbility.SecondAbilityDefine.AbilityEffectType != EffectDefine.AbilityEffectEnum.未定义) return true;

            switch (CardAbility.FirstAbilityDefine.AbilityEffectType)
            {
                case EffectDefine.AbilityEffectEnum.点数:
                case EffectDefine.AbilityEffectEnum.变形:
                    if (CardAbility.FirstAbilityDefine.SelectOpt.EffectTargetSelectDirect == CardUtility.TargetSelectDirectEnum.本方)
                    {
                        return gameManager.MyInfo.BattleField.MinionCount > 0;
                    }
                    if (CardAbility.FirstAbilityDefine.SelectOpt.EffectTargetSelectDirect == CardUtility.TargetSelectDirectEnum.对方)
                    {
                        return gameManager.YourInfo.BattleField.MinionCount > 0;
                    }
                    if (CardAbility.FirstAbilityDefine.SelectOpt.EffectTargetSelectDirect == CardUtility.TargetSelectDirectEnum.双方)
                    {
                        return (gameManager.YourInfo.BattleField.MinionCount +
                                gameManager.MyInfo.BattleField.MinionCount) > 0;
                    }
                    break;
                case EffectDefine.AbilityEffectEnum.召唤:
                    if (CardAbility.FirstAbilityDefine.SelectOpt.EffectTargetSelectDirect == CardUtility.TargetSelectDirectEnum.本方)
                    {
                        return gameManager.MyInfo.BattleField.MinionCount < Card.Client.BattleFieldInfo.MaxMinionCount;
                    }
                    if (CardAbility.FirstAbilityDefine.SelectOpt.EffectTargetSelectDirect == CardUtility.TargetSelectDirectEnum.对方)
                    {
                        return gameManager.YourInfo.BattleField.MinionCount < Card.Client.BattleFieldInfo.MaxMinionCount;
                    }
                    if (CardAbility.FirstAbilityDefine.SelectOpt.EffectTargetSelectDirect == CardUtility.TargetSelectDirectEnum.双方)
                    {
                        return (gameManager.YourInfo.BattleField.MinionCount < Card.Client.BattleFieldInfo.MaxMinionCount) &&
                               (gameManager.MyInfo.BattleField.MinionCount < Card.Client.BattleFieldInfo.MaxMinionCount);
                    }
                    break;
            }
            return true;
        }
    }
}
