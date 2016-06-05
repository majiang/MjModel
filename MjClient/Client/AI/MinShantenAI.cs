using System;
using System.Collections.Generic;
using System.Linq;
using MjNetworkProtocolLibrary;
using MjModelLibrary.Result;
using MjModelLibrary;


namespace MjClient.AI
{
    class MinShantenAI : AIInterface
    {

        ShantenCalclator shantenCalclator = ShantenCalclator.GetInstance();

        public event SendPonHandler SendPon;
        public event SendChiHandler SendChi;
        public event SendDaiminkanHandler SendDaiminkan;
        public event SendNoneHandler SendNone;



        public void ThinkOnOtherPlayerDoroped(int mypositionId, int dapaiActor, string pai, List<Tehai> tehais,
                                              List<Kawa> kawas, Field field, List<InfoForResult> ifrs, Yama yama)
        {
            if((ifrs[mypositionId].IsReach || ifrs[mypositionId].IsDoubleReach) && shantenCalclator.CalcShanten(tehais[mypositionId], pai) == -1)
            {
                SendHora(new MJsonMessageHora(mypositionId,dapaiActor, pai));
                return;
            }

            SendNone(new MJsonMessageNone());
        }






        public event SendHoraHandler SendHora;
        public event SendDahaiHandler SendDahai;
        public event SendAnkanHandler SendAnkan;
        public event SendKakanHandler SendKakan;
        public event SendReachHandler SendReach;
        public MJsonMessageDahai MessagebufferForReach;
        public void ThinkOnMyTsumo(int mypositionId, string pai, List<Tehai> tehais, List<Kawa> kawas,
                                 Field field, List<InfoForResult> ifrs, Yama yama)
        {
            var myTehai = tehais[mypositionId];
        
            if (myTehai.IsHora())
            {
                SendHora(new MJsonMessageHora(mypositionId, mypositionId, pai));
                return;
            }


            var dahaiPaiString = CalcMinShantenPai(mypositionId, pai, tehais, kawas, field);
            
            if (CanReach(tehais[mypositionId],ifrs[mypositionId],yama))
            {
                
                MessagebufferForReach = new MJsonMessageDahai(mypositionId,dahaiPaiString, dahaiPaiString == pai);
                SendReach(new MJsonMessageReach(mypositionId));
                return;
            }

            SendDahai(new MJsonMessageDahai(mypositionId, dahaiPaiString, dahaiPaiString == pai));

        }



        public void ThinkOnFuroDahai(int mypositionId, string pai, List<Tehai> tehais, List<Kawa> kawas,
                                 Field field, List<InfoForResult> ifrs, Yama yama)
        {
            var myTehai = tehais[mypositionId];

            var dahaiPaiString = CalcMinShantenPai(mypositionId, pai, tehais, kawas, field);

            SendDahai(new MJsonMessageDahai(mypositionId, dahaiPaiString, dahaiPaiString == pai));

        }


        private string CalcMinShantenPai(int mypositionId, string pai, List<Tehai> tehais, List<Kawa> kawas, Field field)
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
            var bestPaiIndex = resultDict.OrderBy(e => e.Value).First().Key;
            var paiString = tehais[mypositionId].tehai.Where(e => e.PaiNumber == bestPaiIndex).First().PaiString;


            return paiString;
        }

        bool CanReach(Tehai tehai, InfoForResult infoForResult, Yama yama)
        {
            return ( tehai.IsTenpai() || tehai.IsHora() )
            && tehai.IsMenzen()
            && (infoForResult.IsReach == false && infoForResult.IsDoubleReach == false)
            && (yama.GetRestYamaNum() >= Constants.PLAYER_NUM);
        }

        MJsonMessageDahai GetMessageBufferForRiachDahai()
        {
            return MessagebufferForReach;
        }
    }
}
