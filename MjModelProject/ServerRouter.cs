using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjModelProject
{
    class ServerRouter
    {
               




        //受信処理
        public void GetMessageRouteingClient(string msgJsonString)
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
                    StartKyoku(msgobj.bakaze, msgobj.kyoku);
                    break;
                
            }

        }


        //フィールドをいじる関数群
        //CtoS
        void Join(string name, string room)
        {

        }

        //StoC
        void StartGame(int id, List<string> names)
        {
            
        }

        //StoC
        void StartKyoku(int bakaze, int kyoku, int honba, int kyotaku, int oya, int doraMaker, List<List<int>> tehais) { }

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
        void ReachAccept(int actor, List<int> deltas, List<int> scores) { }

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
}
