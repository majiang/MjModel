using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MjNetworkProtocolLibrary;
namespace MjServer
{
    interface GameState
    {
        bool ValidateMessage(MJsonMessageAll msg, List<MJsonMessageAll> msgList);
        bool CanExecuteNextAction();
        bool ExecuteAction(GameContext context, List<MJsonMessageAll> msgList);


    }
    class StateBase
    {
        MJsonMessageAll GetMostHighPriorityMessage(List<MJsonMessageAll> msgList)
        {
            throw new NotImplementedException();
        }
    }
    class AfterInitialiseState : GameState
    {
        int lastActor = 0;
        private static GameState state = new AfterInitialiseState();
        private AfterInitialiseState() { }
        public static GameState GetInstance()
        {
            return state;
        }

        public bool ValidateMessage(MJsonMessageAll msg, List<MJsonMessageAll> msgList)
        {

            //validate msgtype 
            //validate use model


            throw new NotImplementedException();
        }

        public bool CanExecuteNextAction()
        {
            throw new NotImplementedException();
        }

        public bool ExecuteAction(GameContext context, List<MJsonMessageAll> msg)
        {
            //execute model by using 4 message
            //change state
            return context.DoGameStart();
        }

    }
}
