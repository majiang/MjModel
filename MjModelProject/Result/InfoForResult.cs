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
        public bool IsMenzen { get; set; }
        public bool IsTsumo { get; set; }
        public bool IsIppatsu { get; set; }
        public bool IsHaitei { get; set; }
        public bool IsHoutei { get; set; }
        public bool IsRinshan { get; set; }
        public bool IsChankan { get; set; }
        public int LastAddedSyu { get; set; }
        public bool IsReach { get; set; }
        public bool IsDoubleReach { get; set; }
        public bool IsOya { get; set; }
        public int PassedTurn { get; set; }
        public bool IsFured { get; set; }
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
    }
}
