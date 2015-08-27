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
            Router router = new Router();
            MjsonMessage msg = new MjsonMessage();
            msg.type = "tsumo";

            var jsonstring = "{\"type\":\"tsumo\",\"id\":0}";
            router.Routeing(jsonstring);


            //var msgJsonString = JsonConvert.SerializeObject(msg);
            //router.Routeing(msgJsonString);

        }
    }
}
