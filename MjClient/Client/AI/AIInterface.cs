using System;
using System.Collections.Generic;
using MjNetworkProtocolLibrary;
using MjModelLibrary;
using MjModelLibrary.Result;


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

    public delegate HoraResult CalcHoraHandler(int target, string pai);

    interface AIInterface {
        event SendPonHandler SendPon;
        event SendChiHandler SendChi;
        event SendDaiminkanHandler SendDaiminkan;
        event SendNoneHandler SendNone;
        /// <summary>
        /// think action is called when other player discard pai.
        /// AI must send hora, pon, chi, daiminkan or none message in this function.
        /// </summary>
        /// <param name="mypositionId">your ID</param>
        /// <param name="dapaiActor">player ID who doropped pai</param>
        /// <param name="pai">dopped pai name</param>
        /// <param name="tehais">all player's tehais</param>
        /// <param name="kawas">all player's discards</param>
        /// <param name="field">field infomation</param>
        void ThinkOnOtherPlayerDoroped(int myPositionId, int dapaiActor, string pai, List<Tehai> tehais, List<Kawa> kawas, Field field, List<InfoForResult> ifrs, Yama yama);



        event SendHoraHandler SendHora;
        event SendDahaiHandler SendDahai;
        event SendAnkanHandler SendAnkan;
        event SendKakanHandler SendKakan;
        event SendReachHandler SendReach;
        /// <summary>
        /// think action is called when player tumo pai.
        /// this function must send hora, ankan, kakan, reach or dahai message.
        /// </summary>
        /// <param name="mypositionId">my id</param>
        /// <param name="pai">tsumo pai</param>
        /// <param name="tehais">my tehai</param>
        /// <param name="kawas">all player kawa</param>
        /// <param name="field">field infomation</param>
        void ThinkOnMyTsumo(int myPositionId, string pai, List<Tehai> tehais, List<Kawa> kawas, Field field, List<InfoForResult> ifrs, Yama yama);



        //action after furo
        void ThinkOnFuroDahai(int mypositionId, string pai, List<Tehai> tehais, List<Kawa> kawas,
                         Field field, List<InfoForResult> ifrs, Yama yama);

        MJsonMessageDahai GetMessageBufferForRiachDahai();

        event CalcHoraHandler CalcHora;
    }
}
