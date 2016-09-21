using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;
using Sunisoft.IrisSkin;
using System.Net;
using Newtonsoft.Json;
using System.Web;
using System.Threading;
using System.Diagnostics;

namespace LOLReplay
{
    public partial class MainForm : Form
    {
        public string cmd { get; set; }
        LoLLauncher launcher { get; set; }
        protected override void DefWndProc(ref System.Windows.Forms.Message m)
        {
            switch (m.Msg)
            {
                case 0x004A:
                    LOLReplay.Program.COPYDATASTRUCT mystr = new LOLReplay.Program.COPYDATASTRUCT();
                    Type mytype = mystr.GetType();
                    mystr = (LOLReplay.Program.COPYDATASTRUCT)m.GetLParam(mytype);
                    cmd = mystr.lpData;
                    ProcessFile();
                    break;
                default:
                    base.DefWndProc(ref m);
                    break;
            }
        }
        public MainForm()
        {
            InitializeComponent();
            //se.SkinDialogs = false;
            se.SkinFile = Properties.Settings.Default.SEFile; //Properties.Settings.Default.SEFile;

        }
        protected void Record()
        {
            while (true)
            {
                if (launcher != null)
                {
                    if (launcher.isPlay == false)
                    {
                        LoLRecorder r = new LoLRecorder();
                    }
                }

            }
        }

        public void WriteRegistryKey()
        {
            try
            {
                //写注册表
                RegistryKey key = Registry.ClassesRoot.CreateSubKey("LoLReplay");
                key.SetValue("", "URL:LoLReplay Protocol");
                key.SetValue("URL Protocol", "");
                RegistryKey Subkey = key.CreateSubKey("DefaultICon");
                Subkey.SetValue("", Application.ExecutablePath);
                Subkey = key.CreateSubKey("shell").CreateSubKey("open").CreateSubKey("Command");
                Subkey.SetValue("", "\"" + Application.ExecutablePath + "\" \"%1\"");

                SaveReg(Application.ExecutablePath, ".lpr");
            }
            catch
            {
                MessageBox.Show("当前用户没有管理员权限初始化程序");
                Application.Exit();
            }
        }
        /// <summary>
        /// 设置文件关联
        /// </summary>
        /// <param name="p_Filename">程序的名称</param>
        /// <param name="p_FileTypeName">扩展名 .VRD </param>
        public static void SaveReg(string p_Filename, string p_FileTypeName)
        {
            RegistryKey _RegKey = Registry.ClassesRoot.OpenSubKey("", true);

            RegistryKey _VRPkey = _RegKey.OpenSubKey(p_FileTypeName);
            //if (_VRPkey != null)
            //_RegKey.DeleteSubKey(p_FileTypeName, true);

            _RegKey.Flush();
            _RegKey.CreateSubKey(p_FileTypeName);
            _VRPkey = _RegKey.OpenSubKey(p_FileTypeName, true);
            _VRPkey.CreateSubKey("Shell");
            _VRPkey = _VRPkey.OpenSubKey("Shell", true);
            _VRPkey.CreateSubKey("Open");
            _VRPkey = _VRPkey.OpenSubKey("Open", true);
            _VRPkey.CreateSubKey("Command");
            _VRPkey = _VRPkey.OpenSubKey("Command", true);
            string _PathString = "\"" + p_Filename + "\" \"%1\"";
            _VRPkey.SetValue("", _PathString);
            _VRPkey.Flush();
            _RegKey.Flush();

        }

        /// <summary>
        /// 删除文件关联
        /// </summary>
        /// <param name="p_FileTypeName">扩展名 .VRD </param>
        public static void DelReg(string p_FileTypeName)
        {
            RegistryKey _Regkey = Registry.ClassesRoot.OpenSubKey("", true);
            _Regkey.DeleteSubKeyTree(p_FileTypeName);

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                //WriteRegistryKey();
            }
            catch { }
            //WriteRegistryKey();
            SetMenu_skin();

            if (string.IsNullOrEmpty(Properties.Settings.Default.ReplayPath))
            {
                Properties.Settings.Default.ReplayPath = Application.StartupPath + "\\Replay";
                Properties.Settings.Default.Save();
            }

            string ReplayPath = Properties.Settings.Default.ReplayPath;
            
            if (!Directory.Exists(ReplayPath))
            {
                Directory.CreateDirectory(ReplayPath);
                //while (!Directory.Exists(Properties.Settings.Default.ReplayPath))
                //{
                //    MessageBox.Show("请指定存放录像文件夹路径");
                //    SetReplayFolder();
                //}
            }
            else
                InitReplayFolder();


