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

        int lastActor = 0;
        bool canExecuteNextAction = false;
        MJsonMessageAll executedAction;

        public GameContext()
        {
            gameState = AfterInitialiseState.GetInstance();
        }     


        public bool ValidateMessage()
        {
            return gameState.ValidateMessage();
        }

        public bool CanExecuteNextAction()
        {
            return canExecuteNextAction;
        }

        public void ChangeState()
        {
            gameState = gameState.ChangeState();
        }
        
    }


}
