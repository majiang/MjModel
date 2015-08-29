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
        public void ルーターテスト()
        {
            ClientRouter router = new ClientRouter();
            MjsonMessageAll msg = new MjsonMessageAll();
            msg.type = "tsumo";

            var jsonstring = "{\"type\":\"tsumo\",\"id\":0}";
            router.RouteGetMessage(jsonstring);


            //var msgJsonString = JsonConvert.SerializeObject(msg);
            //router.Routeing(msgJsonString);

        }
    }
}
