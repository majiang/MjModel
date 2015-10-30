using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MjModelProject.Util;

namespace MjModelProject
{
    public class Tehai
    {
        public List<Pai> tehai { get; set; }
        public List<Furo> furos { get; set; }
        public NakiAnalizer nakiAnalizer = new NakiAnalizer();
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
            return tehai.Select(e => e.PaiString).ToList();
        }

        public void Tsumo(Pai tsumopai)
        {
            tehai.Add(tsumopai);
        }
        public void Tsumo(string tsumopai)
        {
            Tsumo(new Pai(tsumopai));
        }

        public void Da(Pai dapai)
        {
            if (tehai.Any( e => e.PaiString == dapai.PaiString))
            {
                tehai.RemoveAt(tehai.FindIndex(e => e.PaiString == dapai.PaiString));
            }
            else
            {
                Console.Write("tehai doesn't contains {0}! @Tehai_Da", dapai);
                return;
            }
        }
        public void Da(string dapai)
        {
            Da(new Pai(dapai));
        }

        public void Chi(int actor, int target, Pai pai, List<Pai> consumed)
        {
            if ( !IsValidConsumed(consumed) )
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
            furos.Add(new Furo(MJUtil.TartsuType.MINSYUN, target, pai, consumed));

        }
        public void Chi(int actor, int target, string pai, List<string> consumed)
        {
            Chi(actor, target, new Pai(pai), ConsumedStringToConsumedPai(consumed));
        }



        public void Pon(int actor, int target, Pai pai, List<Pai> consumed)
        {
            if (!IsValidConsumed(consumed))
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
            furos.Add(new Furo(MJUtil.TartsuType.Minko, target, pai, consumed));
        }
        public void Pon(int actor, int target, string pai, List<string> consumed)
        {
            Pon(actor, target, new Pai(pai), ConsumedStringToConsumedPai(consumed));
        }


        public void Daiminkan(int actor, int target, Pai pai, List<Pai> consumed)
        {
            if (!IsValidConsumed(consumed))
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
            furos.Add(new Furo(MJUtil.TartsuType.MINKANTSU, target, pai, consumed));
        }
        public void Daiminkan(int actor, int target, string pai, List<string> consumed)
        {
            Daiminkan(actor, target, new Pai(pai), ConsumedStringToConsumedPai(consumed));
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
            furos.Add(new Furo(MJUtil.TartsuType.ANKANTSU, actor, new Pai(), consumed));//暗槓は牌がすべてconsumedに入る。対象牌は空文字とする
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
                if (furo.ftype == MJUtil.TartsuType.Minko && furo.consumed.SequenceEqual(consumed))
                {
                    furo.ftype = MJUtil.TartsuType.MINKANTSU;
                    furo.consumed.Add(pai);
                    furo.consumed.Sort();
                    break;
                }
            }
        }


        private bool IsValidConsumed(List<Pai> consumed)
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
                if (furo.ftype == MJUtil.TartsuType.Minko && furo.consumed.SequenceEqual(consumed))
                {
                   return true;
                }
            }

            return false;
        }



        private List<Pai> ConsumedStringToConsumedPai(List<string> consumed)
        {
            var consumedPai = new List<Pai>();
            foreach (var e in consumed)
            {
                consumedPai.Add(new Pai(e));
            }
            return consumedPai;
        }

        public bool CanChi(int actor, int playerId, string pai)
        {
            return nakiAnalizer.CanChi(actor, playerId, tehai, pai);
        }

        public bool CanPon(int actor, int playerId, string pai)
        {
            return nakiAnalizer.CanPon(actor, playerId, tehai, pai);
        }

        public MJsonMessageChi GetChiMessage()
        {
            return nakiAnalizer.GetChiMessage();
        }
        public MJsonMessagePon GetPonMessage()
        {
            return nakiAnalizer.GetPonMessage();
        }


    }

    public class Furo
    {


        public MJUtil.TartsuType ftype;
        public int target;
        public List<Pai> consumed;
        public Pai furopai;
        public int minPaiSyu = Int32.MaxValue;

        public Furo(MJUtil.TartsuType type, int target, Pai furopai, List<Pai> consumed)
        {
            this.ftype = type;
            this.target = target;
            this.furopai = furopai;
            this.consumed = new List<Pai>(consumed);
            this.minPaiSyu = GetMin(furopai, consumed);

        }

        private int GetMin(Pai furopai, List<Pai> consumed){
            consumed.Sort();
            return Math.Min(furopai.PaiNumber, consumed[0].PaiNumber);
        }

    }

    
 
}
