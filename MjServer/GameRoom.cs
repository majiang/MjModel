using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjServer
{
    class GameRoom
    {
        // -Logger
        // -MjModel
        // -message router
        // -validator


        private void OnGetMessageFromClient(string message)
        {
            // understand message
            // validate message 
            // push Message to Stack
            // if server gets messages from all clients, fire event and send message.
            
        }

        private void OnError(string errorMessage)
        {
            // log errorMessage
            // send error to client
            // disconnect client
        } 


        private void SendSameMessageToClients(string message)
        {

        }

        private void SendIndividualMessageToClients(string message)
        {

        }


        // mj action functions
        private void DoTsumo()
        {

        }




    }

    


}
