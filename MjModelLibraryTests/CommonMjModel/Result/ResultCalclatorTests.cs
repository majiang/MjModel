using Microsoft.VisualStudio.TestTools.UnitTesting;
using MjModelLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json;

namespace MjModelLibrary.Tests
{
    [TestClass()]
    public class ResultCalclatorTests
    {
        class MJsonMessageHora_Extend
        {
            public string type;
            public int kyoku;
            public string bakaze;
            public int actor;
            public int target;
            public string pai;
            public List<string> dora_markers;
            public List<string> uradora_markers;
            public List<string> hora_tehais;
            public List<List<string>> hora_furos;
            public List<List<object>> yakus;
            public int fu;
            public int fan;
            public int hora_points;
            public List<int> deltas;
            public List<int> scores;

            public MJsonMessageHora_Extend(int actor, int kyoku, string bakaze, int target, string pai, List<string> dora_markers, List<string> uradora_markers, List<string> hora_tehais, List<List<string>> hora_furos, List<List<object>> yakus, int fu, int fan, int hora_points, List<int> deltas, List<int> scores)
            {

                type = "hora";
                this.kyoku = kyoku;
                this.actor = actor;
                this.bakaze = bakaze;
                this.target = target;
                this.pai = pai;
                this.dora_markers = dora_markers;
                this.uradora_markers = uradora_markers;
                this.hora_tehais = hora_tehais;
                this.hora_furos = hora_furos;
                this.yakus = yakus;
                this.fu = fu;
                this.fan = fan;
                this.hora_points = hora_points;
                this.deltas = deltas;
                this.scores = scores;
            }

        }

        [TestClass]
        public class PointTest
        {
            [TestMethod]
            public void テストデータ使用テスト()
            {
                using (StreamReader sr = new StreamReader(@"../../HoraPatternOutputSmall.txt", Encoding.GetEncoding("shift_jis")))
                {
                    var lineNumber = 0;


                    while (true)
                    {
                        string line = sr.ReadLine();
                        if (line == null) break;
                        lineNumber++;
                        var obj = JsonConvert.DeserializeObject<MJsonMessageHora_Extend>(line);



                        int expected = obj.hora_points;

                        var gameId = obj.kyoku;
                        var playerPosition = obj.actor;
                        var lastAddedPai = obj.pai;

                        var ifr = new InfoForResult(gameId, playerPosition, obj.bakaze);
                        ifr.IsTsumo = obj.actor == obj.target;

                        var ankantsuCount = obj.hora_furos.Count(e => e[0] == "ankantsu");
                        ifr.IsMenzen = obj.hora_furos.Count - ankantsuCount == 0;
                        ifr.IsFured = !ifr.IsMenzen;
                        ifr.PassedTurn = 4;
                        ifr.SetLastAddedPai(lastAddedPai);

                        ifr.IsIppatsu = yakuContains(obj, "ippatsu");
                        ifr.IsReach = yakuContains(obj, "reach");
                        ifr.IsDoubleReach = yakuContains(obj, "double_reach");
                        ifr.IsHaitei = yakuContains(obj, "haiteiraoyue");
                        ifr.IsHoutei = yakuContains(obj, "hoteiraoyui");
                        ifr.IsRinshan = yakuContains(obj, "rinshankaiho");
                        ifr.IsChankan = yakuContains(obj, "chankan");

                        



                        ifr.IsOya = (obj.kyoku - 1 - obj.actor + 4) % 4 == 0;


                        foreach (var doraMarker in obj.dora_markers)
                        {
                            ifr.RegisterDoraMarker(doraMarker);
                        }

                        ifr.RegisterUraDoraMarker(obj.uradora_markers);


                        var fd = new Field(obj.kyoku, 0, 0, obj.bakaze);
                        var tehai = new Tehai(obj.hora_tehais);

                        foreach (var furo in obj.hora_furos)
                        {
                            var f = new Furo(furo[0], furo[1], furo.GetRange(2, furo.Count - 2));
                            tehai.furos.Add(f);
                        }

                        if (ifr.IsTsumo)
                        {
                            tehai.tehai.Add(new Pai(lastAddedPai));
                        }

                        /*
                        var result = ResultCalclator.CalcHoraResult(tehai, ifr, fd, lastAddedPai);
                        var myPointResult = result.pointResult.HoraPlayerIncome;

                        if (myPointResult != expected)
                        {
                            Debug.WriteLine(lineNumber + ", acc = " + expected + " --> " + myPointResult + " tehais:" + string.Join(",", tehai.GetTehaiStringList().ToList()));

                            foreach (var yaku in result.yakuResult.yakus) {
                                Debug.WriteLine("accual {0}, {1}",yaku[0],yaku[1]);
                            }

                            result = ResultCalclator.CalcHoraResult(tehai, ifr, fd, lastAddedPai);
                        }
                        Assert.AreEqual(expected, myPointResult);
                        */

                    }
                }
            }

