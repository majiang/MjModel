using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MjModelProject.Util;
using MjModelProject;
using MjModelProject.Model;

namespace MjModelProject.Result
{






    public class YakuResult
    {
        public Dictionary<string, int> yakus = new Dictionary<string, int>();
        public int Han = 0;
        public int Fu = 0;
        public bool IsYakuman;
        public bool IsTsumo;
        public bool IsOya;
    }




    public static class YakuAnalizer
    {
        public static YakuResult CalcSpecialYaku(InfoForResult ifr, int[] horaSyu)
        {
            YakuResult result = new YakuResult();
            result.Fu = 25;
            result.IsTsumo = ifr.IsTsumo;
            result.IsOya = ifr.IsOya;

            //役の文字列取得
            var yakuString = MJUtil.YAKU_STRING;
            //飜数の辞書選択
            var yakuHanNum = ifr.IsMenzen ? MJUtil.YAKU_HAN_MENZEN : MJUtil.YAKU_HAN_FUROED;

            if (ifr.IsDoubleReach)
            {
                result.yakus.Add(yakuString[(int)MJUtil.Yaku.DOUBLEREACH], yakuHanNum[(int)MJUtil.Yaku.DOUBLEREACH]);
            }
            else if (ifr.IsReach)
            {
                result.yakus.Add(yakuString[(int)MJUtil.Yaku.REACH], yakuHanNum[(int)MJUtil.Yaku.REACH]);
            }

            if (ifr.IsTsumo && ifr.IsMenzen)
            {
                result.yakus.Add(yakuString[(int)MJUtil.Yaku.TSUMO], yakuHanNum[(int)MJUtil.Yaku.TSUMO]);
            }
            if (ifr.IsIppatsu)
            {
                result.yakus.Add(yakuString[(int)MJUtil.Yaku.IPPATSU], yakuHanNum[(int)MJUtil.Yaku.IPPATSU]);
            }
            /*
            if (IsDora(horaMentsu, ifr))
            {
                result.yakus.Add(yakuString[(int)MJUtil.Yaku.DORA], CalcDoraNum(horaMentsu, ifr));
            }
            /*
            setDORA(calcDoraWithChitoitsu(ifpc, horaSyu));
            setCHINNITSU(calcChinnitsu(horaSyu));
            setHONNITSU(calcHonnitsu(horaSyu));
            setTANNYAO(calcTannyao(horaSyu));
            setHONROTO(calcHonroto(horaSyu));
            setTSUISO(calcTsuiso(horaSyu));
            setHAITEI(ifpc.getRestTurn() == 0);
            //		setHOUTEI(ifpc.getRestTurn()==0); solo Mahjong can't houtei
            setTENHO(ifpc.getPassedTurn() == 1);
            setCHITOITSU(true);
            setFu(25);

            checkYakuman();*/
            return result;
        }


