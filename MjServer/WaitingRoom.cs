using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

using Newtonsoft.Json;
using MjNetworkProtocolLibrary;
using MjModelLibrary;

namespace MjServer
{
    delegate void StartGameRoom(List<ClientHolderInterface> client);

    class WaitingRoom
    {
        public event StartGameRoom StartRoomHandler;
        Dictionary<ClientHolderInterface, string> clientRoomNameDictionary;
        Dictionary<string, int> roomNameWaitingNumDictionary;

        public WaitingRoom() { }
        
        public async void StartWaiting()
        {

            IPAddress ipAdd = IPAddress.Parse(Properties.Settings.Default.ipAddress);//LAN
            int port = Properties.Settings.Default.port;
            TcpListener server = new TcpListener(ipAdd, port);

            server.Start();
            Console.WriteLine(
                "Start Listen({0}:{1})",
                ( (IPEndPoint)server.LocalEndpoint).Address,
                ( (IPEndPoint)server.LocalEndpoint).Port
                );
            
            clientRoomNameDictionary = new Dictionary<ClientHolderInterface, string>();
            roomNameWaitingNumDictionary = new Dictionary<string, int>();

            while (true)
            {
                TcpClient client = await server.AcceptTcpClientAsync();
                ClientUsingTcpHolder clientHolder = new ClientUsingTcpHolder(client);
                clientHolder.GetMessageFromClientHandler += RouteMessage;
                Task.Run(() => clientHolder.StartWaiting());
            }
        }


        private System.Threading.SemaphoreSlim semaphore = new System.Threading.SemaphoreSlim(1, 1);
        void RouteMessage(string message, ClientHolderInterface clientHolder)
        {
        
            MJsonMessageAll mjsonObject = null;
            try
            {
                mjsonObject = JsonConvert.DeserializeObject<MJsonMessageAll>(message);
            }
            catch (JsonException e)
            {
                if (mjsonObject != null)
                {
                    var errorMessasge = JsonConvert.SerializeObject(new MJsonMessageError(message));
                    clientHolder.SendMessage(errorMessasge);
                    clientHolder.Disconnect();
                }
                return;
            }

            semaphore.WaitAsync();
            if (mjsonObject.IsJOIN())
            {
                var roomName = mjsonObject.room;
                clientRoomNameDictionary.Add(clientHolder, roomName);
                var alreadyWaitingSameRoom = roomNameWaitingNumDictionary.ContainsKey(roomName);
                var newWaitingNum = alreadyWaitingSameRoom ?
                    roomNameWaitingNumDictionary[roomName] + 1: 1;

                roomNameWaitingNumDictionary.Add(roomName, newWaitingNum);
            }

            foreach( var room in GetStartableRoomList())
            {
                StartGameRoom(room);
            }

            semaphore.Release();

        }


        List<string> GetStartableRoomList()
        {
            List<string> canStartRoom = new List<string>();
            foreach(var map in roomNameWaitingNumDictionary)
            {
                if( map.Value == Constants.PLAYER_NUM)
                {
                    canStartRoom.Add(map.Key);
                }
            }
            return canStartRoom;
        }

        void StartGameRoom(string startRoomName)
        {
            List<ClientHolderInterface> playerList = new List<ClientHolderInterface>();
            foreach(var map in clientRoomNameDictionary)
            {
                if( map.Value == startRoomName)
                {
                    playerList.Add(map.Key);
                }
            }

            //remove client from waitingroom
            playerList.ForEach(e => clientRoomNameDictionary.Remove(e));
            roomNameWaitingNumDictionary.Remove(startRoomName);
            playerList.ForEach(e => e.GetMessageFromClientHandler -= RouteMessage);

            //register client togameroom
            StartRoomHandler(playerList);
        }


    }
}
