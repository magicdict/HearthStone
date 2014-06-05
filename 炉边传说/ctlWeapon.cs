using System.ComponentModel;
using System.Windows.Forms;

namespace 炉边传说
{
    public partial class ctlWeapon : UserControl
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] 
        public Card.WeaponCard Weapon
        {
            set
            {
                this.Visible = true;
                lblHealthPoint.Visible = true;
                lblAttackPoint.Visible = true;
                lblHealthPoint.Text = value.实际耐久度.ToString();
                lblAttackPoint.Text = value.ActualAttackPoint.ToString();
            }
        }
        public ctlWeapon()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 清除
        /// </summary>
        internal void Clear()
        {
            this.Visible = false;
            lblHealthPoint.Visible = false;
            lblAttackPoint.Visible = false;
        }
    }
}
