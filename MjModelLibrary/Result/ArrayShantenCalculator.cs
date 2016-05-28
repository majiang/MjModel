using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MjModelLibrary.Result
{
    public class ArrayShantenCalculator
    {

        private static ArrayShantenCalculator ss;
        IShantenLogic sf = new ShantenLogicFast();

        private ArrayShantenCalculator() { }
        
        public static ArrayShantenCalculator GetInstance()
        {
            if (ss == null)
            {
                ss = new ArrayShantenCalculator();
            }

            return ss;
        }

        public int CalcShanten(int[] syu)
        {
            return sf.CalcShantenWithFuro(syu, 0);
        }

        public int CalcShanten(Tehai tehai, string pai)
        {
            return sf.CalcShanten(tehai, pai);
        }


        public int CalcShanten(Tehai tehai)
        {
            return sf.CalcShanten(tehai);
        }

        public int CalcShantenWithFuro(int[] syu, int furoNum)
        {
            return sf.CalcShantenWithFuro(syu, furoNum);
        }

        public void SetUseChitoitsu(bool useChitoitsu)
        {
            sf.UseChitoitsu(useChitoitsu);
        }
        public void SetUseKokushi(bool useKokushi)
        {
            sf.UseKokushi(useKokushi);
        }
    }
}




