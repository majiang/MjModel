﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using MjModelProject.Result;

namespace MjModelProject
{
    public class ServerMjModel
    {
       
        public Yama yama { get; set; }
        public List<Kawa> kawas { get; set; }
        public List<Tehai> tehais { get; set; }
        public Field field { get; set; }
        
        public int currentActor;
        public List<InfoForResult> infoForResult { get; set; }

        public List<int> points { get; set; }

        private void Init()
        {
            yama = new Yama();
            kawas = new List<Kawa> { new Kawa(), new Kawa(), new Kawa(), new Kawa() };
            tehais = new List<Tehai> { new Tehai(), new Tehai(), new Tehai(), new Tehai() };
            field = new Field();

            currentActor = 0;
            infoForResult = new List<InfoForResult>() {new InfoForResult(), new InfoForResult(), new InfoForResult(), new InfoForResult() };
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
            SetCurrentActor(0);
            infoForResult = new List<InfoForResult>() { new InfoForResult(), new InfoForResult(), new InfoForResult(), new InfoForResult() };

            return new MJsonMessageStartKyoku(
                        field.Bakaze.PaiString,
                        field.KyokuId,
                        field.Honba,
                        field.Kyotaku,
                        field.OyaPlayerId,
                        yama.doraMarkers[0].PaiString,
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
            GoNextActor();
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

        public MJsonMessageKakan Kakan(int actor, int target, string pai, List<string> consumed)
        {
            tehais[actor].Kakan( actor, target, pai, consumed);
            SetCurrentActor(actor);
            return new MJsonMessageKakan(actor, target, pai, consumed);
        }

        public MJsonMessageAnkan Ankan(int actor, int target, string pai, List<string> consumed)
        {
            tehais[actor].Ankan(actor, consumed);
            SetCurrentActor(actor);
            return new MJsonMessageAnkan(actor, target, pai, consumed);
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

        public MJsonMessageHora Hora(int actor, int target, string pai)
        {
            // validate Hora

            var uradoraMarkers = yama.GetUradoraMarker();
            var horaResult = HoraResultCalclator.CalcHoraResult(tehais[actor],  new InfoForResult());
            //var deltas = 
            

            //場況を更新
            //TODO 


            return new MJsonMessageHora(actor, target, pai, uradoraMarkers, tehais[actor].GetTehaiString(), horaResult.yakuResult.yakus, horaResult.yakuResult.Fu,
                horaResult.yakuResult.Han, new List<int> { 0,0,0,0}, new List<int> { 0, 0, 0, 0 });
        }

        public void None()
        {
            throw new NotImplementedException();
        }

        public MJsonMessageRyukyoku Ryukyoku()
        {
            var tehaisString = new List<List<string>>() {
                    tehais[0].GetTehaiString(),
                    tehais[1].GetTehaiString(),
                    tehais[2].GetTehaiString(),
                    tehais[3].GetTehaiString()
                };
            var tenpais = new List<bool>() { tehais[0].IsTenpai(), tehais[1].IsTenpai(), tehais[2].IsTenpai(), tehais[3].IsTenpai() };
            var deltas = CalcRyukyokuDeltaPoint(tenpais);
            
            //点数を更新
            points = CalcResultPoint(points, deltas);

            //場況を更新
            field = Field.ChangeOnRyukyoku(field, tenpais);

            return new MJsonMessageRyukyoku("fanpai", tehaisString, tenpais, deltas, points);
        }





        //以下Validater
        public bool CanFinishKyoku()
        {
            return (yama != null) && (yama.GetRestYamaNum() == 0);
        }


        private Field CreateNextKyokuField(int kyokuID, int honba, int kyotaku)
        {
            return new Field(kyokuID, honba, kyotaku);
        }

        public bool CanEndGame()
        {
            return field.KyokuId == Constants.KYOKU_NUM;
        }


        private static int DELTA_POINT_BASE = 3000;
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

        private List<int> CalcResultPoint(List<int> points, List<int> deltas)
        {
            return points.Zip(deltas, (p, d) => p + d).ToList();
        }
        

      

    }
}
