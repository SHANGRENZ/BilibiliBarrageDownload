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
    public partial class Setting : Form
    {
        string MyDocumentPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
        public Setting()
        {
            InitializeComponent();
        }

        private void Setting_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.DownloadPath == "")
            {
                textBox1.Text = MyDocumentPath;
            }
            else
            {
                textBox1.Text = Properties.Settings.Default.DownloadPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == MyDocumentPath && textBox1.Text == "")
            {
                Properties.Settings.Default.DownloadPath = "";
            }
            else
            {
                Properties.Settings.Default.DownloadPath = textBox1.Text;
            }
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            if (folderBrowserDialog1.SelectedPath != null)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }
    }
}
