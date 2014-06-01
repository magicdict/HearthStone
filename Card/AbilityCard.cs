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
                    if (CardAbility.FirstAbilityDefine.EffectTargetSelectDirect == CardUtility.TargetSelectDirectEnum.本方)
                    {
                        return gameManager.MySelf.RoleInfo.BattleField.MinionCount > 0;
                    }
                    if (CardAbility.FirstAbilityDefine.EffectTargetSelectDirect == CardUtility.TargetSelectDirectEnum.对方)
                    {
                        return gameManager.YourInfo.BattleField.MinionCount > 0;
                    }
                    if (CardAbility.FirstAbilityDefine.EffectTargetSelectDirect == CardUtility.TargetSelectDirectEnum.双方)
                    {
                        return (gameManager.YourInfo.BattleField.MinionCount +
                                gameManager.MySelf.RoleInfo.BattleField.MinionCount) >0 ;
                    }
                    break;
                case EffectDefine.AbilityEffectEnum.召唤:
                    if (CardAbility.FirstAbilityDefine.EffectTargetSelectDirect == CardUtility.TargetSelectDirectEnum.本方)
                    {
                        return gameManager.MySelf.RoleInfo.BattleField.MinionCount < Card.Client.BattleFieldInfo.MaxMinionCount;
                    }
                    if (CardAbility.FirstAbilityDefine.EffectTargetSelectDirect == CardUtility.TargetSelectDirectEnum.对方)
                    {
                        return gameManager.YourInfo.BattleField.MinionCount < Card.Client.BattleFieldInfo.MaxMinionCount;
                    }
                    if (CardAbility.FirstAbilityDefine.EffectTargetSelectDirect == CardUtility.TargetSelectDirectEnum.双方)
                    {
                        return (gameManager.YourInfo.BattleField.MinionCount < Card.Client.BattleFieldInfo.MaxMinionCount) &&
                               (gameManager.MySelf.RoleInfo.BattleField.MinionCount < Card.Client.BattleFieldInfo.MaxMinionCount);
                    }
                    break;
            }
            return true;
        }
    }
}
