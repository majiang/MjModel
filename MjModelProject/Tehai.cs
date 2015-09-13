using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjModelProject
{
    public class Tehai
    {
        public List<string> tehai { get; set; }
        public List<Furo> furos { get; set; }

        public Tehai() { }
        public Tehai(List<string> tehai)
        {
            this.tehai = new List<string>(tehai);
            this.furos = new List<Furo>();
        }

        public void Tsumo(string tsumopai)
        {
            tehai.Add(tsumopai);
        }

        public void Da(string dapai)
        {
            if (tehai.Contains(dapai))
            {
                tehai.Remove(dapai);
            }
            else
            {
                Console.Write("tehai doesn't contains {0}! @Tehai_Da", dapai);
                return;
            }
        }

        public void Chi(int actor, int target, string pai, List<string> consumed)
        {
            if ( !IsValidNaki(consumed) )
            {
                Console.Write("invalied naki! @Tehai_Chi");
                return;
            }

            //remove consumed
            foreach (var consumedPai in consumed)
            {
                tehai.Remove(consumedPai);
            }


            //add furo
            furos.Add(new Furo(Furo.Furotype.chi, target, pai, consumed));

        }
        public void Pon(int actor, int target, string pai, List<string> consumed)
        {
            if (!IsValidNaki(consumed))
            {
                Console.Write("invalied naki! @Tehai_Pon");
                return;
            }
            //remove consumed
            foreach (var consumedPai in consumed)
            {
                tehai.Remove(consumedPai);
            }

            //add furo
            furos.Add(new Furo(Furo.Furotype.pon, target, pai, consumed));
        }

        public void Daiminkan(int actor, int target, string pai, List<string> consumed)
        {
            if (!IsValidNaki(consumed))
            {
                Console.Write("invalied naki! @Tehai_Daiminkan");
                return;
            }
            //remove consumed
            foreach (var consumedPai in consumed)
            {
                tehai.Remove(consumedPai);
            }

            //add furo
            furos.Add(new Furo(Furo.Furotype.daiminkan, target, pai, consumed));
        }

        public void Ankan(int actor, List<string> consumed)
        {
            if (!IsValidAnkan(consumed))
            {
                Console.Write("invalied naki! @Tehai_Ankan");
                return;
            }
            //remove consumed
            foreach (var consumedPai in consumed)
            {
                tehai.Remove(consumedPai);
            }

            //add furo
            furos.Add(new Furo(Furo.Furotype.ankan, actor, "" , consumed));//暗槓は牌がすべてconsumedに入る。対象牌は空文字とする
        }

        public void Kakan(int actor, int target, string pai, List<string> consumed)
        {
            if (!IsValidKakan(pai, consumed))
            {
                Console.Write("invalied naki! @Tehai_Ankan");
                return;
            }
            //remove pai
            tehai.Remove(pai);

            //change pon to kakan
            foreach (var furo in furos)
            {
                if (furo.ftype == Furo.Furotype.pon && furo.consumed.SequenceEqual(consumed))
                {
                    furo.ftype = Furo.Furotype.kakan;
                    furo.consumed.Add(pai);
                    furo.consumed.Sort();
                    break;
                }
            }
        }


        private bool IsValidNaki(List<string> consumed)
        {
            //check consumed;
            foreach (var consumedPai in consumed)
            {
                if (!tehai.Contains(consumedPai))
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsValidAnkan(List<string> consumed)
        {
            //check consumed;
            foreach (var consumedPai in consumed)
            {
                if (!tehai.Contains(consumedPai))
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsValidKakan(string pai, List<string> consumed)
        {
            if (!tehai.Contains(pai))
            {
                return false;
            }

            foreach(var furo in furos)
            {
                if( furo.ftype == Furo.Furotype.pon && furo.consumed.SequenceEqual(consumed))
                {
                   return true;
                }
            }

            return false;
        }
    }

    public class Furo
    {

        public enum Furotype
        {
            pon,
            chi,
            daiminkan,
            ankan,
            kakan
        }

        public Furotype ftype;
        public int target;
        public List<string> consumed;
        public string furopai;

        public Furo(Furotype type, int target, string furopai, List<string> consumed)
        {
            this.ftype = type;
            this.target = target;
            this.furopai = furopai;
            this.consumed = new List<string>(consumed);
        }
    }


 
}
