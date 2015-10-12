using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MjModelProject
{
    public class ClientMjModel
    {
        public Yama yama { get; set; }
        public List<Kawa> kawas { get; set; }
        public List<Tehai> tehais { get; set; }
        public Field field { get; set; }
        public List<int> turns;
        public int currentActor;
        private int myPositionId;
        //private Strategy mjai;
        private MjsonMessageAll sendMessage;
        

        public void Init()
        {
            yama = new Yama();
            kawas = new List<Kawa> { new Kawa(), new Kawa(), new Kawa(), new Kawa() };
            tehais = new List<Tehai> { new Tehai(), new Tehai(), new Tehai(), new Tehai() };
            field = new Field();
            var turn = new List<int> { 0, 1, 2, 3 };
            turns = new List<int>(turn.OrderBy(i => Guid.NewGuid()));
            currentActor = 0;
            //初期座席配置作成
        }

        public void StartGame(int id)
        {
            Init();
            myPositionId = id;
        }

        public void StartKyoku(string bakaze, int kyoku, int honba, int kyotaku, int oya, string doraMarker, List<List<string>> tehais)
        {
            field.bakaze = new Pai(bakaze);
            field.kyoku = kyoku;
            field.honba = honba;
            field.kyotaku = kyotaku;
            field.oya = oya;
            field.doramarker.Add(new Pai(doraMarker));

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
            kawas[actor].Sutehai(new Pai(pai),false,tsumogiri);
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

        public void Hora(int p1, int p2, int p3)
        {
            throw new NotImplementedException();
        }

        public void None()
        {
            throw new NotImplementedException();
        }



        public bool CanChi(int actor, int playerId, string pai)
        {
            return ( actor != myPositionId ) && ((actor + 1) % 4 == myPositionId) && tehais[playerId].CanChi(pai);
        }
        public bool CanPon(int actor, int playerId, string pai)
        {
            return ( actor != myPositionId ) && tehais[playerId].CanPon(pai);
        }

        public MJsonMessageChi GetChiMessage(int playerId, int targetId, string pai)
        {
            return tehais[playerId].GetChiMessage(playerId, targetId, pai);
        }
        public MJsonMessagePon GetPonMessage(int playerId, int targetId, string pai)
        {
            return tehais[playerId].GetPonMessage(playerId, targetId, pai);
        }
    }
}
