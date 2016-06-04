using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using MjModelLibrary;
using MjModelLibrary.Result;
using MjNetworkProtocolLibrary;


namespace MjServer
{

    public class GameModel
    {

        public Yama yama;
        public List<Kawa> kawas;
        public List<Tehai> tehais;
        public Field field;
        
        public int CurrentActor { get; private set; }
        public List<InfoForResult> infoForResultList { get; set; }

        public List<int> scores { get; set; }
        MjLogger logger;
        public GameModel(MjLogger logger)
        {
            this.logger = logger;
        }

        private void Init()
        {
            yama = new Yama();
            kawas = new List<Kawa> { new Kawa(), new Kawa(), new Kawa(), new Kawa() };
            tehais = new List<Tehai> { new Tehai(), new Tehai(), new Tehai(), new Tehai() };
            field = new Field();
            scores = new List<int> { 25000, 25000, 25000, 25000 };
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
            infoForResultList = new List<InfoForResult>() { new InfoForResult(field.KyokuId, 0, field.OyaPlayerId), new InfoForResult(field.KyokuId, 1, field.OyaPlayerId), new InfoForResult(field.KyokuId, 2, field.OyaPlayerId), new InfoForResult(field.KyokuId, 3, field.OyaPlayerId) };

            return new MJsonMessageStartKyoku(
                        field.Bakaze.PaiString,
                        field.KyokuId,
                        field.Honba,
                        field.Kyotaku,
                        field.OyaPlayerId,
                        yama.GetDoraMarkerStrings()[0],
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
            return (currentActor + 1) % Constants.PLAYER_NUM;
        }

        public void IncrementActor()
        {
            CurrentActor = CalcNextActor(CurrentActor);
        }
        public void SetCurrentActor(int i)
        {
            CurrentActor = i;
        }


        // model change functions
        public MJsonMessageTsumo Tsumo()
        {
            var tsumoPai = yama.DoTsumo();
            tehais[CurrentActor].Tsumo(tsumoPai);
            infoForResultList[CurrentActor].SetLastAddedPai(tsumoPai);

            return new MJsonMessageTsumo(
                CurrentActor,
                tsumoPai.PaiString
                );
        }

        public MJsonMessageTsumo Rinshan()
        {
            var tsumoPai = yama.DoRinshan();
            tehais[CurrentActor].Tsumo(tsumoPai);
            infoForResultList[CurrentActor].SetLastAddedPai(tsumoPai);

            EnableRinshanFlag(CurrentActor);
            DisableAllOlayersIppatsuFlag();

            return new MJsonMessageTsumo(
                CurrentActor,
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
            DisableMyIppatsuFlag(actor);

            IncrementActor();

            return new MJsonMessageDahai(actor, pai, tsumogiri);
        }

        public MJsonMessagePon Pon(int actor, int target, string pai, List<string> consumed)
        {
            kawas[target].discards[kawas[target].discards.Count - 1].isFuroTargeted = true;
            tehais[actor].Pon(actor, target, pai, consumed);

            DisableAllOlayersIppatsuFlag();

            SetCurrentActor(actor);
        
            return new MJsonMessagePon(actor, target, pai, consumed);
        }

        public MJsonMessageChi Chi(int actor, int target, string pai, List<string> consumed)
        {
            kawas[target].discards[kawas[target].discards.Count - 1].isFuroTargeted = true;
            tehais[actor].Chi(actor, target, pai, consumed);

            DisableAllOlayersIppatsuFlag();

            SetCurrentActor(actor);

            return new MJsonMessageChi(actor, target, pai, consumed);
        }

        public MJsonMessageKakan Kakan(int actor, string pai, List<string> consumed)
        {
            tehais[actor].Kakan( actor, pai, consumed);

            EnableOtherPlayersChankanFlags(actor);
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
            var reachedActor = ((CurrentActor - 1) + Constants.PLAYER_NUM) % Constants.PLAYER_NUM;
            var deltas = new List<int> { 0, 0, 0, 0 };
            deltas[reachedActor] = -Constants.REACH_POINT;
            scores = AddPoints(scores, deltas);
            field.AddKyotaku();

            SetReachFlag(reachedActor);
            EnableIppatsuFlag(reachedActor);            
            return new MJsonMessageReachAccept(reachedActor, deltas, scores);
        }



        public MJsonMessageHora Hora(int actor, int target, string pai)
        {

            field = Field.ChangeOnHora(field, actor);
            scores = calclatedHoraMessage.scores;
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
            
            scores = AddPoints(scores, deltas);

            field = Field.ChangeOnRyukyoku(field, tenpais);

            return new MJsonMessageRyukyoku("fanpai", tehaisString, tenpais, deltas, scores);
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
                    if (tehais[actor].tehai.Count(e => e.PaiString == pai) >= 2)
                    {
                        // karagiri
                        return true;
                    }
                    else
                    {
                        Debug.Fail("invalid Tsumogiri! tsumogiri is false but tumopai and discard pai are same.");
                        return false;
                    }
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
                        Debug.Fail("invalid Tsumogiri! tsumogiri is true but tumopai and discard pai are NOT same.");
                        return false;
                    }
                }
            }
        }

