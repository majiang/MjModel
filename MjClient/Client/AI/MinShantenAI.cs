using System;
using System.Collections.Generic;
using System.Linq;
using MjNetworkProtocol;
using MjModelLibrary;

namespace MjClient.AI
{
    class MinShantenAI : AIInterface
    {
        public event SendPonHandler SendPon;
        public event SendChiHandler SendChi;
        public event SendDaiminkanHandler SendDaiminkan;
        public event SendNoneHandler SendNone;

        public event SendHoraHandler SendHora;
        public event SendDahaiHandler SendDahai;
        public event SendAnkanHandler SendAnkan;
        public event SendKakanHandler SendKakan;



        ShantenCalclator shantenCalclator = ShantenCalclator.GetInstance();

        public void thinkOnOtherPlayerDoroped(int mypositionId, int dapaiActor, string pai, List<Tehai> tehais, List<Kawa> kawas, Field field)
        {
            SendNone();
        }

        public void thinkOnTsumo(int mypositionId, string pai, List<Tehai> tehais, List<Kawa> kawas, Field field)
        {
            var nowShanten = tehais[mypositionId].GetShanten();


            if (nowShanten == -1)
            {
                SendHora(new MJsonMessageHora(mypositionId, mypositionId, pai));
                return;
            }


            var dahaiPaiString = CalcMinShantenPai(mypositionId, pai, tehais, kawas, field);

            if (nowShanten == 0){

            }


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
                // if pai num is 0, go next pai
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


    }
}
