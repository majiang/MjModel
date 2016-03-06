using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MjServer.Util;

namespace MjModelLibrary
{

    public class InfoForResult
    {
        public bool IsMenzen;
        public bool IsTsumo;
        public bool IsIppatsu;
        public bool IsHaitei;
        public bool IsHoutei;
        public bool IsRinshan;
        public bool IsChankan;
        public Pai LastAddedPai;
        public bool IsReach;
        public bool IsDoubleReach;
        public bool IsOya;
        public int PassedTurn;
        public bool IsFured;//for player 
        public bool IsFuredOnField;
        private int gameId;
        private List<int> doraMarkerList = new List<int>();
        private List<int> uradoraMarkerList = new List<int>();
        private int[] doraOmoteMultiple = new int[MJUtil.LENGTH_SYU_ALL];
        private int[] uradoraOmoteMultiple = new int[MJUtil.LENGTH_SYU_ALL];


        private int myPositionId;
        private List<Pai> jifuuList = new List<Pai>();
        private List<Pai> bafuuList = new List<Pai>();
        private static readonly List<Pai> WindList = new List<Pai>() { new Pai("E"), new Pai("S"), new Pai("W"), new Pai("N") }; 

        public InfoForResult(int gameId = 1, int myPositionId = 0)
        {
            //GameIdを1indexから0indexへ変更


            this.gameId = gameId;
            var zeroIdxGameId = gameId - 1;
            this.myPositionId = myPositionId;
            IsOya = (zeroIdxGameId % 4) == myPositionId;

            var bafuuIndex = zeroIdxGameId / 4;
            bafuuList.Add(WindList[bafuuIndex]);

            var jifuuIndex = (myPositionId - (zeroIdxGameId % 4) + 4) % 4;
            jifuuList.Add(WindList[jifuuIndex]);
        }

        public void RegisterDoraMarker(string paiString)
        {
            doraMarkerList.Add(PaiConverter.STRING_TO_ID[paiString]);
            doraOmoteMultiple[PaiConverter.STRING_TO_ID[paiString]]++;

        }
        public void RegisterUraDoraMarker(List<string> paiStringList)
        {
            foreach (var paiString in paiStringList)
            {
                uradoraMarkerList.Add(PaiConverter.STRING_TO_ID[paiString]);
                uradoraOmoteMultiple[PaiConverter.STRING_TO_ID[paiString]]++;
            }
        }


        public bool IsDora(int id) { 

             //リーチしてたら表ドラと裏ドラを考慮
            var targetDoraOmote = MJUtil.GetDoraOmote(id);
            if ( IsReach )
            {
                return doraOmoteMultiple[targetDoraOmote] > 0
                    || uradoraOmoteMultiple[targetDoraOmote] > 0;
            }
            //リーチしていない場合表ドラを考慮
            return doraOmoteMultiple[targetDoraOmote] > 0;
        }

        public int GetDoraMultiple(int id)
        {
            var targetDoraOmote = MJUtil.GetDoraOmote(id);
            if (IsReach)
            {
                return doraOmoteMultiple[targetDoraOmote] + uradoraOmoteMultiple[targetDoraOmote];
            }
            //リーチしていない場合表ドラを考慮
            return doraOmoteMultiple[targetDoraOmote];
        }

        public int CalcDoraNum(int[] syu)
        {
            var doraSummation = 0;

            foreach (var element in syu.Select((value, index) => new { value, index }))
            {

                if (element.value == 0)
                {
                    continue;
                }

                if (IsDora(element.index))
                {
                    doraSummation += element.value * GetDoraMultiple(element.index);
                }
            }

            return doraSummation;
        }


        public bool IsJifuu(string pai)
        {
            return jifuuList.Count( e => e.PaiString == pai) > 0;
        }
        public bool IsBafuu(string pai)
        {
            return bafuuList.Count(e => e.PaiString == pai) > 0;
        }

        public bool IsJifuu(int paiId)
        {
            return jifuuList.Count(e => e.PaiNumber == paiId) > 0;
        }
        public bool IsBafuu(int paiId)
        {
            return bafuuList.Count(e => e.PaiNumber == paiId) > 0;
        }

        public void SetLastAddedPai(Pai pai)
        {
            LastAddedPai = pai;
        }
        public void SetLastAddedPai(int id)
        {
            LastAddedPai = new Pai(id);
        }

        public void SetLastAddedPai(string paiString)
        {
            LastAddedPai = new Pai(paiString);
        }

        public int GetLastAddedSyu()
        {
            return LastAddedPai.PaiNumber;
        }

        public bool IsFirstTurn()
        {
            return (0 <= PassedTurn && PassedTurn <= 3) && IsFuredOnField == false;
        }


    }
}
