using System.Threading;
using MjModelLibrary;
using MjModelLibrary.Result;
using System.Collections.Generic;
using System.Diagnostics;

namespace MjServer
{
    class Program
    {
        static void Main(string[] args)
        {
            SandBox();
            var serverFacerde = new ServerFacade();
            serverFacerde.StartServer();
            
            while (true)
            {
                //wait client
                Thread.Sleep(10000);
            }
        }


        static void SandBox()
        {

        }
    }
}
