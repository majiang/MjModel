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
        List<string> clientNames = new List<string>();
        List<string> messageStack = new List<string>();
        public GameRoom(Dictionary<ClientHolderInterface, string> inClients)
        {
            foreach(var client in inClients)
            {
                client.Key.GetMessageFromClientHandler += OnGetMessageFromClient;
                clients.Add(client.Key);
                clientNames.Add(client.Value);
            }
        }
        // -Logger
        // -MjModel

        // -message router
        // -validator

        public void StartGame()
        {
            //shuffle client


            //each message has difference in playerID
            foreach (var client in clients.Select((e,index) => new { e, index}))
            {
                var gameStartMessage = JsonConvert.SerializeObject(new MJsonMessageStartGame(client.index,clientNames));
                SendMessageToOneClient(gameStartMessage, client.index);
            }
        }


        public void SendSameMessageToClients(string jsonMessage)
        {
            clients.ForEach(e => e.SendMessage(jsonMessage));
        }

        public void SendMessageToOneClient(string message, int targetClientId)
        {
            Debug.Assert(0 <= targetClientId && targetClientId < clients.Count);
            clients[targetClientId].SendMessage(message);
        }


        SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        async void OnGetMessageFromClient(string message, ClientHolderInterface client)
        {
            await semaphore.WaitAsync();

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




        // mj action functions
         void OnTsumo()
        {

        }




    }

    


}
