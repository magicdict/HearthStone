using System;
using System.Windows.Forms;

namespace 炉边传说
{
    public partial class ctlCard : UserControl
    {
        /// <summary>
        /// 随从
        /// </summary>
        public Card.CardBasicInfo CardInfo
        {
            set
            {
                lblName.Text = value.Name;
                lblDescription.Text = value.Description;
                switch (value.CardType)
                {
                    case Card.CardBasicInfo.CardTypeEnum.随从:
                        lblHealthPoint.Visible = true;
                        lblAttackPoint.Visible = true;
                        lblAttackPoint.Text = ((Card.MinionCard)value).TotalAttack().ToString();
                        lblHealthPoint.Text = ((Card.MinionCard)value).实际生命值.ToString();
                        break;
                    case Card.CardBasicInfo.CardTypeEnum.法术:
                        lblHealthPoint.Visible = false;
                        lblAttackPoint.Visible = true;
                        lblAttackPoint.Text = ((Card.AbilityCard)value).StandardCostPoint.ToString();
                        break;
                    case Card.CardBasicInfo.CardTypeEnum.奥秘:
                        break;
                    case Card.CardBasicInfo.CardTypeEnum.武器:
                        lblHealthPoint.Visible = true;
                        lblAttackPoint.Visible = true;
                        lblAttackPoint.Text = ((Card.WeaponCard)value).ActualAttackPoint.ToString();
                        lblHealthPoint.Text = ((Card.WeaponCard)value).实际耐久度.ToString();
                        break;
                    default:
                        break;
                }
            }
        }
        public Boolean CanAttack
        {
            set
            {
                if (value)
                {
                    btnFight.Visible = true;
                }
                else
                {
                    btnFight.Visible = false;
                }

            }
        }
        public ctlCard()
        {
            InitializeComponent();
        }
        public delegate void BtnClick(object sender, EventArgs e);
        public BtnClick FightClick; 
        private void btnFight_Click(object sender, EventArgs e)
        {
            FightClick(sender, e);
        }
    }
}
