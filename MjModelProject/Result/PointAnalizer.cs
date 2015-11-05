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
        public int RonOutcome;
        public int Income;
    }

    public static class PointAnalizer
    {
        public static PointResult AnalyzePoint(YakuResult yakuResult, InfoForResult ifr, int[] syu)
        {
            return new PointResult();
        }

    }
}
