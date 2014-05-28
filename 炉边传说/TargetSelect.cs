using Card;
using Card.Client;
using System;
using System.Windows.Forms;

namespace 炉边传说
{
    public partial class TargetSelect : Form
    {
        Card.CardUtility.TargetSelectDirectEnum direct;
        Card.CardUtility.TargetSelectRoleEnum role;
        GameManager game;
        Boolean 嘲讽限制;
        public CardUtility.TargetPosition pos = new CardUtility.TargetPosition();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mDirect"></param>
        /// <param name="mRole"></param>
        /// <param name="mGame"></param>
        /// <param name="嘲讽限制"></param>
        public TargetSelect(CardUtility.TargetSelectDirectEnum mDirect, CardUtility.TargetSelectRoleEnum mRole, GameManager mGame, Boolean m嘲讽限制)
        {
            InitializeComponent();
            direct = mDirect;
            role = mRole;
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
            btnMyHero.Text = game.MySelf.RoleInfo.GetInfo();
            btnYourHero.Text = game.YourInfo.GetInfo();
            for (int i = 0; i < game.MySelf.RoleInfo.BattleField.MinionCount; i++)
            {
                Controls.Find("btnMe" + (i + 1).ToString(), true)[0].Text = game.MySelf.RoleInfo.BattleField.BattleMinions[i].GetInfo();
                Controls.Find("btnMe" + (i + 1).ToString(), true)[0].Click += (x, y) =>
                {
                    pos.MeOrYou = true;
                    //这里千万不能使用 i ,每次 i 都是固定值
                    //pos.Postion = i + 1;
                    pos.Postion = int.Parse(((Button)x).Name.Substring("btnMe".Length));
                    this.Close();
                };
            }
            for (int i = 0; i < game.YourInfo.BattleField.MinionCount; i++)
            {
                Controls.Find("btnYou" + (i + 1).ToString(), true)[0].Text = game.YourInfo.BattleField.BattleMinions[i].GetInfo();
                Controls.Find("btnYou" + (i + 1).ToString(), true)[0].Click += (x, y) =>
                {
                    pos.MeOrYou = false;
                    //这里千万不能使用 i ,每次 i 都是固定值
                    //pos.Postion = i + 1;
                    pos.Postion = int.Parse(((Button)x).Name.Substring("btnYou".Length));
                    this.Close();
                };
            }
            btnMyHero.Enabled = false;
            btnMyHero.Click += (x, y) =>
            {
                pos.MeOrYou = true;
                pos.Postion = 0;
                this.Close();
            };
            btnYourHero.Enabled = false;
            btnYourHero.Click += (x, y) =>
            {
                pos.MeOrYou = false;
                pos.Postion = 0;
                this.Close();
            };

            for (int i = 0; i < 7; i++)
            {
                Controls.Find("btnMe" + (i + 1).ToString(), true)[0].Enabled = false;
            }
            for (int i = 0; i < 7; i++)
            {
                Controls.Find("btnYou" + (i + 1).ToString(), true)[0].Enabled = false;
            }
            switch (direct)
            {
                case CardUtility.TargetSelectDirectEnum.本方:
                    switch (role)
                    {
                        case CardUtility.TargetSelectRoleEnum.随从:
                            for (int i = 0; i < game.MySelf.RoleInfo.BattleField.MinionCount; i++)
                            {
                                Controls.Find("btnMe" + (i + 1).ToString(), true)[0].Enabled = true;
                            }
                            break;
                        case CardUtility.TargetSelectRoleEnum.英雄:
                            btnMyHero.Enabled = true;
                            break;
                        case CardUtility.TargetSelectRoleEnum.所有角色:
                            btnMyHero.Enabled = true;
                            for (int i = 0; i < game.MySelf.RoleInfo.BattleField.MinionCount; i++)
                            {
                                Controls.Find("btnMe" + (i + 1).ToString(), true)[0].Enabled = true;
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case CardUtility.TargetSelectDirectEnum.对方:
                    switch (role)
                    {
                        case CardUtility.TargetSelectRoleEnum.随从:
                            for (int i = 0; i < game.YourInfo.BattleField.MinionCount; i++)
                            {
                                Controls.Find("btnYou" + (i + 1).ToString(), true)[0].Enabled = true;
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
                                        Controls.Find("btnYou" + (i + 1).ToString(), true)[0].Enabled = true;
                                    }
                                }
                            }
                            else
                            {
                                btnYourHero.Enabled = true;
                                for (int i = 0; i < game.YourInfo.BattleField.MinionCount; i++)
                                {
                                    Controls.Find("btnYou" + (i + 1).ToString(), true)[0].Enabled = true;
                                }
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case CardUtility.TargetSelectDirectEnum.双方:
                    switch (role)
                    {
                        case CardUtility.TargetSelectRoleEnum.随从:
                            for (int i = 0; i < game.MySelf.RoleInfo.BattleField.MinionCount; i++)
                            {
                                Controls.Find("btnMe" + (i + 1).ToString(), true)[0].Enabled = true;
                            }
                            for (int i = 0; i < game.YourInfo.BattleField.MinionCount; i++)
                            {
                                Controls.Find("btnYou" + (i + 1).ToString(), true)[0].Enabled = true;
                            }
                            break;
                        case CardUtility.TargetSelectRoleEnum.英雄:
                            btnMyHero.Enabled = true;
                            btnYourHero.Enabled = true;
                            break;
                        case CardUtility.TargetSelectRoleEnum.所有角色:
                            btnMyHero.Enabled = true;
                            btnYourHero.Enabled = true;
                            for (int i = 0; i < game.MySelf.RoleInfo.BattleField.MinionCount; i++)
                            {
                                Controls.Find("btnMe" + (i + 1).ToString(), true)[0].Enabled = true;
                            }
                            for (int i = 0; i < game.YourInfo.BattleField.MinionCount; i++)
                            {
                                Controls.Find("btnYou" + (i + 1).ToString(), true)[0].Enabled = true;
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