            private bool yakuContains(MJsonMessageHora_Extend obj ,string yakuString)
            {
                return obj.yakus.Count(e => e.Contains(yakuString)) > 0;
            }


            [TestMethod]
            public void 点数算出テスト()
            {
                //親40符3翻 ダブ東 ツモ 7800点
                {
                    var expected = 7800;
                    var tehai = new Tehai(new List<string>() { "1m", "2m", "3m", "4p", "5p", "6p", "7p", "7p", "7p", "9p", "9p", "E", "E", "E" });

                    var gameId = 1;
                    var playerPosition = 0;
                    var lastAddedPai = "E";

                    var ifr = new InfoForResult(gameId, playerPosition);
                    ifr.IsTsumo = true;
                    ifr.IsFured = false;
                    ifr.IsMenzen = true;
                    ifr.PassedTurn = 4;
                    ifr.SetLastAddedPai(lastAddedPai);
                    var fd = new Field();

                    var result = ResultCalclator.CalcHoraResult(tehai, ifr, fd, lastAddedPai);
                    Assert.AreEqual(expected, result.pointResult.HoraPlayerIncome);
                }

                //親50符2翻 ツモ 三暗刻
                {
                    var expected = 9600;
                    var tehai = new Tehai(new List<string>() { "1m", "1m", "1m", "2p", "2p", "2p", "4p", "5p", "5p", "5p", "6p", "E", "E", "5p" });

                    var gameId = 1;
                    var playerPosition = 0;
                    var lastAddedPai = "5p";

                    var ifr = new InfoForResult(gameId, playerPosition);
                    ifr.IsTsumo = true;
                    ifr.IsFured = false;
                    ifr.IsMenzen = true;
                    ifr.PassedTurn = 4;
                    ifr.SetLastAddedPai(lastAddedPai);
                    var fd = new Field();

                    var result = ResultCalclator.CalcHoraResult(tehai, ifr, fd, lastAddedPai);
                    Assert.AreEqual(expected, result.pointResult.HoraPlayerIncome);
                }

                //子20符2翻 タンピン
                {
                    var expected = 2000;
                    var tehai = new Tehai(new List<string>() { "2m", "3m", "4m", "2p", "3p", "4p", "4p", "5p", "6p", "4s", "5s", "8s", "8s" });

                    var gameId = 1;
                    var playerPosition = 1;
                    var lastAddedPai = "6s";

                    var ifr = new InfoForResult(gameId, playerPosition);
                    ifr.IsTsumo = false;
                    ifr.IsFured = false;
                    ifr.IsMenzen = true;
                    ifr.PassedTurn = 4;
                    var fd = new Field();
                    ifr.SetLastAddedPai(lastAddedPai);
                    var result = ResultCalclator.CalcHoraResult(tehai, ifr, fd, lastAddedPai);
                    Assert.AreEqual(expected, result.pointResult.HoraPlayerIncome);
                }


                //子20符3翻 タンヤオ三食一盃口
                {
                    var expected = 8000;
                    var tehai = new Tehai(new List<string>() { "2m", "3m", "4m", "2p", "3p", "4p", "2p", "4p", "5s", "5s", "2s", "3s", "4s" });

                    var gameId = 1;
                    var playerPosition = 1;
                    var lastAddedPai = "3p";

                    var ifr = new InfoForResult(gameId, playerPosition);
                    ifr.IsTsumo = false;
                    ifr.IsFured = false;
                    ifr.IsMenzen = true;
                    ifr.PassedTurn = 4;
                    var fd = new Field();
                    ifr.SetLastAddedPai(lastAddedPai);
                    var result = ResultCalclator.CalcHoraResult(tehai, ifr, fd, lastAddedPai);
                    Assert.AreEqual(expected, result.pointResult.HoraPlayerIncome);
                }

                //子40符1翻 自摸
                {
                    var expected = 1500;
                    var tehai = new Tehai(new List<string>() { "1m", "2m", "3m", "3m", "4m", "5m", "1p", "1p", "1p", "2s", "3s", "4s", "5s", "5s" });

                    var gameId = 1;
                    var playerPosition = 1;
                    var lastAddedPai = "3m";

                    var ifr = new InfoForResult(gameId, playerPosition);
                    ifr.IsTsumo = true;
                    ifr.IsFured = false;
                    ifr.IsMenzen = true;
                    ifr.PassedTurn = 4;
                    var fd = new Field();
                    ifr.SetLastAddedPai(lastAddedPai);
                    var result = ResultCalclator.CalcHoraResult(tehai, ifr, fd, lastAddedPai);
                    Assert.AreEqual(expected, result.pointResult.HoraPlayerIncome);
                }

                //子50符2翻 ロン
                {
                    var expected = 3200;
                    var tehai = new Tehai(new List<string>() { "1m", "1m", "1m", "2m", "2m", "2m", "1p", "1p", "1p", "2p", "3p", "4s", "4s" });

                    var gameId = 1;
                    var playerPosition = 1;
                    var lastAddedPai = "1p";

                    var ifr = new InfoForResult(gameId, playerPosition);
                    ifr.IsTsumo = false;
                    ifr.IsFured = false;
                    ifr.IsMenzen = true;
                    ifr.PassedTurn = 4;
                    var fd = new Field();
                    ifr.SetLastAddedPai(lastAddedPai);
                    var result = ResultCalclator.CalcHoraResult(tehai, ifr, fd, lastAddedPai);
                    Assert.AreEqual(expected, result.pointResult.HoraPlayerIncome);
                }


                //Test
                {
                    var expected = 5200;
                    var tehai = new Tehai(new List<string>() { "1p", "2p", "3p", "7p", "8p", "9p", "1s", "2s", "3s", "W", "W", "W", "P", "P" });

                    var lastAddedPai = "3p";
                    var isOya = false;
                    var isMenzen = true;

                    var result = CalcTsumo(tehai, lastAddedPai, isOya, isMenzen);
                    Assert.AreEqual(expected, result.pointResult.HoraPlayerIncome);
                }

            }


