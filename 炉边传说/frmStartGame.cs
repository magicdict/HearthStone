using Card.Client;
using Card.Server;
using System;
using System.Threading;
using System.Windows.Forms;
namespace 炉边传说
{
    public partial class frmStartGame : Form
    {
        private static GameManager game = new GameManager();
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
            if (!String.IsNullOrEmpty(txtNickName.Text)) game.PlayerNickName = txtNickName.Text;
            String GameId;
            game.IsHost = true;
            GameId = Card.Client.ClientRequest.CreateGame(game.PlayerNickName);
            Card.CardUtility.Init(txtCardPath.Text);
            game.GameId = int.Parse(GameId);
            btnJoinGame.Enabled = false;
            btnRefresh.Enabled = false;
            btnCreateGame.Enabled = false;
            Thread ServerThread;
            ServerThread = new Thread(frmStartGame.Wait);
            ServerThread.IsBackground = true;
            ServerThread.Start();
        }
        private static void Wait()
        {
            while (!Card.Client.ClientRequest.IsGameStart(game.GameId.ToString(GameServer.GameIdFormat)))
            {
                Thread.Sleep(3000);
            }
            game.IsFirst = Card.Client.ClientRequest.IsFirst(game.GameId.ToString(GameServer.GameIdFormat), game.IsHost);
            game.Init();
            var t = new BattleField();
            t.game = game;
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
            String WaitGame = Card.Client.ClientRequest.GetWatiGameList();
            String[] WaitGameArray = WaitGame.Split(Card.CardUtility.strSplitArrayMark.ToCharArray());
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
            if (!String.IsNullOrEmpty(txtNickName.Text)) game.PlayerNickName = txtNickName.Text;

            game.IsHost = false;
            if (lstWaitGuest.SelectedItems.Count != 1) return;
            var strWait = lstWaitGuest.SelectedItem.ToString();
            Card.CardUtility.Init(txtCardPath.Text);
            String GameId = Card.Client.ClientRequest.JoinGame(int.Parse(strWait.Substring(0, strWait.IndexOf("("))), game.PlayerNickName);
            game.GameId = int.Parse(GameId);
            game.IsFirst = Card.Client.ClientRequest.IsFirst(game.GameId.ToString(GameServer.GameIdFormat), game.IsHost);
            game.Init();
            var t = new BattleField();
            t.game = game;
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
            //DEBUG
            txtCardPath.Text = @"C:\炉石Git\CardHelper\CardXML";
            txtNickName.Text = game.PlayerNickName;
        }
        private void btnPickCard_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog cardPath = new FolderBrowserDialog();
            if (cardPath.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtCardPath.Text = cardPath.SelectedPath;
            }
        }
    }
}
