﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MjModelProject.Util;
using MjModelProject;
using MjModelProject.Model;

namespace MjModelProject.Result
{
    public class HoraResult
    {
        public YakuResult yakuResult;
        public PointResult pointResult;
    }

    public static class ResultCalclator
    {
        public static HoraResult CalcHoraResult(Tehai tehai, InfoForResult ifr, Field field, string horaPai)
        {
            //面子手の取りうる和了形を全て列挙 
            SplitedTehai splited = SplitedTehaiCalclator.CalcSplitedTehai(tehai, horaPai, !ifr.IsTsumo );
            List<YakuResult> yakuResultList = new List<YakuResult>();

            //面子手和了型が0の場合はチートイツか国士無双
            if (splited.AllHoraPatternList.Count == 0)
            {
                yakuResultList.Add(YakuResultCalclator.CalcSpecialYaku(ifr, field, splited.SyuNum));
            }
            else
            {
                //面子手の役を計算
                foreach (var pattern in splited.AllHoraPatternList)
                {
                    yakuResultList.Add(YakuResultCalclator.CalcNormalYaku(pattern, ifr, field, splited.SyuNum));
                }
            }
            
            Dictionary<YakuResult, PointResult> pointResultMap = new Dictionary<YakuResult, PointResult>();
            foreach (var yakuResult in yakuResultList)
            {
                //役と符から点数を計算
                pointResultMap.Add(yakuResult, PointResultCalclator.AnalyzePoint(yakuResult));
            }


            //一番点数が高い和了形の役と点数を返却
            var maxMap = pointResultMap.OrderBy(e => e.Value.HoraPlayerIncome).Last();
            HoraResult maxResult = new HoraResult();
            maxResult.yakuResult = maxMap.Key;     
            maxResult.pointResult = maxMap.Value;     
            
            return maxResult;
        }


     
    }
}
