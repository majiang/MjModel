using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using MjModelLibrary;
using MjNetworkProtocolLibrary;

namespace MjServer
{



    public class GameModel
    {
        
        public Yama yama { get; set; }
        public List<Kawa> kawas { get; set; }
        public List<Tehai> tehais { get; set; }
        public Field field { get; set; }
        
        public int currentActor;
        public List<InfoForResult> infoForResultList { get; set; }

        public List<int> points { get; set; }

        public GameModel(){}

        private void Init()
        {
            yama = new Yama();
            kawas = new List<Kawa> { new Kawa(), new Kawa(), new Kawa(), new Kawa() };
            tehais = new List<Tehai> { new Tehai(), new Tehai(), new Tehai(), new Tehai() };
            field = new Field();
            currentActor = 0;
            infoForResultList = new List<InfoForResult>() {new InfoForResult(field.KyokuId,0), new InfoForResult(field.KyokuId,1), new InfoForResult(field.KyokuId,2), new InfoForResult(field.KyokuId,3) };
            points = new List<int> { 25000, 25000, 25000, 25000 };
        }



        public void StartGame()
        {
            Init();
        }

        public MJsonMessageStartKyoku StartKyoku()
        {
            yama.Init();
            kawas.ForEach(e => e.Init());
            var haipais = yama.MakeHaipai();
            tehais = new List<Tehai> { new Tehai(haipais[0]), new Tehai(haipais[1]), new Tehai(haipais[2]), new Tehai(haipais[3]), };
            SetCurrentActor(field.OyaPlayerId);
            infoForResultList = new List<InfoForResult>() { new InfoForResult(field.KyokuId, 0), new InfoForResult(field.KyokuId, 1), new InfoForResult(field.KyokuId, 2), new InfoForResult(field.KyokuId, 3) };

            return new MJsonMessageStartKyoku(
                        field.Bakaze.PaiString,
                        field.KyokuId,
                        field.Honba,
                        field.Kyotaku,
                        field.OyaPlayerId,
                        yama.GetDoraMarkers()[0].PaiString,
                        new List<List<string>> { 
                            tehais[0].GetTehaiStringList(),
                            tehais[1].GetTehaiStringList(),
                            tehais[2].GetTehaiStringList(),
                            tehais[3].GetTehaiStringList()
                        }
                    );
        }

        


        public int CalcNextActor(int currentActor)
        {
            return (currentActor + 1) % 4;
        }

        public void IncrementActor()
        {
            currentActor = CalcNextActor(currentActor);
        }
        public void SetCurrentActor(int i)
        {
            currentActor = i;
        }


        // model change functions
        public MJsonMessageTsumo Tsumo()
        {
            var tsumoPai = yama.DoTsumo();
            tehais[currentActor].Tsumo(tsumoPai);
            infoForResultList[currentActor].SetLastAddedPai(tsumoPai);

            return new MJsonMessageTsumo(
                currentActor,
                tsumoPai.PaiString
                );
        }

        public MJsonMessageTsumo Rinshan()
        {
            var tsumoPai = yama.DoRinshan();
            tehais[currentActor].Tsumo(tsumoPai);
            infoForResultList[currentActor].SetLastAddedPai(tsumoPai);

            EnableRinshanFlag(currentActor);

            return new MJsonMessageTsumo(
                currentActor,
                tsumoPai.PaiString
                );
        }

        public MJsonMessageDahai Dahai(int actor, string pai, bool tsumogiri)
        {
            var dapai = new Pai(pai);
            tehais[actor].Da(dapai);
            kawas[actor].Sutehai(dapai);


            DisableMyIppatsuRinshanFlags(actor);
            DisableOtherPlayersChankanFlags(actor);

            IncrementActor();

            return new MJsonMessageDahai(actor, pai, tsumogiri);
        }

        public MJsonMessagePon Pon(int actor, int target, string pai, List<string> consumed)
        {
            kawas[target].discards[kawas[target].discards.Count - 1].isFuroTargeted = true;
            tehais[actor].Pon(actor, target, pai, consumed);
            SetCurrentActor(actor);
            return new MJsonMessagePon(actor, target, pai, consumed);
        }

        public MJsonMessageChi Chi(int actor, int target, string pai, List<string> consumed)
        {
            kawas[target].discards[kawas[target].discards.Count - 1].isFuroTargeted = true;
            tehais[actor].Chi(actor, target, pai, consumed);
            SetCurrentActor(actor);
            return new MJsonMessageChi(actor, target, pai, consumed);
        }

        public MJsonMessageKakan Kakan(int actor, string pai, List<string> consumed)
        {
            tehais[actor].Kakan( actor, pai, consumed);
            SetCurrentActor(actor);
            return new MJsonMessageKakan(actor, pai, consumed);
        }

