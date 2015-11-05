using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjModelProject.Result
{

    public class InfoForResult
    {
        public bool IsMenzen;
        public bool IsTsumo;
        public bool IsIppatsu;
        public int LastAddedSyu;
        public bool IsReach;
        public bool IsDoubleReach;
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
        public void registerDora()
        {

        }
    }
}
