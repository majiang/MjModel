using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using MjModelLibrary;

namespace MjClient
{
    public class ClientMjModel
    {
        public Yama yama { get; set; }
        public List<Kawa> kawas { get; set; }
        public List<Tehai> tehais { get; set; }
        public Field field { get; set; }

        public int currentActor;
        public List<InfoForResult> infoForResult { get; set; }

        public List<int> points { get; set; }
        private int myPositionId;
        ShantenCalclator shantenCalclator = ShantenCalclator.GetInstance();

        public ClientMjModel() : base()
        {

        }

        public void Init()
        {
            yama = new Yama();
            kawas = new List<Kawa> { new Kawa(), new Kawa(), new Kawa(), new Kawa() };
            tehais = new List<Tehai> { new Tehai(), new Tehai(), new Tehai(), new Tehai() };
            field = new Field();
            infoForResult = new List<InfoForResult>() { new InfoForResult(), new InfoForResult(), new InfoForResult(), new InfoForResult() };
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
            field = new Field(kyoku, honba, kyotaku);
            currentActor = 0;
            infoForResult = new List<InfoForResult>() { new InfoForResult(field.KyokuId, 0), new InfoForResult(field.KyokuId, 1), new InfoForResult(field.KyokuId, 2), new InfoForResult(field.KyokuId, 3) };

            this.tehais = new List<Tehai> { new Tehai(tehais[0]), new Tehai(tehais[1]), new Tehai(tehais[2]), new Tehai(tehais[3]) };
        }

        public void Tsumo(int actor, string pai)
        {
            tehais[actor].Tsumo(pai);
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

        public void Kakan(int p1, int p2, int p3, List<int> list)
        {
            throw new NotImplementedException();
        }

        public void Ankan(int p1, int p2, int p3, List<int> list)
        {
            throw new NotImplementedException();
        }

        public void Daiminkan(int p1, int p2, int p3, List<int> list)
        {
            throw new NotImplementedException();
        }

        public void Reach(int p)
        {
            throw new NotImplementedException();
        }
        public void ReachAccept(int actor, List<int> points)
        {
            SetReach(actor);
            this.points = points;
        }


        public void Hora(int p1, int p2, int p3)
        {
            throw new NotImplementedException();
        }

        public void None()
        {
            throw new NotImplementedException();
        }



        public bool CanChi(int dapaiActor, int playerId, string pai)
        {
            if ( (dapaiActor != myPositionId) && ( (dapaiActor + 1) % 4 == myPositionId) )
            {
                return tehais[myPositionId].CanChi(dapaiActor, playerId, pai);
            }
            return false;
            
        }
        public bool CanPon(int dapaiActor, int playerId, string pai)
        {
            if (dapaiActor != myPositionId)
            {
                return tehais[myPositionId].CanPon(dapaiActor, playerId, pai);
            }
            return false;
        }

        public bool CanReach(int playerId)
        {
            return ( shantenCalclator.CalcShanten(tehais[playerId]) <= 0) 
                && ( (infoForResult[playerId].IsDoubleReach || infoForResult[playerId].IsReach) == false );
        }

        public bool CanTsumoHora(string pai)
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




        //public MJsonMessageChi GetChiMessage()
        //{
        //    return tehais[myPositionId].GetChiMessage();
        //}
        //public MJsonMessagePon GetPonMessage()
        //{
        //    return tehais[myPositionId].GetPonMessage();
        //}

        public void SetReach(int actor)
        {
            if (yama.GetUsedYamaNum() > 4 && infoForResult.Count(e => e.IsFured) == 0)
            {
                infoForResult[actor].IsDoubleReach = true;
            }
            else
            {
                infoForResult[actor].IsReach = true;
            }
            field.AddKyotaku();
        }

    }
}
