using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MjModelProject;
using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Text;

using System.Threading.Tasks;
using System.IO;

namespace MjModelProjectTests
{
    [TestClass]
    public class RouterTest
    {
        [TestMethod]
        public void クライアントルーターテスト()
        {


        }

        [TestMethod]
        public void サーバールーターテスト()
        {


        }

        public TcpClient MakeClient()
        {
            string ipOrHost = "127.0.0.1";
            int port = 10021;
            TcpClient client = new TcpClient(ipOrHost, port);
            return client;

        }

        public async void PingPongMessage(TcpClient client ,string jsonString)
        {
            var stream = client.GetStream();

            // 送信
            Encoding enc = Encoding.UTF8;
            byte[] sendBytes = enc.GetBytes(jsonString);
            //データを送信する
            stream.Write(sendBytes, 0, sendBytes.Length);

            //受信
            StreamReader reader = new StreamReader(client.GetStream());
            string line = await reader.ReadLineAsync();
            Console.WriteLine("-TestChat Message:" + line);


        }


        
        [TestMethod]
        public void ルーター間通信テスト()
        {
 




        }
        
    }
}
