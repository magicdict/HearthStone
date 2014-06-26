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
        Engine.Card.CardBasicInfo mHandCard = new Engine.Card.CardBasicInfo();
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] 
        public Engine.Card.CardBasicInfo HandCard
        {
            set
            {
                lblName.Text = value.名称;
                lblCostPoint.Text = value.使用成本.ToString();
                lblDescription.Text = value.描述;
                mHandCard = value;
                switch (value.卡牌种类)
                {
                    case Engine.Card.CardBasicInfo.卡牌类型枚举.随从:
                        lblHealthPoint.Visible = true;
                        lblHealthPoint.Text = ((Engine.Card.MinionCard)value).生命值上限.ToString();
                        lblAttackPoint.Visible = true;
                        lblAttackPoint.Text = ((Engine.Card.MinionCard)value).攻击力.ToString();
                        break;
                    case Engine.Card.CardBasicInfo.卡牌类型枚举.法术:
                        lblHealthPoint.Visible = false;
                        lblAttackPoint.Visible = false;
                        break;
                    case Engine.Card.CardBasicInfo.卡牌类型枚举.武器:
                        lblHealthPoint.Visible = true;
                        lblHealthPoint.Text = ((Engine.Card.WeaponCard)value).耐久度.ToString();
                        lblAttackPoint.Visible = true;
                        lblAttackPoint.Text = ((Engine.Card.WeaponCard)value).攻击力.ToString();
                        break;
                    case Engine.Card.CardBasicInfo.卡牌类型枚举.奥秘:
                        lblHealthPoint.Visible = false;
                        lblAttackPoint.Visible = false;
                        break;
                    case Engine.Card.CardBasicInfo.卡牌类型枚举.其他:
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
