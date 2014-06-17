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
        GameManager game;
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
        public TargetSelect(CardUtility.PositionSelectOption option, GameManager mGame)
        {
            InitializeComponent();
            SelectOption = option;
            game = mGame;
            嘲讽限制 = option.嘲讽限制;
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
            btnMyHero.Hero = game.MyInfo;
            btnYourHero.Hero = game.YourInfo;
            int LeftPos = (this.Width - (game.MyInfo.BattleField.MinionCount * btnMe1.Width +
            (game.MyInfo.BattleField.MinionCount - 1) * Megrate)) / 2;
            for (int i = 0; i < game.MyInfo.BattleField.MinionCount; i++)
            {
                ((ctlCard)Controls.Find("btnMe" + (i + 1).ToString(), true)[0]).Visible = game.MyInfo.BattleField.BattleMinions[i].能否成为动作对象;
                ((ctlCard)Controls.Find("btnMe" + (i + 1).ToString(), true)[0]).CardInfo = game.MyInfo.BattleField.BattleMinions[i];
                ((ctlCard)Controls.Find("btnMe" + (i + 1).ToString(), true)[0]).Left = LeftPos;
                ((ctlCard)Controls.Find("btnMe" + (i + 1).ToString(), true)[0]).FightClick += (x, y) =>
                {
                    Position.MeOrYou = true;
                    //这里千万不能使用 i ,每次 i 都是固定值
                    Position.Postion = int.Parse(((Button)x).Parent.Name.Substring("btnMe".Length));
                    this.Close();
                };
                LeftPos += btnMe1.Width + Megrate;
            }

            LeftPos = (this.Width - (game.YourInfo.BattleField.MinionCount * btnMe1.Width + (game.YourInfo.BattleField.MinionCount - 1) * Megrate)) / 2;
            for (int i = 0; i < game.YourInfo.BattleField.MinionCount; i++)
            {
                ((ctlCard)Controls.Find("btnYou" + (i + 1).ToString(), true)[0]).Visible = game.YourInfo.BattleField.BattleMinions[i].能否成为动作对象;
                ((ctlCard)Controls.Find("btnYou" + (i + 1).ToString(), true)[0]).CardInfo = game.YourInfo.BattleField.BattleMinions[i];
                ((ctlCard)Controls.Find("btnYou" + (i + 1).ToString(), true)[0]).Left = LeftPos;
                ((ctlCard)Controls.Find("btnYou" + (i + 1).ToString(), true)[0]).FightClick += (x, y) =>
                {
                    Position.MeOrYou = false;
                    //这里千万不能使用 i ,每次 i 都是固定值
                    //pos.Postion = i + 1;
                    Position.Postion = int.Parse(((Button)x).Parent.Name.Substring("btnYou".Length));
                    this.Close();
                };
                LeftPos += btnMe1.Width + Megrate;
            }
            btnMyHero.Enabled = game.MyInfo.能否成为动作对象;
            btnMyHero.Left = (this.Width - btnMyHero.Width) / 2;
            btnMyHero.Click += (x, y) =>
            {
                Position.MeOrYou = true;
                Position.Postion = 0;
                this.Close();
            };
            btnYourHero.Enabled = game.YourInfo.能否成为动作对象; ;
            btnYourHero.Left = (this.Width - btnYourHero.Width) / 2;
            btnYourHero.Click += (x, y) =>
            {
                Position.MeOrYou = false;
                Position.Postion = 0;
                this.Close();
            };
        }
    }
}
