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
            switch (CardAbility.FirstAbilityDefine.MainAbilityDefine.AbilityEffectType)
            {
                case AtomicEffectDefine.AbilityEffectEnum.攻击:
                    if (CardAbility.FirstAbilityDefine.MainAbilityDefine.StandardEffectCount == 1)
                    {
                        CardAbility.FirstAbilityDefine.MainAbilityDefine.ActualEffectPoint = CardAbility.FirstAbilityDefine.MainAbilityDefine.StandardEffectPoint + AbilityEffect;
                    }
                    else
                    {
                        CardAbility.FirstAbilityDefine.MainAbilityDefine.ActualEffectCount = CardAbility.FirstAbilityDefine.MainAbilityDefine.StandardEffectCount + AbilityEffect;
                    }
                    break;
                case AtomicEffectDefine.AbilityEffectEnum.回复:
                    CardAbility.FirstAbilityDefine.MainAbilityDefine.ActualEffectPoint = CardAbility.FirstAbilityDefine.MainAbilityDefine.StandardEffectPoint + AbilityEffect;
                    break;
            }
            if (CardAbility.SecondAbilityDefine.MainAbilityDefine.AbilityEffectType != AtomicEffectDefine.AbilityEffectEnum.未定义)
            {
                switch (CardAbility.SecondAbilityDefine.MainAbilityDefine.AbilityEffectType)
                {
                    case AtomicEffectDefine.AbilityEffectEnum.攻击:
                        if (CardAbility.SecondAbilityDefine.MainAbilityDefine.StandardEffectCount == 1)
                        {
                            CardAbility.SecondAbilityDefine.MainAbilityDefine.ActualEffectPoint = CardAbility.SecondAbilityDefine.MainAbilityDefine.StandardEffectPoint + AbilityEffect;
                        }
                        else
                        {
                            CardAbility.SecondAbilityDefine.MainAbilityDefine.ActualEffectCount = CardAbility.SecondAbilityDefine.MainAbilityDefine.StandardEffectCount + AbilityEffect;
                        }
                        break;
                    case AtomicEffectDefine.AbilityEffectEnum.回复:
                        CardAbility.SecondAbilityDefine.MainAbilityDefine.ActualEffectPoint = CardAbility.SecondAbilityDefine.MainAbilityDefine.StandardEffectPoint + AbilityEffect;
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
            if (CardAbility.SecondAbilityDefine.MainAbilityDefine.AbilityEffectType != AtomicEffectDefine.AbilityEffectEnum.未定义) return true;

            switch (CardAbility.FirstAbilityDefine.MainAbilityDefine.AbilityEffectType)
            {
                case AtomicEffectDefine.AbilityEffectEnum.增益:
                case AtomicEffectDefine.AbilityEffectEnum.变形:
                    if (CardAbility.FirstAbilityDefine.MainAbilityDefine.SelectOpt.EffectTargetSelectDirect == CardUtility.TargetSelectDirectEnum.本方)
                    {
                        return gameManager.MyInfo.BattleField.MinionCount > 0;
                    }
                    if (CardAbility.FirstAbilityDefine.MainAbilityDefine.SelectOpt.EffectTargetSelectDirect == CardUtility.TargetSelectDirectEnum.对方)
                    {
                        return gameManager.YourInfo.BattleField.MinionCount > 0;
                    }
                    if (CardAbility.FirstAbilityDefine.MainAbilityDefine.SelectOpt.EffectTargetSelectDirect == CardUtility.TargetSelectDirectEnum.双方)
                    {
                        return (gameManager.YourInfo.BattleField.MinionCount +
                                gameManager.MyInfo.BattleField.MinionCount) > 0;
                    }
                    break;
                case AtomicEffectDefine.AbilityEffectEnum.召唤:
                    if (CardAbility.FirstAbilityDefine.MainAbilityDefine.SelectOpt.EffectTargetSelectDirect == CardUtility.TargetSelectDirectEnum.本方)
                    {
                        return gameManager.MyInfo.BattleField.MinionCount < Card.Client.BattleFieldInfo.MaxMinionCount;
                    }
                    if (CardAbility.FirstAbilityDefine.MainAbilityDefine.SelectOpt.EffectTargetSelectDirect == CardUtility.TargetSelectDirectEnum.对方)
                    {
                        return gameManager.YourInfo.BattleField.MinionCount < Card.Client.BattleFieldInfo.MaxMinionCount;
                    }
                    if (CardAbility.FirstAbilityDefine.MainAbilityDefine.SelectOpt.EffectTargetSelectDirect == CardUtility.TargetSelectDirectEnum.双方)
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
