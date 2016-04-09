using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MjNetworkProtocolLibrary;

namespace MjServer
{
    public class GameContext
    {
      //  GameState serverState;


        public GameContext()
        {

      //      serverState = new AfterInitialiseState();
        }     



        //ここからメッセージを受け取った際の状態遷移関数
        public void GetMessage(MJsonMessageAll msgobj)
        {
    //        serverState = serverState.GetMessage(msgobj);
        }

    }
}
