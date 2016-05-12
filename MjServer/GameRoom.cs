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



    public delegate bool StartKyokuHandler();
    public delegate bool TsumoHandler();
    public delegate bool DahaiHandler(int actor, string pai, bool tsumogiri);
    public delegate bool ChiHandler(int actor, int target, string pai, List<string> consumed);
    public delegate bool PonHandler(int actor, int target, string pai, List<string> consumed);
    public delegate bool KakanHandler(int actor, string pai, List<string> consumed);
    public delegate bool DaiminkanHandler(int actor, int target, string pai, List<string> consumed);
    public delegate bool AnkanHandler(int actor, List<string> consumed);
    public delegate bool OpenDoraHandler();
    public delegate bool RinshanHandler();
    public delegate bool ReachHandler(int actor);
    public delegate bool ReachDahaiHandler(int actor, string pai, bool tsumogiri);
    public delegate bool ReachAcceptHandler();
    public delegate bool HoraHandler(int actor, int target, string pai);
    public delegate bool RyukyokuHandler();
    public delegate bool EndKyokuHandler();
    public delegate bool EndGameHandler();
    
    public delegate bool CheckIsEndGameHandler();
    public delegate bool CheckIsEndKyokuHandler();


    public class GameRoom
    {
        public event AfterErrorHandler OnAfterError;
        public event AfterGameEndHandler OnAfterGameEnd;

        public List<ClientHolderInterface> clients = new List<ClientHolderInterface>();
        List<string> clientNames = new List<string>();
        public GameModel gameModel = new GameModel();
        SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        public GameContext gameContext = new GameContext();

        public GameRoom(Dictionary<ClientHolderInterface, string> inputClients)
        {
            foreach(var client in inputClients)
            {
                client.Key.GetMessageFromClientHandler += OnGetMessageFromClient;
                clients.Add(client.Key);
                clientNames.Add(client.Value);
            }

            gameContext.StartKyoku += StartKyoku;
            gameContext.Tsumo += Tsumo;
            gameContext.Dahai += Dahai;
            gameContext.Chi += Chi;
            gameContext.Pon += Pon;
            gameContext.Kakan += Kakan;
            gameContext.Daiminkan += Daiminkan;
            gameContext.Ankan += Ankan;
            gameContext.OpenDora += OpenDora;
            gameContext.Rinshan += Rinshan;
            gameContext.Reach += Reach;
            gameContext.ReachDahai += ReachDahai;
            gameContext.ReachAccept += ReachAccept;
            gameContext.Hora += Hora;
            gameContext.Ryukyoku += Ryukyoku;
            gameContext.Endkyoku += EndKyoku;
            gameContext.EndGame += EndGame;
            gameContext.CheckIsRyuKyoku += CheckIsRyukyoku;
            gameContext.CheckIsEndGame += CheckIsEndGame;
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
        public void ReplaceStartKyokuForTest(MJsonMessageAll msg)
        {
            var haipais = msg.tehais;
            gameModel.tehais = haipais.Select(e => new Tehai(e)).ToList();
        }
        public void ReplaceYamaForTest(List<string> tsumopais,List<string> rinshanpais)
        {
            gameModel.yama.ReplaceTsumoForTest(tsumopais);
            gameModel.yama.ReplaceRinshanForTest(rinshanpais);
        }


        public void SendSameMessageToClients(string jsonMessage)
        {
            clients.ForEach(e => e.SendMessageToClient(jsonMessage));
        }

        public void SendMessageToOneClient(string message, int targetClientId)
        {
            Debug.Assert(0 <= targetClientId && targetClientId < clients.Count);
            clients[targetClientId].SendMessageToClient(message);
        }




        public async void OnGetMessageFromClient(string message, ClientHolderInterface client)
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

                gameContext.RegisterMessage(messageObj);
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
                    Debug.WriteLine(sb.ToString());
                    OnErrorDetected( sb.ToString() );
                }

            }
            finally
            {
                semaphore.Release();
            }
        }

        void CloseRoom()
        {
            clients.ForEach(e => e.Disconnect());
            clients.Clear();
        }

        void OnErrorDetected(string jsonEerrorMessage)
        {
            // log errorMessage
            Debug.WriteLine(jsonEerrorMessage);
            // send error to client
            clients.ForEach(e => e.SendMessageToClient(jsonEerrorMessage));

            // disconnect client
            clients.ForEach(e => e.Disconnect());
            clients.Clear();
            //OnAfterError(this);
        }




        // mj status check functions called by GameState
        bool CheckIsRyukyoku()
        {
            return gameModel.CanRyukyoku();
        }
        bool CheckIsEndGame()
        {
            return gameModel.CanEndGame();
        }



        // mj action functions called by GameState
        bool StartKyoku()
        {
            var msgobj = gameModel.StartKyoku();
            SendMJsonObject(msgobj);
            gameContext.ChangeState(msgobj);
            return true;
        }
        bool Tsumo()
        {
            if( gameModel.CanTsumo() )
            {
                var msgobj = gameModel.Tsumo();
                SendMJsonObject(msgobj);
                gameContext.ChangeState(msgobj);
                return true;
            }
            return false;
        }

        bool Dahai(int actor, string pai, bool tsumogiri)
        {
            if (gameModel.CanDahai(actor,pai, tsumogiri))
            {
                var msgobj = gameModel.Dahai(actor, pai, tsumogiri);
                SendMJsonObject(msgobj);
                var isReachDahai = false;
                gameContext.ChangeState(msgobj, isReachDahai);
                return true;
            }
            return false;
        }

        bool Pon(int actor, int target, string pai, List<string> consumed)
        {
            if (gameModel.CanPon( actor, target, pai, consumed))
            {
                var msgobj = gameModel.Pon(actor, target, pai, consumed);
                SendMJsonObject(msgobj);
                gameContext.ChangeState(msgobj);
                return true;
            }
            return false;
        }

        bool Chi(int actor, int target, string pai, List<string> consumed)
        {
            if (gameModel.CanChi(actor, target, pai, consumed))
            {
                var msgobj = gameModel.Chi(actor, target, pai, consumed);
                SendMJsonObject(msgobj);
                gameContext.ChangeState(msgobj);
                return true;
            }
            return false;
        }

        bool Kakan(int actor, string pai, List<string> consumed)
        {
            if (gameModel.CanKakan(actor, pai, consumed))
            {
                var msgobj = gameModel.Kakan(actor, pai, consumed);
                SendMJsonObject(msgobj);
                gameContext.ChangeState(msgobj);
                return true;
            }
            return false;
        }

        bool Ankan(int actor, List<string> consumed)
        {
            if (gameModel.CanAnkan(actor, consumed))
            {
                var msgobj = gameModel.Ankan(actor, consumed);
                SendMJsonObject(msgobj);
                gameContext.ChangeState(msgobj);
                return true;
            }
            return false;
        }

        bool Daiminkan(int actor, int target, string pai, List<string> consumed)
        {
            if (gameModel.CanDaiminkan(actor, target, pai, consumed))
            {
                var msgobj = gameModel.Daiminkan(actor, target, pai, consumed);
                SendMJsonObject(msgobj);
                gameContext.ChangeState(msgobj);
                return true;
            }
            return false;
        }

        bool Rinshan()
        {
            if (gameModel.CanRinshan())
            {
                var msgobj = gameModel.Rinshan();
                SendMJsonObject(msgobj);
                gameContext.ChangeState(msgobj);
                return true;
            }
            return false;
        }

        bool OpenDora()
        {
            if (gameModel.CanOpenDora())
            {
                var msgobj = gameModel.OpenDora();
                SendMJsonObject(msgobj);
                gameContext.ChangeState(msgobj);
                return true;
            }
            return false;
        }

        bool Reach(int actor)
        {
            if (gameModel.CanReach(actor))
            {
                var msgobj = gameModel.Reach(actor);
                SendMJsonObject(msgobj);
                gameContext.ChangeState(msgobj);
                return true;
            }
            return false;
        }

        bool ReachDahai(int actor, string pai, bool tsumogiri)
        {
            if (gameModel.CanReachDahai(actor, pai, tsumogiri))
            {
                var msgobj = gameModel.Dahai(actor, pai, tsumogiri);
                SendMJsonObject(msgobj);
                var isReachDahai = true;
                gameContext.ChangeState(msgobj, isReachDahai);
                return true;
            }
            return false;
        }

        bool ReachAccept()
        {
            if (gameModel.CanReachAccept())
            {
                var msgobj = gameModel.ReachAccept();
                SendMJsonObject(msgobj);
                gameContext.ChangeState(msgobj);
                return true;
            }
            return false;
        }
        bool Hora(int actor, int target, string pai)
        {
            if (gameModel.CanHora(actor, target, pai))
            {
                var msgobj = gameModel.Hora(actor, target, pai);
                SendMJsonObject(msgobj);
                gameContext.ChangeState(msgobj);
                return true;
            }
            return false;
        }

        bool Ryukyoku()
        {
            if (gameModel.CanRyukyoku())
            {
                var msgobj = gameModel.Ryukyoku();
                SendMJsonObject(msgobj);
                gameContext.ChangeState(msgobj);
                return true;
            }
            return false;
        }

        bool EndKyoku()
        {
            var msgobj = gameModel.EndKyoku();
            SendMJsonObject(msgobj);
            gameContext.ChangeState(msgobj);
            return true;
        }

        bool EndGame()
        {
            var msgobj = gameModel.EndGame();
            SendMJsonObject(msgobj);
            gameContext.ChangeState(msgobj);
            CloseRoom();
            return true;
        }



        // hide tsumopai and haipai in messages by follow functions
        public void SendMJsonObject(MJsonMessageStartKyoku jsonmsg)
        {
            var opentehais = jsonmsg.tehais;
            var hidetehais = new List<List<string>>() {
                Tehai.UNKNOWN_TEHAI_STRING ,
                Tehai.UNKNOWN_TEHAI_STRING ,
                Tehai.UNKNOWN_TEHAI_STRING ,
                Tehai.UNKNOWN_TEHAI_STRING
            };
            
            for(int i = 0; i < clients.Count; i++)
            {
                var sendMessage = jsonmsg;
                sendMessage.tehais = hidetehais;
                sendMessage.tehais[i] = opentehais[i];
                clients[i].SendMessageToClient(MjsonObjectToString(sendMessage));
            }
        }
        public void SendMJsonObject(MJsonMessageTsumo jsonmsg)
        {
            for (int i = 0; i < clients.Count; i++)
            {
                var sendMsssage = jsonmsg;
                if( i != jsonmsg.actor )
                {
                    sendMsssage.pai = Pai.UNKNOWN_PAI_STRING;
                }
                clients[i].SendMessageToClient(MjsonObjectToString(sendMsssage));
            }
        }
        public void SendMJsonObject(object jsonmsg)
        {
            clients.ForEach(e => e.SendMessageToClient(MjsonObjectToString(jsonmsg)));
        }

        string MjsonObjectToString(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

    }




}