        public MJsonMessageAnkan Ankan(int actor, List<string> consumed)
        {
            tehais[actor].Ankan(actor, consumed);
            SetCurrentActor(actor);
            return new MJsonMessageAnkan(actor, consumed);
        }

        public MJsonMessageDaiminkan Daiminkan(int actor, int target, string pai, List<string> consumed)
        {
            kawas[target].discards[kawas[target].discards.Count - 1].isFuroTargeted = true;
            tehais[actor].Daiminkan(actor, target, pai, consumed);
            SetCurrentActor(actor);
            return new MJsonMessageDaiminkan(actor, target, pai, consumed);
        }

        public MJsonMessageDora OpenDora()
        {
            var openedPai = yama.OpenDoraOmote();
            return new MJsonMessageDora(openedPai.PaiString);
        }

        public MJsonMessageReach Reach(int actor)
        {
            return new MJsonMessageReach(actor);
        }

        public MJsonMessageReachAccept ReachAccept()
        {
            var reachedActor = ((currentActor - 1) + 4) % 4;
            var deltas = new List<int> { 0, 0, 0, 0 };
            deltas[reachedActor] = -Constants.REACH_POINT;
            points = AddPoints(points, deltas);

            SetReach(reachedActor);

            return new MJsonMessageReachAccept(reachedActor, deltas, points);
        }


        
        
        public MJsonMessageHora Hora(int actor, int target, string pai)
        {

            // change field
            field = Field.ChangeOnHora(field, actor);
            
            return calclatedHoraMessage;
        }

        public MJsonMessageRyukyoku Ryukyoku()
        {
            var tehaisString = new List<List<string>>() {
                    tehais[0].GetTehaiStringList(),
                    tehais[1].GetTehaiStringList(),
                    tehais[2].GetTehaiStringList(),
                    tehais[3].GetTehaiStringList()
                };
            var tenpais = new List<bool>() { tehais[0].IsTenpai(), tehais[1].IsTenpai(), tehais[2].IsTenpai(), tehais[3].IsTenpai() };
            var deltas = CalcRyukyokuDeltaPoint(tenpais);
            
            points = AddPoints(points, deltas);

            field = Field.ChangeOnRyukyoku(field, tenpais);

            return new MJsonMessageRyukyoku("fanpai", tehaisString, tenpais, deltas, points);
        }


        public  MJsonMessageEndkyoku EndKyoku()
        {
            return new MJsonMessageEndkyoku();
        }


        public MJsonMessageEndgame EndGame()
        {
            return new MJsonMessageEndgame();
        }


        // action validate functions
        public bool CanTsumo()
        {
            return true;
        }
        public bool CanDahai(int actor,string pai, bool tsumogiri)
        {
            return isValidTsumogiri(actor, pai, tsumogiri)
                && tehais[actor].tehai.Any(e => e.PaiString == pai);
        }

