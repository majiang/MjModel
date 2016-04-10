using Microsoft.VisualStudio.TestTools.UnitTesting;
using MjServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MjServer.Tests
{
    [TestClass()]
    public class ServerFacadeTests
    {
        [TestMethod()]
        public void ServerFacadeTest()
        {
            ServerFacade sf = new ServerFacade();
            sf.StartServer();
            
        }
    }
}