        public static YakuResult CalcNormalYaku(HoraPattern horaMentsu, InfoForResult ifr, int[] horaSyu)
        {
            YakuResult result = new YakuResult();

            result.Fu = CalcFu(horaMentsu, ifr);
            result.IsTsumo = ifr.IsTsumo;
            result.IsOya = ifr.IsOya;


            //役の文字列取得
            var yakuString = MJUtil.YAKU_STRING;
            //飜数の辞書選択
            var yakuHanNum = ifr.IsMenzen ? MJUtil.YAKU_HAN_MENZEN : MJUtil.YAKU_HAN_FUROED;

            if (ifr.IsDoubleReach)
            {
                result.yakus.Add(yakuString[(int)MJUtil.Yaku.DOUBLEREACH], yakuHanNum[(int)MJUtil.Yaku.DOUBLEREACH]);
            }
            else if (ifr.IsReach)
            {
                result.yakus.Add(yakuString[(int)MJUtil.Yaku.REACH], yakuHanNum[(int)MJUtil.Yaku.REACH]);
            }

            if (ifr.IsTsumo && ifr.IsMenzen)
            {
                result.yakus.Add(yakuString[(int)MJUtil.Yaku.TSUMO], yakuHanNum[(int)MJUtil.Yaku.TSUMO]);
            }
            if (ifr.IsIppatsu)
            {
                result.yakus.Add(yakuString[(int)MJUtil.Yaku.IPPATSU], yakuHanNum[(int)MJUtil.Yaku.IPPATSU]);
            }
            if (IsPinfu(horaMentsu, ifr))
            {
                result.yakus.Add(yakuString[(int)MJUtil.Yaku.PINFU], yakuHanNum[(int)MJUtil.Yaku.PINFU]);
                //ピンフの場合強制的に20符になる
                result.Fu = 20;
            }
            if (IsTannyao(horaMentsu))
            {
                result.yakus.Add(yakuString[(int)MJUtil.Yaku.TANNYAO], yakuHanNum[(int)MJUtil.Yaku.TANNYAO]);
            }

            //一盃口と二盃口は両立しない
            if (IsRyanpeiko(horaMentsu, ifr))
            {
                result.yakus.Add(yakuString[(int)MJUtil.Yaku.RYANPEIKO], yakuHanNum[(int)MJUtil.Yaku.RYANPEIKO]);
            }
            else if (IsIipeiko(horaMentsu, ifr))
            {
                result.yakus.Add(yakuString[(int)MJUtil.Yaku.IIPEIKOU], yakuHanNum[(int)MJUtil.Yaku.IIPEIKOU]);
            }

            if (IsYakuhai(horaMentsu, ifr))
            {
                result.yakus.Add(yakuString[(int)MJUtil.Yaku.YAKUHAI], CalcYakuhaiNum(horaMentsu, ifr));
            }
            if (ifr.IsHoutei)
            {
                result.yakus.Add(yakuString[(int)MJUtil.Yaku.HOUTEI], yakuHanNum[(int)MJUtil.Yaku.HOUTEI]);
            }
            if (ifr.IsHaitei)
            {
                result.yakus.Add(yakuString[(int)MJUtil.Yaku.HAITEI], yakuHanNum[(int)MJUtil.Yaku.HAITEI]);
            }
            if (ifr.IsRinshan)
            {
                result.yakus.Add(yakuString[(int)MJUtil.Yaku.RINSHAN], yakuHanNum[(int)MJUtil.Yaku.RINSHAN]);
            }
            if (ifr.IsChankan)
            {
                result.yakus.Add(yakuString[(int)MJUtil.Yaku.CHANKAN], yakuHanNum[(int)MJUtil.Yaku.CHANKAN]);
            }
            if (IsDora(horaMentsu, ifr))
            {
                result.yakus.Add(yakuString[(int)MJUtil.Yaku.DORA], CalcDoraNum(horaMentsu, ifr));
            }
            if (IsSansyokuDoujun(horaMentsu))
            {
                result.yakus.Add(yakuString[(int)MJUtil.Yaku.SANSYOKUDOJUN], yakuHanNum[(int)MJUtil.Yaku.SANSYOKUDOJUN]);
            }

            if (IsIttsuu(horaMentsu))
            {
                result.yakus.Add(yakuString[(int)MJUtil.Yaku.ITTSUU], yakuHanNum[(int)MJUtil.Yaku.ITTSUU]);
            }

            if (IsSananko(horaMentsu))
            {
                result.yakus.Add(yakuString[(int)MJUtil.Yaku.SANANKO], yakuHanNum[(int)MJUtil.Yaku.SANANKO]);
            }

            if (IsToitoi(horaMentsu))
            {
                result.yakus.Add(yakuString[(int)MJUtil.Yaku.TOITOI], yakuHanNum[(int)MJUtil.Yaku.TOITOI]);
            }

            if (IsShosangen(horaMentsu))
            {
                result.yakus.Add(yakuString[(int)MJUtil.Yaku.SHOSANGEN], yakuHanNum[(int)MJUtil.Yaku.SHOSANGEN]);
            }

            //混老頭とチャンタ系は同時に成立しないためif elseで判定する
            if (IsHonroto(horaMentsu))
            {
                result.yakus.Add(yakuString[(int)MJUtil.Yaku.HONROTO], yakuHanNum[(int)MJUtil.Yaku.HONROTO]);
            }
            else
            {
                if (IsChanta(horaMentsu))
                {
                    result.yakus.Add(yakuString[(int)MJUtil.Yaku.CHANTA], yakuHanNum[(int)MJUtil.Yaku.CHANTA]);
                }
                if (IsJunChanta(horaMentsu))
                {
                    result.yakus.Add(yakuString[(int)MJUtil.Yaku.JUNCHANTA], yakuHanNum[(int)MJUtil.Yaku.JUNCHANTA]);
                }
            }


            if (IsSansyokuDoko(horaMentsu))
            {
                result.yakus.Add(yakuString[(int)MJUtil.Yaku.SANSYOKUDOKO], yakuHanNum[(int)MJUtil.Yaku.SANSYOKUDOKO]);
            }

            if (IsSankantsu(horaMentsu))
            {
                result.yakus.Add(yakuString[(int)MJUtil.Yaku.SANKANTSU], yakuHanNum[(int)MJUtil.Yaku.SANKANTSU]);
            }

            //混一色と清一色は同時に成立しないためif elseで判定する
            if (IsHonnitsu(horaMentsu))
            {
                result.yakus.Add(yakuString[(int)MJUtil.Yaku.HONNITSU], yakuHanNum[(int)MJUtil.Yaku.HONNITSU]);
            }
            else if (IsChinnitsu(horaMentsu))
            {
                result.yakus.Add(yakuString[(int)MJUtil.Yaku.CHINNITSU], yakuHanNum[(int)MJUtil.Yaku.CHINNITSU]);
            }



            //ここから役満
            if(IsSuuanko(horaMentsu))
            {
                result.yakus.Add(yakuString[(int)MJUtil.Yaku.SUUANKO], yakuHanNum[(int)MJUtil.Yaku.SUUANKO]);
            }

            if (IsDaisangen(horaMentsu))
            {
                result.yakus.Add(yakuString[(int)MJUtil.Yaku.DAISANGEN], yakuHanNum[(int)MJUtil.Yaku.DAISANGEN]);
            }

            if (IsShosushi(horaMentsu))
            {
                result.yakus.Add(yakuString[(int)MJUtil.Yaku.SHOSUSHI], yakuHanNum[(int)MJUtil.Yaku.SHOSUSHI]);
            }

            if (IsDaisushi(horaMentsu))
            {
                result.yakus.Add(yakuString[(int)MJUtil.Yaku.DAISUSHI], yakuHanNum[(int)MJUtil.Yaku.DAISUSHI]);
            }

            if(IsTsuiso(horaMentsu))
            {
                result.yakus.Add(yakuString[(int)MJUtil.Yaku.TSUISO], yakuHanNum[(int)MJUtil.Yaku.TSUISO]);
            }

            if (IsRyuiso(horaMentsu))
            {
                result.yakus.Add(yakuString[(int)MJUtil.Yaku.RYUISO], yakuHanNum[(int)MJUtil.Yaku.RYUISO]);
            }

            if (IsChinroto(horaMentsu)) {
                result.yakus.Add(yakuString[(int)MJUtil.Yaku.CHINROTO], yakuHanNum[(int)MJUtil.Yaku.CHINROTO]);
            }

            if(IsChurenpoto(ifr,horaSyu))
            {
                result.yakus.Add(yakuString[(int)MJUtil.Yaku.CHURENPOTO], yakuHanNum[(int)MJUtil.Yaku.CHURENPOTO]);
            }

            if (IsSukantsu(horaMentsu))
            {
                result.yakus.Add(yakuString[(int)MJUtil.Yaku.SUKANTSU], yakuHanNum[(int)MJUtil.Yaku.SUKANTSU]);
            }

            if (IsTenho(ifr))
            {
                result.yakus.Add(yakuString[(int)MJUtil.Yaku.TENHO], yakuHanNum[(int)MJUtil.Yaku.TENHO]);
            }
            
            if( IsChiho(ifr))
            {
                result.yakus.Add(yakuString[(int)MJUtil.Yaku.CHIHO], yakuHanNum[(int)MJUtil.Yaku.CHIHO]);
            }

            if (HasYakuman(result.yakus))
            {
                result.yakus = SelectYakuman(result.yakus);
            }

            //飜数計算
            result.Han = CalcHanSum(result);
            return result;
        }






