using System.Windows.Forms;

namespace 炉边传说
{
    public partial class ctlHeroAbility : UserControl
    {
        public ctlHeroAbility()
        {
            InitializeComponent();
        }
        public bool IsEnable
        {
            set
            {
                if (value)
                {
                    lblEnable.BackColor = System.Drawing.Color.LightGreen;
                }
                else
                {
                    lblEnable.BackColor = System.Drawing.Color.LightPink;
                }
            }
        } 
    }
}
