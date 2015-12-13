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
        private List<int> doraMarkerList = new List<int>();
        private List<int> uradoraMarkerList = new List<int>();
        private List<int> jifuuList = new List<int>();
        private List<int> bafuuList = new List<int>();
         

        public bool IsJifuu(int paiid)
        {
            return jifuuList.Contains(paiid);
        }
        public bool IsBafuu(int paiid)
        {
            return bafuuList.Contains(paiid);
        }
        public void RegisterDora()
        {

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

    }
}
