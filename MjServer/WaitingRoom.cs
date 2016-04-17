using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using Newtonsoft.Json;
using MjNetworkProtocolLibrary;
using MjModelLibrary;

namespace MjServer
{
    
    class WaitingRoom
    {
        public event StartGameRoom StartRoomHandler;
        Dictionary<ClientHolderInterface, string> clientHolderRoomNameDictionary;
        Dictionary<ClientHolderInterface, string> clientHolderClientNameDictionary;
        Dictionary<string, int> roomNameWaitingNumDictionary;

        public WaitingRoom() { }
        
        public async void StartWaiting()
        {

            IPAddress ipAdd = IPAddress.Parse(Properties.Settings.Default.ipAddress);//LAN
            int port = Properties.Settings.Default.port;
            TcpListener server = new TcpListener(ipAdd, port);

            server.Start();
            Console.WriteLine(
                "Start Listen({0}:{1})" ,
                ( (IPEndPoint)server.LocalEndpoint).Address ,
                ( (IPEndPoint)server.LocalEndpoint).Port
                );
            
            clientHolderRoomNameDictionary = new Dictionary<ClientHolderInterface, string>();
            clientHolderClientNameDictionary = new Dictionary<ClientHolderInterface, string>();
            roomNameWaitingNumDictionary = new Dictionary<string, int>();

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                ClientUsingTcpHolder clientHolder = new ClientUsingTcpHolder(client);
                clientHolder.GetMessageFromClientHandler += RouteMessage;
                Task.Run(() => clientHolder.StartWaiting());
            }
        }


        private System.Threading.SemaphoreSlim semaphore = new System.Threading.SemaphoreSlim(1, 1);
        async void RouteMessage(string message, ClientHolderInterface clientHolder)
        {

            if (string.IsNullOrEmpty(message))
            {
                return;
            }

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

            await semaphore.WaitAsync();
            if (mjsonObject.IsJOIN())
            {
                var roomName = mjsonObject.room;
                clientHolderRoomNameDictionary.Add(clientHolder, roomName);
                clientHolderClientNameDictionary.Add(clientHolder, mjsonObject.name);
                var alreadyWaitingSameRoom = roomNameWaitingNumDictionary.ContainsKey(roomName);
                var newWaitingNum = alreadyWaitingSameRoom ?
                    roomNameWaitingNumDictionary[roomName] + 1: 1;
                roomNameWaitingNumDictionary[roomName] = newWaitingNum;
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
            Dictionary<ClientHolderInterface,string> playerList = new Dictionary<ClientHolderInterface,string>();
            foreach(var map in clientHolderRoomNameDictionary)
            {
                if( map.Value == startRoomName)
                {
                    playerList[map.Key] = clientHolderClientNameDictionary[map.Key];
                }
            }

            //remove client from waitingroom
            foreach(var player in playerList)
            {
                clientHolderRoomNameDictionary.Remove(player.Key);
                clientHolderClientNameDictionary.Remove(player.Key);
                player.Key.GetMessageFromClientHandler -= RouteMessage;
            }
            roomNameWaitingNumDictionary.Remove(startRoomName);
 

            //register client to Gameroom
            StartRoomHandler(playerList);
        }


    }
}
