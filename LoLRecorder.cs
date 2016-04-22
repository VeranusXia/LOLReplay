
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using ReplayDll;
namespace LOLReplay
{
    public class LoLRecorder
    {
        public string filePath;
        public int chunkCount;
        public int startupChunkEnd;
        public int gameChunkStart;
        public int chunkGot;
        public int keyframeGot;
        public int keyframeCount;
        private System.Collections.Generic.Dictionary<int, byte[]> keyFrames;
        private System.Collections.Generic.Dictionary<int, byte[]> chunks;
        public LoLRecord record;
        public bool selfGame;
        public PlayerInfo selfPlayerInfo;
        public string platformAddress;
        public string specAddressPrefix;
        public static System.Collections.Generic.List<LoLRecorder> Recorders;
        private static uint _lastTimeAddress = 0u;
        //public event Program.RecordDoneDelegate doneEvent;

        public LoLRecorder()
        {
            this.record = new LoLRecord();
        }

        public LoLRecorder(GameInfo gameinfo)
        {
            if (LoLRecorder.Recorders == null)
            {
                LoLRecorder.Recorders = new System.Collections.Generic.List<LoLRecorder>();
            }
            this.record = new LoLRecord();
            this.record.gameId = gameinfo.GameId;
            this.record.observerEncryptionKey = gameinfo.ObKey.ToCharArray();
            this.record.gamePlatform = gameinfo.PlatformId.ToCharArray();
            this.setPlatformAddress(gameinfo.ServerAddress);
            this.selfGame = false;
            //this.setPlatformAddress(Utilities.LoLObserveServersIpMapping[new string(this.record.gamePlatform)]);


        }
        public void setPlatformAddress(string url)
        {
            this.platformAddress = url;
            this.specAddressPrefix = "http://" + this.platformAddress + "/observer-mode/rest/consumer/";
        }

        private bool analyzeObKeyInMemoryBytesMethod1(byte[] memBytes)
        {
            try
            {
                int i = 0;
                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                bool flag = false;
                while (i < memBytes.Length)
                {
                    if (memBytes[i] > 32 && memBytes[i] < 125)
                    {
                        stringBuilder.Append((char)memBytes[i]);
                        flag = false;
                    }
                    else
                    {
                        if (!flag)
                        {
                            stringBuilder.Append(' ');
                            flag = true;
                        }
                    }
                    i++;
                }
                string[] array = stringBuilder.ToString().Split(new char[]
				{
					' '
				});
                string[] array2 = array;
                for (int j = 0; j < array2.Length; j++)
                {
                    string text = array2[j];
                    if (text.Length == 33 && text[0] == 'A')
                    {
                        this.record.observerEncryptionKey = text.Substring(1, 32).ToCharArray();
                        bool result = true;
                        return result;
                    }
                }
            }
            catch
            {
                bool result = false;
                return result;
            }
            return false;
        }
        //public void StopRecording(bool isSuccess, string reason)
        //{
        //    lock (obj)
        //    {
        //        LoLRecorder.Recorders.Remove(this);
        //    }
        //    this.doneEvent(this, isSuccess, reason);
        //}

        private static object obj = new object();
        private bool CheckDuplicateRecorder()
        {
            //foreach (LoLRecorder current in LoLRecorder.Recorders)
            //{
            //    if (this.record.gameId == current.record.gameId && this.platformAddress == current.platformAddress)
            //    {
            //        return false;
            //    }
            //}
            lock (obj)
            {
                if (LoLRecorder.Recorders.FirstOrDefault(k => k.platformAddress == this.platformAddress && k.record.gameId == this.record.gameId) != null)
                    return false;

                LoLRecorder.Recorders.Add(this);
                return true;
            }
        } 


