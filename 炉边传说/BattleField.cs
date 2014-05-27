using Card;
using Card.Client;
using Card.Server;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
namespace 炉边传说
{
    public partial class BattleField : Form
    {
        public BattleField()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 游戏控制
        /// </summary>
        public GameManager game;
        /// <summary>
        /// 等待对方行动Timer
        /// </summary>
        private Timer WaitTimer = new Timer();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BattleField_Load(object sender, System.EventArgs e)
        {
            game.GetSelectTarget = SelectPanel;
            game.PickEffect = PickEffect;
            RunAction.GetPutPos = GetPutPos;
            WaitTimer.Interval = 3000;
            WaitTimer.Tick += WaitFor;
            DisplayMyInfo();
            game.IsMyTurn = game.IsFirst;
            for (int i = 0; i < 10; i++)
            {
                Controls.Find("btnHandCard" + (i + 1).ToString(), true)[0].Click += this.btnUseHandCard_Click;
            }
            for (int i = 0; i < 7; i++)
            {
                Controls.Find("btnYou" + (i + 1).ToString(), true)[0].Enabled = false;
                Controls.Find("btnMe" + (i + 1).ToString(), true)[0].Enabled = false;
                Controls.Find("btnMe" + (i + 1).ToString(), true)[0].Click += (x, y) =>
                {
                    //这里千万不能使用 i ,每次 i 都是固定值
                    //pos.Postion = i + 1;
                    int AttackPostion = int.Parse(((Button)x).Name.Substring("btnMe".Length));
                    Fight(AttackPostion);
                };
            }
            btnMyHero.Click += (x, y) =>
            {
                btnMyHero.Enabled = false;
                Fight(0);
            };
            StartNewTurn();
        }
        /// <summary>
        /// 随从进场位置
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        private int GetPutPos(GameManager game)
        {
            var frm = new PutMinion(game);
            frm.ShowDialog();
            return frm.MinionPos;
        }
        /// <summary>
        /// 选择目标
        /// </summary>
        /// <returns></returns>
        private CardUtility.TargetPosition SelectPanel(Card.CardUtility.TargetSelectDirectEnum direct,
                                                       Card.CardUtility.TargetSelectRoleEnum role,
                                                       Boolean 嘲讽限制)
        {
            CardUtility.TargetPosition SelectPos;
            var frm = new TargetSelect(direct, role, game, 嘲讽限制);
            frm.ShowDialog();
            SelectPos = frm.pos;
            return SelectPos;
        }
        /// <summary>
        /// 显示我方状态
        /// </summary>
        private void DisplayMyInfo()
        {
            btnMyHero.Text = game.MySelf.RoleInfo.GetInfo();
            btnYourHero.Text = game.AgainstInfo.GetInfo();
            for (int i = 0; i < BattleFieldInfo.MaxMinionCount; i++)
            {
                var myMinion = game.MySelf.RoleInfo.BattleField.BattleMinions[i];
                if (myMinion != null)
                {
                    Controls.Find("btnMe" + (i + 1).ToString(), true)[0].Text = myMinion.GetInfo();
                    if (myMinion.RemainAttactTimes > 0)
                    {
                        if (game.IsMyTurn) Controls.Find("btnMe" + (i + 1).ToString(), true)[0].Enabled = true;
                    }
                    else
                    {
                        Controls.Find("btnMe" + (i + 1).ToString(), true)[0].Enabled = false;
                    }
                }
                else
                {
                    Controls.Find("btnMe" + (i + 1).ToString(), true)[0].Text = "[无]";
                    Controls.Find("btnMe" + (i + 1).ToString(), true)[0].Enabled = false;
                }
            }
            if (game.MySelf.RoleInfo.RemainAttackCount != 0 && game.MySelf.RoleInfo.Weapon != null && game.MySelf.RoleInfo.Weapon.实际耐久度 > 0)
            {
                btnMyHero.Enabled = true;
            }
            else
            {
                btnMyHero.Enabled = false;
            }
            for (int i = 0; i < BattleFieldInfo.MaxMinionCount; i++)
            {
                if (game.AgainstInfo.BattleField.BattleMinions[i] != null)
                {
                    Controls.Find("btnYou" + (i + 1).ToString(), true)[0].Text = game.AgainstInfo.BattleField.BattleMinions[i].GetInfo();
                }
                else
                {
                    Controls.Find("btnYou" + (i + 1).ToString(), true)[0].Text = "[无]";
                }
            }

            for (int i = 0; i < 10; i++)
            {
                if (i < game.MySelf.handCards.Count)
                {
                    Controls.Find("btnHandCard" + (i + 1).ToString(), true)[0].Text = Card.CardUtility.GetCardNameBySN(game.MySelf.handCards[i]);
                    Controls.Find("btnHandCard" + (i + 1).ToString(), true)[0].Tag = game.MySelf.handCards[i];
                }
                else
                {
                    Controls.Find("btnHandCard" + (i + 1).ToString(), true)[0].Text = "[无]";
                    Controls.Find("btnHandCard" + (i + 1).ToString(), true)[0].Tag = null;
                    Controls.Find("btnHandCard" + (i + 1).ToString(), true)[0].Enabled = false;
                }
            }
        }
        /// <summary>
        /// 新的回合
        /// </summary>
        private void StartNewTurn()
        {
            game.NewTurn();
            if (game.IsMyTurn)
            {
                DisplayMyInfo();
                btnEndTurn.Enabled = true;
                for (int i = 0; i < 10; i++)
                {
                    if (Controls.Find("btnHandCard" + (i + 1).ToString(), true)[0].Tag != null)
                    {
                        Controls.Find("btnHandCard" + (i + 1).ToString(), true)[0].Enabled = true;
                    }
                }
            }
            else
            {
                btnEndTurn.Enabled = false;
                for (int i = 0; i < 10; i++)
                {
                    Controls.Find("btnHandCard" + (i + 1).ToString(), true)[0].Enabled = false;
                }
                for (int i = 0; i < 7; i++)
                {
                    Controls.Find("btnMe" + (i + 1).ToString(), true)[0].Enabled = false;
                }
                WaitTimer.Start();
            }
        }

