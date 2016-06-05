using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using Newtonsoft.Json;
using MjNetworkProtocolLibrary;
using MjModelLibrary;
using System.Threading;

namespace MjServer
{
    
    class WaitingRoom
    {
        public event StartGameRoom StartRoomHandler;
        Dictionary<ClientHolderInterface, string> clientHolderRoomNameMap;
        Dictionary<ClientHolderInterface, string> clientHolderClientNameMap;
        Dictionary<string, int> roomNameWaitingNumMap;
        BotClientManager botClientManager = new BotClientManager();

        public WaitingRoom() { }
        
        public async void StartWaiting()
        {

            IPAddress ipAdd = IPAddress.Parse(Properties.Settings.Default.ipAddress);//LAN
            int port = Properties.Settings.Default.port;
            TcpListener server = new TcpListener(ipAdd, port);

            server.Start();
            Debug.WriteLine(
                "Start Listen({0}:{1})" ,
                ( (IPEndPoint)server.LocalEndpoint).Address ,
                ( (IPEndPoint)server.LocalEndpoint).Port
                );
            
            clientHolderRoomNameMap = new Dictionary<ClientHolderInterface, string>();
            clientHolderClientNameMap = new Dictionary<ClientHolderInterface, string>();
            roomNameWaitingNumMap = new Dictionary<string, int>();


            // bot client genarator 
            Task.Run(() => TimerTrigger());

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                ClientHolderUsingTcpHolder clientHolder = new ClientHolderUsingTcpHolder(client);
                clientHolder.GetMessageFromClientHandler += RouteMessage;
                Task.Run(() => clientHolder.StartWaiting());
            }
        }

        void TimerTrigger()
        {
            //var TrigerBitween = 5 * 1000;
            var TrigerBitween = 10;
            while (true)
            {
                //Debug.WriteLine("Triggerd!");
                AllRoomGameStart();
                Thread.Sleep(TrigerBitween);
            }
        }

        void AllRoomGameStart()
        {
            semaphore.WaitAsync();
            var needClients = GetNeedMemberNum();

            foreach (var map in needClients)
            {
                Debug.WriteLine("room = {0}, num = {1}", map.Key, map.Value);
                botClientManager.GenerateClient(map.Key, map.Value);
            }
            semaphore.Release();
        }

        private SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
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
                    Debug.WriteLine("Json Deserialize Error! "+e.Message);
                    Debug.Fail("Json Deserialize Error! "+e.Message);
                    var errorMessasge = JsonConvert.SerializeObject(new MJsonMessageError(message));
                    clientHolder.SendMessageToClient(errorMessasge);
                    clientHolder.Disconnect();
                }
                return;
            }

            await semaphore.WaitAsync();
            if (mjsonObject.IsJOIN())
            {
                var roomName = mjsonObject.room;

                // check clientNum
                if ( roomNameWaitingNumMap.ContainsKey(roomName) && roomNameWaitingNumMap[roomName] >= Constants.PLAYER_NUM)
                {
                    Debug.WriteLine("target room is full!");
                    var errorMessasge = JsonConvert.SerializeObject(new MJsonMessageError(message));
                    clientHolder.SendMessageToClient(errorMessasge);
                    clientHolder.Disconnect();
                    return;
                }

                // add client
                clientHolderRoomNameMap.Add(clientHolder, roomName);
                clientHolderClientNameMap.Add(clientHolder, mjsonObject.name);
                var alreadyWaitingInTargetRoom = roomNameWaitingNumMap.ContainsKey(roomName);
                var newWaitingNum = alreadyWaitingInTargetRoom ?
                    roomNameWaitingNumMap[roomName] + 1: 1;
                roomNameWaitingNumMap[roomName] = newWaitingNum;
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
            foreach(var map in roomNameWaitingNumMap)
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
            foreach(var map in clientHolderRoomNameMap)
            {
                if( map.Value == startRoomName)
                {
                    playerList[map.Key] = clientHolderClientNameMap[map.Key];
                }
            }

            // remove client from waitingroom
            foreach(var player in playerList)
            {
                clientHolderRoomNameMap.Remove(player.Key);
                clientHolderClientNameMap.Remove(player.Key);
                player.Key.GetMessageFromClientHandler -= RouteMessage;
            }
            roomNameWaitingNumMap.Remove(startRoomName);
 

            // register client to Gameroom
            StartRoomHandler(playerList);
        }


        Dictionary<string,int> GetNeedMemberNum()
        {
            var roomNameNeedMemberNumMap = new Dictionary<string, int>();

            foreach(var map in roomNameWaitingNumMap)
            {
                roomNameNeedMemberNumMap[map.Key] = Constants.PLAYER_NUM - map.Value;
            }

            return roomNameNeedMemberNumMap;
        }
    }
}
