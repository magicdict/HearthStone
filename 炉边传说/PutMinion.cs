using Engine.Client;
using System;
using System.Windows.Forms;

namespace 炉边传说
{
    public partial class PutMinion : Form
    {
        public PutMinion(GameStatus mgame)
        {
            InitializeComponent();
            game = mgame;
        }
        /// <summary>
        /// 游戏管理者
        /// </summary>
        GameStatus game;
        /// <summary>
        /// 战场位置
        /// </summary>
        public int MinionPos;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PutMinion_Load(object sender, EventArgs e)
        {
            int Megrate = 3;
            int LeftPos = (this.Width - (game.client.HostInfo.BattleField.MinionCount * btnMe1.Width +
            (game.client.HostInfo.BattleField.MinionCount - 1) * Megrate)) / 2;

            for (int i = 0; i < Engine.Client.BattleFieldInfo.MaxMinionCount - 1; i++)
            {
                Controls.Find("btnMe" + (i + 1).ToString(), true)[0].Visible = false;
            }
            for (int i = 0; i < game.client.HostInfo.BattleField.MinionCount; i++)
            {
                Controls.Find("btnMe" + (i + 1).ToString(), true)[0].Visible = true;
                Controls.Find("btnMe" + (i + 1).ToString(), true)[0].Left = LeftPos;
                ((ctlCard)Controls.Find("btnMe" + (i + 1).ToString(), true)[0]).CardInfo = game.client.HostInfo.BattleField.BattleMinions[i];
                ((ctlCard)Controls.Find("btnMe" + (i + 1).ToString(), true)[0]).CanAttack = false;
                LeftPos += btnMe1.Width + Megrate;
            }

            LeftPos = (this.Width - ((game.client.HostInfo.BattleField.MinionCount + 1) * btnPos1.Width +
            (game.client.HostInfo.BattleField.MinionCount - 1) * Megrate)) / 2;
            for (int i = 0; i < Engine.Client.BattleFieldInfo.MaxMinionCount; i++)
            {
                Controls.Find("btnPos" + (i + 1).ToString(), true)[0].Visible = false;
            }
            for (int i = 0; i < game.client.HostInfo.BattleField.MinionCount + 1; i++)
            {
                Controls.Find("btnPos" + (i + 1).ToString(), true)[0].Visible = true;
                Controls.Find("btnPos" + (i + 1).ToString(), true)[0].Left = LeftPos;
                Controls.Find("btnPos" + (i + 1).ToString(), true)[0].Click += (x, y) =>
                {
                    MinionPos = int.Parse(((Button)x).Name.Substring("btnPos".Length));
                    this.Close();
                };
                LeftPos += btnPos1.Width + Megrate;
            }
        }
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            MinionPos = -1;
            this.Close();
        }
    }
}
