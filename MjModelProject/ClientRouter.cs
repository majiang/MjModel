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

        
        public ClientController clientController;
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


        public void SetClientController(ClientController clientController)
        {
            this.clientController = clientController;
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
                    clientController.StartGame(msgobj.id, msgobj.names);
                    break;

                case MsgType.START_KYOKU:
                    clientController.StartKyoku(msgobj.bakaze, msgobj.kyoku, msgobj.honba, msgobj.kyotaku, msgobj.oya, msgobj.doraMarker, msgobj.tehais);
                    break;

                case MsgType.TSUMO:
                    clientController.Tsumo(msgobj.actor, msgobj.pai);
                    break;
                
                case MsgType.DAHAI:
                    clientController.Dahai(msgobj.actor, msgobj.pai, msgobj.tsumogiri);
                    break;

                case MsgType.PON:
                    clientController.Pon(msgobj.actor, msgobj.target, msgobj.pai, msgobj.consumed);
                    break;

                case MsgType.CHI:
                    clientController.Chi(msgobj.actor, msgobj.target, msgobj.pai, msgobj.consumed);
                    break;

                case MsgType.KAKAN:
                    clientController.Kakan(msgobj.actor, msgobj.target, msgobj.pai, msgobj.consumed);
                    break;

                case MsgType.ANKAN:
                    clientController.Ankan(msgobj.actor, msgobj.target, msgobj.pai, msgobj.consumed);
                    break;

                case MsgType.DAIMINKAN:
                    clientController.Daiminkan(msgobj.actor, msgobj.target, msgobj.pai, msgobj.consumed);
                    break;

                case MsgType.DORA:
                    clientController.Dora(msgobj.doraMarker);
                    break;

                case MsgType.REACH:
                    clientController.Reach(msgobj.actor);
                    break;

                case MsgType.REACH_ACCEPTED:
                    clientController.ReachAccepted(msgobj.actor, msgobj.deltas, msgobj.scores);
                    break;

                case MsgType.HORA:
                    clientController.Hora(msgobj.actor, msgobj.target, msgobj.pai, msgobj.uradoraMarkers, msgobj.horaTehais, msgobj.yakus, msgobj.fu, msgobj.fan, msgobj.horaPoints, msgobj.deltas, msgobj.scores);
                    break;

                case MsgType.RYUKYOKU:
                    clientController.Ryukyoku(msgobj.reason, msgobj.tehais);
                    break;

                case MsgType.END_KYOKU:
                    clientController.EndKyoku();
                    break;



            }

        }


        public void SendMessageToServer(string message)
        {
            virtualInternet.RoutePacket(new Packet(clientIpAddress, SERVER_IP, message));
        }

        //サーバにメッセージを送信する命令群
        //CtoS
        void SendJoin(string name, string room){        }

        //StoC
 //       void SendStartGame(int id, List<string> names)
 
        //StoC
//        void SendStartKyoku(int bakaze, int kyoku, int honba, int kyotaku, int oya, int doraMarker, List<List<int>> tehais) { }

        //StoC
//        void SendTsumo(int actor, string pai) { }

        //Both
        void SendDahai(int actor, string pai, bool tsumogiri) { }

        //Both
        void SendPon(int actor, int target, string pai, List<int> consumed) { }

        //Both
        void SendChi(int actor, int target, string pai, List<int> consumed) { }

        //Both
        void SendKakan(int actor, int target, string pai, List<int> consumed) { }

        //Both
        void SendDaiminkan(int actor, int target, string pai, List<int> consumed) { }

        //Both
        void SendAnkan(int actor, int target, string pai, List<int> consumed) { }

        //StoC
//        void Dora(int doraMarker) { }

        //Both
        void SendReach(int actor) { }

        //StoC
        //void ReachAccepted(int actor, List<int> deltas, List<int> scores) { }

        //CtoS
        void SendHora(int actor, int target, string pai) { }

        //StoC
        //void Hora(int actor, int target, string pai, List<int> uradoraMarkers, List<int> horaTehais, Dictionary<string, int> yakus, int fu, int fan, int horaPoints, List<int> deltas, List<int> scores) { }

        //StoC
        //void Ryukyoku(string reason, List<List<int>> tehais) { }

        //StoC
        //void EndKyoku()


        //CtoS
        void SendNone() { }

    }

 
}
