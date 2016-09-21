using System;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
namespace LOLReplay
{
	public class LoLRecordPlayer
	{
		private LoLRecord record;
		private TcpListener listener;
		private Socket dataSocket;
		public LoLRecordPlayer(LoLRecord playThis)
		{
			this.record = playThis;
		}
		public void StopPlaying()
		{
			using (WebClient webClient = new WebClient())
			{
				try
				{
                    webClient.DownloadString("http://127.0.0.1:8089/exit");
				}
				catch (System.Exception)
				{
				}
			}
		}
		public void startPlaying()
		{
            IPAddress localaddr = IPAddress.Parse("127.0.0.1");
            if (this.listener != null)
            {
                this.listener.Stop();
            }
			this.listener = new TcpListener(localaddr, 8089);
			this.listener.Start();
			while (true)
			{
				try
				{
					this.dataSocket = this.listener.AcceptSocket();
				}
				catch (SocketException)
				{
					return;
				}
				if (this.dataSocket.Connected)
				{
					byte[] array = new byte[1024];
					int count = this.dataSocket.Receive(array);
					string @string = System.Text.Encoding.ASCII.GetString(array, 0, count);
					byte[] array2 = this.gotRequest(@string);
					if (array2 == null)
					{
						break;
					}
					this.dataSocket.Send(array2, SocketFlags.None);
				}
				this.dataSocket.Close();
			}
			this.dataSocket.Send(this.strToByte(this.makeHeader(2, 0) + "OK"), SocketFlags.None);
			this.dataSocket.Close();
			this.listener.Stop();
		}
		private string makeHeader(int size, int type)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("HTTP/1.1 200 OK\r\n");
			stringBuilder.Append("Server: LOLCN.cc Spectator\r\n");
			stringBuilder.Append("Pragma: no-cache\r\n");
			stringBuilder.Append("Cash-Control: no-cache\r\n");
			stringBuilder.Append("Expires: Thu, 01 Jan 1970 08:00:00 CST \r\n");
			if (type == 1)
			{
				stringBuilder.Append("Content-Type: application/json\r\n");
			}
			if (type == 2)
			{
				stringBuilder.Append("Content-Type: application/octet-stream\r\n");
			}
			else
			{
				stringBuilder.Append("Content-Type: text/plain\r\n");
			}
			stringBuilder.Append("Content-Length: " + size + "\r\n");
			stringBuilder.Append("Accept-Ranges: bytes\r\n");
			System.Globalization.CultureInfo provider = new System.Globalization.CultureInfo("en-US");
			//System.DateTime dateTime = default(System.DateTime);
			stringBuilder.Append("Date: " + System.DateTime.UtcNow.ToString("ddd, dd MM yyyy HH:mm:ss GMT\r\n", provider));
			stringBuilder.Append("Connection: close\r\n\r\n");
			return stringBuilder.ToString();
		}
		private byte[] strToByte(string str)
		{
			return System.Text.Encoding.ASCII.GetBytes(str);
		}
		private bool checkGameId(string getRequest)
		{
			Regex regex = new Regex("/" + new string(this.record.gamePlatform) + "/(?<GID>[0-9]+)", RegexOptions.IgnoreCase);
			Match match = regex.Match(getRequest);
			return this.record.gameId == ulong.Parse(match.Groups["GID"].Value);
		}
		private byte[] appendBytes(byte[] b1, byte[] b2)
		{
			byte[] array = new byte[b1.Length + b2.Length];
			b1.CopyTo(array, 0);
			b2.CopyTo(array, b1.Length);
			return array;
		}
		private byte[] gotRequest(string reqstr)
		{
            Console.WriteLine(reqstr);
            try
            {
                if (reqstr.Contains("/observer-mode/rest/consumer/version"))
                {
                    return this.strToByte(this.makeHeader(this.record.spectatorClientVersion.Length, 0) + new string(this.record.spectatorClientVersion));
                }
                if (reqstr.Contains("/exit"))
                {
                    return null;
                }
                if (!this.checkGameId(reqstr))
                {
                    return null;
                }
                if (reqstr.Contains("/observer-mode/rest/consumer/getGameMetaData/" + new string(this.record.gamePlatform)))
                {
                    string metaData = this.record.getMetaData();
                    return this.strToByte(this.makeHeader(metaData.Length, 1) + metaData);
                }
                if (reqstr.Contains("/observer-mode/rest/consumer/getLastChunkInfo/" + new string(this.record.gamePlatform)))
                {
                    string lastChunkInfo = this.record.getLastChunkInfo();
                    return this.strToByte(this.makeHeader(lastChunkInfo.Length, 1) + lastChunkInfo);
                }
                if (reqstr.Contains("/observer-mode/rest/consumer/getGameDataChunk/" + new string(this.record.gamePlatform) + "/"))
                {
                    Regex regex = new Regex("/observer-mode/rest/consumer/getGameDataChunk/" + new string(this.record.gamePlatform) + "/(?<GID>[0-9]+)/(?<NUMBER>[0-9]+)", RegexOptions.IgnoreCase);
                    Match match = regex.Match(reqstr);
                    byte[] chunkContent = this.record.getChunkContent(int.Parse(match.Groups["NUMBER"].Value));
                    byte[] b = this.strToByte(this.makeHeader(chunkContent.Length, 2));
                    return this.appendBytes(b, chunkContent);
                }
                if (reqstr.Contains("/observer-mode/rest/consumer/getKeyFrame/" + new string(this.record.gamePlatform) + "/"))
                {
                    Regex regex2 = new Regex("/observer-mode/rest/consumer/getKeyFrame/" + new string(this.record.gamePlatform) + "/(?<GID>[0-9]+)/(?<NUMBER>[0-9]+)", RegexOptions.IgnoreCase);
                    Match match2 = regex2.Match(reqstr);
                    byte[] keyFrameContent = this.record.getKeyFrameContent(int.Parse(match2.Groups["NUMBER"].Value));
                    byte[] b2 = this.strToByte(this.makeHeader(keyFrameContent.Length, 2));
                    return this.appendBytes(b2, keyFrameContent);
                }
                return null;
            }
            catch
            {
                return null;
            }
		}
	}
}
