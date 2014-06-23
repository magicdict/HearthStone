using Engine.Client;
using Engine.Utility;
using System;
using System.Windows.Forms;

namespace 炉边传说
{
    public partial class TargetSelect : Form
    {
        /// <summary>
        /// 游戏控制
        /// </summary>
        GameStatus game;
        /// <summary>
        /// 嘲讽限制
        /// </summary>
        Boolean 嘲讽限制;
        /// <summary>
        /// 位置选项
        /// </summary>
        CardUtility.PositionSelectOption SelectOption;
        /// <summary>
        /// 选定位置
        /// </summary>
        public CardUtility.TargetPosition Position = new CardUtility.TargetPosition();
        /// <summary>
        /// TargetSelect
        /// </summary>
        /// <param name="option"></param>
        /// <param name="mGame"></param>
        public TargetSelect(CardUtility.PositionSelectOption option, GameStatus mGame)
        {
            InitializeComponent();
            SelectOption = option;
            game = mGame;
            嘲讽限制 = option.嘲讽限制;
            Position.Postion = -1;
        }
        /// <summary>
        /// 取消选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Position.Postion = -1;
            this.Close();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TargetSelect_Load(object sender, EventArgs e)
        {
            BattleFieldInfo.SetTargetSelectEnable(SelectOption, game);
            int Megrate = 3;
            btnMyHero.Hero = game.client.HostInfo;
            btnYourHero.Hero = game.client.GuestInfo;
            int LeftPos = (this.Width - (game.client.HostInfo.BattleField.MinionCount * btnMe1.Width +
            (game.client.HostInfo.BattleField.MinionCount - 1) * Megrate)) / 2;
            for (int i = 0; i < game.client.HostInfo.BattleField.MinionCount; i++)
            {
                ((ctlCard)Controls.Find("btnMe" + (i + 1).ToString(), true)[0]).Visible = game.client.HostInfo.BattleField.BattleMinions[i].能否成为动作对象;
                ((ctlCard)Controls.Find("btnMe" + (i + 1).ToString(), true)[0]).CardInfo = game.client.HostInfo.BattleField.BattleMinions[i];
                ((ctlCard)Controls.Find("btnMe" + (i + 1).ToString(), true)[0]).Left = LeftPos;
                ((ctlCard)Controls.Find("btnMe" + (i + 1).ToString(), true)[0]).FightClick += (x, y) =>
                {
                    Position.本方对方标识 = true;
                    //这里千万不能使用 i ,每次 i 都是固定值
                    Position.Postion = int.Parse(((Button)x).Parent.Name.Substring("btnMe".Length));
                    this.Close();
                };
                LeftPos += btnMe1.Width + Megrate;
            }

            LeftPos = (this.Width - (game.client.GuestInfo.BattleField.MinionCount * btnMe1.Width + (game.client.GuestInfo.BattleField.MinionCount - 1) * Megrate)) / 2;
            for (int i = 0; i < game.client.GuestInfo.BattleField.MinionCount; i++)
            {
                ((ctlCard)Controls.Find("btnYou" + (i + 1).ToString(), true)[0]).Visible = game.client.GuestInfo.BattleField.BattleMinions[i].能否成为动作对象;
                ((ctlCard)Controls.Find("btnYou" + (i + 1).ToString(), true)[0]).CardInfo = game.client.GuestInfo.BattleField.BattleMinions[i];
                ((ctlCard)Controls.Find("btnYou" + (i + 1).ToString(), true)[0]).Left = LeftPos;
                ((ctlCard)Controls.Find("btnYou" + (i + 1).ToString(), true)[0]).FightClick += (x, y) =>
                {
                    Position.本方对方标识 = false;
                    //这里千万不能使用 i ,每次 i 都是固定值
                    //pos.Postion = i + 1;
                    Position.Postion = int.Parse(((Button)x).Parent.Name.Substring("btnYou".Length));
                    this.Close();
                };
                LeftPos += btnMe1.Width + Megrate;
            }
            btnMyHero.Enabled = game.client.HostInfo.能否成为动作对象;
            btnMyHero.Left = (this.Width - btnMyHero.Width) / 2;
            btnMyHero.Click += (x, y) =>
            {
                Position.本方对方标识 = true;
                Position.Postion = 0;
                this.Close();
            };
            btnYourHero.Enabled = game.client.GuestInfo.能否成为动作对象; ;
            btnYourHero.Left = (this.Width - btnYourHero.Width) / 2;
            btnYourHero.Click += (x, y) =>
            {
                Position.本方对方标识 = false;
                Position.Postion = 0;
                this.Close();
            };
        }
    }
}
