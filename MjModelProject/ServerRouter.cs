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
        //ユーザーネームは一意という前提。
        Dictionary<string, ServerController> roomServerControllers;//部屋ごとにコントローラを作成< room, servercontroller >
        Dictionary<string, string> clientNameRooms;//クライアントリスト< name, room >
        Dictionary<string, ClientRouter> roomClientRouters;//プレーヤ名とクライアントのリスト< name, clientRouter > //ソケットに変更予定
        
        List<string> getMessageList;


 
        public void UpDateServer()
        {
            if (getMessageList.Count > 0)
            {
                foreach (var message in getMessageList)
                {
                    RouteGetMessage(message);
                }
                getMessageList.Clear();
            }
        }



        public ServerRouter()
        {
            roomServerControllers = new Dictionary<string, ServerController>();
            clientNameRooms = new Dictionary<string, string>();
            roomClientRouters = new Dictionary<string, ClientRouter>();
        }

        public void SetClientRouter(){

        }

        //受信処理
        public void GetMessage(string msgJsonString){
            getMessageList.Add(msgJsonString);
        }
        

        //受信メッセージルーティング処理
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
                    roomServerControllers[msgobj.name].Dahai(msgobj.actor, msgobj.pai, msgobj.tsumogiri);
                    break;

                case MsgType.PON:
                    roomServerControllers[msgobj.name].Pon(msgobj.actor, msgobj.target, msgobj.pai, msgobj.consumed);
                    break;

                case MsgType.CHI:
                    roomServerControllers[msgobj.name].Chi(msgobj.actor, msgobj.target, msgobj.pai, msgobj.consumed);
                    break;

                case MsgType.KAKAN:
                    roomServerControllers[msgobj.name].Kakan(msgobj.actor, msgobj.target, msgobj.pai, msgobj.consumed);
                    break;

                case MsgType.ANKAN:
                    roomServerControllers[msgobj.name].Ankan(msgobj.actor, msgobj.target, msgobj.pai, msgobj.consumed);
                    break;

                case MsgType.DAIMINKAN:
                    roomServerControllers[msgobj.name].Daiminkan(msgobj.actor, msgobj.target, msgobj.pai, msgobj.consumed);
                    break;
            
                case MsgType.REACH:
                    roomServerControllers[msgobj.name].Reach(msgobj.actor);
                    break;

                case MsgType.HORA:
                    roomServerControllers[msgobj.name].Hora(msgobj.actor, msgobj.target, msgobj.pai);
                    break;

                case MsgType.NONE:
                    roomServerControllers[msgobj.name].None();
                    break;
            }

        }


   
        //クライアントにメッセージを送る関数群
        //CtoS
 //       public void SendJoin(string name , string room){}
   



     
        //StoC
        public void SendStartGame(string name, int id, List<string> names)
        {
            
        }

        //StoC
        public void SendStartKyoku(string name, int bakaze, int kyoku, int honba, int kyotaku, int oya, int doraMaker, List<List<int>> tehais) { }

        //StoC
        public void SendTsumo(string name, int actor, int pai) { }

        //Both
        public void SendDahai(string name, int actor, int pai, bool tsumogiri) { }

        //Both
        public void SendPon(string name, int actor, int target, int pai, List<int> consumed) { }

        //Both
        public void SendChi(string name, int actor, int target, int pai, List<int> consumed) { }

        //Both
        public void SendKakan(string name, int actor, int target, int pai, List<int> consumed) { }

        //Both
        public void SendDaiminkan(string name, int actor, int target, int pai, List<int> consumed) { }

        //Both
        public void SendAnkan(string name, int actor, int target, int pai, List<int> consumed) { }

        //StoC
        public void SendDora(string name, int doraMarker) { }

        //Both
        public void SendReach(string name, int actor) { }

        //StoC
        public void SendReachAccept(string name, int actor, List<int> deltas, List<int> scores) { }

        //CtoS
        //      public   void Hora(int actor, int target, int pai) { }

        //StoC
        public void SendHora(string name, int actor, int target, int pai, List<int> uradoraMarkers, List<int> horaTehais, Dictionary<string, int> yakus, int fu, int fan, int horaPoints, List<int> deltas, List<int> scores) { }

        //StoC
        public void SendRyukyoku(string name, string reason, List<List<int>> tehais) { }

        //StoC
        public void SendEndKyoku(string name)
        {

        }

        //CtoS
        //     public   void None() { }

    }
    
}
