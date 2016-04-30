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
    delegate void StartGameRoom(Dictionary<ClientHolderInterface, string> client);


    // server action is decided by clients action
    delegate bool StartKyokuHandler();
    delegate bool TsumoHandler();
    delegate bool DahaiHandler(int actor, string pai, bool tsumogiri);
    delegate bool ChiHandler(int actor, int target, string pai, List<string> consumed);
    delegate bool PonHandler(int actor, int target, string pai, List<string> consumed);
    delegate bool KakanHandler(int actor, string pai, List<string> consumed);
    delegate bool DaiminkanHandler(int actor, int target, string pai, List<string> consumed);
    delegate bool AnkanHandler(int actor, List<string> consumed);
    delegate bool OpenDoraHandler();
    delegate bool RinshanHandler();
    delegate bool ReachHandler(int actor);
    delegate bool ReachDahaiHandler(int actor, string pai, bool tsumogiri);
    delegate bool ReachAcceptHandler();
    delegate bool HoraHandler(int actor, int target, string pai);
    delegate bool RyukyokuHandler();
    delegate bool EndKyokuHandler();
    delegate bool EndGameHandler();

    delegate bool CheckEndGameHandler();


    class GameRoom
    {
        public event AfterErrorHandler OnAfterError;
        public event AfterGameEndHandler OnAfterGameEnd;

        List<ClientHolderInterface> clients = new List<ClientHolderInterface>();
        List<string> clientNames = new List<string>();
        // List<string> messageStack = new List<string>();
        GameModel gameModel = new GameModel();

        public GameRoom(Dictionary<ClientHolderInterface, string> inClients)
        {
            foreach(var client in inClients)
            {
                client.Key.GetMessageFromClientHandler += OnGetMessageFromClient;
                clients.Add(client.Key);
                clientNames.Add(client.Value);
            }


            AfterInitialiseState.OnStartKyoku += StartKyoku;

            AfterStartKyokuState.OnTsumo += Tsumo;

            AfterTsumoState.OnReach += Reach;
            AfterTsumoState.OnAnkan += Ankan;
            AfterTsumoState.OnKakan += Kakan;
            AfterTsumoState.OnHora += Hora;
            AfterTsumoState.OnDahai += Dahai;

            AfterDahiState.OnTsumo += Tsumo;
            AfterDahiState.OnPon += Pon;
            AfterDahiState.OnChi += Chi;
            AfterDahiState.OnHora += Hora;
            AfterDahiState.OnDaiminkan += Daiminkan;



            


        }



        public void StartGame()
        {

            gameModel.StartGame();
            //shuffle client


            //send start messages which have different playerID
            foreach (var client in clients.Select((e,index) => new { e, index}))
            {
                var gameStartMessage = MjsonObjectToString(new MJsonMessageStartGame(client.index, clientNames));
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
        GameContext gameContext = new GameContext();
        async void OnGetMessageFromClient(string message, ClientHolderInterface client)
        {
            try
            {
                await semaphore.WaitAsync();
                MJsonMessageAll messageObj = JsonConvert.DeserializeObject<MJsonMessageAll>(message);
                
                
                // validate message
                if ( gameContext.ValidateMessage(messageObj) == false)
                {
                    OnErrorDetected(message);
                    return;
                }
                
                // check all clients has sent message
                if ( gameContext.HasRecievedMessageFromAllClients() == false)
                {
                    return;
                }

                // if server gets messages from all clients, fire event and send message.
                var isSuccesseded = gameContext.ExecuteAction();

                if ( isSuccesseded == false)
                {
                    var errorMessages = gameContext.GetMessageList();
                    StringBuilder sb = new StringBuilder();
                    errorMessages.Select(e => sb.Append(MjsonObjectToString(errorMessages)));
                    OnErrorDetected( sb.ToString() );
                }
            }
            finally
            {
                semaphore.Release();
            }
        }


        void OnErrorDetected(string jsonEerrorMessage)
        {
            // log errorMessage

            // send error to client
            clients.ForEach(e => e.SendMessage(jsonEerrorMessage));

            // disconnect client
            clients.ForEach(e => e.Disconnect());
            clients.Clear();
            OnAfterError(this);
        }




        // mj status check functions called by GameState
        bool CanEndKyoku()
        {
            throw new NotImplementedException();
        }


        // mj action functions called by GameState
        bool StartKyoku()
        {
            // bool = mjmodel evalate
            //if false return false;

            // mjsonObj = mjmodel execution 
            // set action to context(changestate)
            //gameContext.ChangeState(mjsonObj);

            // send message
            //return ture
            throw new NotImplementedException();
            
        }
        bool Tsumo()
        {
            throw new NotImplementedException();
           // return gameModel.Tsumo();
        }

        bool Dahai(int actor, string pai, bool tsumogiri)
        {
            
            throw new NotImplementedException();
        }

        bool Pon(int actor, int target, string pai, List<string> consumed)
        {


            throw new NotImplementedException();
        }

        bool Chi(int actor, int target, string pai, List<string> consumed)
        {
            throw new NotImplementedException();
        }

        bool Kakan(int actor, string pai, List<string> consumed)
        {
            throw new NotImplementedException();
        }

        bool Ankan(int actor, List<string> consumed)
        {
            throw new NotImplementedException();
        }

        bool Daiminkan(int actor, int target, string pai, List<string> consumed)
        {
            throw new NotImplementedException();
        }

        bool Rinshan()
        {
            throw new NotImplementedException();
        }

        bool OpenDora()
        {
            throw new NotImplementedException();
        }

        bool Reach(int actor)
        {
            throw new NotImplementedException();
        }

        bool Hora(int actor, int target, string pai)
        {
            throw new NotImplementedException();
        }

        bool Ryukyoku()
        {
            throw new NotImplementedException();
        }

        bool ReachAccept()
        {
            //send reachaccept

            //not clear message stack
            //gameContext.ChangeState(mjsonObj);

            throw new NotImplementedException();
        }
        bool OnKyokuEnd()
        {
            throw new NotImplementedException();
        }

        bool OnGameEnd()
        {
            throw new NotImplementedException();
        }













        private string MjsonObjectToString(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public void SendMJsonObject(MJsonMessageJoin jsonmsg)
        {
            clients.ForEach(e => e.SendMessage(MjsonObjectToString(jsonmsg)));
        }
        public void SendMJsonObject(MJsonMessagePon jsonmsg)
        {
            clients.ForEach(e => e.SendMessage(MjsonObjectToString(jsonmsg)));
        }
        public void SendMJsonObject(MJsonMessageChi jsonmsg)
        {
            clients.ForEach(e => e.SendMessage(MjsonObjectToString(jsonmsg)));
        }
        public void SendMJsonObject(MJsonMessageDaiminkan jsonmsg)
        {
            clients.ForEach(e => e.SendMessage(MjsonObjectToString(jsonmsg)));
        }
        public void SendMJsonObject(MJsonMessageNone jsonmsg)
        {
            clients.ForEach(e => e.SendMessage(MjsonObjectToString(jsonmsg)));
        }

        public void SendMJsonObject(MJsonMessageHora jsonmsg)
        {
            clients.ForEach(e => e.SendMessage(MjsonObjectToString(jsonmsg)));
        }
        public void SendMJsonObject(MJsonMessageDahai jsonmsg)
        {
            clients.ForEach(e => e.SendMessage(MjsonObjectToString(jsonmsg)));
        }
        public void SendMJsonObject(MJsonMessageAnkan jsonmsg)
        {
            clients.ForEach(e => e.SendMessage(MjsonObjectToString(jsonmsg)));
        }
        public void SendMJsonObject(MJsonMessageKakan jsonmsg)
        {
            clients.ForEach(e => e.SendMessage(MjsonObjectToString(jsonmsg)));
        }
        public void SendMJsonObject(MJsonMessageReach jsonmsg)
        {
            clients.ForEach(e => e.SendMessage(MjsonObjectToString(jsonmsg)));
        }

    }

    


}
