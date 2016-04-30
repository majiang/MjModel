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


        GameState gameState;

        List<MJsonMessageAll> messageList;
//        public event GameStart GameStartHandler;
        MJsonMessageAll ReachActionBuffer;


        public GameContext()
        {
            gameState = AfterInitialiseState.GetInstance();
        }     
        
        public bool ValidateMessage(MJsonMessageAll msg)
        {
            return gameState.ValidateMessage(msg);
        }

        public void RegisterMessage(MJsonMessageAll msg)
        {
            messageList.Add(msg);
        }

        public bool HasRecievedMessageFromAllClients()
        {
            return messageList.Count == MjModelLibrary.Constants.PLAYER_NUM;
        }

        public bool ExecuteAction()
        {
            return gameState.ExecuteAction(this, messageList);
        }
        



        public List<MJsonMessageAll> GetMessageList()
        {
            return messageList;
        }
 
        // overrode
        /*
        public void ChangeState(MJsonMessageTsumo msg)
        {
            gameState = AfterTsumoState.GetInstance();
            gameState.SetLastActor(msg.actor);
        }
        public void ChangeState(MJsonMessageTsumo msg)
        {
            gameState = AfterTsumoState.GetInstance();
            gameState.SetLastActor(msg.actor);
        }
        public void ChangeState(MJsonMessageTsumo msg)
        {
            gameState = AfterTsumoState.GetInstance();
            gameState.SetLastActor(msg.actor);
        }
        public void ChangeState(MJsonMessageTsumo msg)
        {
            gameState = AfterTsumoState.GetInstance();
            gameState.SetLastActor(msg.actor);
        }
        public void ChangeState(MJsonMessageTsumo msg)
        {
            gameState = AfterTsumoState.GetInstance();
            gameState.SetLastActor(msg.actor);
        }
        public void ChangeState(MJsonMessageTsumo msg)
        {
            gameState = AfterTsumoState.GetInstance();
            gameState.SetLastActor(msg.actor);
        }
        */
    }


}
