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

        public int currentActor;
        public List<InfoForResult> infoForResults { get; set; }

        public List<int> points { get; set; }
        private int myPositionId;
        ShantenCalclator shantenCalclator = ShantenCalclator.GetInstance();

        public ClientMjModel(){}

        public void Init()
        {
            yama = new Yama();
            kawas = new List<Kawa> { new Kawa(), new Kawa(), new Kawa(), new Kawa() };
            tehais = new List<Tehai> { new Tehai(), new Tehai(), new Tehai(), new Tehai() };
            field = new Field();
            infoForResults = new List<InfoForResult>() { new InfoForResult(), new InfoForResult(), new InfoForResult(), new InfoForResult() };
            points = new List<int> { 25000, 25000, 25000, 25000 };
            currentActor = 0;
        }

        public void StartGame(int id)
        {
            
            Init();
            myPositionId = id;

        }

        public void StartKyoku(string bakaze, int kyoku, int honba, int kyotaku, int oya, string doraMarker, List<List<string>> tehais)
        {
            field = new Field(kyoku, honba, kyotaku, oya, bakaze);
            currentActor = oya;
            infoForResults = new List<InfoForResult>() { new InfoForResult(field.KyokuId, 0, oya, bakaze), new InfoForResult(field.KyokuId, 1, oya, bakaze), new InfoForResult(field.KyokuId, 2, oya,bakaze), new InfoForResult(field.KyokuId, 3, oya, bakaze) };

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
            kawas[target].discards.Last().isFuroTargeted = true;

        }

        public void Chi(int actor, int target, string pai, List<string> consumed)
        {
            if (actor == myPositionId)
            {
                tehais[actor].Chi(actor, target, pai, consumed);
            }
            kawas[target].discards.Last().isFuroTargeted = true;
        }

        public void Kakan(int actor, string pai, List<string> consumed)
        {
            //TODO
            
        }

        public void Ankan(int actor, List<string> consumed)
        {
            //TODO
        }

        public void Daiminkan(int actor, int target, string pai, List<string> consumed)
        {
            //TODO 
        }

        public void Reach(int actor)
        {
            //TODO
        }
        public void ReachAccept(int actor, List<int> delta, List<int> scores)
        {
            SetReach(actor);
            this.points = points;
        }

        public void Dora(string newDoraMarker)
        {
            infoForResults.ForEach(e => e.RegisterDoraMarker(newDoraMarker));
        }

        public void Hora(int actor, int target, string pai, List<string> uradora_markers, List<string> hora_tehais, List<List<object>> yakus, int fu, int fan, int hora_points, List<int> deltas, List<int> scores)
        {
            //TODO
        }

        public void None()
        {
        }

        public void Ryuukyoku(string reason, List<List<string>> tehais, List<bool> tenpais, List<int> deltas, List<int> scores)
        {
            this.points = scores;
        }





        public bool CanReach(int playerId)
        {
            return (tehais[playerId].IsTenpai() || tehais[playerId].IsHora() )
                && tehais[playerId].IsMenzen()
                && (infoForResults[playerId].IsReach == false && infoForResults[playerId].IsDoubleReach == false)
                && (yama.GetRestYamaNum() >= Constants.PLAYER_NUM);
        }

        public bool CanTsumoHora()
        {
            //TODO condsider yaku
            return shantenCalclator.CalcShanten(tehais[myPositionId]) == -1;
        }

        public bool CanRonHora(int target, string pai)
        {
            if(target == myPositionId)
            {
                return false;
            }
            //TODO condsider yaku
            return shantenCalclator.CalcShanten(tehais[myPositionId], pai) == -1;
        }


        public void SetReach(int actor)
        {
            if (yama.GetTsumoedYamaNum() > Constants.PLAYER_NUM && infoForResults.Count(e => e.IsFured) == 0)
            {
                infoForResults[actor].IsDoubleReach = true;
            }
            else
            {
                infoForResults[actor].IsReach = true;
            }
            field.AddKyotaku();
        }

    }
}
