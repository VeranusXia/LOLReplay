using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using ReplayDll;
namespace LOLReplay
{
    public class LoLRecord
    {
        public const int LPRLatestVersion = 4;
        public bool IsBroken;
        public int ThisLPRVersion = 4;
        public string relatedFileName;
        public char[] spectatorClientVersion;
        public ulong gameId;
        public int gameChunkTimeInterval;
        public int gameKeyFrameTimeInterval;
        public int gameStartChunkId;
        public int gameEndKeyFrameId;
        public int gameEndChunkId;
        public int gameEndStartupChunkId;
        public int gameLength;
        public int gameClientAddLag;
        public int gameELOLevel;
        public char[] gamePlatform;
        public char[] observerEncryptionKey;
        public char[] gameCreateTime;
        public char[] gameStartTime;
        public char[] gameEndTime;
        public int gameDelayTime;
        public int gameLastChunkTime;
        public int gameLastChunkDuration;
        public char[] lolVersion;
        public bool hasResult;
        public EndOfGameStats gameStats;
        public System.Collections.Generic.Dictionary<int, byte[]> gameChunks;
        public System.Collections.Generic.Dictionary<int, byte[]> gameKeyFrames;
        private byte[] endOfGameStatsBytes;
        public PlayerInfo[] players;
     
        public string getMetaData()
        {
            JObject jObject = new JObject();
            jObject.Add("gameId", this.gameId);
            jObject.Add("platformId", new string(this.gamePlatform));

            return new JObject
			{
				{
					"gameKey",
					jObject
				},

				{
					"gameServerAddress",
					""
				},

				{
					"port",
					0
				},

				{
					"encryptionKey",
					""
				},

				{
					"chunkTimeInterval",
					this.gameChunkTimeInterval
				},

				{
					"startTime",
					new string(this.gameStartTime)
				},

				{
					"endTime",
					new string(this.gameEndTime)
				},

				{
					"gameEnded",
					true
				},

				{
					"lastChunkId",
					this.gameEndChunkId
				},

				{
					"lastKeyFrameId",
					this.gameEndKeyFrameId
				},
                {
					"endGameChunkId",
					-1
				},
                {
                    "endGamekeyFrameId",
                    -1
                },
				{
					"endStartupChunkId",
					this.gameEndStartupChunkId
				},

				{
					"delayTime",
					this.gameDelayTime
				},

				{
					"pendingAvailableChunkInfo",
					""
				},

				{
					"pendingAvailableKeyFrameInfo",
					""
				},

				{
					"keyFrameTimeInterval",
					this.gameKeyFrameTimeInterval
				},

				{
					"decodedEncryptionKey",
					""
				},

				{
					"startGameChunkId",
					this.gameStartChunkId
				},

				{
					"gameLength",
					this.gameLength
				},

				{
					"clientAddedLag",
					this.gameClientAddLag
				},

				{
					"clientBackFetchingEnabled",
					true
				},

				{
					"clientBackFetchingFreq",
					"50"
				},

				{
					"interestScore",
					this.gameELOLevel
				},

				{
					"featuredGame",
					"false"
				},

				{
					"createTime",
					new string(this.gameCreateTime)
				}
			}.ToString();
        }

        public string getLastChunkInfo()
        {
            return new JObject
			{

				{
					"chunkId",
					this.gameEndChunkId
				},

				{
					"availableSince",
					this.gameLastChunkTime
				},

				{
					"nextAvailableChunk",
					1000
				},

				{
					"keyFrameId",
					this.gameEndKeyFrameId
				},

				{
					"nextChunkId",
					this.gameEndChunkId
				},

				{
					"endStartupChunkId",
					this.gameEndStartupChunkId
				},

				{
					"startGameChunkId",
					this.gameStartChunkId
				},

				{
					"endGameChunkId",
					this.gameEndChunkId
				},

				{
					"duration",
					this.gameLastChunkDuration
				}
			}.ToString();
        }
        private void allocateChunkAndKeyFrameSpaces()
        {
            this.gameKeyFrames = new System.Collections.Generic.Dictionary<int, byte[]>();
            this.gameChunks = new System.Collections.Generic.Dictionary<int, byte[]>();
        }
    
        public byte[] getKeyFrameContent(int n)
        {
            if (this.gameKeyFrames.ContainsKey(n))
            {
                return this.gameKeyFrames[n];
            }
            return null;
        }
       
