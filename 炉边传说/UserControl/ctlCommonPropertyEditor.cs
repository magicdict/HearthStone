using System.Windows.Forms;
using Engine.Card;

namespace 炉边传说.usrControl
{
    public partial class ctlCommonPropertyEditor : UserControl
    {
        public ctlCommonPropertyEditor()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 将对象表示在UI上
        /// </summary>
        /// <param name="card"></param>
        public void GetBasicInfo(MinionCard card)
        {
            numCost.Value = card.使用成本;
            txtName.Text = card.名称;
            txtDescription.Text = card.描述;
        }
        /// <summary>
        /// 将UI数据设定到对象上
        /// </summary>
        /// <param name="card"></param>
        public void SetBasicInfo(CardBasicInfo card)
        {
            card.使用成本 = (int)numCost.Value;
            card.名称 = txtName.Text;
            card.描述 = txtDescription.Text;
        }
    }
}
