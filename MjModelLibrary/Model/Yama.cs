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

        public void SetScene(int rest_tsumo_num, List<string> dora_markers,List<List<string>> tehais, List<List<List<string>>> furos,List<List<string>> kawas)
        {
            Init();

            doraMarkers = dora_markers.Select(e=>new Pai(e)).ToList();
            doraPointer = dora_markers.Count;
            rinshanPointer = dora_markers.Count - 1;
            yamaPointer = YAMA_LENGTH - WANPAI_LENGTH - rinshanPointer - rest_tsumo_num;

            mYama = GenerateYamaUseRestPai(dora_markers,tehais,furos,kawas);
        }

        private List<Pai> GenerateYamaUseRestPai(List<string> dora_markers, List<List<string>> tehais, List<List<List<string>>> furos, List<List<string>> kawas)
        {
            var restPais = Enumerable.Range(0, (34 * 4)-1).Select(e => e >> 2).Select(e => new Pai(e)).ToList();
            var usedPais = new List<string>() { };
            usedPais.AddRange(dora_markers);
            tehais.ForEach(e => usedPais.AddRange(e));
            foreach(var onePlayerFuros in furos)
            {
                foreach(var furo in onePlayerFuros)
                {
                    var type = furo[0];
                    var actor = Int32.Parse(furo[1]);
                    var target = Int32.Parse(furo[2]);
                    var furopai = furo[3];

                    if (MJUtil.TARTSU_TYPE_STRING_ENUM_MAP[type] != MJUtil.TartsuType.ANKANTSU)
                    {
                        usedPais.Add(furopai);
                    }
                    usedPais.AddRange(furo.GetRange(3, furo.Count - 3));
                }
            }

            foreach(var usedPai in usedPais)
            {
                var removeIndex = restPais.FindIndex(e => e.PaiString == usedPai); 
                restPais.RemoveAt(removeIndex);
            }
            var shuffled = new List<Pai>(restPais.OrderBy(i => Guid.NewGuid()));
            

            return usedPais.Select(e=> new Pai(e)).Concat(shuffled).ToList();
        }
    }
}
