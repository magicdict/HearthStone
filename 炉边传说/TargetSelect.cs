using Engine.Action;
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
        ActionStatus actionStatus;
        /// <summary>
        /// 位置选项
        /// </summary>
        CardUtility.位置选择用参数结构体 SelectOption;
        /// <summary>
        /// 选定位置
        /// </summary>
        public CardUtility.指定位置结构体 Position = new CardUtility.指定位置结构体();
        /// <summary>
        /// TargetSelect
        /// </summary>
        /// <param name="option"></param>
        /// <param name="_actionStatus"></param>
        public TargetSelect(CardUtility.位置选择用参数结构体 option, ActionStatus _actionStatus)
        {
            InitializeComponent();
            SelectOption = option;
            actionStatus = _actionStatus;
            Position.位置 = -1;
            ctlUsageCard.CardInfo = GameManager.MyClientManager.CurrentActiveCard;
            ctlUsageCard.Enabled = false;
            ctlUsageCard.Visible = true;
        }
        /// <summary>
        /// 取消选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Position.位置 = -1;
            Close();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TargetSelect_Load(object sender, EventArgs e)
        {
            SelectUtility.SetTargetSelectEnable(SelectOption, actionStatus);
            int Megrate = 3;
            btnMyHero.Hero = actionStatus.AllRole.MyPublicInfo;
            btnYourHero.Hero = actionStatus.AllRole.YourPublicInfo;
            int LeftPos = (Width - (actionStatus.AllRole.MyPublicInfo.BattleField.MinionCount * btnMe1.Width +
            (actionStatus.AllRole.MyPublicInfo.BattleField.MinionCount - 1) * Megrate)) / 2;
            for (int i = 0; i < actionStatus.AllRole.MyPublicInfo.BattleField.MinionCount; i++)
            {
                Controls.Find("btnMe" + (i + 1).ToString(), true)[0].Visible = actionStatus.AllRole.MyPublicInfo.BattleField.BattleMinions[i].能否成为动作对象;
                ((ctlCard)Controls.Find("btnMe" + (i + 1).ToString(), true)[0]).CardInfo = actionStatus.AllRole.MyPublicInfo.BattleField.BattleMinions[i];
                Controls.Find("btnMe" + (i + 1).ToString(), true)[0].Left = LeftPos;
                ((ctlCard)Controls.Find("btnMe" + (i + 1).ToString(), true)[0]).FightClick += (x, y) =>
                {
                    Position.本方对方标识 = true;
                    //这里千万不能使用 i ,每次 i 都是固定值
                    Position.位置 = int.Parse(((Button)x).Parent.Name.Substring("btnMe".Length));
                    Close();
                };
                LeftPos += btnMe1.Width + Megrate;
            }

            LeftPos = (Width - (actionStatus.AllRole.YourPublicInfo.BattleField.MinionCount * btnMe1.Width + (actionStatus.AllRole.YourPublicInfo.BattleField.MinionCount - 1) * Megrate)) / 2;
            for (int i = 0; i < actionStatus.AllRole.YourPublicInfo.BattleField.MinionCount; i++)
            {
                Controls.Find("btnYou" + (i + 1).ToString(), true)[0].Visible = actionStatus.AllRole.YourPublicInfo.BattleField.BattleMinions[i].能否成为动作对象;
                ((ctlCard)Controls.Find("btnYou" + (i + 1).ToString(), true)[0]).CardInfo = actionStatus.AllRole.YourPublicInfo.BattleField.BattleMinions[i];
                Controls.Find("btnYou" + (i + 1).ToString(), true)[0].Left = LeftPos;
                ((ctlCard)Controls.Find("btnYou" + (i + 1).ToString(), true)[0]).FightClick += (x, y) =>
                {
                    Position.本方对方标识 = false;
                    //这里千万不能使用 i ,每次 i 都是固定值
                    //pos.Postion = i + 1;
                    Position.位置 = int.Parse(((Button)x).Parent.Name.Substring("btnYou".Length));
                    Close();
                };
                LeftPos += btnMe1.Width + Megrate;
            }
            btnMyHero.Enabled = actionStatus.AllRole.MyPublicInfo.能否成为动作对象;
            btnMyHero.Left = (Width - btnMyHero.Width) / 2;
            btnMyHero.Click += (x, y) =>
            {
                Position.本方对方标识 = true;
                Position.位置 = 0;
                Close();
            };
            btnYourHero.Enabled = actionStatus.AllRole.YourPublicInfo.能否成为动作对象; ;
            btnYourHero.Left = (Width - btnYourHero.Width) / 2;
            btnYourHero.Click += (x, y) =>
            {
                Position.本方对方标识 = false;
                Position.位置 = 0;
                Close();
            };
        }
    }
}
