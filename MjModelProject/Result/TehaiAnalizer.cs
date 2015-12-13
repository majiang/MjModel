using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MjModelProject.Result;
using MjModelProject.Util;

namespace MjModelProject.Model
{
    public static class TehaiAnalizer
    {

        public static SplitedTehai AnalizePattern(Tehai tehai)
        {
            int[] syu = new int[MJUtil.LENGTH_SYU_ALL];
            foreach (var pai in tehai.tehai)
            {
                syu[pai.PaiNumber]++;
            }
            TehaiSpliter ts = new TehaiSpliter();

           
            var splited = ts.SplitTehai(syu, tehai.furos);

            foreach (var furopai in tehai.furos)
            {
                switch (furopai.ftype)
                {
                    case MJUtil.TartsuType.MINSYUN:
                        splited.Syu[furopai.minPaiSyu]++;
                        splited.Syu[furopai.minPaiSyu + 1]++;
                        splited.Syu[furopai.minPaiSyu + 2]++;
                        break;
                    case MJUtil.TartsuType.MINKO:
                        splited.Syu[furopai.minPaiSyu] += 3;
                        break;
                    case MJUtil.TartsuType.MINKANTSU:
                        splited.Syu[furopai.minPaiSyu] += 4;
                        break;
                }
            }

            return splited;
        }


    }



    public class SplitedTehai
    {
        public List<HoraPattern> AllHoraPatternList { get; set; }
        public int[] Syu { get; set; }
        public SplitedTehai()
        {
            AllHoraPatternList = new List<HoraPattern>();
            Syu = new int[MJUtil.LENGTH_SYU_ALL];
        }
    }


    public class HoraPattern
    {
        public List<Tartsu> TartsuList;
        public List<Tartsu> WithoutHeadTartsuList;
        public Tartsu Head;

        public HoraPattern(List<Tartsu> horaPatternTartsuList)
        {
            TartsuList = new List<Tartsu>(horaPatternTartsuList);
            WithoutHeadTartsuList = TartsuList.Where(e => e.TartsuType != MJUtil.TartsuType.HEAD).ToList();
            Head = TartsuList.First(e => e.TartsuType == MJUtil.TartsuType.HEAD);
        }

        public void AddFuro(List<Tartsu> furoTartsuList)
        {
            TartsuList.AddRange(furoTartsuList);
            WithoutHeadTartsuList.AddRange(furoTartsuList);
        }

    }

    public class Tartsu
    {

        public MJUtil.TartsuType TartsuType { get; private set; }
        public int TartsuStartPaiSyu { get; private set; }

        public Tartsu(MJUtil.TartsuType tt, int tartsuStartPaiSyu)
        {
            this.TartsuType = tt;
            this.TartsuStartPaiSyu = tartsuStartPaiSyu;
        }

    }



    public class TehaiSpliter
    {
        static readonly int BLOCK_NUM = 5;
        static readonly int BLOCK_LENGTH = 2;
        static readonly int HORAMENTSU_TYPE = MJUtil.HORAMENTSU_TYPE;
        static readonly int HORAMENTSU_SYU = MJUtil.HORAMENTSU_SYU;



        public SplitedTehai SplitTehai(int[] syu, List<Furo> furos)
        {
            //全通りの上がり構成を算出
            List<HoraPattern> horaMentsuList = split(syu);

            //Make Furo Tartsu
            List<Tartsu> furoTartsu = new List<Tartsu>();
            foreach (var furo in furos)
            {
                furoTartsu.Add(new Tartsu(furo.ftype, furo.minPaiSyu));
            }
            //Add Furo
            foreach (var horaMentsu in horaMentsuList)
            {
                horaMentsu.AddFuro(furoTartsu);
            }

            //メンツ構造と種類枚数を持つオブジェクトを生成
            SplitedTehai horaMentsuData = new SplitedTehai();
            horaMentsuData.AllHoraPatternList = horaMentsuList;
            horaMentsuData.Syu = syu;

            return horaMentsuData;
        }



        private static List<HoraPattern> split(int[] syu)
        {
            List<Tartsu> work = new List<Tartsu>();
            List<HoraPattern> resultList = new List<HoraPattern>();
            int start = 0;
            for (int i = 0; i < MJUtil.LENGTH_SYU_ALL; i++)
            {
                if (syu[i] >= 2)
                {
                    syu[i] -= 2;
                    Tartsu ts = new Tartsu(MJUtil.TartsuType.HEAD, i);
                    work.Add(ts);
                    reSplit(start, syu, ref work, ref resultList);
                    work.Remove(ts);
                    syu[i] += 2;
                }
            }
            return resultList;
        }

        private static void reSplit(int start, int[] syu, ref List<Tartsu> work, ref List<HoraPattern> resultList)
        {

            if (syuIsZero(syu))
            {
                HoraPattern result = new HoraPattern(work);
                resultList.Add(result);
                return;
            }

            for (int i = start; i < MJUtil.LENGTH_SYU_ALL; i++)
            {

                if (syu[i] >= 3)
                {
                    syu[i] -= 3;
                    Tartsu ts = new Tartsu(MJUtil.TartsuType.ANKO, i);
                    work.Add(ts);
                    reSplit(i, syu, ref work, ref resultList);
                    work.Remove(ts);
                    syu[i] += 3;
                }
                if ((i <= 24) && ((syu[i] >= 1) && (syu[i + 1] >= 1) && (syu[i + 2] >= 1)))
                {
                    syu[i]--;
                    syu[i + 1]--;
                    syu[i + 2]--;
                    Tartsu ts = new Tartsu(MJUtil.TartsuType.ANSYUN, i);
                    work.Add(ts);
                    reSplit(i, syu, ref work, ref resultList);
                    work.Remove(ts);
                    syu[i]++;
                    syu[i + 1]++;
                    syu[i + 2]++;
                }

            }
        }
        private static bool syuIsZero(int[] syu)
        {
            int[] zeros = new int[MJUtil.LENGTH_SYU_ALL];
            return zeros.SequenceEqual(syu);
        }

    }
}
