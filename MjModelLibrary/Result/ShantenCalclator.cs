using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MjModelLibrary.Result
{
    public class ShantenCalclator
    {

        private static ShantenCalclator ss;
        IShantenLogic sb = new ShantenLogicFast();

        private ShantenCalclator() { }


        public static ShantenCalclator GetInstance()
        {
            if (ss == null)
            {
                ss = new ShantenCalclator();
            }

            return ss;
        }

        public int CalcShanten(int[] syu)
        {
            return sb.CalcShantenWithFuro(syu, 0);
        }

        public int CalcShanten(Tehai tehai, string pai)
        {
            return sb.CalcShanten(tehai, pai);
        }


        public int CalcShanten(Tehai tehai)
        {
            return sb.CalcShanten(tehai);
        }

        public int CalcShantenWithFuro(int[] syu, int furoNum)
        {
            return sb.CalcShantenWithFuro(syu, furoNum);
        }
        public void SetUseChitoitsu(bool useChitoitsu)
        {
            sb.UseChitoitsu(useChitoitsu);
        }
        public void SetUseKokushi(bool useKokushi)
        {
            sb.UseKokushi(useKokushi);
        }
        public void CalcMentsuteShanten(int[] syu, int furoNum)
        {
            sb.CalcMentsuteShanten(syu, furoNum);
        }
        public void CalcChitoitsuShanten(int[] syu)
        {
            sb.CalcChitoitsuShanten(syu);
        }
        public void CalcKokushiShanten(int[] syu)
        {
            sb.CalcKokushiShanten(syu);
        }

    }
}
