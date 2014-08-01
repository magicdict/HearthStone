using Engine.Action;
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
            UseMinionAction.GetMinionPos = GetMinionPos;
            GameManager.MyClientManager.InitPlayInfo();
            ActionStatus.GetSelectTarget = SelectPanel;
            ActionStatus.PickEffect = PickEffect;
            switch (SystemManager.游戏类型)
            {
                case SystemManager.GameType.单机版:
                    btnMyHeroAblity.Tag = GameManager.MyFullServerManager.gameStatus(true).AllRole.MyPublicInfo.Hero.HeroSkill;
                    //GameManager.MyFullServerManager.actionStatus.IsMyTurn = GameManager.MyClientManager.actionStatus.IsFirst;
                    //GameManager.MyFullServerManager.TurnStart(GameManager.MyClientManager.IsMyTurn);
                    break;
                case SystemManager.GameType.客户端服务器版:
                    WaitTimer.Tick += WaitFor;
                    GameManager.MyClientManager.InitHandCard();
                    btnMyHeroAblity.Tag = GameManager.MyClientManager.actionStatus.AllRole.MyPublicInfo.Hero.HeroSkill;
                    GameManager.MyClientManager.IsMyTurn = GameManager.MyClientManager.IsFirst;
                    GameManager.MyClientManager.TurnStart(GameManager.MyClientManager.IsMyTurn);
                    break;
                case SystemManager.GameType.HTML版:
                    //HTML版不实现
                    break;
                default:
                    break;
            }
            for (int i = 0; i < 10; i++)
            {
                ((ctlHandCard)Controls.Find("btnHandCard" + (i + 1).ToString(), true)[0]).UseClick += btnUseHandCard_Click;
            }
            for (int i = 0; i < 7; i++)
            {
                ((ctlCard)Controls.Find("btnYou" + (i + 1).ToString(), true)[0]).CanAttack = false;
                ((ctlCard)Controls.Find("btnMe" + (i + 1).ToString(), true)[0]).CanAttack = false;
                ((ctlCard)Controls.Find("btnMe" + (i + 1).ToString(), true)[0]).FightClick += (x, y) =>
                {
                    //这里千万不能使用 i ,每次 i 都是固定值
                    //和Javascript一样也会产生闭包的问题
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
            btnMyHeroAblity.IsEnable = false;
            btnMyHeroAblity.Click += btnUseHandCard_Click;
            StartNewTurn();
            DisplayMyInfo();
        }
        /// <summary>
        /// 随从进场位置
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        private int GetMinionPos(BattleFieldInfo battleInfo)
        {
            var frm = new PutMinion(battleInfo);
            frm.ShowDialog();
            return frm.MinionPos;
        }
        /// <summary>
        /// 选择目标
        /// </summary>
        /// <returns></returns>
        private CardUtility.指定位置结构体 SelectPanel(CardUtility.位置选择用参数结构体 SelectOpt)
        {
            var frm = new TargetSelect(SelectOpt, GameManager.MyClientManager.actionStatus);
            frm.ShowDialog();
            var SelectPos = frm.Position;
            return SelectPos;
        }
        /// <summary>
        /// 显示我方状态
        /// </summary>
        private void DisplayMyInfo()
        {
            int LeftPos = (Width - (btnMyHero.Width + btnMyHeroAblity.Width + btnMyWeapon.Width + 2 * Megrate)) / 2;
            //武器
            btnMyWeapon.Left = LeftPos;
            if (GameManager.MyClientManager.actionStatus.AllRole.MyPublicInfo.Hero.Weapon != null)
            {
                btnMyWeapon.Weapon = GameManager.MyClientManager.actionStatus.AllRole.MyPublicInfo.Hero.Weapon;
            }
            else
            {
                btnMyWeapon.Clear();
            }
            btnYourWeapon.Left = LeftPos;
            if (GameManager.MyClientManager.actionStatus.AllRole.YourPublicInfo.Hero.Weapon != null)
            {
                btnYourWeapon.Weapon = GameManager.MyClientManager.actionStatus.AllRole.YourPublicInfo.Hero.Weapon;
            }
            else
            {
                btnYourWeapon.Clear();
            }
            LeftPos += (btnMyWeapon.Width + Megrate);
            //没有使用过，有武器，武器耐久度不为零
            if (GameManager.MyClientManager.actionStatus.AllRole.MyPublicInfo.Hero.IsAttackEnable(GameManager.MyClientManager.IsMyTurn))
            {
                btnMyWeapon.Visible = true;
            }
            else
            {
                btnMyWeapon.Visible = false;
            }

            btnMyHero.Hero = GameManager.MyClientManager.actionStatus.AllRole.MyPublicInfo.Hero;
            btnMyHero.Left = LeftPos;
            btnYourHero.Hero = GameManager.MyClientManager.actionStatus.AllRole.YourPublicInfo.Hero;
            btnYourHero.Left = LeftPos;

            LeftPos += btnMyHero.Width + Megrate;

            //没有使用过，能够使用
            if (GameManager.MyClientManager.actionStatus.AllRole.MyPublicInfo.IsHeroSkillEnable(GameManager.MyClientManager.IsMyTurn))
            {
                btnMyHeroAblity.Enabled = true;
                btnMyHeroAblity.IsEnable = true;
            }
            else
            {
                btnMyHeroAblity.Enabled = false;
                btnMyHeroAblity.IsEnable = false;
            }
            btnMyHeroAblity.Left = LeftPos;
            btnYourHeroAblity.Left = LeftPos;

            MyCrystalBar.CrystalInfo = GameManager.MyClientManager.actionStatus.AllRole.MyPublicInfo.crystal;
            YourCrystalBar.CrystalInfo = GameManager.MyClientManager.actionStatus.AllRole.YourPublicInfo.crystal;


            LeftPos = (Width - (GameManager.MyClientManager.actionStatus.AllRole.MyPublicInfo.BattleField.MinionCount * btnMe1.Width +
                      (GameManager.MyClientManager.actionStatus.AllRole.MyPublicInfo.BattleField.MinionCount - 1) * Megrate)) / 2;
            for (int i = 0; i < SystemManager.MaxMinionCount; i++)
            {
                var myMinion = GameManager.MyClientManager.actionStatus.AllRole.MyPublicInfo.BattleField.BattleMinions[i];
                if (myMinion != null)
                {
                    Controls.Find("btnMe" + (i + 1).ToString(), true)[0].Visible = true;
                    ((ctlCard)Controls.Find("btnMe" + (i + 1).ToString(), true)[0]).CardInfo = myMinion;
                    Controls.Find("btnMe" + (i + 1).ToString(), true)[0].Left = LeftPos;
                    LeftPos += btnMe1.Width + Megrate;
                    if (myMinion.能否攻击)
                    {
                        if (GameManager.MyClientManager.IsMyTurn) ((ctlCard)Controls.Find("btnMe" + (i + 1).ToString(), true)[0]).CanAttack = true;
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
            LeftPos = (Width - (GameManager.MyClientManager.actionStatus.AllRole.YourPublicInfo.BattleField.MinionCount * btnYou1.Width +
                      (GameManager.MyClientManager.actionStatus.AllRole.YourPublicInfo.BattleField.MinionCount - 1) * Megrate)) / 2;
            for (int i = 0; i < SystemManager.MaxMinionCount; i++)
            {
                if (GameManager.MyClientManager.actionStatus.AllRole.YourPublicInfo.BattleField.BattleMinions[i] != null)
                {
                    Controls.Find("btnYou" + (i + 1).ToString(), true)[0].Visible = true;
                    //这里注意：随从有个 能否成为动作对象 属性，这个会影响CanAttack的状态
                    ((ctlCard)Controls.Find("btnYou" + (i + 1).ToString(), true)[0]).CardInfo = GameManager.MyClientManager.actionStatus.AllRole.YourPublicInfo.BattleField.BattleMinions[i];
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
            LeftPos = (Width - (GameManager.MyClientManager.actionStatus.AllRole.MyPrivateInfo.handCards.Count * btnHandCard1.Width + (GameManager.MyClientManager.actionStatus.AllRole.MyPrivateInfo.handCards.Count - 1) * Megrate)) / 2;
            for (int i = 0; i < 10; i++)
            {
                if (i < GameManager.MyClientManager.actionStatus.AllRole.MyPrivateInfo.handCards.Count)
                {
                    ((ctlHandCard)Controls.Find("btnHandCard" + (i + 1).ToString(), true)[0]).HandCard = GameManager.MyClientManager.actionStatus.AllRole.MyPrivateInfo.handCards[i];
                    Controls.Find("btnHandCard" + (i + 1).ToString(), true)[0].Tag = GameManager.MyClientManager.actionStatus.AllRole.MyPrivateInfo.handCards[i];
                    if (GameManager.MyClientManager.IsMyTurn) Controls.Find("btnHandCard" + (i + 1).ToString(), true)[0].Enabled = true;
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
            lblGameStatus.Text = "GameInfo";
            //胜负判定
            if (GameManager.MyClientManager.actionStatus.AllRole.MyPublicInfo.Hero.LifePoint <= 0 && GameManager.MyClientManager.actionStatus.AllRole.YourPublicInfo.Hero.LifePoint <= 0)
            {
                MessageBox.Show("Draw Game");
                if (SystemManager.游戏类型 != SystemManager.GameType.单机版) WaitTimer.Stop();
                GameManager.MyClientManager.事件处理组件.事件特殊处理 = null;
            }
            else
            {
                if (GameManager.MyClientManager.actionStatus.AllRole.MyPublicInfo.Hero.LifePoint <= 0)
                {
                    MessageBox.Show("You Lose");
                    if (SystemManager.游戏类型 != SystemManager.GameType.单机版) WaitTimer.Stop();
                    GameManager.MyClientManager.事件处理组件.事件特殊处理 = null;
                }
                if (GameManager.MyClientManager.actionStatus.AllRole.YourPublicInfo.Hero.LifePoint <= 0)
                {
                    MessageBox.Show("You Win");
                    if (SystemManager.游戏类型 != SystemManager.GameType.单机版) WaitTimer.Stop();
                    GameManager.MyClientManager.事件处理组件.事件特殊处理 = null;
                }
            }
        }
        /// <summary>
        /// 结束回合
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEndTurn_Click(object sender, EventArgs e)
        {
            var ActionLst = GameManager.MyClientManager.TurnEnd(true);
            if (ActionLst.Count != 0 && SystemManager.游戏类型 != SystemManager.GameType.单机版)
                ClientRequest.WriteAction(GameManager.MyClientManager.GameId.ToString(GameServer.GameIdFormat), ActionLst);
            //结束回合
            if (SystemManager.游戏类型 != SystemManager.GameType.单机版)
                ClientRequest.TurnEnd(GameManager.MyClientManager.GameId.ToString(GameServer.GameIdFormat));
            GameManager.MyClientManager.IsMyTurn = false;
            GameManager.MyClientManager.TurnStart(false);
            StartNewTurn();
        }
        /// <summary>
        /// 新的回合
        /// </summary>
        private void StartNewTurn()
        {
            if (GameManager.MyClientManager.IsMyTurn)
            {
                //回合开始效果
                List<string> ActionLst = new List<string>();
                for (int i = 0; i < GameManager.MyClientManager.actionStatus.AllRole.MyPublicInfo.BattleField.MinionCount; i++)
                {
                    if (GameManager.MyClientManager.actionStatus.AllRole.MyPublicInfo.BattleField.BattleMinions[i] != null)
                    {
                        ActionLst.AddRange(GameManager.MyClientManager.actionStatus.AllRole.MyPublicInfo.BattleField.BattleMinions[i].回合开始(GameManager.MyClientManager.actionStatus));
                    }
                }
                if (ActionLst.Count != 0 && SystemManager.游戏类型 != SystemManager.GameType.单机版)
                    ClientRequest.WriteAction(GameManager.MyClientManager.GameId.ToString(GameServer.GameIdFormat), ActionLst);
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
                if (SystemManager.游戏类型 != SystemManager.GameType.单机版)
                {
                    WaitTimer.Start();
                }
                else
                {
                    //List<string> ActionList = Engine.AI.DoAction.Run(GameManager.MyClientManager.actionStatus,false);
                    btnEndTurn.Enabled = true;
                    GameManager.MyClientManager.TurnEnd(false);
                    GameManager.MyClientManager.IsMyTurn = true;
                    GameManager.MyClientManager.TurnStart(true);
                    StartNewTurn();
                }
            }
            //刷新双方状态
            DisplayMyInfo();
        }
        /// <summary>
        /// 读取
        /// </summary>
        private void WaitFor(object sender, EventArgs e)
        {
            var Actions = ClientRequest.ReadAction(GameManager.MyClientManager.GameId.ToString(GameServer.GameIdFormat));
            if (string.IsNullOrEmpty(Actions)) return;
            var ActionList = Actions.Split(CardUtility.strSplitArrayMark.ToCharArray());
            foreach (var item in ActionList)
            {
                if (ActionCode.GetActionType(item) != ActionCode.ActionType.EndTurn)
                {
                    ProcessAction.Process(item, GameManager.MyClientManager.actionStatus);
                }
                else
                {
                    WaitTimer.Stop();
                    btnEndTurn.Enabled = true;
                    GameManager.MyClientManager.TurnEnd(false);
                    GameManager.MyClientManager.IsMyTurn = true;
                    GameManager.MyClientManager.TurnStart(true);
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
        private CardUtility.抉择枚举 PickEffect(string FirstEffect, string SecondEffect)
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
            CardBasicInfo UseHandCard;
            if ((sender.GetType()) == typeof(Button))
            {
                UseHandCard = (CardBasicInfo)((ctlHandCard)(((Button)sender).Parent)).Tag;
            }
            else
            {
                UseHandCard = (CardBasicInfo)(((ctlHeroAbility)sender).Tag);
            }
            GameManager.MyClientManager.CurrentActiveCard = UseHandCard;
            var msg = CardBasicInfo.CheckCondition(UseHandCard, GameManager.MyClientManager.actionStatus.AllRole.MyPublicInfo);
            if (!string.IsNullOrEmpty(msg))
            {
                MessageBox.Show(msg);
                return;
            }
            var actionlst = RunAction.StartAction(GameManager.MyClientManager.actionStatus, UseHandCard.序列号);
            if (actionlst.Count != 0)
            {
                if ((sender.GetType()) == typeof(Button))
                {
                    actionlst.Insert(0, ActionCode.strCard + CardUtility.strSplitMark + CardUtility.strMe);
                    GameManager.MyClientManager.actionStatus.AllRole.MyPublicInfo.crystal.CurrentRemainPoint -= UseHandCard.使用成本;
                    GameManager.MyClientManager.actionStatus.AllRole.MyPublicInfo.HandCardCount--;
                    GameManager.MyClientManager.actionStatus.AllRole.MyPrivateInfo.RemoveUsedCard(UseHandCard.序列号);
                }
                else
                {
                    GameManager.MyClientManager.actionStatus.AllRole.MyPublicInfo.crystal.CurrentRemainPoint -= UseHandCard.使用成本;
                    GameManager.MyClientManager.actionStatus.AllRole.MyPublicInfo.Hero.IsUsedHeroAbility = true;
                }
                actionlst.Add(ActionCode.strCrystal + CardUtility.strSplitMark + CardUtility.strMe + CardUtility.strSplitMark +
                             GameManager.MyClientManager.actionStatus.AllRole.MyPublicInfo.crystal.CurrentRemainPoint + CardUtility.strSplitMark + GameManager.MyClientManager.actionStatus.AllRole.MyPublicInfo.crystal.CurrentFullPoint);
                //奥秘计算
                actionlst.AddRange(SecretCard.奥秘计算(actionlst, GameManager.MyClientManager.actionStatus, GameManager.MyClientManager.GameId));
                GameManager.MyClientManager.actionStatus.AllRole.MyPrivateInfo.ResetHandCardCost(GameManager.MyClientManager.actionStatus);
                if (SystemManager.游戏类型 != SystemManager.GameType.单机版) ClientRequest.WriteAction(GameManager.MyClientManager.GameId.ToString(GameServer.GameIdFormat), actionlst);
                DisplayMyInfo();
            }

        }
        /// <summary>
        /// 攻击
        /// </summary>
        /// <param name="MyPos"></param>
        private void Fight(int MyPos)
        {
            GameManager.MyClientManager.CurrentActiveCard = GameManager.MyClientManager.actionStatus.AllRole.MyPublicInfo.BattleField.BattleMinions[MyPos - 1];
            var YourPos = SelectPanel(SelectUtility.GetFightSelectOpt());
            List<string> actionlst = RunAction.Fight(GameManager.MyClientManager.actionStatus, MyPos, YourPos.位置, true);
            actionlst.AddRange(SecretCard.奥秘计算(actionlst, GameManager.MyClientManager.actionStatus, GameManager.MyClientManager.GameId));
            GameManager.MyClientManager.actionStatus.AllRole.MyPrivateInfo.ResetHandCardCost(GameManager.MyClientManager.actionStatus);
            if (SystemManager.游戏类型 != SystemManager.GameType.单机版) ClientRequest.WriteAction(GameManager.MyClientManager.GameId.ToString(GameServer.GameIdFormat), actionlst);
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
            if (GameManager.MyClientManager.IsStart) InitGame();
        }
    }
}
