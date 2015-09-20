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
            serverState = new AfterStartGameState(serverController);
        }

        public void Init()
        {
            mjModel = new ServerMjModel();
            serverState = new AfterStartGameState(serverController);
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
        public void Execute()
        {
            serverState = serverState.Execute();
        }


        
        //以下Executeから呼び出される関数群.
       // iranaikamo

        public void GetDahai(MjsonMessageAll msgobj)
        {
            serverController.Dahai(msgobj.actor, msgobj.pai, msgobj.tsumogiri);
        }

        public void GetPon(MjsonMessageAll msgobj)
        {
            serverController.Pon(msgobj.actor, msgobj.target, msgobj.pai, msgobj.consumed);
        }

        public void GetChi(MjsonMessageAll msgobj)
        {
            serverController.Chi(msgobj.actor, msgobj.target, msgobj.pai, msgobj.consumed);
        }

        public void GetKakan(MjsonMessageAll msgobj)
        {
            serverController.Kakan(msgobj.actor, msgobj.target, msgobj.pai, msgobj.consumed);
        }

        public void GetAnkan(MjsonMessageAll msgobj)
        {
            serverController.Ankan(msgobj.actor, msgobj.target, msgobj.pai, msgobj.consumed);
        }

        public void GetDaiminkan(MjsonMessageAll msgobj)
        {
            serverController.Daiminkan(msgobj.actor, msgobj.target, msgobj.pai, msgobj.consumed);
        }

        public void GetReach(MjsonMessageAll msgobj)
        {
            serverController.Reach(msgobj.actor);
        }

        public void GetHora(MjsonMessageAll msgobj)
        {
            serverController.Hora(msgobj.actor, msgobj.target, msgobj.pai);
        }

        public void GetNone()
        {
            //log
        }





    }
}
