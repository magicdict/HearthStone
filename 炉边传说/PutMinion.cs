using Card.Client;
using System;
using System.Windows.Forms;

namespace 炉边传说
{
    public partial class PutMinion : Form
    {
        public PutMinion(GameManager mgame)
        {
            InitializeComponent();
            game = mgame;
        }
        /// <summary>
        /// 游戏管理者
        /// </summary>
        GameManager game;
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
            for (int i = 0; i < Card.Client.BattleFieldInfo.MaxMinionCount - 1; i++)
            {
                Controls.Find("btnMe" + (i + 1).ToString(), true)[0].Text = "[空]";
                Controls.Find("btnMe" + (i + 1).ToString(), true)[0].Enabled = false;
            }
            for (int i = 0; i < Card.Client.BattleFieldInfo.MaxMinionCount; i++)
            {
                Controls.Find("btnPos" + (i + 1).ToString(), true)[0].Enabled = false;
            }
            for (int i = 0; i < game.MySelf.RoleInfo.BattleField.MinionCount; i++)
            {
                Controls.Find("btnMe" + (i + 1).ToString(), true)[0].Text = game.MySelf.RoleInfo.BattleField.BattleMinions[i].GetInfo();
                Controls.Find("btnPos" + (i + 1).ToString(), true)[0].Enabled = true;
            }
            Controls.Find("btnPos" + (game.MySelf.RoleInfo.BattleField.MinionCount + 1).ToString(), true)[0].Enabled = true;

            for (int i = 0; i < game.MySelf.RoleInfo.BattleField.MinionCount + 1; i++)
            {
                Controls.Find("btnPos" + (i + 1).ToString(), true)[0].Click += (x, y) =>
                {
                    MinionPos = int.Parse(((Button)x).Name.Substring("btnPos".Length));
                    this.Close();
                };
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
