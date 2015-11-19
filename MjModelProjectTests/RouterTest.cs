using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MjModelProject;
using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ConsoleApplication1;
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
            ServerRouter sr = new ServerRouter();
            var jsonstring0 = "{\"type\":\"join\",\"name\":\"testname0\",\"room\":\"testroom\"}";
            var jsonstring1 = "{\"type\":\"join\",\"name\":\"testname1\",\"room\":\"testroom\"}";
            var jsonstring2 = "{\"type\":\"join\",\"name\":\"testname2\",\"room\":\"testroom\"}";
            var jsonstring3 = "{\"type\":\"join\",\"name\":\"testname3\",\"room\":\"testroom\"}";

            
            var client0 = MakeClient();
            var client1 = MakeClient();
            var client2 = MakeClient();
            var client3 = MakeClient();

            Task.Run(() => PingPongMessage(client0, jsonstring0));
            Task.Run(() => PingPongMessage(client1, jsonstring1));
            Task.Run(() => PingPongMessage(client2, jsonstring2));
            Task.Run(() => PingPongMessage(client3, jsonstring3));

            var jsonNone = "{\"type\":\"none\"}";


            PingPongMessage(client0, jsonNone);
            PingPongMessage(client1, jsonNone);
            PingPongMessage(client2, jsonNone);
            PingPongMessage(client3, jsonNone);



            PingPongMessage(client0, jsonNone);
            PingPongMessage(client1, jsonNone);
            PingPongMessage(client2, jsonNone);
            PingPongMessage(client3, jsonNone);


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
            //サーバールータテストセットアップ
            VirtualInternet vi = new VirtualInternet();


            //クライアントセットアップ
            ClientRouter cr0 = new ClientRouter(vi);
            Client cl0 = new Client(cr0);
            cr0.client = cl0;
            cl0.clientRouter.clientIpAddress = IPAddress.Parse("192.168.0.10");


            ClientRouter cr1 = new ClientRouter(vi);
            Client cl1 = new Client(cr1);
            cr1.client = cl1;
            cl1.clientRouter.clientIpAddress = IPAddress.Parse("192.168.0.11");

            ClientRouter cr2 = new ClientRouter(vi);
            Client cl2 = new Client(cr2);
            cr2.client = cl2;
            cl2.clientRouter.clientIpAddress = IPAddress.Parse("192.168.0.12");

            ClientRouter cr3 = new ClientRouter(vi);
            Client cl3 = new Client(cr3);
            cr3.client = cl3;
            cl3.clientRouter.clientIpAddress = IPAddress.Parse("192.168.0.13");

            vi.AddRouter(cl0.clientRouter.clientIpAddress, cl0.clientRouter);
            vi.AddRouter(cl1.clientRouter.clientIpAddress, cl1.clientRouter);
            vi.AddRouter(cl2.clientRouter.clientIpAddress, cl2.clientRouter);
            vi.AddRouter(cl3.clientRouter.clientIpAddress, cl3.clientRouter);


            //サーバーセットアップ
            ServerRouter sr = new ServerRouter();
            //vi.AddRouter(Constants.SERVER_IP, sr);
            

           

            var jsonstring0 = "{\"type\":\"join\",\"name\":\"testname0\",\"room\":\"testroom\"}";
            var jsonstring1 = "{\"type\":\"join\",\"name\":\"testname1\",\"room\":\"testroom\"}";
            var jsonstring2 = "{\"type\":\"join\",\"name\":\"testname2\",\"room\":\"testroom\"}";
            var jsonstring3 = "{\"type\":\"join\",\"name\":\"testname3\",\"room\":\"testroom\"}";

            

            /*

            cl0.clientRouter.SendMessageToServer(jsonstring0);
            cl1.clientRouter.SendMessageToServer(jsonstring1);
            cl2.clientRouter.SendMessageToServer(jsonstring2);
            cl3.clientRouter.SendMessageToServer(jsonstring3);
            */
            //セットアップここまで


            //sr.UpDateServer();//受信したメッセージを処理

            Assert.IsTrue(sr.clientNameRoomDictionary.ContainsKey("testname0"));
            Assert.AreEqual(sr.clientNameRoomDictionary["testname0"], "testroom");

            cl0.clientRouter.UpDateServer();
            //Console.Write(cl0.positionId);





        }
        
    }
}
