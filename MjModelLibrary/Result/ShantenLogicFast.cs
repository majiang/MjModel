using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjModelLibrary.Result
{
    class ShantenLogicFast :  IShantenLogic
    {
        static uint[] tartsuNumMap = new uint[(uint)Math.Pow(5, 9)];

        public ShantenLogicFast()
        {
            SetUp();
        }

        void SetUp()
        {
            var inputPath = "../../shaneten.dat";
            using (StreamReader sr = new StreamReader(inputPath))
            {
                var line = sr.ReadLine();
                while (line != null && line != string.Empty)
                {

                    var args = line.Split(' ');


                    var index = parce10base(Int32.Parse(args[0]));

                    // line represents {index_in5base, mentsuNum_coefficient2, tartsuNum_coef2, mentsNum_coef10, tartsuNum,coef10}
                    var mentsuNum_coef2 = uint.Parse(args[1]);
                    var tartsuNum_coef2 = uint.Parse(args[2]);
                    var mentsuNum_coef10 = uint.Parse(args[3]);
                    var tartsuNum_coef10 = uint.Parse(args[4]);
                    uint combined = (mentsuNum_coef2 << 24)
                                  + (tartsuNum_coef2 << 16)
                                  + (mentsuNum_coef10 << 8)
                                  + (tartsuNum_coef10);

                    tartsuNumMap[index] = combined;


                    line = sr.ReadLine();
                }
            }
        }


        private int[] FastCountBaseMentsuNum(int[] numberPai)
        {
            var result = new int[4];
            var combined = tartsuNumMap[parce10base(numberPai)];
            var mask_8bit = (uint)Math.Pow(2, 8) - 1;
            result[0] = (int)((combined >> 24) & mask_8bit);
            result[1] = (int)((combined >> 16) & mask_8bit);
            result[2] = (int)((combined >> 8) & mask_8bit);
            result[3] = (int)((combined) & mask_8bit);
            return result;
        }

        int parce10base(int input5base)
        {
            var rawInput = input5base;
            var result = 0;

            for (int i = 8; i > 0; i--)
            {
                var div = input5base / (int)Math.Pow(10, i);
                result += (int)Math.Pow(5, i) * div;
                input5base -= div * (int)Math.Pow(10, i);
            }
            result += input5base;
            return result;
        }

        int parce10base(int[] input5base)
        {
            var rawInput = input5base;
            var result = 0;

            for (int i = 8; i > 0; i--)
            {
                var div = input5base[i];
                result += (int)Math.Pow(5, i) * div;
            }
            result += input5base[0];
            return result;
        }




        const int SYU_LENGTH = 9;
        const int JI_LENGTH = 7;
        const int SHANTEN_MAX_NORMAL = 8;
        const int SHANTEN_MAX_CHITOITSU = 6;
        const int SHANTEN_MAX_KOKUSHI = 13;
        const int CHITOITSU_SYU_NUM = 7;
        bool useChitoitsu = true;
        bool useKokushi = true;

        public int CalcShanten(int[] syu)
        {
            return CalcShantenWithFuro(syu, 0);
        }

        public int CalcShanten(Tehai tehai, string pai)
        {
            var syu = new int[MJUtil.LENGTH_SYU_ALL];
            foreach (var p in tehai.tehai)
            {
                syu[p.PaiNumber]++;
            }
            syu[PaiConverter.STRING_TO_ID[pai]]++;
            return CalcShantenWithFuro(syu, tehai.furos.Count);
        }


        public int CalcShanten(Tehai tehai)
        {
            var syu = new int[MJUtil.LENGTH_SYU_ALL];
            foreach (var p in tehai.tehai)
            {
                syu[p.PaiNumber]++;
            }

            return CalcShantenWithFuro(syu, tehai.furos.Count);
        }

        public int CalcShantenWithFuro(int[] syu, int furoNum)
        {

            int[] workSyu = new int[syu.Length];
            Array.Copy(syu, workSyu, syu.Length);


            int minSyantenWithHead = CalcMentsuteShanten(workSyu, furoNum);

            int minSyantenWithoutHead = SHANTEN_MAX_NORMAL;
            for (int i = 0; i < workSyu.Length; i++)
            {
                if (workSyu[i] >= 2)
                {
                    workSyu[i] -= 2;
                    int tempMin = CalcMentsuteShanten(workSyu, furoNum) - 1;
                    if (minSyantenWithoutHead > tempMin)
                    {
                        minSyantenWithoutHead = tempMin;
                    }
                    workSyu[i] += 2;
                }
            }

            int syantenChitoitsu = SHANTEN_MAX_CHITOITSU;
            int syantenKokushi = SHANTEN_MAX_KOKUSHI;
            if (furoNum == 0)
            {
                if (useChitoitsu)
                {
                    syantenChitoitsu = CalcChitoitsuShanten(syu);
                }
                if (useKokushi)
                {
                    syantenKokushi = CalcKokushiShanten(syu);
                }
            }
            int syantenNotNormal = Math.Min(syantenChitoitsu, syantenKokushi);
            int syantenNormal = Math.Min(minSyantenWithHead, minSyantenWithoutHead);

            return Math.Min(syantenNotNormal, syantenNormal);
        }

        public int CalcChitoitsuShanten(int[] syu)
        {
            int shanten = SHANTEN_MAX_CHITOITSU;
            int syuNum = 0;


            for (int i = 0; i < syu.Length; i++)
            {
                if (syu[i] >= 1)
                {
                    syuNum++;
                    if (syu[i] >= 2)
                    {
                        shanten--;
                    }
                }
            }
            // ex.111122255m55577p > return 2 (by chitoitsu)
            if (CHITOITSU_SYU_NUM - syuNum > shanten)
            {
                shanten = CHITOITSU_SYU_NUM - syuNum;
            }
            return shanten;
        }

        public int CalcKokushiShanten(int[] syu)
        {
            int shanten = SHANTEN_MAX_KOKUSHI;
            bool hasHead = false;

            //Kokushi effective tiles
            int[] target = { 0, 8, 9, 17, 18, 26, 27, 28, 29, 30, 31, 32, 33 };
            

            for (int i = 0; i < target.Length; i++)
            {
                if (syu[(target[i])] >= 1)
                {
                    shanten--;
                }

                if ((syu[(target[i])] >= 2))
                {
                    hasHead = true;
                }
            }
            if (hasHead)
            {
                shanten--;
            }

            return shanten;
        }

        public int CalcMentsuteShanten(int[] syu, int furoNum)
        {
            int minSyanten = SHANTEN_MAX_NORMAL;



            int[] man = new int[SYU_LENGTH];
            int[] pin = new int[SYU_LENGTH];
            int[] sou = new int[SYU_LENGTH];
            int[] ji = new int[JI_LENGTH];

            Array.Copy(syu, 0, man, 0, SYU_LENGTH);
            Array.Copy(syu, SYU_LENGTH, pin, 0, SYU_LENGTH);
            Array.Copy(syu, SYU_LENGTH * 2, sou, 0, SYU_LENGTH);
            Array.Copy(syu, SYU_LENGTH * 3, ji, 0, JI_LENGTH);


            // Result contains values as follws > { mentsuNum_coefficient2, kouhoNum_coef2, mentsNum_coef10, kouhoNum,coef10 }
            // reference > http://mahjong.ara.black/etc/shanten/shanten7.htm
            int[] manResult = FastCountBaseMentsuNum(man);
            int[] pinResult = FastCountBaseMentsuNum(pin);
            int[] souResult = FastCountBaseMentsuNum(sou);

            int[] jiResult = CalcMentsuJi(ji);




            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    for (int k = 0; k < 2; k++)
                    {

                        int mentsNum = manResult[i * 2] + pinResult[j * 2] + souResult[k * 2] + jiResult[0];
                        int kouhoNum = manResult[i * 2 + 1] + pinResult[j * 2 + 1] + souResult[k * 2 + 1] + jiResult[1];
                        if ((mentsNum + kouhoNum + furoNum) > 4)
                        {
                            kouhoNum = 4 - mentsNum - furoNum;
                        }
                        int tempSyanten = SHANTEN_MAX_NORMAL - mentsNum * 2 - furoNum * 2 - kouhoNum;
                        if (minSyanten > tempSyanten)
                        {
                            minSyanten = tempSyanten;
                        }
                    }
                }
            }


            return minSyanten;
        }


        private int[] CalcMentsuJi(int[] ji)
        {
            int[] result = new int[2];

            for (int i = 0; i < ji.Length; i++)
            {
                if (ji[i] >= 3)
                {
                    result[0]++;
                }
                else if (ji[i] >= 2)
                {
                    result[1]++;
                }
            }
            return result;
        }

        public void UseChitoitsu(bool useChitoitsu)
        {
            this.useChitoitsu = useChitoitsu;
        }

        public void UseKokushi(bool useKokushi)
        {
            this.useKokushi = useKokushi;
        }

    }
}
