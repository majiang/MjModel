using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using MjNetworkProtocol;

namespace MjClient
{
    //client側ね
    public class ClientRouter
    {

        
        public TcpClient tcpClient;
        public ClientPlayer clientPlayer;

        public ClientRouter(TcpClient tclt)
        {
            tcpClient = tclt;
            clientPlayer = new ClientPlayer(this);
        }



        //サーバからメッセージを受信してクライアントコントローラに命令を出す部分
        public void RouteGetMessage(string msg)
        {
            var msgobj = JsonConvert.DeserializeObject<MjsonMessageAll>(msg);

            switch (msgobj.type)
                {
                    case MsgType.START_GAME:
                        clientPlayer.OnStartGame(msgobj.id, msgobj.names);
                        break;

                    case MsgType.START_KYOKU:
                        clientPlayer.OnStartKyoku(msgobj.bakaze, msgobj.kyoku, msgobj.honba, msgobj.kyotaku, msgobj.oya, msgobj.dora_marker, msgobj.tehais);
                        break;

                    case MsgType.TSUMO:
                        clientPlayer.OnTsumo(msgobj.actor, msgobj.pai);
                        break;

                    case MsgType.DAHAI:
                        clientPlayer.OnDahai(msgobj.actor, msgobj.pai, msgobj.tsumogiri);
                        break;

                    case MsgType.PON:
                        clientPlayer.OnPon(msgobj.actor, msgobj.target, msgobj.pai, msgobj.consumed);
                        break;

                    case MsgType.CHI:
                        clientPlayer.OnChi(msgobj.actor, msgobj.target, msgobj.pai, msgobj.consumed);
                        break;

                    case MsgType.KAKAN:
                        clientPlayer.OnKakan(msgobj.actor, msgobj.target, msgobj.pai, msgobj.consumed);
                        break;

                    case MsgType.ANKAN:
                        clientPlayer.OnAnkan(msgobj.actor, msgobj.target, msgobj.pai, msgobj.consumed);
                        break;

                    case MsgType.DAIMINKAN:
                        clientPlayer.OnDaiminkan(msgobj.actor, msgobj.target, msgobj.pai, msgobj.consumed);
                        break;

                    case MsgType.DORA:
                        clientPlayer.OnDora(msgobj.dora_marker);
                        break;

                    case MsgType.REACH:
                        clientPlayer.OnReach(msgobj.actor);
                        break;

                    case MsgType.REACH_ACCEPTED:
                        clientPlayer.OnReachAccepted(msgobj.actor, msgobj.deltas, msgobj.scores);
                        break;

                    case MsgType.HORA:
                        clientPlayer.OnHora(msgobj.actor, msgobj.target, msgobj.pai, msgobj.uradora_markers, msgobj.hora_tehais, msgobj.yakus, msgobj.fu, msgobj.fan, msgobj.hora_points, msgobj.deltas, msgobj.scores);
                        break;

                    case MsgType.RYUKYOKU:
                        clientPlayer.OnRyukyoku(msgobj.reason, msgobj.tehais);
                        break;

                    case MsgType.END_KYOKU:
                        clientPlayer.OnEndKyoku();
                        break;

                    case MsgType.END_GAME:
                        clientPlayer.OnEndGame();
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
