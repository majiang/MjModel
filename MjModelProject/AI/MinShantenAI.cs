using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MjModelProject.Result;
using MjModelProject.Util;

namespace MjModelProject.AI
{
    class MinShantenAI : AIBase
    {
        ShantenCalclator shantenCalclator = new ShantenCalclator();

        public override MjsonMessageBase thinkAction(int dapaiActor, string pai, List<Tehai> tehais, int AIPositionId, List<Kawa> kawas, Field field)
        {
            return new MJsonMessageNone();
        }

        public override MjsonMessageBase thinkDahai(int mypositionId, string pai, List<Tehai> tehais, int AIPositionId, List<Kawa> kawas, Field field)
        {

            var syu = new int[MJUtil.LENGTH_SYU_ALL];
            foreach (var p in tehais[mypositionId].tehai)
            {
                syu[p.PaiNumber]++;
            }
            var resultDict = new Dictionary<int, int>();
            foreach (var id in syu.Select((value, index) => new { value, index }))
            {
                if (id.value == 0)
                {
                    continue;
                }

                syu[id.index]--;
                resultDict.Add(id.index, shantenCalclator.CalcShantenWithFuro(syu, tehais[mypositionId].furos.Count));
                syu[id.index]++;
            }
            var best = resultDict.OrderBy(e => e.Value).Single().Key;
            var paiString = tehais[mypositionId].tehai.Where(e => e.PaiNumber == best).First().PaiString;

            return new MJsonMessageDahai(mypositionId, paiString, pai == paiString);

        }


        private string CalcMinShantenPai(string pai, Tehai tehai)
        {
            var bestPai = "";



            return bestPai;
        }


    }
}
