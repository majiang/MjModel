using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjModelProject
{
    public class Yama
    {
        private int YAMA_LENGTH = 34*4;
        private int WANPAI_LENGTH = 14;
        private int HAIPAI_LENGTH = 13;

        public int restPai;
        public List<Pai> doraOmote;

        private List<Pai> mYama;
        private int yamaPointer;

        public Yama() {
            mYama = MakeYama();
            restPai = YAMA_LENGTH - WANPAI_LENGTH;
            doraOmote = new List<Pai>();
            yamaPointer = 0;
        }

        private List<Pai> MakeYama()
        {
            var ym = new Pai[YAMA_LENGTH];
            for (int i = 0; i < YAMA_LENGTH; i++)
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

        public Pai Tsumo()
        {
            return mYama[yamaPointer++];
        }

        public List<Pai> GetDoraOmote()
        {
            return doraOmote;
        }

        public int GetRestYamaNum()
        {
            return YAMA_LENGTH - WANPAI_LENGTH - yamaPointer;
        }

        public bool CanKan()
        {
            return YAMA_LENGTH - WANPAI_LENGTH - yamaPointer > 14;
        }

        public List<List<Pai>> GetHaipai()
        {
            var haipais = new List<List<Pai>>();

            for (int i = 0; i < HAIPAI_LENGTH; i++ ){
                haipais[0].Add(Tsumo());
                haipais[1].Add(Tsumo());
                haipais[2].Add(Tsumo());
                haipais[3].Add(Tsumo());
            }

            return haipais;
        }

    }
}
