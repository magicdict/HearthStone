using System;
using System.Collections.Generic;

namespace Card.Effect
{
    [Serializable]
    public class EffectDefine
    {
        /// <summary>
        /// 描述
        /// </summary>
        public String Description = String.Empty;
        /// <summary>
        /// 魔法效果
        /// </summary>
        public enum AbilityEffectEnum
        {
            /// <summary>
            /// 未定义
            /// </summary>
            未定义,
            /// <summary>
            /// 攻击类
            /// </summary>
            攻击,
            /// <summary>
            /// 治疗回复
            /// </summary>
            回复,
            /// <summary>
            /// 改变状态
            /// </summary>
            状态,
            /// <summary>
            /// 召唤
            /// </summary>
            召唤,
            /// <summary>
            /// 改变卡牌点数
            /// </summary>
            点数,
            /// <summary>
            /// 抽牌/弃牌
            /// </summary>
            卡牌,
            /// <summary>
            /// 变形
            /// 变羊，变青蛙
            /// </summary>
            变形,
            /// <summary>
            /// 获得水晶
            /// </summary>
            水晶,
            /// <summary>
            /// 控制权
            /// </summary>
            控制,
            /// <summary>
            /// 奥秘
            /// </summary>
            奥秘,
        }
        /// <summary>
        /// 法术类型
        /// </summary>
        public AbilityEffectEnum AbilityEffectType;

        public CardUtility.SelectOption SelectOpt = new CardUtility.SelectOption();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Boolean IsNeedSelectTarget()
        {
            return SelectOpt.EffictTargetSelectMode == CardUtility.TargetSelectModeEnum.指定;
        }
        /// 攻击的时候：99表示消灭一个单位
        /// 治疗的时候：99表示完全回复一个单位
        /// 抽牌的时候：表示抽牌的数量
        /// <summary>
        /// 效果点数(标准)
        /// </summary>
        public int StandardEffectPoint;
        /// <summary>
        /// 效果点数(实际)
        /// </summary>
        public int ActualEffectPoint;
        /// <summary>
        /// 效果回数
        /// </summary>
        public int StandardEffectCount;
        /// <summary>
        /// 效果回数(实际)
        /// </summary>
        public int ActualEffectCount;
        /// <summary>
        /// 附加信息
        /// </summary>
        public String AddtionInfo;

