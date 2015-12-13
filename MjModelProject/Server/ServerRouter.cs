using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MjModelProject
{
    public class ServerRouter
    {
        private System.Threading.SemaphoreSlim semaphore = new System.Threading.SemaphoreSlim(1, 1);
        //public VirtualInternet virtualInternet;
        //ユーザーネームは一意という前提。

        //メッセージ受信時にクライアントの名前を見てルーム名を取得。後にコントローラを取得
        public Dictionary<string, ServerContext> roomNameServerDictionary;//部屋ごとにコントローラを作成< room, servercontroller >
        public Dictionary<string, string> clientNameRoomDictionary;//クライアントリスト< name, room >

        public Dictionary<string, TcpClient> clientNameTcpClientDictionary;

        //メッセージ送信時にクライアントの名前が渡されるので名前でクライアントルータを取得して送信。
        public Dictionary<string, IPAddress> clientNameIpDictionary;//プレーヤ名とクライアントIPのリスト< name, clientIP > //ソケットに変更予定
        


        //受信メッセージ保管リスト
        public List<Packet> getPacketList;
        


        public ServerRouter()
        {
            //virtualInternet = vi;
            roomNameServerDictionary = new Dictionary<string, ServerContext>();
            clientNameRoomDictionary = new Dictionary<string, string>();
            //clientNameIpDictionary = new Dictionary<string, IPAddress>();
            //getPacketList = new List<Packet>();
            clientNameTcpClientDictionary = new Dictionary<string, TcpClient>();
        }

        

        //受信メッセージルーティング処理
        public async void RouteGetMessage(TcpClient client, string message)
        {
            await semaphore.WaitAsync();
            try {

                MjsonMessageAll msgobj = JsonConvert.DeserializeObject<MjsonMessageAll>(message);

                if (IsValidMessageType(msgobj.type) == false)
                {
                    SendErrorToClient(client, message);
                    return;
                }

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
                            Console.WriteLine("{0} Join at {1}", msgobj.name, msgobj.room);
                            clientNameRoomDictionary.Add(msgobj.name, msgobj.room);
                            clientNameTcpClientDictionary.Add(msgobj.name, client);
                            //Join実行中にクライアント名をキーにしてIPを探索するので、
                            //Join実行前にclientNameIpDictionaryを登録する必要あり。
                            roomNameServerDictionary[msgobj.room].GetJoin(msgobj);
                        } else
                        {
                            Console.WriteLine("Can't Join at {0}", msgobj.room);
                        }
                        break;

                    default:
                        var clientName = clientNameTcpClientDictionary.FirstOrDefault(e => e.Value == client).Key;
                        if(clientName == null)
                        {
                            break;
                        }

                        var roomName = clientNameRoomDictionary[clientName];

                        if (roomName == null)
                        {
                            SendErrorToClient(client, message);
                        }
                        else {
                            roomNameServerDictionary[roomName].GetMessage(msgobj);

                        }
                        break;
                }
            }
            finally
            {
                semaphore.Release();
            }
        }

        private bool IsValidMessageType(string type)
        {
            return MsgType.MsgTypeList.Contains(type);
        }

        public void SendErrorToClient(TcpClient client, string message)
        {
            var errorMsg = MakeErrorMessage(message);
            if (client.Connected)
            {
                NetworkStream stream = client.GetStream();
                stream.Write(Encoding.ASCII.GetBytes(errorMsg), 0, errorMsg.Length);
            }
            DebugUtil.ServerDebug(errorMsg);
        }


        public void SendErrorToClient(string clientName, string message)
        {
            var errorMsg = MakeErrorMessage(message);

            if (clientNameTcpClientDictionary[clientName].Connected)
            {
                NetworkStream stream = clientNameTcpClientDictionary[clientName].GetStream();
                stream.Write(Encoding.ASCII.GetBytes(errorMsg), 0, errorMsg.Length);
            }
            DebugUtil.ServerDebug(errorMsg);
            RemoveClient(clientName);
        }

        public void RemoveClient(string clientName)
        {
            var roomName = clientNameRoomDictionary[clientName];
            roomNameServerDictionary.Remove(roomName);
            clientNameTcpClientDictionary.Remove(clientName);
            clientNameRoomDictionary.Remove(clientName);
        }
   
        public string MakeErrorMessage(string message)
        {
            return "{\"type\":\"error\", \"lastMessage\":" + message.Replace("\n", "") + "}\n";
        }


        //クライアントにメッセージを送る関数群
        public void SendMessageToClient(string name, string message)
        {
            
            if (clientNameTcpClientDictionary[name].Connected)
            {
                message += "\n";
                NetworkStream stream = clientNameTcpClientDictionary[name].GetStream();
                stream.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
            }
            else
            {
                DebugUtil.ServerDebug("Client is not Connected");
            }
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

        public void SendEndgame(string name, MJsonMessageEndgame msgobj)
        {
            SendMessageToClient(name, JsonConvert.SerializeObject(msgobj));
            RemoveClient(name);
        }




        //CtoS
        //     public   void None() { }

    }
    
}
