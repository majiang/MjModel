using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjModelProject
{
    public class Client
    {
        public ClientRouter clientRouter;
        public ClientMjModel clientMjModel;
        public ClientController clientController;
        public int myPositionId;
        public List<string> playerNames;//入室した順番でプレイヤー名が入っている

        public bool IsKyokuEnd;//for Debug


        public Client(ClientRouter cr)
        {
            clientRouter = cr;
            clientMjModel = new ClientMjModel();
            clientController = new ClientController();

            clientController.clientMjModel = clientMjModel;
            clientController.clientRouter = cr;
        }

        public void kickGame(string name, string room)
        {
            var msgobj = new MJsonMessageJoin(name, room);
            
            clientRouter.SendJoin(msgobj);
            
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
            clientController.Tsumo(actor, pai);
            if (actor == myPositionId)
            {

                //thinkDahai();
                var tsumogiri = true;
                clientRouter.SendDahai(new MJsonMessageDahai(actor, pai, tsumogiri));
            }
            else
            {
                clientRouter.SendNone();
            }

        }

        internal void OnDahai(int actor, string pai, bool tsumogiri)
        {
            clientController.Dahai(actor, pai, tsumogiri);
            //thinkNaki();
            
            //CHI
            if (clientMjModel.CanChi(actor, myPositionId, pai))
            {
                //var thinked = thinkNaki();
                var doaction = true;
                if (doaction)
                {
                    var msgobj = clientMjModel.GetChiMessage(myPositionId, actor, pai);
                    clientRouter.SendChi(msgobj);
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
                var doaction = true;
                if (doaction)
                {
                    var msgobj = clientMjModel.GetPonMessage(myPositionId, actor, pai);
                    clientRouter.SendPon(msgobj);
                    return;
                }
                else
                {
                    clientRouter.SendNone();
                    return;
                }
            }

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
                clientRouter.SendDahai(new MJsonMessageDahai(actor, lastPai.PaiString, tsumogiri));
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
                clientRouter.SendDahai(new MJsonMessageDahai(actor, lastPai.PaiString, tsumogiri));
            }
            else
            {
                clientRouter.SendNone();
            }
        }

        internal void OnKakan(int actor, int target, string pai, List<string> consumed)
        {
            throw new NotImplementedException();
        }

        internal void OnAnkan(int actor, int target, string pai, List<string> consumed)
        {
            throw new NotImplementedException();
        }

        internal void OnDaiminkan(int actor, int target, string pai, List<string> consumed)
        {
            throw new NotImplementedException();
        }

        internal void OnDora(string pai)
        {
            throw new NotImplementedException();
        }

        internal void OnReach(int actor)
        {
            throw new NotImplementedException();
        }

        internal void OnReachAccepted(int actor, List<int> deltas, List<int> scores)
        {
            throw new NotImplementedException();
        }

        internal void OnHora(int actor, int target, string pai, List<string> uradoraMarkers, List<string> horaTehais, Dictionary<string, int> yakus, int fu, int fan, int horaPoints, List<int> deltas, List<int> scores)
        {
            throw new NotImplementedException();
        }

        internal void OnRyukyoku(string reason, List<List<string>> tehais)
        {
            clientRouter.SendNone();
        }

        internal void OnEndKyoku()
        {
            IsKyokuEnd = true;
            clientRouter.SendNone();
        }


    }
}
