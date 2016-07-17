using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using MjModelLibrary;
using MjModelLibrary.Result;

namespace MjClient
{
    public class ClientMjModel
    {
        public Yama yama { get; set; }
        public List<Kawa> kawas { get; set; }
        public List<Tehai> tehais { get; set; }
        public Field field { get; set; }

        public int CurrentActor;
        public List<InfoForResult> infoForResultList { get; set; }

        public List<int> scores { get; set; }
        private int myPositionId;
        ShantenCalclator shantenCalclator = ShantenCalclator.GetInstance();

        public ClientMjModel() { }

        public void Init()
        {
            yama = new Yama();
            kawas = new List<Kawa> { new Kawa(), new Kawa(), new Kawa(), new Kawa() };
            tehais = new List<Tehai> { new Tehai(), new Tehai(), new Tehai(), new Tehai() };
            field = new Field();
            infoForResultList = new List<InfoForResult>() { new InfoForResult(), new InfoForResult(), new InfoForResult(), new InfoForResult() };
            scores = new List<int> { 25000, 25000, 25000, 25000 };
            CurrentActor = 0;
        }


        public void Replace() { }

        public void StartGame(int id)
        {

            Init();
            myPositionId = id;

        }

        public void StartKyoku(string bakaze, int kyoku, int honba, int kyotaku, int oya, string doraMarker, List<List<string>> tehais)
        {
            field = new Field(kyoku, honba, kyotaku, oya, bakaze);
            CurrentActor = oya;
            infoForResultList = new List<InfoForResult>() { new InfoForResult(field.KyokuId, 0, oya, bakaze), new InfoForResult(field.KyokuId, 1, oya, bakaze), new InfoForResult(field.KyokuId, 2, oya, bakaze), new InfoForResult(field.KyokuId, 3, oya, bakaze) };

            this.tehais = new List<Tehai> { new Tehai(tehais[0]), new Tehai(tehais[1]), new Tehai(tehais[2]), new Tehai(tehais[3]) };

            // Don't use this haipai and this operation is for restYamaNum count
            yama = new Yama();
            yama.MakeHaipai();

            kawas = new List<Kawa> { new Kawa(), new Kawa(), new Kawa(), new Kawa() };
        }

        public void Tsumo(int actor, string pai)
        {
            if (actor == myPositionId)
            {
                tehais[actor].Tsumo(pai);
                infoForResultList[CurrentActor].SetLastAddedPai(pai);
            }
            // Don't use this tsumo and this operation is for restYamaNum count
            yama.DoTsumo();
        }

        public void Dahai(int actor, string pai, bool tsumogiri)
        {
            if (actor == myPositionId)
            {
                tehais[actor].Da(pai);
            }
            kawas[actor].Sutehai(pai);
        }

        public void Pon(int actor, int target, string pai, List<string> consumed)
        {
            if (actor == myPositionId)
            {
                tehais[actor].Pon(actor, target, pai, consumed);
            }
            else
            {
                tehais[actor].PonOnlyMakeFuro(actor, target, pai, consumed);
            }

            kawas[target].discards.Last().isFuroTargeted = true;

        }

        public void Chi(int actor, int target, string pai, List<string> consumed)
        {
            if (actor == myPositionId)
            {
                tehais[actor].Chi(actor, target, pai, consumed);
            }
            else
            {
                tehais[actor].ChiOnlyMakeFuro(actor, target, pai, consumed);
            }

            kawas[target].discards.Last().isFuroTargeted = true;
        }

        public void Kakan(int actor, string pai, List<string> consumed)
        {
            if (actor == myPositionId)
            {
                tehais[actor].Kakan(actor, pai, consumed);
            }
            else
            {
                tehais[actor].kakanOnlyMakeFuro(actor, pai, consumed);
            }

        }

        public void Ankan(int actor, List<string> consumed)
        {
            if (actor == myPositionId)
            {
                tehais[actor].Ankan(actor, consumed);
            }
            else
            {
                tehais[actor].AnkanOnlyMakeFuro(actor, consumed);
            }
        }

        public void Daiminkan(int actor, int target, string pai, List<string> consumed)
        {
            if (actor == myPositionId)
            {
                tehais[actor].Daiminkan(actor, target, pai, consumed);
            }
            else
            {
                tehais[actor].DaiminkanOnlyMakeFuro(actor, target, pai, consumed);
            }

            kawas[target].discards.Last().isFuroTargeted = true;
        }

