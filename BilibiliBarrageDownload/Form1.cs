using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BilibiliBarrageDownload
{
    public partial class Form1 : Form
    {
        //****版本号****
        public static double version = 1.0;
        //****版本号****
        private MessageBoxFrm msg;
        public static string MsgTitle;
        public static string MsgContent;
        public static bool MsgButton;
        public static bool MsgTop;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Alert("获取中...","提示",false,false);
            GetBarrage();
        }

        private bool GetBarrage()
        {
            string av_id = textBox1.Text;
            string file_id = GetHTMLSource("https://www.bilibili.com/video/" + av_id);
            file_id = ReplaceText(file_id, "cid=.*&aid");
            file_id = ReplaceText(file_id, "\\d.*\\d");
            if (file_id == "")
            {
                Alert("网络或AV号错误", "无法获取",true,false);
                return false;
            }
            System.IO.File.WriteAllText(Properties.Settings.Default.DownloadPath + "\\"+av_id+".xml",GetXMLSource("http://comment.bilibili.tv/"+file_id+".xml"));
            Alert("下载完成","提示",true,false);
            return true;
        }

        private string ReplaceText(string text,string code)
        {
            foreach(Match match in Regex.Matches(text, code))
            {
                return match.Value;
            }
            return "";
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

        //兼容更佳:
        private string GetHTMLSource(string url)
        {
            Uri uri = new Uri(url);
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(uri);
            myReq.UserAgent = "User-Agent:Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705";
            myReq.Accept = "*/*";
            myReq.KeepAlive = true;
            myReq.Headers.Add("Accept-Language", "zh-cn,en-us;q=0.5");
            HttpWebResponse result = (HttpWebResponse)myReq.GetResponse();
            Stream receviceStream = result.GetResponseStream();
            StreamReader readerOfStream = new StreamReader(receviceStream, System.Text.Encoding.GetEncoding("utf-8"));
            string strHTML = readerOfStream.ReadToEnd();
            readerOfStream.Close();
            receviceStream.Close();
            result.Close();
            return strHTML;
        }

        //解决压缩问题
        private string GetXMLSource(string url)
        {
            Uri uri = new Uri(url);
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(uri);
            myReq.UserAgent = "User-Agent:Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705";
            myReq.Accept = "*/*";
            myReq.KeepAlive = true;
            myReq.Headers.Add("Accept-Encoding", "deflate");
            myReq.AutomaticDecompression = DecompressionMethods.Deflate;
            HttpWebResponse result = (HttpWebResponse)myReq.GetResponse();
            Stream receviceStream = result.GetResponseStream();
            StreamReader readerOfStream = new StreamReader(receviceStream, System.Text.Encoding.GetEncoding("utf-8"));
            string strHTML = readerOfStream.ReadToEnd();
            readerOfStream.Close();
            receviceStream.Close();
            result.Close();
            return strHTML;
        }

        private bool Alert(string content,string title,bool button,bool top)
        {
            MsgTitle = title;
            MsgContent = content;
            MsgButton = button;
            MsgTop = top;
            if (msg == null)
            {
                msg = new MessageBoxFrm();
                msg.Show();
            }
            else
            {
                if (!msg.IsDisposed)
                {
                    msg.Close();
                }
                msg = new MessageBoxFrm();
                msg.Show();
            }
            return true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string x = Properties.Settings.Default.DownloadPath;
            if (x == "")
            {
                x = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            }
            if (!Directory.Exists(x))
            {
                Directory.CreateDirectory(x);
            }

            if (Properties.Settings.Default.FirstRun)
            {
                Setting setting = new Setting();
                setting.Show();
                setting.TopMost = true;
                //MessageBoxFrm msg = new MessageBoxFrm();
                //msg.Show();
                //msg.TopMost = true;
                //msg.Text = "首次运行 | 请设置您的弹幕储存路径";
                Alert("请设置您的弹幕储存路径","首次运行",true,true);
                Properties.Settings.Default.FirstRun = false;
            }

            if (File.Exists(this.GetType().Assembly.Location + ".cmd"))
            {
                System.IO.File.Delete(this.GetType().Assembly.Location + ".cmd");
                Alert("弹幕下载助手已经成功更新至"+version.ToString(),"升级成功",true,true);
            }
        }

        private bool DownFile(string url,string dir)
        {
            WebClient myWebClient = new WebClient();
            myWebClient.DownloadFile(url, dir);
            return true;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Setting setting = new Setting();
            setting.Show();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("Explorer.exe",Properties.Settings.Default.DownloadPath);
        }
    }
}
