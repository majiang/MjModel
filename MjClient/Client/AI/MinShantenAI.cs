using System;
using System.Collections.Generic;
using System.Linq;
using MjNetworkProtocol;
using MjModelLibrary;

namespace MjClient
{
    class MinShantenAI : AIBase
    {
        ShantenCalclator shantenCalclator = ShantenCalclator.GetInstance();

        public override MjsonMessageBase thinkAction(int mypositionId, int dapaiActor, string pai, List<Tehai> tehais, List<Kawa> kawas, Field field)
        {
            return new MJsonMessageNone();
        }

        public override MjsonMessageBase thinkDahai(int mypositionId, string pai, List<Tehai> tehais, List<Kawa> kawas, Field field)
        {
            var paiString = CalcMinShantenPai(mypositionId, pai, tehais, kawas, field);
            return new MJsonMessageDahai(mypositionId, paiString, pai == paiString);
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


    }
}
