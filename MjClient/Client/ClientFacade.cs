using System;
using System.Collections.Generic;
using System.Linq;
using MjNetworkProtocolLibrary;
using MjClient.AI;
using MjClient.IO;
using MjClient.Logger;


namespace MjClient
{
    public class ClientFacade
    {
        ClientRouter clientRouter;
        ClientMjModel clientMjModel;
        //MessageValidater
        ClientIO clientIO;
        ClientLogger clientLogger = ClientLogger.GetInstance();



        int myPositionId;
        List<string> playerNames;
        AIInterface ai;


        public bool IsEndGame;
        string playerName;
        string roomName;

        public ClientFacade(string playerName, string roomName = "default")
        {
            this.playerName = playerName;
            this.roomName = roomName;

            // Initialize follows
            // io
            clientIO = new ClientIO();
            clientIO.OnGetMessage += MessageFromServerHandler;

            // logger
            // router
            clientRouter = new ClientRouter();

            clientRouter.OnAgari += OnHora;
            clientRouter.OnAnkan += OnAnkan;
            clientRouter.OnChi += OnChi;
            clientRouter.OnDaiminkan += OnDaiminkan;
            clientRouter.OnDropPai += OnDahai;
            clientRouter.OnEndGame += OnEndGame;
            clientRouter.OnEndKyoku += OnEndKyoku;
            clientRouter.OnGetPai += OnTsumo;
            clientRouter.OnHello += OnHello;
            clientRouter.OnKakan += OnKakan;
            clientRouter.OnOpenDora += OnDora;
            clientRouter.OnPon += OnPon;
            clientRouter.OnReach += OnReach;
            clientRouter.OnReachAccepted += OnReachAccepted;
            clientRouter.OnRyukyoku += OnRyukyoku;
            clientRouter.OnStartGame += OnStartGame;
            clientRouter.OnStartKyoku += OnStartKyoku;
            


            //Mjmodel
            clientMjModel = new ClientMjModel();


            //ai
            ai = new MinShantenAI();
            ai.SendPon += clientIO.SendMJsonObject;
            ai.SendChi += clientIO.SendMJsonObject;
            ai.SendDaiminkan += clientIO.SendMJsonObject;
            ai.SendNone += clientIO.SendMJsonObject;
            ai.SendHora += clientIO.SendMJsonObject;
            ai.SendDahai += clientIO.SendMJsonObject;
            ai.SendAnkan += clientIO.SendMJsonObject;
            ai.SendKakan += clientIO.SendMJsonObject;
            ai.SendReach += clientIO.SendMJsonObject;
        }


        public void StartClient()
        {
            try
            {
                clientIO.MakeConnection();
            }
            catch (Exception e)
            {
                //clientLogger.Log(e.Message);
            }
        }



        /// <summary>
        /// callback for recieve message from server.
        /// </summary>
        /// <param name="message">message</param>
        void MessageFromServerHandler(string message)
        {
            // don't verify message because clients can trust messages from server 
            clientLogger.Log(message);
            clientRouter.RouteGetMessage(message);
        }

        /// <summary>
        /// callback for Hello message which is sent when client connects server.
        /// clients must send none(no action) message in this function.
        /// </summary>
        void OnHello()
        {
            var startMessage = new MJsonMessageJoin(playerName, roomName);
            clientIO.SendMJsonObject(startMessage);
        }

        /// <summary>
        /// callback for StartGame message.
        /// clients must send none(no action) message in this function.
        /// </summary>
        /// <param name="id">client's id (0~3)</param>
        /// <param name="names">all player names</param>
        void OnStartGame(int id, List<string> names)
        {
            myPositionId = id;
            playerNames = names;
            clientMjModel.StartGame(id);
            clientIO.SendMJsonObject(new MJsonMessageNone());
        }

        /// <summary>
        /// callback for StartKyoku message.
        /// clients must send none(no action) message in this function.
        /// </summary>
        /// <param name="bakaze">bakaze</param>
        /// <param name="kyoku">kyoku id (1-4)</param>
        /// <param name="honba">honba</param>
        /// <param name="kyotaku">kyotaku</param>
        /// <param name="oya">oya player id</param>
        /// <param name="doraMarker">doraMarker pai</param>
        /// <param name="tehais">all player's tehais</param>
        void OnStartKyoku(string bakaze, int kyoku, int honba, int kyotaku, int oya, string doraMarker, List<List<string>> tehais)
        {
            clientMjModel.StartKyoku(bakaze, kyoku, honba, kyotaku, oya, doraMarker, tehais);
            clientIO.SendMJsonObject(new MJsonMessageNone());
        }

        
        /// <summary>
        /// callback for Tsumo message
        /// clients must send dahai, ankan, kakan, reach or none(no action) message in this function.
        /// </summary>
        /// <param name="actor">tsumo player</param>
        /// <param name="pai">tumo pai</param>
        void OnTsumo(int actor, string pai)
        {
            clientMjModel.Tsumo(actor, pai);

            if (actor == myPositionId)
            {
                
                ai.ThinkOnMyTsumo(myPositionId, pai, clientMjModel.tehais, clientMjModel.kawas, clientMjModel.field, clientMjModel.infoForResults, clientMjModel.yama);

            }
            else
            {
                clientIO.SendMJsonObject(new MJsonMessageNone());
            }


        }

