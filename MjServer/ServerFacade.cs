using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


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

            // timer start
            // start wait for client
            // register client to waitingList
            // make GameRoom in waitingroom by time pass or 4 player registerd in same room




        }



        //  -GameRoom has 4 tcpClient
        //  -MessageRouter
        //  -ServerMjModel
        //  -ServerLogger
        //
        //  send start
        //


        

         void OnStartGameRoom( List<ClientHolderInterface> clients )
        {
            gameRooms.Add(new GameRoom(clients));
        }



    }




}
