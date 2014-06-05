using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace 炉边传说
{
    public partial class CardDeck : Form
    {
        public CardDeck()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 法术，奥秘
        /// </summary>
        private List<String> CardAblitiy = new List<string>();
        /// <summary>
        /// 随从，武器
        /// </summary>
        private List<String> CardMinion = new List<string>();
        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CardDeck_Load(object sender, EventArgs e)
        {
            foreach (var CardSn in Card.CardUtility.ReadyCardDic.Keys)
            {
                //AX00001 - X等于0的是可以使用的卡牌
                if (CardSn.StartsWith("A0") || CardSn.StartsWith("S0")) CardAblitiy.Add(CardSn);
                if (CardSn.StartsWith("M0") || CardSn.StartsWith("W0")) CardMinion.Add(CardSn);
            }
            AblityList.CardList = CardAblitiy;
            AblityList.PickCard = CardPicked;
            MinionList.CardList = CardMinion;
            MinionList.PickCard = CardPicked;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CardSN"></param>
        private void CardPicked(String CardSN)
        {
            lstSelectCardDeck.Items.Add(CardSN + ":" + Card.CardUtility.GetCardNameBySN(CardSN));
            lblCardCount.Text = "套牌数量：" + lstSelectCardDeck.Items.Count + "/30";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(Application.StartupPath + "\\CardDeck\\")){
                Directory.CreateDirectory(Application.StartupPath + "\\CardDeck\\");
            }
            StreamWriter newDeck = new StreamWriter(Application.StartupPath + "\\CardDeck\\" + txtCardDeckName.Text + ".txt",false);
            foreach (String item in lstSelectCardDeck.Items)
            {
                newDeck.WriteLine(item.Substring(0,7));
            }
            newDeck.Close();
            MessageBox.Show("保存完了");
            this.Close();
        }
    }
}
