using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjModelProject
{
    public class Tehai
    {
        public List<Pai> tehai { get; set; }
        public List<Furo> furos { get; set; }
        public static readonly List<Pai> UNKNOWN_TEHAI_PAI = new List<Pai> 
        {
            new Pai("?"), new Pai("?"), new Pai("?"), new Pai("?"), new Pai("?"),
            new Pai("?"), new Pai("?"), new Pai("?"), new Pai("?"), new Pai("?"),
            new Pai("?"), new Pai("?"), new Pai("?")
        };
        public static readonly List<string> UNKNOWN_TEHAI_STRING = new List<string> 
        {
           "?","?","?","?","?","?","?","?","?","?","?","?","?"
        };

        public Tehai() { }
        public Tehai(List<Pai> tehai)
        {
            this.tehai = new List<Pai>(tehai);//コピーを作成。
            this.furos = new List<Furo>();
        }

        public Tehai(List<string> tehai)
        {
            var paiTehai = from hai in tehai select new Pai(hai);
            this.tehai = new List<Pai>(paiTehai);//コピーを作成。
            this.furos = new List<Furo>();
        }

        public List<string> GetTehaiString()
        {
            return tehai.Select(e => e.paiString).ToList();
        }

        public void Tsumo(Pai tsumopai)
        {
            tehai.Add(tsumopai);
        }

        public void Da(Pai dapai)
        {
            if (tehai.Any( e => e == dapai))
            {
                tehai.RemoveAt(tehai.FindIndex(e => e == dapai));
            }
            else
            {
                Console.Write("tehai doesn't contains {0}! @Tehai_Da", dapai);
                return;
            }
        }

        public void Chi(int actor, int target, Pai pai, List<Pai> consumed)
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
        public void Pon(int actor, int target, Pai pai, List<Pai> consumed)
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

        public void Daiminkan(int actor, int target, Pai pai, List<Pai> consumed)
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

        public void Ankan(int actor, List<Pai> consumed)
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
            furos.Add(new Furo(Furo.Furotype.ankan, actor, new Pai() , consumed));//暗槓は牌がすべてconsumedに入る。対象牌は空文字とする
        }

        public void Kakan(int actor, int target, Pai pai, List<Pai> consumed)
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


        private bool IsValidNaki(List<Pai> consumed)
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

        private bool IsValidAnkan(List<Pai> consumed)
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

        private bool IsValidKakan(Pai pai, List<Pai> consumed)
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
        public List<Pai> consumed;
        public Pai furopai;

        public Furo(Furotype type, int target, Pai furopai, List<Pai> consumed)
        {
            this.ftype = type;
            this.target = target;
            this.furopai = furopai;
            this.consumed = new List<Pai>(consumed);
        }
    }


 
}
