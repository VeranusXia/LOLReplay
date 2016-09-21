namespace LOLReplay
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.se = new Sunisoft.IrisSkin.SkinEngine();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.功能ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.登陆ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.同步录像ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查询召唤师ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.开启后台录像ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Skin = new System.Windows.Forms.ToolStripMenuItem();
            this.游戏设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.客户端路径选择ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.国服ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.外服ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.录像文件路径ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.官网ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listView1 = new System.Windows.Forms.ListView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.查看对阵表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.播放ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.重命名ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lbStatus = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.timeRecord = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // se
            // 
            this.se.@__DrawButtonFocusRectangle = true;
            this.se.DisabledButtonTextColor = System.Drawing.Color.Gray;
            this.se.DisabledMenuFontColor = System.Drawing.SystemColors.GrayText;
            this.se.Enable3rdControl = true;
            this.se.InactiveCaptionColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.se.SerialNumber = "";
            this.se.SkinFile = null;
            this.se.SkinStreamMain = ((System.IO.Stream)(resources.GetObject("se.SkinStreamMain")));
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(0);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.功能ToolStripMenuItem,
            this.设置ToolStripMenuItem,
            this.帮助ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(3, 2);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(172, 25);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 功能ToolStripMenuItem
            // 
            this.功能ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.登陆ToolStripMenuItem,
            this.同步录像ToolStripMenuItem,
            this.查询召唤师ToolStripMenuItem,
            this.开启后台录像ToolStripMenuItem});
            this.功能ToolStripMenuItem.Name = "功能ToolStripMenuItem";
            this.功能ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.功能ToolStripMenuItem.Text = "功能";
            // 
            // 登陆ToolStripMenuItem
            // 
            this.登陆ToolStripMenuItem.Enabled = false;
            this.登陆ToolStripMenuItem.Name = "登陆ToolStripMenuItem";
            this.登陆ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.登陆ToolStripMenuItem.Text = "登陆";
            // 
            // 同步录像ToolStripMenuItem
            // 
            this.同步录像ToolStripMenuItem.Enabled = false;
            this.同步录像ToolStripMenuItem.Name = "同步录像ToolStripMenuItem";
            this.同步录像ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.同步录像ToolStripMenuItem.Text = "同步录像";
            // 
            // 查询召唤师ToolStripMenuItem
            // 
            this.查询召唤师ToolStripMenuItem.Enabled = false;
            this.查询召唤师ToolStripMenuItem.Name = "查询召唤师ToolStripMenuItem";
            this.查询召唤师ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.查询召唤师ToolStripMenuItem.Text = "查询召唤师";
            this.查询召唤师ToolStripMenuItem.Click += new System.EventHandler(this.查询召唤师ToolStripMenuItem_Click);
            // 
            // 开启后台录像ToolStripMenuItem
            // 
            this.开启后台录像ToolStripMenuItem.Name = "开启后台录像ToolStripMenuItem";
            this.开启后台录像ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.开启后台录像ToolStripMenuItem.Text = "开启后台录像";
            this.开启后台录像ToolStripMenuItem.Click += new System.EventHandler(this.开启后台录像ToolStripMenuItem_Click);
            // 
            // 设置ToolStripMenuItem
            // 
            this.设置ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_Skin,
            this.游戏设置ToolStripMenuItem});
            this.设置ToolStripMenuItem.Name = "设置ToolStripMenuItem";
            this.设置ToolStripMenuItem.Size = new System.Drawing.Size(59, 21);
            this.设置ToolStripMenuItem.Text = "设置(&S)";
            // 
            // Menu_Skin
            // 
            this.Menu_Skin.Name = "Menu_Skin";
            this.Menu_Skin.Size = new System.Drawing.Size(124, 22);
            this.Menu_Skin.Text = "软件换肤";
            // 
            // 游戏设置ToolStripMenuItem
            // 
            this.游戏设置ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.客户端路径选择ToolStripMenuItem,
            this.录像文件路径ToolStripMenuItem});
            this.游戏设置ToolStripMenuItem.Name = "游戏设置ToolStripMenuItem";
            this.游戏设置ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.游戏设置ToolStripMenuItem.Text = "游戏设置";
            // 
            // 客户端路径选择ToolStripMenuItem
            // 
            this.客户端路径选择ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.国服ToolStripMenuItem,
            this.外服ToolStripMenuItem});
            this.客户端路径选择ToolStripMenuItem.Name = "客户端路径选择ToolStripMenuItem";
            this.客户端路径选择ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.客户端路径选择ToolStripMenuItem.Text = "客户端路径选择";
            // 
            // 国服ToolStripMenuItem
            // 
            this.国服ToolStripMenuItem.Name = "国服ToolStripMenuItem";
            this.国服ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.国服ToolStripMenuItem.Text = "国服";
            this.国服ToolStripMenuItem.Click += new System.EventHandler(this.国服ToolStripMenuItem_Click);
            // 
            // 外服ToolStripMenuItem
            // 
            this.外服ToolStripMenuItem.Name = "外服ToolStripMenuItem";
            this.外服ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.外服ToolStripMenuItem.Text = "外服";
            this.外服ToolStripMenuItem.Click += new System.EventHandler(this.外服ToolStripMenuItem_Click);
            // 
            // 录像文件路径ToolStripMenuItem
            // 
            this.录像文件路径ToolStripMenuItem.Name = "录像文件路径ToolStripMenuItem";
            this.录像文件路径ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.录像文件路径ToolStripMenuItem.Text = "录像文件路径";
            this.录像文件路径ToolStripMenuItem.Click += new System.EventHandler(this.录像文件路径ToolStripMenuItem_Click);
            // 
            // 帮助ToolStripMenuItem
            // 
            this.帮助ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.关于ToolStripMenuItem,
            this.官网ToolStripMenuItem});
            this.帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem";
            this.帮助ToolStripMenuItem.Size = new System.Drawing.Size(61, 21);
            this.帮助ToolStripMenuItem.Text = "帮助(&H)";
            // 
            // 关于ToolStripMenuItem
            // 
            this.关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            this.关于ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.关于ToolStripMenuItem.Text = "关于";
            // 
            // 官网ToolStripMenuItem
            // 
            this.官网ToolStripMenuItem.Name = "官网ToolStripMenuItem";
            this.官网ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.官网ToolStripMenuItem.Text = "官网";
            this.官网ToolStripMenuItem.Click += new System.EventHandler(this.官网ToolStripMenuItem_Click);
            // 
            // listView1
            // 
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(3, 29);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(778, 488);
            this.listView1.TabIndex = 3;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            this.listView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.查看对阵表ToolStripMenuItem,
            this.播放ToolStripMenuItem,
            this.重命名ToolStripMenuItem,
            this.删除ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(137, 92);
            // 
            // 查看对阵表ToolStripMenuItem
            // 
            this.查看对阵表ToolStripMenuItem.Name = "查看对阵表ToolStripMenuItem";
            this.查看对阵表ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.查看对阵表ToolStripMenuItem.Text = "查看对阵表";
            this.查看对阵表ToolStripMenuItem.Click += new System.EventHandler(this.查看对阵表ToolStripMenuItem_Click);
            // 
            // 播放ToolStripMenuItem
            // 
            this.播放ToolStripMenuItem.Name = "播放ToolStripMenuItem";
            this.播放ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.播放ToolStripMenuItem.Text = "播放";
            this.播放ToolStripMenuItem.Click += new System.EventHandler(this.播放ToolStripMenuItem_Click);
            // 
            // 重命名ToolStripMenuItem
            // 
            this.重命名ToolStripMenuItem.Name = "重命名ToolStripMenuItem";
            this.重命名ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.重命名ToolStripMenuItem.Text = "重命名";
            this.重命名ToolStripMenuItem.Click += new System.EventHandler(this.重命名ToolStripMenuItem_Click);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.删除ToolStripMenuItem.Text = "删除";
            this.删除ToolStripMenuItem.Click += new System.EventHandler(this.删除ToolStripMenuItem_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(3, 538);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(778, 23);
            this.progressBar1.TabIndex = 4;
            // 
            // lbStatus
            // 
            this.lbStatus.AutoSize = true;
            this.lbStatus.Location = new System.Drawing.Point(324, 522);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(0, 12);
            this.lbStatus.TabIndex = 5;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "League of Legends.exe|League of Legends.exe";
            // 
            // timeRecord
            // 
            this.timeRecord.Enabled = true;
            this.timeRecord.Interval = 10000;
            this.timeRecord.Tick += new System.EventHandler(this.timeRecord_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.lbStatus);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LOL Replay - www.lolcn.cc";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public Sunisoft.IrisSkin.SkinEngine se;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Menu_Skin;
        private System.Windows.Forms.ToolStripMenuItem 游戏设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 客户端路径选择ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 国服ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 外服ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 录像文件路径ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 帮助ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于ToolStripMenuItem;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem 官网ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 播放ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 重命名ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 查看对阵表ToolStripMenuItem;
        private System.Windows.Forms.Timer timeRecord;
        private System.Windows.Forms.ToolStripMenuItem 功能ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 登陆ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 同步录像ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 查询召唤师ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 开启后台录像ToolStripMenuItem;
    }
}

