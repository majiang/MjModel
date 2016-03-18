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
        AIBase ai;
        object MsgBufferForReach;

        public bool IsEndGame;


        public ClientFacade()
        {
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
            clientRouter.OnKakan += OnKakan;
            clientRouter.OnOpenDora += OnDora;


            // messageanalyzer
            // player
            // model

        }


        public void StartClient(string playerName, string roomName = "debugRoom")
        {
            try
            {
                clientIO.MakeConnection();
                var startMessage = new MJsonMessageJoin(playerName, "roomName");
                clientIO.SendMJsonObject(startMessage);
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
        public void MessageFromServerHandler(string message)
        {
            //if (msssageValidater.Validate(message) == false)
            //{
            //    clientLogger.Log("invalid message : " + message);
            //    return;
            //}
            clientLogger.Log(message);
            clientRouter.RouteGetMessage(message);
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
        internal void OnTsumo(int actor, string pai)
        {   
            if (actor == myPositionId)
            {
                clientMjModel.Tsumo(myPositionId, pai);

                if (clientMjModel.CanTsumoHora(pai))
                {
                    clientIO.SendMJsonObject(new MJsonMessageHora(myPositionId, myPositionId, pai));
                    return;
                }


                if (clientMjModel.CanReach(myPositionId))
                {
                    MsgBufferForReach = ai.thinkDahai(myPositionId, pai, clientMjModel.tehais, clientMjModel.kawas, clientMjModel.field);
                    clientIO.SendMJsonObject(new MJsonMessageReach(myPositionId));
                    return;
                }


                var msgobj = ai.thinkDahai(myPositionId ,pai, clientMjModel.tehais, clientMjModel.kawas, clientMjModel.field);
                clientIO.SendMJsonObject(msgobj);
            }
            else
            {
                clientIO.SendNone();
            }

        }

        /// <summary>
        /// callback for Dahai
        /// </summary>
        /// <param name="actor">dahai player</param>
        /// <param name="pai">drop pai</param>
        /// <param name="tsumogiri"></param>
        internal void OnDahai(int actor, string pai, bool tsumogiri)
        {
            if (clientMjModel.CanRonHora(actor, pai))
                
            {
                clientIO.SendMJsonObject(new MJsonMessageHora(myPositionId, actor, pai));
                return;
            }

            clientMjModel.Dahai(actor, pai, tsumogiri);

            //ターチャの打牌をうけてからの行動
            //thinkNaki();

            /*
            //CHI
            if (clientMjModel.CanChi(actor, myPositionId, pai))
            {
                //var thinked = thinkNaki();
                var doAction = true;
                if (doAction)
                {
                    var msgobj = clientMjModel.GetChiMessage();
                    clientRouter.SendMJsonMessage(msgobj);
                    return;
                }
                else
                {
                    clientRouter.SendNone();
                    return;
                }
            }

            //PON
            if (clientMjModel.CanPon(actor, myPositionId, pai))
            {
                //var thinked = thinkNaki();
                var doAction = true;
                if (doAction)
                {
                    var msgobj = clientMjModel.GetPonMessage();
                    clientRouter.SendMJsonMessage(msgobj);
                    return;
                }
                else
                {
                    clientRouter.SendNone();
                    return;
                }
            }
            */

            // do nothing
            clientIO.SendNone();
        }

        internal void OnPon(int actor, int target, string pai, List<string> consumed)
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

        internal void OnChi(int actor, int target, string pai, List<string> consumed)
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

        internal void OnKakan(int actor, string pai, List<string> consumed)
        {
            clientIO.SendNone();
        }

        internal void OnAnkan(int actor, string pai, List<string> consumed)
        {
            clientIO.SendNone();
        }

        internal void OnDaiminkan(int actor, int target, string pai, List<string> consumed)
        {
            clientIO.SendNone();
        }

        internal void OnDora(string pai)
        {
            clientIO.SendNone();
        }

        internal void OnReach(int actor)
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

        internal void OnReachAccepted(int actor, List<int> deltas, List<int> scores)
        {
            clientMjModel.ReachAccept(actor, scores);
            clientIO.SendNone();
        }

        internal void OnHora(int actor, int target, string pai, List<string> uradoraMarkers, List<string> horaTehais, List<List<object>> yakus, int fu, int fan, int horaPoints, List<int> deltas, List<int> scores)
        {
            clientIO.SendNone();
        }

        internal void OnRyukyoku(string reason, List<List<string>> tehais)
        {
            clientIO.SendNone();
        }

        internal void OnEndKyoku()
        {

            clientIO.SendNone();
        }
        internal void OnEndGame()
        {
            IsEndGame = true;
            clientIO.SendNone();
        }


    }
}