        public byte[] getChunkContent(int n)
        {
            if (this.gameChunks.ContainsKey(n))
            {
                return this.gameChunks[n];
            }
            return new byte[] { };
        }
        
       
        public void readFromFile(string path, bool withOutChunks)
        {
            this.relatedFileName = path;
            try
            {
                System.IO.FileStream fileStream = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                System.IO.BinaryReader binaryReader = new System.IO.BinaryReader(fileStream);
                this.ThisLPRVersion = binaryReader.ReadInt32();
                if (this.ThisLPRVersion >= 0)
                {
                    int count = binaryReader.ReadInt32();
                    this.spectatorClientVersion = binaryReader.ReadChars(count);
                    if (this.ThisLPRVersion < 2)
                    {
                        this.gameId = (ulong)((long)binaryReader.ReadInt32());
                    }
                    else
                    {
                        this.gameId = binaryReader.ReadUInt64();
                    }
                    this.gameEndStartupChunkId = binaryReader.ReadInt32();
                    this.gameStartChunkId = binaryReader.ReadInt32();
                    this.gameEndChunkId = binaryReader.ReadInt32();
                    this.gameEndKeyFrameId = binaryReader.ReadInt32();
                    this.gameLength = binaryReader.ReadInt32();
                    this.gameDelayTime = binaryReader.ReadInt32();
                    this.gameClientAddLag = binaryReader.ReadInt32();
                    this.gameChunkTimeInterval = binaryReader.ReadInt32();
                    this.gameKeyFrameTimeInterval = binaryReader.ReadInt32();
                    this.gameELOLevel = binaryReader.ReadInt32();
                    this.gameLastChunkTime = binaryReader.ReadInt32();
                    this.gameLastChunkDuration = binaryReader.ReadInt32();
                    count = binaryReader.ReadInt32();
                    this.gamePlatform = binaryReader.ReadChars(count);
                    count = binaryReader.ReadInt32();
                    this.observerEncryptionKey = binaryReader.ReadChars(count);
                    count = binaryReader.ReadInt32();
                    this.gameCreateTime = binaryReader.ReadChars(count);
                    count = binaryReader.ReadInt32();
                    this.gameStartTime = binaryReader.ReadChars(count);
                    count = binaryReader.ReadInt32();
                    this.gameEndTime = binaryReader.ReadChars(count);
                    if (this.ThisLPRVersion >= 3)
                    {
                        count = binaryReader.ReadInt32();
                        this.lolVersion = binaryReader.ReadChars(count);
                    }
                    else
                    {
                        this.lolVersion = string.Empty.ToCharArray();
                    }
                    if (this.ThisLPRVersion >= 2)
                    {
                        this.hasResult = binaryReader.ReadBoolean();
                        if (this.ThisLPRVersion >= 4)
                        {
                            if (this.hasResult)
                            {
                                count = binaryReader.ReadInt32();
                                this.endOfGameStatsBytes = binaryReader.ReadBytes(count);
                                this.gameStats = new EndOfGameStats(this.endOfGameStatsBytes);
                            }
                            if (binaryReader.ReadBoolean())
                            {
                                this.readPlayerOldFormat(binaryReader);
                            }
                        }
                        else
                        {
                            if (this.hasResult)
                            {
                                count = binaryReader.ReadInt32();
                                this.endOfGameStatsBytes = binaryReader.ReadBytes(count);
                                this.gameStats = new EndOfGameStats(this.endOfGameStatsBytes);
                            }
                            else
                            {
                                this.readPlayerOldFormat(binaryReader);
                            }
                        }
                    }
                    else
                    {
                        this.readPlayerOldFormat(binaryReader);
                    }
                    if (!withOutChunks)
                    {
                        this.readChunks(binaryReader);
                    }

                    binaryReader.Close();
                    fileStream.Close();
                }
            }
            catch
            {
                this.IsBroken = true;
            }
        }
        public void readPlayerOldFormat(System.IO.BinaryReader dataReader)
        {
            this.players = new PlayerInfo[dataReader.ReadInt32()];
            for (int i = 0; i < this.players.Length; i++)
            {
                this.players[i] = new PlayerInfo();
                int count = dataReader.ReadInt32();
                char[] value = dataReader.ReadChars(count);
                this.players[i].playerName = new string(value);
                count = dataReader.ReadInt32();
                char[] value2 = dataReader.ReadChars(count);
                this.players[i].championName = new string(value2);
                this.players[i].team = dataReader.ReadUInt32();
                this.players[i].clientID = dataReader.ReadInt32();
            }
        }
        private void readChunks(System.IO.BinaryReader dataReader)
        {
            this.allocateChunkAndKeyFrameSpaces();
            if (this.ThisLPRVersion == 0)
            {
                this.readChunksVersion0(dataReader);
                return;
            }
            int num = dataReader.ReadInt32();
            for (int i = 0; i < num; i++)
            {
                int key = dataReader.ReadInt32();
                int count = dataReader.ReadInt32();
                this.gameKeyFrames.Add(key, dataReader.ReadBytes(count));
            }
            num = dataReader.ReadInt32();
            for (int j = 0; j < num; j++)
            {
                int key2 = dataReader.ReadInt32();
                int count2 = dataReader.ReadInt32();
                this.gameChunks.Add(key2, dataReader.ReadBytes(count2));
            }
        }
        private void readChunksVersion0(System.IO.BinaryReader dataReader)
        {
            for (int i = 0; i < this.gameEndKeyFrameId; i++)
            {
                int count = dataReader.ReadInt32();
                this.gameKeyFrames.Add(i + 1, dataReader.ReadBytes(count));
            }
            for (int j = 0; j < this.gameEndStartupChunkId; j++)
            {
                int count = dataReader.ReadInt32();
                this.gameChunks.Add(j + 1, dataReader.ReadBytes(count));
            }
            for (int k = 0; k <= this.gameEndChunkId - this.gameStartChunkId; k++)
            {
                int count = dataReader.ReadInt32();
                this.gameChunks.Add(this.gameStartChunkId + k, dataReader.ReadBytes(count));
            }
        }
          
    }
}
