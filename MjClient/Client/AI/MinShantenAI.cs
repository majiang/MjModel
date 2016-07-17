using System;
using System.Collections.Generic;
using System.Linq;
using MjNetworkProtocolLibrary;
using MjModelLibrary.Result;
using MjModelLibrary;
using System.Diagnostics;

namespace MjClient.AI
{
    class MinShantenAI : AIInterface
    {
        public event CalcHoraHandler CalcHora;

        ShantenCalclator shantenCalclator = ShantenCalclator.GetInstance();
        

        public event SendPonHandler SendPon;
        public event SendChiHandler SendChi;
        public event SendDaiminkanHandler SendDaiminkan;
        public event SendNoneHandler SendNone;



        public void ThinkOnOtherPlayerDoroped(int mypositionId, int dapaiActor, string pai, List<Tehai> tehais,
                                              List<Kawa> kawas, Field field, List<InfoForResult> ifrs, Yama yama)
        {
            if (shantenCalclator.CalcShanten(tehais[mypositionId], pai) != MJUtil.SHANTEN_HORA)
            {
                SendNone(new MJsonMessageNone());
                return;
            }


            // check yaku
            var tempSave = ifrs[mypositionId].LastAddedPai;
            ifrs[mypositionId].LastAddedPai = new Pai(pai);

            var myTehai = tehais[mypositionId];
            var result = CalcHora(dapaiActor, pai);
            if (result.yakuResult.HasYakuExcludeDora)
            {
                Debug.WriteLine("on other player doroped");
                Debug.WriteLine(myTehai.ToString());
                result.yakuResult.yakus.ForEach(e => Debug.Write(e[0] + ","));
                Debug.WriteLine("");
                Debug.WriteLine("--------------------------------------------------------------------------------");

                SendHora(new MJsonMessageHora(mypositionId, dapaiActor, pai));
                return;
            }

            ifrs[mypositionId].LastAddedPai = tempSave;
            SendNone(new MJsonMessageNone());
        }






        public event SendHoraHandler SendHora;
        public event SendDahaiHandler SendDahai;
        public event SendAnkanHandler SendAnkan;
        public event SendKakanHandler SendKakan;
        public event SendReachHandler SendReach;
        public MJsonMessageDahai MessagebufferForReach;
        public void ThinkOnMyTsumo(int mypositionId, string tsumopai, List<Tehai> tehais, List<Kawa> kawas,
                                 Field field, List<InfoForResult> ifrs, Yama yama)
        {
            var myTehai = tehais[mypositionId];
        
            if (myTehai.IsHora())
            {
                var result = CalcHora(mypositionId, tsumopai);

                if (result.yakuResult.HasYakuExcludeDora)
                {
                    Debug.WriteLine("on tsumo");
                    Debug.WriteLine(myTehai.ToString());
                    result.yakuResult.yakus.ForEach(e => Debug.Write(e[0]+","));
                    Debug.WriteLine("");
                    Debug.WriteLine("--------------------------------------------------------------------------------");
                    SendHora(new MJsonMessageHora(mypositionId, mypositionId, tsumopai));
                    CalcHora(mypositionId, tsumopai);
                    
                    return;
                }

            }


            var dahaiPaiString = CalcMinShantenPai(mypositionId, tehais, kawas, field);
            
            if (CanReach(tehais[mypositionId],ifrs[mypositionId],yama))
            {
                
                MessagebufferForReach = new MJsonMessageDahai(mypositionId,dahaiPaiString, dahaiPaiString == tsumopai);
                SendReach(new MJsonMessageReach(mypositionId));
                return;
            }

            SendDahai(new MJsonMessageDahai(mypositionId, dahaiPaiString, dahaiPaiString == tsumopai));

        }



        public void ThinkOnFuroDahai(int mypositionId, string pai, List<Tehai> tehais, List<Kawa> kawas,
                                 Field field, List<InfoForResult> ifrs, Yama yama)
        {
            var myTehai = tehais[mypositionId];

            var dahaiPaiString = CalcMinShantenPai(mypositionId, tehais, kawas, field);

            SendDahai(new MJsonMessageDahai(mypositionId, dahaiPaiString, dahaiPaiString == pai));

        }


        private string CalcMinShantenPai(int mypositionId, List<Tehai> tehais, List<Kawa> kawas, Field field)
        {
            
            var syu = new int[MJUtil.LENGTH_SYU_ALL];
            foreach (var p in tehais[mypositionId].tehai)
            {
                syu[p.PaiNumber]++;
            }
            var resultDict = new Dictionary<int, int>();
            foreach (var paiId in syu.Select((value, index) => new { value, index }))
            {
                if (paiId.value == 0)
                {
                    continue;
                }

                syu[paiId.index]--;
                resultDict.Add(paiId.index, shantenCalclator.CalcShantenWithFuro(syu, tehais[mypositionId].furos.Count));
                syu[paiId.index]++;
            }
            var bestShanten = resultDict.OrderBy(e => e.Value).First().Value;
            var bestNumber = resultDict.Where(e => e.Value == bestShanten).OrderBy( e => Guid.NewGuid() ).First().Key;
            var paiString = tehais[mypositionId].tehai.Where(e => e.PaiNumber == bestNumber).First().PaiString;


            return paiString;
        }

        bool CanReach(Tehai tehai, InfoForResult infoForResult, Yama yama)
        {
            return ( tehai.IsTenpai() || tehai.IsHora() )
            && tehai.IsMenzen()
            && (infoForResult.IsReach == false && infoForResult.IsDoubleReach == false)
            && (yama.GetRestYamaNum() >= Constants.PLAYER_NUM);
        }

        public MJsonMessageDahai GetMessageBufferForRiachDahai()
        {
            return MessagebufferForReach;
        }


    }
}
