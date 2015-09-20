using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

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


        public ClientRouter()
        {
            getPacketList = new List<Packet>();
        }


        public void SetClientController(Client client)
        {
            this.client = client;
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
            Console.WriteLine(msgobj.type);
            Console.WriteLine(packet.jsonMessage);
            Console.WriteLine(msgobj.doraMarker.ToString());

            switch (msgobj.type)
            {
                case MsgType.START_GAME:
                    client.GetStartGame(msgobj.id, msgobj.names);
                    break;

                case MsgType.START_KYOKU:
                    client.GetStartKyoku(msgobj.bakaze, msgobj.kyoku, msgobj.honba, msgobj.kyotaku, msgobj.oya, msgobj.doraMarker, msgobj.tehais);
                    break;

                case MsgType.TSUMO:
                    client.GetTsumo(msgobj.actor, msgobj.pai);
                    break;
                
                case MsgType.DAHAI:
                    client.GetDahai(msgobj.actor, msgobj.pai, msgobj.tsumogiri);
                    break;

                case MsgType.PON:
                    client.GetPon(msgobj.actor, msgobj.target, msgobj.pai, msgobj.consumed);
                    break;

                case MsgType.CHI:
                    client.GetChi(msgobj.actor, msgobj.target, msgobj.pai, msgobj.consumed);
                    break;

                case MsgType.KAKAN:
                    client.GetKakan(msgobj.actor, msgobj.target, msgobj.pai, msgobj.consumed);
                    break;

                case MsgType.ANKAN:
                    client.GetAnkan(msgobj.actor, msgobj.target, msgobj.pai, msgobj.consumed);
                    break;

                case MsgType.DAIMINKAN:
                    client.GetDaiminkan(msgobj.actor, msgobj.target, msgobj.pai, msgobj.consumed);
                    break;

                case MsgType.DORA:
                    client.GetDora(msgobj.doraMarker);
                    break;

                case MsgType.REACH:
                    client.GetReach(msgobj.actor);
                    break;

                case MsgType.REACH_ACCEPTED:
                    client.GetReachAccepted(msgobj.actor, msgobj.deltas, msgobj.scores);
                    break;

                case MsgType.HORA:
                    client.GetHora(msgobj.actor, msgobj.target, msgobj.pai, msgobj.uradoraMarkers, msgobj.horaTehais, msgobj.yakus, msgobj.fu, msgobj.fan, msgobj.horaPoints, msgobj.deltas, msgobj.scores);
                    break;

                case MsgType.RYUKYOKU:
                    client.GetRyukyoku(msgobj.reason, msgobj.tehais);
                    break;

                case MsgType.END_KYOKU:
                    client.GetEndKyoku();
                    break;



            }

        }


        public void SendMessageToServer(string message)
        {
            virtualInternet.RoutePacket(new Packet(clientIpAddress, SERVER_IP, message));
        }

        //サーバにメッセージを送信する命令群
        //CtoS
        public void SendJoin(string name, string room){        }

        //StoC
 //       void SendStartGame(int id, List<string> names)
 
        //StoC
//        void SendStartKyoku(int bakaze, int kyoku, int honba, int kyotaku, int oya, int doraMarker, List<List<int>> tehais) { }

        //StoC
//        void SendTsumo(int actor, string pai) { }

        //Both
        public void SendDahai(int actor, string pai, bool tsumogiri) {
            SendMessageToServer(JsonConvert.SerializeObject(new MJsonMessageDahai(actor, pai, tsumogiri)));
        }

        //Both
        public void SendPon(int actor, int target, string pai, List<int> consumed) { }

        //Both
        public void SendChi(int actor, int target, string pai, List<int> consumed) { }

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
