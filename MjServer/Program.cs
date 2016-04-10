using System.Threading;

namespace MjServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var serverFacerde = new ServerFacade();
            serverFacerde.StartServer();

            while (true)
            {
                //continue wait call async function
                Thread.Sleep(10000);
            }
        }
    }
}
