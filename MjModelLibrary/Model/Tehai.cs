using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using MjModelLibrary.Result;


namespace MjModelLibrary
{
    public class Tehai
    {
        //private ShantenCalclator shantenCanclator = ShantenCalclator.GetInstance();
        private ArrayShantenCalculator shantenCanclator = ArrayShantenCalculator.GetInstance();

        public List<string> tehaiString;
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
            tehaiString = GetTehaiStringList();
        }

        public Tehai(List<string> tehai)
        {
            var paiTehai = from hai in tehai select new Pai(hai);
            this.tehai = new List<Pai>(paiTehai);
            this.furos = new List<Furo>();
            tehaiString = GetTehaiStringList();
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
            tehaiString.Add(tsumopai.PaiString);
           
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
            tehaiString = GetTehaiStringList();
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

            var consumedIdList = consumed.Select(e => PaiConverter.STRING_TO_ID[e]).ToList();

            // chi consumed pais don't contains jihai.
            var paiId = PaiConverter.STRING_TO_ID[pai];
            if ( MJUtil.IsJihaiPaiId(paiId) || consumedIdList.Any( e => MJUtil.IsJihaiPaiId(e) ) )
            {
                return false;
            }

            // all chi pais must constituted one type ( manzu, pinzu, souzu )
            var type = paiId / MJUtil.LENGTH_SYU_ONE_NUMBERS;
            if (consumedIdList.Any(e => (e / MJUtil.LENGTH_SYU_ONE_NUMBERS) != type))
            {
                return false;
            }


            // TODO
            // kuikae is not allowed
            /*
            consumedIdList.Sort();
            var consumedStartId = consumedIdList[0] % MJUtil.LENGTH_SYU_ONE_NUMBERS;
            var isRyanmen =  ( (consumedIdList[1] - consumedIdList[0]) == 1)
                            && ( 1 <= consumedStartId && consumedStartId <= 7 );

            if (isRyanmen)
            {
                
            }
            */

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
                // ankan don't has furo.furopai
                if (furo.ftype != MJUtil.TartsuType.ANKANTSU)
                {
                    syu[furo.furopai.PaiNumber]++;
                }

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


        // for Unit Test
        public Furo(string typeString, string pai,List<string> consumed)
        {
            if( MJUtil.TARTSU_TYPE_STRING_ENUM_MAP.ContainsKey(typeString) == false)
            {
                Debug.WriteLine("invalid Furo Type string !");
                Debug.Assert(false);
            }
            this.ftype = MJUtil.TARTSU_TYPE_STRING_ENUM_MAP[typeString];
            this.furopai = new Pai(pai);
            this.consumed = consumed.Select(e => new Pai(e)).ToList();
            this.minPaiSyu = GetMin(this.furopai, this.consumed);
        }


        int GetMin(Pai furopai, List<Pai> consumed){
            consumed.Sort();

            if (furopai.PaiString == Pai.UNKNOWN_PAI_STRING)
            {
                return consumed[0].PaiNumber;
            }
            else
            {
                return Math.Min(furopai.PaiNumber, consumed[0].PaiNumber);
            }

        }

        public bool IsKantsu()
        {
            return this.ftype == MJUtil.TartsuType.ANKANTSU
                || this.ftype == MJUtil.TartsuType.MINKANTSU;
        }

    }

    
 
}
