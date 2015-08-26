using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjModelProject
{
    public class Yama
    {
        private int YamaLength = 34*4;
        private int WanpaiLength = 14;

        public int restPai;
        public List<int> doraOmote;

        private List<int> mYama;
        private int yamaPointer;

        public Yama() {
            mYama = makeYama();
            restPai = YamaLength - WanpaiLength;
            doraOmote = new List<int>();
            yamaPointer = 0;
        }
        
        private List<int> makeYama()
        {
            var ym = new int[YamaLength];
            for (int i = 0; i < YamaLength; i++)
            {
                ym[i] = i;
            }
            List<int> shuffled = new List<int>(ym.OrderBy(i => Guid.NewGuid()));
            return shuffled;
        }

        public int tsumo()
        {
            return mYama[yamaPointer++];
        }



    }
}
