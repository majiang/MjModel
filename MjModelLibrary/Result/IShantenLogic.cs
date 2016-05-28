using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjModelLibrary.Result
{
    interface IShantenLogic
    {
        int CalcShanten(int[] syu);

        int CalcShanten(Tehai tehai, string pai);
        
        int CalcShanten(Tehai tehai);
        
        int CalcShantenWithFuro(int[] syu, int furoNum);

        void UseChitoitsu(bool useChitoitsu);
        void UseKokushi(bool useKokushi);

    }
}