        private byte[] getEndOfGameStats()
        {
            string urlString = this.getUrlString(string.Concat(new object[]
			{
				this.specAddressPrefix,
				"endOfGameStats/",
				new string(this.record.gamePlatform),
				"/",
				this.record.gameId,
				"/null"
			}));
            byte[] result;
            try
            {
                result = System.Convert.FromBase64String(urlString.Replace(" ", "+"));
            }
            catch
            {
                return null;
            }
            return result;
        }
        private JObject getUrlJson(string url)
        {
            JObject result = null;
            try
            {
                result = JsonConvert.DeserializeObject<JObject>(this.getUrlString(url));
            }
            catch (System.Exception)
            {
                return null;
            }
            return result;
        }
        private string getUrlString(string url)
        {
            string text = null;
            using (WebClient webClient = new WebClient())
            {
                int num = 0;
                do
                {
                    try
                    {
                        text = webClient.DownloadString(url);
                    }
                    catch (WebException)
                    {
                        if (num++ >= 3)
                        {
                            return null;
                        }
                        System.Threading.Thread.Sleep(10000);
                    }
                }
                while (text == null);
            }
            return text;
        }
        private bool getInitBlocksNumber()
        {
            JObject urlJson = this.getUrlJson(string.Concat(new object[]
			{
				this.specAddressPrefix,
				"getGameMetaData/",
				new string(this.record.gamePlatform),
				"/",
				this.record.gameId,
				"/1/token"
			}));
            if (urlJson == null)
            {
                return false;
            }
            this.chunkCount = int.Parse(urlJson["lastChunkId"].ToString());
            this.startupChunkEnd = int.Parse(urlJson["endStartupChunkId"].ToString());
            this.gameChunkStart = int.Parse(urlJson["startGameChunkId"].ToString());
            return true;
        }
        private bool waitForSpectatorStart()
        {
            if (!this.selfGame)
            {
                return true;
            }
            int num = 0;
            System.DateTime now = System.DateTime.Now;
            do
            {
                System.Threading.Thread.Sleep(10000);
                JObject urlJson = this.getUrlJson(string.Concat(new object[]
				{
					this.specAddressPrefix,
					"getLastChunkInfo/",
					new string(this.record.gamePlatform),
					"/",
					this.record.gameId,
					"/30000/token"
				}));
                try
                {
                    num = int.Parse(urlJson["nextAvailableChunk"].ToString());
                }
                catch
                {
                    if ((System.DateTime.Now - now).TotalMinutes > 3.0)
                    {
                        return false;
                    }
                }
            }
            while (num < 0);
            System.Threading.Thread.Sleep(num);
            return true;
        }
        private bool getGameContentFromServer()
        {
            this.chunks = new System.Collections.Generic.Dictionary<int, byte[]>();
            this.keyFrames = new System.Collections.Generic.Dictionary<int, byte[]>();
            this.chunkGot = 1;
            this.keyframeGot = 1;
            while (true)
            {

                JObject urlJson = this.getUrlJson(string.Concat(new object[]
				{
					this.specAddressPrefix,
					"getLastChunkInfo/",
					new string(this.record.gamePlatform),
					"/",
					this.record.gameId,
					"/30000/token"
				}));
                if (urlJson == null)
                {
                    break;
                }
                this.chunkCount = int.Parse(urlJson["chunkId"].ToString());
                this.keyframeCount = int.Parse(urlJson["keyFrameId"].ToString());
                int num = int.Parse(urlJson["nextAvailableChunk"].ToString());
                System.Threading.Thread.Sleep(num);
                if (!this.getGameChunksFromServer())
                {
                    return false;
                }
                if (!this.getKeyFramesFromServer())
                {
                    return false;
                }
                if (num == 0)
                {
                    return true;
                }
            }
            return false;
        }
        public JObject getGameMeta()
        {
            return this.getUrlJson(string.Concat(new object[]
			{
				this.specAddressPrefix,
				"getGameMetaData/",
				new string(this.record.gamePlatform),
				"/",
				this.record.gameId,
				"/1/token"
			}));
        }
        public JObject getLastChunkInfo()
        {
            return this.getUrlJson(string.Concat(new object[]
			{
				this.specAddressPrefix,
				"getLastChunkInfo/",
				new string(this.record.gamePlatform),
				"/",
				this.record.gameId,
				"/30000/token"
			}));
        }
        private bool getNthBlockAndWrite(int n, bool IsKeyFrame)
        {
            WebClient webClient = new WebClient();
            int num = 0;
            byte[] value;
        IL_08:
            try
            {
                if (IsKeyFrame)
                {
                    value = webClient.DownloadData(string.Concat(new object[]
					{
						this.specAddressPrefix,
						"getKeyFrame/",
						new string(this.record.gamePlatform),
						"/",
						this.record.gameId,
						"/",
						n,
						"/token"
					}));
                }
                else
                {
                    value = webClient.DownloadData(string.Concat(new object[]
					{
						this.specAddressPrefix,
						"getGameDataChunk/",
						new string(this.record.gamePlatform),
						"/",
						this.record.gameId,
						"/",
						n,
						"/token"
					}));
                }
            }
            catch (WebException)
            {
                if (num++ < 3)
                {
                    System.Threading.Thread.Sleep(5000);
                    goto IL_08;
                }
                return false;
            }
            if (IsKeyFrame)
            {
                if (!this.keyFrames.ContainsKey(n))
                {
                    this.keyFrames.Add(n, value);
                }
            }
            else
            {
                if (!this.chunks.ContainsKey(n))
                {
                    this.chunks.Add(n, value);
                }
            }
            return true;
        }
        private bool getGameChunksFromServer()
        {
            while (this.chunkGot <= this.chunkCount)
            {
                if (!this.getNthBlockAndWrite(this.chunkGot, false))
                {
                    return false;
                }
                this.chunkGot++;
            }
            return true;
        }
        private bool getKeyFramesFromServer()
        {
            while (this.keyframeGot <= this.keyframeCount)
            {
                if (!this.getNthBlockAndWrite(this.keyframeGot, true))
                {
                    return false;
                }
                this.keyframeGot++;
            }
            return true;
        }

