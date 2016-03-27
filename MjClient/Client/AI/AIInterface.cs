using System;
using System.Collections.Generic;
using MjNetworkProtocol;
using MjModelLibrary;

namespace MjClient.AI
{

    
    public delegate void SendPonHandler(MJsonMessagePon msgobj);
    public delegate void SendChiHandler(MJsonMessageChi msgobj);
    public delegate void SendDaiminkanHandler(MJsonMessageDaiminkan msgobj);
    public delegate void SendNoneHandler(MJsonMessageNone msgobj);

    public delegate void SendHoraHandler(MJsonMessageHora msgobj);
    public delegate void SendDahaiHandler(MJsonMessageDahai msgobj);
    public delegate void SendAnkanHandler(MJsonMessageAnkan msgobj);
    public delegate void SendKakanHandler(MJsonMessageKakan msgobj);
    public delegate void SendReachHandler(MJsonMessageReach msgobj);


    interface AIInterface {

        //外家が打牌した時にどんなアクションするか
        //何もしない、和了、ポン、カン、チーのイベントを発生させる
        event SendPonHandler SendPon;
        event SendChiHandler SendChi;
        event SendDaiminkanHandler SendDaiminkan;
        event SendNoneHandler SendNone;
        void thinkOnOtherPlayerDoroped(int myPositionId, int dapaiActor, string pai, List<Tehai> tehais, List<Kawa> kawas, Field field);



        //ツモ順が来た時にどんなアクションするか
        //ツモ、打牌、暗槓、加槓、リーチのイベントを発生させる
        event SendHoraHandler SendHora;
        event SendDahaiHandler SendDahai;
        event SendAnkanHandler SendAnkan;
        event SendKakanHandler SendKakan;
        event SendReachHandler SendReach;
        void thinkOnTsumo(int myPositionId, string pai, List<Tehai> tehais, List<Kawa> kawas, Field field);

    }
}
