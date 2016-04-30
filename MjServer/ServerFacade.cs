using System.Collections.Generic;


namespace MjServer
{
    delegate void AfterErrorHandler(GameRoom gm);
    delegate void AfterGameEndHandler(GameRoom gm);

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

        void OnStartGameRoom( Dictionary<ClientHolderInterface, string> clients )
        {
            var gameRoom = new GameRoom(clients);
            RegisterGameRoom(gameRoom);
            gameRoom.StartGame();
        }

        void AfterErrorDetected(GameRoom gameRoom)
        {
            TerminateGameRoom(gameRoom);
        }

        void AfterGameEnd(GameRoom gameRoom)
        {
            TerminateGameRoom(gameRoom);
        }

        
        void RegisterGameRoom(GameRoom gameRoom)
        {
            gameRoom.OnAfterError += AfterErrorDetected;
            gameRoom.OnAfterGameEnd += AfterGameEnd;
            gameRooms.Add(gameRoom);
        }

        void TerminateGameRoom(GameRoom gameRoom)
        {
            gameRoom.OnAfterError -= AfterErrorDetected;
            gameRoom.OnAfterGameEnd -= AfterGameEnd;
            gameRooms.Remove(gameRoom);
        }
    }




}
