using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace MjModelProject
{
    //client側ね
    public class ClientRouter
    {

        
        public TcpClient tcpClient;
        public Client client;

        public ClientRouter(TcpClient tclt)
        {
            tcpClient = tclt;
            client = new Client(this);
        }



        //サーバからメッセージを受信してクライアントコントローラに命令を出す部分
        public void RouteGetMessage(string msg)
        {
            var msgobj = JsonConvert.DeserializeObject<MjsonMessageAll>(msg);

            switch (msgobj.type)
                {
                    case MsgType.START_GAME:
                        client.OnStartGame(msgobj.id, msgobj.names);
                        break;

                    case MsgType.START_KYOKU:
                        client.OnStartKyoku(msgobj.bakaze, msgobj.kyoku, msgobj.honba, msgobj.kyotaku, msgobj.oya, msgobj.dora_marker, msgobj.tehais);
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
                        client.OnDora(msgobj.dora_marker);
                        break;

                    case MsgType.REACH:
                        client.OnReach(msgobj.actor);
                        break;

                    case MsgType.REACH_ACCEPTED:
                        client.OnReachAccepted(msgobj.actor, msgobj.deltas, msgobj.scores);
                        break;

                    case MsgType.HORA:
                        client.OnHora(msgobj.actor, msgobj.target, msgobj.pai, msgobj.uradora_markers, msgobj.hora_tehais, msgobj.yakus, msgobj.fu, msgobj.fan, msgobj.hora_points, msgobj.deltas, msgobj.scores);
                        break;

                    case MsgType.RYUKYOKU:
                        client.OnRyukyoku(msgobj.reason, msgobj.tehais);
                        break;

                    case MsgType.END_KYOKU:
                        client.OnEndKyoku();
                        break;

                    case MsgType.END_GAME:
                        client.OnEndGame();
                        break;
            }



        }




        public async void SendMessageToServer(string message)
        {
            message += "\n";
            Encoding enc = Encoding.UTF8;
            byte[] sendBytes = enc.GetBytes(message);
            //データを送信する
            await tcpClient.GetStream().WriteAsync(sendBytes, 0, sendBytes.Length);
            Console.WriteLine("send : " + message);
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
