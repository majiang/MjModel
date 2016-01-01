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

        public static SplitedTehai AnalizePattern(Tehai tehai, string horaPai, bool isRon)
        {

            //手配＆和了牌の牌の合計枚数をカウント
            int[] inHandSyu = new int[MJUtil.LENGTH_SYU_ALL];
            foreach (var pai in tehai.tehai)
            {
                inHandSyu[pai.PaiNumber]++;
            }
            if (isRon)
            {
                inHandSyu[PaiConverter.STRING_TO_ID[horaPai]]++;
            }

            TehaiSpliter ts = new TehaiSpliter();

            //手に残っている手配のターツ構成を全て算出
            var splited = ts.SplitTehai(inHandSyu, tehai.furos, horaPai, isRon);

            foreach (var furopai in tehai.furos)
            {
                switch (furopai.ftype)
                {
                    case MJUtil.TartsuType.MINSYUN:
                        splited.SyuNum[furopai.minPaiSyu]++;
                        splited.SyuNum[furopai.minPaiSyu + 1]++;
                        splited.SyuNum[furopai.minPaiSyu + 2]++;
                        break;
                    case MJUtil.TartsuType.MINKO:
                        splited.SyuNum[furopai.minPaiSyu] += 3;
                        break;
                    case MJUtil.TartsuType.MINKANTSU:
                        splited.SyuNum[furopai.minPaiSyu] += 4;
                        break;
                }
            }

            return splited;
        }


    }



    public class SplitedTehai
    {
        public List<HoraPattern> AllHoraPatternList { get; set; }
        public int[] SyuNum { get; set; }//フーロ牌も含めた牌枚数配列
        public SplitedTehai()
        {
            AllHoraPatternList = new List<HoraPattern>();
            SyuNum = new int[MJUtil.LENGTH_SYU_ALL];
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

        public HoraPattern GetCopy()
        {
            return new HoraPattern(this.TartsuList);
        }

    }

    public class Tartsu
    {
        public bool IsRonedTartsu;
        public MJUtil.TartsuType TartsuType { get; private set; }
        public int TartsuStartPaiSyu { get; private set; }

        public Tartsu(MJUtil.TartsuType tt, int tartsuStartPaiSyu)
        {
            this.TartsuType = tt;
            this.TartsuStartPaiSyu = tartsuStartPaiSyu;
        }

        public bool Contains(string pai)
        {
            var targetPaiId = PaiConverter.STRING_TO_ID[pai];
            if (IsAnsyun() || IsMinsyun())
            {
                return TartsuStartPaiSyu <= targetPaiId
                    && targetPaiId <= TartsuStartPaiSyu + 2;
            }
            else
            {
                return TartsuStartPaiSyu == targetPaiId;
            }
        }


        public bool IsHead()
        {
            return TartsuType == MJUtil.TartsuType.HEAD;
        }

        public bool IsAnsyun()
        {
            return TartsuType == MJUtil.TartsuType.ANSYUN;
        }
        public bool IsMinsyun()
        {
            return TartsuType == MJUtil.TartsuType.MINSYUN;
        }
        public bool IsAnko()
        {
            return TartsuType == MJUtil.TartsuType.ANKO;
        }
        public bool IsMinko()
        {
            return TartsuType == MJUtil.TartsuType.MINKO;
        }
        public bool IsAnkantsu()
        {
            return TartsuType == MJUtil.TartsuType.ANKANTSU;
        }
        public bool IsMinkantsu()
        {
            return TartsuType == MJUtil.TartsuType.MINKANTSU;
        }


        public void ChangeRonedTartsu()
        {
            IsRonedTartsu = true;
            if (IsAnsyun())
            {
                TartsuType = MJUtil.TartsuType.MINSYUN;
            }
            else if (IsAnko())
            {
                TartsuType = MJUtil.TartsuType.MINKO;
            }
            

        }


    }



    public class TehaiSpliter
    {


        public SplitedTehai SplitTehai(int[] syu, List<Furo> furos, string horaPai, bool isRon)
        {
            //全通りの上がり構成を算出
            List<HoraPattern> horaMentsuList = split(syu);

            //ロン牌を含むターツが複数ある場合はそれぞれに対してロン上がりのフラグを立てて手配構成を作成。
            if (isRon)
            {
                var ronConsiderdMentsuList = new List<HoraPattern>();
                foreach(var horaMentsu in horaMentsuList)
                {
                    foreach(var tartsu in horaMentsu.TartsuList.Select( (val,index) => new { val, index }).Where(e => e.val.Contains(horaPai)))
                    {
                        var considerd = horaMentsu.GetCopy();
                        considerd.TartsuList[tartsu.index].ChangeRonedTartsu();
                        ronConsiderdMentsuList.Add(considerd);
                    }
                }

                horaMentsuList = ronConsiderdMentsuList;
            }




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
            horaMentsuData.SyuNum = syu;

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
