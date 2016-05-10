using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace MjModelLibrary
{
    public class Tehai
    {
        private ShantenCalclator shantenCanclator = ShantenCalclator.GetInstance();

        public List<Pai> tehai;
        public List<Furo> furos;
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
            this.tehai = new List<Pai>(tehai);
            this.furos = new List<Furo>();
        }

        public Tehai(List<string> tehai)
        {
            var paiTehai = from hai in tehai select new Pai(hai);
            this.tehai = new List<Pai>(paiTehai);
            this.furos = new List<Furo>();
        }


        public List<string> GetTehaiStringList()
        {
            // sort before display
            tehai = tehai.OrderBy(e => e.PaiNumber).ToList();
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
                Debug.Write("tehai doesn't contains {0}! @Tehai_Da", dapai.PaiString);
                Debug.Assert(false);
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
                Debug.Write("invalied naki! @Tehai_Chi");
                Debug.Assert(false);
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
        public void ChiOnlyMakeFuro(int actor, int target, string pai, List<string> consumed)
        {
            furos.Add(new Furo(MJUtil.TartsuType.MINSYUN, target, new Pai(pai), ConsumedStringToConsumedPai(consumed)));
        }


        public void Pon(int actor, int target, Pai pai, List<Pai> consumed)
        {


            //remove consumed
            foreach (var consumedPai in consumed)
            {
                tehai.Remove(consumedPai);
            }

            //add furo
            furos.Add(new Furo(MJUtil.TartsuType.MINKO, target, pai, consumed));
        }
        public void Pon(int actor, int target, string pai, List<string> consumed)
        {
            Pon(actor, target, new Pai(pai), ConsumedStringToConsumedPai(consumed));
        }
        public void PonOnlyMakeFuro(int actor, int target, string pai, List<string> consumed)
        {
            furos.Add(new Furo(MJUtil.TartsuType.MINKO, target, new Pai(pai), ConsumedStringToConsumedPai(consumed)));
        }

        public void Daiminkan(int actor, int target, Pai pai, List<Pai> consumed)
        {
            if (!IsValidConsumed(consumed))
            {
                Debug.Write("invalied naki! @Tehai_Daiminkan");
                Debug.Assert(false);
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
        public void DaiminkanOnlyMakeFuro(int actor, int target, string pai, List<string> consumed)
        {
            furos.Add(new Furo(MJUtil.TartsuType.MINKANTSU, target, new Pai(pai), ConsumedStringToConsumedPai(consumed)));
        }


        public void Ankan(int actor, List<Pai> consumed)
        {
            if (!IsValidConsumed(consumed))
            {
                Debug.WriteLine("invalied naki! @Tehai_Ankan");
                Debug.Assert(false);
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
        public void Ankan(int actor, List<string> consumed)
        {
            Ankan(actor, ConsumedStringToConsumedPai(consumed));
        }
        public void AnkanOnlyMakeFuro(int actor, List<string> consumed)
        {
            furos.Add(new Furo(MJUtil.TartsuType.ANKANTSU, actor, new Pai(), ConsumedStringToConsumedPai(consumed)));//暗槓は牌がすべてconsumedに入る。対象牌は空文字とする
        }

        public void Kakan(int actor, Pai pai, List<Pai> consumed)
        {
            if (!IsValidKakan(pai, consumed))
            {
                Debug.Write("invalied naki! @Tehai_kakan");
                Debug.Assert(false);
                return;
            }
            //remove pai
            tehai.Remove(pai);

            //change pon to kakan
            foreach (var furo in furos)
            {
                if (furo.ftype == MJUtil.TartsuType.MINKO && furo.furopai.PaiNumber == pai.PaiNumber)
                {
                    furo.ftype = MJUtil.TartsuType.MINKANTSU;
                    furo.consumed.Add(pai);
                    furo.consumed.Sort();
                    break;
                }
            }
        }
        public void Kakan(int actor,  string pai, List<string> consumed)
        {
            Kakan(actor,new Pai(pai), ConsumedStringToConsumedPai(consumed));
        }
        public void KakanOnlyMakeFuro(int actor, string paiString, List<string> consumed)
        {
            var pai = new Pai(paiString);
            //change pon to kakan
            foreach (var furo in furos)
            {
                if (furo.ftype == MJUtil.TartsuType.MINKO && furo.furopai.PaiNumber == pai.PaiNumber)
                {
                    furo.ftype = MJUtil.TartsuType.MINKANTSU;
                    furo.consumed.Add(pai);
                    furo.consumed.Sort();
                    break;
                }
            }

        }




        private bool IsValidKakan(Pai pai, List<Pai> consumed)
        {
            if (!tehai.Contains(pai))
            {
                return false;
            }

            foreach(var furo in furos)
            {
                if (furo.ftype == MJUtil.TartsuType.MINKO && furo.furopai.PaiNumber == pai.PaiNumber)
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

        public bool CanChi(string pai, List <string> consumed)
        {
            foreach(var furopai in consumed)
            {
                if (tehai.Contains(new Pai(furopai)) == false)
                {
                    return false;
                }
            }

            var ids = consumed.Select(e => PaiConverter.STRING_TO_ID[e]);

            // chi consumed pais don't contains jihai.
            if( ids.Any( e => MJUtil.IsJihaiPaiId(e) ))
            {
                return false;
            }

            // chi pais must contains one type ( Characters, Circles, Bamboos )
            var type = ids.First() / 9;
            if (ids.Any(e => (e/9) != type))
            {
                return false;
            }

            return true;
        }

        public bool CanPon(string pai, List<string> consumed)
        {
            var tehaiStrings = tehai.Select(e => e.PaiString).ToList();
            foreach( var consumedOne in consumed)
            {
                if (tehaiStrings.Contains(consumedOne) == false)
                {
                    return false;
                }
                tehaiStrings.Remove(consumedOne);
            }

            return true;
        }

        public bool CanKakan(string pai, List<string> consumed)
        {
            if(tehai.Any(e => e.PaiString == pai) == false)
            {
                return false;
            }

            foreach (var furo in furos)
            {
                if (furo.ftype == MJUtil.TartsuType.MINKO && (furo.consumed[0].PaiNumber == PaiConverter.STRING_TO_ID[consumed[0]]) )
                {
                    return true;
                }
            }
            return false;
        }

        public bool CanDaiminkan(string pai, List<string> consumed)
        {
            return IsValidConsumed(consumed);
        }

        public bool CanAnkan(List<string> consumed)
        {
            return IsValidConsumed(consumed);
        }

        bool IsValidConsumed(List<string> consumed)
        {
            var tehaiStrings = tehai.Select(e => e.PaiString).ToList();
            foreach (var consumedOne in consumed)
            {
                if (tehaiStrings.Contains(consumedOne) == false)
                {
                    return false;
                }
                tehaiStrings.Remove(consumedOne);
            }

            return true;
        }
        bool IsValidConsumed(List<Pai> consumed)
        {
            var tehaiStrings = tehai.Select(e => e.PaiString).ToList();
            foreach (var consumedOne in consumed)
            {
                if (tehaiStrings.Contains(consumedOne.PaiString) == false)
                {
                    return false;
                }
                tehaiStrings.Remove(consumedOne.PaiString);
            }

            return true;
        }

        bool IsSameConsumed(List<Pai> paiConsumed, List<string> stringConsumed)
        {
            if(paiConsumed.Count != stringConsumed.Count)
            {
                return false;
            }
            paiConsumed.Sort();
            stringConsumed.Sort();
            for(int i = 0; i < paiConsumed.Count; i++)
            {
                if( paiConsumed[i].PaiString != stringConsumed[i])
                {
                    return false;
                }
            }
            return true;
        }


        public int GetShanten()
        {
            int[] syu = new int[MJUtil.LENGTH_SYU_ALL];
            foreach(var pai in tehai)
            {
                syu[pai.PaiNumber]++;
            }
            return shantenCanclator.CalcShantenWithFuro(syu,furos.Count);

        }


        public int GetShanten(string targetPai)
        {
            int[] syu = new int[MJUtil.LENGTH_SYU_ALL];
            syu[PaiConverter.STRING_TO_ID[targetPai]]++;
            foreach (var pai in tehai)
            {
                syu[pai.PaiNumber]++;
            }
            return shantenCanclator.CalcShantenWithFuro(syu, furos.Count);
        }

        public int[] GetRealPaiNum()
        {
            int[] syu = new int[MJUtil.LENGTH_SYU_ALL];
            foreach (var pai in tehai)
            {
                syu[pai.PaiNumber]++;
            }
            foreach (var furo in furos)
            {
                syu[furo.furopai.PaiNumber]++;

                foreach(var consumedpai in furo.consumed)
                {
                    syu[consumedpai.PaiNumber]++;
                }
            }

            return syu;
        }

        public bool IsHora()
        {
            return GetShanten() == -1;
        }

        public bool IsTenpai()
        {
            return GetShanten() == 0;
        }

        public bool IsMenzen()
        {
            return furos.Count - furos.Count(e => e.ftype == MJUtil.TartsuType.ANKANTSU) == 0;
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

        public Furo(string typeString, string pai,List<string> consumed)
        {
            if(MJUtil.TARTSU_TYPE_STRING[(int)MJUtil.TartsuType.HEAD] == typeString)
            {
                ftype = MJUtil.TartsuType.HEAD;
            }
            if (MJUtil.TARTSU_TYPE_STRING[(int)MJUtil.TartsuType.ANKO] == typeString)
            {
                ftype = MJUtil.TartsuType.ANKO;
            }
            if (MJUtil.TARTSU_TYPE_STRING[(int)MJUtil.TartsuType.MINKO] == typeString)
            {
                ftype = MJUtil.TartsuType.MINKO;
            }
            if (MJUtil.TARTSU_TYPE_STRING[(int)MJUtil.TartsuType.ANSYUN] == typeString)
            {
                ftype = MJUtil.TartsuType.ANSYUN;
            }
            if (MJUtil.TARTSU_TYPE_STRING[(int)MJUtil.TartsuType.MINSYUN] == typeString)
            {
                ftype = MJUtil.TartsuType.MINSYUN;
            }
            if (MJUtil.TARTSU_TYPE_STRING[(int)MJUtil.TartsuType.ANKANTSU] == typeString)
            {
                ftype = MJUtil.TartsuType.ANKANTSU;
            }
            if (MJUtil.TARTSU_TYPE_STRING[(int)MJUtil.TartsuType.MINKANTSU] == typeString)
            {
                ftype = MJUtil.TartsuType.MINKANTSU;
            }
            this.furopai = new Pai(pai);
            this.consumed = consumed.Select(e => new Pai(e)).ToList();
            this.minPaiSyu = GetMin(this.furopai, this.consumed);
        }

        private int GetMin(Pai furopai, List<Pai> consumed){
            consumed.Sort();
            return Math.Min(furopai.PaiNumber, consumed[0].PaiNumber);
        }

    }

    
 
}
