using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MjModelProject;
using Newtonsoft.Json;
using System.Net;

namespace MjModelProjectTests
{
    [TestClass]
    public class IntegrationTest
    {
        [TestMethod]
        public void 統合ツモ切りクライアントテスト()
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
            ServerRouter sr = new ServerRouter(vi);
            
            vi.AddRouter(Constants.SERVER_IP, sr);

            var rn = "room";
            cl0.kickGame("tester0", rn);
            cl1.kickGame("tester1", rn);
            cl2.kickGame("tester2", rn);
            cl3.kickGame("tester3", rn);
            sr.UpDateServer();//受信したメッセージを処理   
            //セットアップここまで

            while (sr.roomNameServerDictionary[rn].CanFinishTest() == false)
            {
                cl0.clientRouter.UpDateServer();
                cl1.clientRouter.UpDateServer();
                cl2.clientRouter.UpDateServer();
                cl3.clientRouter.UpDateServer();
                sr.UpDateServer();//受信したメッセージを処理  
            }
        }
    }
}
