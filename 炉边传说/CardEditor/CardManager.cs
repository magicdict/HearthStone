using Engine.Utility;
using System;
using System.Windows.Forms;
using Engine.Card;

namespace 炉边传说.CardEditor
{
    public partial class CardManager : Form
    {
        public CardManager()
        {
            InitializeComponent();
        }

        delegate bool Filter(CardBasicInfo card);
        Filter CardFilter = (x) => { return true; };

        private void CardManager_Load(object sender, EventArgs e)
        {
            //HardCoding Only for Test
            CardUtility.Init(@"C:\炉石Git\炉石设计\Card");
            //init fillter
            cmbCardFilter.Items.Clear();
            cmbCardFilter.Items.Add("全部");
            cmbCardFilter.Items.Add("随从");
            cmbCardFilter.Items.Add("法术");
            cmbCardFilter.Items.Add("武器");
            cmbCardFilter.SelectedIndex = 0;

            //Set Header of ListView
            lstCards.Clear();
            lstCards.Columns.Add("序列号");
            lstCards.Columns.Add("名称");
            lstCards.Columns.Add("描述");
            lstCards.Columns.Add("使用成本");
            lstCards.Columns.Add("分类");
            lstCards.Columns.Add("细分");
            SetCardListView();
        }

        private void SetCardListView()
        {
            //SetData
            lstCards.Items.Clear();
            foreach (var card in CardUtility.CardCollections.Values)
            {
                if (!CardFilter(card)) continue;
                ListViewItem item = new ListViewItem(card.序列号);
                item.SubItems.Add(card.名称);
                item.SubItems.Add(card.描述);
                item.SubItems.Add(card.使用成本.ToString());
                item.SubItems.Add(card.卡牌种类.ToString());
                item.SubItems.Add("");
                lstCards.Items.Add(item);
            }
            //Set ColWidth
            lstCards.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void cmbCardFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbCardFilter.SelectedIndex)
            {
                case 0:
                    //"全部"
                    CardFilter = (x) => { return true; };
                    break;
                case 1:
                    //"随从"
                    CardFilter = (x) => { return x.卡牌种类 == CardBasicInfo.资源类型枚举.随从; };
                    break;
                case 2:
                    //"法术"
                    CardFilter = (x) => { return x.卡牌种类 == CardBasicInfo.资源类型枚举.法术; };
                    break;
                case 3:
                    // "武器"
                    CardFilter = (x) => { return x.卡牌种类 == CardBasicInfo.资源类型枚举.武器; };
                    break;
                default:
                    break;
            }
            SetCardListView();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            //编辑项目
            if (lstCards.SelectedItems.Count == 1)
            {
                CardBasicInfo EditCard = CardUtility.GetCardInfoBySN(lstCards.SelectedItems[0].Text);
                switch (EditCard.卡牌种类)
                {
                    case CardBasicInfo.资源类型枚举.随从:
                        (new MinionCardEditor(EditCard.序列号)).ShowDialog();
                        break;
                    case CardBasicInfo.资源类型枚举.法术:
                        break;
                    case CardBasicInfo.资源类型枚举.武器:
                        break;
                    case CardBasicInfo.资源类型枚举.奥秘:
                        break;
                    case CardBasicInfo.资源类型枚举.其他:
                        break;
                    default:
                        break;
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //只是将ReadyFlg设置为False，不会物理删除
            if (lstCards.SelectedItems.Count == 1)
            {

            }
        }
    }
}
