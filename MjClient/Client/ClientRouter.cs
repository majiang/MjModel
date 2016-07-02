using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using MjNetworkProtocolLibrary;

namespace MjClient
{
    //client側ね
    public class ClientRouter
    {
        public delegate void HelloHandler();
        public delegate void StartGameHandler(int id, List<string> names);
        public delegate void StartKyokuHandler(string bakaze, int kyoku, int honba, int kyotaku, int oya, string dora_marker, List<List<string>> tehais);
        public delegate void GetPaiHandler(int actor, string pai);
        public delegate void DropPaiHandler(int actor, string pai, bool tsumogiri);
        public delegate void ChiHandler(int actor, int target, string pai, List<string> consumed);
        public delegate void PonHandler(int actor, int target, string pai, List<string> consumed);
        public delegate void KakanHandler(int actor, string pai, List<string> consumed);
        public delegate void DaiminkanHandler(int actor, int target, string pai, List<string> consumed);
        public delegate void AnkanHandler(int actor, List<string> consumed);
        public delegate void ReachHandler(int actor);
        public delegate void ReachAcceptedHandler(int actor, List<int> delta, List<int> scores);
        public delegate void OpenDoraHandler(string dora_marker);
        public delegate void AgariHandler(int actor, int target, string pai, List<string> uradora_markers, List<string> hora_tehais, List<List<object>> yakus, int fu, int fan, int hora_points, List<int> deltas, List<int> scores);
        public delegate void RyukyokuHandler(string reason, List<List<string>> tehais, List<bool> tenpais, List<int> deltas, List<int> scores);
        public delegate void EndKyokuHandler();
        public delegate void EndGameHandler();
        public delegate void SetSceneHandler(int rest_tsumo_num, List<string> dora_markers, List<List<string>> kawas, List<List<bool>> is_reached_kawapai, List<int> scores, int kyoku, int honba, int kyotaku, string bakaze, int oya, List<List<string>> tehais, List<List<List<string>>> furos, int actor, int mypositionid);

        public event HelloHandler OnHello;
        public event StartGameHandler OnStartGame;
        public event StartKyokuHandler OnStartKyoku;
        public event GetPaiHandler OnGetPai;
        public event DropPaiHandler OnDropPai;
        public event ChiHandler OnChi;
        public event PonHandler OnPon;
        public event KakanHandler OnKakan;
        public event DaiminkanHandler OnDaiminkan;
        public event AnkanHandler OnAnkan;
        public event ReachHandler OnReach;
        public event ReachAcceptedHandler OnReachAccepted;
        public event OpenDoraHandler OnOpenDora;
        public event AgariHandler OnAgari;
        public event RyukyokuHandler OnRyukyoku;
        public event EndKyokuHandler OnEndKyoku;
        public event EndGameHandler OnEndGame;
        public event SetSceneHandler OnSetScene;
       

        public TcpClient tcpClient;
        public ClientFacade clientPlayer;

        public ClientRouter()
        {
        }




        public void RouteGetMessage(string msg)
        {
            var msgobj = JsonConvert.DeserializeObject<MJsonMessageAll>(msg);

            switch (msgobj.type)
            {
                case MsgType.HELLO:
                    OnHello();
                    break;

                case MsgType.START_GAME:
                    OnStartGame(msgobj.id, msgobj.names);
                    break;

                case MsgType.START_KYOKU:
                    OnStartKyoku(msgobj.bakaze, msgobj.kyoku, msgobj.honba, msgobj.kyotaku, msgobj.oya, msgobj.dora_marker, msgobj.tehais);
                    break;

                case MsgType.TSUMO:
                    OnGetPai(msgobj.actor, msgobj.pai);
                    break;

                case MsgType.DAHAI:
                    OnDropPai(msgobj.actor, msgobj.pai, msgobj.tsumogiri);
                    break;

                case MsgType.PON:
                    OnPon(msgobj.actor, msgobj.target, msgobj.pai, msgobj.consumed);
                    break;

                case MsgType.CHI:
                    OnChi(msgobj.actor, msgobj.target, msgobj.pai, msgobj.consumed);
                    break;

                case MsgType.KAKAN:
                    OnKakan(msgobj.actor, msgobj.pai, msgobj.consumed);
                    break;

                case MsgType.ANKAN:
                    OnAnkan(msgobj.actor, msgobj.consumed);
                    break;

                case MsgType.DAIMINKAN:
                    OnDaiminkan(msgobj.actor, msgobj.target, msgobj.pai, msgobj.consumed);
                    break;

                case MsgType.DORA:
                    OnOpenDora(msgobj.dora_marker);
                    break;

                case MsgType.REACH:
                    OnReach(msgobj.actor);
                    break;

                case MsgType.REACH_ACCEPTED:
                    OnReachAccepted(msgobj.actor, msgobj.deltas, msgobj.scores);
                    break;

                case MsgType.HORA:
                    OnAgari(msgobj.actor, msgobj.target, msgobj.pai, msgobj.uradora_markers, msgobj.hora_tehais, msgobj.yakus, msgobj.fu, msgobj.fan, msgobj.hora_points, msgobj.deltas, msgobj.scores);
                    break;

                case MsgType.RYUKYOKU:
                    OnRyukyoku(msgobj.reason, msgobj.tehais, msgobj.tenpais, msgobj.deltas, msgobj.scores); ;
                    break;

                case MsgType.END_KYOKU:
                    OnEndKyoku();
                    break;

                case MsgType.END_GAME:
                    OnEndGame();
                    break;

                case MsgType.SET_SCENE:
                    OnSetScene(msgobj.rest_tsumo_num, msgobj.dora_markers, msgobj.kawas, msgobj.is_reached_kawapai, msgobj.scores, msgobj.kyoku, msgobj.honba, msgobj.kyotaku, msgobj.bakaze, msgobj.oya, msgobj.tehais, msgobj.furos, msgobj.actor, msgobj.mypositionid);
                    break;
            }



        }

    }

 
}