        public void Reach(int actor)
        {
            // Do anything
        }
        public void ReachAccept(int actor, List<int> delta, List<int> scores)
        {
            SetReachFlag(actor);
            field.AddKyotaku();
            this.scores = this.scores;
        }

        public void Dora(string newDoraMarker)
        {
            infoForResultList.ForEach(e => e.RegisterDoraMarker(newDoraMarker));
        }

        public void Hora(int actor, int target, string pai, List<string> uradora_markers, List<string> hora_tehais, List<List<object>> yakus, int fu, int fan, int hora_points, List<int> deltas, List<int> scores)
        {
            // Do anything
        }

        public void None()
        {
            // Do anything

        }

        public void Ryuukyoku(string reason, List<List<string>> tehais, List<bool> tenpais, List<int> deltas, List<int> scores)
        {
            this.scores = scores;
        }





        public bool CanReach(int playerId)
        {
            return (tehais[playerId].IsTenpai() || tehais[playerId].IsHora())
                && tehais[playerId].IsMenzen()
                && (infoForResultList[playerId].IsReach == false && infoForResultList[playerId].IsDoubleReach == false)
                && (yama.GetRestYamaNum() >= Constants.PLAYER_NUM);
        }


        public HoraResult CalcHora(int target, string pai)
        {
            var ifr = infoForResultList[myPositionId];
            ifr.UseYamaPaiNum = yama.GetTsumoedYamaNum();
            ifr.IsMenzen = tehais[myPositionId].IsMenzen();
            ifr.IsFured = !ifr.IsMenzen;
            ifr.IsTsumo = target == myPositionId;
            ifr.RegisterUraDoraMarker(yama.GetUradoraMarkerStrings());

            if (ifr.IsTsumo)
            {
                ifr.IsHaitei = yama.GetRestYamaNum() == 0;
            }
            else
            {
                infoForResultList[myPositionId].SetLastAddedPai(pai);
                ifr.IsHoutei = yama.GetRestYamaNum() == 0;
            }
            var horaResult = ResultCalclator.CalcHoraResult(tehais[myPositionId], infoForResultList[myPositionId], field, pai);

            return horaResult;
        }


        private void SetReachFlag(int actor)
        {
            if (yama.GetTsumoedYamaNum() <= Constants.PLAYER_NUM && infoForResultList.Count(e => e.IsFured) == 0)
            {
                infoForResultList[actor].IsDoubleReach = true;
            }
            else
            {
                infoForResultList[actor].IsReach = true;
            }
        }

        public void SetScene(int rest_tsumo_num, List<string> dora_markers, List<List<string>> kawas, List<List<bool>> is_reached_kawapai, List<int> scores, int kyoku, int honba, int kyotaku, string bakaze, int oya, List<List<string>> tehais, List<List<List<string>>> furos, int actor, int mypositionid)
        {
            Init();
            yama.SetScene(rest_tsumo_num, dora_markers, tehais, furos, kawas);
            for (int i = 0; i < Constants.PLAYER_NUM; i++)
            {
                this.kawas[i].SetScene(kawas[i], is_reached_kawapai[i]);
            }

            for (int i = 0; i < Constants.PLAYER_NUM; i++)
            {
                this.tehais[i].SetScene(tehais[i], furos[i]);
            }

            field = new Field(kyoku, honba, kyotaku, oya, bakaze);
            this.scores = scores;

            myPositionId = mypositionid;
            CurrentActor = actor;
            infoForResultList = new List<InfoForResult>() { new InfoForResult(field.KyokuId, 0, field.OyaPlayerId), new InfoForResult(field.KyokuId, 1, field.OyaPlayerId), new InfoForResult(field.KyokuId, 2, field.OyaPlayerId), new InfoForResult(field.KyokuId, 3, field.OyaPlayerId) };
            foreach (var doraMarker in dora_markers)
            {
                infoForResultList.ForEach(e => e.RegisterDoraMarker(doraMarker));
            }
            for (int i = 0; i < Constants.PLAYER_NUM; i++)
            {
                infoForResultList[i].SetLastAddedPai(tehais[i].Last());
            }

            // configure reach flag
            // SetReachFlag must be configred after configure yama.   
            var isReach = is_reached_kawapai.Select(e => e.Contains(true)).ToList();
            for (int i = 0; i < Constants.PLAYER_NUM; i++)
            {
                if (isReach[i])
                {
                    SetReachFlag(i);
                }
            }
        }



    }
}
