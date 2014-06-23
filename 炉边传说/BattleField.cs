using Engine.Card;
using Engine.Client;
using Engine.Server;
using Engine.Utility;
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
        /// 等待对方行动Timer
        /// </summary>
        private Timer WaitTimer = new Timer();
        /// <summary>
        /// 间距
        /// </summary>
        int Megrate = 3;
        /// <summary>
        /// 开始游戏
        /// </summary>
        private void InitGame()
        {
            RunAction.GetPutPos = GetPutPos;
            GameManager.InitPlayInfo();
            switch (GameManager.游戏类型)
            {
                case SystemManager.GameType.单机版:
                    //Host,Guest
                    GameManager.InitHandCard(true);
                    GameManager.InitHandCard(false);
                    btnMyHeroAblity.Tag = GameManager.gameStatus.client.MyInfo.HeroAbility;
                    GameManager.gameStatus.client.IsMyTurn = GameManager.gameStatus.client.IsFirst;
                    GameManager.TurnStart(true);
                    break;
                case SystemManager.GameType.客户端服务器版:
                    GameManager.InitHandCard();
                    btnMyHeroAblity.Tag = GameManager.gameStatus.client.MyInfo.HeroAbility;
                    GameManager.gameStatus.client.IsMyTurn = GameManager.gameStatus.client.IsFirst;
                    GameManager.TurnStart(true);
                    break;
                case SystemManager.GameType.HTML版:
                    //HTML版不实现
                    break;
                default:
                    break;
            }
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
            btnMyHeroAblity.Click += btnUseHandCard_Click;
            StartNewTurn();
            DisplayMyInfo();
        }
        /// <summary>
        /// 随从进场位置
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        private int GetPutPos(GameStatus game)
        {
            var frm = new PutMinion(game);
            frm.ShowDialog();
            return frm.MinionPos;
        }
        /// <summary>
        /// 选择目标
        /// </summary>
        /// <returns></returns>
        private CardUtility.TargetPosition SelectPanel(CardUtility.PositionSelectOption SelectOpt)
        {
            var frm = new TargetSelect(SelectOpt, GameManager.gameStatus);
            frm.ShowDialog();
            var SelectPos = frm.Position;
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
            if (GameManager.gameStatus.client.MyInfo.Weapon != null)
            {
                btnMyWeapon.Weapon = GameManager.gameStatus.client.MyInfo.Weapon;
            }
            else
            {
                btnMyWeapon.Clear();
            }
            btnYourWeapon.Left = LeftPos;
            if (GameManager.gameStatus.client.YourInfo.Weapon != null)
            {
                btnYourWeapon.Weapon = GameManager.gameStatus.client.YourInfo.Weapon;
            }
            else
            {
                btnYourWeapon.Clear();
            }
            LeftPos += (btnMyWeapon.Width + Megrate);
            //没有使用过，有武器，武器耐久度不为零
            if (GameManager.gameStatus.client.MyInfo.IsWeaponEnable(GameManager.gameStatus.client.IsMyTurn))
            {
                btnMyWeapon.Visible = true;
            }
            else
            {
                btnMyWeapon.Visible = false;
            }

            btnMyHero.Hero = GameManager.gameStatus.client.MyInfo;
            btnMyHero.Left = LeftPos;
            btnYourHero.Hero = GameManager.gameStatus.client.YourInfo;
            btnYourHero.Left = LeftPos;

            LeftPos += btnMyHero.Width + Megrate;

            //没有使用过，能够使用
            if (GameManager.gameStatus.client.MyInfo.IsHeroAblityEnable(GameManager.gameStatus.client.IsMyTurn))
            {
                btnMyHeroAblity.Enabled = true;
            }
            else
            {
                btnMyHeroAblity.Enabled = false;
            }
            btnMyHeroAblity.Left = LeftPos;
            btnYourHeroAblity.Left = LeftPos;

            MyCrystalBar.CrystalInfo = GameManager.gameStatus.client.MyInfo.crystal;
            YourCrystalBar.CrystalInfo = GameManager.gameStatus.client.YourInfo.crystal;


            LeftPos = (this.Width - (GameManager.gameStatus.client.MyInfo.BattleField.MinionCount * btnMe1.Width +
                      (GameManager.gameStatus.client.MyInfo.BattleField.MinionCount - 1) * Megrate)) / 2;
            for (int i = 0; i < BattleFieldInfo.MaxMinionCount; i++)
            {
                var myMinion = GameManager.gameStatus.client.MyInfo.BattleField.BattleMinions[i];
                if (myMinion != null)
                {
                    Controls.Find("btnMe" + (i + 1).ToString(), true)[0].Visible = true;
                    ((ctlCard)Controls.Find("btnMe" + (i + 1).ToString(), true)[0]).CardInfo = myMinion;
                    Controls.Find("btnMe" + (i + 1).ToString(), true)[0].Left = LeftPos;
                    LeftPos += btnMe1.Width + Megrate;
                    if (myMinion.CanAttack())
                    {
                        if (GameManager.gameStatus.client.IsMyTurn) ((ctlCard)Controls.Find("btnMe" + (i + 1).ToString(), true)[0]).CanAttack = true;
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
            LeftPos = (this.Width - (GameManager.gameStatus.client.YourInfo.BattleField.MinionCount * btnYou1.Width +
                      (GameManager.gameStatus.client.YourInfo.BattleField.MinionCount - 1) * Megrate)) / 2;
            for (int i = 0; i < BattleFieldInfo.MaxMinionCount; i++)
            {
                if (GameManager.gameStatus.client.YourInfo.BattleField.BattleMinions[i] != null)
                {
                    Controls.Find("btnYou" + (i + 1).ToString(), true)[0].Visible = true;
                    //这里注意：随从有个 能否成为动作对象 属性，这个会影响CanAttack的状态
                    ((ctlCard)Controls.Find("btnYou" + (i + 1).ToString(), true)[0]).CardInfo = GameManager.gameStatus.client.YourInfo.BattleField.BattleMinions[i];
                    //能否成为动作对象 在对象选择的时候用的，这里的话还是要设置为 False
                    ((ctlCard)Controls.Find("btnYou" + (i + 1).ToString(), true)[0]).CanAttack = false;
                    Controls.Find("btnYou" + (i + 1).ToString(), true)[0].Left = LeftPos;
                    LeftPos += btnYou1.Width + Megrate;
                }
                else
                {
                    Controls.Find("btnYou" + (i + 1).ToString(), true)[0].Visible = false;
                }
            }
            LeftPos = (this.Width - (GameManager.gameStatus.client.MySelfInfo.handCards.Count * btnHandCard1.Width + (GameManager.gameStatus.client.MySelfInfo.handCards.Count - 1) * Megrate)) / 2;
            for (int i = 0; i < 10; i++)
            {
                if (i < GameManager.gameStatus.client.MySelfInfo.handCards.Count)
                {
                    ((ctlHandCard)Controls.Find("btnHandCard" + (i + 1).ToString(), true)[0]).HandCard = GameManager.gameStatus.client.MySelfInfo.handCards[i];
                    Controls.Find("btnHandCard" + (i + 1).ToString(), true)[0].Tag = GameManager.gameStatus.client.MySelfInfo.handCards[i];
                    if (GameManager.gameStatus.client.IsMyTurn) Controls.Find("btnHandCard" + (i + 1).ToString(), true)[0].Enabled = true;
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
            lblGameStatus.Text = GameManager.gameStatus.GetGameInfo();
            //胜负判定
            if (GameManager.gameStatus.client.MyInfo.LifePoint <= 0 && GameManager.gameStatus.client.YourInfo.LifePoint <= 0)
            {
                MessageBox.Show("Draw Game");
                WaitTimer.Stop();
                this.Close();
            }
            else
            {
                if (GameManager.gameStatus.client.MyInfo.LifePoint <= 0)
                {
                    MessageBox.Show("You Lose");
                    WaitTimer.Stop();
                    this.Close();
                }
                if (GameManager.gameStatus.client.YourInfo.LifePoint <= 0)
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
            var ActionLst = GameManager.TurnEnd(true);
            if (ActionLst.Count != 0) Engine.Client.ClientRequest.WriteAction(GameManager.gameStatus.GameId.ToString(GameServer.GameIdFormat), ActionLst);
            //结束回合
            Engine.Client.ClientRequest.TurnEnd(GameManager.gameStatus.GameId.ToString(GameServer.GameIdFormat));
            GameManager.gameStatus.client.IsMyTurn = false;
            GameManager.TurnStart(false);
            StartNewTurn();
            WaitTimer.Start();
        }
        /// <summary>
        /// 新的回合
        /// </summary>
        private void StartNewTurn()
        {
            if (GameManager.gameStatus.client.IsMyTurn)
            {
                //回合开始效果
                List<String> ActionLst = new List<string>();
                for (int i = 0; i < GameManager.gameStatus.client.MyInfo.BattleField.MinionCount; i++)
                {
                    if (GameManager.gameStatus.client.MyInfo.BattleField.BattleMinions[i] != null)
                    {
                        ActionLst.AddRange(GameManager.gameStatus.client.MyInfo.BattleField.BattleMinions[i].回合开始(GameManager.gameStatus));
                    }
                }
                if (ActionLst.Count != 0) Engine.Client.ClientRequest.WriteAction(GameManager.gameStatus.GameId.ToString(GameServer.GameIdFormat), ActionLst);
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
            var Actions = Engine.Client.ClientRequest.ReadAction(GameManager.gameStatus.GameId.ToString(GameServer.GameIdFormat));
            if (String.IsNullOrEmpty(Actions)) return;
            var ActionList = Actions.Split(Engine.Utility.CardUtility.strSplitArrayMark.ToCharArray());
            foreach (var item in ActionList)
            {
                if (ActionCode.GetActionType(item) != ActionCode.ActionType.EndTurn)
                {
                    ProcessAction.Process(item, GameManager.gameStatus);
                }
                else
                {
                    WaitTimer.Stop();
                    btnEndTurn.Enabled = true;
                    GameManager.TurnEnd(false);
                    GameManager.gameStatus.client.IsMyTurn = true;
                    GameManager.TurnStart(true);
                    StartNewTurn();
                    break;
                }
            }
            DisplayMyInfo();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FirstEffect"></param>
        /// <param name="SecondEffect"></param>
        /// <returns></returns>
        private Engine.Utility.CardUtility.PickEffect PickEffect(String FirstEffect, String SecondEffect)
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
            var msg = Engine.Card.CardBasicInfo.CheckCondition(card, GameManager.gameStatus.client.MyInfo);
            if (!String.IsNullOrEmpty(msg))
            {
                MessageBox.Show(msg);
                return;
            }
            var actionlst = RunAction.StartAction(GameManager.gameStatus, card.序列号);
            if (actionlst.Count != 0)
            {
                if ((sender.GetType()) == typeof(Button))
                {
                    actionlst.Insert(0, ActionCode.strCard + CardUtility.strSplitMark + CardUtility.strMe);
                    GameManager.gameStatus.client.MyInfo.crystal.CurrentRemainPoint -= card.使用成本;
                    GameManager.gameStatus.client.MyInfo.HandCardCount--;
                    GameManager.gameStatus.client.MySelfInfo.RemoveUsedCard(card.序列号);
                }
                else
                {
                    GameManager.gameStatus.client.MyInfo.crystal.CurrentRemainPoint -= card.使用成本;
                    GameManager.gameStatus.client.MyInfo.IsUsedHeroAbility = true;
                }
                actionlst.Add(ActionCode.strCrystal + CardUtility.strSplitMark + CardUtility.strMe + CardUtility.strSplitMark +
                             GameManager.gameStatus.client.MyInfo.crystal.CurrentRemainPoint + CardUtility.strSplitMark + GameManager.gameStatus.client.MyInfo.crystal.CurrentFullPoint);
                //奥秘计算
                actionlst.AddRange(SecretCard.奥秘计算(actionlst, GameManager.gameStatus));
                GameManager.gameStatus.client.MySelfInfo.ResetHandCardCost(GameManager.gameStatus);
                Engine.Client.ClientRequest.WriteAction(GameManager.gameStatus.GameId.ToString(GameServer.GameIdFormat), actionlst);
                DisplayMyInfo();
            }

        }
        /// <summary>
        /// 攻击
        /// </summary>
        /// <param name="MyPos"></param>
        private void Fight(int MyPos)
        {
            var SelectOpt = new Engine.Utility.CardUtility.PositionSelectOption();
            SelectOpt.EffectTargetSelectDirect = CardUtility.TargetSelectDirectEnum.对方;
            SelectOpt.EffectTargetSelectRole = CardUtility.TargetSelectRoleEnum.所有角色;
            SelectOpt.嘲讽限制 = true;
            var YourPos = SelectPanel(SelectOpt);
            List<String> actionlst = RunAction.Fight(GameManager.gameStatus, MyPos, YourPos.Postion);
            actionlst.AddRange(SecretCard.奥秘计算(actionlst, GameManager.gameStatus));
            GameManager.gameStatus.client.MySelfInfo.ResetHandCardCost(GameManager.gameStatus);
            Engine.Client.ClientRequest.WriteAction(GameManager.gameStatus.GameId.ToString(GameServer.GameIdFormat), actionlst);
            DisplayMyInfo();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStartGame_Click(object sender, EventArgs e)
        {
            var startform = new frmStartGame();
            startform.ShowDialog();
            if (GameManager.IsStart) InitGame();
        }
    }
}