        /// <summary>
        /// 实施效果
        /// </summary>
        /// <param name="singleEffect">效果</param>
        /// <param name="Field"></param>
        /// <param name="Pos">指定对象</param>
        /// <returns></returns>
        public static List<String> RunSingleEffect(EffectDefine singleEffect, Card.Client.GameManager game, Card.CardUtility.TargetPosition Pos, int Seed)
        {
            List<String> Result = new List<string>();
            List<String> PosList = Card.Effect.EffectDefine.GetTargetList(singleEffect, game, Pos, Seed);
            //切记，这里的EffectCount都是1
            switch (singleEffect.AbilityEffectType)
            {
                case AbilityEffectEnum.攻击:
                    Result.AddRange(AttackEffect.RunEffect(singleEffect, game, PosList));
                    break;
                case AbilityEffectEnum.回复:
                    Result.AddRange(HealthEffect.RunEffect(singleEffect, game, PosList));
                    break;
                case AbilityEffectEnum.状态:
                    Result.AddRange(StatusEffect.RunEffect(singleEffect, game, PosList));
                    break;
                case AbilityEffectEnum.召唤:
                    Result.AddRange(SummonEffect.RunEffect(singleEffect, game, Seed));
                    break;
                case AbilityEffectEnum.点数:
                    Result.AddRange(PointEffect.RunEffect(singleEffect, game, PosList));
                    break;
                case AbilityEffectEnum.卡牌:
                    Result.AddRange(CardEffect.RunEffect(singleEffect, game));
                    break;
                case AbilityEffectEnum.变形:
                    Result.AddRange(TransformEffect.RunEffect(singleEffect, game, Pos));
                    break;
                case AbilityEffectEnum.水晶:
                    Result.AddRange(CrystalEffect.RunEffect(singleEffect, game));
                    break;
                case AbilityEffectEnum.控制:
                    Result.AddRange(ControlEffect.RunEffect(singleEffect, game, PosList));
                    break;
                case AbilityEffectEnum.奥秘:
                    break;
                default:
                    break;
            }
            return Result;
        }
        /// <summary>
        /// 施法对象列表
        /// </summary>
        /// <param name="singleEffect"></param>
        /// <param name="game"></param>
        /// <param name="PosInfo"></param>
        /// <param name="Seed"></param>
        /// <returns></returns>
        public static List<string> GetTargetList(EffectDefine singleEffect, Client.GameManager game, CardUtility.TargetPosition PosInfo, int Seed)
        {
            //切记，这里的EffectCount都是1
            List<string> Result = new List<string>();
            switch (singleEffect.SelectOpt.EffictTargetSelectMode)
            {
                case CardUtility.TargetSelectModeEnum.随机:
                    Random t = new Random(DateTime.Now.Millisecond + Seed);
                    switch (singleEffect.SelectOpt.EffectTargetSelectDirect)
                    {
                        case CardUtility.TargetSelectDirectEnum.本方:
                            switch (singleEffect.SelectOpt.EffectTargetSelectRole)
                            {
                                case CardUtility.TargetSelectRoleEnum.随从:
                                    PosInfo.Postion = t.Next(1, game.MySelf.RoleInfo.BattleField.MinionCount + 1);
                                    PosInfo.MeOrYou = true;
                                    break;
                                case CardUtility.TargetSelectRoleEnum.所有角色:
                                    PosInfo.Postion = t.Next(Client.BattleFieldInfo.HeroPos, game.MySelf.RoleInfo.BattleField.MinionCount + 1);
                                    PosInfo.MeOrYou = true;
                                    break;
                            }
                            //ME#POS
                            Result.Add(CardUtility.strMe + CardUtility.strSplitMark + PosInfo.Postion.ToString("D1"));
                            break;
                        case CardUtility.TargetSelectDirectEnum.对方:
                            switch (singleEffect.SelectOpt.EffectTargetSelectRole)
                            {
                                case CardUtility.TargetSelectRoleEnum.随从:
                                    PosInfo.Postion = t.Next(1, game.YourInfo.BattleField.MinionCount + 1);
                                    PosInfo.MeOrYou = false;
                                    break;
                                case CardUtility.TargetSelectRoleEnum.所有角色:
                                    PosInfo.Postion = t.Next(Client.BattleFieldInfo.HeroPos, game.YourInfo.BattleField.MinionCount + 1);
                                    PosInfo.MeOrYou = false;
                                    break;
                            }
                            //ME#POS
                            Result.Add(CardUtility.strYou + CardUtility.strSplitMark + PosInfo.Postion.ToString("D1"));
                            break;
                        case CardUtility.TargetSelectDirectEnum.双方:
                            //本方对方
                            int MinionCount;
                            if (t.Next(1, 3) == 1)
                            {
                                PosInfo.MeOrYou = true;
                                MinionCount = game.MySelf.RoleInfo.BattleField.MinionCount;
                            }
                            else
                            {
                                PosInfo.MeOrYou = false;
                                MinionCount = game.YourInfo.BattleField.MinionCount;
                            }
                            switch (singleEffect.SelectOpt.EffectTargetSelectRole)
                            {
                                case CardUtility.TargetSelectRoleEnum.随从:
                                    PosInfo.Postion = t.Next(1, MinionCount + 1);
                                    break;
                                case CardUtility.TargetSelectRoleEnum.所有角色:
                                    PosInfo.Postion = t.Next(Client.BattleFieldInfo.HeroPos, MinionCount + 1);
                                    break;
                            }
                            //ME#POS
                            Result.Add((PosInfo.MeOrYou ? CardUtility.strMe : CardUtility.strYou) + CardUtility.strSplitMark + PosInfo.Postion.ToString("D1"));
                            break;
                        default:
                            break;
                    }
                    break;
                case CardUtility.TargetSelectModeEnum.全体:
                    switch (singleEffect.SelectOpt.EffectTargetSelectDirect)
                    {
                        case CardUtility.TargetSelectDirectEnum.本方:
                            switch (singleEffect.SelectOpt.EffectTargetSelectRole)
                            {
                                case CardUtility.TargetSelectRoleEnum.随从:
                                    for (int i = 0; i < game.MySelf.RoleInfo.BattleField.MinionCount; i++)
                                    {
                                        if (Card.CardUtility.符合种族条件(game.MySelf.RoleInfo.BattleField.BattleMinions[i], singleEffect.SelectOpt)) 
                                        Result.Add(CardUtility.strMe + CardUtility.strSplitMark + (i + 1).ToString("D1"));
                                    }
                                    break;
                                case CardUtility.TargetSelectRoleEnum.英雄:
                                    Result.Add(CardUtility.strMe + CardUtility.strSplitMark + Client.BattleFieldInfo.HeroPos.ToString("D1"));
                                    break;
                                case CardUtility.TargetSelectRoleEnum.所有角色:
                                    Result.Add(CardUtility.strMe + CardUtility.strSplitMark + Client.BattleFieldInfo.HeroPos.ToString("D1"));
                                    for (int i = 0; i < game.MySelf.RoleInfo.BattleField.MinionCount; i++)
                                    {
                                        if (Card.CardUtility.符合种族条件(game.MySelf.RoleInfo.BattleField.BattleMinions[i], singleEffect.SelectOpt)) 
                                        Result.Add(CardUtility.strMe + CardUtility.strSplitMark + (i + 1).ToString("D1"));
                                    }
                                    break;
                            }
                            break;
                        case CardUtility.TargetSelectDirectEnum.对方:
                            switch (singleEffect.SelectOpt.EffectTargetSelectRole)
                            {
                                case CardUtility.TargetSelectRoleEnum.随从:
                                    for (int i = 0; i < game.YourInfo.BattleField.MinionCount; i++)
                                    {
                                        if (Card.CardUtility.符合种族条件(game.YourInfo.BattleField.BattleMinions[i], singleEffect.SelectOpt)) 
                                        Result.Add(CardUtility.strYou + CardUtility.strSplitMark + (i + 1).ToString("D1"));
                                    }
                                    break;
                                case CardUtility.TargetSelectRoleEnum.英雄:
                                    Result.Add(CardUtility.strYou + CardUtility.strSplitMark + Client.BattleFieldInfo.HeroPos.ToString("D1"));
                                    break;
                                case CardUtility.TargetSelectRoleEnum.所有角色:
                                    Result.Add(CardUtility.strYou + CardUtility.strSplitMark + Client.BattleFieldInfo.HeroPos.ToString("D1"));
                                    for (int i = 0; i < game.YourInfo.BattleField.MinionCount; i++)
                                    {
                                        if (Card.CardUtility.符合种族条件(game.YourInfo.BattleField.BattleMinions[i], singleEffect.SelectOpt)) 
                                        Result.Add(CardUtility.strYou + CardUtility.strSplitMark + (i + 1).ToString("D1"));
                                    }
                                    break;
                            }
                            break;
                        case CardUtility.TargetSelectDirectEnum.双方:
                            switch (singleEffect.SelectOpt.EffectTargetSelectRole)
                            {
                                case CardUtility.TargetSelectRoleEnum.随从:
                                    for (int i = 0; i < game.MySelf.RoleInfo.BattleField.MinionCount; i++)
                                    {
                                        if (Card.CardUtility.符合种族条件(game.MySelf.RoleInfo.BattleField.BattleMinions[i], singleEffect.SelectOpt)) 
                                        Result.Add(CardUtility.strMe + CardUtility.strSplitMark + (i + 1).ToString("D1"));
                                    }
                                    for (int i = 0; i < game.YourInfo.BattleField.MinionCount; i++)
                                    {
                                        if (Card.CardUtility.符合种族条件(game.YourInfo.BattleField.BattleMinions[i], singleEffect.SelectOpt)) 
                                        Result.Add(CardUtility.strYou + CardUtility.strSplitMark + (i + 1).ToString("D1"));
                                    }
                                    break;
                                case CardUtility.TargetSelectRoleEnum.英雄:
                                    Result.Add(CardUtility.strMe + CardUtility.strSplitMark + Client.BattleFieldInfo.HeroPos.ToString("D1"));
                                    Result.Add(CardUtility.strYou + CardUtility.strSplitMark + Client.BattleFieldInfo.HeroPos.ToString("D1"));
                                    break;
                                case CardUtility.TargetSelectRoleEnum.所有角色:
                                    Result.Add(CardUtility.strMe + CardUtility.strSplitMark + Client.BattleFieldInfo.HeroPos.ToString("D1"));
                                    Result.Add(CardUtility.strYou + CardUtility.strSplitMark + Client.BattleFieldInfo.HeroPos.ToString("D1"));
                                    for (int i = 0; i < game.MySelf.RoleInfo.BattleField.MinionCount; i++)
                                    {
                                        if (Card.CardUtility.符合种族条件(game.MySelf.RoleInfo.BattleField.BattleMinions[i], singleEffect.SelectOpt)) 
                                        Result.Add(CardUtility.strMe + CardUtility.strSplitMark + (i + 1).ToString("D1"));
                                    }
                                    for (int i = 0; i < game.YourInfo.BattleField.MinionCount; i++)
                                    {
                                        if (Card.CardUtility.符合种族条件(game.YourInfo.BattleField.BattleMinions[i], singleEffect.SelectOpt)) 
                                        Result.Add(CardUtility.strYou + CardUtility.strSplitMark + (i + 1).ToString("D1"));
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
                case CardUtility.TargetSelectModeEnum.指定:
                case CardUtility.TargetSelectModeEnum.继承:
                    Result.Add((PosInfo.MeOrYou ? CardUtility.strMe : CardUtility.strYou) + CardUtility.strSplitMark + PosInfo.Postion.ToString("D1"));
                    break;
                case CardUtility.TargetSelectModeEnum.不用选择:
                    if (singleEffect.SelectOpt.EffectTargetSelectRole == CardUtility.TargetSelectRoleEnum.英雄)
                    {
                        switch (singleEffect.SelectOpt.EffectTargetSelectDirect)
                        {
                            case CardUtility.TargetSelectDirectEnum.本方:
                                Result.Add(CardUtility.strMe + CardUtility.strSplitMark + Client.BattleFieldInfo.HeroPos.ToString("D1"));
                                break;
                            case CardUtility.TargetSelectDirectEnum.对方:
                                Result.Add(CardUtility.strYou + CardUtility.strSplitMark + Client.BattleFieldInfo.HeroPos.ToString("D1"));
                                break;
                            case CardUtility.TargetSelectDirectEnum.双方:
                                Result.Add(CardUtility.strMe + CardUtility.strSplitMark + Client.BattleFieldInfo.HeroPos.ToString("D1"));
                                Result.Add(CardUtility.strYou + CardUtility.strSplitMark + Client.BattleFieldInfo.HeroPos.ToString("D1"));
                                break;
                            default:
                                break;
                        }
                    }
                    break;
            }
            return Result;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            ActualEffectPoint = StandardEffectPoint;
            ActualEffectCount = StandardEffectCount;
        }
    }
}
