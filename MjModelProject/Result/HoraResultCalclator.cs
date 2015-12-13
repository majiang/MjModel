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
    public class HoraResult
    {
        public YakuResult yakuResult;
        public PointResult pointResult;
    }

    public static class HoraResultCalclator
    {
        public static HoraResult CalcHoraResult(Tehai tehai, InfoForResult ifr)
        {
            //面子手の取りうる和了形を全て列挙 
            SplitedTehai splited = TehaiAnalizer.AnalizePattern(tehai);
            List<YakuResult> yakuResultList = new List<YakuResult>();

            //面子手和了型が0の場合はチートイツか国士無双
            if (splited.AllHoraPatternList.Count == 0)
            {
                yakuResultList.Add(YakuAnalizer.CalcSpecialYaku(ifr, splited.Syu));
            }
            else
            {
                //面子手の役を計算   
                foreach (var pattern in splited.AllHoraPatternList)
                {
                    yakuResultList.Add(YakuAnalizer.CalcNormalYaku(pattern, ifr, splited.Syu));
                }
            }
            //それぞれの点数を計算
            Dictionary<YakuResult, PointResult> pointResultMap = new Dictionary<YakuResult, PointResult>();
            foreach (var yakuResult in yakuResultList)
            {
                pointResultMap.Add(yakuResult, PointAnalizer.AnalyzePoint(yakuResult, splited.Syu));
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
