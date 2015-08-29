using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjModelProject
{
    //client側ね
    public class ClientRouter
    {
       
       //controller




        //受信処理
        public void RouteGetMessage(string msgJsonString)
        {
            var msgobj = JsonConvert.DeserializeObject<MjsonMessageAll>(msgJsonString);
            Console.WriteLine(msgobj.type);
            Console.WriteLine(msgJsonString.ToString());
            Console.WriteLine(msgobj.doraMarker.ToString());

            switch (msgobj.type)
            {
                case MsgType.START_GAME:
                    StartGame(msgobj.id, msgobj.names);
                    break;

                case MsgType.START_KYOKU:
                    StartKyoku(msgobj.bakaze, msgobj.kyoku, msgobj.honba, msgobj.kyotaku, msgobj.oya, msgobj.doraMarker, msgobj.tehais);
                    break;

                case MsgType.TSUMO:
                    Tsumo(msgobj.actor, msgobj.pai);
                    break;
                
                case MsgType.DAHAI:
                    Dahai(msgobj.actor, msgobj.pai, msgobj.tsumogiri);
                    break;

                case MsgType.PON:
                    Pon(msgobj.actor, msgobj.target, msgobj.pai, msgobj.consumed);
                    break;

                case MsgType.CHI:
                    Chi(msgobj.actor, msgobj.target, msgobj.pai, msgobj.consumed);
                    break;

                case MsgType.KAKAN:
                    Kakan(msgobj.actor, msgobj.target, msgobj.pai, msgobj.consumed);
                    break;

                case MsgType.ANKAN:
                    Ankan(msgobj.actor, msgobj.target, msgobj.pai, msgobj.consumed);
                    break;

                case MsgType.DAIMINKAN:
                    Daiminkan(msgobj.actor, msgobj.target, msgobj.pai, msgobj.consumed);
                    break;

                case MsgType.DORA:
                    Dora(msgobj.doraMarker);
                    break;

                case MsgType.REACH:
                    Reach(msgobj.actor);
                    break;

                case MsgType.REACH_ACCEPTED:
                    ReachAccepted(msgobj.actor, msgobj.deltas, msgobj.scores);
                    break;

                case MsgType.HORA:
                    Hora(msgobj.actor, msgobj.target, msgobj.pai, msgobj.uradoraMarkers, msgobj.horaTehais, msgobj.yakus, msgobj.fu, msgobj.fan, msgobj.horaPoints, msgobj.deltas, msgobj.scores);
                    break;

                case MsgType.RYUKYOKU:
                    Ryukyoku(msgobj.reason, msgobj.tehais);
                    break;

                case MsgType.END_KYOKU:
                    EndKyoku();
                    break;



            }

        }


        //フィールドをいじる関数群
        //CtoS
        void Join(string name, string room){        }

        //StoC
        void StartGame(int id, List<string> names)
        {

        }

        //StoC
        void StartKyoku(int bakaze, int kyoku, int honba, int kyotaku, int oya, int doraMarker, List<List<int>> tehais) { }

        //StoC
        void Tsumo(int actor, int pai) { }

        //Both
        void Dahai(int actor, int pai, bool tsumogiri) { }

        //Both
        void Pon(int actor, int target, int pai, List<int> consumed) { }

        //Both
        void Chi(int actor, int target, int pai, List<int> consumed) { }

        //Both
        void Kakan(int actor, int target, int pai, List<int> consumed) { }

        //Both
        void Daiminkan(int actor, int target, int pai, List<int> consumed) { }

        //Both
        void Ankan(int actor, int target, int pai, List<int> consumed) { }

        //StoC
        void Dora(int doraMarker) { }

        //Both
        void Reach(int actor) { }

        //StoC
        void ReachAccepted(int actor, List<int> deltas, List<int> scores) { }

        //CtoS
        void Hora(int actor, int target, int pai) { }

        //StoC
        void Hora(int actor, int target, int pai, List<int> uradoraMarkers, List<int> horaTehais, Dictionary<string, int> yakus, int fu, int fan, int horaPoints, List<int> deltas, List<int> scores) { }

        //StoC
        void Ryukyoku(string reason, List<List<int>> tehais) { }

        //StoC
        void EndKyoku()
        {

        }

        //CtoS
        void None() { }

    }

 
}
