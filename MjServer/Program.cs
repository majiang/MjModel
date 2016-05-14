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
            var tehai = new List<string>() { "1m","2m", "3m" };
            var dict = new List<Dictionary<string, string>>();
            dict.Add(new Dictionary<string, string>() { { "type", "minsyun" }, { "target", "1" }, { "pai", "1m" }, { "consumed", "2m, 3m" } });

            var rt = new ReadOnlyTehai(tehai, dict);
            Debug.WriteLine(rt);
        }
    }
}
