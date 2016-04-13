using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldMjServer
{
    public class GameContext
    {
        public ServerRouter serverRouter;
        public GameController serverController;
        public GameModel mjModel;
        public string roomName;
        GameState serverState;


        public GameContext(ServerRouter sr, string rn)
        {
            serverRouter = sr;
            mjModel = new GameModel();
            serverController = new GameController(sr, mjModel);
            roomName = rn;
            serverState = new AfterInitialiseState(serverController);
        }     


        public bool CanJoin()
        {
            return serverController.CanJoin();
        }

        public void GetJoin(MjsonMessageAll msgobj)
        {
            serverController.Join(msgobj.name);
            serverState = serverState.GetMessage(msgobj);
        }

        //ここからメッセージを受け取った際の状態遷移関数
        public void GetMessage(MjsonMessageAll msgobj)
        {
            serverState = serverState.GetMessage(msgobj);
        }

    }
}
