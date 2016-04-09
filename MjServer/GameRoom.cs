using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using MjModelLibrary;
using System.Threading;
using MjNetworkProtocolLibrary;
using Newtonsoft.Json;

namespace MjServer
{
    class GameRoom
    {
        List<ClientHolderInterface> clients;
        List<string> messageStack;

        public GameRoom(
            ClientHolderInterface client0,
            ClientHolderInterface client1,
            ClientHolderInterface client2,
            ClientHolderInterface client3
        )
        {
            clients.Add(client0);
            clients.Add(client1);
            clients.Add(client2);
            clients.Add(client3);

            clients.ForEach(e => e.GetMessageFromClientHandler += OnGetMessageFromClient);
        }
        // -Logger
        // -MjModel

        // -message router
        // -validator

        private SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        private void OnGetMessageFromClient(string message)
        {
            semaphore.WaitAsync();

            // validate message
            // var isValidMessageType = validator.validateMessage()
            
            // if ( isValidMessageType == false )
            // {
            //      OnErrorDetected( message );
            //      return;
            // }

            // if server gets messages from all clients, fire event and send message.
            messageStack.Add(message);
            if (messageStack.Count < clients.Count)
            {
                
                // execute message
                // var isValidChange = chengeModel()
                // if( isValidChange == false )
                // {
                //      OnErrorDetected( message );
                //      return;
                // }
                // register executed message to validator

            }
            semaphore.Release();
        }


        private void OnErrorDetected(string jsonEerrorMessage)
        {
            // log errorMessage

            // send error to client
            clients.ForEach(e => e.SendMessage(jsonEerrorMessage));

            // disconnect client
            clients.ForEach(e => e.Disconnect());
            clients.Clear();
            
        }


        private void SendSameMessageToClientsHandler(string jsonMessage)
        {
            clients.ForEach(e => e.SendMessage(jsonMessage));
        }

        private void SendMessageToOneClientHandlers(string message, int targetClientId)
        {
            Debug.Assert(0 <= targetClientId && targetClientId < clients.Count);
            clients[targetClientId].SendMessage(message);
        }


        // mj action functions
        private void OnTsumo()
        {

        }




    }

    


}
