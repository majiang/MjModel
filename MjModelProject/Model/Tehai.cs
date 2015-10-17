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








        internal bool CanChi(string pai)
        {
            var paiId = PaiConverter.STRING_TO_ID[pai];
            if (paiId > 27)
            {
                return false;
            }
            else if (paiId % 9 == 0)
            {
                return tehai.Any(e => e.PaiNumber == paiId + 1) &&
                       tehai.Any(e => e.PaiNumber == paiId + 2);
            }
            else if (paiId % 9 == 1)
            {
                return tehai.Any(e => e.PaiNumber == paiId + 1) &&
                       tehai.Any(e => e.PaiNumber == paiId + 2) ||
                       
                       tehai.Any(e => e.PaiNumber == paiId - 1) && 
                       tehai.Any(e => e.PaiNumber == paiId + 1);

            }
            else if (paiId % 9 == 7)
            {
                return tehai.Any(e => e.PaiNumber == paiId - 2) &&
                       tehai.Any(e => e.PaiNumber == paiId - 1) ||

                       tehai.Any(e => e.PaiNumber == paiId - 1) &&
                       tehai.Any(e => e.PaiNumber == paiId + 1);

            }
            else if (paiId % 9 == 8)
            {
                return tehai.Any(e => e.PaiNumber == paiId - 2) &&
                       tehai.Any(e => e.PaiNumber == paiId - 1);
            }
            else 
            {
                return tehai.Any(e => e.PaiNumber == paiId - 2) &&
                       tehai.Any(e => e.PaiNumber == paiId - 1) ||

                       tehai.Any(e => e.PaiNumber == paiId - 1) &&
                       tehai.Any(e => e.PaiNumber == paiId + 1) ||

                       tehai.Any(e => e.PaiNumber == paiId + 1) &&
                       tehai.Any(e => e.PaiNumber == paiId + 2);

            }
        }

        internal bool CanPon(string pai)
        {
            var paiId = PaiConverter.STRING_TO_ID[pai];
            return tehai.Where(e => e.PaiNumber == paiId).Count() >= 2;
        }


        internal MJsonMessageChi GetChiMessage(int playerId, int targetId, string pai)
        {
            var paiId = PaiConverter.STRING_TO_ID[pai];
            if (paiId > 27)
            {
                throw new InvalidNakiException("target pai shoud be number pai");
            }
            
            
            if (paiId % 9 == 0)
            {
                return new MJsonMessageChi(playerId, targetId, pai, new List<string> { PaiConverter.ID_TO_STRING[paiId + 1],  PaiConverter.ID_TO_STRING[paiId + 2] });
            }
            else if (paiId % 9 == 1)
            {
                if( tehai.Any(e => e.PaiNumber == paiId - 1) &&
                    tehai.Any(e => e.PaiNumber == paiId + 1))
                {
                    return new MJsonMessageChi(playerId, targetId, pai, new List<string> { PaiConverter.ID_TO_STRING[paiId - 1], PaiConverter.ID_TO_STRING[paiId + 1] });
                }
                else 
                {
                    return new MJsonMessageChi(playerId, targetId, pai, new List<string> { PaiConverter.ID_TO_STRING[paiId + 1],  PaiConverter.ID_TO_STRING[paiId + 2] });
                }
                
            }
            else if (paiId % 9 == 7)
            {
                if (tehai.Any(e => e.PaiNumber == paiId - 2) &&
                    tehai.Any(e => e.PaiNumber == paiId - 1))
                {
                    return new MJsonMessageChi(playerId, targetId, pai, new List<string> { PaiConverter.ID_TO_STRING[paiId - 2], PaiConverter.ID_TO_STRING[paiId - 1] });
                }
                else 
                {
                    return new MJsonMessageChi(playerId, targetId, pai, new List<string> { PaiConverter.ID_TO_STRING[paiId - 1], PaiConverter.ID_TO_STRING[paiId + 1] });
                }
            }
            else if (paiId % 9 == 8)
            {
                return new MJsonMessageChi(playerId, targetId, pai, new List<string> { PaiConverter.ID_TO_STRING[paiId - 2], PaiConverter.ID_TO_STRING[paiId - 1] });
            }
            else
            {
                if (tehai.Any(e => e.PaiNumber == paiId - 2) &&
                    tehai.Any(e => e.PaiNumber == paiId - 1))
                {
                    return new MJsonMessageChi(playerId, targetId, pai, new List<string> { PaiConverter.ID_TO_STRING[paiId - 2], PaiConverter.ID_TO_STRING[paiId - 1] });
                }
                else if (tehai.Any(e => e.PaiNumber == paiId - 1) &&
                         tehai.Any(e => e.PaiNumber == paiId + 1))
                {
                    return new MJsonMessageChi(playerId, targetId, pai, new List<string> { PaiConverter.ID_TO_STRING[paiId - 1], PaiConverter.ID_TO_STRING[paiId + 1] });
                }
                else
                    //以下の条件が成り立っている
                    //if (tehai.Any(e => e.paiNumber == paiId + 1) &&
                    // tehai.Any(e => e.paiNumber == paiId + 2))
                {
                    return new MJsonMessageChi(playerId, targetId, pai, new List<string> { PaiConverter.ID_TO_STRING[paiId + 1], PaiConverter.ID_TO_STRING[paiId + 2] });
                }
            }
        }
        internal MJsonMessagePon GetPonMessage(int playerId, int targetId, string pai)
        {
            var paiId = PaiConverter.STRING_TO_ID[pai];
            var consumedCandidates = tehai.Where(e => e.PaiNumber == paiId).ToList();
            return new MJsonMessagePon(playerId, targetId, pai, new List<string> { consumedCandidates[0].PaiString, consumedCandidates[1].PaiString });
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
