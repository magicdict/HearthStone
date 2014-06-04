using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 炉边传说
{
    public partial class ctlCardList : UserControl
    {
        /// <summary>
        /// 最大随从页
        /// </summary>
        private int MaxPage = 1;
        /// <summary>
        /// 最大法术页
        /// </summary>
        private int CurrentPage = 1;

        List<String> mCardList = new List<string>();

        /// <summary>
        /// 
        /// </summary>
        public List<String> CardList
        {
            set
            {
                MaxPage = (int)Math.Ceiling((double)((double)value.Count / (double)10));
                CurrentPage = 1;
                mCardList = value;
                ResetUI();
            }
        }

        public ctlCardList()
        {
            InitializeComponent();
        }

        private void ctlCardList_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                ((ctlHandCard)Controls.Find("btnHandCard" + (i + 1).ToString(), true)[0]).UseClick += (x, y) =>
                {
                    PickCard(((ctlHandCard)((Button)x).Parent).HandCard.SN);
                };
            }
            ResetUI();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            CurrentPage++;
            ResetUI();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            CurrentPage--;
            ResetUI();
        }

        public delegate void delegatePickCard(String CardSn);
        public delegatePickCard PickCard;

        /// <summary>
        /// 
        /// </summary>
        private void ResetUI()
        {
            btnPrevious.Enabled = true;
            btnNext.Enabled = true;
            if (CurrentPage == 1) btnPrevious.Enabled = false;
            if (CurrentPage == MaxPage) btnNext.Enabled = false;
            if (mCardList.Count != 0)
            {
                //CurrentPage = 2  11-20
                int fromNo = (CurrentPage - 1) * 10 + 1;
                int toNo = Math.Min(mCardList.Count, CurrentPage * 10);
                //ClearContent
                for (int i = 0; i < 10; i++)
                {
                    Controls.Find("btnHandCard" + (i + 1).ToString(), true)[0].Visible = false;
                }

                for (int i = fromNo; i < toNo + 1; i++)
                {
                    Controls.Find("btnHandCard" + (i - (CurrentPage - 1) * 10).ToString(), true)[0].Visible = true;
                    ((ctlHandCard)Controls.Find("btnHandCard" + (i - (CurrentPage - 1) * 10).ToString(), true)[0]).HandCard =
                        Card.CardUtility.GetCardInfoBySN(mCardList[i - 1]);
                }
            }
        }
    }
}
