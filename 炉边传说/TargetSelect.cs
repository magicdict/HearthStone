using Card;
using Card.Client;
using Card.Effect;
using System;
using System.Windows.Forms;

namespace 炉边传说
{
    public partial class TargetSelect : Form
    {
        GameManager game;
        Boolean 嘲讽限制;
        CardUtility.SelectOption SelectOption;
        public CardUtility.TargetPosition pos = new CardUtility.TargetPosition();
        int Megrate = 3;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mDirect"></param>
        /// <param name="mRole"></param>
        /// <param name="mGame"></param>
        /// <param name="嘲讽限制"></param>
        public TargetSelect(CardUtility.SelectOption opt, GameManager mGame, Boolean m嘲讽限制)
        {
            InitializeComponent();
            SelectOption = opt;
            game = mGame;
            嘲讽限制 = m嘲讽限制;
        }
        /// <summary>
        /// 取消选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            pos.Postion = -1;
            this.Close();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TargetSelect_Load(object sender, EventArgs e)
        {
            btnMyHero.Hero = game.MyInfo;
            btnYourHero.Hero = game.YourInfo;
            int LeftPos = (this.Width - (game.MyInfo.BattleField.MinionCount * btnMe1.Width +
            (game.MyInfo.BattleField.MinionCount - 1) * Megrate)) / 2;
            for (int i = 0; i < game.MyInfo.BattleField.MinionCount; i++)
            {
                ((ctlCard)Controls.Find("btnMe" + (i + 1).ToString(), true)[0]).CardInfo = game.MyInfo.BattleField.BattleMinions[i];
                ((ctlCard)Controls.Find("btnMe" + (i + 1).ToString(), true)[0]).Left = LeftPos;
                ((ctlCard)Controls.Find("btnMe" + (i + 1).ToString(), true)[0]).FightClick += (x, y) =>
                {
                    pos.MeOrYou = true;
                    //这里千万不能使用 i ,每次 i 都是固定值
                    //pos.Postion = i + 1;
                    pos.Postion = int.Parse(((Button)x).Parent.Name.Substring("btnMe".Length));
                    this.Close();
                };
                LeftPos += btnMe1.Width + Megrate;
            }

            LeftPos = (this.Width - (game.YourInfo.BattleField.MinionCount * btnMe1.Width + (game.YourInfo.BattleField.MinionCount - 1) * Megrate)) / 2;
            for (int i = 0; i < game.YourInfo.BattleField.MinionCount; i++)
            {
                ((ctlCard)Controls.Find("btnYou" + (i + 1).ToString(), true)[0]).CardInfo = game.YourInfo.BattleField.BattleMinions[i];
                ((ctlCard)Controls.Find("btnYou" + (i + 1).ToString(), true)[0]).Left = LeftPos;
                ((ctlCard)Controls.Find("btnYou" + (i + 1).ToString(), true)[0]).FightClick += (x, y) =>
                {
                    pos.MeOrYou = false;
                    //这里千万不能使用 i ,每次 i 都是固定值
                    //pos.Postion = i + 1;
                    pos.Postion = int.Parse(((Button)x).Parent.Name.Substring("btnYou".Length));
                    this.Close();
                };
                LeftPos += btnMe1.Width + Megrate;
            }
            btnMyHero.Enabled = false;
            btnMyHero.Left = (this.Width - btnMyHero.Width) / 2;
            btnMyHero.Click += (x, y) =>
            {
                pos.MeOrYou = true;
                pos.Postion = 0;
                this.Close();
            };
            btnYourHero.Enabled = false;
            btnYourHero.Left = (this.Width - btnYourHero.Width) / 2;
            btnYourHero.Click += (x, y) =>
            {
                pos.MeOrYou = false;
                pos.Postion = 0;
                this.Close();
            };

            for (int i = 0; i < 7; i++)
            {
                Controls.Find("btnMe" + (i + 1).ToString(), true)[0].Visible = false;
            }
            for (int i = 0; i < 7; i++)
            {
                Controls.Find("btnYou" + (i + 1).ToString(), true)[0].Visible = false;
            }
            switch (SelectOption.EffectTargetSelectDirect)
            {
                case CardUtility.TargetSelectDirectEnum.本方:
                    switch (SelectOption.EffectTargetSelectRole)
                    {
                        case CardUtility.TargetSelectRoleEnum.随从:
                            for (int i = 0; i < game.MyInfo.BattleField.MinionCount; i++)
                            {
                                if (Card.CardUtility.符合种族条件(game.MyInfo.BattleField.BattleMinions[i], SelectOption))
                                    Controls.Find("btnMe" + (i + 1).ToString(), true)[0].Visible = true;
                                ((ctlCard)Controls.Find("btnMe" + (i + 1).ToString(), true)[0]).CanAttack = true;
                            }
                            break;
                        case CardUtility.TargetSelectRoleEnum.英雄:
                            btnMyHero.Enabled = true;
                            break;
                        case CardUtility.TargetSelectRoleEnum.所有角色:
                            btnMyHero.Enabled = true;
                            for (int i = 0; i < game.MyInfo.BattleField.MinionCount; i++)
                            {
                                if (Card.CardUtility.符合种族条件(game.MyInfo.BattleField.BattleMinions[i], SelectOption))
                                    Controls.Find("btnMe" + (i + 1).ToString(), true)[0].Visible = true;
                                ((ctlCard)Controls.Find("btnMe" + (i + 1).ToString(), true)[0]).CanAttack = true;
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case CardUtility.TargetSelectDirectEnum.对方:
                    switch (SelectOption.EffectTargetSelectRole)
                    {
                        case CardUtility.TargetSelectRoleEnum.随从:
                            for (int i = 0; i < game.YourInfo.BattleField.MinionCount; i++)
                            {
                                if (Card.CardUtility.符合种族条件(game.YourInfo.BattleField.BattleMinions[i], SelectOption))
                                    Controls.Find("btnYou" + (i + 1).ToString(), true)[0].Visible = true;
                                ((ctlCard)Controls.Find("btnYou" + (i + 1).ToString(), true)[0]).CanAttack = true;
                            }
                            break;
                        case CardUtility.TargetSelectRoleEnum.英雄:
                            btnYourHero.Enabled = true;
                            break;
                        case CardUtility.TargetSelectRoleEnum.所有角色:

                            Boolean Has嘲讽 = false;
                            for (int i = 0; i < game.YourInfo.BattleField.MinionCount; i++)
                            {
                                if (game.YourInfo.BattleField.BattleMinions[i].Actual嘲讽)
                                {
                                    Has嘲讽 = true;
                                    break;
                                }
                            }
                            if (嘲讽限制 && Has嘲讽)
                            {
                                btnYourHero.Enabled = false;
                                for (int i = 0; i < game.YourInfo.BattleField.MinionCount; i++)
                                {
                                    //只能选择嘲讽对象
                                    if (game.YourInfo.BattleField.BattleMinions[i].Actual嘲讽)
                                    {
                                        Controls.Find("btnYou" + (i + 1).ToString(), true)[0].Visible = true;
                                        ((ctlCard)Controls.Find("btnYou" + (i + 1).ToString(), true)[0]).CanAttack = true;
                                    }
                                }
                            }
                            else
                            {
                                btnYourHero.Enabled = true;
                                for (int i = 0; i < game.YourInfo.BattleField.MinionCount; i++)
                                {
                                    if (Card.CardUtility.符合种族条件(game.YourInfo.BattleField.BattleMinions[i], SelectOption))
                                        Controls.Find("btnYou" + (i + 1).ToString(), true)[0].Visible = true;
                                    ((ctlCard)Controls.Find("btnYou" + (i + 1).ToString(), true)[0]).CanAttack = true;
                                }
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case CardUtility.TargetSelectDirectEnum.双方:
                    switch (SelectOption.EffectTargetSelectRole)
                    {
                        case CardUtility.TargetSelectRoleEnum.随从:
                            for (int i = 0; i < game.MyInfo.BattleField.MinionCount; i++)
                            {
                                if (Card.CardUtility.符合种族条件(game.MyInfo.BattleField.BattleMinions[i], SelectOption))
                                    Controls.Find("btnMe" + (i + 1).ToString(), true)[0].Visible = true;
                                ((ctlCard)Controls.Find("btnMe" + (i + 1).ToString(), true)[0]).CanAttack = true;
                            }
                            for (int i = 0; i < game.YourInfo.BattleField.MinionCount; i++)
                            {
                                if (Card.CardUtility.符合种族条件(game.YourInfo.BattleField.BattleMinions[i], SelectOption))
                                    Controls.Find("btnYou" + (i + 1).ToString(), true)[0].Visible = true;
                                ((ctlCard)Controls.Find("btnYou" + (i + 1).ToString(), true)[0]).CanAttack = true;
                            }
                            break;
                        case CardUtility.TargetSelectRoleEnum.英雄:
                            btnMyHero.Enabled = true;
                            btnYourHero.Enabled = true;
                            break;
                        case CardUtility.TargetSelectRoleEnum.所有角色:
                            btnMyHero.Enabled = true;
                            btnYourHero.Enabled = true;
                            for (int i = 0; i < game.MyInfo.BattleField.MinionCount; i++)
                            {
                                if (Card.CardUtility.符合种族条件(game.MyInfo.BattleField.BattleMinions[i], SelectOption))
                                    Controls.Find("btnMe" + (i + 1).ToString(), true)[0].Visible = true;
                                ((ctlCard)Controls.Find("btnMe" + (i + 1).ToString(), true)[0]).CanAttack = true;
                            }
                            for (int i = 0; i < game.YourInfo.BattleField.MinionCount; i++)
                            {
                                if (Card.CardUtility.符合种族条件(game.YourInfo.BattleField.BattleMinions[i], SelectOption))
                                    Controls.Find("btnYou" + (i + 1).ToString(), true)[0].Visible = true;
                                ((ctlCard)Controls.Find("btnYou" + (i + 1).ToString(), true)[0]).CanAttack = true;
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
