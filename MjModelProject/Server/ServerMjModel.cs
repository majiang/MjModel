using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MjModelProject
{
    public class ServerMjModel
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
            turns = new List<int> { 0, 1, 2, 3 };
            currentActor = 0;
        }

        public void StartGame()
        {
            Init();
        }
        public MJsonMessageStartKyoku StartKyoku()
        {

            //ここでturn更新
            //

            //ここでフィールド更新.
            field.bakaze = new Pai("E");
            field.kyoku = 1;
            field.honba = 0;
            field.kyotaku = 0;
            field.oya = 0;
            field.doramarker = yama.doraMarkers;

            var haipais = yama.MakeHaipai();
            tehais = new List<Tehai> { new Tehai(haipais[0]), new Tehai(haipais[1]), new Tehai(haipais[2]), new Tehai(haipais[3]), };
            return new MJsonMessageStartKyoku(
                        field.bakaze.PaiString,
                        field.kyoku,
                        field.honba,
                        field.kyotaku,
                        field.oya,
                        field.doramarker[0].PaiString,
                        new List<List<string>> { 
                            tehais[0].GetTehaiString(),
                            tehais[1].GetTehaiString(),
                            tehais[2].GetTehaiString(),
                            tehais[3].GetTehaiString()
                        }
                    );
        }

        public void GoNextActor()
        {
            currentActor = (currentActor + 1) % 4;
        }
        public void SetCurrentActor(int i)
        {
            currentActor = i;
        }


        public MJsonMessageTsumo Tsumo()
        {
            var tsumoPai = yama.DoTsumo();
            tehais[currentActor].Tsumo(tsumoPai);

            return new MJsonMessageTsumo(
                currentActor,
                tsumoPai.PaiString
                );
        }

        public MJsonMessageTsumo Rinshan()
        {
            var tsumoPai = yama.DoRinshan();
            tehais[currentActor].Tsumo(tsumoPai);

            return new MJsonMessageTsumo(
                currentActor,
                tsumoPai.PaiString
                );
        }

        //ここからクライアントからの命令を受けてモデル内情報を更新する関数群
        public MJsonMessageDahai Dahai(int actor, string pai, bool tsumogiri)
        {
            var dapai = new Pai(pai);
            tehais[actor].Da(dapai);
            kawas[actor].Sutehai(dapai, false, false);
            return new MJsonMessageDahai(actor, pai, tsumogiri);
        }

        public MJsonMessagePon Pon(int actor, int target, string pai, List<string> consumed)
        {
            kawas[target].discards[kawas[target].discards.Count - 1].isFuroTargeted = true;
            tehais[actor].Pon(actor, target, pai, consumed);
            return new MJsonMessagePon(actor, target, pai, consumed);
        }

        public MJsonMessageChi Chi(int actor, int target, string pai, List<string> consumed)
        {
            kawas[target].discards[kawas[target].discards.Count - 1].isFuroTargeted = true;
            tehais[actor].Chi(actor, target, pai, consumed);
            return new MJsonMessageChi(actor, target, pai, consumed);
        }

        public void Kakan(int actor, int target, string pai, List<string> consumed)
        {
            throw new NotImplementedException();
        }

        public void Ankan(int actor, int target, string pai, List<string> consumed)
        {
            throw new NotImplementedException();
        }

        public MJsonMessageDaiminkan Daiminkan(int actor, int target, string pai, List<string> consumed)
        {
            kawas[target].discards[kawas[target].discards.Count - 1].isFuroTargeted = true;
            tehais[actor].Daiminkan(actor, target, pai, consumed);
            return new MJsonMessageDaiminkan(actor, target, pai, consumed);
        }

        public MJsonMessageDora OpenDora()
        {
            var openedPai = yama.OpenDoraOmote();
            return new MJsonMessageDora(openedPai.PaiString);
        }

        public void Reach(int p)
        {
            throw new NotImplementedException();
        }

        public void Hora(int actor, int target, string pai)
        {
// (int actor,int target, string pai, List<string> uradoraMarkers, List<string> horaTehais, Dictionary<string, int> yakus, int fu, int fan, List<int> deltas, List<int> scores)
           // var uradoraMarkers = yama.GetUradoraMarker();
            
           // return new MJsonMessageHora(actor, target, pai, uradoraMarkers, tehais[actor].GetTehaiString(), );
        }

        public void None()
        {
            throw new NotImplementedException();
        }
















        //以下Validater
        public bool CanFinishKyoku()
        {
            return (yama != null) && (yama.GetRestYamaNum() == 0);
        }
    }
}
