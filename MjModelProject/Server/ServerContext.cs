using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjModelProject
{
    public class ServerContext
    {
        public ServerRouter serverRouter;
        public ServerController serverController;
        public ServerMjModel mjModel;
        public string roomName;
        private ServerState serverState;


        public ServerContext(ServerRouter sr, string rn)
        {
            serverRouter = sr;
            mjModel = new ServerMjModel();
            serverController = new ServerController(sr, mjModel);
            roomName = rn;
            serverState = new AfterInitialiseState(serverController);
        }

        public void Init()
        {
            mjModel = new ServerMjModel();
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
            serverState = serverState.Execute();
        }

        //ここからメッセージを受け取った際の状態遷移関数
        public void GetMessage(MjsonMessageAll msgobj)
        {
            serverState = serverState.GetMessage(msgobj);
        }
        public void Execute()
        {
            serverState = serverState.Execute();
        }

        public bool CanFinishTest()//テスト用function
        {
            return serverState.GetType() == typeof(EndState);
        }
    }
}
