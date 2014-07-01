using Engine.Client;
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
        /// 游戏管理者
        /// </summary>
        public GameManager MyGameManager = new GameManager();
        /// <summary>
        /// 开始游戏的请求
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreateGame_Click(object sender, EventArgs e)
        {
            //新建游戏的时候，已经决定游戏的先后手
            if (!String.IsNullOrEmpty(txtServerIP.Text)) TcpSocketServer.strIP = txtServerIP.Text;
            if (!String.IsNullOrEmpty(txtNickName.Text)) MyGameManager.gameStatus.client.PlayerNickName = txtNickName.Text;
            if (String.IsNullOrEmpty(cmbCardDeck.Text))
            {
                MessageBox.Show("请选择套牌");
                return;
            }
            GameManager.游戏类型 = Engine.Utility.SystemManager.GameType.客户端服务器版;
            MyGameManager.gameStatus.client.IsHost = true;
            String GameId = Engine.Client.ClientRequest.CreateGame(MyGameManager.gameStatus.client.PlayerNickName);
            Engine.Utility.CardUtility.Init(txtCardPath.Text);
            MyGameManager.GameId = int.Parse(GameId);
            MyGameManager.gameStatus.GameId = int.Parse(GameId);
            var CardList = GetCardDeckList();
            Engine.Client.ClientRequest.SendDeck(MyGameManager.GameId.ToString(GameServer.GameIdFormat), MyGameManager.gameStatus.client.IsHost, CardList);
            btnJoinGame.Enabled = false;
            btnRefresh.Enabled = false;
            btnCreateGame.Enabled = false;
            int Count = 0;
            while (!Engine.Client.ClientRequest.IsGameStart(MyGameManager.GameId.ToString(GameServer.GameIdFormat)) && Count <= 10)
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
                MyGameManager.gameStatus.client.IsFirst = Engine.Client.ClientRequest.IsFirst(MyGameManager.GameId.ToString(GameServer.GameIdFormat), true);
                GameManager.IsStart = true;
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
            if (!String.IsNullOrEmpty(txtServerIP.Text)) TcpSocketServer.strIP = txtServerIP.Text;
            if (!String.IsNullOrEmpty(txtNickName.Text)) MyGameManager.gameStatus.client.PlayerNickName = txtNickName.Text;
            if (lstWaitGuest.SelectedItems.Count != 1) return;
            if (String.IsNullOrEmpty(cmbCardDeck.Text))
            {
                MessageBox.Show("请选择套牌");
                return;
            }
            GameManager.游戏类型 = Engine.Utility.SystemManager.GameType.客户端服务器版;
            MyGameManager.gameStatus.client.IsHost = false;
            var strWait = lstWaitGuest.SelectedItem.ToString();
            MyGameManager.GameId = int.Parse(strWait.Substring(0, strWait.IndexOf("(")));
            MyGameManager.gameStatus.GameId = MyGameManager.GameId;
            Engine.Client.ClientRequest.JoinGame(MyGameManager.GameId.ToString(GameServer.GameIdFormat), MyGameManager.gameStatus.client.PlayerNickName);
            MyGameManager.gameStatus.client.IsFirst = Engine.Client.ClientRequest.IsFirst(MyGameManager.GameId.ToString(GameServer.GameIdFormat), MyGameManager.gameStatus.client.IsHost);
            Engine.Utility.CardUtility.Init(txtCardPath.Text);
            var CardList = GetCardDeckList();
            Engine.Client.ClientRequest.SendDeck(MyGameManager.GameId.ToString(GameServer.GameIdFormat), MyGameManager.gameStatus.client.IsHost, CardList);
            GameManager.IsStart = true;
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
            GameManager.游戏类型 = Engine.Utility.SystemManager.GameType.单机版;
            GameManager.游戏模式 = Engine.Utility.SystemManager.GameMode.标准;
            MyGameManager.gameStatus.client.IsHost = true;
            MyGameManager.SimulateServer = new RemoteGameManager(0, txtNickName.Text, Engine.Utility.SystemManager.GameType.单机版);
            MyGameManager.SimulateServer.serverinfo.Init();
            MyGameManager.SimulateServer.serverinfo.HostAsFirst = (DateTime.Now.Millisecond % 2) == 0;
            MyGameManager.gameStatus.client.IsFirst = MyGameManager.SimulateServer.serverinfo.HostAsFirst;
            var CardList = GetCardDeckList();
            var CardStackFirst = new Stack<String>();
            foreach (String card in CardList)
            {
                CardStackFirst.Push(card);
            }
            MyGameManager.SimulateServer.SetCardStack(true, CardStackFirst);

            var CardStackSecond = new Stack<String>();
            foreach (String card in CardList)
            {
                CardStackSecond.Push(card);
            }
            MyGameManager.SimulateServer.SetCardStack(false, CardStackSecond);
            GameManager.IsStart = true;
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
            Engine.Utility.CardUtility.Init(txtCardPath.Text);
            GameManager.游戏类型 = Engine.Utility.SystemManager.GameType.单机版;
            GameManager.游戏模式 = Engine.Utility.SystemManager.GameMode.塔防;
            MyGameManager.gameStatus.client.IsHost = true;
            MyGameManager.SimulateServer = new RemoteGameManager(0, txtNickName.Text, Engine.Utility.SystemManager.GameType.单机版);
            MyGameManager.SimulateServer.serverinfo.Init();
            MyGameManager.SimulateServer.serverinfo.HostAsFirst = true;
            MyGameManager.gameStatus.client.IsFirst = true;
            GameManager.事件处理组件.事件特殊处理 += (x) =>
            {
                foreach (var item in GameManager.事件处理组件.事件池)
                {
                    if (item.触发事件类型 == Engine.Utility.CardUtility.事件类型枚举.死亡 && item.触发位置.本方对方标识 == false)
                    {
                        x.client.MyInfo.LifePoint++;
                    }
                }
            };
            var CardStackSecond = new Stack<String>();
            for (int i = 20; i >= 1; i--)
            {
                for (int j = 0; j < 7; j++)
                {
                    CardStackSecond.Push("M9100" + i.ToString("D2"));
                }
            }
            //不随机
            MyGameManager.SimulateServer.serverinfo.GuestCardDeck.CardList = CardStackSecond;
            var CardStackFirst = new Stack<String>();
            for (int i = 0; i < 20; i++)
            {
                foreach (var card in Engine.Utility.CardUtility.ReadyCardDic.Keys)
                {
                    if (card.Substring(1, 1) == "0") CardStackFirst.Push(card);
                }
            }
            MyGameManager.SimulateServer.SetCardStack(true, CardStackFirst);
            GameManager.IsStart = true;
            this.Close();
        }
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
            MyGameManager.gameStatus.client.HostInfo.crystal.CurrentFullPoint = (int)crystalCount.Value;
            MyGameManager.gameStatus.client.HostInfo.crystal.CurrentRemainPoint = (int)crystalCount.Value;
            MyGameManager.gameStatus.client.GuestInfo.crystal.CurrentFullPoint = (int)crystalCount.Value;
            MyGameManager.gameStatus.client.GuestInfo.crystal.CurrentRemainPoint = (int)crystalCount.Value;
            //DEBUG END
        }
        /// <summary>
        /// 添加测试手牌
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTestHandCard_Click(object sender, EventArgs e)
        {
            MyGameManager.gameStatus.client.HostSelfInfo.handCards.Add(Engine.Utility.CardUtility.GetCardInfoBySN(cmbHandCard.Text.Substring(0, 7)));
        }


    }
}
