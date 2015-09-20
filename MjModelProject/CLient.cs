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
        public int positionId;
        public List<string> playerNames;//入室した順番でプレイヤー名が入っている

        public Client(ClientRouter cr)
        {
            clientRouter = cr;
            clientMjModel = new ClientMjModel();
            clientController = new ClientController();

            clientController.clientMjModel = clientMjModel;
            clientController.clientRouter = cr;
        }

        public void GetStartGame(int id, List<string> names)
        {
            positionId = id;
            playerNames = names;
            clientController.StartGame(id, names);
            clientRouter.SendNone();
        }

        internal void GetStartKyoku(string bakaze, int kyoku, int honba, int kyotaku, int oya, string doraMarker, List<List<string>> tehais)
        {
            clientController.StartKyoku(bakaze, kyoku, honba, kyotaku, oya, doraMarker, tehais);
            clientRouter.SendNone();
        }



        internal void GetTsumo(int actor, string pai)
        {
            clientController.Tsumo(actor, pai);
            if (actor == positionId)
            {
                //thinkDahai();
                var tsumogiri = true;
                clientRouter.SendDahai(actor, pai, tsumogiri);
            }
            else
            {
                clientRouter.SendNone();
            }

        }

        internal void GetDahai(int actor, string pai, bool tsumogiri)
        {
            clientController.Dahai(actor, pai, tsumogiri);
            //thinkNaki();
            var doNaki = false;

            if (doNaki)
            {
//                clientRouter.SendPon();
            }
            else
            {
                clientRouter.SendNone();
            }
        }

        internal void GetPon(int actor, int target, string pai, List<string> consumed)
        {
            throw new NotImplementedException();
        }

        internal void GetChi(int actor, int target, string pai, List<string> consumed)
        {
            throw new NotImplementedException();
        }

        internal void GetKakan(int actor, int target, string pai, List<string> consumed)
        {
            throw new NotImplementedException();
        }

        internal void GetAnkan(int actor, int target, string pai, List<string> consumed)
        {
            throw new NotImplementedException();
        }

        internal void GetDaiminkan(int actor, int target, string pai, List<string> consumed)
        {
            throw new NotImplementedException();
        }

        internal void GetDora(string pai)
        {
            throw new NotImplementedException();
        }

        internal void GetReach(int actor)
        {
            throw new NotImplementedException();
        }

        internal void GetReachAccepted(int actor, List<int> deltas, List<int> scores)
        {
            throw new NotImplementedException();
        }

        internal void GetHora(int actor, int target, string pai, List<string> uradoraMarkers, List<string> horaTehais, Dictionary<string, int> yakus, int fu, int fan, int horaPoints, List<int> deltas, List<int> scores)
        {
            throw new NotImplementedException();
        }

        internal void GetRyukyoku(string reason, List<List<string>> tehais)
        {
            clientRouter.SendNone();
        }

        internal void GetEndKyoku()
        {
            clientRouter.SendNone();
        }
    }
}
