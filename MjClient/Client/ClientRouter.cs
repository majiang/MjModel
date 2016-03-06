using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace MjClient
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

        public void SendMJsonMessage(object jsonmsg)
        {
            SendMessageToServer(JsonConvert.SerializeObject(jsonmsg));
        }


        //CtoS
        public void SendNone() {
            SendMessageToServer(JsonConvert.SerializeObject(new MJsonMessageNone()));
        }

    }

 
}
