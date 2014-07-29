using Engine.Action;
using Engine.Effect;
using Engine.Utility;
using System;
using System.Collections.Generic;

namespace Engine.Card
{
    /// <summary>
    /// 法术卡牌
    /// </summary>
    [Serializable]
    public class SpellCard : CardBasicInfo
    {
        /// <summary>
        /// 幸运币
        /// </summary>
        public const string SN幸运币 = "A900001";
        /// <summary>
        /// 效果选择类型枚举
        /// </summary>
        public enum 效果选择类型枚举
        {
            /// <summary>
            /// 无需选择
            /// </summary>
            无需选择,
            /// <summary>
            /// 用户主动
            /// </summary>
            主动选择,
            /// <summary>
            /// 自动判定
            /// </summary>
            自动判定
        }
        /// <summary>
        /// 效果选择类型枚举
        /// </summary>
        public 效果选择类型枚举 效果选择类型 = 效果选择类型枚举.无需选择;
        /// <summary>
        /// 效果选择条件
        /// </summary>
        public string 效果选择条件 = string.Empty;
        /// <summary>
        /// 效果回数表达式
        /// </summary>
        public string 效果回数表达式 = string.Empty;
        /// <summary>
        /// 第一效果
        /// </summary>
        public AbilityDefine FirstAbilityDefine = new AbilityDefine();
        /// <summary>
        /// 第二效果
        /// </summary>
        public AbilityDefine SecondAbilityDefine = new AbilityDefine();
        /// <summary>
        /// 效果定义
        /// </summary>
        [Serializable]
        public struct AbilityDefine
        {
            /// <summary>
            /// 描述
            /// </summary>
            public string 描述;
            /// <summary>
            /// 主效果定义
            /// </summary>
            public EffectDefine MainAbilityDefine;
            /// <summary>
            /// 追加效果定义
            /// </summary>
            public EffectDefine AppendAbilityDefine;
            /// <summary>
            /// 追加效果启动条件
            /// </summary>
            public string AppendEffectCondition;
            /// <summary>
            /// 初始化[未被使用]
            /// </summary>
            public void Init()
            {
                MainAbilityDefine = new EffectDefine();
                AppendAbilityDefine = new EffectDefine();
            }
            /// <summary>
            /// 是否需要位置选择
            /// </summary>
            public bool IsNeedTargetSelect()
            {
                return MainAbilityDefine.AbliltyPosPicker.IsNeedTargetSelect() ||
                      (AppendAbilityDefine != null && AppendAbilityDefine.AbliltyPosPicker.IsNeedTargetSelect());
            }
        }
        /// <summary>
        /// 使用法术
        /// </summary>
        /// <param name="game"></param>
        /// <param name="IsMyAction">对象方向转换</param>
        public List<string> UseSpell(ActionStatus game)
        {
            List<string> Result = new List<string>();
            CardUtility.抉择枚举 PickAbilityResult = CardUtility.抉择枚举.第一效果;
            switch (效果选择类型)
            {
                case 效果选择类型枚举.无需选择:
                    break;
                case 效果选择类型枚举.主动选择:
                    PickAbilityResult = ActionStatus.PickEffect(FirstAbilityDefine.描述, SecondAbilityDefine.描述);
                    if (PickAbilityResult == CardUtility.抉择枚举.取消)
                    {
                        return new List<string>();
                    }
                    break;
                case 效果选择类型枚举.自动判定:
                    if (!ExpressHandler.AbilityPickCondition(game, 效果选择条件)) PickAbilityResult = CardUtility.抉择枚举.第二效果;
                    break;
                default:
                    break;
            }
            List<EffectDefine> SingleEffectList = new List<EffectDefine>();
            AbilityDefine ability;
            if (PickAbilityResult == CardUtility.抉择枚举.第一效果)
            {
                ability = FirstAbilityDefine;
            }
            else
            {
                ability = SecondAbilityDefine;
            }
            Result.AddRange(RunAbility(game, ability));
            return Result;
        }
        /// <summary>
        /// 运行法术
        /// </summary>
        /// <param name="game"></param>
        /// <param name="IsMyAction"></param>
        /// <param name="Ability"></param>
        /// <returns></returns>
        public static List<string> RunAbility(ActionStatus game, AbilityDefine Ability)
        {
            List<string> Result = new List<string>();
            //对象选择处理
            if (Ability.MainAbilityDefine.AbliltyPosPicker.IsNeedTargetSelect())
            {
                //如果是BS的话，可以通过game的上下文数据获得位置信息
                if (SystemManager.游戏类型 == SystemManager.GameType.HTML版)
                {
                    //如果是随从卡牌的时候，这里是战吼的动作，战吼的动作的话，前面已经对于指定位置的设置了
                    if (game.ActionName == "USESPELLCARD")
                    {
                        Ability.MainAbilityDefine.AbliltyPosPicker.SelectedPos = CardUtility.指定位置结构体.FromString(game.Interrupt.SessionDic["SPELLPOSITION"]);
                    }
                }
                else
                {
                    Ability.MainAbilityDefine.AbliltyPosPicker.SelectedPos = ActionStatus.GetSelectTarget(Ability.MainAbilityDefine.AbliltyPosPicker);
                }
            }
            //取消处理
            if (Ability.MainAbilityDefine.AbliltyPosPicker.SelectedPos.位置 == Client.BattleFieldInfo.UnknowPos)
            {
                Result.Clear();
                return Result;
            }
            //法术伤害对于攻击型效果的加成
            if (Ability.MainAbilityDefine.效果条件 == CardUtility.strIgnore && Ability.MainAbilityDefine.EffectCount > 1)
            {
                Ability.MainAbilityDefine.EffectCount += game.AllRole.MyPublicInfo.BattleField.AbilityDamagePlus;
            }
            //按照回数执行效果
            for (int cnt = 0; cnt < Ability.MainAbilityDefine.EffectCount; cnt++)
            {
                //系统法术
                switch (Ability.MainAbilityDefine.TrueAtomicEffect.AtomicEffectType)
                {
                    case AtomicEffectDefine.AtomicEffectEnum.卡牌:
                    case AtomicEffectDefine.AtomicEffectEnum.水晶:
                    case AtomicEffectDefine.AtomicEffectEnum.召唤:
                    case AtomicEffectDefine.AtomicEffectEnum.武器:
                        Result.AddRange(RunGameSystemEffect(game, Ability.MainAbilityDefine.TrueAtomicEffect, Ability.MainAbilityDefine.AbliltyPosPicker));
                        break;
                    case AtomicEffectDefine.AtomicEffectEnum.控制:
                        Result.AddRange(ControlEffect.RunEffect(game, Ability.MainAbilityDefine.AbliltyPosPicker.SelectedPos.ToString()));
                        break;
                    case AtomicEffectDefine.AtomicEffectEnum.召回:
                        Result.AddRange(CallBackEffect.RunEffect(game, Ability.MainAbilityDefine.AbliltyPosPicker.SelectedPos.ToString()));
                        break;
                    case AtomicEffectDefine.AtomicEffectEnum.未定义:
                        break;
                    default:
                        Result.AddRange(Effecthandler.RunSingleEffect(Ability.MainAbilityDefine, game, ActionStatus.RandomSeed));
                        break;
                }
                ActionStatus.RandomSeed++;
                //是否每次结算？这里的逻辑需要确认！
                Result.AddRange(ActionStatus.Settle(game));
            }
            //追加条件计算
            if (Ability.AppendAbilityDefine == null || (!ExpressHandler.AppendAbilityCondition(game, Ability)))
            {
                return Result;
            }
            //按照回数执行追加效果
            //继承主效果的选定位置信息
            Ability.AppendAbilityDefine.AbliltyPosPicker.SelectedPos = Ability.MainAbilityDefine.AbliltyPosPicker.SelectedPos;
            for (int cnt = 0; cnt < Ability.AppendAbilityDefine.EffectCount; cnt++)
            {
                //系统法术
                switch (Ability.AppendAbilityDefine.TrueAtomicEffect.AtomicEffectType)
                {
                    case AtomicEffectDefine.AtomicEffectEnum.卡牌:
                    case AtomicEffectDefine.AtomicEffectEnum.水晶:
                    case AtomicEffectDefine.AtomicEffectEnum.召唤:
                    case AtomicEffectDefine.AtomicEffectEnum.武器:
                        Result.AddRange(RunGameSystemEffect(game, Ability.AppendAbilityDefine.TrueAtomicEffect,
                                                                                    Ability.AppendAbilityDefine.AbliltyPosPicker));
                        break;
                    case AtomicEffectDefine.AtomicEffectEnum.控制:
                        Result.AddRange(ControlEffect.RunEffect(game, Ability.AppendAbilityDefine.AbliltyPosPicker.SelectedPos.ToString()));
                        break;
                    case AtomicEffectDefine.AtomicEffectEnum.召回:
                        Result.AddRange(CallBackEffect.RunEffect(game, Ability.AppendAbilityDefine.AbliltyPosPicker.SelectedPos.ToString()));
                        break;
                    case AtomicEffectDefine.AtomicEffectEnum.未定义:
                        break;
                    default:
                        Result.AddRange(Effecthandler.RunSingleEffect(Ability.AppendAbilityDefine, game, ActionStatus.RandomSeed));
                        break;
                }
                ActionStatus.RandomSeed++;
                Result.AddRange(ActionStatus.Settle(game));
            }
            return Result;
        }
        /// <summary>
        /// 针对系统的法术效果
        /// </summary>
        /// <param name="game"></param>
        /// <param name="ConvertPosDirect"></param>
        /// <param name="Ability"></param>
        /// <returns></returns>
        public static List<string> RunGameSystemEffect(ActionStatus game, AtomicEffectDefine effect, CardUtility.位置选择用参数结构体 Option)
        {
            List<string> Result = new List<string>();
            switch (effect.AtomicEffectType)
            {
                case AtomicEffectDefine.AtomicEffectEnum.卡牌:
                    CardEffect CardAtomic = new CardEffect();
                    CardAtomic.GetField(effect.InfoArray);
                    return CardAtomic.RunEffect(game, Option.EffectTargetSelectDirect);
                case AtomicEffectDefine.AtomicEffectEnum.水晶:
                    CrystalEffect CrystalAtomic = new CrystalEffect();
                    CrystalAtomic.GetField(effect.InfoArray);
                    return CrystalAtomic.RunEffect(game, Option.EffectTargetSelectDirect);
                case AtomicEffectDefine.AtomicEffectEnum.武器:
                    WeaponPointEffect WeaponPointAtomic = new WeaponPointEffect();
                    WeaponPointAtomic.GetField(effect.InfoArray);
                    return WeaponPointAtomic.RunEffect(game, Option.EffectTargetSelectDirect);
                case AtomicEffectDefine.AtomicEffectEnum.召唤:
                    SummonEffect SummonAtomic = new SummonEffect();
                    SummonAtomic.GetField(effect.InfoArray);
                    return SummonAtomic.RunEffect(game, Option.EffectTargetSelectDirect);
            }
            return Result;
        }
    }
}