        private bool detectobkey()
        {
            if (this.selfGame)
            {
                this.getObKeyFormLoLClientMemory();
                if (this.record.observerEncryptionKey == null)
                {
                    return false;
                }
            }
            else
            {
                Regex regex = new Regex("spectator (?<addr>[a-za-z0-9\\.-]+:[0-9]*) (?<key>.+) (?<gid>[0-9]+)", RegexOptions.IgnoreCase);
                Match match = regex.Match(this.gameLogContent);
                if (!match.Success)
                {
                    return false;
                }
                this.record.observerEncryptionKey = match.Groups["key"].Value.ToCharArray();
            }
            return true;
        }

        private string gameLogContent;
        private void getObKeyFormLoLClientMemory()
        {
            Regex regex = new Regex(" [0-9]+ (?<KEY>.+) <id_removed>", RegexOptions.IgnoreCase);
            Match match = regex.Match(this.gameLogContent);
            ProcessMemory processMemory = new ProcessMemory();
            processMemory.openProcess("LolClient");
            if (LoLRecorder._lastTimeAddress != 0u && this.analyzeObKeyInMemoryBytesMethod1(processMemory.readMemory(LoLRecorder._lastTimeAddress, 256u)))
            {
                processMemory.closeProcess();
                return;
            }
            processMemory.recordMemorysInfo();
            uint[] array = processMemory.findString(match.Groups["KEY"].Value, System.Text.Encoding.ASCII, 1);
            for (int i = 0; i < array.Length; i++)
            {
                byte[] memBytes = processMemory.readMemory(array[i], 256u);
                if (this.analyzeObKeyInMemoryBytesMethod1(memBytes))
                {
                    LoLRecorder._lastTimeAddress = array[i];
                    break;
                }
            }
            processMemory.closeProcess();
        }
        private string exePath;
        private string getLogFilePath()
        {
            this.exePath = Properties.Settings.Default.CNPath.Replace("\\game_patch", "");
            if (this.exePath == null)
            {
                return null;
            }
            string r3dLogDirectory = this.getR3dLogDirectory(this.exePath);

            Process process = Utilities.GetProcess("League of Legends");
            System.DateTime startTime = process.StartTime;
            System.IO.FileInfo fileInfo;
            do
            {
                System.Threading.Thread.Sleep(5000);
                System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(r3dLogDirectory);
                fileInfo = (
                    from x in directoryInfo.GetFiles("*.txt")
                    orderby x.LastWriteTime descending
                    select x).ToArray<System.IO.FileInfo>()[0];
            }
            while (fileInfo.CreationTime < startTime);
            return fileInfo.FullName;
        }
        private string getR3dLogDirectory(string exePath)
        {
            if (exePath.Contains("\\RADS\\solutions\\lol_game_client_sln\\releases\\"))
            {
                exePath = exePath.Substring(0, exePath.IndexOf("\\RADS"));
            }
            System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(exePath.Replace("\\League of Legends.exe", "").Replace("\\game_patch", ""));
            while (!System.IO.Directory.Exists(directoryInfo.FullName + "\\Logs"))
            {
                directoryInfo = directoryInfo.Parent;
            }
            return directoryInfo.FullName + "\\Logs\\Game - R3d Logs";

        }
        private bool detectGameIdAndPlatformId()
        {
            int num = 0;
            Match match;
            while (true)
            {
                System.Threading.Thread.Sleep(5000);
                this.getNewestLog();
                Regex regex = new Regex("GameID: (?<GID>[a-f0-9]+), PlatformID: (?<PID>[A-Z0-9_]+)", RegexOptions.IgnoreCase);
                match = regex.Match(this.gameLogContent);
                if (match.Success)
                {
                    break;
                }
                if (num++ >= 36)
                {
                    return false;
                }
            }
            this.record.gameId = ulong.Parse(match.Groups["GID"].Value, System.Globalization.NumberStyles.HexNumber);
            this.record.gamePlatform = match.Groups["PID"].Value.ToCharArray();
            return true;
        }
        private string _tempFileName;
        private void getNewestLog()
        {
            if (this._tempFileName == null)
            {
                System.Random random = new System.Random();
                this._tempFileName = random.NextDouble().ToString();
            }
            if (this.filePath == null)
            {
                return;
            }
            System.IO.File.Copy(this.filePath, System.IO.Path.GetTempPath() + "\\" + this._tempFileName, true);
            System.IO.StreamReader streamReader = new System.IO.StreamReader(System.IO.Path.GetTempPath() + "\\" + this._tempFileName);
            string text = streamReader.ReadToEnd();
            streamReader.Close();
            this.gameLogContent = text;
        } 
 
    }
}
