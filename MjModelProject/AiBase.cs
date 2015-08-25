using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjModelProject
{
    public class AiBase
    {
        //通信マネージャ
        //NetworkManager

        
        //思考マネージャ
        //Storategy


        //フィールドをいじる関数群
        //全てサーバからのメッセージ
        void StartGame(int id, List<string> names){}

        void StartGame(int bakaze, int kyoku, int honba, int kyotaku, int oya, int doraMaker, List<List<int>> tehais){}

        void Tsumo(int actor, int pai){}

        void Dahai(int actor, int pai, bool tsumogiri){}

        void Pon(int actor, int target, int pai, List<int> consumed){}

        void Chi(int actor, int target, int pai, List<int> consumed){}

        void Kakan(int actor, int target, int pai, List<int> consumed){}

        void Daiminkan(int actor, int target, int pai, List<int> consumed){}

        void Ankan(int actor, int target, int pai, List<int> consumed){}

        void Dora(int doraMarker){}

        void Reach(int actor){}

        void ReachAccept(int actor, List<int> deltas, List<int> scores){}

        void Hora(int actor, int target, int pai, List<int> uradoraMarkers, List<int> horaTehais, Dictionary<string, int> yakus, int fu, int fan, int horaPoints, List<int> deltas, List<int> scores){}

        void Ryukyoku(string reason, List<List<int>> tehais){}
        


        //思考する関数群
        //非同期で考える？
        public virtual void ThinkDahai(){}

        public virtual void ThinkNaki(){}

        public virtual void ThinkReach(){}

        public virtual void ThinkHora(){}

    }
}
