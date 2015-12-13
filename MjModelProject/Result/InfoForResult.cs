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
        public int LastAddedSyu;
        public bool IsReach;
        public bool IsDoubleReach;
        public bool IsOya;
        public List<int> doraMarkerList = new List<int>();
        public List<int> uradoraMarkerList = new List<int>();
        public List<int> jifuuList = new List<int>();
        public List<int> bafuuList = new List<int>();
         

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

    }
}
