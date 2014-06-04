using System.Windows.Forms;

namespace 炉边传说
{
    public partial class ctlHero : UserControl
    {
        public Card.Client.PlayerBasicInfo Hero
        {
            set
            {
                lblHealthPoint.Text = value.HealthPoint.ToString();
                if (value.ShieldPoint == 0)
                {
                    lblShieldPoint.Visible = false;
                }
                else
                {
                    lblShieldPoint.Visible = true;
                    lblShieldPoint.Text = value.ShieldPoint.ToString();
                }
            }
        }
        public ctlHero()
        {
            InitializeComponent();
        }
    }
}