        public bool CanChi(int actor , int  target, string pai, List<string> consumed)
        {
            if ( ( (target != actor) && ((target + 1) % 4 == actor) ) == false)
            {
                Debug.Fail("invalid chi! target or actor or both is invalid.");
                return false;
            }
            if (pai != kawas[target].discards.Last().PaiString)
            {
                Debug.Fail("invalid chi! target didn't discard the pai.");
                return false;
            }

            return tehais[actor].CanChi(pai,consumed);

        }


        public bool CanPon(int actor, int  target, string pai, List< string > consumed)
        {
            if ((target != actor) == false)
            {
                Debug.Fail("invalid pon! target or actor or both is invalid.");
                return false;
            }

            if (pai != kawas[target].discards.Last().PaiString)
            {
                Debug.Fail("invalid pon! target didn't discard the pai.");
                return false;
            }

            return tehais[actor].CanPon(pai, consumed);
        }

        MJsonMessageHora calclatedHoraMessage;
        public bool CanHora(int actor, int target, string pai)
        {
            // OreCelcHoraResult affects calclatedHoraMessage.
            var horaResult = PreCalcHoraResult(actor, target, pai);
            // if hora contains any yaku, return false. 
            if (horaResult.yakuResult.HasYakuExcludeDora == false)
            {
                Debug.Fail("invalid hora! actor tehai don't contain yaku.");
                return false;
            }

            return true;
            
        }
        HoraResult PreCalcHoraResult(int actor, int target, string pai)
        {

            var ifr = infoForResultList[actor];
            ifr.UseYamaPaiNum = yama.GetTsumoedYamaNum();
            ifr.IsMenzen = tehais[actor].IsMenzen();
            ifr.IsFured = !ifr.IsMenzen;
            ifr.IsTsumo = actor == target;

            if (ifr.IsTsumo)
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
            var nextDeltas = CalcDeltaPoint(horaResult.pointResult, actor, target, field.OyaPlayerId, field.Honba, field.Kyotaku);
            var nextScores = nextDeltas.Zip(this.scores, (delta, score) => delta + score).ToList();

            var uradoraMarkers = (ifr.IsReach || ifr.IsDoubleReach) ? yama.GetUradoraMarkerStrings() : new List<string>();

            // save calclated horaresult
            calclatedHoraMessage = new MJsonMessageHora(actor, target, pai, uradoraMarkers, tehais[actor].GetTehaiStringList(), horaResult.yakuResult.yakus, horaResult.yakuResult.Fu,
                horaResult.yakuResult.Han, horaResult.pointResult.HoraPlayerIncome, nextDeltas, nextScores);

            return horaResult;
        }

