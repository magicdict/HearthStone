using System.ComponentModel;
using System.Windows.Forms;

namespace 炉边传说
{
    public partial class ctlHero : UserControl
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] 
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
                lblSecret1.Visible = false;
                lblSecret2.Visible = false;
                lblSecret3.Visible = false;
                if (value.SecretCount == 1)
                {
                    lblSecret1.Visible = true;
                }
                if (value.SecretCount == 2)
                {
                    lblSecret1.Visible = true;
                    lblSecret2.Visible = true;
                }
                if (value.SecretCount == 3)
                {
                    lblSecret1.Visible = true;
                    lblSecret2.Visible = true;
                    lblSecret3.Visible = true;
                }
                lblSecret1.Left = (this.Width - lblSecret1.Width) / 2;
            }
        }
        public ctlHero()
        {
            InitializeComponent();
        }
    }
}
