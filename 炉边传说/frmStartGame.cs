using Engine.Client;
using Engine.Server;
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
        /// 
        /// </summary>
        private String DeckDir = Application.StartupPath + "\\CardDeck\\";
        /// <summary>
        /// 
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
            if (!String.IsNullOrEmpty(txtServerIP.Text)) ClientRequest.strIP = txtServerIP.Text;
            if (!String.IsNullOrEmpty(txtNickName.Text)) ClientInfo.PlayerNickName = txtNickName.Text;
            if (String.IsNullOrEmpty(cmbCardDeck.Text))
            {
                MessageBox.Show("请选择套牌");
                return;
            }
            ClientInfo.IsHost = true;
            String GameId = Engine.Client.ClientRequest.CreateGame(ClientInfo.PlayerNickName);
            Engine.Utility.CardUtility.Init(txtCardPath.Text);
            ClientInfo.gamestatus.GameId = int.Parse(GameId);
            var CardList = GetCardDeckList();
            Engine.Client.ClientRequest.SendDeck(int.Parse(GameId), ClientInfo.IsHost, CardList);
            btnJoinGame.Enabled = false;
            btnRefresh.Enabled = false;
            btnCreateGame.Enabled = false;
            int Count = 0;
            while (!Engine.Client.ClientRequest.IsGameStart(ClientInfo.gamestatus.GameId.ToString(GameServer.GameIdFormat)) && Count <= 10)
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
                ClientInfo.IsFirst = Engine.Client.ClientRequest.IsFirst(ClientInfo.gamestatus.GameId.ToString(GameServer.GameIdFormat), ClientInfo.IsHost);
                ClientInfo.gameType = Engine.Utility.SystemManager.GameType.客户端服务器版;
                ClientInfo.IsStart = true;
                this.Close();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            lstWaitGuest.Items.Clear();
            String WaitGame = Engine.Client.ClientRequest.GetWatiGameList();
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
            if (!String.IsNullOrEmpty(txtServerIP.Text)) ClientRequest.strIP = txtServerIP.Text;
            if (!String.IsNullOrEmpty(txtNickName.Text)) ClientInfo.PlayerNickName = txtNickName.Text;
            if (String.IsNullOrEmpty(cmbCardDeck.Text))
            {
                MessageBox.Show("请选择套牌");
                return;
            }
            ClientInfo.IsHost = false;
            if (lstWaitGuest.SelectedItems.Count != 1) return;
            var strWait = lstWaitGuest.SelectedItem.ToString();
            Engine.Utility.CardUtility.Init(txtCardPath.Text);
            String GameId = Engine.Client.ClientRequest.JoinGame(int.Parse(strWait.Substring(0, strWait.IndexOf("("))), ClientInfo.PlayerNickName);
            var CardList = GetCardDeckList();
            Engine.Client.ClientRequest.SendDeck(int.Parse(GameId), ClientInfo.IsHost, CardList);
            ClientInfo.gamestatus.GameId = int.Parse(GameId);
            ClientInfo.IsFirst = Engine.Client.ClientRequest.IsFirst(ClientInfo.gamestatus.GameId.ToString(GameServer.GameIdFormat), ClientInfo.IsHost);
            ClientInfo.gameType = Engine.Utility.SystemManager.GameType.客户端服务器版;
            ClientInfo.IsStart = true;
            this.Close();
        }
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
            ClientInfoSingle.HostAsFirst = (DateTime.Now.Millisecond % 2) == 0;
            ClientInfoSingle.IsStart = true;
            var CardList = GetCardDeckList();

            var CardStackFirst = new Stack<String>();
            foreach (String card in CardList)
            {
                CardStackFirst.Push(card);
            }
            ClientInfoSingle.remoteGame.SetCardStack(true, CardStackFirst);

            var CardStackSecond = new Stack<String>();
            foreach (String card in CardList)
            {
                CardStackSecond.Push(card);
            }
            ClientInfoSingle.remoteGame.SetCardStack(false, CardStackSecond);
            this.Close();
        }
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
            txtNickName.Text = ClientInfo.PlayerNickName;
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
            ClientInfo.gamestatus.HostInfo.crystal.CurrentFullPoint = (int)crystalCount.Value;
            ClientInfo.gamestatus.HostInfo.crystal.CurrentRemainPoint = (int)crystalCount.Value;
            ClientInfo.gamestatus.GuestInfo.crystal.CurrentFullPoint = (int)crystalCount.Value;
            ClientInfo.gamestatus.GuestInfo.crystal.CurrentRemainPoint = (int)crystalCount.Value;
            //DEBUG END
        }
        /// <summary>
        /// 添加测试手牌
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTestHandCard_Click(object sender, EventArgs e)
        {
            ClientInfo.gamestatus.HostSelfInfo.handCards.Add(Engine.Utility.CardUtility.GetCardInfoBySN(cmbHandCard.Text.Substring(0, 7)));
        }


    }
}
