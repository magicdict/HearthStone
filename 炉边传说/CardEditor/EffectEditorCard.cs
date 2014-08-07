using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 炉边传说.CardEditor
{
    public partial class EffectEditorCard : Form
    {
        public EffectEditorCard()
        {
            InitializeComponent();
        }

        private void EffectEditorCard_Load(object sender, EventArgs e)
        {
            //Init Enum
            cmbDirect.Items.Clear();
            foreach (var item in Enum.GetValues(typeof(Engine.Utility.CardUtility.目标选择方向枚举)))
            {
                cmbDirect.Items.Add(item.ToString());
            }
            cmbDirect.SelectedIndex = 0;
        }
    }
}
