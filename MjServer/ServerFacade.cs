using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjServer
{

    class ServerFacade
    {
        WaitingRoom waitingRoom;
        
        public ServerFacade()
        {
            waitingRoom = new WaitingRoom();
            waitingRoom.StartRoomHandler += OnStartGameRoom; 
            waitingRoom.StartWaiting();


        }

        public void StartServer()
        {
            //-WairtingRoom
            // make waiting room
            
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


        

        private void OnStartGameRoom(
            ClientHolderInterface client0,
            ClientHolderInterface client1,
            ClientHolderInterface client2,
            ClientHolderInterface client3
        )
        {

        }



    }




}