        /// <summary>
        /// callback for Dahai message.
        /// clients must send hora, pon, chi, daiminkan or none(no action) message in this function.
        /// </summary>
        /// <param name="actor">dahai player</param>
        /// <param name="pai">drop pai</param>
        /// <param name="tsumogiri">true means discard pai and tumo pai is same</param>
        void OnDahai(int actor, string pai, bool tsumogiri)
        {

            clientMjModel.Dahai(actor, pai, tsumogiri);

            // ai think action 
            ai.ThinkOnOtherPlayerDoroped(myPositionId,actor,pai,clientMjModel.tehais,clientMjModel.kawas, clientMjModel.field, clientMjModel.infoForResults, clientMjModel.yama);

        }

        /// <summary>
        ///  callback for Pon message.
        ///  clients must send dahai or none(no action) message in this function.
        /// </summary>
        /// <param name="actor">pon player</param>
        /// <param name="target">poned player</param>
        /// <param name="pai">pon get pai</param>
        /// <param name="consumed">pon player show pais</param>
        void OnPon(int actor, int target, string pai, List<string> consumed)
        {
            clientMjModel.Pon(actor, target, pai, consumed);
            if (actor == myPositionId)
            {
                ai.ThinkOnFuroDahai(myPositionId, pai, clientMjModel.tehais, clientMjModel.kawas, clientMjModel.field, clientMjModel.infoForResults, clientMjModel.yama);
            }
            else
            {
                clientIO.SendMJsonObject(new MJsonMessageNone());
            }
        }

        /// <summary>
        /// callback for Chi message.
        /// clients must send dahai or none(no action) message in this function.
        /// </summary>
        /// <param name="actor">chi player</param>
        /// <param name="target">chied player</param>
        /// <param name="pai">chi get pai</param>
        /// <param name="consumed">chi player show pais</param>
        void OnChi(int actor, int target, string pai, List<string> consumed)
        {
            clientMjModel.Chi(actor, target, pai, consumed);
            if (actor == myPositionId)
            {
                ai.ThinkOnFuroDahai(myPositionId, pai, clientMjModel.tehais, clientMjModel.kawas, clientMjModel.field, clientMjModel.infoForResults, clientMjModel.yama);
            }
            else
            {
                clientIO.SendMJsonObject(new MJsonMessageNone());
            }
        }


        void OnKakan(int actor, string pai, List<string> consumed)
        {
            clientMjModel.Kakan(actor, pai, consumed);
            clientIO.SendMJsonObject(new MJsonMessageNone());
        }

        void OnAnkan(int actor, List<string> consumed)
        {
            clientMjModel.Ankan(actor, consumed);
            clientIO.SendMJsonObject(new MJsonMessageNone());
        }

        void OnDaiminkan(int actor, int target, string pai, List<string> consumed)
        {
            clientMjModel.Daiminkan(actor, target, pai, consumed);
            clientIO.SendMJsonObject(new MJsonMessageNone());
        }

        void OnDora(string doraMarker)
        {
            clientMjModel.Dora(doraMarker);
            clientIO.SendMJsonObject(new MJsonMessageNone());
        }



        void OnReach(int actor)
        {
            if (actor == myPositionId)
            {
                clientIO.SendMJsonObject(ai.GetMessageBufferForRiachDahai());
            }
            else
            {
                clientIO.SendMJsonObject(new MJsonMessageNone());
            }
        }

        void OnReachAccepted(int actor, List<int> deltas, List<int> scores)
        {
            clientMjModel.ReachAccept(actor,deltas, scores);
            clientIO.SendMJsonObject(new MJsonMessageNone());
        }

        void OnHora(int actor, int target, string pai, List<string> uradoraMarkers, List<string> horaTehais, List<List<object>> yakus, int fu, int fan, int horaPoints, List<int> deltas, List<int> scores)
        {
            clientMjModel.Hora(actor, target, pai, uradoraMarkers, horaTehais, yakus, fu, fan, horaPoints, deltas, scores);
            clientIO.SendMJsonObject(new MJsonMessageNone());
        }

        void OnRyukyoku(string reason, List<List<string>> tehais, List<bool> tenpais, List<int> deltas, List<int> scores)
        {
            clientIO.SendMJsonObject(new MJsonMessageNone());
        }

        void OnEndKyoku()
        {
            clientIO.SendMJsonObject(new MJsonMessageNone());
        }

        void OnEndGame()
        {
            IsEndGame = true;
        }

        public void SetLoggable(bool flg)
        {
            clientLogger.EnableLog = flg;
        }
    }
}
