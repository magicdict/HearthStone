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
        /// 间距
        /// </summary>
        int Megrate = 3;
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
            game.IsMyTurn = game.IsFirst;
            for (int i = 0; i < 10; i++)
            {
                ((ctlHandCard)Controls.Find("btnHandCard" + (i + 1).ToString(), true)[0]).UseClick += this.btnUseHandCard_Click;
            }
            for (int i = 0; i < 7; i++)
            {
                ((ctlCard)Controls.Find("btnYou" + (i + 1).ToString(), true)[0]).CanAttack = false;
                ((ctlCard)Controls.Find("btnMe" + (i + 1).ToString(), true)[0]).CanAttack = false;
                ((ctlCard)Controls.Find("btnMe" + (i + 1).ToString(), true)[0]).FightClick += (x, y) =>
                {
                    //这里千万不能使用 i ,每次 i 都是固定值
                    //pos.Postion = i + 1;
                    int AttackPostion = int.Parse(((Button)x).Parent.Name.Substring("btnMe".Length));
                    Fight(AttackPostion);
                };
            }
            btnYourHero.Enabled = false;
            btnMyHero.Enabled = false;

            btnYourWeapon.Enabled = false;
            btnMyWeapon.Click += (x, y) =>
            {
                btnMyWeapon.Enabled = false;
                Fight(0);
            };

            btnYourHeroAblity.Enabled = false;
            btnMyHeroAblity.Enabled = false;
            btnMyHeroAblity.Tag = game.MySelf.RoleInfo.HeroAbility;
            btnMyHeroAblity.Click += btnUseHandCard_Click;
            StartNewTurn();
            DisplayMyInfo();
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
        private CardUtility.TargetPosition SelectPanel(CardUtility.SelectOption SelectOpt, Boolean 嘲讽限制)
        {
            var frm = new TargetSelect(SelectOpt, game, 嘲讽限制);
            frm.ShowDialog();
            var SelectPos = frm.pos;
            return SelectPos;
        }
        /// <summary>
        /// 显示我方状态
        /// </summary>
        private void DisplayMyInfo()
        {
            int LeftPos = (this.Width - (btnMyHero.Width + btnMyHeroAblity.Width + btnMyWeapon.Width + 2 * Megrate)) / 2;
            //武器
            btnMyWeapon.Left = LeftPos;
            if (game.MySelf.RoleInfo.Weapon != null)
            {
                btnMyWeapon.Weapon = game.MySelf.RoleInfo.Weapon;
            }
            else
            {
                btnMyWeapon.Clear();
            }
            btnYourWeapon.Left = LeftPos;
            if (game.YourInfo.Weapon != null)
            {
                btnYourWeapon.Weapon = game.YourInfo.Weapon;
            }
            else
            {
                btnYourWeapon.Clear();
            }
            LeftPos += (btnMyWeapon.Width + Megrate);
            //没有使用过，有武器，武器耐久度不为零
            if (game.IsWeaponEnable())
            {
                btnMyWeapon.Enabled = true;
            }
            else
            {
                btnMyWeapon.Enabled = false;
            }

            btnMyHero.Hero = game.MySelf.RoleInfo;
            btnMyHero.Left = LeftPos;
            btnYourHero.Hero = game.YourInfo;
            btnYourHero.Left = LeftPos;

            LeftPos += btnMyHero.Width + Megrate;


            //没有使用过，能够使用
            if (game.IsHeroAblityEnable())
            {
                btnMyHeroAblity.Enabled = true;
            }
            else
            {
                btnMyHeroAblity.Enabled = false;
            }
            btnMyHeroAblity.Left = LeftPos;
            btnYourHeroAblity.Left = LeftPos;

            MyCrystalBar.CrystalInfo = game.MySelf.RoleInfo.crystal;
            YourCrystalBar.CrystalInfo = game.YourInfo.crystal;


            LeftPos = (this.Width - (game.MySelf.RoleInfo.BattleField.MinionCount * btnMe1.Width +
                      (game.MySelf.RoleInfo.BattleField.MinionCount - 1) * Megrate)) / 2;
            for (int i = 0; i < BattleFieldInfo.MaxMinionCount; i++)
            {
                var myMinion = game.MySelf.RoleInfo.BattleField.BattleMinions[i];
                if (myMinion != null)
                {
                    Controls.Find("btnMe" + (i + 1).ToString(), true)[0].Visible = true;
                    ((ctlCard)Controls.Find("btnMe" + (i + 1).ToString(), true)[0]).CardInfo = myMinion;
                    Controls.Find("btnMe" + (i + 1).ToString(), true)[0].Left = LeftPos;
                    LeftPos += btnMe1.Width + Megrate;
                    if (myMinion.CanAttack())
                    {
                        if (game.IsMyTurn) ((ctlCard)Controls.Find("btnMe" + (i + 1).ToString(), true)[0]).CanAttack = true;
                    }
                    else
                    {
                        ((ctlCard)Controls.Find("btnMe" + (i + 1).ToString(), true)[0]).CanAttack = false;
                    }
                }
                else
                {
                    Controls.Find("btnMe" + (i + 1).ToString(), true)[0].Visible = false;
                }
            }
            LeftPos = (this.Width - (game.YourInfo.BattleField.MinionCount * btnYou1.Width +
                      (game.YourInfo.BattleField.MinionCount - 1) * Megrate)) / 2;
            for (int i = 0; i < BattleFieldInfo.MaxMinionCount; i++)
            {
                if (game.YourInfo.BattleField.BattleMinions[i] != null)
                {
                    Controls.Find("btnYou" + (i + 1).ToString(), true)[0].Visible = true;
                    ((ctlCard)Controls.Find("btnYou" + (i + 1).ToString(), true)[0]).CanAttack = false;
                    ((ctlCard)Controls.Find("btnYou" + (i + 1).ToString(), true)[0]).CardInfo = game.YourInfo.BattleField.BattleMinions[i];
                    Controls.Find("btnYou" + (i + 1).ToString(), true)[0].Left = LeftPos;
                    LeftPos += btnYou1.Width + Megrate;
                }
                else
                {
                    Controls.Find("btnYou" + (i + 1).ToString(), true)[0].Visible = false;
                }
            }
            LeftPos = (this.Width - (game.MySelf.handCards.Count * btnHandCard1.Width + (game.MySelf.handCards.Count - 1) * Megrate)) / 2;
            for (int i = 0; i < 10; i++)
            {
                if (i < game.MySelf.handCards.Count)
                {
                    ((ctlHandCard)Controls.Find("btnHandCard" + (i + 1).ToString(), true)[0]).HandCard = game.MySelf.handCards[i];
                    Controls.Find("btnHandCard" + (i + 1).ToString(), true)[0].Tag = game.MySelf.handCards[i];
                    if (game.IsMyTurn) Controls.Find("btnHandCard" + (i + 1).ToString(), true)[0].Enabled = true;
                    Controls.Find("btnHandCard" + (i + 1).ToString(), true)[0].Visible = true;
                    Controls.Find("btnHandCard" + (i + 1).ToString(), true)[0].Left = LeftPos;
                    LeftPos += btnHandCard1.Width + Megrate;
                }
                else
                {
                    Controls.Find("btnHandCard" + (i + 1).ToString(), true)[0].Text = "[无]";
                    Controls.Find("btnHandCard" + (i + 1).ToString(), true)[0].Tag = null;
                    Controls.Find("btnHandCard" + (i + 1).ToString(), true)[0].Visible = false;
                }
            }
            //胜负判定
            if (game.MySelf.RoleInfo.HealthPoint <= 0 && game.YourInfo.HealthPoint <= 0)
            {
                MessageBox.Show("Draw Game");
                WaitTimer.Stop();
                this.Close();
            }
            else
            {
                if (game.MySelf.RoleInfo.HealthPoint <= 0)
                {
                    MessageBox.Show("You Lose");
                    WaitTimer.Stop();
                    this.Close();
                }
                if (game.YourInfo.HealthPoint <= 0)
                {
                    MessageBox.Show("You Win");
                    WaitTimer.Stop();
                    this.Close();
                }
            }
        }
        /// <summary>
        /// 结束回合
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEndTurn_Click(object sender, System.EventArgs e)
        {
            var ActionLst = game.TurnEnd();
            if (ActionLst.Count != 0) Card.Client.ClientRequest.WriteAction(game.GameId.ToString(GameServer.GameIdFormat), ActionLst);
            //结束回合
            Card.Client.ClientRequest.TurnEnd(game.GameId.ToString(GameServer.GameIdFormat));
            game.IsMyTurn = false;
            StartNewTurn();
            WaitTimer.Start();
        }
        /// <summary>
        /// 新的回合
        /// </summary>
        private void StartNewTurn()
        {
            game.TurnStart();
            if (game.IsMyTurn)
            {
                //回合开始效果
                List<String> ActionLst = new List<string>();
                for (int i = 0; i < game.MySelf.RoleInfo.BattleField.MinionCount; i++)
                {
                    if (game.MySelf.RoleInfo.BattleField.BattleMinions[i] != null)
                    {
                        ActionLst.AddRange(game.MySelf.RoleInfo.BattleField.BattleMinions[i].回合开始(game));
                    }
                }
                if (ActionLst.Count != 0) Card.Client.ClientRequest.WriteAction(game.GameId.ToString(GameServer.GameIdFormat), ActionLst);
                //按钮可用性设定
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
                    ((ctlCard)Controls.Find("btnMe" + (i + 1).ToString(), true)[0]).CanAttack = false;
                }
                WaitTimer.Start();
            }
            //刷新双方状态
            DisplayMyInfo();
        }
        /// <summary>
        /// 读取
        /// </summary>
        private void WaitFor(object sender, System.EventArgs e)
        {
            var Actions = Card.Client.ClientRequest.ReadAction(game.GameId.ToString(GameServer.GameIdFormat));
            if (String.IsNullOrEmpty(Actions)) return;
            var ActionList = Actions.Split(Card.CardUtility.strSplitArrayMark.ToCharArray());
            foreach (var item in ActionList)
            {
                //lstAction.Items.Add("Received:[" + item + "]");
                if (ActionCode.GetActionType(item) != ActionCode.ActionType.EndTurn)
                {
                    //lstAction.Items.Clear();
                    //ShowMinionInfo("Before:");
                    ProcessAction.Process(item, game);
                    //ShowMinionInfo("After ：");
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
        /// <summary>
        /// LOG用
        /// </summary>
        /// <param name="title"></param>
        private void ShowMinionInfo(string title)
        {
            foreach (var item in game.MySelf.RoleInfo.BattleField.ShowMinions())
            {
                //lstAction.Items.Add(title + "My:" + item);
            }
            foreach (var item in game.YourInfo.BattleField.ShowMinions())
            {
                //lstAction.Items.Add(title + "You:" + item);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FirstEffect"></param>
        /// <param name="SecondEffect"></param>
        /// <returns></returns>
        private Card.CardUtility.PickEffect PickEffect(String FirstEffect, String SecondEffect)
        {
            EffectSelect frm = new EffectSelect();
            frm.FirstEffect = FirstEffect;
            frm.SecondEffect = SecondEffect;
            frm.ShowDialog();
            return frm.IsFirstEffect;
        }

        /// <summary>
        /// 使用手牌
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUseHandCard_Click(object sender, EventArgs e)
        {
            CardBasicInfo card;
            if ((sender.GetType()) == typeof(Button))
            {
                card = (CardBasicInfo)((ctlHandCard)(((Button)sender).Parent)).Tag;
            }
            else
            {
                card = (CardBasicInfo)(((ctlHeroAbility)sender).Tag);
            }
            var msg = game.CheckCondition(card);
            if (!String.IsNullOrEmpty(msg))
            {
                MessageBox.Show(msg);
                return;
            }
            var actionlst = RunAction.StartAction(game, card.SN);
            if (actionlst.Count != 0)
            {
                if ((sender.GetType()) == typeof(Button))
                {
                    game.MySelf.RoleInfo.crystal.CurrentRemainPoint -= card.ActualCostPoint;
                    game.RemoveUsedCard(card.SN);
                }
                else
                {
                    game.MySelf.RoleInfo.crystal.CurrentRemainPoint -= card.StandardCostPoint;
                    game.MySelf.RoleInfo.IsUsedHeroAbility = true;
                }
                actionlst.Add(ActionCode.strCrystal + CardUtility.strSplitMark + CardUtility.strMe + CardUtility.strSplitMark +
                             game.MySelf.RoleInfo.crystal.CurrentRemainPoint + CardUtility.strSplitMark + game.MySelf.RoleInfo.crystal.CurrentFullPoint);
                //奥秘计算
                actionlst.AddRange(game.奥秘计算(actionlst));
                game.MySelf.ResetHandCardCost();
                Card.Client.ClientRequest.WriteAction(game.GameId.ToString(GameServer.GameIdFormat), actionlst);
                DisplayMyInfo();
            }

        }
        /// <summary>
        /// 攻击
        /// </summary>
        /// <param name="MyPos"></param>
        private void Fight(int MyPos)
        {
            var SelectOpt = new Card.CardUtility.SelectOption();
            SelectOpt.EffectTargetSelectDirect = CardUtility.TargetSelectDirectEnum.对方;
            SelectOpt.EffectTargetSelectRole = CardUtility.TargetSelectRoleEnum.所有角色;
            var YourPos = SelectPanel(SelectOpt, true);
            List<String> actionlst = RunAction.Fight(game, MyPos, YourPos.Postion);
            actionlst.AddRange(game.奥秘计算(actionlst));
            game.MySelf.ResetHandCardCost();
            Card.Client.ClientRequest.WriteAction(game.GameId.ToString(GameServer.GameIdFormat), actionlst);
            DisplayMyInfo();
        }
    }
}
