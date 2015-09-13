using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net;

namespace MjModelProject
{
    public class ServerRouter : RouterInterface
    {

        public VirtualInternet virtualInternet;
        //ユーザーネームは一意という前提。

        //メッセージ受信時にクライアントの名前を見てルーム名を取得後にコントローラを取得
        public Dictionary<string, ServerController> roomServerControllers;//部屋ごとにコントローラを作成< room, servercontroller >
        public Dictionary<string, string> clientNameRooms;//クライアントリスト< name, room >
        
        //メッセージ送信時にクライアントの名前が渡されるので名前でクライアントルータを取得して送信。
        public Dictionary<string, IPAddress> clientNameIPs;//プレーヤ名とクライアントIPのリスト< name, clientIP > //ソケットに変更予定
        

        //受信メッセージ保管リスト
        public List<Packet> getPacketList;



        public ServerRouter()
        {
            roomServerControllers = new Dictionary<string, ServerController>();
            clientNameRooms = new Dictionary<string, string>();
            clientNameIPs = new Dictionary<string, IPAddress>();
            getPacketList = new List<Packet>();
        }




 
        public void UpDateServer()
        {
            if (getPacketList.Count > 0)
            {
                foreach (var message in getPacketList)
                {
                    RouteGetMessage(message);
                }
                getPacketList.Clear();
            }
        }







        //受信処理
        public void AddPacket(Packet packet){
            getPacketList.Add(packet);
        }
        

        //受信メッセージルーティング処理
        public void RouteGetMessage(Packet packet)
        {
            var msgobj = JsonConvert.DeserializeObject<MjsonMessageAll>(packet.jsonMessage);
            Console.WriteLine(msgobj.type);
            Console.WriteLine(packet.jsonMessage.ToString());
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
                        Console.WriteLine("{0} Join at {1}",msgobj.name,msgobj.room);
                        clientNameRooms.Add(msgobj.name, msgobj.room);
                        clientNameIPs.Add(msgobj.name, packet.fromIpAddress);
                        //Join実行中にクライアント名をキーにしてIPを探索するのでIPの登録は実行した後にJoinを実行する
                        roomServerControllers[msgobj.room].Join(msgobj.name);
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
        public void SendMessageToClient(string name, string message)
        {
            virtualInternet.RoutePacket( new Packet(Constants.SERVER_IP, clientNameIPs[name], message) );
        }


        //CtoS
 //       public void SendJoin(string name , string room){}
     
        //StoC
        public void SendStartGame(string name, int id, List<string> names)
        {
            
        }

        //StoC
        public void SendStartKyoku(string name, int bakaze, int kyoku, int honba, int kyotaku, int oya, int doraMarker, List<List<string>> tehais)
        {
            SendMessageToClient(name, JsonConvert.SerializeObject(new MJsonMessageStartKyoku(bakaze, kyoku, honba, kyotaku, oya, doraMarker, tehais) ));
        }

        //StoC
        public void SendTsumo(string name, int actor, string pai) { }

        //Both
        public void SendDahai(string name, int actor, string pai, bool tsumogiri) { }

        //Both
        public void SendPon(string name, int actor, int target, string pai, List<string> consumed) { }

        //Both
        public void SendChi(string name, int actor, int target, string pai, List<string> consumed) { }

        //Both
        public void SendKakan(string name, int actor, int target, string pai, List<string> consumed) { }

        //Both
        public void SendDaiminkan(string name, int actor, int target, string pai, List<string> consumed) { }

        //Both
        public void SendAnkan(string name, int actor, int target, string pai, List<string> consumed) { }

        //StoC
        public void SendDora(string name, int doraMarker) { }

        //Both
        public void SendReach(string name, int actor) { }

        //StoC
        public void SendReachAccept(string name, int actor, List<int> deltas, List<int> scores) { }

        //CtoS
        //      public   void Hora(int actor, int target, string pai) { }

        //StoC
        public void SendHora(string name, int actor, int target, string pai, List<int> uradoraMarkers, List<int> horaTehais, Dictionary<string, int> yakus, int fu, int fan, int horaPoints, List<int> deltas, List<int> scores) { }

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