        private static int CalcFu(HoraPattern horaMentsu, InfoForResult ifpc) {
            int fuSum = 0;
            int futei = 20;
            fuSum += futei;

            if (ifpc.IsMenzen && (!ifpc.IsTsumo)) {
                fuSum += 10;
            }


            int head = horaMentsu.TartsuList.Where(e => e.TartsuType == MJUtil.TartsuType.HEAD).First().TartsuStartPaiSyu;
            if (ifpc.IsJifuu(head)) {
                fuSum += 2;
            }
            if (ifpc.IsJifuu(head)) {
                fuSum += 2;
            }
            if (MJUtil.IsDragon(head)) {
                fuSum += 2;
            }

            if (ifpc.IsTsumo) {
                fuSum += 2;
            }


            int multiple;

            foreach (var tartsu in horaMentsu.TartsuList)
            {
                if (MJUtil.IsYaochu(tartsu.TartsuStartPaiSyu))
                {
                    multiple = 2;
                }
                else
                {
                    multiple = 1;
                }

                switch (tartsu.TartsuType)
                {
                    case MJUtil.TartsuType.MINKO:
                        fuSum += 2 * multiple;
                        continue;
                    case MJUtil.TartsuType.ANKO:
                        fuSum += 4 * multiple;
                        continue;
                    case MJUtil.TartsuType.MINKANTSU:
                        fuSum += 8 * multiple;
                        continue;
                    case MJUtil.TartsuType.ANKANTSU:
                        fuSum += 16 * multiple;
                        continue;
                }

            }

            int lastAddedSyu = ifpc.LastAddedSyu;


            //単騎待ちの場合＋２符
            if (lastAddedSyu == horaMentsu.TartsuList[0].TartsuStartPaiSyu)
            {
                fuSum += 2;
            }
            else
            {
                //カンチャンorペンチャンの場合＋２符
                for (int i = 1; i < horaMentsu.TartsuList.Count; i++) {
                    if ((horaMentsu.TartsuList[i].TartsuType != MJUtil.TartsuType.ANSYUN) && (horaMentsu.TartsuList[i].TartsuType != MJUtil.TartsuType.MINSYUN))
                    {
                        continue;
                    }
                    //順子前提
                    if (lastAddedSyu == horaMentsu.TartsuList[i].TartsuStartPaiSyu + 1) {//カンチャン
                        fuSum += 2;
                        break;
                    } else if ((lastAddedSyu == horaMentsu.TartsuList[i].TartsuStartPaiSyu) && (lastAddedSyu % 9 == 6)) {//7待ちの89ペンチャン
                        fuSum += 2;
                        break;
                    } else if ((lastAddedSyu == horaMentsu.TartsuList[i].TartsuStartPaiSyu + 2) && (lastAddedSyu % 9 == 2)) {//3待ちの12ペンチャン
                        fuSum += 2;
                        break;
                    }
                }
            }


            //喰いタンのみ平和系の場合２０符であるが、２符足して３０符に切り上げる必要あり
            if ((fuSum == 20) && (ifpc.IsMenzen == false)) {
                fuSum += 2;
            }

            return (int)(Math.Ceiling(fuSum / 10.0) * 10);
        }

