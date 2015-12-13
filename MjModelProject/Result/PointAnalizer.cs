using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjModelProject.Result
{

    public class PointResult
    {
        public int ChildOutcome;
        public int OyaOutcome;
        public int RonedPlayerOutcome;
        public int HoraPlayerIncome;
        public bool HoraIsTsumo;
    }

    public static class PointAnalizer
    {
        private static readonly int BASE_POINT_YAKUMAN = 8000;
        private static readonly int BASE_POINT_SANBAIMAN = 6000;
        private static readonly int BASE_POINT_BAIMAN = 4000;
        private static readonly int BASE_POINT_HANEMAN = 3000;
        private static readonly int BASE_POINT_MANGAN = 2000;


        public static PointResult AnalyzePoint(YakuResult yakuResult, int[] syu)
        {
            PointResult pointResult = new PointResult();

            int bp = calcBasicPoint(yakuResult.Fu, yakuResult.Han, yakuResult.IsYakuman);


            if (yakuResult.IsTsumo)
            {//tsumo
                if (yakuResult.IsOya)
                {
                    pointResult.ChildOutcome = ceilAt100(bp * 2);
                    pointResult.HoraPlayerIncome = pointResult.ChildOutcome * 3;
                }
                else
                {
                    pointResult.ChildOutcome = ceilAt100(bp);
                    pointResult.OyaOutcome = ceilAt100(bp * 2);
                    pointResult.HoraPlayerIncome = pointResult.ChildOutcome * 2 + pointResult. OyaOutcome;
                }
            }
            else
            {//ron
                if (yakuResult.IsOya)
                {
                    pointResult.HoraPlayerIncome = ceilAt100(bp * 6);
                }
                else
                {
                    pointResult.HoraPlayerIncome = ceilAt100(bp * 4);
                }
            }

            return pointResult;
        }

        private static int ceilAt100(int basicPoint)
        {
            return (int)(Math.Ceiling(basicPoint / 100.0) * 100);
        }
        private static int calcBasicPoint(int fu, int han, bool isYakuman)
        {
            
            if (isYakuman)
            {
                return BASE_POINT_YAKUMAN;
            }
            //数え役満はなし
            if (han >= 11)
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

            //飜数が4翻以下の場合は、マンガンの基準点を超えないようにする
            var basePoint = (int)(fu * Math.Pow(2, han + 2));
            if (basePoint > BASE_POINT_MANGAN)
            {
                //マンガン扱い
                return BASE_POINT_MANGAN;
            }
            return basePoint;
        }


        
    }

}
