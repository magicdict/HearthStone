using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Card;

namespace 炉边传说
{
    public partial class ctlCard : UserControl
    {
        /// <summary>
        /// 随从
        /// </summary>
        public Card.MinionCard Minion
        {
            set
            {
                lblName.Text = value.Name;
                lblDescription.Text = value.Description;
                lblAttackPoint.Text = value.TotalAttack().ToString();
                lblHealthPoint.Text = value.实际生命值.ToString();
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