        readonly static int HONBA_POINT_BASE = 100;
        List<int> CalcDeltaPoint(PointResult pointResult, int horaActor, int horaTarget, int oyaPlayerId, int honba, int kyotaku)
        {
            var deltas = new List<int>() { 0,0,0,0};
            
            

            if (pointResult.IsTsumoHora) {

                if (horaActor == field.OyaPlayerId)
                {
                    for (int i = 0; i < Constants.PLAYER_NUM; i++)
                    {
                        if (i == horaActor)
                        {
                            deltas[i] = pointResult.HoraPlayerIncome
                                      + honba * HONBA_POINT_BASE * (Constants.PLAYER_NUM - 1)
                                      + kyotaku * Constants.REACH_POINT;
                        }
                        else
                        {
                            deltas[i] = -1 * pointResult.ChildOutcome
                                      - honba * HONBA_POINT_BASE;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < Constants.PLAYER_NUM; i++)
                    {
                        if (i == oyaPlayerId)
                        {
                            deltas[i] = -1 * pointResult.OyaOutcome
                                      - honba * HONBA_POINT_BASE;
                        }
                        else if (i == horaActor)
                        {
                            deltas[i] = pointResult.HoraPlayerIncome
                                      + honba * HONBA_POINT_BASE * (Constants.PLAYER_NUM - 1)
                                      + kyotaku * Constants.REACH_POINT;
                        }
                        else
                        {
                            deltas[i] = -1 * pointResult.ChildOutcome
                                      - honba * HONBA_POINT_BASE;
                        }
                    }
                }
            }
            else
            {
                deltas[horaActor] = pointResult.HoraPlayerIncome
                                  + honba * HONBA_POINT_BASE * (Constants.PLAYER_NUM - 1)
                                  + kyotaku * Constants.REACH_POINT;

                deltas[horaTarget] = -1 * pointResult.RonedPlayerOutcome
                                   - honba * HONBA_POINT_BASE * (Constants.PLAYER_NUM - 1);
            }



            return deltas;
        }

        public bool CanReach(int actor)
        {
            var isTenpai = (tehais[actor].IsTenpai() || tehais[actor].IsHora());
            if (isTenpai == false)
            {
                Debug.Fail("invalid reach! tehai is not tenpai.");
            }

            var isMenzen = tehais[actor].IsMenzen();
            if (isMenzen == false)
            {
                Debug.Fail("invalid reach! tehai is not menzen.");
            }

            var isActorNotAlreadyReached = (infoForResultList[actor].IsReach == false && infoForResultList[actor].IsDoubleReach == false);
            if (isActorNotAlreadyReached == false)
            {
                Debug.Fail("invalid reach! actor already reached.");
            }

            var isAllowedTurn = (yama.GetRestYamaNum() >= Constants.PLAYER_NUM);
            if (isAllowedTurn == false)
            {
                Debug.Fail("invalid reach! reach is not allowed in last turn.");
            }

            return isTenpai && isMenzen && isActorNotAlreadyReached && isAllowedTurn;
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

        static readonly int TONPU_KYOKU_NUM = 4;
        static readonly int TONNAN_KYOKU_NUM = 8;


        public bool CanEndGame()
        {
            if (Properties.Settings.Default.KyokuNum == TONPU_KYOKU_NUM)
            {
                return IsTonpuEnd();
            }
            else if(Properties.Settings.Default.KyokuNum == TONNAN_KYOKU_NUM)
            {
                return IsTonnanEnd();
            }
            else
            {
                logger.Log("invalid kyoku number is set in Settings.settings.");
                Debug.Assert(false);
                return true;
            }
        }

        bool IsTonpuEnd()
        {
            return field.KyokuId == 1 && field.Bakaze.PaiString == "S";
        }
        bool IsTonnanEnd()
        {
            return field.KyokuId == 1 && field.Bakaze.PaiString == "W";
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
        

        public void SetReachFlag(int actor)
        {
            if (yama.GetTsumoedYamaNum() <= Constants.PLAYER_NUM  && infoForResultList[actor].IsFuredOnField == false)
            {
                infoForResultList[actor].IsDoubleReach = true;
            }
            else
            {
                infoForResultList[actor].IsReach = true;
            }

        }




        bool IsDifferentPlayer(int actor, int target)
        {
            return actor == target;
        }

        bool IsRightSutehai(int target, string sutehai)
        {
            return sutehai == kawas[target].discards.Last().PaiString;
        }


        void EnableOtherPlayersChankanFlags(int actor)
        {
            infoForResultList.Where(e => infoForResultList.IndexOf(e) != actor)
                             .ToList()
                             .ForEach(e => e.IsChankan = true);
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
        void DisableMyIppatsuRinshanFlags(int actor)
        {
            infoForResultList[actor].IsIppatsu = false;
            infoForResultList[actor].IsRinshan = false;
        }

        void EnableIppatsuFlag(int actor)
        {
            infoForResultList[actor].IsIppatsu = true;
        }
        void DisableMyIppatsuFlag(int actor)
        {
            infoForResultList[actor].IsIppatsu = false;
        }
        void DisableAllOlayersIppatsuFlag()
        {
            infoForResultList.ForEach(e => e.IsIppatsu = false);
        }

    }
}
