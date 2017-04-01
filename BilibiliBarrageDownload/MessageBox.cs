using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BilibiliBarrageDownload
{
    public partial class MessageBoxFrm : Form
    {

        public MessageBoxFrm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MessageBox_Load(object sender, EventArgs e)
        {
            this.Text = Form1.MsgTitle;
            textBox1.Text = Form1.MsgContent;
            button1.Visible = Form1.MsgButton;
            this.TopMost = Form1.MsgTop;
        }
    }
}
