using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BilibiliBarrageDownload
{
    public partial class Update : Form
    {
        string UpdateLink;
        public Update()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GetUpdateInfo();
        }
        private bool GetUpdateInfo()
        {
            string[] updateinfo = Regex.Split(GetSource("https://api.zhoushangren.com/BilibiliBarrageDown/update"),"\n");
            if (Convert.ToDouble(updateinfo[0]) == Form1.version)
            {
                textBox1.Text = "[*]已经是最新版本了!";
                button1.Enabled = false;
                return false;
            }
            textBox1.Text = "[*]发现新版本(v" + updateinfo[0] + ")!\r\n\r\n更新日期：" + updateinfo[1] + "\r\n\r\n更新内容：\r\n\r\n";
            UpdateLink = updateinfo[2];
            if (updateinfo.Length > 4)
            {
                for(int x = 3; x < updateinfo.Length; x++)
                {
                    textBox1.Text += updateinfo[x] + "\r\n";
                }
            }
            else
            {
                if (updateinfo[3] == "none")
                {
                    textBox1.Text += "没有更新内容";
                }
                else
                {
                    textBox1.Text += updateinfo[3];
                }
            }
            button2.Enabled = true;
            return true;
        }
        private bool DownloadUpdate()
        {
            string AppPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string FullPath = this.GetType().Assembly.Location;
            string AppName = System.IO.Path.GetFileName(FullPath);
            WebClient myWebclient = new WebClient();
            myWebclient.DownloadFile(UpdateLink, FullPath + "_UPDATE");
            string CommText = "ping 127.0.0.1 -n 1 >nul\r\n";
            CommText += "del \""+ FullPath + "\"\r\n";
            CommText += "ren \"" + FullPath + "_UPDATE\" \"" + AppName + "\"\r\n";
            CommText += "start \"\" \"" + FullPath + "\"\r\n";
            CommText += "exit";
            System.IO.File.WriteAllText(FullPath + ".cmd", CommText, Encoding.GetEncoding("GB2312"));//GB2312解决出现中文时乱码的问题
            Process pr = new Process();
            pr.StartInfo.FileName = FullPath + ".cmd";
            pr.Start();
            Application.Exit();
            return true;
        }

        private string GetSource(string url)
        {
            string strHTML = "";
            WebClient myWebClient = new WebClient();
            Stream myStream = myWebClient.OpenRead(url);
            StreamReader sr = new StreamReader(myStream, System.Text.Encoding.UTF8);
            strHTML = sr.ReadToEnd();
            myStream.Close();
            return strHTML;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DownloadUpdate();
        }
    }
}
