using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MjNetworkProtocolLibrary;

namespace MjServer
{
    public delegate bool GameStart();

    public class GameContext
    {
        GameState gameState;

        List<MJsonMessageAll> messageList;
        public event GameStart GameStartHandler;
        MJsonMessageAll ReachActionBuffer;


        public GameContext()
        {
            gameState = AfterInitialiseState.GetInstance();
            
        }     
        
        public bool ValidateMessage(MJsonMessageAll msg)
        {
            return gameState.ValidateMessage(msg, messageList);
        }

        public void RegisterMessage(MJsonMessageAll msg)
        {
            messageList.Add(msg);
        }

        public bool CanExecuteNextAction()
        {
            return messageList.Count == MjModelLibrary.Constants.PLAYER_NUM;
        }

        public bool ExecuteAction()
        {
            return gameState.ExecuteAction(this, messageList);
        }
        

        // follows functions executed in state
        public bool DoGameStart()
        {
            var mjModelExecutionStatus = GameStartHandler();
            //gameState = AfterGameStartState.GetInstance();//change State
            return mjModelExecutionStatus;
        } 






    }


}
