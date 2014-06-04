using System;
using System.Windows.Forms;

namespace 炉边传说
{
    public partial class ctlCrystalBar : UserControl
    {
        public Card.Client.Crystal CrystalInfo
        {
            set
            {
                lblInfo.Text = value.CurrentRemainPoint.ToString() + "/" + value.CurrentFullPoint.ToString();
                lblImage.Text = String.Empty;
                for (int i = 0; i < value.CurrentRemainPoint; i++)
                {
                    lblImage.Text += "●";
                }
                for (int i = 0; i < value.CurrentFullPoint - value.CurrentRemainPoint; i++)
                {
                    lblImage.Text += "○";
                }
            }
        }
        public ctlCrystalBar()
        {
            InitializeComponent();
        }
    }
}
