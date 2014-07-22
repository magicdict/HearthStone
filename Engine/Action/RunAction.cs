using Engine.Card;
using Engine.Client;
using Engine.Server;
using Engine.Utility;
using System;
using System.Collections.Generic;

namespace Engine.Action
{
    /// <summary>
    /// 执行
    /// </summary>
    public static class RunAction
    {
        /// <summary>
        /// 开始一个动作
        /// </summary>
        /// <param name="actionStatus">
        /// 在调用这个过程之前，actionStatus里面的本方和对方都应该已经设定好了
        /// 在这里不进行任何的方向判断，默认这个方法是由MyInfo发起的
        /// </param>
        /// <param name="CardSn"></param>
        /// <param name="AIParm">
        ///     AI决定的参数
        ///     HTML交互的时候，可以放置客户端的选项
        /// </param>
        /// <returns></returns>
        public static List<String> StartAction(ActionStatus actionStatus, String CardSn)
        {
            //清除事件池，注意，事件将在动作结束后整体结算
            actionStatus.battleEvenetHandler.事件池.Clear();
            Engine.Card.CardBasicInfo card = Engine.Utility.CardUtility.GetCardInfoBySN(CardSn);
            List<String> ActionCodeLst = new List<string>();
            //未知的异常，卡牌资料缺失
            if (card == null) return ActionCodeLst;
            PublicInfo PlayInfo = actionStatus.AllRole.MyPublicInfo;
            switch (card.卡牌种类)
            {
                case CardBasicInfo.卡牌类型枚举.法术:
                    if (SystemManager.游戏类型 == SystemManager.GameType.HTML版)
                    {
                        actionStatus.ActionName = "USESPELLCARD";
                        UseSpellAction.RunBS(actionStatus, CardSn);
                    }
                    break;
                case CardBasicInfo.卡牌类型枚举.随从:
                    if (SystemManager.游戏类型 == SystemManager.GameType.HTML版)
                    {
                        actionStatus.ActionName = "USEMINIONCARD";
                        UseMinionAction.RunBS(actionStatus, CardSn);
                    }
                    break;
                case CardBasicInfo.卡牌类型枚举.武器:
                    ActionCodeLst.Add(ActionCode.strWeapon + CardUtility.strSplitMark + CardSn);
                    PlayInfo.Weapon = (Engine.Card.WeaponCard)card;
                    break;
                case CardBasicInfo.卡牌类型枚举.奥秘:
                    ActionCodeLst.Add(ActionCode.strSecret + CardUtility.strSplitMark + CardSn);
                    actionStatus.AllRole.MyPrivateInfo.奥秘列表.Add((Engine.Card.SecretCard)card);
                    PlayInfo.SecretCount = actionStatus.AllRole.MyPrivateInfo.奥秘列表.Count;
                    break;
                default:
                    break;
            }
            //随从卡牌的连击效果启动
            if ((card.卡牌种类 != CardBasicInfo.卡牌类型枚举.法术) &&
                PlayInfo.连击状态 && (!String.IsNullOrEmpty(card.连击效果)))
            {
                //初始化 Buff效果等等
                Engine.Card.SpellCard ablity = (Engine.Card.SpellCard)CardUtility.GetCardInfoBySN(card.连击效果);
                if (ablity != null)
                {
                    var ResultArg = ablity.UseSpell(actionStatus);
                    if (ResultArg.Count != 0)
                    {
                        ActionCodeLst.AddRange(ResultArg);
                        //英雄技能等的时候，不算[本方施法] 
                        if (card.原生卡牌)
                            actionStatus.battleEvenetHandler.事件池.Add(new Engine.Utility.CardUtility.全局事件()
                            {
                                触发事件类型 = CardUtility.事件类型枚举.施法,
                                触发位置 = PlayInfo.战场位置
                            });
                    }
                }
            }
            if (ActionCodeLst.Count != 0)
            {
                PlayInfo.连击状态 = true;
                ActionCodeLst.AddRange(actionStatus.battleEvenetHandler.事件处理(actionStatus));
            }
            return ActionCodeLst;
        }
        /// <summary>
        /// 攻击动作
        /// </summary>
        /// <param name="game"></param>
        /// <param name="MyPos">攻击方</param>
        /// <param name="YourPos">被攻击方</param>
        /// <param name="IsMyAction">动作发起方</param>
        /// <returns></returns>
        public static List<String> Fight(ActionStatus game, int MyPos, int YourPos, Boolean IsMyAction)
        {
            game.battleEvenetHandler.事件池.Clear();
            //FIGHT#1#2
            String actionCode = ActionCode.strFight + CardUtility.strSplitMark + MyPos + CardUtility.strSplitMark + YourPos;
            List<String> ActionCodeLst = new List<string>();
            ActionCodeLst.Add(actionCode);
            ActionCodeLst.AddRange(FightHandler.Fight(MyPos, YourPos, game, IsMyAction));
            ActionCodeLst.AddRange(game.battleEvenetHandler.事件处理(game));
            return ActionCodeLst;
        }
    }
}
