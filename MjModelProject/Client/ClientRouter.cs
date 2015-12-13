using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Diagnostics;

namespace MjModelProject
{
    //client側ね
    public class ClientRouter : RouterInterface
    {

        
        public Client client;
        public IPAddress clientIpAddress;
        public static readonly IPAddress SERVER_IP = IPAddress.Parse("10.0.1.1");

        //メッセージ送信用
        public VirtualInternet virtualInternet;

        //受信メッセージ補完リスト
        List<Packet> getPacketList;


        public ClientRouter(VirtualInternet vi)
        {
            virtualInternet = vi;
            getPacketList = new List<Packet>();
        }






        public void UpDateServer()
        {
            if (getPacketList.Count > 0)
            {
                foreach (var packet in getPacketList)
                {
                    RouteGetMessage(packet);
                }
                getPacketList.Clear();
            }
        }

       
        //受信処理
        public void AddPacket(Packet packet)
        {
            getPacketList.Add(packet);
        }

        //サーバからメッセージを受信してクライアントコントローラに命令を出す部分
        public void RouteGetMessage(Packet packet)
        {
            
            var msgobj = JsonConvert.DeserializeObject<MjsonMessageAll>(packet.jsonMessage);


            switch (msgobj.type)
            {
                case MsgType.START_GAME:
                    client.OnStartGame(msgobj.id, msgobj.names);
                    break;

                case MsgType.START_KYOKU:
                    client.OnStartKyoku(msgobj.bakaze, msgobj.kyoku, msgobj.honba, msgobj.kyotaku, msgobj.oya, msgobj.doraMarker, msgobj.tehais);
                    break;

                case MsgType.TSUMO:
                    client.OnTsumo(msgobj.actor, msgobj.pai);
                    break;
                
                case MsgType.DAHAI:
                    client.OnDahai(msgobj.actor, msgobj.pai, msgobj.tsumogiri);
                    break;

                case MsgType.PON:
                    client.OnPon(msgobj.actor, msgobj.target, msgobj.pai, msgobj.consumed);
                    break;

                case MsgType.CHI:
                    client.OnChi(msgobj.actor, msgobj.target, msgobj.pai, msgobj.consumed);
                    break;

                case MsgType.KAKAN:
                    client.OnKakan(msgobj.actor, msgobj.target, msgobj.pai, msgobj.consumed);
                    break;

                case MsgType.ANKAN:
                    client.OnAnkan(msgobj.actor, msgobj.target, msgobj.pai, msgobj.consumed);
                    break;

                case MsgType.DAIMINKAN:
                    client.OnDaiminkan(msgobj.actor, msgobj.target, msgobj.pai, msgobj.consumed);
                    break;

                case MsgType.DORA:
                    client.OnDora(msgobj.doraMarker);
                    break;

                case MsgType.REACH:
                    client.OnReach(msgobj.actor);
                    break;

                case MsgType.REACH_ACCEPTED:
                    client.OnReachAccepted(msgobj.actor, msgobj.deltas, msgobj.scores);
                    break;

                case MsgType.HORA:
                    client.OnHora(msgobj.actor, msgobj.target, msgobj.pai, msgobj.uradoraMarkers, msgobj.horaTehais, msgobj.yakus, msgobj.fu, msgobj.fan, msgobj.horaPoints, msgobj.deltas, msgobj.scores);
                    break;

                case MsgType.RYUKYOKU:
                    client.OnRyukyoku(msgobj.reason, msgobj.tehais);
                    break;

                case MsgType.END_KYOKU:
                    client.OnEndKyoku();
                    break;



            }

        }




        public void SendMessageToServer(string message)
        {
            virtualInternet.RoutePacket(new Packet(clientIpAddress, SERVER_IP, message));
        }

        //サーバにメッセージを送信する命令群
        //CtoS
        public void SendJoin(MJsonMessageJoin msg){
            SendMessageToServer(JsonConvert.SerializeObject(msg));
        }

        //StoC
 //       void SendStartGame(int id, List<string> names)
 
        //StoC
//        void SendStartKyoku(int bakaze, int kyoku, int honba, int kyotaku, int oya, int doraMarker, List<List<int>> tehais) { }

        //StoC
//        void SendTsumo(int actor, string pai) { }

        //Both
        public void SendDahai(MJsonMessageDahai msg){
            SendMessageToServer(JsonConvert.SerializeObject(msg));
        }

        //Both
        public void SendPon(MJsonMessagePon msg){
            SendMessageToServer(JsonConvert.SerializeObject(msg));
        }
        //Both
        public void SendChi(MJsonMessageChi msg)
        {
            SendMessageToServer(JsonConvert.SerializeObject(msg));
        }
        //Both
        public void SendKakan(int actor, int target, string pai, List<int> consumed) { }

        //Both
        public void SendDaiminkan(int actor, int target, string pai, List<int> consumed) { }

        //Both
        public void SendAnkan(int actor, int target, string pai, List<int> consumed) { }

        //StoC
//        void Dora(int doraMarker) { }

        //Both
        public void SendReach(int actor) { }

        //StoC
        //void ReachAccepted(int actor, List<int> deltas, List<int> scores) { }

        //CtoS
        public void SendHora(int actor, int target, string pai) { }

        //StoC
        //void Hora(int actor, int target, string pai, List<int> uradoraMarkers, List<int> horaTehais, Dictionary<string, int> yakus, int fu, int fan, int horaPoints, List<int> deltas, List<int> scores) { }

        //StoC
        //void Ryukyoku(string reason, List<List<int>> tehais) { }

        //StoC
        //void EndKyoku()


        //CtoS
        public void SendNone() {
            SendMessageToServer(JsonConvert.SerializeObject(new MJsonMessageNone()));
        }

    }

 
}
