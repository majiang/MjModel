using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MjModelProject
{
    public class ServerRouter
    {

        Dictionary<string , ServerController> roomServerControllers;//部屋ごとにコントローラを作成< room, servercontroller >
        Dictionary<string, string> clientNameRooms;//クライアントリスト< name, room >
        Dictionary<string, List<ClientRouter>> roomClientRouters;//部屋ごとのクライアントリスト< room, List<client> >

        public ServerRouter()
        {
            roomServerControllers = new Dictionary<string, ServerController>();
            clientNameRooms = new Dictionary<string, string>();
        }

        public void SetClientRouter(){

        }

        //送信処理
        public void SendMessage(string roomName, string msgJsonString){
            
        }
        

        //受信処理
        public void RouteGetMessage(string msgJsonString)
        {
            var msgobj = JsonConvert.DeserializeObject<MjsonMessageAll>(msgJsonString);
            Console.WriteLine(msgobj.type);
            Console.WriteLine(msgJsonString.ToString());
            Console.WriteLine(msgobj.doraMarker.ToString());

            switch (msgobj.type)
            {
                case MsgType.JOIN:
                    //roomがない場合作成
                    if (!roomServerControllers.ContainsKey(msgobj.room))
                    {
                        roomServerControllers.Add(msgobj.room, new ServerController(this, msgobj.room));
                    }
                    if (roomServerControllers[msgobj.room].CanJoin())
                    {
#if DEBUG
                        Console.WriteLine("Join at {0}",msgobj.room);
                        roomServerControllers[msgobj.room].Join(msgobj.name);
#endif
                    }
                    else
                    {
                        Console.WriteLine("Can't Join at {0}", msgobj.room);
                    }

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
            
                case MsgType.REACH:
                    Reach(msgobj.actor);
                    break;

                case MsgType.HORA:
                    Hora(msgobj.actor, msgobj.target, msgobj.pai);
                    break;

                case MsgType.NONE:
                    None();
                    break;
            }

        }


        //こっからしたは無視。

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