            ProcessFile();

        }
        private void ProcessFile()
        {
            if (!string.IsNullOrEmpty(cmd))
            {
                if (cmd.StartsWith("lolreplay"))
                {
                    cmd = cmd.Substring(12);
                    while (cmd.EndsWith("/"))
                    {
                        cmd = cmd.Substring(0, cmd.Length - 1);
                    }
                    cmd = HttpUtility.UrlDecode(cmd);
                    if (progressBar1.Value > 0 && progressBar1.Value < 100)
                    {
                    }
                    else
                        DownLoadReplay();
                }
                else
                    StartWatch(cmd);
            }
        }
        private void DownLoadReplay()
        {
            string ReplayPath = Properties.Settings.Default.ReplayPath;

            if (!Directory.Exists(ReplayPath))
            {
                MessageBox.Show("请指定存放录像文件夹路径,否则无法下载");
                while (!Directory.Exists(Properties.Settings.Default.ReplayPath))
                {
                    SetReplayFolder();
                }
            }
            progressBar1.Value = 0;
            LinkInfo li = JsonConvert.DeserializeObject<LinkInfo>(cmd);
            WebClient wc = new WebClient();
            wc.DownloadFileCompleted += new AsyncCompletedEventHandler(wcDownloadCompleted);
            wc.DownloadProgressChanged += wc_DownloadProgressChanged;

            string FileName = MsgBox.ShowInputBox("录像文件重命名", "请输入文件名:", li.GameID.ToString());

            wc.DownloadFileAsync(new Uri(li.Link), Properties.Settings.Default.ReplayPath + "\\" + (string.IsNullOrEmpty(FileName) ? li.GameID.ToString() : FileName) + ".lpr");
        }

        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            lbStatus.Text = "已下载" + e.ProgressPercentage + "%(" + e.BytesReceived / 1024 + "K/" + e.TotalBytesToReceive / 1024 + "K)";
        }
        protected void wcDownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            lbStatus.Text = "下载完成";
            StartWatch(InitReplayFolder());
        }
        protected void StartWatch(string filename)
        {

            if (MessageBox.Show("是否要开始播放" + filename, "播放", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                string CNPath = Properties.Settings.Default.CNPath;
                if (!File.Exists(CNPath))
                {
                    MessageBox.Show("请指定国服客户端路径,在LOL的Game文件夹中");
                    while (!File.Exists(Properties.Settings.Default.CNPath))
                    {
                        SetGF();
                    }
                }

                string OtherPath = Properties.Settings.Default.OtherPath;
                if (!File.Exists(OtherPath))
                {

                    MessageBox.Show(" 请指定外服客户端路径,在RADS\\solutions\\lol_game_client_sln\\releases\\0.0.0.xxx\\deploy中,如果没有请指定国服客户端");
                    while (!File.Exists(Properties.Settings.Default.OtherPath))
                    {
                        SetWF();
                    }
                }
                launcher = new LoLLauncher(filename);
                launcher.StartPlaying();
                MsgBox.ShowInputBox("正在播放:", filename, "", false);
            }
        }
        private void createHeader()//为listview添加列名
        {
            listView1.Columns.Clear();
            ColumnHeader ch = new ColumnHeader();
            ch.Text = "文件名";
            ch.Width = listView1.Width / 3; ;
            this.listView1.Columns.Add(ch);
            ch = new ColumnHeader();
            ch.Width = listView1.Width / 3;
            ch.Text = "大小";
            this.listView1.Columns.Add(ch);
            ch = new ColumnHeader();
            ch.Width = listView1.Width / 3;
            ch.Text = "修改日期";
            this.listView1.Columns.Add(ch);
        }
        private string InitReplayFolder()
        {
            createHeader();
            listView1.Items.Clear();
            listView1.BeginUpdate();
            ListViewItem lvi;
            ListViewItem.ListViewSubItem lvsi;
            List<FileInfo> files = new DirectoryInfo(Properties.Settings.Default.ReplayPath).GetFiles().Where(u => u.Name.EndsWith(".lpr")).ToList();
            foreach (FileInfo file in files)
            {
                lvi = new ListViewItem();
                lvi.Text = file.Name;
                lvi.ImageIndex = 1;
                lvi.Tag = file.FullName;
                lvsi = new System.Windows.Forms.ListViewItem.ListViewSubItem();
                lvsi.Text = (file.Length / 1024).ToString() + "KB";
                lvi.SubItems.Add(lvsi);
                lvsi = new System.Windows.Forms.ListViewItem.ListViewSubItem();
                lvsi.Text = file.LastAccessTime.ToString();
                lvi.SubItems.Add(lvsi);

                this.listView1.Items.Add(lvi);

            }
            listView1.EndUpdate();
            if (files.Count > 0)
                return files.OrderByDescending(u => u.LastWriteTime).FirstOrDefault().FullName;
            else
                return null;
        }
        protected void SetMenu_skin()
        {
            Menu_Skin.DropDownItems.Clear();

            foreach (DirectoryInfo dir in new DirectoryInfo(Application.StartupPath + "\\Skins").GetDirectories())
            {
                ToolStripMenuItem sub = new ToolStripMenuItem();
                sub.Name = sub.Text = dir.Name;
                Menu_Skin.DropDownItems.Add(sub);

                foreach (FileInfo file in dir.GetFiles().Where(u => u.Name.EndsWith(".ssk") || u.Name.EndsWith(".ssk.deploy")))
                {
                    ToolStripMenuItem sub2 = new ToolStripMenuItem();
                    sub2.Name = file.FullName;
                    sub2.Text = file.Name;
                    sub2.Click += Menu_Click;
                    sub.DropDownItems.Add(sub2);
                }
            }
        }
        private void Menu_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menu = sender as ToolStripMenuItem;
            se.SkinFile = menu.Name;

            Properties.Settings.Default.SEFile = menu.Name;
            Properties.Settings.Default.Save();
        }

        private void 国服ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //openFileDialog1.InitialDirectory = Environment.SpecialFolder.MyComputer.ToString();
            SetGF();
        }

        protected void SetGF()
        {
            openFileDialog1.Title = "请指定国服客户端路径,在LOL的Game文件中";
            openFileDialog1.FileName = "";
            if (!string.IsNullOrEmpty(Properties.Settings.Default.CNPath))
            {
                openFileDialog1.FileName = Properties.Settings.Default.CNPath;
            }
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Properties.Settings.Default.CNPath = openFileDialog1.FileName;
                //MessageBox.Show("设置成功");
            }
        }

        private void 外服ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //openFileDialog1.InitialDirectory =  Environment.SpecialFolder.MyComputer.ToString();
            SetWF();
        }

        protected void SetWF()
        {
            openFileDialog1.Title = "请指定外服客户端路径,在RADS\\solutions\\lol_game_client_sln\\releases\\0.0.0.xxx\\deploy中,如果没有请指定国服客户端";

            openFileDialog1.FileName = "";
            if (!string.IsNullOrEmpty(Properties.Settings.Default.OtherPath))
            {
                openFileDialog1.FileName = Properties.Settings.Default.OtherPath;
            }
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Properties.Settings.Default.OtherPath = openFileDialog1.FileName;
                Properties.Settings.Default.Save();
                //MessageBox.Show("设置成功");
            }
        }
        private void 录像文件路径ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetReplayFolder();
        }
        protected void SetReplayFolder()
        {
            using (FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog())
            {
                folderBrowserDialog1.Description = "请指定录像文件夹路径";
                if (!string.IsNullOrEmpty(Properties.Settings.Default.ReplayPath))
                {
                    folderBrowserDialog1.SelectedPath = Properties.Settings.Default.ReplayPath;
                }
                se.SkinDialogs = false;
                if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Properties.Settings.Default.ReplayPath = folderBrowserDialog1.SelectedPath;
                    Properties.Settings.Default.Save();
                    //MessageBox.Show("设置成功");
                    InitReplayFolder();
                }
                se.SkinDialogs = true;
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            //StartWatch(Setting.Load("ReplayPath") + "\\" + listView1.SelectedItems[0].Text);
        }

        private void 官网ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process p = Process.Start("http://www.lolcn.cc:8080/");
        }

        private void 播放ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
                StartWatch(Properties.Settings.Default.ReplayPath + "\\" + listView1.SelectedItems[0].Text);
        }

        private void 重命名ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (listView1.SelectedItems.Count == 1)
            {
                string FileName = MsgBox.ShowInputBox("录像文件重命名", "请输入文件名:", listView1.SelectedItems[0].Text.Replace(".lpr", ""));
                string oldName = Properties.Settings.Default.ReplayPath + "\\" + listView1.SelectedItems[0].Text;
                if (!string.IsNullOrEmpty(FileName))
                {
                    File.Move(oldName, Properties.Settings.Default.ReplayPath + "\\" + FileName + ".lpr");
                }
                InitReplayFolder();
            }
        }
        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (listView1.SelectedItems.Count == 1)
            {
                string filename = Properties.Settings.Default.ReplayPath + "\\" + listView1.SelectedItems[0].Text;
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }

                InitReplayFolder();
            }
        }
        private void listView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (this.listView1.SelectedItems.Count > 0)
                {

                    this.contextMenuStrip1.Show(this.listView1, e.X, e.Y);
                }
            }
        }

        private void 查看对阵表ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private object _lolTimerLocck = new object();
        private bool LastTimeIsExecuting;

        private void timeRecord_Tick(object sender, EventArgs e)
        {
            lock (this._lolTimerLocck)
            {
                if (Utilities.IsLoLExists())
                {
                    if (!this.LastTimeIsExecuting)
                    {
                        this.LastTimeIsExecuting = true;
                    }
                }
                else
                {
                    if (this.LastTimeIsExecuting)
                    {
                        this.LastTimeIsExecuting = false;
                    }
                }
            }
        }



        private void 查询召唤师ToolStripMenuItem_Click(object sender, EventArgs e)
        {


        }

        private void 开启后台录像ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(System.Environment.CurrentDirectory + "\\Recoder.exe");
            }
            catch { MessageBox.Show("请将Recoder.exe手动放到程序根目录打开"); }
        }



    }
}
