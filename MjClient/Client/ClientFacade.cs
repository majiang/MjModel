using System;
using System.Collections.Generic;
using System.Linq;
using MjNetworkProtocol;
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
        MinShantenAI ai;


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


            // messageanalyzer
            // player


            //ai
            ai = new MinShantenAI();
            
            
        }


        public void StartClient()
        {
            
            try
            {
                clientIO.MakeConnection();

            }
            catch (Exception e)
            {
                clientLogger.Log(e.Message);
            }
        }



        /// <summary>
        /// callback for recieve message from server.
        /// </summary>
        /// <param name="message">message</param>
        void MessageFromServerHandler(string message)
        {
            //if (msssageValidater.Validate(message) == false)
            //{
            //    clientLogger.Log("invalid message : " + message);
            //    return;
            //}
            clientLogger.Log(message);
            clientRouter.RouteGetMessage(message);
        }
        
        void OnHello()
        {
            var startMessage = new MJsonMessageJoin(playerName, roomName);
            clientIO.SendMJsonObject(startMessage);
        }

        /// <summary>
        /// callback for StartGame message.
        /// </summary>
        /// <param name="id">client's id (0~3)</param>
        /// <param name="names">all player names</param>
        void OnStartGame(int id, List<string> names)
        {
            myPositionId = id;
            playerNames = names;
            clientMjModel.StartGame(id);
            clientIO.SendNone();
        }

        /// <summary>
        /// callback for StartKyoku message.
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
            clientIO.SendNone();
        }

        
        /// <summary>
        /// callback for Tsumo message
        /// </summary>
        /// <param name="actor">tsumo player</param>
        /// <param name="pai">tumo pai</param>
        void OnTsumo(int actor, string pai)
        {   
            if(actor != myPositionId)
            {
                clientIO.SendNone();
                return;
            }

            clientMjModel.Tsumo(myPositionId, pai);
            ai.thinkOnTsumo(myPositionId, pai, clientMjModel.tehais, clientMjModel.kawas, clientMjModel.field);
        }

        /// <summary>
        /// callback for Dahai
        /// </summary>
        /// <param name="actor">dahai player</param>
        /// <param name="pai">drop pai</param>
        /// <param name="tsumogiri"></param>
        internal void OnDahai(int actor, string pai, bool tsumogiri)
        {

            clientMjModel.Dahai(actor, pai, tsumogiri);

            ai.thinkOnOtherPlayerDoroped(myPositionId,actor,pai,clientMjModel.tehais,clientMjModel.kawas, clientMjModel.field);
            
            // do nothing
            clientIO.SendNone();
        }

        void OnPon(int actor, int target, string pai, List<string> consumed)
        {
            clientMjModel.Pon(actor, target, pai, consumed);
            if (actor == myPositionId)
            {
                var tsumogiri = false;
                var lastPai = clientMjModel.tehais[actor].tehai[clientMjModel.tehais[actor].tehai.Count - 1];
                clientIO.SendMJsonObject(new MJsonMessageDahai(actor, lastPai.PaiString, tsumogiri));
            }
            else
            {
                clientIO.SendNone();
            }
        }

        void OnChi(int actor, int target, string pai, List<string> consumed)
        {
            clientMjModel.Chi(actor, target, pai, consumed);
            if (actor == myPositionId)
            {
                var tsumogiri = false;
                var lastPai = clientMjModel.tehais[actor].tehai[clientMjModel.tehais[actor].tehai.Count-1];
                clientIO.SendMJsonObject(new MJsonMessageDahai(actor, lastPai.PaiString, tsumogiri));
            }
            else
            {
                clientIO.SendNone();
            }
        }

        void OnKakan(int actor, string pai, List<string> consumed)
        {
            clientMjModel.Kakan(actor, pai, consumed);
            clientIO.SendNone();
        }

        void OnAnkan(int actor, List<string> consumed)
        {
            clientMjModel.Ankan(actor, consumed);
            clientIO.SendNone();
        }

        void OnDaiminkan(int actor, int target, string pai, List<string> consumed)
        {
            clientIO.SendNone();
        }

        void OnDora(string pai)
        {
            clientIO.SendNone();
        }

        void OnReach(int actor)
        {

            if (actor == myPositionId)
            {
                clientIO.SendMJsonObject(MsgBufferForReach);
            }
            else
            {
                clientIO.SendNone();
            }
        }

        void OnReachAccepted(int actor, List<int> deltas, List<int> scores)
        {
            clientMjModel.ReachAccept(actor,deltas, scores);
            clientIO.SendNone();
        }

        void OnHora(int actor, int target, string pai, List<string> uradoraMarkers, List<string> horaTehais, List<List<object>> yakus, int fu, int fan, int horaPoints, List<int> deltas, List<int> scores)
        {
            clientIO.SendNone();
        }

        void OnRyukyoku(string reason, List<List<string>> tehais, List<bool> tenpais, List<int> deltas, List<int> scores)
        {
            clientIO.SendNone();
        }

        void OnEndKyoku()
        {

            clientIO.SendNone();
        }

        void OnEndGame()
        {
            IsEndGame = true;
            clientIO.SendNone();
        }


    }
}
