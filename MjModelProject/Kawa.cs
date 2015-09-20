using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjModelProject
{
    public class Kawa
    {
        public List<KawaPai> discards;

        public Kawa()
        {
            discards = new List<KawaPai>();
        }

        public void Sutehai(Pai pai , bool isFuroTargeted, bool isReached){
            discards.Add(new KawaPai(pai, isFuroTargeted, isReached));
        }

        public bool hasPai(string pai)
        {
            return discards.Any(e => e.paiString == pai);
        }

    }

    public class KawaPai : Pai
    {
        public bool isFuroTargeted;
        public bool isReached;
        
        public KawaPai(Pai pai, bool isFuroTargeted, bool isReached){
            this.isFuroTargeted = isFuroTargeted;
            this.isReached = isReached;
            this.paiNumber = pai.paiNumber;
            this.paiString = pai.paiString;
            this.isRedDora = pai.isRedDora;
        }
    }
}
