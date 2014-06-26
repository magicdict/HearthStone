using Engine.Utility;
using System;

namespace Engine.Server
{
    public static class ActionCode
    {
        #region"常数"
        /// <summary>
        /// 动作类型
        /// </summary>
        public enum ActionType
        {
            /// <summary>
            /// 使用武器
            /// </summary>
            UseWeapon,
            /// <summary>
            /// 使用随从
            /// </summary>
            UseMinion,
            /// <summary>
            /// 使用法术
            /// </summary>
            UseAbility,
            /// <summary>
            /// 使用奥秘
            /// </summary>
            UseSecret,
            /// <summary>
            /// 命中奥秘
            /// </summary>
            HitSecret,
            /// <summary>
            /// 命中事件
            /// </summary>
            HitEvent,
            /// <summary>
            /// 攻击处理
            /// </summary>
            Attack,
            /// <summary>
            /// 状态变化
            /// </summary>
            Status,
            /// <summary>
            /// 武器点数
            /// </summary>
            WeaponPoint,
            /// <summary>
            /// 改变数值
            /// </summary>
            Point,
            /// <summary>
            /// 治疗
            /// </summary>
            Health,
            /// <summary>
            /// 变形（效果）
            /// </summary>
            Transform,
            /// <summary>
            /// 召唤
            /// </summary>
            Summon,
            /// <summary>
            /// 控制
            /// </summary>
            Control,
            /// <summary>
            /// 水晶
            /// </summary>
            Crystal,
            /// <summary>
            /// 卡牌
            /// </summary>
            Card,
            /// <summary>
            /// 结算
            /// </summary>
            Settle,
            /// <summary>
            /// 直接攻击
            /// </summary>
            Fight,
            /// <summary>
            /// 结束TURN
            /// </summary>
            EndTurn,
            /// <summary>
            /// 未知
            /// </summary>
            UnKnown
        }
        /// <summary>
        /// 变形（效果）
        /// </summary>
        public const string strTransform = "TRANSFORM";
        /// <summary>
        /// 召唤
        /// </summary>
        public const string strSummon = "SUMMON";
        /// <summary>
        /// 水晶
        /// </summary>
        public const string strCrystal = "CRYSTAL";
        /// <summary>
        /// 卡牌
        /// </summary>
        public const string strCard = "CARD";
        /// <summary>
        /// 攻击
        /// </summary>
        public const string strAttack = "ATTACK";
        /// <summary>
        /// 治疗
        /// </summary>
        public const string strHealth = "HEALTH";
        /// <summary>
        /// 改变状态
        /// </summary>
        public const string strStatus = "STATUS";
        /// <summary>
        /// 改变数值
        /// </summary>
        public const string strPoint = "POINT";
        /// <summary>
        /// 武器
        /// </summary>
        public const string strWeapon = "WEAPON";
        /// <summary>
        /// 随从
        /// </summary>
        public const string strMinion = "MINION";
        /// <summary>
        /// 法术
        /// </summary>
        public const string strAbility = "ABILITY";
        /// <summary>
        /// 武器
        /// </summary>
        public const string strWeaponPoint = "WEAPONPOINT";
        /// <summary>
        /// 控制
        /// </summary>
        public const string strControl = "CONTROL";
        /// <summary>
        /// 奥秘(埋伏)
        /// </summary>
        public const string strSecret = "SECRET";
        /// <summary>
        /// 奥秘(揭示)
        /// </summary>
        public const string strHitSecret = "HITSECRET";
        /// <summary>
        /// 命中事件
        /// </summary>
        public const string strHitEvent = "HITEVENT";
        /// <summary>
        /// 战斗
        /// </summary>
        public const string strFight = "FIGHT";
        /// <summary>
        /// ENDTURN
        /// </summary>
        public const String strSettle = "SETTLE";
        /// <summary>
        /// ENDTURN
        /// </summary>
        public const String strEndTurn = "ENDTURN";
        /// <summary>
        /// 随从死亡
        /// </summary>
        public const String strDead = "DEAD";

        /// <summary>
        /// 获得类型
        /// </summary>
        public static ActionType GetActionType(String ActionWord)
        {
            ActionType actionType = ActionType.UnKnown;
            //动作
            if (ActionWord.StartsWith(strWeapon + CardUtility.strSplitMark)) actionType = ActionType.UseWeapon;
            if (ActionWord.StartsWith(strMinion + CardUtility.strSplitMark)) actionType = ActionType.UseMinion;
            if (ActionWord.StartsWith(strAbility + CardUtility.strSplitMark)) actionType = ActionType.UseAbility;
            if (ActionWord.StartsWith(strHitSecret + CardUtility.strSplitMark)) actionType = ActionType.HitSecret;
            if (ActionWord.StartsWith(strHitEvent + CardUtility.strSplitMark)) actionType = ActionType.HitEvent;
            if (ActionWord.StartsWith(strFight + CardUtility.strSplitMark)) actionType = ActionType.Fight;
            //法术效果
            if (ActionWord.StartsWith(strTransform + CardUtility.strSplitMark)) actionType = ActionType.Transform;
            if (ActionWord.StartsWith(strAttack + CardUtility.strSplitMark)) actionType = ActionType.Attack;
            if (ActionWord.StartsWith(strStatus + CardUtility.strSplitMark)) actionType = ActionType.Status;
            if (ActionWord.StartsWith(strHealth + CardUtility.strSplitMark)) actionType = ActionType.Health;
            if (ActionWord.StartsWith(strCrystal + CardUtility.strSplitMark)) actionType = ActionType.Crystal;
            if (ActionWord.StartsWith(strSummon + CardUtility.strSplitMark)) actionType = ActionType.Summon;
            if (ActionWord.StartsWith(strCard + CardUtility.strSplitMark)) actionType = ActionType.Card;
            if (ActionWord.StartsWith(strPoint + CardUtility.strSplitMark)) actionType = ActionType.Point;
            if (ActionWord.StartsWith(strControl + CardUtility.strSplitMark)) actionType = ActionType.Control;
            if (ActionWord.StartsWith(strWeaponPoint + CardUtility.strSplitMark)) actionType = ActionType.WeaponPoint;
            //服务器不发送具体奥秘内容
            if (ActionWord.Equals(strSecret)) actionType = ActionType.UseSecret;
            if (ActionWord.Equals(strSettle)) actionType = ActionType.Settle;
            if (ActionWord.Equals(strEndTurn)) actionType = ActionType.EndTurn;
            
            return actionType;
        }

        #endregion

    }
}
