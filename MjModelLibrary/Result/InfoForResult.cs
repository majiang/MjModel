using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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
        public int UseYamaPaiNum;
        public bool IsFured;// if player furoed, it will be true 
        public bool IsFuredOnField;// if all player or other player furoed, it will be true; 
        private int gameId;
        private List<int> doraMarkerList = new List<int>();
        private List<int> uradoraMarkerList = new List<int>();
        private int[] doraOmoteMultiple = new int[MJUtil.LENGTH_SYU_ALL];
        private int[] uradoraOmoteMultiple = new int[MJUtil.LENGTH_SYU_ALL];


        private int myPositionId;
        private List<Pai> jifuuList = new List<Pai>();
        private List<Pai> bakazeList = new List<Pai>();
        private static readonly List<Pai> WindList = new List<Pai>() { new Pai("E"), new Pai("S"), new Pai("W"), new Pai("N") }; 

        public InfoForResult(int gameId = 1, int myPositionId = 0, int oyaId = 0, string bakaze = "E")
        {
            this.gameId = gameId;
            var zeroIdxGameId = gameId - 1;
            this.myPositionId = myPositionId;
            IsOya = oyaId == myPositionId;

            bakazeList.Add(WindList.Where(e => e.PaiString == bakaze).First());

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


        public bool IsDora(int id)
        {    
           return GetDoraMultiple(id) > 0;
        }

        public int GetDoraMultiple(int id)
        {
            var targetDoraOmote = MJUtil.GetDoraOmote(id);
            if (IsReach || IsDoubleReach)
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
        public bool Isbakaze(string pai)
        {
            return bakazeList.Count(e => e.PaiString == pai) > 0;
        }

        public bool IsJifuu(int paiId)
        {
            return jifuuList.Count(e => e.PaiNumber == paiId) > 0;
        }
        public bool Isbakaze(int paiId)
        {
            return bakazeList.Count(e => e.PaiNumber == paiId) > 0;
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
            return (1 <= UseYamaPaiNum && UseYamaPaiNum <= 4) && IsFuredOnField == false;
        }


    }
}
