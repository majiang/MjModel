using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MjModelLibrary
{






    public class YakuResult
    {
        public List<List<object>> yakus = new List<List<object>>();
        public int Han = 0;
        public int Fu = 0;
        public int YakumanMultiple;
        public bool IsTsumo;
        public bool IsOya;
        public bool HasYakuExcludeDora;
    }




    public static class YakuResultCalclator
    {
        public static YakuResult CalcSpecialYaku(InfoForResult ifr, Field field, int[] horaSyu, int redDoraNum)
        {
            YakuResult result = new YakuResult();
            result.Fu = 25;//国士無双の場合は符を考慮しなくてよいため七対子の符に設定
            result.IsTsumo = ifr.IsTsumo;
            result.IsOya = ifr.IsOya;




            //役の文字列取得
            var yakuString = MJUtil.YAKU_STRING;
            //飜数の辞書選択
            var yakuHanNum = ifr.IsMenzen ? MJUtil.YAKU_HAN_MENZEN : MJUtil.YAKU_HAN_FUROED;

            //七対子をあらかじめセット
            //国士無双の場合はSelectYakumanで七対子が消えるため初期段階でセットして問題ない
            result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.CHITOITSU], yakuHanNum[(int)MJUtil.Yaku.CHITOITSU] });




            if (ifr.IsDoubleReach)
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.DOUBLEREACH], yakuHanNum[(int)MJUtil.Yaku.DOUBLEREACH] });
            }
            else if (ifr.IsReach)
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.REACH], yakuHanNum[(int)MJUtil.Yaku.REACH] });
            }

            if (ifr.IsTsumo && ifr.IsMenzen)
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.TSUMO], yakuHanNum[(int)MJUtil.Yaku.TSUMO] });
            }
            if (ifr.IsIppatsu)
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.IPPATSU], yakuHanNum[(int)MJUtil.Yaku.IPPATSU] });
            }
            
            if (ifr.CalcDoraNum(horaSyu)>0)
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.DORA], ifr.CalcDoraNum(horaSyu)  });
            }
            if (redDoraNum>0)
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.REDDORA],  redDoraNum });
            }

            if (IsChinnitsu(horaSyu))
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.CHINNITSU], yakuHanNum[(int)MJUtil.Yaku.CHINNITSU] });
            }
            if (IsHonnitsu(horaSyu))
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.HONNITSU], yakuHanNum[(int)MJUtil.Yaku.HONNITSU] });
            }
            if (IsTannyao(horaSyu))
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.TANNYAO], yakuHanNum[(int)MJUtil.Yaku.TANNYAO] });
            }
            if (IsHonroto(horaSyu))
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.HONROTO], yakuHanNum[(int)MJUtil.Yaku.HONROTO] });
            }
            if (ifr.IsHoutei)
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.HOUTEI], yakuHanNum[(int)MJUtil.Yaku.HOUTEI] });
            }
            if (ifr.IsHaitei)
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.HAITEI], yakuHanNum[(int)MJUtil.Yaku.HAITEI] });
            }
            if (ifr.IsChankan)
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.CHANKAN], yakuHanNum[(int)MJUtil.Yaku.CHANKAN] });
            }
            if (IsTenho(ifr))
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.TENHO], yakuHanNum[(int)MJUtil.Yaku.TENHO] });
            }

            if (IsChiho(ifr))
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.CHIHO], yakuHanNum[(int)MJUtil.Yaku.CHIHO] });
            }
            if (IsTsuiso(horaSyu))
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.TSUISO], yakuHanNum[(int)MJUtil.Yaku.TSUISO] });
            }


            if (HasYakuman(result.yakus))
            {
                result.yakus = SelectYakuman(result.yakus);
                result.YakumanMultiple = result.yakus.Count;
            }

            result.Han = CalcHanSum(result.yakus);

            result.HasYakuExcludeDora = CalcHanSumWithoutDora(result.yakus) > 0;

            return result;
        }


        public static YakuResult CalcNormalYaku(HoraPattern horaMentsu, InfoForResult ifr, Field field,int[] horaSyu, int[] realPaiNum, int redDoraNum)
        {
            YakuResult result = new YakuResult();

            result.Fu = CalcFu(horaMentsu, field, ifr);
            result.IsTsumo = ifr.IsTsumo;
            result.IsOya = ifr.IsOya;


            //役の文字列取得
            var yakuString = MJUtil.YAKU_STRING;
            //飜数の辞書選択
            var yakuHanNum = ifr.IsMenzen ? MJUtil.YAKU_HAN_MENZEN : MJUtil.YAKU_HAN_FUROED;

            if (ifr.IsDoubleReach)
            {

                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.DOUBLEREACH], yakuHanNum[(int)MJUtil.Yaku.DOUBLEREACH] });
            }
            else if (ifr.IsReach)
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.REACH], yakuHanNum[(int)MJUtil.Yaku.REACH] });
            }

            if (ifr.IsTsumo && ifr.IsMenzen)
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.TSUMO], yakuHanNum[(int)MJUtil.Yaku.TSUMO] });
            }
            if (ifr.IsIppatsu)
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.IPPATSU], yakuHanNum[(int)MJUtil.Yaku.IPPATSU] });
            }
            if (IsPinfu(horaMentsu, ifr))
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.PINFU], yakuHanNum[(int)MJUtil.Yaku.PINFU] });
                result.Fu = ifr.IsTsumo ? 20 : 30;//ピンフツモは20符、ピンフロンは30符
            }
            if (IsTannyao(horaMentsu))
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.TANNYAO], yakuHanNum[(int)MJUtil.Yaku.TANNYAO] });
            }

            //一盃口と二盃口は両立しない
            if (IsRyanpeiko(horaMentsu, ifr))
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.RYANPEIKO], yakuHanNum[(int)MJUtil.Yaku.RYANPEIKO] });
            }
            else if (IsIipeiko(horaMentsu, ifr))
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.IIPEIKOU], yakuHanNum[(int)MJUtil.Yaku.IIPEIKOU] });
            }

            if (IsYakuhai(horaMentsu, ifr))
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.YAKUHAI], CalcYakuhaiNum(horaMentsu, ifr)});
            }
            if (ifr.IsHoutei)
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.HOUTEI], yakuHanNum[(int)MJUtil.Yaku.HOUTEI] });
            }
            if (ifr.IsHaitei)
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.HAITEI], yakuHanNum[(int)MJUtil.Yaku.HAITEI] });
            }
            if (ifr.IsRinshan)
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.RINSHAN], yakuHanNum[(int)MJUtil.Yaku.RINSHAN] });
            }
            if (ifr.IsChankan)
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.CHANKAN], yakuHanNum[(int)MJUtil.Yaku.CHANKAN] });
            }
            if (ifr.CalcDoraNum(realPaiNum) > 0)
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.DORA], ifr.CalcDoraNum(realPaiNum) });
            }
            if (redDoraNum > 0)
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.REDDORA], redDoraNum });
            }
            if (IsSansyokuDoujun(horaMentsu))
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.SANSYOKUDOJUN], yakuHanNum[(int)MJUtil.Yaku.SANSYOKUDOJUN] });
            }

            if (IsIttsuu(horaMentsu))
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.ITTSUU], yakuHanNum[(int)MJUtil.Yaku.ITTSUU] });
            }

            if (IsSananko(horaMentsu))
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.SANANKO], yakuHanNum[(int)MJUtil.Yaku.SANANKO] });
            }

            if (IsToitoi(horaMentsu))
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.TOITOI], yakuHanNum[(int)MJUtil.Yaku.TOITOI] });
            }

            if (IsShosangen(horaMentsu))
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.SHOSANGEN], yakuHanNum[(int)MJUtil.Yaku.SHOSANGEN] });
            }

            //混老頭とチャンタ系は同時に成立しない
            if (IsHonroto(horaMentsu))
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.HONROTO], yakuHanNum[(int)MJUtil.Yaku.HONROTO] });
            }
            else
            {
                //純チャンタと混チャンタは同時に成立しない
                if (IsJunChanta(horaMentsu))
                {
                    result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.JUNCHANTA], yakuHanNum[(int)MJUtil.Yaku.JUNCHANTA] });
                }
                else if (IsChanta(horaMentsu))
                {
                    result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.CHANTA], yakuHanNum[(int)MJUtil.Yaku.CHANTA] });
                }
            }


            if (IsSansyokuDoko(horaMentsu))
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.SANSYOKUDOKO], yakuHanNum[(int)MJUtil.Yaku.SANSYOKUDOKO] });
            }

            if (IsSankantsu(horaMentsu))
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.SANKANTSU], yakuHanNum[(int)MJUtil.Yaku.SANKANTSU] });
            }

            //混一色と清一色は同時に成立しない
            if (IsHonnitsu(horaMentsu))
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.HONNITSU], yakuHanNum[(int)MJUtil.Yaku.HONNITSU] });
            }
            else if (IsChinnitsu(horaMentsu))
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.CHINNITSU], yakuHanNum[(int)MJUtil.Yaku.CHINNITSU] });
            }



            //ここから役満
            if(IsSuuanko(horaMentsu))
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.SUUANKO], yakuHanNum[(int)MJUtil.Yaku.SUUANKO] });
            }

            if (IsDaisangen(horaMentsu))
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.DAISANGEN], yakuHanNum[(int)MJUtil.Yaku.DAISANGEN] });
            }

            if (IsShosushi(horaMentsu))
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.SHOSUSHI], yakuHanNum[(int)MJUtil.Yaku.SHOSUSHI] });
            }

            if (IsDaisushi(horaMentsu))
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.DAISUSHI], yakuHanNum[(int)MJUtil.Yaku.DAISUSHI] });
            }

            if (IsTsuiso(horaMentsu))
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.TSUISO], yakuHanNum[(int)MJUtil.Yaku.TSUISO] });
            }

            if (IsRyuiso(horaMentsu))
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.RYUISO], yakuHanNum[(int)MJUtil.Yaku.RYUISO] });
            }

            if (IsChinroto(horaMentsu)) {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.CHINROTO], yakuHanNum[(int)MJUtil.Yaku.CHINROTO] });
            }

            if(IsChurenpoto(ifr,horaSyu))
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.CHURENPOTO], yakuHanNum[(int)MJUtil.Yaku.CHURENPOTO] });
            }

            if (IsSukantsu(horaMentsu))
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.SUKANTSU], yakuHanNum[(int)MJUtil.Yaku.SUKANTSU] });
            }

            if (IsTenho(ifr))
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.TENHO], yakuHanNum[(int)MJUtil.Yaku.TENHO] });
            }
            
            if( IsChiho(ifr))
            {
                result.yakus.Add(new List<object>() { yakuString[(int)MJUtil.Yaku.CHIHO], yakuHanNum[(int)MJUtil.Yaku.CHIHO] });
            }

            if (HasYakuman(result.yakus))
            {
                result.yakus = SelectYakuman(result.yakus);
                result.YakumanMultiple = result.yakus.Count;
            }

            //飜数計算
            result.Han = CalcHanSum(result.yakus);

            result.HasYakuExcludeDora = CalcHanSumWithoutDora(result.yakus) > 0;
            return result;
        }


        



        private static int CalcFu(HoraPattern horaMentsu, Field field, InfoForResult ifpc) {
            int fuSum = 0;
            int futei = 20;
            fuSum += futei;

            //門前ロンの場合
            if (ifpc.IsMenzen && (!ifpc.IsTsumo)) {
                fuSum += 10;
            }

            //頭が役牌の場合
            int headSyuId = horaMentsu.TartsuList.Where(e => e.IsHead()).First().TartsuStartPaiSyu;
            if (ifpc.Isbakaze(headSyuId)) {
                fuSum += 2;
            }
            if (ifpc.IsJifuu(headSyuId)) {
                fuSum += 2;
            }
            if (MJUtil.IsDragonPaiId(headSyuId)) {
                fuSum += 2;
            }

            //ツモの場合
            if (ifpc.IsTsumo) {
                fuSum += 2;
            }


            int multiple;

            foreach (var tartsu in horaMentsu.TartsuList)
            {
                if (tartsu.IsYaochuTartsu())
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

            int lastAddedSyu = ifpc.GetLastAddedSyu();

            //待ちによる符加算
            //単騎待ちの場合
            if (horaMentsu.Head.IsRonedTartsu || horaMentsu.Head.TartsuStartPaiSyu == ifpc.GetLastAddedSyu())
            {
                fuSum += 2;
            }
            else
            {
                //カンチャンorペンチャンの場合
                for (int i = 1; i < horaMentsu.TartsuList.Count; i++) {
                    if ((horaMentsu.TartsuList[i].IsAnsyun() || (horaMentsu.TartsuList[i].IsMinsyun() && horaMentsu.TartsuList[i].IsRonedTartsu) ) == false)
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


            //喰い平和系の場合そのまま計算すると20符だが、ルール的にピンフ以外は最低30符のため2符足して30符に切り上げる
            if ((fuSum == 20) && (ifpc.IsMenzen == false)) {
                fuSum += 2;
            }

            return (int)(Math.Ceiling(fuSum / 10.0) * 10);
        }

        private static bool IsPinfu(HoraPattern hp, InfoForResult ifr) {
            int headSyu = hp.Head.TartsuStartPaiSyu;

            //頭が役牌でないか判定
            if (ifr.Isbakaze(headSyu) || ifr.IsJifuu(headSyu) || MJUtil.IsDragonPaiId(headSyu))
            {
                return false;
            }

            //門前順子またはロン和了明順メンツではない場合ピンフではない
            foreach (var tartsu in hp.WithoutHeadTartsuList) {
                if ( ( (tartsu.IsAnsyun()) || ( tartsu.IsMinsyun() && tartsu.IsRonedTartsu) ) == false)
                {
                    return false;
                }
            }

            //リャンメン待ちか判定
            int lastAddedSyu = ifr.GetLastAddedSyu();
            foreach (var tartsu in hp.WithoutHeadTartsuList )
            {
                if ( (tartsu.TartsuStartPaiSyu == lastAddedSyu) && (lastAddedSyu % 9 != 6)
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
                if(tartsu.IsYaochuTartsu())
                {
                    return false;
                }
                
            }
            return true;
        }

        private static bool IsTannyao(int[] horaSyu)
        {
            foreach (var syu in horaSyu.Select( (val, index) => new { val,index }).Where(e => e.val > 0) )
            {
                if (MJUtil.IsYaochuPai(syu.index))
                {
                    return false;
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

            //4ターツ全てが門前順子またはロンした明順でない場合は終了
            if (hp.WithoutHeadTartsuList.Count( e => e.IsAnsyun() || (e.IsMinsyun()&&e.IsRonedTartsu) ) != 4)
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

            //一盃口対象である門前順子またはロンした明順のみ抜き出し
            var ansyuns = hp.WithoutHeadTartsuList.Where(e => e.IsAnsyun() || (e.IsMinsyun() && e.IsRonedTartsu))
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
            foreach (var tartsu in hp.WithoutHeadTartsuList)
            {
                if (ifr.IsJifuu(tartsu.TartsuStartPaiSyu) 
                    || ifr.Isbakaze(tartsu.TartsuStartPaiSyu)
                    || MJUtil.IsDragonPaiId(tartsu.TartsuStartPaiSyu)
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
            foreach (var tartsu in hp.WithoutHeadTartsuList)
            {
                //ダブ東、ダブ南の場合があるので自風と場風は独立に判定する
                if (ifr.IsJifuu(tartsu.TartsuStartPaiSyu))
                {
                    yakuhaiNum++;
                }
                if (ifr.Isbakaze(tartsu.TartsuStartPaiSyu))
                {
                    yakuhaiNum++;
                }
                if (MJUtil.IsDragonPaiId(tartsu.TartsuStartPaiSyu))
                {
                    yakuhaiNum++;
                }
            }
            return yakuhaiNum;
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
            return hp.WithoutHeadTartsuList.Count(e => e.IsAnko() || e.IsAnkantsu() )
                >= 3;
        }

        private static bool IsToitoi(HoraPattern hp)
        {
            foreach (var tartsu in hp.WithoutHeadTartsuList)
            {
                if (tartsu.IsAnsyun()
                    || tartsu.IsMinsyun())
                {
                    return false;
                }
            }
            return true;
        }
        private static bool IsShosangen(HoraPattern hp)
        {
            if (MJUtil.IsDragonPaiId(hp.Head.TartsuStartPaiSyu) == false)
            {
                return false;
            }

            var doragonCount = 0;
            foreach (var tartsu in hp.WithoutHeadTartsuList)
            {
                if (MJUtil.IsDragonPaiId(tartsu.TartsuStartPaiSyu))
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

                if (MJUtil.IsYaochuPai(tartsu.TartsuStartPaiSyu) == false)
                {
                    return false;
                }
            }
            return true;
        }

        private static bool IsHonroto(int[] horaSyu)
        {
            foreach (var syu in horaSyu.Select((val, index) => new { val, index }).Where(e => e.val > 0))
            {
                if (MJUtil.IsYaochuPai(syu.index) == false)
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
                    if (MJUtil.IsYaochuPai(tartsu.TartsuStartPaiSyu))
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
                    if (MJUtil.IsRotoPai(tartsu.TartsuStartPaiSyu))
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
                if( MJUtil.IsJihaiPaiId(tartsu.TartsuStartPaiSyu))
                {
                    hasJi |= true;
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

        //チートイ用
        private static bool IsHonnitsu(int[] horaSyu)
        {
            var hasManzu = false;
            var hasPinzu = false;
            var hasSouzu = false;
            var hasJi = false;

            foreach (var syu in horaSyu.Select((val, index) => new { val, index }))
            {
                if (syu.val == 0)
                {
                    continue;
                }
                if (MJUtil.IsJihaiPaiId(syu.index))
                {
                    hasJi |= true;
                    continue;
                }
                var div = syu.index / MJUtil.LENGTH_SYU_NUMBERS;
                hasManzu |= (div == 0);
                hasPinzu |= (div == 1);
                hasSouzu |= (div == 2);
            }
            var isOnecolor = (hasManzu && !hasPinzu && !hasSouzu)
                || (!hasManzu && hasPinzu && !hasSouzu)
                || (!hasManzu && !hasPinzu && hasSouzu);
            return isOnecolor && hasJi;
        }


        private static bool IsChinnitsu(HoraPattern hp)
        {
            var hasManzu = false;
            var hasPinzu = false;
            var hasSouzu = false;

            foreach (var tartsu in hp.TartsuList)
            {
                if (MJUtil.IsJihaiPaiId(tartsu.TartsuStartPaiSyu))
                {
                    return false;
                }
                var div = tartsu.TartsuStartPaiSyu / MJUtil.LENGTH_SYU_NUMBERS;
                hasManzu |= (div == 0);
                hasPinzu |= (div == 1);
                hasSouzu |= (div == 2);
            }
            var isOneColor = (hasManzu && !hasPinzu && !hasSouzu)
                        || (!hasManzu && hasPinzu && !hasSouzu)
                        || (!hasManzu && !hasPinzu && hasSouzu);
            return isOneColor;
        }
        private static bool IsChinnitsu(int[] horaSyu)
        {
            var hasManzu = false;
            var hasPinzu = false;
            var hasSouzu = false;

            foreach (var syu in horaSyu.Select( (val,index) => new { val, index } ))
            {
                if(syu.val == 0)
                {
                    continue;
                }
                if (MJUtil.IsJihaiPaiId(syu.index))
                {
                    return false;
                }
                var div = syu.index / MJUtil.LENGTH_SYU_NUMBERS;
                hasManzu |= (div == 0);
                hasPinzu |= (div == 1);
                hasSouzu |= (div == 2);
            }
            return (hasManzu && !hasPinzu && !hasSouzu)
                || (!hasManzu && hasPinzu && !hasSouzu)
                || (!hasManzu && !hasPinzu && hasSouzu);

        }


        private static bool IsSuuanko(HoraPattern hp)
        {
            return hp.WithoutHeadTartsuList.Count(e => e.TartsuType == MJUtil.TartsuType.ANKO
                                                    || e.TartsuType == MJUtil.TartsuType.ANKANTSU)
                                                    >= 4;
        }

        private static bool IsDaisangen(HoraPattern hp)
        {
            return hp.WithoutHeadTartsuList.Count(e => MJUtil.IsDragonPaiId( e.TartsuStartPaiSyu ) ) == 3;
        }

        private static bool IsShosushi(HoraPattern hp)
        {
            return MJUtil.IsWindPaiId(hp.Head.TartsuStartPaiSyu)
                && hp.WithoutHeadTartsuList.Count(e => MJUtil.IsWindPaiId(e.TartsuStartPaiSyu)) == 3;
        }

        private static bool IsDaisushi(HoraPattern hp)
        {
            return hp.WithoutHeadTartsuList.Count(e => MJUtil.IsWindPaiId(e.TartsuStartPaiSyu)) == 4;
        }

        private static bool IsTsuiso(HoraPattern hp)
        {
            return hp.TartsuList.Count(e => MJUtil.IsJihaiPaiId(e.TartsuStartPaiSyu)) == 5;
        }
        private static bool IsTsuiso(int[] horaSyu)
        {
            foreach(var syu in horaSyu.Select( (val,index) => new {val,index }).Where( syu => syu.val > 0))
            {
                if(MJUtil.IsJihaiPaiId(syu.index) == false)
                {
                    return false;
                }
            }
            return true;
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
                    if (MJUtil.IsGreenPai(tartsu.TartsuStartPaiSyu))
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

            return hp.TartsuList.Count(e => MJUtil.IsRotoPai(e.TartsuStartPaiSyu)) == 5;
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
            return ifr.UseYamaPaiNum == 1;
        }

        private static bool IsChiho(InfoForResult ifr)
        {
            return ifr.IsFirstTurn() && ifr.IsOya == false; 
        }


        private static bool HasYakuman(List<List<object>> yakus)
        {
            return yakus.Where(e => e.Count > 1)
                .Where( e => ((string)e[0]) != (MJUtil.YAKU_STRING[(int)MJUtil.Yaku.DORA]) && (int)(e[1]) == 13).Count() > 0;
        }

        private static List<List<object>> SelectYakuman(List<List<object>> yakus)
        {
            return yakus.Where(e => e.Count > 1)
                .Where( e => (string)e[0] != MJUtil.YAKU_STRING[(int)MJUtil.Yaku.DORA] && (int)e[1] == 13).ToList();
        }

        private static int CalcHanSum(List<List<object>> yakus)
        {
            var hanSum = 0;
            foreach( var yaku in yakus)
            {
                hanSum += (int)yaku[1];
            }
            return hanSum;
        }

        private static int CalcHanSumWithoutDora(List<List<object>> yakus)
        {
            var hanSum = 0;
            foreach (var yaku in yakus)
            {
                if((string)yaku[0] == MJUtil.YAKU_STRING[(int)MJUtil.Yaku.DORA])
                {
                    continue;
                }
                hanSum += (int)yaku[1];
            }
            return hanSum;
        }

    }
}
