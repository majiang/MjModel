using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MjModelLibrary
{
    public class Yama
    {
        bool USE_RED_DORA = true;
        const int YAMA_LENGTH = 34 * 4;
        const int WANPAI_LENGTH = 14;
        const int DORA_START_POS = YAMA_LENGTH - WANPAI_LENGTH + 4;
        const int WANPAI_START_POS = YAMA_LENGTH - WANPAI_LENGTH;

        private int ONEPLAYER_HAIPAI_LENGTH = 13;

        List<Pai> doraMarkers;


        List<Pai> mYama;
        int yamaPointer;
        int rinshanPointer;
        int doraPointer;

        public Yama()
        {
            Init();
        }

        public void Init()
        {
            mYama = MakeYama();

            doraMarkers = new List<Pai>();
            yamaPointer = 0;
            rinshanPointer = 0;
            doraPointer = 0;
            OpenDoraOmote();
        }

        public void ReplaceTsumoForTest(List<string> pais)
        {
            var tsumoStartIndex = MJUtil.LENGTH_HAIPAI * 4;
            mYama.RemoveRange(tsumoStartIndex, pais.Count);
            mYama.InsertRange(tsumoStartIndex, pais.Select(e => new Pai(e)));
        }

        public void ReplaceRinshanForTest(List<string> pais)
        {
            var rinshanStartIndex = WANPAI_START_POS;
            mYama.RemoveRange(rinshanStartIndex, pais.Count);
            mYama.InsertRange(rinshanStartIndex, pais.Select(e => new Pai(e)));
        }


        List<Pai> MakeYama()
        {
            //牌作成
            var ym = new Pai[YAMA_LENGTH];
            for (int i = 0; i < YAMA_LENGTH; i++)
            {
                ym[i] = new Pai(i >> 2);
            }

            //赤ドラ設定
            if (USE_RED_DORA)
            {
                foreach (var redDora in PaiConverter.RED_DORA_STRING_ID)
                {
                    ym[redDora.Value * 4].IsRedDora = true;
                    ym[redDora.Value * 4].PaiString = redDora.Key;
                }
            }

            //シャッフル
            List<Pai> shuffled = new List<Pai>(ym.OrderBy(i => Guid.NewGuid()));
            return shuffled;
        }

        public Pai DoTsumo()
        {
            return mYama[yamaPointer++];
        }



        public int GetRestYamaNum()
        {
            return YAMA_LENGTH - WANPAI_LENGTH - yamaPointer - rinshanPointer;
        }

        public int GetTsumoedYamaNum()
        {
            return yamaPointer + rinshanPointer - ONEPLAYER_HAIPAI_LENGTH * 4;
        }

        public bool CanKan()
        {
            return (GetRestYamaNum() > 0) && (rinshanPointer < 4);
        }



        public Pai DoRinshan()
        {
            return mYama[WANPAI_START_POS + rinshanPointer++];
        }
        public Pai OpenDoraOmote()
        {
            var opened = mYama[DORA_START_POS + doraPointer++];
            doraMarkers.Add(opened);
            return opened;
        }

        public List<List<Pai>> MakeHaipai()
        {
            var haipais = new List<List<Pai>>() { new List<Pai>(), new List<Pai>(), new List<Pai>(), new List<Pai>(), };

            for (int i = 0; i < ONEPLAYER_HAIPAI_LENGTH; i++)
            {
                haipais[0].Add(DoTsumo());
                haipais[1].Add(DoTsumo());
                haipais[2].Add(DoTsumo());
                haipais[3].Add(DoTsumo());
            }

            return haipais;
        }

        public List<string> GetDoraMarkerStrings()
        {
            return doraMarkers.Select(e=>e.PaiString).ToList();
        }

        public List<string> GetUradoraMarkerStrings()
        {
            var uradoraMarkers = new List<string>();

            for (int i = 0; i < doraPointer; i++)
            {
                uradoraMarkers.Add( (mYama[DORA_START_POS + doraPointer + i]).PaiString);
            }

            return uradoraMarkers;
        }
    }
}
