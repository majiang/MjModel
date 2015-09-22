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

        public void StartGame()
        {
            Init();
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
            tehais[actor].Tsumo(new Pai(pai));
        }

        public void Dahai(int actor, string pai, bool tsumogiri)
        {
            tehais[actor].Da(new Pai(pai));
            kawas[actor].Sutehai(new Pai(pai),false,tsumogiri);
        }

        public void Pon(int p1, int p2, int p3, List<int> list)
        {
            throw new NotImplementedException();
        }

        public void Chi(int p1, int p2, int p3, List<int> list)
        {
            throw new NotImplementedException();
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
    }
}
