using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjModelProject
{
    public class Field
    {
        public Pai bakaze;
        public int kyoku;//1~8が入る
        public int honba;
        public int kyotaku;
        public int oya;
        public List<Pai> doramarker;

        public Field()
        {
            Init();
        }

        public void Init()
        {
            bakaze = new Pai();
            kyoku = 0;
            honba = 0;
            kyotaku = 0;
            oya = 0;
            doramarker = new List<Pai>();
        }
    }
}
