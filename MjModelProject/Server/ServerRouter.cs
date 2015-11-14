using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace MjModelProject
{
    public class ServerRouter : RouterInterface
    {

        public VirtualInternet virtualInternet;
        //ユーザーネームは一意という前提。

        //メッセージ受信時にクライアントの名前を見てルーム名を取得。後にコントローラを取得
        public Dictionary<string, ServerContext> roomNameServerDictionary;//部屋ごとにコントローラを作成< room, servercontroller >
        public Dictionary<string, string> clientNameRoomDictionary;//クライアントリスト< name, room >

        public Dictionary<string, TcpClient> clientNameClientDictionary;

        //メッセージ送信時にクライアントの名前が渡されるので名前でクライアントルータを取得して送信。
        public Dictionary<string, IPAddress> clientNameIpDictionary;//プレーヤ名とクライアントIPのリスト< name, clientIP > //ソケットに変更予定
        


        //受信メッセージ保管リスト
        public List<Packet> getPacketList;
        


        public ServerRouter(VirtualInternet vi)
        {
            virtualInternet = vi;
            roomNameServerDictionary = new Dictionary<string, ServerContext>();
            clientNameRoomDictionary = new Dictionary<string, string>();
            clientNameIpDictionary = new Dictionary<string, IPAddress>();
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

            switch (msgobj.type)
            {
                
                case MsgType.JOIN:
                    //roomがない場合room作成
                    if (!roomNameServerDictionary.ContainsKey(msgobj.room))
                    {
                        roomNameServerDictionary.Add(msgobj.room, new ServerContext(this, msgobj.room));
                    }
                    //参加人数が上限に達していないかチェック
                    if (roomNameServerDictionary[msgobj.room].CanJoin())
                    {
                        Console.WriteLine("{0} Join at {1}",msgobj.name,msgobj.room);
                        clientNameRoomDictionary.Add(msgobj.name, msgobj.room);
                        clientNameIpDictionary.Add(msgobj.name, packet.fromIpAddress);
                        //Join実行中にクライアント名をキーにしてIPを探索するので、
                        //Join実行前にclientNameIpDictionaryを登録する必要あり。
                        roomNameServerDictionary[msgobj.room].GetJoin(msgobj);
                    }else
                    {
                        Console.WriteLine("Can't Join at {0}", msgobj.room);
                    }
                    break;

                default:
                    var clientName = clientNameIpDictionary.FirstOrDefault(e => e.Value == packet.fromIpAddress).Key;
                    var roomName = clientNameRoomDictionary[clientName];
                    roomNameServerDictionary[roomName].GetMessage(msgobj);
                    roomNameServerDictionary[roomName].Execute();
                    break;
            }

        }


   

        //クライアントにメッセージを送る関数群
        public void SendMessageToClient(string name, string message)
        {
            virtualInternet.RoutePacket( new Packet(Constants.SERVER_IP, clientNameIpDictionary[name], message) );
        }
        //CtoS
 //       public void SendJoin(string name , string room){}
     
        //StoC
        public void SendStartGame(string name, MJsonMessageStartGame msgobj)
        {
            SendMessageToClient(name, JsonConvert.SerializeObject(msgobj));
        }

        //StoC
        public void SendStartKyoku(string name, MJsonMessageStartKyoku msgobj)
        {
            SendMessageToClient(name, JsonConvert.SerializeObject(msgobj));
         }

        //StoC
        public void SendTsumo(string name, MJsonMessageTsumo msgobj) 
        {
            SendMessageToClient(name, JsonConvert.SerializeObject(msgobj));
        }

        //Both
        public void SendDahai(string name, MJsonMessageDahai msgobj) 
        {
            SendMessageToClient(name, JsonConvert.SerializeObject(msgobj));
        }

        //Both
        public void SendPon(string name, MJsonMessagePon msgobj)
        {
            SendMessageToClient(name, JsonConvert.SerializeObject(msgobj));
        }
        //Both
        public void SendChi(string name, MJsonMessageChi msgobj)
        {
            SendMessageToClient(name, JsonConvert.SerializeObject(msgobj));
        }

        //Both
        public void SendKakan(string name, MJsonMessageKakan msgobj) 
        {
            SendMessageToClient(name, JsonConvert.SerializeObject(msgobj));
        }

        //Both
        public void SendDaiminkan(string name, MJsonMessageDaiminkan msgobj) 
        {
            SendMessageToClient(name, JsonConvert.SerializeObject(msgobj));
        }

        //Both
        public void SendAnkan(string name, MJsonMessageAnkan msgobj) 
        {
            SendMessageToClient(name, JsonConvert.SerializeObject(msgobj));
        }

        //StoC
        public void SendDora(string name, MJsonMessageDora msgobj) 
        {
            SendMessageToClient(name, JsonConvert.SerializeObject(msgobj));
        }

        //Both
        public void SendReach(string name, MJsonMessageReach msgobj)
        {
            SendMessageToClient(name, JsonConvert.SerializeObject(msgobj));
        }

        //StoC
        public void SendReachAccept(string name, MJsonMessageReachAccept msgobj)
        {
            SendMessageToClient(name, JsonConvert.SerializeObject(msgobj));
        }

        //CtoS
        //      public   void Hora(int actor, int target, string pai) { }

        //StoC
        public void SendHora(string name, MJsonMessageHora msgobj)
        {
            SendMessageToClient(name, JsonConvert.SerializeObject(msgobj));
        }
        //StoC
        public void SendRyukyoku(string name, MJsonMessageRyukyoku msgobj)
        {
            SendMessageToClient(name, JsonConvert.SerializeObject(msgobj));
        }
        //StoC
        public void SendEndkyoku(string name, MJsonMessageEndkyoku msgobj)
        {
            SendMessageToClient(name, JsonConvert.SerializeObject(msgobj));
        }

        //CtoS
        //     public   void None() { }

    }
    
}