        /// <summary>
        /// 读取
        /// </summary>
        private void WaitFor(object sender, System.EventArgs e)
        {
            var Actions = Card.Server.ClientUtlity.ReadAction(game.GameId.ToString(GameServer.GameIdFormat));
            if (String.IsNullOrEmpty(Actions)) return;
            var ActionList = Actions.Split("|".ToCharArray());
            foreach (var item in ActionList)
            {
                lstAction.Items.Add("Received:[" + item + "]");
                if (ActionCode.GetActionType(item) != ActionCode.ActionType.EndTurn)
                {
                    lstAction.Items.Clear();
                    ShowMinionInfo("Before:");
                    ProcessAction.Process(item, game);
                    ShowMinionInfo("After ：");
                }
                else
                {
                    WaitTimer.Stop();
                    btnEndTurn.Enabled = true;
                    game.IsMyTurn = true;
                    StartNewTurn();
                    break;
                }
            }
            DisplayMyInfo();
        }

        private void ShowMinionInfo(string title)
        {
            foreach (var item in game.MySelf.RoleInfo.BattleField.ShowMinions())
            {
                lstAction.Items.Add(title + "My:" + item);
            }
            foreach (var item in game.AgainstInfo.BattleField.ShowMinions())
            {
                lstAction.Items.Add(title + "You:" + item);
            }
        }

        /// <summary>
        /// 结束回合
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEndTurn_Click(object sender, System.EventArgs e)
        {
            Card.Server.ClientUtlity.TurnEnd(game.GameId.ToString(GameServer.GameIdFormat));
            game.IsMyTurn = false;
            StartNewTurn();
            WaitTimer.Start();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FirstEffect"></param>
        /// <param name="SecondEffect"></param>
        /// <returns></returns>
        private Boolean PickEffect(String FirstEffect, String SecondEffect)
        {
            Boolean IsFirst = false;
            EffectSelect t = new EffectSelect();
            t.FirstEffect = FirstEffect;
            t.SecondEffect = SecondEffect;
            t.ShowDialog();
            IsFirst = t.IsFirstEffect;
            t = null;
            return IsFirst;
        }

        /// <summary>
        /// 使用手牌
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUseHandCard_Click(object sender, EventArgs e)
        {
            if (((Button)sender).Tag == null) return;
            String CardSn = ((Button)sender).Tag.ToString();
            if (Card.CardUtility.GetCardInfoBySN(CardSn) != null)
            {
                Card.CardBasicInfo card = Card.CardUtility.GetCardInfoBySN(CardSn);
                if (game.MySelf.RoleInfo.crystal.CurrentRemainPoint >= card.ActualCostPoint)
                {
                    game.MySelf.RoleInfo.crystal.CurrentRemainPoint -= card.ActualCostPoint;
                    game.MySelf.handCards.Remove(CardSn);
                    game.MySelf.RoleInfo.HandCardCount = game.MySelf.handCards.Count;
                }
                else
                {
                    MessageBox.Show("水晶不够");
                    return;
                }
                var actionlst = RunAction.StartAction(game, CardSn);
                foreach (var action in actionlst)
                {
                    Card.Server.ClientUtlity.WriteAction(game.GameId.ToString(GameServer.GameIdFormat), action);
                }
            }
            DisplayMyInfo();
        }
        /// <summary>
        /// 攻击
        /// </summary>
        /// <param name="MyPos"></param>
        private void Fight(int MyPos)
        {
            var YourPos = SelectPanel(CardUtility.TargetSelectDirectEnum.对方, CardUtility.TargetSelectRoleEnum.所有角色, true);
            lstAction.Items.Clear();
            //暂时不考虑嘲讽
            ShowMinionInfo("Before:");
            List<String> actionlst = RunAction.Fight(game, MyPos, YourPos.Postion);
            foreach (var action in actionlst)
            {
                lstAction.Items.Add("Send:[" + action + "]");
                Card.Server.ClientUtlity.WriteAction(game.GameId.ToString(GameServer.GameIdFormat), action);
            }
            ShowMinionInfo("After:");
            DisplayMyInfo();
        }
    }
}
