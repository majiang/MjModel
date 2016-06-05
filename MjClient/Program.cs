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
        readonly static int AllGameNum = 100;
        readonly static int ConcurrentNum = 3;

        static int FinishCount = 0;

        static void Main(string[] args)
        {
            var i = 0;
            while(i < ConcurrentNum)
            {
                var roomId = i.ToString();
                Task.Run(() => DoGame(roomId));
                i += 1;
            }

            while( FinishCount < ConcurrentNum)
            {
                Thread.Sleep(10);
            }
        }


        static void DoGame(string roomId)
        {

            var roomName = "testRoom" + roomId;
            var targetGameNum = AllGameNum / ConcurrentNum;
            var gameCount = 0;
            while (gameCount < targetGameNum)
            {
                Console.WriteLine("start game : room" + roomId + ", gameCount:" + gameCount);
                
                ClientFacade cf0 = new ClientFacade("player0", roomName);
                //cf0.SetLoggable(true);
                cf0.StartClient();

                while (cf0.IsEndGame == false)
                {
                    Thread.Sleep(10);
                }

                Console.WriteLine("end game : room" + roomId +", gameCount:"+gameCount);
                gameCount += 1;
            }
            Interlocked.Increment(ref FinishCount);

        }
    }
}
