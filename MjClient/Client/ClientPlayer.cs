using System;
using System.Collections.Generic;
using System.Linq;
using MjNetworkProtocol;


namespace MjClient
{
    public class ClientPlayer
    {
        ClientRouter clientRouter;
        ClientMjModel clientMjModel;
        ClientController clientController;
         

        int myPositionId;
        List<string> playerNames;//入室した順番でプレイヤー名が入っている
        AIBase ai;
        object MsgBufferForReach;

        public bool IsEndGame;


        public ClientPlayer(ClientRouter cr)
        {
            clientRouter = cr;
            clientMjModel = new ClientMjModel();
            clientController = new ClientController();

            clientController.clientMjModel = clientMjModel;
            clientController.clientRouter = cr;

            ai = new MinShantenAI();
        }

        public void kickGame(string name, string room)
        {
            var msgobj = new MJsonMessageJoin(name, room);
            
            clientRouter.SendMJsonMessage(msgobj);
            
        }


        public void OnStartGame(int id, List<string> names)
        {
            myPositionId = id;
            playerNames = names;
            clientController.StartGame(id, names);
            clientRouter.SendNone();
        }

        internal void OnStartKyoku(string bakaze, int kyoku, int honba, int kyotaku, int oya, string doraMarker, List<List<string>> tehais)
        {
            clientController.StartKyoku(bakaze, kyoku, honba, kyotaku, oya, doraMarker, tehais);
            clientRouter.SendNone();
        }



        internal void OnTsumo(int actor, string pai)
        {   
            if (actor == myPositionId)
            {
                clientController.Tsumo(myPositionId, pai);

                if (clientMjModel.CanTsumoHora(pai))
                {
                    clientRouter.SendMJsonMessage(new MJsonMessageHora(myPositionId, myPositionId, pai));
                    return;
                }


                if (clientMjModel.CanReach(myPositionId))
                {
                    MsgBufferForReach = ai.thinkDahai(myPositionId, pai, clientMjModel.tehais, clientMjModel.kawas, clientMjModel.field);
                    clientRouter.SendMJsonMessage(new MJsonMessageReach(myPositionId));
                    return;
                }


                var msgobj = ai.thinkDahai(myPositionId ,pai, clientMjModel.tehais, clientMjModel.kawas, clientMjModel.field);
                clientRouter.SendMJsonMessage(msgobj);
            }
            else
            {
                clientRouter.SendNone();
            }

        }

        internal void OnDahai(int actor, string pai, bool tsumogiri)
        {
            if (clientMjModel.CanRonHora(actor, pai))
                
            {
                clientRouter.SendMJsonMessage(new MJsonMessageHora(myPositionId, actor, pai));
                return;
            }
         
            clientController.Dahai(actor, pai, tsumogiri);

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
             clientRouter.SendNone();
        }

        internal void OnPon(int actor, int target, string pai, List<string> consumed)
        {
            clientController.Pon(actor, target, pai, consumed);
            if (actor == myPositionId)
            {
                var tsumogiri = false;
                var lastPai = clientMjModel.tehais[actor].tehai[clientMjModel.tehais[actor].tehai.Count - 1];
                //clientController.Dahai(actor,lastPai.paiString,tsumogiri);
                clientRouter.SendMJsonMessage(new MJsonMessageDahai(actor, lastPai.PaiString, tsumogiri));
            }
            else
            {
                clientRouter.SendNone();
            }
        }

        internal void OnChi(int actor, int target, string pai, List<string> consumed)
        {
            clientController.Chi(actor, target, pai, consumed);
            if (actor == myPositionId)
            {
                var tsumogiri = false;
                var lastPai = clientMjModel.tehais[actor].tehai[clientMjModel.tehais[actor].tehai.Count-1];
                //clientController.Dahai(actor,lastPai.paiString,tsumogiri);
                clientRouter.SendMJsonMessage(new MJsonMessageDahai(actor, lastPai.PaiString, tsumogiri));
            }
            else
            {
                clientRouter.SendNone();
            }
        }

        internal void OnKakan(int actor, int target, string pai, List<string> consumed)
        {
            clientRouter.SendNone();
        }

        internal void OnAnkan(int actor, int target, string pai, List<string> consumed)
        {
            clientRouter.SendNone();
        }

        internal void OnDaiminkan(int actor, int target, string pai, List<string> consumed)
        {
            clientRouter.SendNone();
        }

        internal void OnDora(string pai)
        {
            clientRouter.SendNone();
        }

        internal void OnReach(int actor)
        {

            if (actor == myPositionId)
            {
                clientRouter.SendMJsonMessage(MsgBufferForReach);
            }
            else
            {
                clientRouter.SendNone();
            }
        }

        internal void OnReachAccepted(int actor, List<int> deltas, List<int> scores)
        {
            clientMjModel.ReachAccept(actor, scores);
            clientRouter.SendNone();
        }

        internal void OnHora(int actor, int target, string pai, List<string> uradoraMarkers, List<string> horaTehais, List<List<object>> yakus, int fu, int fan, int horaPoints, List<int> deltas, List<int> scores)
        {
            clientRouter.SendNone();
        }

        internal void OnRyukyoku(string reason, List<List<string>> tehais)
        {
            clientRouter.SendNone();
        }

        internal void OnEndKyoku()
        {

            clientRouter.SendNone();
        }
        internal void OnEndGame()
        {
            IsEndGame = true;
            clientRouter.SendNone();
        }


    }
}
