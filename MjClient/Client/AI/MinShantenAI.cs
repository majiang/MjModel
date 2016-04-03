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
        public event SendReachHandler SendReach;

        public MJsonMessageDahai MessagebufferForReach;



        ShantenCalclator shantenCalclator = ShantenCalclator.GetInstance();



        /// <summary>
        /// think action after other player.
        /// this function must send hora, pon, chi, daiminkan or none message.
        /// </summary>
        /// <param name="mypositionId">your ID</param>
        /// <param name="dapaiActor">player ID who doropped pai</param>
        /// <param name="pai">dopped pai name</param>
        /// <param name="tehais">all player's tehais</param>
        /// <param name="kawas">all player's discards</param>
        /// <param name="field">field infomation</param>
        public void thinkOnOtherPlayerDoroped(int mypositionId, int dapaiActor, string pai, List<Tehai> tehais,
                                              List<Kawa> kawas, Field field, List<InfoForResult> ifrs)
        {
            if(ifrs[mypositionId].IsReach && shantenCalclator.CalcShanten(tehais[mypositionId], pai) == -1)
            {
                SendHora(new MJsonMessageHora(mypositionId,dapaiActor, pai));
                return;
            }

            SendNone(new MJsonMessageNone());
        }

        /// <summary>
        /// think action after my tumo.
        /// this function must send hora, ankan, kakan, reach or dahai message.
        /// </summary>
        /// <param name="mypositionId">my id</param>
        /// <param name="pai">tsumo pai</param>
        /// <param name="tehais">my tehai</param>
        /// <param name="kawas">all player kawa</param>
        /// <param name="field">field infomation</param>
        public void thinkOnTsumo(int mypositionId, string pai, List<Tehai> tehais, List<Kawa> kawas,
                                 Field field, List<InfoForResult> ifrs)
        {
            var myTehai = tehais[mypositionId];
        
            if (myTehai.IsHora())
            {
                SendHora(new MJsonMessageHora(mypositionId, mypositionId, pai));
                return;
            }


            var dahaiPaiString = CalcMinShantenPai(mypositionId, pai, tehais, kawas, field);
            
            if (myTehai.IsTenpai() && ifrs[mypositionId].IsReach == false)
            {
                MessagebufferForReach = new MJsonMessageDahai(mypositionId,dahaiPaiString,false);
                SendReach(new MJsonMessageReach(mypositionId));
                return;
            }

            SendDahai(new MJsonMessageDahai(mypositionId, dahaiPaiString, dahaiPaiString == pai));

        }
        public void thinkOnFuroDahai(int mypositionId, string pai, List<Tehai> tehais, List<Kawa> kawas,
                                 Field field, List<InfoForResult> ifrs)
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
