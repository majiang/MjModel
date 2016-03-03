using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjModelProject.Util
{
    public static class PaiConverter
    {


        public static readonly Dictionary<string, int> RED_DORA_STRING_ID = new Dictionary<string, int>()
        {
            { "5mr", 4 },
            { "5pr", 13},
            { "5sr", 22}
        };

        public static readonly Dictionary<string, int> STRING_TO_ID = new Dictionary<string, int>()
        {
            { "5mr", 4},
            { "1m",  0},
            { "2m",  1},
            { "3m",  2},
            { "4m",  3},
            { "5m",  4},
            { "6m",  5},
            { "7m",  6},
            { "8m",  7},
            { "9m",  8},

            { "5pr", 13},
            { "1p",  9},
            { "2p",  10},
            { "3p",  11},
            { "4p",  12},
            { "5p",  13},
            { "6p",  14},
            { "7p",  15},
            { "8p",  16},
            { "9p",  17},

            { "5sr", 22},
            { "1s",  18},
            { "2s",  19},
            { "3s",  20},
            { "4s",  21},
            { "5s",  22},
            { "6s",  23},
            { "7s",  24},
            { "8s",  25},
            { "9s",  26},

            { "E",  27},
            { "S",  28},
            { "W",  29},
            { "N",  30},
            { "P",  31},
            { "F",  32},
            { "C",  33},

            {"?", -1}
        };
        public static readonly Dictionary<int, string> ID_TO_STRING = new Dictionary<int, string>()
        {
            { 0, "1m"},
            { 1, "2m"},
            { 2, "3m"},
            { 3, "4m"},
            { 4, "5m"},
            { 5, "6m"},
            { 6, "7m"},
            { 7, "8m"},
            { 8, "9m"},

            { 9, "1p"},
            { 10,"2p"},
            { 11,"3p"},
            { 12,"4p"},
            { 13,"5p"},
            { 14,"6p"},
            { 15,"7p"},
            { 16,"8p"},
            { 17,"9p"},

            { 18,"1s"},
            { 19,"2s"},
            { 20,"3s"},
            { 21,"4s"},
            { 22,"5s"},
            { 23,"6s"},
            { 24,"7s"},
            { 25,"8s"},
            { 26,"9s"},

            { 27,"E"},
            { 28,"S"},
            { 29,"W"},
            { 30,"N"},
            { 31,"P"},
            { 32,"F"},
            { 33,"C"},

            { -1,"?"}
        };

    }
}
