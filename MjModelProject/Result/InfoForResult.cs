using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MjModelProject.Util;

namespace MjModelProject.Result
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
        private int myPositionId;
        private List<Pai> jifuuList = new List<Pai>();
        private List<Pai> bafuuList = new List<Pai>();
        private static readonly List<Pai> WindList = new List<Pai>() { new Pai("E"), new Pai("S"), new Pai("W"), new Pai("N") }; 

        public InfoForResult(int gameId = 0, int myPositionId = 0)
        {
            this.gameId = gameId;
            this.myPositionId = myPositionId;
            IsOya = (gameId % 4) == myPositionId;

            var bafuuIndex = gameId / 4;
            bafuuList.Add(WindList[bafuuIndex]);

            var jifuuIndex = (myPositionId - (gameId % 4) + 4) % 4;
            jifuuList.Add(WindList[jifuuIndex]);
        }

        public void RegisterDora(string paiString)
        {
            doraMarkerList.Add(PaiConverter.STRING_TO_ID[paiString]);
        }

        public bool IsDora(int syu) { 
            //リーチしてたら裏ドラも考慮
            var targetDoraOmote = MJUtil.GetDoraOmote(syu);
            if ( IsReach )
            {
                return uradoraMarkerList.Contains( targetDoraOmote ) || doraMarkerList.Contains( targetDoraOmote );
            }

            //リーチしてない場合は表ドラだけ考慮
            return doraMarkerList.Contains(targetDoraOmote);
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
                    doraSummation += element.value;
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
            return jifuuList.Count(e => e.PaiString == pai) > 0;
        }

        public bool IsJifuu(int paiId)
        {
            return jifuuList.Count(e => e.PaiNumber == paiId) > 0;
        }
        public bool IsBafuu(int paiId)
        {
            return jifuuList.Count(e => e.PaiNumber == paiId) > 0;
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
