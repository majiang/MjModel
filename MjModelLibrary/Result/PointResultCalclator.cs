using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjModelLibrary.Result
{

    public struct PointResult
    {
        public int ChildOutcome;
        public int OyaOutcome;
        public int RonedPlayerOutcome;
        public int HoraPlayerIncome;
        public bool IsTsumoHora;
        public bool IsOyaHora;

    }

    public static class PointResultCalclator
    {
        private static bool UseKiriageMangan = false;
        private static readonly int BASE_POINT_YAKUMAN = 8000;
        private static readonly int BASE_POINT_SANBAIMAN = 6000;
        private static readonly int BASE_POINT_BAIMAN = 4000;
        private static readonly int BASE_POINT_HANEMAN = 3000;
        private static readonly int BASE_POINT_MANGAN = 2000;


        public static PointResult AnalyzePoint(YakuResult yakuResult)
        {
            PointResult pointResult = new PointResult();

            int bp = calcBasicPoint(yakuResult.Fu, yakuResult.Han, yakuResult.YakumanMultiple);

            pointResult.IsTsumoHora = yakuResult.IsTsumo;
            pointResult.IsOyaHora = yakuResult.IsOya;

            if (pointResult.IsTsumoHora)
            {//tsumo
                if (pointResult.IsOyaHora)
                {
                    pointResult.ChildOutcome = ceilAt100(bp * 2);
                    pointResult.HoraPlayerIncome = pointResult.ChildOutcome * 3;
                }
                else
                {
                    pointResult.ChildOutcome = ceilAt100(bp);
                    pointResult.OyaOutcome = ceilAt100(bp * 2);
                    pointResult.HoraPlayerIncome = pointResult.ChildOutcome * 2 + pointResult.OyaOutcome;
                }
            }
            else
            {//ron
                if (pointResult.IsOyaHora)
                {
                    pointResult.HoraPlayerIncome = ceilAt100(bp * 6);
                    pointResult.RonedPlayerOutcome = pointResult.HoraPlayerIncome;
                }
                else
                {
                    pointResult.HoraPlayerIncome = ceilAt100(bp * 4);
                    pointResult.RonedPlayerOutcome = pointResult.HoraPlayerIncome;
                }
            }

            return pointResult;
        }

        private static int ceilAt100(int basicPoint)
        {
            return (int)(Math.Ceiling(basicPoint / 100.0) * 100);
        }
        private static int calcBasicPoint(int fu, int han, int yakumanMultiple)
        {
            
            if (yakumanMultiple > 0)
            {
                return BASE_POINT_YAKUMAN * yakumanMultiple;
            }
            //数え役満
            if (han >= 13)
            {
                return BASE_POINT_YAKUMAN;
            }
            else if (han >= 11)
            {
                return BASE_POINT_SANBAIMAN;
            }
            else if (han >= 8)
            {
                return BASE_POINT_BAIMAN;
            }
            else if (han >= 6)
            {
                return BASE_POINT_HANEMAN;
            }
            else if (han >= 5)
            {
                return BASE_POINT_MANGAN;
            }

            //飜数が4翻以下の場合は、マンガンの基準点を超えていても満貫の基準点が上限となる
            var basePoint = (int)(fu * Math.Pow(2, han + 2));
            if (basePoint > BASE_POINT_MANGAN)
            {
                //基準点が満貫の基準点を超えていたら満貫扱い
                return BASE_POINT_MANGAN;
            }

            if (UseKiriageMangan)
            {
                if( (fu == 30) && (han == 4))
                {
                    //切り上げ満貫有りの場合は30譜4翻は満貫扱い
                    return BASE_POINT_MANGAN;
                }
            }
            return basePoint;
        }


        
    }

}
