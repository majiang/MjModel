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
        public List<Pai> doraOmote;

        private List<Pai> mYama;
        private int yamaPointer;

        public Yama() {
            mYama = makeYama();
            restPai = YamaLength - WanpaiLength;
            doraOmote = new List<Pai>();
            yamaPointer = 0;
        }

        private List<Pai> makeYama()
        {
            var ym = new Pai[YamaLength];
            for (int i = 0; i < YamaLength; i++)
            {
                ym[i] = new Pai(i >> 2);
            }
            //赤ドラ設定 

            foreach (var redDora in PaiConverter.RED_DORA_STRING_ID) { 
                ym[redDora.Value * 4 - 1].isRedDora = true;
                ym[redDora.Value * 4 - 1].paiString = redDora.Key;
            }

            List<Pai> shuffled = new List<Pai>(ym.OrderBy(i => Guid.NewGuid()));
            return shuffled;
        }

        public Pai tsumo()
        {
            return mYama[yamaPointer++];
        }



    }
}
