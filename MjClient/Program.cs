using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MjClient
{
    class Program
    {
        readonly static int ALL_GAME_NUM = 100;
        static void Main(string[] args)
        {
            for (int i = 0; i < ALL_GAME_NUM; i++)
            {
                Console.WriteLine("start game : " + i);
                DoOneGame();
                Console.WriteLine("end game : " + i);
            }
        }


        static void DoOneGame()
        {
            var roomName = "testRoom";
            ClientFacade cf0 = new ClientFacade("player0", roomName);


            //cf0.SetLoggable(true);

            cf0.StartClient();

            while (cf0.IsEndGame == false)
            {
                Thread.Sleep(10);
            }
        }
    }
}