        private static bool IsPinfu(HoraPattern hp, InfoForResult ifr) {
            int headSyu = hp.Head.TartsuStartPaiSyu;

            //頭が役牌でないか判定
            if (ifr.IsBafuu(headSyu) || ifr.IsJifuu(headSyu) || MJUtil.IsDragon(headSyu))
            {
                return false;
            }

            //門前順子でない場合ピンフではない
            foreach (var tartsu in hp.WithoutHeadTartsuList) {
                if ((tartsu.TartsuType != MJUtil.TartsuType.ANSYUN)) {
                    return false;
                }
            }

            //リャンメン待ちか判定
            int lastAddedSyu = ifr.LastAddedSyu;
            foreach (var tartsu in hp.WithoutHeadTartsuList)
            {
                if ((tartsu.TartsuStartPaiSyu == lastAddedSyu) && (lastAddedSyu % 9 != 6)
                    || (tartsu.TartsuStartPaiSyu == lastAddedSyu - 2) && (lastAddedSyu % 9 != 2))
                {
                    return true;
                }
            }
            return false;
        }

        private static bool IsTannyao(HoraPattern hp)
        {
            foreach (var tartsu in hp.TartsuList)
            {
                switch (tartsu.TartsuType)
                {
                    case MJUtil.TartsuType.ANSYUN:
                        if ((tartsu.TartsuStartPaiSyu % 9) == 0 || (tartsu.TartsuStartPaiSyu % 9) == 6)
                        {
                            return false;
                        }
                        break;
                    case MJUtil.TartsuType.MINSYUN:
                        if ((tartsu.TartsuStartPaiSyu % 9) == 0 || (tartsu.TartsuStartPaiSyu % 9) == 6)
                        {
                            return false;
                        }
                        break;
                    default:
                        if (tartsu.TartsuStartPaiSyu >= 27 || (tartsu.TartsuStartPaiSyu % 9) == 0 || (tartsu.TartsuStartPaiSyu % 9) == 8)
                        {
                            return false;
                        }
                        break;
                }
            }
            return true;
        }

        private static bool IsRyanpeiko(HoraPattern hp, InfoForResult ifr)
        {
            //門前でない場合は終了
            if (ifr.IsMenzen == false)
            {
                return false;
            }

            //4ターツ全てが門前順子でない場合は終了
            if (hp.TartsuList.Select(e => e.TartsuType == MJUtil.TartsuType.ANSYUN).Count() != 4)
            {
                return false;
            }


            var sorted = hp.WithoutHeadTartsuList.OrderBy(e => e.TartsuStartPaiSyu).ToList();

            if (sorted[0].TartsuStartPaiSyu == sorted[1].TartsuStartPaiSyu && sorted[2].TartsuStartPaiSyu == sorted[3].TartsuStartPaiSyu)
            {
                return true;
            }

            return false;
        }

        private static bool IsIipeiko(HoraPattern hp, InfoForResult ifr)
        {
            //門前でない場合は終了
            if (ifr.IsMenzen == false)
            {
                return false;
            }

            //一盃口対象である門前順子のみ抜き出し
            var ansyuns = hp.WithoutHeadTartsuList.Where(e => e.TartsuType == MJUtil.TartsuType.ANSYUN)
                                                  .OrderBy(e => e.TartsuStartPaiSyu);
            var prevStartPaisyu = -1;
            foreach (var ansyun in ansyuns)
            {
                if (ansyun.TartsuStartPaiSyu == prevStartPaisyu)
                {
                    return true;
                }
                prevStartPaisyu = ansyun.TartsuStartPaiSyu;
            }
            return false;
        }

