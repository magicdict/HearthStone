using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 炉边传说
{
    public partial class ManagerTool : Form
    {
        public ManagerTool()
        {
            InitializeComponent();
        }

        private void hTTP服务器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new ServerConfig()).Show();
        }
    }
}
