using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjServer
{
    public class ClientController
    {
        public ClientRouter clientRouter;
        public ClientMjModel clientMjModel;

       

        public void StartGame(int id, List<string> names)
        {
            clientMjModel.StartGame(id);
        }

        internal void StartKyoku(string bakaze, int kyoku, int honba, int kyotaku, int oya, string doraMarker, List<List<string>> tehais)
        {
            clientMjModel.StartKyoku(bakaze, kyoku, honba, kyotaku, oya, doraMarker, tehais);
        }

        internal void Tsumo(int actor, string pai)
        {
            clientMjModel.Tsumo(actor, pai);
        }

        internal void Dahai(int actor, string pai, bool tsumogiri)
        {
            clientMjModel.Dahai(actor, pai, tsumogiri);
        }

        internal void Pon(int actor, int target, string pai, List<string> consumed)
        {
            clientMjModel.Pon(actor, target, pai, consumed);
        }

        internal void Chi(int actor, int target, string pai, List<string> consumed)
        {
            clientMjModel.Chi(actor, target, pai, consumed);
        }

        internal void Kakan(int actor, int target, string pai, List<string> consumed)
        {
            throw new NotImplementedException();
        }

        internal void Ankan(int actor, int target, string pai, List<string> consumed)
        {
            throw new NotImplementedException();
        }

        internal void Daiminkan(int actor, int target, string pai, List<string> consumed)
        {
            throw new NotImplementedException();
        }

        internal void Dora(string pai)
        {
            throw new NotImplementedException();
        }

        internal void Reach(int actor)
        {
            throw new NotImplementedException();
        }

        internal void ReachAccepted(int actor, List<int> deltas, List<int> scores)
        {
            throw new NotImplementedException();
        }

        internal void Hora(int actor, int target, string pai, List<string> uradoraMarkers, List<string> horaTehais, Dictionary<string, int> yakus, int fu, int fan, int horaPoints, List<int> deltas, List<int> scores)
        {
            throw new NotImplementedException();
        }

        internal void Ryukyoku(string reason, List<List<string>> tehais)
        {
            throw new NotImplementedException();
        }

        internal void EndKyoku()
        {
            throw new NotImplementedException();
        }

        public void SendNone()
        {
            clientRouter.SendNone();
        }
    }
}
