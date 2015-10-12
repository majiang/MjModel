using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MjModelProject.Result;
using MjModelProject.Util;

namespace MjModelProject.Model
{
    public class TehaiAnalizer
    {
        Tehai tehai;

        public TehaiAnalizer(Tehai th)
        {
            tehai = th;
        }

        public SplitedTehai AnalizePattern()
        {
            int[] syu = new int[MJUtil.LENGTH_SYU];
            foreach (var pai in tehai.tehai)
            {
                syu[pai.PaiNumber]++;
            }
            TehaiSpliter ts = new TehaiSpliter();

            return ts.SplitTehai(syu, tehai.furos);
        }


    }



    public class SplitedTehai
    {
        public List<List<Tartsu>> AllPatternTartsuList { get; set; }
        public int[] Syu { get; set; }
        public SplitedTehai()
        {
            AllPatternTartsuList = new List<List<Tartsu>>();
            Syu = new int[MJUtil.LENGTH_SYU];
        }
    }


    public class Tartsu
    {

        MJUtil.TartsuType tartsuType;
        int tartsuStartPaiSyu;

        public Tartsu(MJUtil.TartsuType tt, int tartsuStartPaiSyu)
        {
            this.tartsuType = tt;
            this.tartsuStartPaiSyu = tartsuStartPaiSyu;
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
            List<List<Tartsu>> horaMentsuList = split(syu);

            //Make Furo Tartsu
            List<Tartsu> furoTartsu = new List<Tartsu>();
            foreach (var furo in furos)
            {
                furoTartsu.Add(new Tartsu(furo.ftype, furo.minPaiSyu));
            }
            //Add Furo
            foreach (var horaMentsu in horaMentsuList)
            {
                horaMentsu.AddRange(furoTartsu);
            }

            //メンツ構造と種類枚数を持つオブジェクトを生成
            SplitedTehai horaMentsuData = new SplitedTehai();
            horaMentsuData.AllPatternTartsuList = horaMentsuList;
            horaMentsuData.Syu = syu;

            return horaMentsuData;
        }



        private static List<List<Tartsu>> split(int[] syu)
        {
            List<Tartsu> work = new List<Tartsu>();
            List<List<Tartsu>> resultList = new List<List<Tartsu>>();
            int start = 0;
            for (int i = 0; i < MJUtil.LENGTH_SYU; i++)
            {
                if (syu[i] >= 2)
                {
                    syu[i] -= 2;
                    Tartsu ts = new Tartsu(MJUtil.TartsuType.Head, i);
                    work.Add(ts);
                    reSplit(start, syu, ref work, ref resultList);
                    work.Remove(ts);
                    syu[i] += 2;
                }
            }
            return resultList;
        }

        private static void reSplit(int start, int[] syu, ref List<Tartsu> work, ref List<List<Tartsu>> resultList)
        {

            if (syuIsZero(syu))
            {
                List<Tartsu> result = new List<Tartsu>();
                foreach (var horaTartsu in work)
                {
                    result.Add(horaTartsu);
                }
                resultList.Add(result);
                return;
            }

            for (int i = start; i < MJUtil.LENGTH_SYU; i++)
            {

                if (syu[i] >= 3)
                {
                    syu[i] -= 3;
                    Tartsu ts = new Tartsu(MJUtil.TartsuType.Anko, i);
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
                    Tartsu ts = new Tartsu(MJUtil.TartsuType.Ansyun, i);
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
            int[] zeros = new int[MJUtil.LENGTH_SYU];
            return zeros.SequenceEqual(syu);
        }

    }
}
