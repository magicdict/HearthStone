using Engine.Client;
using Engine.Control;
using Engine.Server;
using Engine.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
namespace 炉边传说
{
    public partial class frmStartGame : Form
    {
        /// <summary>
        /// frmStartGame
        /// </summary>
        public frmStartGame()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 开始游戏的请求
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreateGame_Click(object sender, EventArgs e)
        {
            //新建游戏的时候，已经决定游戏的先后手
            if (!String.IsNullOrEmpty(txtServerIP.Text)) TcpSocketServer.strIP = txtServerIP.Text;
            if (!String.IsNullOrEmpty(txtNickName.Text)) GameManager.MyClientManager.PlayerNickName = txtNickName.Text;
            if (String.IsNullOrEmpty(cmbCardDeck.Text))
            {
                MessageBox.Show("请选择套牌");
                return;
            }
            SystemManager.游戏类型 = Engine.Utility.SystemManager.GameType.客户端服务器版;
            GameManager.MyClientManager.actionStatus.IsHost = true;
            String GameId = ClientRequest.CreateGame(GameManager.MyClientManager.PlayerNickName);
            Engine.Utility.CardUtility.Init(txtCardPath.Text);
            GameManager.MyClientManager.GameId = int.Parse(GameId);
            var CardList = GetCardDeckList();
            ClientRequest.SendDeck(GameManager.MyClientManager.GameId.ToString(GameServer.GameIdFormat), GameManager.MyClientManager.actionStatus.IsHost, CardList);
            btnJoinGame.Enabled = false;
            btnRefresh.Enabled = false;
            btnCreateGame.Enabled = false;
            int Count = 0;
            while (!ClientRequest.IsGameStart(GameManager.MyClientManager.GameId.ToString(GameServer.GameIdFormat)) && Count <= 10)
            {
                Thread.Sleep(3000);
                Count++;
            }
            if (Count == 11)
            {
                MessageBox.Show("找不到对手");
            }
            else
            {
                GameManager.MyClientManager.IsFirst = ClientRequest.IsFirst(GameManager.MyClientManager.GameId.ToString(GameServer.GameIdFormat), true);
                GameManager.MyClientManager.IsStart = true;
                this.Close();
            }
        }
        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            lstWaitGuest.Items.Clear();
            String WaitGame = ClientRequest.GetWatiGameList();
            String[] WaitGameArray = WaitGame.Split(Engine.Utility.CardUtility.strSplitArrayMark.ToCharArray());
            for (int i = 0; i < WaitGameArray.Length; i++)
            {
                lstWaitGuest.Items.Add(WaitGameArray[i]);
            }
        }
        /// <summary>
        /// 加入一局游戏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnJoinGame_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtServerIP.Text)) TcpSocketServer.strIP = txtServerIP.Text;
            if (!String.IsNullOrEmpty(txtNickName.Text)) GameManager.MyClientManager.PlayerNickName = txtNickName.Text;
            if (lstWaitGuest.SelectedItems.Count != 1) return;
            if (String.IsNullOrEmpty(cmbCardDeck.Text))
            {
                MessageBox.Show("请选择套牌");
                return;
            }
            SystemManager.游戏类型 = Engine.Utility.SystemManager.GameType.客户端服务器版;
            GameManager.MyClientManager.actionStatus.IsHost = false;
            var strWait = lstWaitGuest.SelectedItem.ToString();
            GameManager.MyClientManager.GameId = int.Parse(strWait.Substring(0, strWait.IndexOf("(")));
            ClientRequest.JoinGame(GameManager.MyClientManager.GameId.ToString(GameServer.GameIdFormat), GameManager.MyClientManager.PlayerNickName);
            GameManager.MyClientManager.IsFirst = ClientRequest.IsFirst(GameManager.MyClientManager.GameId.ToString(GameServer.GameIdFormat), GameManager.MyClientManager.actionStatus.IsHost);
            Engine.Utility.CardUtility.Init(txtCardPath.Text);
            var CardList = GetCardDeckList();
            ClientRequest.SendDeck(GameManager.MyClientManager.GameId.ToString(GameServer.GameIdFormat), GameManager.MyClientManager.actionStatus.IsHost, CardList);
            GameManager.MyClientManager.IsStart = true;
            this.Close();
        }

        #region"单机游戏"
        /// <summary>
        /// 单机游戏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSingleGame_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(cmbCardDeck.Text))
            {
                MessageBox.Show("请选择套牌");
                return;
            }
            Engine.Utility.CardUtility.Init(txtCardPath.Text);
            GameManager.MyFullServerManager = new FullServerManager(1, txtNickName.Text);
            GameManager.CreateSingleGame(GetCardDeckList());
            this.Close();
        }
        /// <summary>
        /// 塔防游戏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSingleGameDefance_Click(object sender, EventArgs e)
        {
            SystemManager.MaxHealthPoint = Engine.Utility.CardUtility.Max;
            if (String.IsNullOrEmpty(cmbCardDeck.Text))
            {
                MessageBox.Show("请选择套牌");
                return;
            }
            Engine.Utility.CardUtility.Init(txtCardPath.Text);
            GameManager.MyFullServerManager = new FullServerManager(1, txtNickName.Text);

            GameManager.CreateSingleGameDefance();
            this.Close();
        }

 
        #endregion

        #region"其他"
        /// <summary>
        /// 套牌目录
        /// </summary>
        String DeckDir = Application.StartupPath + "\\CardDeck\\";
        /// <summary>
        /// Load/Init
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmStartGame_Load(object sender, EventArgs e)
        {
            //DEBUG START
            txtCardPath.Text = @"C:\炉石Git\炉石设计\Card";
            Engine.Utility.CardUtility.Init(txtCardPath.Text);
            foreach (var CardSn in Engine.Utility.CardUtility.ReadyCardDic)
            {
                cmbHandCard.Items.Add(CardSn.Key + "(" + CardSn.Value + ")");
            }
            //DEBUG END
            txtNickName.Text = "PlayerNickName";
            cmbCardDeck.Items.Clear();
            if (Directory.Exists(DeckDir))
            {
                foreach (var item in Directory.GetFiles(DeckDir))
                {
                    cmbCardDeck.Items.Add(item.Substring(DeckDir.Length).TrimEnd(".txt".ToCharArray()));
                }
            }
        }
        /// <summary>
        /// 获取套牌
        /// </summary>
        /// <returns></returns>
        private List<string> GetCardDeckList()
        {
            var CardList = new List<String>();
            StreamReader fileReader = new StreamReader(DeckDir + cmbCardDeck.Text + ".txt");
            while (!fileReader.EndOfStream)
            {
                CardList.Add(fileReader.ReadLine());
            }
            fileReader.Close();
            return CardList;
        }
        private void btnPickCard_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog cardPath = new FolderBrowserDialog();
            if (cardPath.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtCardPath.Text = cardPath.SelectedPath;
            }
        }
        /// <summary>
        /// 卡牌资料导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreateCard_Click(object sender, EventArgs e)
        {
            (new frmExport()).ShowDialog();
        }
        /// <summary>
        /// 套牌制作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreateCardDeck_Click(object sender, EventArgs e)
        {
            Engine.Utility.CardUtility.Init(txtCardPath.Text);
            (new CardDeck()).ShowDialog();
            cmbCardDeck.Items.Clear();
            if (Directory.Exists(DeckDir))
            {
                foreach (var item in Directory.GetFiles(DeckDir))
                {
                    cmbCardDeck.Items.Add(item.Substring(DeckDir.Length).TrimEnd(".txt".ToCharArray()));
                }
            }
        }
        /// <summary>
        /// 服务器界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnServer_Click(object sender, EventArgs e)
        {
            (new ServerConfig()).ShowDialog();
        }
        /// <summary>
        /// 添加测试水晶
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTestCrystal_Click(object sender, EventArgs e)
        {
            //DEBUG START
            GameManager.MyClientManager.actionStatus.AllRole.MyPublicInfo.crystal.CurrentFullPoint = (int)crystalCount.Value;
            GameManager.MyClientManager.actionStatus.AllRole.MyPublicInfo.crystal.CurrentRemainPoint = (int)crystalCount.Value;
            GameManager.MyClientManager.actionStatus.AllRole.YourPublicInfo.crystal.CurrentFullPoint = (int)crystalCount.Value;
            GameManager.MyClientManager.actionStatus.AllRole.YourPublicInfo.crystal.CurrentRemainPoint = (int)crystalCount.Value;
            //DEBUG END
        }
        /// <summary>
        /// 添加测试手牌
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTestHandCard_Click(object sender, EventArgs e)
        {
            GameManager.MyClientManager.actionStatus.AllRole.MyPrivateInfo.handCards.Add(Engine.Utility.CardUtility.GetCardInfoBySN(cmbHandCard.Text.Substring(0, 7)));
        }
        #endregion

    }
}
