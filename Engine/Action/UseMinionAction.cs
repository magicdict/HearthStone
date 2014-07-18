using Engine.Card;
using Engine.Client;
using Engine.Server;
using Engine.Utility;
using System.Collections.Generic;

namespace Engine.Action
{
    public static class UseMinionAction
    {
        #region "CS架构"
        /// <summary>
        /// 获目标位置
        /// </summary>
        public static CardUtility.delegateGetMinionPos GetMinionPos;
        /// <summary>
        /// 随从卡牌
        /// </summary>
        /// <param name="game"></param>
        /// <param name="CardSn"></param>
        /// <param name="AIParm"></param>
        /// <param name="card"></param>
        /// <param name="ActionCodeLst"></param>
        /// <param name="PlayInfo"></param>
        public static void RunCS(ActionStatus game,
                               string CardSn,
                               CardBasicInfo card,
                               List<string> ActionCodeLst,
                               PublicInfo PlayInfo)
        {
            int MinionPos = 1;
            MinionCard minion = null;
            //Step1
            if (PlayInfo.BattleField.MinionCount != 0)
            {
                if (game.Interrupt.SessionData == null)
                {
                    MinionPos = GetMinionPos(game.AllRole.MyPublicInfo.BattleField);
                }
                else
                {
                    MinionPos = int.Parse(game.Interrupt.SessionDic[""]);
                }
            }
            //Step2
            if (MinionPos != -1)
            {
                ActionCodeLst.Add(ActionCode.strMinion + CardUtility.strSplitMark + CardSn + CardUtility.strSplitMark + MinionPos.ToString("D1"));
                minion = (MinionCard)card;
                //初始化
                minion.初始化();
                //必须在放入之前做得原因是，被放入的随从不能被触发这个事件
                game.battleEvenetHandler.事件池.Add(new CardUtility.全局事件()
                {
                    触发事件类型 = CardUtility.事件类型枚举.召唤,
                    触发位置 = new CardUtility.指定位置结构体()
                    {
                        位置 = MinionPos,
                        本方对方标识 = true
                    }
                });
            }
            //Step3
            if (minion != null)
            {
                UseSpellAction.RunBS(game, CardSn);
                PlayInfo.BattleField.ResetBuff();
            }
        }
        #endregion

        #region "BS架构"
        /// <summary>
        /// 随从卡牌
        /// </summary>
        /// <param name="game"></param>
        /// <param name="CardSn"></param>
        public static void RunBS(ActionStatus game, string CardSn)
        {
            int MinionPos = -1;
            MinionCard minion = minion = (MinionCard)CardUtility.GetCardInfoBySN(CardSn);
            //Step1
            if (game.Interrupt.Step == 1)
            {
                if (game.AllRole.MyPublicInfo.BattleField.MinionCount != 0)
                {
                    game.Interrupt.Step = 2;
                    game.Interrupt.ActionName = "MINIONPOSITION";
                    return;
                }
                else
                {
                    game.Interrupt.SessionData = "MINIONPOSITION:1|";
                    //game.Interrupt.SessionDic.Add("MINIONPOSITION", "1");
                }
                MinionPos = 1;
                game.Interrupt.Step = 2;
            }
            //Step2
            if (game.Interrupt.Step == 2)
            {
                if (MinionPos == -1) MinionPos = int.Parse(game.Interrupt.SessionDic["MINIONPOSITION"]);
                //初始化
                minion.初始化();
                //随从入场
                game.AllRole.MyPublicInfo.BattleField.PutToBattle(MinionPos, minion);
                //必须在放入之前做得原因是，被放入的随从不能被触发这个事件
                game.battleEvenetHandler.事件池.Add(new CardUtility.全局事件()
                {
                    触发事件类型 = CardUtility.事件类型枚举.召唤,
                    触发位置 = new CardUtility.指定位置结构体()
                    {
                        位置 = MinionPos,
                        本方对方标识 = game.IsHost
                    }
                });
                game.Interrupt.Step = 3;
            }

            //Step3
            if (game.Interrupt.Step == 3 && !string.IsNullOrEmpty(minion.战吼效果))
            {
                SpellCard spell = (SpellCard)CardUtility.GetCardInfoBySN(minion.战吼效果);
                game.Interrupt.Step = 4;
                if (spell.FirstAbilityDefine.IsNeedTargetSelect())
                {
                    SelectUtility.SetTargetSelectEnable(spell.FirstAbilityDefine.MainAbilityDefine.AbliltyPosPicker, game);
                    game.Interrupt.ExternalInfo = SelectUtility.GetTargetListString(game);
                    game.Interrupt.ActionName = "BATTLECRYPOSITION";
                    return;
                }
            }
            if (game.Interrupt.Step == 4)
            {
                //法术位置信息包含在SessionData中，传递下去
                //这种类型的战吼，直接转换为施法
                //这里约定：战吼是无需抉择的
                game.Interrupt.ActionName = "RUNBATTLECRY";
                game.Interrupt.Step = 1;
                UseSpellAction.RunBS(game, minion.战吼效果);
            }
            game.Interrupt.Step = 99;
            game.Interrupt.ActionName = CardUtility.strOK;
        }
        #endregion

    }
}
