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
        List<ClientHolderInterface> clients = new List<ClientHolderInterface>();
        List<string> messageStack = new List<string>();
        public GameRoom( List<ClientHolderInterface> inClients )
        {
            clients.AddRange(inClients);
            clients.ForEach(e => e.GetMessageFromClientHandler += OnGetMessageFromClient);
        }
        // -Logger
        // -MjModel

        // -message router
        // -validator

         SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
         void OnGetMessageFromClient(string message, ClientHolderInterface client)
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


         void OnErrorDetected(string jsonEerrorMessage)
        {
            // log errorMessage

            // send error to client
            clients.ForEach(e => e.SendMessage(jsonEerrorMessage));

            // disconnect client
            clients.ForEach(e => e.Disconnect());
            clients.Clear();
            
        }


         void SendSameMessageToClientsHandler(string jsonMessage)
        {
            clients.ForEach(e => e.SendMessage(jsonMessage));
        }

         void SendMessageToOneClientHandlers(string message, int targetClientId)
        {
            Debug.Assert(0 <= targetClientId && targetClientId < clients.Count);
            clients[targetClientId].SendMessage(message);
        }


        // mj action functions
         void OnTsumo()
        {

        }




    }

    


}
