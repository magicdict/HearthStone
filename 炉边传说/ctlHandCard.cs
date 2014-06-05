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
    public partial class ctlHandCard : UserControl
    {
        Card.CardBasicInfo mHandCard = new Card.CardBasicInfo();
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] 
        public Card.CardBasicInfo HandCard
        {
            set
            {
                lblName.Text = value.Name;
                lblCostPoint.Text = value.ActualCostPoint.ToString();
                lblDescription.Text = value.Description;
                mHandCard = value;
                switch (value.CardType)
                {
                    case Card.CardBasicInfo.CardTypeEnum.随从:
                        lblHealthPoint.Visible = true;
                        lblHealthPoint.Text = ((Card.MinionCard)value).标准生命值上限.ToString();
                        lblAttackPoint.Visible = true;
                        lblAttackPoint.Text = ((Card.MinionCard)value).StandardAttackPoint.ToString();
                        break;
                    case Card.CardBasicInfo.CardTypeEnum.法术:
                        lblHealthPoint.Visible = false;
                        lblAttackPoint.Visible = false;
                        break;
                    case Card.CardBasicInfo.CardTypeEnum.武器:
                        lblHealthPoint.Visible = true;
                        lblHealthPoint.Text = ((Card.WeaponCard)value).标准耐久度.ToString();
                        lblAttackPoint.Visible = true;
                        lblAttackPoint.Text = ((Card.WeaponCard)value).StandardAttackPoint.ToString();
                        break;
                    case Card.CardBasicInfo.CardTypeEnum.奥秘:
                        lblHealthPoint.Visible = false;
                        lblAttackPoint.Visible = false;
                        break;
                    case Card.CardBasicInfo.CardTypeEnum.其他:
                        break;
                    default:
                        break;
                }
            }
            get
            {
                return mHandCard;
            }
        }

        public ctlHandCard()
        {
            InitializeComponent();
        }
        public delegate void BtnClick(object sender, EventArgs e);
        public BtnClick UseClick; 
        private void btnUse_Click(object sender, EventArgs e)
        {
            UseClick(sender, e);
        }
    }
}
