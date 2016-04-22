
using Ionic.Zip;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using ReplayDll;
using System.Windows.Forms;
namespace LOLReplay
{
    public class LoLLauncher
    {
        private LoLRecord _record;
        private string _exePath;
        private string _args;
        private bool _localPlay;
        private LoLRecordPlayer _recordPlayer;
        private System.Threading.Thread _playerThread;
        private LoLRecorder _recoder;

        private DispatcherTimer LoLWatchTimer;
        private GameInfo _gameInfo;
        public bool isPlay;
        private void InitLoLWatchTimer()
        {
            this.LoLWatchTimer = new DispatcherTimer();
            this.LoLWatchTimer.Interval = System.TimeSpan.FromMilliseconds(500.0);
            this.LoLWatchTimer.Tick += new System.EventHandler(this.LoLWatchTimer_Tick);
            this.LoLWatchTimer.Start();
        }
        private static bool IsLoLExists()
        {
            return Utilities.GetProcess("League of Legends") != null;
        }
        private void LoLWatchTimer_Tick(object sender, System.EventArgs args)
        {
            if (!IsLoLExists())
            {
                this.LoLWatchTimer.Stop();
                if (this._recordPlayer != null)
                {
                    this._recordPlayer.StopPlaying();
                }
                isPlay = false;
         
            }
        }
 
   
        public LoLLauncher(string recordFilePath)
        {
            this._record = new LoLRecord();
            this._record.readFromFile(recordFilePath, false);
            this._localPlay = true;
        }
  
        private void CreateLoLRecordPlayer()
        {
            this._recordPlayer = new LoLRecordPlayer(this._record);
            System.Threading.ThreadStart start = new System.Threading.ThreadStart(this._recordPlayer.startPlaying);
            this._playerThread = new System.Threading.Thread(start);
            this._playerThread.Start();
        }
        private void MakeGameInfo()
        {
            if (this._gameInfo == null)
            {
                if (this._localPlay)
                {
                    this._gameInfo = new GameInfo("127.0.0.1:8089", new string(this._record.gamePlatform), this._record.gameId, new string(this._record.observerEncryptionKey));
                    return;
                }
                this._gameInfo = new GameInfo(this._recoder.platformAddress, new string(this._record.gamePlatform), this._record.gameId, new string(this._record.observerEncryptionKey));
            }
        }
        public void StartPlaying()
        {
            string platform = new string(this._record.gamePlatform);

            this.MakeGameInfo();
            if (platform.StartsWith("HN") || platform.StartsWith("WT"))
            {
                this._exePath =  Properties.Settings.Default.CNPath ;
            }
            else
            {
                this._exePath =  Properties.Settings.Default.OtherPath ;
            }
            this.MakeGameInfo();
            if (this._localPlay && this._record.IsBroken)
            {
                return;
            }
           
            this.SetArgs();
            if (this._localPlay)
            {
                this.CreateLoLRecordPlayer();
            }
            this.ExecuteLoLExe();
            this.InitLoLWatchTimer();

            isPlay = true;
        }
        private void SetArgs()
        {
            this._args = string.Concat(new object[]
			{
				"\"8394\" \"lol.launcher_tencent.exe\" \"Air/LolClient.exe\" \"spectator ",
				this._gameInfo.ServerAddress,
				" ",
				this._gameInfo.ObKey,
				" ",
				this._gameInfo.GameId,
				" ",
				this._gameInfo.PlatformId,
				"\""
			});
        }
        private void ExecuteLoLExe()
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    UseShellExecute = false,
                    Arguments = this._args,
                    FileName = this._exePath,
                    Verb = "RunAs",
                    WorkingDirectory = _exePath.Remove(_exePath.LastIndexOf('\\'))
                });
            }
            catch { MessageBox.Show("软件需要管理员权限，请系统关闭UAC/如果您使用的是外服客户端,更新过后请手动更新客户端路径"); }
        }
      
    }
}
