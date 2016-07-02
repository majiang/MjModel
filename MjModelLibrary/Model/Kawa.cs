using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjModelLibrary
{
    public class Kawa
    {
        public List<KawaPai> discards;
        public List<bool> isReachCalledPosition;


        public Kawa()
        {
            Init();
        }

        public void Init()
        {
            discards = new List<KawaPai>();
            isReachCalledPosition = new List<bool>();
        }

        public void Sutehai(Pai pai , bool isReached){
            discards.Add(new KawaPai(pai));
            isReachCalledPosition.Add(isReached);
        }

        public void Sutehai(string pai, bool isReached)
        {
            discards.Add(new KawaPai(pai));
            isReachCalledPosition.Add(isReached);
        }

        public void Sutehai(Pai pai)
        {
            discards.Add(new KawaPai(pai));
            isReachCalledPosition.Add(false);
        }

        public void Sutehai(string pai)
        {
            discards.Add(new KawaPai(pai));
            isReachCalledPosition.Add(false);
        }


        public bool hasPai(string pai)
        {
            return discards.Any(e => e.PaiString == pai);
        }

        public void SetScene(List<string> kawa, List<bool> isReachd)
        {
            discards = kawa.Select(e => new KawaPai(e)).ToList();
            isReachCalledPosition = new List<bool>(isReachd);
        }
    }

    public class KawaPai : Pai
    {
        public bool isFuroTargeted;
        
        public KawaPai(Pai pai) : base(pai.PaiString)
        {
            
        }

        public KawaPai(string pai) : base(pai)
        {

        }
    }
}
