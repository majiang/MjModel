using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MjModelProject;
using Newtonsoft.Json;

namespace MjModelProjectTests
{
    [TestClass]
    public class RouterTest
    {
        [TestMethod]
        public void クライアントルーターテスト()
        {
            ClientRouter clientRouter = new ClientRouter();
            ClientController cc = new ClientController();
            clientRouter.SetClientController(cc);

            MjsonMessageAll msg = new MjsonMessageAll();
            msg.type = "tsumo";

            var jsonstring = "{\"type\":\"tsumo\",\"id\":0}";
            clientRouter.RouteGetMessage(jsonstring);


            //var msgJsonString = JsonConvert.SerializeObject(msg);
            //router.Routeing(msgJsonString);

        }

        [TestMethod]
        public void サーバールーターテスト()
        {
            ServerRouter serverRouter = new ServerRouter();
        }


        [TestMethod]
        public void ルーター間通信テスト()
        {

        }

    }
}
