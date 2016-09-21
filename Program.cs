using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Security.Principal;
using System.Deployment.Application;
using System.ComponentModel;
namespace LOLReplay
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;
                if (ad.CheckForUpdate())
                {
                    MessageBox.Show("程序有更新!");
                    ad.UpdateCompleted += new AsyncCompletedEventHandler(ad_UpdateCompleted);

                    ad.Update();
                    return;
                }
            }
            //var wi = WindowsIdentity.GetCurrent();
            //var wp = new WindowsPrincipal(wi);
            //bool runAsAdmin = wp.IsInRole(WindowsBuiltInRole.Administrator);
            //if (!runAsAdmin)
            //{
            //    MessageBox.Show("对不起程序需要一个管理员权限运行,而当前没有管理员权限");
            //    Application.Exit();
            //}


            //处理未捕获的异常
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            //处理UI线程异常
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Utilities.UnhandledThreadExceptonHandler);
            //处理非UI线程异常
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(Utilities.UnhandledExceptonHandler);

            System.AppDomain.CurrentDomain.UnhandledException += new System.UnhandledExceptionEventHandler(Utilities.UnhandledExceptonHandler);

            Process instance = RunningInstance();
            bool bExist = true;

            Mutex MyMutex = new Mutex(true, "LOLReplay", out bExist);

            if (!bExist || instance != null)
            {
                //Make   sure   the   window   is   not   minimized   or   maximized  
                //ShowWindowAsync(instance.MainWindowHandle, WS_SHOWMAXIMIZED);
                //Set   the   real   intance   to   foreground   window
                SetForegroundWindow(instance.MainWindowHandle);

                string[] cmds = Environment.GetCommandLineArgs();
                if (cmds.Length == 2)
                {
                    int WINDOW_HANDLER = FindWindow(null, @"LOL Replay - www.lolcn.cc");
                    if (WINDOW_HANDLER != 0)
                    {
                        COPYDATASTRUCT cds;
                        cds.dwData = (IntPtr)100;
                        cds.lpData = cmds[1];
                        cds.cbData = cmds[1].Length * 2;
                        SendMessage(WINDOW_HANDLER, 0x004A, 0, ref cds);
                    }
                }
            }
            else
            {

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                string arg = "";
                if (args.Length > 0)
                {
                    arg = args[0].Replace("\\/", "/");
                }
                MainForm mainForm = new MainForm() { cmd = arg };
                Application.Run(mainForm);
            }
        }
        public static void ad_UpdateCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show("更新被取消");
                return;
            }
            else if (e.Error != null)
            {
                MessageBox.Show("更新失败. 原因: \n" + e.Error.Message);
                return;
            }

            DialogResult dr = MessageBox.Show("更新完毕", "重启", MessageBoxButtons.OKCancel);
            if (DialogResult.OK == dr)
            {
                Application.Restart();
            }
        }
        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpData;
        }

        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(
        int hWnd, // handle to destination window
        int Msg, // message
        int wParam, // first message parameter
        ref COPYDATASTRUCT lParam // second message parameter
        );

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern int FindWindow(string lpClassName, string lpWindowName);

        private const int WS_HIDE = 0;
        private const int WS_SHOWNORMAL = 1;
        private const int WS_SHOWMINIMIZED = 2;
        private const int WS_SHOWMAXIMIZED = 3;
        private const int WS_MAXIMIZE = 3;
        private const int WS_SHOWNOACTIVATE = 4;
        private const int WS_SHOW = 5;
        private const int WS_MINIMIZE = 6;
        private const int WS_SHOWMINNOACTIVE = 7;
        private const int WS_SHOWNA = 8;
        private const int WS_RESTORE = 9;
        public static Process RunningInstance()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);
            //Loop   through   the   running   processes   in   with   the   same   name  
            foreach (Process process in processes)
            {
                //Ignore   the   current   process  
                if (process.Id != current.Id)
                {
                    //Make   sure   that   the   process   is   running   from   the   exe   file.  
                    if (Assembly.GetExecutingAssembly().Location.Replace("/", "\\") == current.MainModule.FileName)
                    {
                        //Return   the   other   process   instance.  
                        return process;
                    }
                }
            }
            //No   other   instance   was   found,   return   null.
            return null;
        }

        public static void HandleRunningInstance(Process instance)
        {
            //Make   sure   the   window   is   not   minimized   or   maximized  
            ShowWindowAsync(instance.MainWindowHandle, WS_SHOWMAXIMIZED);
            //Set   the   real   intance   to   foreground   window
            SetForegroundWindow(instance.MainWindowHandle);
        }
    }
    public class LinkInfo
    {
        public long GameID { get; set; }
        public string Link { get; set; }
    }
}