        private static bool IsYakuhai(HoraPattern hp, InfoForResult ifr)
        {
            foreach (var tartsu in hp.TartsuList)
            {
                if (ifr.IsJifuu(tartsu.TartsuStartPaiSyu) 
                    || ifr.IsBafuu(tartsu.TartsuStartPaiSyu)
                    || MJUtil.IsDragon(tartsu.TartsuStartPaiSyu)
                    )
                {
                    return true;
                }
            }
            return false;
        }
        private static int CalcYakuhaiNum(HoraPattern hp, InfoForResult ifr)
        {
            int yakuhaiNum = 0;
            //TODO
            foreach (var tartsu in hp.TartsuList)
            {
                //ダブ東、ダブ南の場合があるので自風と場風は独立に判定する
                if (ifr.IsJifuu(tartsu.TartsuStartPaiSyu))
                {
                    yakuhaiNum++;
                }
                if (ifr.IsBafuu(tartsu.TartsuStartPaiSyu))
                {
                    yakuhaiNum++;
                }
                if (MJUtil.IsDragon(tartsu.TartsuStartPaiSyu))
                {
                    yakuhaiNum++;
                }
            }
            return yakuhaiNum;
        }


        private static bool IsDora(HoraPattern hp, InfoForResult ifr)
        {
            foreach (var tartsu in hp.TartsuList)
            {
                if (tartsu.TartsuType == MJUtil.TartsuType.ANSYUN || tartsu.TartsuType == MJUtil.TartsuType.MINSYUN)
                {
                    if (ifr.IsDora(tartsu.TartsuStartPaiSyu) ||
                         ifr.IsDora(tartsu.TartsuStartPaiSyu + 1) ||
                         ifr.IsDora(tartsu.TartsuStartPaiSyu + 2))
                    {
                        return true;
                    }
                }
                else
                {
                    if (ifr.IsDora(tartsu.TartsuStartPaiSyu))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool IsDoraWithChitoitsu(InfoForResult ifr, int[] horaSyu)
        {
            return ifr.CalcDoraNum(horaSyu) == 0;
        }


        private static int CalcDoraNum(HoraPattern hp, InfoForResult ifr)
        {
            //赤ドラはカウントしない
            var doraNum = 0;
            foreach (var tartsu in hp.TartsuList)
            {
                if (tartsu.TartsuType == MJUtil.TartsuType.ANSYUN || tartsu.TartsuType == MJUtil.TartsuType.MINSYUN)
                {
                    if (ifr.IsDora(tartsu.TartsuStartPaiSyu) ||
                         ifr.IsDora(tartsu.TartsuStartPaiSyu + 1) ||
                         ifr.IsDora(tartsu.TartsuStartPaiSyu + 2))
                    {
                        doraNum++;
                    }
                }
                else if (tartsu.TartsuType == MJUtil.TartsuType.ANKO || tartsu.TartsuType == MJUtil.TartsuType.MINKO)
                {
                    if (ifr.IsDora(tartsu.TartsuStartPaiSyu))
                    {
                        doraNum += 3;
                    }
                }
                else if (tartsu.TartsuType == MJUtil.TartsuType.HEAD)
                {
                    if (ifr.IsDora(tartsu.TartsuStartPaiSyu))
                    {
                        doraNum += 2;
                    }
                }
                else if (tartsu.TartsuType == MJUtil.TartsuType.ANKANTSU || tartsu.TartsuType == MJUtil.TartsuType.MINKANTSU)
                {
                    if (ifr.IsDora(tartsu.TartsuStartPaiSyu))
                    {
                        doraNum += 4;
                    }
                }
            }
            return doraNum;
        }


        private static bool IsSansyokuDoujun(HoraPattern hp)
        {
            var syuntsuStartIndex = new bool[MJUtil.LENGTH_SYU_ALL];

            foreach (var tartsu in hp.WithoutHeadTartsuList)
            {
                if (tartsu.TartsuType == MJUtil.TartsuType.ANSYUN || tartsu.TartsuType == MJUtil.TartsuType.MINSYUN)
                {
                    syuntsuStartIndex[tartsu.TartsuStartPaiSyu] = true;
                }
            }

            var syuLength = 9;
            var manzuBase = 0;
            var pinzuBase = syuLength;
            var souzuBase = syuLength * 2;

            for (int i = 0; i < syuLength; i++)
            {
                if (syuntsuStartIndex[manzuBase + i] && syuntsuStartIndex[pinzuBase + i] && syuntsuStartIndex[souzuBase + i])
                {
                    return true;
                }
            }
            return false;
        }

        private static bool IsIttsuu(HoraPattern hp)
        {
            var syuntsuStartIndex = new bool[MJUtil.LENGTH_SYU_ALL];

            foreach (var tartsu in hp.WithoutHeadTartsuList)
            {
                if (tartsu.TartsuType == MJUtil.TartsuType.ANSYUN || tartsu.TartsuType == MJUtil.TartsuType.MINSYUN)
                {
                    syuntsuStartIndex[tartsu.TartsuStartPaiSyu] = true;
                }
            }

            var syuLength = 9;
            var manzuBase = 0;
            var pinzuBase = syuLength;
            var souzuBase = syuLength * 2;

            var result =
                   (syuntsuStartIndex[manzuBase] && syuntsuStartIndex[manzuBase + 3] && syuntsuStartIndex[manzuBase + 6])
                  || (syuntsuStartIndex[pinzuBase] && syuntsuStartIndex[pinzuBase + 3] && syuntsuStartIndex[pinzuBase + 6])
                  || (syuntsuStartIndex[souzuBase] && syuntsuStartIndex[souzuBase + 3] && syuntsuStartIndex[souzuBase + 6]);

            return result;

        }



        private static bool IsSananko(HoraPattern hp)
        {
            return hp.WithoutHeadTartsuList.Count(e => e.TartsuType == MJUtil.TartsuType.ANKO || e.TartsuType == MJUtil.TartsuType.ANKANTSU)
                >= 3;
        }

        private static bool IsToitoi(HoraPattern hp)
        {
            foreach (var tartsu in hp.WithoutHeadTartsuList)
            {
                if (tartsu.TartsuType == MJUtil.TartsuType.ANSYUN
                    || tartsu.TartsuType == MJUtil.TartsuType.MINSYUN)
                {
                    return false;
                }
            }
            return true;
        }
        private static bool IsShosangen(HoraPattern hp)
        {
            if (MJUtil.IsDragon(hp.Head.TartsuStartPaiSyu) == false)
            {
                return false;
            }

            var doragonCount = 0;
            foreach (var tartsu in hp.WithoutHeadTartsuList)
            {
                if (MJUtil.IsDragon(tartsu.TartsuStartPaiSyu))
                {
                    doragonCount++;
                }
            }

            //頭が三元牌かつ三元牌ターツが２つ以上ある場合
            return doragonCount >= 2;
        }

        private static bool IsHonroto(HoraPattern hp)
        {
            foreach (var tartsu in hp.TartsuList)
            {
                if (tartsu.TartsuType == MJUtil.TartsuType.ANSYUN || tartsu.TartsuType == MJUtil.TartsuType.MINSYUN)
                {
                    return false;
                }

                if (MJUtil.IsYaochu(tartsu.TartsuStartPaiSyu) == false)
                {
                    return false;
                }
            }
            return true;
        }

        private static bool IsChanta(HoraPattern hp)
        {
            foreach (var tartsu in hp.TartsuList)
            {
                if (tartsu.TartsuType == MJUtil.TartsuType.ANSYUN || tartsu.TartsuType == MJUtil.TartsuType.MINSYUN)
                {
                    if ((tartsu.TartsuStartPaiSyu % 9 == 0) || (tartsu.TartsuStartPaiSyu % 9 == 6))
                    {
                        continue;
                    }
                }
                else
                {
                    if (MJUtil.IsRoto(tartsu.TartsuStartPaiSyu))
                    {
                        continue;
                    }
                }
                return false;
            }
            return true;
        }

        private static bool IsJunChanta(HoraPattern hp)
        {
            foreach (var tartsu in hp.TartsuList)
            {
                if (tartsu.TartsuType == MJUtil.TartsuType.ANSYUN || tartsu.TartsuType == MJUtil.TartsuType.MINSYUN)
                {
                    if ((tartsu.TartsuStartPaiSyu % 9 == 0) || (tartsu.TartsuStartPaiSyu % 9 == 6))
                    {
                        continue;
                    }
                }
                else
                {
                    if (MJUtil.IsYaochu(tartsu.TartsuStartPaiSyu))
                    {
                        continue;
                    }
                }
                return false;
            }
            return true;
        }

        private static bool IsSansyokuDoko(HoraPattern hp)
        {
            
            var syu = new int[MJUtil.LENGTH_SYU_ALL];

            foreach( var tartsu in hp.WithoutHeadTartsuList)
            {
                if( tartsu.TartsuType == MJUtil.TartsuType.ANKO
                    || tartsu.TartsuType == MJUtil.TartsuType.MINKO
                    || tartsu.TartsuType == MJUtil.TartsuType.ANKANTSU
                    || tartsu.TartsuType == MJUtil.TartsuType.MINKANTSU)
                {
                    syu[tartsu.TartsuStartPaiSyu]++;
                }
            }

            for(int i=0; i< MJUtil.LENGTH_SYU_NUMBERS; i++)
            {
                if ( syu[i + MJUtil.LENGTH_SYU_NUMBERS * 0] >= 1
                  && syu[i + MJUtil.LENGTH_SYU_NUMBERS * 1] >= 1
                  && syu[i + MJUtil.LENGTH_SYU_NUMBERS * 2] >= 1)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsSankantsu(HoraPattern hp)
        {
            return hp.WithoutHeadTartsuList.Count(e => e.TartsuType == MJUtil.TartsuType.ANKANTSU
                                                    || e.TartsuType == MJUtil.TartsuType.MINKANTSU)
                                                    >= 3;
        }

        private static bool IsHonnitsu(HoraPattern hp)
        {
            var hasManzu = false;
            var hasPinzu = false;
            var hasSouzu = false;
            var hasJi = false;

            foreach (var tartsu in hp.TartsuList)
            {
                if( MJUtil.IsJihai(tartsu.TartsuStartPaiSyu))
                {
                    hasJi = true;
                }
                else
                {
                    var div = tartsu.TartsuStartPaiSyu / MJUtil.LENGTH_SYU_NUMBERS;
                    hasManzu |= (div == 0);
                    hasPinzu |= (div == 1);
                    hasSouzu |= (div == 2);
                }
            }
            var isOneColor = ( hasManzu && !hasPinzu && !hasSouzu)
                        || (!hasManzu &&  hasPinzu && !hasSouzu)
                        || (!hasManzu && !hasPinzu &&  hasSouzu);
            return isOneColor && hasJi;
        }
        
        private static bool IsChinnitsu(HoraPattern hp)
        {
            var hasManzu = false;
            var hasPinzu = false;
            var hasSouzu = false;
 

            foreach (var tartsu in hp.TartsuList)
            {
                var div = tartsu.TartsuStartPaiSyu / MJUtil.LENGTH_SYU_NUMBERS;
                hasManzu |= (div == 0);
                hasPinzu |= (div == 1);
                hasSouzu |= (div == 2);
            }
            return ( hasManzu && !hasPinzu && !hasSouzu)
                || (!hasManzu &&  hasPinzu && !hasSouzu)
                || (!hasManzu && !hasPinzu &&  hasSouzu);            
        }

        private static bool IsSuuanko(HoraPattern hp)
        {
            return hp.WithoutHeadTartsuList.Count(e => e.TartsuType == MJUtil.TartsuType.ANKO
                                                    || e.TartsuType == MJUtil.TartsuType.ANKANTSU)
                                                    >= 4;
        }

        private static bool IsDaisangen(HoraPattern hp)
        {
            return hp.WithoutHeadTartsuList.Count(e => MJUtil.IsDragon( e.TartsuStartPaiSyu ) ) == 3;
        }

        private static bool IsShosushi(HoraPattern hp)
        {
            return MJUtil.IsWind(hp.Head.TartsuStartPaiSyu)
                && hp.WithoutHeadTartsuList.Count(e => MJUtil.IsWind(e.TartsuStartPaiSyu)) == 3;
        }

        private static bool IsDaisushi(HoraPattern hp)
        {
            return hp.WithoutHeadTartsuList.Count(e => MJUtil.IsWind(e.TartsuStartPaiSyu)) == 4;
        }

        private static bool IsTsuiso(HoraPattern hp)
        {
            return hp.TartsuList.Count(e => MJUtil.IsJihai(e.TartsuStartPaiSyu)) == 5;
        }

        private static bool IsRyuiso(HoraPattern hp)
        {
            var greenCount = 0;

            foreach(var tartsu in hp.TartsuList)
            {
                if( tartsu.TartsuType == MJUtil.TartsuType.ANSYUN 
                    || tartsu.TartsuType == MJUtil.TartsuType.MINSYUN)
                {
                    if (MJUtil.IsGreenInSyuntsu(tartsu.TartsuStartPaiSyu))
                    {
                        greenCount++;
                    }
                }
                else
                {
                    if (MJUtil.IsGreen(tartsu.TartsuStartPaiSyu))
                    {
                        greenCount++;
                    }
                }
            }

            return greenCount == 5;
        }

        private static bool IsChinroto(HoraPattern hp)
        {
            if (hp.TartsuList.Count(e => e.TartsuType == MJUtil.TartsuType.ANSYUN
                   || e.TartsuType == MJUtil.TartsuType.MINSYUN) 
                   > 0)
            {
                return false;
            }

            return hp.TartsuList.Count(e => MJUtil.IsRoto(e.TartsuStartPaiSyu)) == 5;
        }


        private static bool IsChurenpoto(InfoForResult ifr,int[] horaSyu)
        {
            if (ifr.IsMenzen == false)
            {
                return false;
            }

            for (int mps = 0; mps < 3; mps++)
            {
                if (
                       (horaSyu[0 + mps * 9] >= 3)
                    && (horaSyu[1 + mps * 9] >= 1)
                    && (horaSyu[2 + mps * 9] >= 1)
                    && (horaSyu[3 + mps * 9] >= 1)
                    && (horaSyu[4 + mps * 9] >= 1)
                    && (horaSyu[5 + mps * 9] >= 1)
                    && (horaSyu[6 + mps * 9] >= 1)
                    && (horaSyu[7 + mps * 9] >= 1)
                    && (horaSyu[8 + mps * 9] >= 3)
                )
                {
                    return true;
                }
            }
            return false;
        }

        private static bool IsSukantsu(HoraPattern hp)
        {
            return hp.TartsuList.Count(e => e.TartsuType == MJUtil.TartsuType.ANKANTSU
                                         || e.TartsuType == MJUtil.TartsuType.MINKANTSU)
                                          == 4;
        }

        private static bool IsTenho(InfoForResult ifr)
        {
            return ifr.PassedTurn == 0;
        }

        private static bool IsChiho(InfoForResult ifr)
        {
            return (1 <= ifr.PassedTurn && ifr.PassedTurn <= 3) && ifr.IsFured;
        }


        private static bool HasYakuman(Dictionary<string, int> yakus)
        {
            return yakus.Where(e => e.Key != MJUtil.YAKU_STRING[(int)MJUtil.Yaku.DORA] && e.Value == 13).Count() > 0;
        }

        private static Dictionary<string, int> SelectYakuman(Dictionary<string, int> yakus)
        {
            return yakus.Where(e => e.Key != MJUtil.YAKU_STRING[(int)MJUtil.Yaku.DORA] && e.Value == 13).ToDictionary(e => e.Key, e => e.Value);
        }

        /*







            private bool calcDora(int[][] horaMentsu, InfoForPointCalc ifpc) {
                int[] syuContainFuro = new int[MJUtil.LENGTH_SYU];
                for(int i=0;i<horaMentsu.length;i++){
			
                    if( (horaMentsu[i][MJUtil.HORAMENTSU_TYPE] == MJUtil.ANSYUN)
                      ||(horaMentsu[i][MJUtil.HORAMENTSU_TYPE] == MJUtil.MINSYUN) ){
				
                        int base = horaMentsu[i][MJUtil.HORAMENTSU_SYU];
                        syuContainFuro[base]++;
                        syuContainFuro[base+1]++;
                        syuContainFuro[base+2]++;
				
                    }else if( (horaMentsu[i][MJUtil.HORAMENTSU_TYPE] == MJUtil.ANKO)
                            ||(horaMentsu[i][MJUtil.HORAMENTSU_TYPE] == MJUtil.MINKO) ){
				
                        int base = horaMentsu[i][MJUtil.HORAMENTSU_SYU];
                        syuContainFuro[base]+=3;
				
                    }else if( (horaMentsu[i][MJUtil.HORAMENTSU_TYPE] == MJUtil.ANKAN)
                            ||(horaMentsu[i][MJUtil.HORAMENTSU_TYPE] == MJUtil.MINKAN) ){
				
                        int base = horaMentsu[i][MJUtil.HORAMENTSU_SYU];
                        syuContainFuro[base]+=4;
                    }else{
                        //head
                        int base = horaMentsu[i][MJUtil.HORAMENTSU_SYU];
                        syuContainFuro[base]+=2;
                    }
                }
                int doraNum = ifpc.getDora().getDoraSum(syuContainFuro,ifpc.isReach());
                setDoraNum(doraNum);
                return doraNum >= 1;
            }



            public void calcYakuWithChitoitsu(InfoForPointCalc ifpc, int[] horaSyu) {

                setInfoForPointCalc( ifpc ); 
                setREACH(ifpc.isReach()&&(!ifpc.isDoubleReach()) );
                setDOUBLEREACH(ifpc.isDoubleReach());
                setIPPATSU(ifpc.isIppatsu());
                setTSUMO(ifpc.isTsumo()&&ifpc.isMenzen());
                setDORA(calcDoraWithChitoitsu(ifpc,horaSyu));
                setCHINNITSU(calcChinnitsu(horaSyu));
                setHONNITSU(calcHonnitsu(horaSyu));
                setTANNYAO(calcTannyao(horaSyu));
                setHONROTO(calcHonroto(horaSyu));
                setTSUISO(calcTsuiso(horaSyu));
                setHAITEI(ifpc.getRestTurn()==0);
        //		setHOUTEI(ifpc.getRestTurn()==0); solo Mahjong can't houtei
                setTENHO(ifpc.getPassedTurn()==1);
                setCHITOITSU(true);
                setFu(25);
		
                checkYakuman();
            }

            private bool calcDoraWithChitoitsu(InfoForPointCalc ifpc, int[] horaSyu) {
                // TODO Auto-generated method stub
                int doraNum = ifpc.getDora().getDoraSum(horaSyu, ifpc.isReach());
		
		
                setDoraNum(doraNum);
                return doraNum > 0;
            }

	
	*/


        private static int CalcHanSum(YakuResult result)
        {
            var hanSum = 0;
            foreach( var yaku in result.yakus)
            {
                hanSum += yaku.Value;
            }
            return hanSum;
        }
        
    }
}
