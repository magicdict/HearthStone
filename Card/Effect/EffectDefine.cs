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
            /// 降低成本
            /// </summary>
            增益,
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
            /// 奥秘
            /// </summary>
            奥秘,
            /// <summary>
            /// 
            /// </summary>
            未知
        }
        /// <summary>
        /// 法术类型
        /// </summary>
        public AbilityEffectEnum AbilityEffectType;
        /// <summary>
        /// 法术对象选择模式
        /// </summary>
        public CardUtility.TargetSelectModeEnum EffictTargetSelectMode;
        /// <summary>
        /// 法术对象选择角色
        /// </summary>
        public CardUtility.TargetSelectRoleEnum EffectTargetSelectRole;
        /// <summary>
        /// 法术对象选择方向
        /// </summary>
        public CardUtility.TargetSelectDirectEnum EffectTargetSelectDirect;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Boolean IsNeedSelectTarget()
        {
            return EffictTargetSelectMode == CardUtility.TargetSelectModeEnum.指定;
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
        public int EffectCount;
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
            //切记，这里的EffectCount都是1
            switch (singleEffect.AbilityEffectType)
            {
                case AbilityEffectEnum.攻击:
                    Result.AddRange(AttackEffect.RunEffect(singleEffect, game, Pos, Seed));
                    break;
                case AbilityEffectEnum.回复:
                    Result.AddRange(HealthEffect.RunEffect(singleEffect, game, Pos, Seed));
                    break;
                case AbilityEffectEnum.状态:
                    Result.AddRange(StatusEffect.RunEffect(singleEffect, game, Pos, Seed));
                    break;
                case AbilityEffectEnum.召唤:
                    Result.AddRange(SummonEffect.RunEffect(singleEffect, game, Seed));
                    break;
                case AbilityEffectEnum.增益:
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
                case AbilityEffectEnum.奥秘:
                    break;
                default:
                    break;
            }
            return Result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="singleEffect"></param>
        /// <param name="game"></param>
        /// <param name="Pos"></param>
        /// <param name="Seed"></param>
        /// <returns></returns>
        public static List<string> GetTargetList(EffectDefine singleEffect, Client.GameManager game, CardUtility.TargetPosition Pos, int Seed)
        {
            //切记，这里的EffectCount都是1
            List<string> Result = new List<string>();
            switch (singleEffect.EffictTargetSelectMode)
            {
                case CardUtility.TargetSelectModeEnum.随机:
                    Random t = new Random(DateTime.Now.Millisecond + Seed);
                    switch (singleEffect.EffectTargetSelectDirect)
                    {
                        case CardUtility.TargetSelectDirectEnum.本方:
                            switch (singleEffect.EffectTargetSelectRole)
                            {
                                case CardUtility.TargetSelectRoleEnum.随从:
                                    Pos.Postion = t.Next(1, game.MySelf.RoleInfo.BattleField.MinionCount + 1);
                                    Pos.MeOrYou = true;
                                    break;
                                case CardUtility.TargetSelectRoleEnum.所有角色:
                                    Pos.Postion = t.Next(0, game.MySelf.RoleInfo.BattleField.MinionCount + 1);
                                    Pos.MeOrYou = true;
                                    break;
                            }
                            //ME#POS
                            Result.Add(CardUtility.strMe + CardUtility.strSplitMark + Pos.Postion.ToString("D1"));
                            break;
                        case CardUtility.TargetSelectDirectEnum.对方:
                            switch (singleEffect.EffectTargetSelectRole)
                            {
                                case CardUtility.TargetSelectRoleEnum.随从:
                                    Pos.Postion = t.Next(1, game.YourInfo.BattleField.MinionCount + 1);
                                    Pos.MeOrYou = false;
                                    break;
                                case CardUtility.TargetSelectRoleEnum.所有角色:
                                    Pos.Postion = t.Next(0, game.YourInfo.BattleField.MinionCount + 1);
                                    Pos.MeOrYou = false;
                                    break;
                            }
                            //ME#POS
                            Result.Add(CardUtility.strYou + CardUtility.strSplitMark + Pos.Postion.ToString("D1"));
                            break;
                        case CardUtility.TargetSelectDirectEnum.双方:
                            //本方对方
                            int MinionCount;
                            if (t.Next(1, 3) == 1)
                            {
                                Pos.MeOrYou = true;
                                MinionCount = game.MySelf.RoleInfo.BattleField.MinionCount;
                            }
                            else
                            {
                                Pos.MeOrYou = false;
                                MinionCount = game.YourInfo.BattleField.MinionCount;
                            }
                            switch (singleEffect.EffectTargetSelectRole)
                            {
                                case CardUtility.TargetSelectRoleEnum.随从:
                                    Pos.Postion = t.Next(1, MinionCount + 1);
                                    break;
                                case CardUtility.TargetSelectRoleEnum.所有角色:
                                    Pos.Postion = t.Next(0, MinionCount + 1);
                                    break;
                            }
                            //ME#POS
                            Result.Add((Pos.MeOrYou ? CardUtility.strMe : CardUtility.strYou) + CardUtility.strSplitMark + Pos.Postion.ToString("D1"));
                            break;
                        default:
                            break;
                    }
                    break;
                case CardUtility.TargetSelectModeEnum.全体:
                    switch (singleEffect.EffectTargetSelectDirect)
                    {
                        case CardUtility.TargetSelectDirectEnum.本方:
                            switch (singleEffect.EffectTargetSelectRole)
                            {
                                case CardUtility.TargetSelectRoleEnum.随从:
                                    for (int i = 0; i < game.MySelf.RoleInfo.BattleField.MinionCount; i++)
                                    {
                                        Result.Add(CardUtility.strMe + CardUtility.strSplitMark + (i + 1).ToString("D1"));
                                    }
                                    break;
                                case CardUtility.TargetSelectRoleEnum.英雄:
                                    Result.Add(CardUtility.strMe + CardUtility.strSplitMark + 0.ToString("D1"));
                                    break;
                                case CardUtility.TargetSelectRoleEnum.所有角色:
                                    Result.Add(CardUtility.strMe + CardUtility.strSplitMark + 0.ToString("D1"));
                                    for (int i = 0; i < game.MySelf.RoleInfo.BattleField.MinionCount; i++)
                                    {
                                        Result.Add(CardUtility.strMe + CardUtility.strSplitMark + (i + 1).ToString("D1"));
                                    }
                                    break;
                            }
                            break;
                        case CardUtility.TargetSelectDirectEnum.对方:
                            switch (singleEffect.EffectTargetSelectRole)
                            {
                                case CardUtility.TargetSelectRoleEnum.随从:
                                    for (int i = 0; i < game.YourInfo.BattleField.MinionCount; i++)
                                    {
                                        Result.Add(CardUtility.strYou + CardUtility.strSplitMark + (i + 1).ToString("D1"));
                                    }
                                    break;
                                case CardUtility.TargetSelectRoleEnum.英雄:
                                    Result.Add(CardUtility.strYou + CardUtility.strSplitMark + 0.ToString("D1"));
                                    break;
                                case CardUtility.TargetSelectRoleEnum.所有角色:
                                    Result.Add(CardUtility.strYou + CardUtility.strSplitMark + 0.ToString("D1"));
                                    for (int i = 0; i < game.YourInfo.BattleField.MinionCount; i++)
                                    {
                                        Result.Add(CardUtility.strYou + CardUtility.strSplitMark + (i + 1).ToString("D1"));
                                    }
                                    break;
                            }
                            break;
                        case CardUtility.TargetSelectDirectEnum.双方:
                            switch (singleEffect.EffectTargetSelectRole)
                            {
                                case CardUtility.TargetSelectRoleEnum.随从:
                                    for (int i = 0; i < game.MySelf.RoleInfo.BattleField.MinionCount; i++)
                                    {
                                        Result.Add(CardUtility.strMe + CardUtility.strSplitMark + (i + 1).ToString("D1"));
                                    }
                                    for (int i = 0; i < game.YourInfo.BattleField.MinionCount; i++)
                                    {
                                        Result.Add(CardUtility.strYou + CardUtility.strSplitMark + (i + 1).ToString("D1"));
                                    }
                                    break;
                                case CardUtility.TargetSelectRoleEnum.英雄:
                                    Result.Add(CardUtility.strMe + CardUtility.strSplitMark + 0.ToString("D1"));
                                    Result.Add(CardUtility.strYou + CardUtility.strSplitMark + 0.ToString("D1"));
                                    break;
                                case CardUtility.TargetSelectRoleEnum.所有角色:
                                    Result.Add(CardUtility.strMe + CardUtility.strSplitMark + 0.ToString("D1"));
                                    Result.Add(CardUtility.strYou + CardUtility.strSplitMark + 0.ToString("D1"));
                                    for (int i = 0; i < game.MySelf.RoleInfo.BattleField.MinionCount; i++)
                                    {
                                        Result.Add(CardUtility.strMe + CardUtility.strSplitMark + (i + 1).ToString("D1"));
                                    }
                                    for (int i = 0; i < game.YourInfo.BattleField.MinionCount; i++)
                                    {
                                        Result.Add(CardUtility.strYou + CardUtility.strSplitMark + (i + 1).ToString("D1"));
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
                case CardUtility.TargetSelectModeEnum.指定:
                    Result.Add((Pos.MeOrYou ? CardUtility.strMe : CardUtility.strYou) + CardUtility.strSplitMark + Pos.Postion.ToString("D1"));
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
        }
    }
}
