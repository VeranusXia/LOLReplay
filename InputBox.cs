using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LOLReplay
{
    public class MsgBox : Form
    {
        #region InputBox
        private System.Windows.Forms.TextBox txtData;
        private System.Windows.Forms.Label lblInfo;
        private System.ComponentModel.Container components = null;
        private Button BT_OK;
        private Button BT_Canncel;
        private MsgBox()
        {
            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.txtData = new System.Windows.Forms.TextBox();
            this.lblInfo = new System.Windows.Forms.Label();
            this.BT_OK = new System.Windows.Forms.Button();
            this.BT_Canncel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtData
            // 
            this.txtData.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtData.Location = new System.Drawing.Point(142, 16);
            this.txtData.Name = "txtData";
            this.txtData.Size = new System.Drawing.Size(184, 23);
            this.txtData.TabIndex = 0;
            this.txtData.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtData_KeyDown);
            // 
            // lblInfo
            // 
            //this.lblInfo.BackColor = System.Drawing.SystemColors.Info;
            //this.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblInfo.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblInfo.ForeColor = System.Drawing.Color.Gray;
            this.lblInfo.Location = new System.Drawing.Point(21, 18);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(98, 23);
            this.lblInfo.TabIndex = 1;
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // BT_OK
            // 
            this.BT_OK.Location = new System.Drawing.Point(142, 59);
            this.BT_OK.Name = "BT_OK";
            this.BT_OK.Size = new System.Drawing.Size(80, 23);
            this.BT_OK.TabIndex = 2;
            this.BT_OK.Text = "[Enter]确定";
            this.BT_OK.UseVisualStyleBackColor = true;
            this.BT_OK.Click += new System.EventHandler(this.BT_OK_Click);
            // 
            // BT_Canncel
            // 
            this.BT_Canncel.Location = new System.Drawing.Point(251, 59);
            this.BT_Canncel.Name = "BT_Canncel";
            this.BT_Canncel.Size = new System.Drawing.Size(75, 23);
            this.BT_Canncel.TabIndex = 3;
            this.BT_Canncel.Text = " [Esc]取消";
            this.BT_Canncel.UseVisualStyleBackColor = true;
            this.BT_Canncel.Click += new System.EventHandler(this.BT_Canncel_Click);
            // 
            // MsgBox
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(343, 99);
            this.ControlBox = false;
            this.Controls.Add(this.BT_Canncel);
            this.Controls.Add(this.BT_OK);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.txtData);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MsgBox";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "InputBox";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        //对键盘进行响应 

        private void txtData_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.Close();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                txtData.Text = string.Empty;
                this.Close();
            }
        }

        //显示InputBox 
        static MsgBox inputbox;
        public static string ShowInputBox(string Title, string keyInfo, string value, bool isShow = true)
        {

            inputbox = new MsgBox();
            inputbox.txtData.Visible = isShow;
            inputbox.BT_Canncel.Visible = isShow;
            if (!isShow)
                inputbox.lblInfo.Width = 250;
            inputbox.Text = Title;
            inputbox.txtData.Text = value;
            if (keyInfo.Trim() != string.Empty)

                inputbox.lblInfo.Text = keyInfo;

            inputbox.ShowDialog();

            return inputbox.txtData.Text;

        }

        private void BT_Canncel_Click(object sender, EventArgs e)
        {
            inputbox.txtData.Text = "";
            inputbox.Close();
        }

        private void BT_OK_Click(object sender, EventArgs e)
        {
            inputbox.Close();
        }
        #endregion
    }
}
