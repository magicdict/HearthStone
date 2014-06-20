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
        private static ClientInfo client = new ClientInfo();
        private String DeckDir = Application.StartupPath + "\\CardDeck\\";
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
            if (!String.IsNullOrEmpty(txtNickName.Text)) client.PlayerNickName = txtNickName.Text;
            if (String.IsNullOrEmpty(cmbCardDeck.Text))
            {
                MessageBox.Show("请选择套牌");
                return;
            }
            String GameId;
            client.IsFirst = true;
            GameId = Engine.Client.ClientRequest.CreateGame(client.PlayerNickName);
            Engine.Utility.CardUtility.Init(txtCardPath.Text);
            client.game.GameId = int.Parse(GameId);
            var CardList = GetCardDeckList();
            Engine.Client.ClientRequest.SendDeck(int.Parse(GameId), client.IsFirst, CardList);
            btnJoinGame.Enabled = false;
            btnRefresh.Enabled = false;
            btnCreateGame.Enabled = false;
            Thread ServerThread;
            ServerThread = new Thread(frmStartGame.Wait);
            ServerThread.IsBackground = true;
            ServerThread.Start();
        }

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
        private static void Wait()
        {
            while (!Engine.Client.ClientRequest.IsGameStart(client.game.GameId.ToString(GameServer.GameIdFormat)))
            {
                Thread.Sleep(3000);
            }
            client.IsFirst = Engine.Client.ClientRequest.IsFirst(client.game.GameId.ToString(GameServer.GameIdFormat), client.IsFirst);
            client.game.InitPlayInfo();
            var t = new BattleField();
            t.client = client;
            t.ShowDialog();
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnJoinGame_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtServerIP.Text)) ClientRequest.strIP = txtServerIP.Text;
            if (!String.IsNullOrEmpty(txtNickName.Text)) client.PlayerNickName = txtNickName.Text;
            if (String.IsNullOrEmpty(cmbCardDeck.Text))
            {
                MessageBox.Show("请选择套牌");
                return;
            }
            client.IsFirst = false;
            if (lstWaitGuest.SelectedItems.Count != 1) return;
            var strWait = lstWaitGuest.SelectedItem.ToString();
            Engine.Utility.CardUtility.Init(txtCardPath.Text);
            String GameId = Engine.Client.ClientRequest.JoinGame(int.Parse(strWait.Substring(0, strWait.IndexOf("("))), client.PlayerNickName);
            var CardList = GetCardDeckList();
            Engine.Client.ClientRequest.SendDeck(int.Parse(GameId), client.IsFirst, CardList);
            client.game.GameId = int.Parse(GameId);
            client.IsFirst = Engine.Client.ClientRequest.IsFirst(client.game.GameId.ToString(GameServer.GameIdFormat), client.IsFirst);
            client.game.InitPlayInfo();
            var t = new BattleField();
            t.client = client;
            t.ShowDialog();
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
            txtNickName.Text = client.PlayerNickName;
            cmbCardDeck.Items.Clear();
            if (Directory.Exists(DeckDir))
            {
                foreach (var item in Directory.GetFiles(DeckDir))
                {
                    cmbCardDeck.Items.Add(item.Substring(DeckDir.Length).TrimEnd(".txt".ToCharArray()));
                }
            }
        }
        private void btnPickCard_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog cardPath = new FolderBrowserDialog();
            if (cardPath.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtCardPath.Text = cardPath.SelectedPath;
            }
        }

        private void btnCreateCard_Click(object sender, EventArgs e)
        {
            (new frmExport()).ShowDialog();
        }

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

        private void btnServer_Click(object sender, EventArgs e)
        {
            (new ServerConfig()).ShowDialog();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTestCrystal_Click(object sender, EventArgs e)
        {
            //DEBUG START
            client.game.HostInfo.crystal.CurrentFullPoint = (int)crystalCount.Value;
            client.game.HostInfo.crystal.CurrentRemainPoint = (int)crystalCount.Value;
            client.game.GuestInfo.crystal.CurrentFullPoint = (int)crystalCount.Value;
            client.game.GuestInfo.crystal.CurrentRemainPoint = (int)crystalCount.Value;
            //DEBUG END
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTestHandCard_Click(object sender, EventArgs e)
        {
            client.game.HostSelfInfo.handCards.Add(Engine.Utility.CardUtility.GetCardInfoBySN(cmbHandCard.Text.Substring(0, 7)));
        }
    }
}
