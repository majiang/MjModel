using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MjNetworkProtocolLibrary;
using Newtonsoft.Json;

namespace MjServer
{

    public class ServerFacade
    {
        WaitingRoom waitingRoom;
        List<GameRoom> gameRooms;
        
        
        public ServerFacade()
        {
            waitingRoom = new WaitingRoom();
            waitingRoom.StartRoomHandler += OnStartGameRoom;
            gameRooms = new List<GameRoom>();
        }

        public void StartServer()
        {
            //-WairtingRoom
            // make waiting room
            waitingRoom.StartWaiting();

        }



        //  -GameRoom has 4 tcpClient
        //  -MessageRouter
        //  -ServerMjModel
        //  -ServerLogger
        //
        //  send start
        //


        void testStartGameRoom()
        {
        }

        void OnStartGameRoom(Dictionary<ClientHolderInterface, string> clients )
        {
            var gameRoom = new GameRoom(clients);
            gameRooms.Add(gameRoom);
            gameRoom.StartGame();

        }



    }




}
