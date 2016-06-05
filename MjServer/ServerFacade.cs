using System.Collections.Generic;


namespace MjServer
{
    public delegate void AfterErrorHandler(GameRoom gm);
    public delegate void AfterGameEndHandler(GameRoom gm);

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








        void OnStartGameRoom( Dictionary<ClientHolderInterface, string> clients )
        {
            var gameRoom = new GameRoom(clients);
            RegisterGameRoom(gameRoom);
            gameRoom.StartGame();
        }

        void AfterErrorDetected(GameRoom gameRoom)
        {
            RemoveGameRoom(gameRoom);
        }

        void AfterGameEnd(GameRoom gameRoom)
        {
            RemoveGameRoom(gameRoom);
        }

        
        void RegisterGameRoom(GameRoom gameRoom)
        {
            gameRoom.OnAfterError += AfterErrorDetected;
            gameRoom.OnAfterGameEnd += AfterGameEnd;
            gameRooms.Add(gameRoom);
        }

        void RemoveGameRoom(GameRoom gameRoom)
        {
            gameRoom.OnAfterError -= AfterErrorDetected;
            gameRoom.OnAfterGameEnd -= AfterGameEnd;
            gameRooms.Remove(gameRoom);
        }
    }




}
