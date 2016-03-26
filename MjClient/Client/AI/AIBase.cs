using System;
using System.Collections.Generic;
using MjNetworkProtocol;
using MjModelLibrary;

namespace MjClient.AI
{

    public delegate void SendHoraHandler(MJsonMessageHora msgobj);
    public delegate void SendDahaiHandler();
    public delegate void SendAnkanHandler();
    public delegate void SendKakanHandler();
    public delegate void SendPonHandler();
    public delegate void SendChiHandler();
    public delegate void SendDaiminkanHandler();
    public delegate void SendNoneHandler();

   

    interface AIInterface {

        event SendPonHandler SendPon;
        event SendChiHandler SendChi;
        event SendDaiminkanHandler SendDaiminkan;
        event SendNoneHandler SendNone;

        event SendHoraHandler SendHora;
        event SendDahaiHandler SendDahai;
        event SendAnkanHandler SendAnkan;
        event SendKakanHandler SendKakan;

        //ツモ順が来た時にどんなアクションするか
        //ツモ、打牌、暗槓、加槓のイベントを発生させる
        void thinkOnTsumo(int myPositionId, string pai, List<Tehai> tehais, List<Kawa> kawas, Field field);

        //外家が打牌した時にどんなアクションするか
        //何もしない、和了、ポン、カン、チーのイベントを発生させる
        void thinkOnOtherPlayerDoroped(int myPositionId,int dapaiActor, string pai, List<Tehai> tehais, List<Kawa> kawas, Field field);

    }
}