            HoraResult CalcRon(Tehai tehai, string ronPai, bool isOya, bool isMenzen)
            {
                Assert.AreEqual(13, tehai.tehai.Count);

                var gameId = 1;
                var playerPosition = isOya ? 0 : 1;
                var lastAddedPai = ronPai;

                var ifr = new InfoForResult(gameId, playerPosition);
                ifr.IsTsumo = false;
                ifr.IsFured = false;
                ifr.IsMenzen = isMenzen;
                ifr.PassedTurn = 4;
                var fd = new Field();
                ifr.SetLastAddedPai(lastAddedPai);
                return ResultCalclator.CalcHoraResult(tehai, ifr, fd, lastAddedPai);
            }

            HoraResult CalcTsumo(Tehai tehai, string tsumoPai, bool isOya, bool isMenzen)
            {
                Assert.AreEqual(14, tehai.tehai.Count);

                var gameId = 1;
                var playerPosition = isOya ? 0 : 1;
                var lastAddedPai = tsumoPai;

                var ifr = new InfoForResult(gameId, playerPosition);
                ifr.IsTsumo = true;
                ifr.IsFured = false;
                ifr.IsMenzen = isMenzen;
                ifr.PassedTurn = 4;
                var fd = new Field();
                ifr.SetLastAddedPai(lastAddedPai);
                return ResultCalclator.CalcHoraResult(tehai, ifr, fd, lastAddedPai);
            }

        }






        [TestClass]
        public class TehaiAnalizerTest
        {
            [TestMethod]
            public void 和了時構成分割テスト()
            {
                Tehai testTehai = new Tehai(new List<string> { "1m", "2m", "3m", "4m", "5m", "6m", "7m", "7m", "1m", "1m", "2m", "2m", "3m" });

                var isRon = true;
                var result = SplitedTehaiCalclator.CalcSplitedTehai(testTehai, "3m", isRon);
                Debug.WriteLine(result.AllHoraPatternList.Count);
                foreach (var r in result.AllHoraPatternList)
                {
                    foreach (var t in r.TartsuList)
                    {
                        Debug.WriteLine(t.TartsuType + "," + t.TartsuStartPaiSyu);
                    }
                    Debug.WriteLine("");
                }

                Assert.AreEqual(result.AllHoraPatternList.Count, 4);//testTehaiは111222333の部分が順子3つのパターンと暗刻3つのパターンがある
            }
        }


        //役判定
        [TestClass]
        public class YakuiAnalizerTest
        {
            [TestMethod]
            public void 役算出テスト()
            {

                //清一色ピンフ一盃口
                {
                    Tehai testTehai = new Tehai(new List<string> { "1m", "2m", "3m", "4m", "5m", "6m", "7m", "7m", "1m", "2m", "2m", "3m", "3m", "4m" });

                    var lastAdded = "4m";
                    var testIfr = new InfoForResult();
                    testIfr.IsMenzen = true;
                    testIfr.IsOya = true;
                    testIfr.IsTsumo = true;
                    testIfr.PassedTurn = 10;
                    testIfr.SetLastAddedPai(lastAdded);
                    var result = new HoraResult();
                    result = ResultCalclator.CalcHoraResult(testTehai, testIfr, new Field(), lastAdded);
                    var yakuMap = result.yakuResult.yakus;
                    Assert.IsTrue(yakuMap.Count(e => (string)e[0] == MJUtil.YAKU_STRING[(int)MJUtil.Yaku.CHINNITSU]) == 1);
                    Assert.IsTrue(yakuMap.Count(e => (string)e[0] == MJUtil.YAKU_STRING[(int)MJUtil.Yaku.PINFU]) == 1);
                    Assert.IsTrue(yakuMap.Count(e => (string)e[0] == MJUtil.YAKU_STRING[(int)MJUtil.Yaku.IIPEIKOU]) == 1);



                }
            }
        }


    }
}