        bool isValidTsumogiri(int actor, string pai, bool tsumogiri)
        {
            if ( tsumogiri == false )
            {
                if ((pai == infoForResultList[actor].LastAddedPai.PaiString))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                if ((pai == infoForResultList[actor].LastAddedPai.PaiString))
                {
                    return true;
                }
                else
                {
                    if(tehais[actor].tehai.Count(e => e.PaiString == pai) >= 2)
                    {
                        // karagiri 
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public bool CanChi(int actor , int  target, string pai, List<string> consumed)
        {
            if ( ( (target != actor) && ((target + 1) % 4 == actor) ) == false)
            {
                return false;
            }
            if (pai != kawas[target].discards.Last().PaiString)
            {
                return false;
            }

            return tehais[actor].CanChi(pai,consumed);

        }
        public bool CanPon(int actor, int  target, string pai, List< string > consumed)
        {
            if ((target != actor) == false)
            {
                return false;
            }

            if (pai != kawas[target].discards.Last().PaiString)
            {
                return false;
            }

            return tehais[actor].CanPon(pai, consumed);
        }

        MJsonMessageHora calclatedHoraMessage;
        public bool CanHora(int actor, int target, string pai)
        {
            var horaResult = PreCalcHoraResult(actor, target, pai);
            // if hora contains any yaku, return false. 
            if (horaResult.yakuResult.HasYakuExcludeDora == false)
            {
                return false;
            }




            return true;
            
        }
        HoraResult PreCalcHoraResult(int actor, int target, string pai)
        {
            var uradoraMarkers = yama.GetUradoraMarker();
            var ifr = infoForResultList[actor];
            ifr.UseYamaPaiNum =
                yama.GetTsumoedYamaNum();
            ifr.IsMenzen = tehais[actor].IsMenzen();
            ifr.IsFured = !ifr.IsMenzen;

            if (actor == target)
            {
                ifr.IsHaitei = yama.GetRestYamaNum() == 0;
                ifr.IsTsumo = true;
            }
            else
            {
                infoForResultList[actor].SetLastAddedPai(pai);
                ifr.IsHoutei = yama.GetRestYamaNum() == 0;
                ifr.IsTsumo = false;
            }


            var horaResult =  ResultCalclator.CalcHoraResult(tehais[actor], infoForResultList[actor], field, pai);
            //TODO  modifie delta and pointResult.

            // save calclated horaresult
            calclatedHoraMessage = new MJsonMessageHora(actor, target, pai, uradoraMarkers, tehais[actor].GetTehaiStringList(), horaResult.yakuResult.yakus, horaResult.yakuResult.Fu,
                horaResult.yakuResult.Han, horaResult.pointResult.HoraPlayerIncome, new List<int> { 0, 0, 0, 0 }, new List<int> { 0, 0, 0, 0 });

            return horaResult;
        }


        public bool CanReach(int actor)
        {
            return infoForResultList[actor].IsReach == false
                && infoForResultList[actor].IsDoubleReach == false;
        }

        public bool CanOpenDora()
        {
            // This function always return true 
            // because already validated in Can[*]kan.
            return true;
        }

        public bool CanRinshan()
        { 
            // This function always return true 
            // because already validated in Can[*]kan.
            return yama.CanKan();
        }

        public bool CanKakan(int actor, string pai, List<string> consumed)
        {
            return tehais[actor].CanKakan(pai, consumed) && yama.CanKan();
        }

        public bool CanReachDahai(int actor, string pai, bool tsumogiri)
        {
            return CanDahai(actor, pai, tsumogiri);
        }

        public bool CanDaiminkan(int actor, int target, string pai, List<string> consumed)
        {
            if(IsDifferentPlayer(actor, target) && IsRightSutehai(target, pai) == false)
            {
                return false;
            }
            
            
            return tehais[actor].CanDaiminkan(pai, consumed) && yama.CanKan();
        }



        public bool CanReachAccept()
        {
            // This function always return true 
            // because already validated in CanReach.
            return true;
        }



        public bool CanAnkan(int actor, List<string> consumed)
        {
            return tehais[actor].CanAnkan(consumed) && yama.CanKan();
        }
        public bool CanRyukyoku()
        {
            return ((yama != null) && (yama.GetRestYamaNum() == 0))
                || IsSukaiKanRyukyoku();
        }
        
        bool IsSukaiKanRyukyoku()
        {
            if (tehais.Sum( tehai => tehai.furos.Count( furo => furo.IsKantsu() ) ) < 4)
            {
                return false;
            }
            else
            {
                if ( tehais.Any( e => e.furos.Count(furo => furo.IsKantsu()) == 4) )
                {
                    // if one player has 4 kantsu, it is not ryukyoku 
                    // because of Sukantsu
                    return false;
                }
                else
                {
                    return true;
                }
            }
            
        }


        public bool CanEndGame()
        {
            return field.KyokuId == 1 && field.Bakaze.PaiString == "S";
        }


        // model change functions 
        private readonly int DELTA_POINT_BASE = 3000;
        private List<int> CalcRyukyokuDeltaPoint(List<bool> tenpais)
        {
            var tenpaiNum = tenpais.Count(e => e == true);

            if (tenpaiNum == 0 || tenpaiNum == Constants.PLAYER_NUM)
            {
                return new List<int>() { 0, 0, 0, 0 };
            }
            else
            {
                return tenpais.Select(e => e ? DELTA_POINT_BASE / tenpaiNum : -DELTA_POINT_BASE / (Constants.PLAYER_NUM - tenpaiNum)).ToList();
            }
        }

        private List<int> AddPoints(List<int> points, List<int> deltas)
        {
            var sums = new List<int>();
            foreach( var p in points.Select( (val,index) => new {val, index }))
            {
                sums.Add( points[p.index] + deltas[p.index]);
            }

            return sums;
        }
        

        public void SetReach(int actor)
        {
            if (yama.GetTsumoedYamaNum() < 4 && infoForResultList.Count(e => e.IsFuredOnField) == 0)
            {
                infoForResultList[actor].IsDoubleReach = true;
            }
            else
            {
                infoForResultList[actor].IsReach = true;
            }
            field.AddKyotaku();
        }



        bool IsDifferentPlayer(int actor, int target)
        {
            return actor == target;
        }

        bool IsRightSutehai(int target, string sutehai)
        {
            return sutehai == kawas[target].discards.Last().PaiString;
        }

        void DisableMyIppatsuRinshanFlags(int actor)
        {
            infoForResultList[actor].IsIppatsu = false;
            infoForResultList[actor].IsRinshan = false;
        }

        void DisableOtherPlayersChankanFlags(int actor)
        {
            infoForResultList.Where(e => infoForResultList.IndexOf(e) != actor)
                             .ToList()
                             .ForEach(e => e.IsChankan = false);
        }
        void EnableRinshanFlag(int actor)
        {
            infoForResultList[actor].IsRinshan = true;
        }


    }
}
