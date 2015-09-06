using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MjModelProject;
using Newtonsoft.Json;
using System.Net;

namespace MjModelProjectTests
{
    [TestClass]
    public class RouterTest
    {
        [TestMethod]
        public void クライアントルーター単体テスト()
        {
            ClientRouter clientRouter = new ClientRouter();
            ClientController cc = new ClientController();
            ServerRouter sr = new ServerRouter();
            clientRouter.SetClientController(cc);


            MjsonMessageAll msg = new MjsonMessageAll();
            msg.type = "tsumo";
        }

        [TestMethod]
        public void サーバールーター単体テスト()
        {

        }


        [TestMethod]
        public void ルーター間通信結合テスト()
        {
            //サーバールータテストセットアップ
            VirtualInternet vi = new VirtualInternet();

            //クライアントセットアップ
            Client cl0 = new Client();
            cl0.clientRouter.virtualInternet = vi;
            cl0.clientRouter.clientIpAddress = IPAddress.Parse("192.168.0.10");
            Client cl1 = new Client();
            cl1.clientRouter.virtualInternet = vi;
            cl1.clientRouter.clientIpAddress = IPAddress.Parse("192.168.0.11");
            Client cl2 = new Client();
            cl2.clientRouter.virtualInternet = vi;
            cl2.clientRouter.clientIpAddress = IPAddress.Parse("192.168.0.12");
            Client cl3 = new Client();
            cl3.clientRouter.virtualInternet = vi;
            cl3.clientRouter.clientIpAddress = IPAddress.Parse("192.168.0.13");

            vi.AddRouter(cl0.clientRouter.clientIpAddress, cl0.clientRouter);
            vi.AddRouter(cl1.clientRouter.clientIpAddress, cl1.clientRouter);
            vi.AddRouter(cl2.clientRouter.clientIpAddress, cl2.clientRouter);
            vi.AddRouter(cl3.clientRouter.clientIpAddress, cl3.clientRouter);


            //サーバーセットアップ
            Server sv = new Server();
            sv.serverRouter.virtualInternet = vi;
            vi.AddRouter(Constants.SERVER_IP, sv.serverRouter);


            //セットアップここまで

            MjsonMessageAll msg = new MjsonMessageAll();
            msg.type = "tsumo";

            var jsonstring0 = "{\"type\":\"join\",\"name\":\"testname0\",\"room\":\"testroom\"}";
            cl0.clientRouter.SendMessageToServer(jsonstring0);

            var jsonstring1 = "{\"type\":\"join\",\"name\":\"testname1\",\"room\":\"testroom\"}";
            cl1.clientRouter.SendMessageToServer(jsonstring1);

            var jsonstring2 = "{\"type\":\"join\",\"name\":\"testname2\",\"room\":\"testroom\"}";
            cl2.clientRouter.SendMessageToServer(jsonstring2);

            var jsonstring3 = "{\"type\":\"join\",\"name\":\"testname3\",\"room\":\"testroom\"}";
            cl3.clientRouter.SendMessageToServer(jsonstring3);

            sv.serverRouter.UpDateServer();

            Assert.IsTrue(sv.serverRouter.clientNameRooms.ContainsKey("testname0"));
            Assert.AreEqual(sv.serverRouter.clientNameRooms["testname0"], "testroom");
        }

    }
}
